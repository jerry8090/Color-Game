using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Save;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.History;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// Controls one complete round. This and PayoutCalculator are the ONLY scripts that decide outcomes.
    /// The player can bet on any number of colors (BetSlip). The TOTAL is spent once when the round starts
    /// (one BET transaction). Winnings are added once. When the round is over (paid, voided or no wager)
    /// the slip and the chip selection are cleared, after the machine is back on the shelf.
    /// See the comments in earlier versions for PHYSICAL / BETTING CYCLE / LEGACY modes: unchanged.
    /// </summary>
 
    /// <summary>DEV TEST ONLY (Editor and development builds): what the Force Result button asks for.</summary>
    public enum ForceResultMode
    {
        Off,
        WinOneMatch,
        WinTwoMatches,
        WinThreeMatches,
        Lose
    }

    public class ColorGameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private BetSlip betSlip;
        [SerializeField] private GameHistoryManager historyManager;
        [SerializeField] private SaveManager saveManager;

        [Header("Roll presentation (optional)")]
        [Tooltip("Drag ColorMachine3D (Machine3DPresenter) here for physical results. Leave empty for the old timed animation.")]
        [SerializeField] private MonoBehaviour rollPresenter;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [Header("DEV TEST ONLY (Editor and development builds only)")]
        [Tooltip("Custom forced colors (works with the 3D machine too). Used when no Force button mode is active.")]
        [SerializeField] private bool useForcedResult = false;
        [SerializeField] private GameColor[] forcedResult = { GameColor.Red, GameColor.Red, GameColor.Red };
#endif

        public event Action<GameState> OnStateChanged;
        public event Action<bool, GameColor> OnSelectedColorChanged;
        public event Action<string> OnRoundStarted;
        public event Action<GameColor[]> OnRollStarted;
        public event Action<GameColor[], PayoutResult> OnResultsShown;
        public event Action<GameRound, PayoutResult> OnRoundCompleted;
        public event Action<string> OnRoundVoided;
        public event Action OnBettingWindowOpened;
        public event Action<bool> OnBettingWindowClosed;
        public event Action OnRoundSkipped;
        public event Action<GameColor[]> OnNoBetResultShown;
        public event Action<GameColor[]> OnNoBetRoundCompleted;

        public GameState State { get; private set; } = GameState.Idle;
        
        /// <summary>The color tapped last (kept for the debug panel and the select sound).</summary>
        public bool HasSelectedColor { get; private set; }
        public GameColor SelectedColor { get; private set; }

        public GameRound LastRound { get; private set; }
        public PayoutResult LastPayout { get; private set; }
        public bool IsRoundRunning => isRoundRunning;
        public bool HasActiveWager { get; private set; }
        public bool IsBettingOpen { get; private set; }
        public bool IsMachineBusy => physicalPresenter != null && physicalPresenter.IsBusy;
        public bool UsesPhysicalResult => physicalPresenter != null;
        
        /// <summary>Set by ForceResultUI. Ignored in release builds. Needs a round with a wager.</summary>
        public ForceResultMode ForcedMode { get; set; } = ForceResultMode.Off;

        /// <summary>True when a round with bets may start right now.</summary>
        public bool CanStartRound =>
            isReady && !isRoundRunning && State == GameState.Idle &&
            betSlip.HasBets && walletManager.CanAfford(betSlip.TotalBet);

        private IResultService resultService;
        private IRollPresenter presenter;
        private IPhysicalRollPresenter physicalPresenter;
        private PayoutCalculator payoutCalculator;
        private bool isReady;
        private bool isRoundRunning;

        private void Awake()
        {
            if (config == null || walletManager == null || betManager == null || betSlip == null ||
                historyManager == null || saveManager == null)
            {
                Debug.LogError("[ColorGameManager] A reference is missing in the Inspector (Bet Slip included).", this);
                return;
            }

            if (rollPresenter != null)
            {
                physicalPresenter = rollPresenter as IPhysicalRollPresenter;

                if (physicalPresenter == null)
                {
                    presenter = rollPresenter as IRollPresenter;
                    if (presenter == null)
                    {
                        Debug.LogError("[ColorGameManager] The Roll Presenter implements neither IPhysicalRollPresenter " +
                                       "nor IRollPresenter. The old timed animation will be used.", this);
                    }
                }
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (physicalPresenter != null && useForcedResult)
            {
                Debug.LogWarning("[ColorGameManager] 'Use Forced Result' is IGNORED: the physical cubes decide the result.", this);
            }
#endif

            resultService = new ColorResultGenerator();
            payoutCalculator = new PayoutCalculator(config);
            isReady = true;
        }

        public void SetResultService(IResultService service)
        {
            if (service == null)
            {
                Debug.LogError("[ColorGameManager] SetResultService called with null.");
                return;
            }
            resultService = service;
        }

        public void SetRollPresenter(IRollPresenter newPresenter)
        {
            presenter = newPresenter;
        }

        // ---------- Last tapped color (no longer decides anything) ----------

        public bool TrySelectColor(GameColor color)
        {
            if (State != GameState.Idle)
            {
                Debug.LogWarning("[ColorGameManager] Color cannot be changed while a round is running.");
                return false;
            }

            if (!Enum.IsDefined(typeof(GameColor), color))
            {
                Debug.LogError($"[ColorGameManager] Invalid color: {color}");
                return false;
            }

            SelectedColor = color;
            HasSelectedColor = true;
            OnSelectedColorChanged?.Invoke(true, SelectedColor);
            return true;
        }

        public void ClearColorSelection()
        {
            if (!HasSelectedColor) return;
            HasSelectedColor = false;
            OnSelectedColorChanged?.Invoke(false, SelectedColor);
        }

        // ---------- Starting a round ----------

        /// <summary>Manual start (ROLL button, if you ever bring it back). Uses the bets on the slip.</summary>
        public bool TryStartRound()
        {
            if (!isReady)
            {
                Debug.LogError("[ColorGameManager] Cannot start: the manager is not set up correctly.", this);
                return false;
            }

            if (isRoundRunning || State != GameState.Idle)
            {
                Debug.LogWarning("[ColorGameManager] A round is already running.");
                return false;
            }

            if (!betSlip.HasBets)
            {
                Debug.LogWarning("[ColorGameManager] No bets placed.");
                return false;
            }

            int total = betSlip.TotalBet;
            if (total <= 0)
            {
                Debug.LogError($"[ColorGameManager] Invalid total bet: {total}");
                return false;
            }

            if (!walletManager.CanAfford(total))
            {
                Debug.LogWarning("[ColorGameManager] Insufficient credits for these bets.");
                return false;
            }

            IsBettingOpen = false;
            isRoundRunning = true;
            StartCoroutine(RoundRoutine(true, betSlip.GetBets())); // a copy: the bets cannot change mid-round
            return true;
        }

        // ---------- Betting window (used by AutoRollController) ----------

        public bool BeginBettingWindow()
        {
            if (!isReady)
            {
                Debug.LogError("[ColorGameManager] Cannot open betting: the manager is not set up correctly.", this);
                return false;
            }

            if (isRoundRunning || State != GameState.Idle)
            {
                Debug.LogWarning("[ColorGameManager] Betting cannot open while a round is running.");
                return false;
            }

            betSlip.Clear();
            ClearColorSelection();
            betManager.ClearBet();

            IsBettingOpen = true;
            OnBettingWindowOpened?.Invoke();
            return true;
        }

        /// <summary>The betting deadline. Starts the machine in every case. True = a wager was placed.</summary>
        public bool CloseBettingAndStartRound()
        {
            if (!isReady || isRoundRunning || State != GameState.Idle)
            {
                Debug.LogWarning("[ColorGameManager] The betting deadline passed while a round was already running. Ignored.");
                IsBettingOpen = false;
                return false;
            }

            bool hasWager = betSlip.HasBets && walletManager.CanAfford(betSlip.TotalBet);
            List<ColorBetEntry> bets = hasWager ? betSlip.GetBets() : null;

            if (!hasWager)
            {
                betSlip.Clear();
                ClearColorSelection();
                betManager.ClearBet();
            }

            IsBettingOpen = false;
            OnBettingWindowClosed?.Invoke(hasWager);

            isRoundRunning = true;
            StartCoroutine(RoundRoutine(hasWager, bets));
            return hasWager;
        }

        // ---------- The round ----------

        private IEnumerator RoundRoutine(bool hasWager, List<ColorBetEntry> bets)
        {
            try
            {
                bool physical = physicalPresenter != null;
                GameColor[] results = null;

                if (!physical)
                {
                    results = GenerateResults(hasWager ? bets : null);
                    if (results == null) yield break;
                }

                int totalBet = 0;
                if (hasWager && bets != null)
                {
                    foreach (ColorBetEntry entry in bets) totalBet += entry.amount;
                }

                SetState(GameState.Rolling);

                long startingBalance = 0;
                string roundId = string.Empty;

                if (hasWager)
                {
                    startingBalance = walletManager.GetBalance();
                    roundId = historyManager.ReserveNextRoundId();

                    // The whole wager is spent exactly once (one BET transaction).
                    if (!walletManager.SpendCredits(totalBet, roundId))
                    {
                        Debug.LogError($"[ColorGameManager] Could not deduct the bet of {totalBet}. The machine runs without a wager.", this);
                        hasWager = false;
                    }
                }

                HasActiveWager = hasWager;

                if (hasWager) OnRoundStarted?.Invoke(roundId);
                else OnRoundSkipped?.Invoke();

                OnRollStarted?.Invoke(results);

                if (physical)
                { 
                    // DEV TEST: hand a forced result to the machine (does nothing in release builds).
                GameColor[] forced = ResolveForcedResult(hasWager ? bets : null);
                if (forced != null && rollPresenter is IForceableRollPresenter forceable)
                {
                 forceable.SetForcedResult(forced);
                 Debug.LogWarning("[ColorGameManager] DEV TEST: forced result " + string.Join(" / ", forced), this);
                }
                    GameColor[] physicalResult = null;
                    yield return StartCoroutine(physicalPresenter.PlayPhysicalRoll(c => physicalResult = c));

                    if (!IsValidResult(physicalResult))
                    {
                        if (hasWager)
                        {
                            VoidRound(roundId, totalBet);
                        }
                        else
                        {
                            Debug.LogWarning("[ColorGameManager] The cubes did not settle cleanly in a round without a wager.", this);
                            OnNoBetRoundCompleted?.Invoke(null);
                        }

                        yield return WaitForMachineIdle();
                        yield break;
                    }

                    results = physicalResult;
                }
                else if (presenter != null)
                {
                    GameColor[] confirmed = null;
                    yield return StartCoroutine(presenter.PlayRoll(results, c => confirmed = c));

                    if (confirmed == null)
                    {
                        Debug.LogWarning("[ColorGameManager] The roll presentation did not confirm a result. Using the official result.", this);
                    }
                    else if (!SameColors(results, confirmed))
                    {
                        Debug.LogError("[ColorGameManager] The physical dice do not match the official result. Using the official result.", this);
                    }
                }
                else
                {
                    float animationTime = Mathf.Max(config.Slot1StopTime, config.Slot2StopTime, config.Slot3StopTime);
                    yield return new WaitForSeconds(animationTime + config.ResultRevealDelay);
                }

                if (!hasWager)
                {
                    SetState(GameState.ShowingResult);
                    OnNoBetResultShown?.Invoke(results);
                    yield return new WaitForSeconds(config.ResultRevealDelay);

                    OnNoBetRoundCompleted?.Invoke(results);
                    yield return WaitForMachineIdle();
                    yield break;
                }

                // The only place a win is decided: every bet color is paid from the REAL result.
                SetState(GameState.ShowingResult);
                PayoutResult payout = payoutCalculator.Calculate(results, bets);
                OnResultsShown?.Invoke(results, payout);
                yield return new WaitForSeconds(config.ResultRevealDelay);

                SetState(GameState.ProcessingPayout);

                if (payout.Winnings > 0)
                {
                    if (!walletManager.AddCredits(payout.Winnings, TransactionType.Win, roundId))
                    {
                        Debug.LogError($"[ColorGameManager] Could not add winnings of {payout.Winnings}.", this);
                    }
                }

                long endingBalance = walletManager.GetBalance();

                GameRound round = GameRound.Create(roundId, bets, results, payout, startingBalance, endingBalance);
                if (round != null) historyManager.AddRound(round);

                saveManager.SaveGame();

                LastRound = round;
                LastPayout = payout;
                OnRoundCompleted?.Invoke(round, payout);

                yield return WaitForMachineIdle();
            }
            finally
            {
                HasActiveWager = false;
                isRoundRunning = false;

                // The round is over (paid, cancelled or without a wager): clear the table for the next round.
                if (betSlip != null) betSlip.Clear();
                if (betManager != null) betManager.ClearBet();
                ClearColorSelection();

                SetState(GameState.Idle);
            }
        }

        private IEnumerator WaitForMachineIdle()
        {
            float timeout = 20f;
            while (IsMachineBusy && timeout > 0f)
            {
                timeout -= Time.deltaTime;
                yield return null;
            }

            if (timeout <= 0f) Debug.LogWarning("[ColorGameManager] The machine did not finish resetting in time.", this);
        }

        private void VoidRound(string roundId, int totalBet)
        {
            Debug.LogWarning($"[ColorGameManager] {roundId}: the cubes did not settle cleanly. Round cancelled, refunding {totalBet}.", this);

            if (!walletManager.AddCredits(totalBet, TransactionType.Refund, roundId))
            {
                Debug.LogError($"[ColorGameManager] Could not refund the bet of {totalBet} for {roundId}.", this);
            }

            saveManager.SaveGame();
            OnRoundVoided?.Invoke(roundId);
        }

        private static bool IsValidResult(GameColor[] result)
        {
            return result != null && result.Length == ColorGameConfig.ResultSlotCount;
        }

                private GameColor[] GenerateResults(IReadOnlyList<ColorBetEntry> bets)
        {
            GameColor[] results = resultService.GenerateResult();

            GameColor[] forced = ResolveForcedResult(bets);
            if (forced != null)
            {
                Debug.LogWarning("[ColorGameManager] DEV TEST: using a forced result instead of a random one.");
                results = forced;
            }

            if (!IsValidResult(results))
            {
                Debug.LogError("[ColorGameManager] Result service returned an invalid result. Round cancelled, nothing was spent.", this);
                return null;
            }

            return results;
        }

        // Returns the colors to force, or null for a normal random round. Always null in release builds.
        private GameColor[] ResolveForcedResult(IReadOnlyList<ColorBetEntry> bets)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (ForcedMode != ForceResultMode.Off)
            {
                if (bets == null || bets.Count == 0)
                {
                    Debug.LogWarning("[ColorGameManager] DEV TEST: Force Result needs a round with a wager. A normal result is used.", this);
                    return null;
                }
                return BuildForcedResult(bets);
            }

            if (useForcedResult && forcedResult != null && forcedResult.Length == ColorGameConfig.ResultSlotCount)
            {
                return (GameColor[])forcedResult.Clone();
            }
#endif
            return null;
        }

        // WIN: the color with the biggest bet shows 1, 2 or 3 times. LOSE: only colors nobody bet on.
        private GameColor[] BuildForcedResult(IReadOnlyList<ColorBetEntry> bets)
        {
            ColorBetEntry main = bets[0];
            foreach (ColorBetEntry bet in bets)
            {
                if (bet.amount > main.amount) main = bet;
            }

            List<GameColor> unbet = new List<GameColor>();
            List<GameColor> otherBets = new List<GameColor>();
            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition == null || definition.color == main.color) continue;

                bool hasBet = false;
                foreach (ColorBetEntry bet in bets)
                {
                    if (bet.color == definition.color) hasBet = true;
                }
                (hasBet ? otherBets : unbet).Add(definition.color);
            }

            GameColor[] result = new GameColor[ColorGameConfig.ResultSlotCount];

            if (ForcedMode == ForceResultMode.Lose)
            {
                if (unbet.Count == 0)
                {
                    Debug.LogWarning("[ColorGameManager] DEV TEST: every color has a bet, so a losing result is impossible. A normal result is used.", this);
                    return null;
                }
                for (int i = 0; i < result.Length; i++) result[i] = unbet[UnityEngine.Random.Range(0, unbet.Count)];
                return result;
            }

            int matches = ForcedMode == ForceResultMode.WinOneMatch ? 1
                        : ForcedMode == ForceResultMode.WinTwoMatches ? 2 : 3;

            List<int> slots = new List<int> { 0, 1, 2 };
            Shuffle(slots);
            Shuffle(unbet);
            Shuffle(otherBets);
            List<GameColor> fillers = new List<GameColor>(unbet);
            fillers.AddRange(otherBets); // distinct fillers, so the winning color keeps the highest count

            for (int i = 0; i < result.Length; i++)
            {
                if (i < matches) result[slots[i]] = main.color;
                else result[slots[i]] = fillers.Count > 0 ? fillers[(i - matches) % fillers.Count] : main.color;
            }
            return result;
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private static bool SameColors(GameColor[] a, GameColor[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        private void SetState(GameState newState)
        {
            if (State == newState) return;

            State = newState;
            if (betManager != null) betManager.SetLocked(State != GameState.Idle);
            OnStateChanged?.Invoke(State);
        }
    }
}