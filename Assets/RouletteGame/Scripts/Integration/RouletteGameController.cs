using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Shared.Save;
using Shared.Wallet;
using RouletteGame.Core;
using RouletteGame.Data;
using RouletteGame.UI;
using RouletteGame.Wheel;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace RouletteGame.Integration
{
    public enum RouletteState
    {
        Betting,        // chips can be placed
        Confirmed,      // the table is locked, waiting for SPIN (nothing charged yet)
        Spinning,       // the round is settled and the wheel is playing it back
        ShowingResult   // the result card is open
    }

    /// <summary>
    /// Runs one round after another. It owns no rules: bets live in RouletteBetSlip, money in RouletteRoundSettler,
    /// the winning number in the outcome source, the pictures in the views.
    ///
    /// Flow: place chips, CONFIRM (locks the table, no charge), SPIN. At SPIN the whole round is settled at once
    /// (charge once, credit once, history saved), THEN the wheel plays the result back. Closing the app mid-spin
    /// can never lose or duplicate credits, and the balance on screen stays behind until the ball has stopped.
    /// </summary>
    public class RouletteGameController : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private RouletteSettings settings;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private RouletteHistoryManager historyManager;

        [Header("Views")]
        [SerializeField] private WheelView wheel;
        [SerializeField] private BettingTableView table;
        [SerializeField] private ChipSelectorUI chips;
        [SerializeField] private RouletteHUD hud;
        [SerializeField] private RouletteResultPanel resultPanel;

        [Header("Buttons")]
        [SerializeField] private Button undoButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button repeatButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private TMP_Text removeLabel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TMP_Text confirmLabel;
        [SerializeField] private Button spinButton;
        [SerializeField] private TMP_Text spinLabel;
        [SerializeField] private Button homeButton;

        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        // Audio and effects listen to these (RouletteAudio, Phase 6). The controller itself plays no sound.
        public event Action OnChipPlaced;
        public event Action OnChipRemoved;
        public event Action OnBetsConfirmed;
        public event Action OnBetsUnlocked;
        public event Action OnButtonPressed;
        public event Action OnSpinBegan;

        /// <summary>The ball has stopped and the result is on screen.</summary>
        public event Action<RouletteSettlementResult> OnRoundResult;

        public RouletteState State { get; private set; } = RouletteState.Betting;

        private RouletteBetSlip slip;
        private RouletteRoundSettler settler;
        private IOutcomeSource outcomeSource;
        private CryptoOutcomeSource ownedSource;
        private RouletteSettlementResult lastResult;
        private string pendingOwedRoundId;
        private float owedTimer;
        private long displayBalance;
        private bool removeMode;
        private bool isReady;
        private bool isLeaving;

        private void Awake()
        {
            if (settings == null || walletManager == null || saveManager == null || historyManager == null ||
                wheel == null || table == null || chips == null || hud == null || resultPanel == null ||
                undoButton == null || clearButton == null || repeatButton == null || removeButton == null ||
                confirmButton == null || spinButton == null || homeButton == null)
            {
                Debug.LogError("[RouletteGameController] A reference is missing in the Inspector.", this);
                enabled = false;
                return;
            }

            var problems = settings.GetProblems();
            if (problems.Count > 0)
            {
                Debug.LogError("[RouletteGameController] The settings asset has problems: " + string.Join(" | ", problems), this);
                enabled = false;
                return;
            }

            RouletteLimits limits = settings.Limits;
            slip = new RouletteBetSlip(limits);
            settler = new RouletteRoundSettler(new UnityRouletteWallet(walletManager, saveManager), limits);
            ownedSource = new CryptoOutcomeSource();
            outcomeSource = ownedSource;
        }

        private void OnDestroy()
        {
            if (ownedSource != null) ownedSource.Dispose();
        }

        private void OnEnable()
        {
            if (slip == null) return;

            slip.Changed += RefreshUi;
            table.OnAreaTapped += HandleAreaTapped;
            chips.OnSelectedChanged += HandleChipSelected;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
            historyManager.OnHistoryChanged += HandleHistoryChanged;
            resultPanel.OnContinue += HandleContinue;
        }

        private void OnDisable()
        {
            if (slip == null) return;

            slip.Changed -= RefreshUi;
            if (table != null) table.OnAreaTapped -= HandleAreaTapped;
            if (chips != null) chips.OnSelectedChanged -= HandleChipSelected;
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
            if (historyManager != null) historyManager.OnHistoryChanged -= HandleHistoryChanged;
            if (resultPanel != null) resultPanel.OnContinue -= HandleContinue;
        }

        private void Start()
        {
            if (slip == null) return;

            wheel.Configure(settings.Spin);

            undoButton.onClick.RemoveAllListeners();
            undoButton.onClick.AddListener(OnUndo);
            clearButton.onClick.RemoveAllListeners();
            clearButton.onClick.AddListener(OnClear);
            repeatButton.onClick.RemoveAllListeners();
            repeatButton.onClick.AddListener(OnRepeat);
            removeButton.onClick.RemoveAllListeners();
            removeButton.onClick.AddListener(OnToggleRemove);
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirm);
            spinButton.onClick.RemoveAllListeners();
            spinButton.onClick.AddListener(OnSpin);
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(GoHome);

            displayBalance = walletManager.GetBalance();
            hud.SetBalance(displayBalance, true);
            RefreshLastNumbers();
            isReady = true;
            RefreshUi();
        }

        private void Update()
        {
            if (!isReady || isLeaving) return;

            TryRecoverOwed();

            if (BackPressed())
            {
                if (State == RouletteState.ShowingResult) HandleContinue();
                else GoHome();
            }
        }

        /// <summary>For tests and development: replaces the winning-number source. Ignored while a round is running.</summary>
        public void SetOutcomeSource(IOutcomeSource source)
        {
            if (source == null || State == RouletteState.Spinning || State == RouletteState.ShowingResult) return;
            outcomeSource = source;
        }

        // ---------- Table and chips ----------

        private void HandleAreaTapped(RouletteBet area)
        {
            if (!isReady || State != RouletteState.Betting)
            {
                if (isReady) hud.ShowMessage(State == RouletteState.Confirmed ? "BETS LOCKED - PRESS EDIT BETS" : RouletteBetSlip.ClosedMessage);
                return;
            }

            string error;
            if (removeMode)
            {
                if (slip.TryRemoveArea(area.Key, out error)) OnChipRemoved?.Invoke();
                else hud.ShowMessage(error);
                return;
            }

            if (slip.TryPlaceChip(area, chips.SelectedAmount, walletManager.GetBalance(), out error)) OnChipPlaced?.Invoke();
            else hud.ShowMessage(error);
        }

        private void HandleChipSelected(long amount)
        {
            OnButtonPressed?.Invoke();
            if (removeMode) { removeMode = false; RefreshUi(); }
        }

        // ---------- Buttons ----------

        private void OnUndo()
        {
            if (State != RouletteState.Betting) return;
            if (slip.Undo()) OnChipRemoved?.Invoke();
        }

        private void OnClear()
        {
            if (State != RouletteState.Betting) return;
            if (slip.Clear()) OnChipRemoved?.Invoke();
        }

        private void OnRepeat()
        {
            if (State != RouletteState.Betting) return;
            if (slip.TryRepeatLast(walletManager.GetBalance(), out string error)) OnChipPlaced?.Invoke();
            else hud.ShowMessage(error);
        }

        private void OnToggleRemove()
        {
            if (State != RouletteState.Betting) return;
            removeMode = !removeMode;
            OnButtonPressed?.Invoke();
            RefreshUi();
        }

        // CONFIRM locks the table (nothing is charged). While locked the same button reads EDIT BETS and unlocks it.
        private void OnConfirm()
        {
            if (State == RouletteState.Betting)
            {
                if (!slip.TryConfirm(out string error)) { hud.ShowMessage(error); return; }
                removeMode = false;
                State = RouletteState.Confirmed;
                OnBetsConfirmed?.Invoke();
            }
            else if (State == RouletteState.Confirmed)
            {
                slip.Unlock();
                State = RouletteState.Betting;
                OnBetsUnlocked?.Invoke();
            }

            RefreshUi();
        }

        private void OnSpin()
        {
            if (State == RouletteState.Confirmed)
            {
                StartCoroutine(RoundRoutine());
            }
            else if (State == RouletteState.Spinning)
            {
                wheel.Speed = settings.FastForwardSpeed; // fast-forward the animation of an already settled round
            }
        }

        // ---------- The round ----------

        private IEnumerator RoundRoutine()
        {
            if (State != RouletteState.Confirmed || !slip.IsLocked) yield break;

            // From here no button can start a second round or change the bets.
            State = RouletteState.Spinning;
            RefreshUi();

            string roundId = historyManager.ReserveNextRoundId();
            RouletteBet[] bets = slip.Snapshot();
            long balanceBefore = walletManager.GetBalance();

            if (!TrySettle(bets, roundId, out RouletteSettlementResult result))
            {
                slip.Unlock();
                State = RouletteState.Betting;
                RefreshUi();
                yield break;
            }

            // The money is settled and the round is recorded BEFORE any animation.
            RouletteRoundRecord record = RouletteHistoryService.FromSettlement(result, DateTime.UtcNow);
            historyManager.AddRound(record);
            saveManager.SaveGame();

            lastResult = result;
            if (result.Status == RouletteSettlementStatus.CreditFailed) pendingOwedRoundId = roundId;

            // The screen balance only shows the stake taken. The winnings appear after the ball has stopped.
            displayBalance = balanceBefore - result.CreditsCharged;
            hud.SetBalance(displayBalance, false);
            hud.SetStatus("NO MORE BETS");
            table.ClearHighlight();

            OnSpinBegan?.Invoke();
            wheel.Speed = 1f;
            yield return wheel.Spin(result.WinningNumber);

            State = RouletteState.ShowingResult;
            hud.SetBalance(walletManager.GetBalance(), false);
            RefreshLastNumbers();
            table.HighlightNumber(result.WinningNumber);
            table.ShowOutcome(result.Payout);
            hud.SetStatus(string.Empty);
            if (pendingOwedRoundId != null) hud.ShowMessage("PAYOUT PENDING - RETRYING", 3f);

            OnRoundResult?.Invoke(result);
            resultPanel.Show(result);
            RefreshUi();

            float wait = settings.ResultAutoCloseSeconds;
            if (wait > 0f)
            {
                float elapsed = 0f;
                while (elapsed < wait && resultPanel.IsOpen && State == RouletteState.ShowingResult)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                if (resultPanel.IsOpen && State == RouletteState.ShowingResult) HandleContinue();
            }
        }

        // Settles the round. On any refusal nothing was charged, and the player is told why.
        private bool TrySettle(RouletteBet[] bets, string roundId, out RouletteSettlementResult result)
        {
            result = null;
            try
            {
                result = settler.Play(bets, roundId, outcomeSource);
            }
            catch (Exception e)
            {
                Debug.LogError("[RouletteGameController] " + roundId + " failed before any charge: " + e.Message, this);
                hud.ShowMessage("ROUND ERROR - NOTHING WAS CHARGED");
                return false;
            }

            bool played = result.Payout != null; // charged and played (CreditFailed is handled after the animation)
            if (!played)
            {
                Debug.LogWarning("[RouletteGameController] " + roundId + " not played: " + result.Status, this);
                hud.ShowMessage(StatusMessage(result.Status));
            }
            return played;
        }

        private void HandleContinue()
        {
            if (State != RouletteState.ShowingResult) return;

            resultPanel.Hide();
            table.ClearHighlight();
            wheel.Speed = 1f;
            slip.FinishRound(); // remembers the bets for REPEAT, clears the table, opens it again
            State = RouletteState.Betting;
            displayBalance = walletManager.GetBalance();
            hud.SetBalance(displayBalance, false);
            RefreshUi();
        }

        // A payout the wallet refused is remembered by the settler. Keep asking until it is paid, exactly once.
        private void TryRecoverOwed()
        {
            if (pendingOwedRoundId == null || State == RouletteState.Spinning) return;

            owedTimer += Time.deltaTime;
            if (owedTimer < 3f) return;
            owedTimer = 0f;

            if (settler.TryRecoverOwed(pendingOwedRoundId, out long credited))
            {
                pendingOwedRoundId = null;
                saveManager.SaveGame();
                hud.ShowMessage("PAYOUT CREDITED: " + RouletteText.Credits(credited), 3f);
            }
        }

        private void GoHome()
        {
            if (isLeaving) return;

            // Never leave with a payout still owed: it would be lost.
            if (pendingOwedRoundId != null && !settler.TryRecoverOwed(pendingOwedRoundId, out _))
            {
                hud.ShowMessage("PAYOUT PENDING - TRY AGAIN IN A MOMENT", 2.5f);
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                Debug.LogError($"[RouletteGameController] Scene '{mainMenuSceneName}' is not in the build list (File > Build Profiles > Scene List).", this);
                return;
            }

            isLeaving = true;
            OnButtonPressed?.Invoke();
            saveManager.SaveGame(); // the wallet was settled before the animation, so leaving is always safe
            SceneManager.LoadScene(mainMenuSceneName);
        }

        // ---------- Display ----------

        private void HandleBalanceChanged(long balance)
        {
            // While a round plays back, the HUD follows the animation instead.
            if (!isReady || State == RouletteState.Spinning || State == RouletteState.ShowingResult) return;
            displayBalance = balance;
            hud.SetBalance(balance, false);
            RefreshUi();
        }

        private void HandleHistoryChanged()
        {
            if (!isReady || State == RouletteState.Spinning || State == RouletteState.ShowingResult) return;
            RefreshLastNumbers();
        }

        private void RefreshLastNumbers()
        {
            var recent = historyManager.RecentNumbers(settings.LastNumbersShown);
            hud.SetLastNumbers(recent);
            if (recent.Count > 0 && wheel.LastNumber < 0) wheel.SetRestingNumber(recent[0]);
        }

        private void RefreshUi()
        {
            if (!isReady) return;

            bool betting = State == RouletteState.Betting;
            long balance = walletManager.GetBalance();
            long total = slip.TotalBet;

            table.Refresh(slip.Bets);
            table.SetInteractable(betting);
            chips.SetInteractable(betting);
            chips.Refresh(balance - total);
            hud.SetTotalBet(total);
            hud.SetPotential(slip.MaxPossibleReturn());

            undoButton.interactable = betting && slip.CanUndo;
            clearButton.interactable = betting && slip.HasBets;
            repeatButton.interactable = betting && slip.HasLastRound;
            removeButton.interactable = betting && (slip.HasBets || removeMode);
            confirmButton.interactable = (betting && slip.HasBets) || State == RouletteState.Confirmed;
            spinButton.interactable = State == RouletteState.Confirmed || State == RouletteState.Spinning;

            if (removeLabel != null) removeLabel.text = removeMode ? "REMOVE: ON" : "REMOVE";
            if (confirmLabel != null) confirmLabel.text = State == RouletteState.Confirmed ? "EDIT BETS" : "CONFIRM BETS";
            if (spinLabel != null) spinLabel.text = State == RouletteState.Spinning ? "SKIP" : "SPIN";

            switch (State)
            {
                case RouletteState.Betting:
                    hud.SetStatus(removeMode ? "TAP A BET TO REMOVE IT" : (slip.HasBets ? "PRESS CONFIRM BETS" : "PLACE YOUR BETS"));
                    break;
                case RouletteState.Confirmed:
                    hud.SetStatus("BETS LOCKED - PRESS SPIN");
                    break;
            }
        }

        private static string StatusMessage(RouletteSettlementStatus status)
        {
            switch (status)
            {
                case RouletteSettlementStatus.InsufficientCredits: return RouletteBetSlip.InsufficientCreditsMessage;
                case RouletteSettlementStatus.LimitExceeded: return "TABLE LIMIT EXCEEDED";
                case RouletteSettlementStatus.Busy: return "PLEASE WAIT";
                case RouletteSettlementStatus.SpendFailed: return "COULD NOT TAKE THE BET - TRY AGAIN";
                case RouletteSettlementStatus.NoBets: return "PLACE A BET FIRST";
                default: return "ROUND NOT STARTED - NOTHING WAS CHARGED";
            }
        }

        private static bool BackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Escape);
#else
            return false;
#endif
        }
    }
}
