using System;
using System.Collections;
using UnityEngine;
using Shared.Save;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.History;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// Controls one complete round. This and PayoutCalculator are the ONLY scripts that decide outcomes.
    /// The UI listens to the events below and only displays what it is told.
    ///
    /// PHYSICAL mode (Machine3DPresenter, IPhysicalRollPresenter):
    ///   validate -> lock + deduct bet -> cubes roll and settle -> READ the real top faces ->
    ///   calculate payout -> show result -> add winnings -> save round -> back to Idle
    ///   The result is whatever the cubes show. Nothing is decided in advance and nothing is corrected.
    ///   If the cubes cannot settle cleanly, the round is cancelled and the bet is refunded.
    ///
    /// BETTING CYCLE (used by AutoRollController): BeginBettingWindow() clears the previous choices, the player picks a
    /// color and a bet, and CloseBettingAndStartRound() runs the machine. If BOTH choices are valid the bet is spent once
    /// and the round is a normal wagered round. Otherwise the round has NO wager: nothing is spent, recorded, paid or
    /// counted, but the machine still completes its full cycle.
    ///
    /// LEGACY modes (IRollPresenter such as the 2D ColorMachine, or no presenter):
    ///   the official result is generated first, as before.
    /// In a real-money version, result generation, payout and wallet changes would run on a server.
    /// </summary>
    public class ColorGameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private GameHistoryManager historyManager;
        [SerializeField] private SaveManager saveManager;

        [Header("Roll presentation (optional)")]
        [Tooltip("Drag ColorMachine3D (Machine3DPresenter) here for physical results. Leave empty for the old timed animation.")]
        [SerializeField] private MonoBehaviour rollPresenter;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [Header("DEV TEST ONLY (Editor and development builds only)")]
        [Tooltip("Only used WITHOUT the physical machine. Physical results can never be forced.")]
        [SerializeField] private bool useForcedResult = false;
        [SerializeField] private GameColor[] forcedResult = { GameColor.Red, GameColor.Red, GameColor.Red };
#endif

        /// <summary>Raised whenever the state changes.</summary>
        public event Action<GameState> OnStateChanged;

        /// <summary>Raised when the selection changes. First value: is a color selected? Second: which one.</summary>
        public event Action<bool, GameColor> OnSelectedColorChanged;

        /// <summary>Raised when the bet has been deducted. Carries the new round ID.</summary>
        public event Action<string> OnRoundStarted;

        /// <summary>
        /// Raised when the roll starts. In PHYSICAL mode the parameter is null, because the result does not exist yet.
        /// In legacy modes it carries the official colors (do not display them before the roll ends).
        /// </summary>
        public event Action<GameColor[]> OnRollStarted;

        /// <summary>Raised when the roll is over: the real colors and the PayoutResult (highlight matches).</summary>
        public event Action<GameColor[], PayoutResult> OnResultsShown;

        /// <summary>Raised after winnings were paid, the round saved and the wallet updated.</summary>
        public event Action<GameRound, PayoutResult> OnRoundCompleted;

        /// <summary>Raised when the cubes could not settle cleanly: the bet was refunded and no round was recorded.</summary>
        public event Action<string> OnRoundVoided;

        /// <summary>Raised when a fresh betting window opens. All earlier choices have been cleared.</summary>
        public event Action OnBettingWindowOpened;

        /// <summary>Raised when the betting deadline passes. True = a valid wager was placed, false = no bet.</summary>
        public event Action<bool> OnBettingWindowClosed;

        /// <summary>Raised when a round starts WITHOUT a wager (deadline passed without a color and a bet).</summary>
        public event Action OnRoundSkipped;

        /// <summary>Raised with the real cube colors of a round without a wager. Display only: no win, no loss.</summary>
        public event Action<GameColor[]> OnNoBetResultShown;

        /// <summary>Raised when a round without a wager is over (the colors, or null if the cubes did not settle).</summary>
        public event Action<GameColor[]> OnNoBetRoundCompleted;

        public GameState State { get; private set; } = GameState.Idle;
        public bool HasSelectedColor { get; private set; }
        public GameColor SelectedColor { get; private set; }

        /// <summary>The last finished round (null until one is played). Used by the debug panel.</summary>
        public GameRound LastRound { get; private set; }
        public PayoutResult LastPayout { get; private set; }

        /// <summary>True while a round is being processed (from ROLL until the controls unlock).</summary>
        public bool IsRoundRunning => isRoundRunning;

        /// <summary>True while a bet has been spent and its round is not finished yet (leaving now would lose the bet).</summary>
        public bool HasActiveWager { get; private set; }

        /// <summary>True between BeginBettingWindow() and the deadline.</summary>
        public bool IsBettingOpen { get; private set; }

        /// <summary>True while the physical machine is rolling or returning the cubes to the shelf.</summary>
        public bool IsMachineBusy => physicalPresenter != null && physicalPresenter.IsBusy;

        /// <summary>True when the result comes from real physics.</summary>
        public bool UsesPhysicalResult => physicalPresenter != null;

        /// <summary>True when ROLL may be pressed right now.</summary>
        public bool CanStartRound =>
            isReady && !isRoundRunning && State == GameState.Idle &&
            HasSelectedColor && betManager.HasBet && walletManager.CanAfford(betManager.SelectedBet);

        private IResultService resultService;
        private IRollPresenter presenter;
        private IPhysicalRollPresenter physicalPresenter;
        private PayoutCalculator payoutCalculator;
        private bool isReady;
        private bool isRoundRunning;

        private void Awake()
        {
            if (config == null || walletManager == null || betManager == null ||
                historyManager == null || saveManager == null)
            {
                Debug.LogError("[ColorGameManager] A reference is missing in the Inspector.", this);
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

        /// <summary>Lets a future server-based result service replace the local random generator (legacy modes).</summary>
        public void SetResultService(IResultService service)
        {
            if (service == null)
            {
                Debug.LogError("[ColorGameManager] SetResultService called with null.");
                return;
            }
            resultService = service;
        }

        /// <summary>Sets a legacy presenter that performs a result decided in advance.</summary>
        public void SetRollPresenter(IRollPresenter newPresenter)
        {
            presenter = newPresenter;
        }

        // ---------- Color selection ----------

        /// <summary>Selects a color. Only allowed while the game is Idle.</summary>
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

        /// <summary>Called by the ROLL button and by AutoRollController. Validates everything, then runs the round.</summary>
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

            if (!HasSelectedColor)
            {
                Debug.LogWarning("[ColorGameManager] No color selected.");
                return false;
            }

            if (!betManager.HasBet)
            {
                Debug.LogWarning("[ColorGameManager] No bet selected.");
                return false;
            }

            int bet = betManager.SelectedBet;

            if (bet <= 0 || !config.IsValidBetAmount(bet))
            {
                Debug.LogError($"[ColorGameManager] Invalid bet: {bet}");
                return false;
            }

            if (!walletManager.CanAfford(bet))
            {
                Debug.LogWarning("[ColorGameManager] Insufficient credits for this bet.");
                return false;
            }

            IsBettingOpen = false;
            isRoundRunning = true;
            StartCoroutine(RoundRoutine(true, SelectedColor, bet)); // color and bet are captured here and cannot change mid-round
            return true;
        }

        // ---------- Betting window (used by AutoRollController) ----------

        /// <summary>
        /// Opens a fresh betting window. The previous color and bet are cleared, so every round
        /// needs a new explicit choice. Does nothing while a round is running.
        /// </summary>
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

            ClearColorSelection();
            betManager.ClearBet();

            IsBettingOpen = true;
            OnBettingWindowOpened?.Invoke();
            return true;
        }

        /// <summary>
        /// The betting deadline. Starts the machine round in every case.
        /// Returns true if a valid wager (color AND bet AND enough credits) was placed.
        /// Without a valid wager nothing is spent or recorded, incomplete choices are cleared,
        /// and the machine still runs a full cycle.
        /// </summary>
        public bool CloseBettingAndStartRound()
        {
            if (!isReady || isRoundRunning || State != GameState.Idle)
            {
                Debug.LogWarning("[ColorGameManager] The betting deadline passed while a round was already running. Ignored.");
                IsBettingOpen = false;
                return false;
            }

            GameColor color = SelectedColor;
            int bet = betManager.HasBet ? betManager.SelectedBet : 0;

            bool hasWager = HasSelectedColor && bet > 0 && config.IsValidBetAmount(bet) && walletManager.CanAfford(bet);

            if (!hasWager)
            {
                // Color only, bet only, nothing, or unaffordable: drop the incomplete choices.
                ClearColorSelection();
                betManager.ClearBet();
            }

            IsBettingOpen = false;
            OnBettingWindowClosed?.Invoke(hasWager);

            isRoundRunning = true;
            StartCoroutine(RoundRoutine(hasWager, color, hasWager ? bet : 0));
            return hasWager;
        }

        // ---------- The round ----------

        private IEnumerator RoundRoutine(bool hasWager, GameColor selectedColor, int bet)
        {
            // try/finally guarantees we always return to Idle, even if something throws.
            try
            {
                bool physical = physicalPresenter != null;
                GameColor[] results = null;

                // Legacy modes decide the result first. Physical mode has nothing to decide yet.
                if (!physical)
                {
                    results = GenerateResults();
                    if (results == null)
                    {
                        yield break;
                    }
                }

                // Lock the controls BEFORE spending, so BetManager does not react to the balance drop.
                SetState(GameState.Rolling);

                long startingBalance = 0;
                string roundId = string.Empty;

                if (hasWager)
                {
                    startingBalance = walletManager.GetBalance();
                    roundId = historyManager.ReserveNextRoundId();

                    // Deduct the bet exactly once (creates a BET transaction and saves the game).
                    if (!walletManager.SpendCredits(bet, roundId))
                    {
                        Debug.LogError($"[ColorGameManager] Could not deduct the bet of {bet}. The machine runs without a wager.", this);
                        hasWager = false;
                    }
                }

                HasActiveWager = hasWager;

                if (hasWager) OnRoundStarted?.Invoke(roundId);
                else OnRoundSkipped?.Invoke();

                OnRollStarted?.Invoke(results); // null in physical mode

                if (physical)
                {
                    // The cubes roll, collide and settle by themselves. We only wait and then read what they show.
                    GameColor[] physicalResult = null;
                    yield return StartCoroutine(physicalPresenter.PlayPhysicalRoll(c => physicalResult = c));

                    if (!IsValidResult(physicalResult))
                    {
                        if (hasWager)
                        {
                            VoidRound(roundId, bet);
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

                // No wager: show the real colors and finish. No payout, no history, no statistics, no win or loss.
                if (!hasWager)
                {
                    SetState(GameState.ShowingResult);
                    OnNoBetResultShown?.Invoke(results);
                    yield return new WaitForSeconds(config.ResultRevealDelay);

                    OnNoBetRoundCompleted?.Invoke(results);
                    yield return WaitForMachineIdle();
                    yield break;
                }

                // Count matches and calculate the payout from the REAL result. This is the only place a win is decided.
                SetState(GameState.ShowingResult);
                PayoutResult payout = payoutCalculator.Calculate(results, selectedColor, bet);
                OnResultsShown?.Invoke(results, payout);
                yield return new WaitForSeconds(config.ResultRevealDelay);

                // Pay out, record the round, save. This runs exactly once per round.
                SetState(GameState.ProcessingPayout);

                if (payout.Winnings > 0)
                {
                    if (!walletManager.AddCredits(payout.Winnings, TransactionType.Win, roundId))
                    {
                        Debug.LogError($"[ColorGameManager] Could not add winnings of {payout.Winnings}.", this);
                    }
                }

                long endingBalance = walletManager.GetBalance();

                GameRound round = GameRound.Create(roundId, selectedColor, bet, results, payout,
                                                   startingBalance, endingBalance);
                if (round != null) historyManager.AddRound(round);

                saveManager.SaveGame();

                LastRound = round;
                LastPayout = payout;
                OnRoundCompleted?.Invoke(round, payout);

                // Stay locked until the cubes are back on the shelf, so the next betting window opens on a ready machine.
                yield return WaitForMachineIdle();
            }
            finally
            {
                HasActiveWager = false;
                isRoundRunning = false;
                SetState(GameState.Idle); // unlocks the buttons
            }
        }

        // Waits (with a safety timeout) until the machine has returned the cubes to the shelf.
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

        // The cubes never produced a clean result (stuck, out of the tray, never settled).
        // Nothing is decided and nothing is faked: the bet goes back and no round is recorded.
        private void VoidRound(string roundId, int bet)
        {
            Debug.LogWarning($"[ColorGameManager] {roundId}: the cubes did not settle cleanly. Round cancelled, refunding {bet}.", this);

            if (!walletManager.AddCredits(bet, TransactionType.Refund, roundId))
            {
                Debug.LogError($"[ColorGameManager] Could not refund the bet of {bet} for {roundId}.", this);
            }

            saveManager.SaveGame();
            OnRoundVoided?.Invoke(roundId);
        }

        private static bool IsValidResult(GameColor[] result)
        {
            return result != null && result.Length == ColorGameConfig.ResultSlotCount;
        }

        private GameColor[] GenerateResults()
        {
            GameColor[] results = resultService.GenerateResult();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (useForcedResult && forcedResult != null && forcedResult.Length == ColorGameConfig.ResultSlotCount)
            {
                Debug.LogWarning("[ColorGameManager] DEV TEST: using the forced result instead of a random one.");
                results = (GameColor[])forcedResult.Clone();
            }
#endif

            if (!IsValidResult(results))
            {
                Debug.LogError("[ColorGameManager] Result service returned an invalid result. Round cancelled, nothing was spent.", this);
                return null;
            }

            return results;
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

            // Betting is only editable while Idle.
            if (betManager != null) betManager.SetLocked(State != GameState.Idle);

            OnStateChanged?.Invoke(State);
        }
    }
}
