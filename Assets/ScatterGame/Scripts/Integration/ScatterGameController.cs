using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Shared.Audio;
using Shared.Save;
using Shared.Wallet;
using ScatterGame.Core;
using ScatterGame.Runtime;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ScatterGame.Integration
{
    public enum ScatterUiState
    {
        Idle,
        Spinning,
        Cascading,
        FreeSpins,
        Settling
    }

    /// <summary>
    /// Runs the play screen. The wallet is already settled by ScatterRoundService before any animation starts,
    /// so this script only PLAYS BACK the finished result: spin, cascades, free spins, banners and counters.
    /// Closing the app or leaving the scene mid-animation can never lose or duplicate credits.
    /// </summary>
    public class ScatterGameController : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private ScatterRoundService roundService;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private AudioManager audioManager;

        [Header("Views")]
        [SerializeField] private SlotGridView grid;
        [SerializeField] private ScatterHUD hud;
        [SerializeField] private ScatterHistoryUI historyUI;

        [Header("Buttons")]
        [SerializeField] private Button spinButton;
        [SerializeField] private Button betDownButton;
        [SerializeField] private Button betUpButton;
        [SerializeField] private Button autoButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private TMP_Text spinLabel;
        [SerializeField] private TMP_Text autoLabel;

        [Header("Tuning")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [Tooltip("Animation speed after SPIN is pressed during a round.")]
        [SerializeField] private float fastSpeed = 4f;
        [Tooltip("A win of this many times the bet shows BIG WIN.")]
        [SerializeField] private int bigWinMultiple = 10;
        [Tooltip("A win of this many times the bet shows MEGA WIN.")]
        [SerializeField] private int megaWinMultiple = 25;
        [SerializeField] private float autoDelaySeconds = 0.9f;
        [SerializeField] private float bannerHold = 1.4f;

        public ScatterUiState State { get; private set; } = ScatterUiState.Idle;

        private ScatterGameSettings settings;
        private SlotConfig config;
        private int wildId;
        private int betIndex;
        private bool isReady;
        private bool isBusy;
        private bool isLeaving;
        private bool autoOn;
        private float autoTimer;
        private float speed = 1f;
        private long balanceBase;
        private long runningMilli;

        private int CurrentBet => settings.BetAmounts[betIndex];

        private void Awake()
        {
            if (roundService == null || walletManager == null || saveManager == null || grid == null || hud == null ||
                spinButton == null || betDownButton == null || betUpButton == null || autoButton == null || homeButton == null)
            {
                Debug.LogError("[ScatterGameController] A reference is missing in the Inspector.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (walletManager != null) walletManager.OnBalanceChanged += HandleBalanceChanged;
            if (grid != null) grid.OnReelStopped += HandleReelStopped;
        }

        private void OnDisable()
        {
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
            if (grid != null) grid.OnReelStopped -= HandleReelStopped;
        }

        private void Start()
        {
            // All Awake methods have run by now, so the round service is ready.
            if (!roundService.IsReady)
            {
                Debug.LogError("[ScatterGameController] ScatterRoundService is not ready. Check its settings asset and references.", this);
                enabled = false;
                return;
            }

            settings = roundService.Settings;
            config = settings.BuildSlotConfig();
            wildId = config.WildSymbolId;

            if (!grid.Init(settings))
            {
                enabled = false;
                return;
            }

            grid.ShowGrid(GridGenerator.Generate(new WeightedSymbolSource(config, new System.Random())));

            betIndex = settings.DefaultBetIndex;
            hud.SetBet(CurrentBet);
            hud.SetBalance(walletManager.GetBalance(), true);
            hud.SetWin(0, true);
            hud.SetMultiplier(config.PaidMultiplierLadder[0], false);

            spinButton.onClick.RemoveAllListeners();
            spinButton.onClick.AddListener(OnSpinPressed);
            betDownButton.onClick.RemoveAllListeners();
            betDownButton.onClick.AddListener(() => ChangeBet(-1));
            betUpButton.onClick.RemoveAllListeners();
            betUpButton.onClick.AddListener(() => ChangeBet(1));
            autoButton.onClick.RemoveAllListeners();
            autoButton.onClick.AddListener(ToggleAuto);
            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(GoHome);

            SetAuto(false);
            isReady = true;
        }

        private void Update()
        {
            if (!isReady || isLeaving) return;

            bool canPlay = false;
            if (!isBusy)
            {
                canPlay = roundService.CanPlay(CurrentBet, out string reason);
                hud.SetStatus(canPlay ? string.Empty : reason);
            }

            spinButton.interactable = isBusy || canPlay;
            betDownButton.interactable = !isBusy && betIndex > 0;
            betUpButton.interactable = !isBusy && betIndex < settings.BetAmounts.Count - 1;
            if (historyUI != null) historyUI.SetOpenEnabled(!isBusy);
            if (spinLabel != null) spinLabel.text = isBusy ? (speed > 1f ? "..." : "SKIP") : "SPIN";

            if (autoOn && !isBusy && (historyUI == null || !historyUI.IsOpen))
            {
                autoTimer += Time.deltaTime;
                if (autoTimer >= autoDelaySeconds)
                {
                    autoTimer = 0f;
                    if (canPlay) StartRound();
                    else SetAuto(false); // cannot afford the next spin
                }
            }

            if (BackPressed())
            {
                if (historyUI != null && historyUI.IsOpen) historyUI.Close();
                else GoHome();
            }
        }

        // ---------- Buttons ----------

        private void OnSpinPressed()
        {
            if (!isReady) return;

            if (isBusy)
            {
                SetSpeed(fastSpeed); // fast-forward the running round
                return;
            }

            if (!roundService.CanPlay(CurrentBet, out _)) return;
            Click();
            StartRound();
        }

        private void ChangeBet(int delta)
        {
            if (isBusy) return;

            int next = Mathf.Clamp(betIndex + delta, 0, settings.BetAmounts.Count - 1);
            if (next == betIndex) return;

            betIndex = next;
            hud.SetBet(CurrentBet);
            Click();
        }

        private void ToggleAuto()
        {
            Click();
            SetAuto(!autoOn);
        }

        private void SetAuto(bool on)
        {
            autoOn = on;
            autoTimer = 0f;
            if (autoLabel != null) autoLabel.text = on ? "AUTO: ON" : "AUTO";
        }

        private void GoHome()
        {
            if (isLeaving) return;

            if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                Debug.LogError($"[ScatterGameController] Scene '{mainMenuSceneName}' is not in the build list (File > Build Profiles > Scene List).", this);
                return;
            }

            isLeaving = true;
            Click();
            saveManager.SaveGame(); // the wallet was settled before the animation, so leaving is always safe
            SceneManager.LoadScene(mainMenuSceneName);
        }

        // ---------- Round ----------

        private void StartRound()
        {
            if (isBusy) return;
            StartCoroutine(RoundRoutine());
        }

        private IEnumerator RoundRoutine()
        {
            isBusy = true;
            SetSpeed(1f);
            State = ScatterUiState.Spinning;
            if (historyUI != null && historyUI.IsOpen) historyUI.Close();

            int bet = CurrentBet;
            SettlementResult result;
            if (!roundService.TryPlayRound(bet, out result))
            {
                FinishRound(false);
                yield break;
            }

            RoundResult round = result.Round;
            balanceBase = result.BalanceBefore - result.Bet;
            runningMilli = 0;

            hud.SetBalance(balanceBase, false);
            hud.SetWin(0, true);
            hud.HideFreeSpins();

            yield return PlaySpin(round.PaidSpin, false, true);

            if (round.BonusTriggered) yield return PlayBonus(round);

            State = ScatterUiState.Settling;
            hud.SetWin(result.CreditsPaid, false);
            hud.SetBalance(walletManager.GetBalance(), false);

            if (result.CreditsPaid > 0)
            {
                PlaySfx(SfxType.Win);

                if (result.CreditsPaid >= (long)bet * megaWinMultiple)
                    yield return hud.ShowBanner("MEGA WIN\n" + result.CreditsPaid.ToString("N0"), bannerHold);
                else if (result.CreditsPaid >= (long)bet * bigWinMultiple)
                    yield return hud.ShowBanner("BIG WIN\n" + result.CreditsPaid.ToString("N0"), bannerHold);
            }

            FinishRound(round.BonusTriggered);
        }

        private void FinishRound(bool stopAuto)
        {
            isBusy = false;
            State = ScatterUiState.Idle;
            autoTimer = 0f;
            SetSpeed(1f);
            if (stopAuto) SetAuto(false);
        }

        // One spin: reels, then every cascade. Used for the paid spin and for every free spin.
        private IEnumerator PlaySpin(SpinResult spin, bool isFree, bool resetMultiplier)
        {
            if (resetMultiplier)
            {
                int[] ladder = isFree ? config.FreeSpinMultiplierLadder : config.PaidMultiplierLadder;
                hud.SetMultiplier(Mathf.Min(ladder[0], config.MaxMultiplier), false);
            }

            State = ScatterUiState.Spinning;
            PlaySfx(SfxType.Roll);
            yield return grid.SpinTo(spin.InitialGrid);

            if (spin.ScatterCount >= config.MinScattersForBonus)
            {
                PlaySfx(SfxType.Select);
                yield return grid.PulseCategory(SymbolCategory.Scatter);
            }

            if (spin.ScatterWinMilli > 0)
            {
                runningMilli += spin.ScatterWinMilli;
                UpdateWinHud();
            }

            State = isFree ? ScatterUiState.FreeSpins : ScatterUiState.Cascading;

            foreach (CascadeStep step in spin.Steps)
            {
                hud.SetMultiplier(step.Multiplier, true);

                HashSet<GridPosition> winning = new HashSet<GridPosition>();
                foreach (SymbolWin win in step.Wins)
                {
                    foreach (GridPosition position in win.Positions) winning.Add(position);
                }

                yield return grid.Highlight(winning);

                runningMilli += step.WinMilli;
                UpdateWinHud();
                PlaySfx(SfxType.SlotStop);

                yield return grid.Cascade(step, wildId);
            }

            if (spin.HitCascadeLimit) yield return hud.ShowBanner("CASCADE LIMIT", bannerHold * 0.5f);
        }

        // Mirrors RoundResolver: total free spins grow by the retrigger amount, up to the per-round cap.
        private IEnumerator PlayBonus(RoundResult round)
        {
            State = ScatterUiState.FreeSpins;
            int total = round.FreeSpinsAwardedInitial;
            yield return hud.ShowBanner("FREE SPINS\n" + total, bannerHold);

            for (int i = 0; i < round.FreeSpins.Count; i++)
            {
                SpinResult spin = round.FreeSpins[i];
                hud.SetFreeSpins(i + 1, total);

                bool resetMultiplier = !config.FreeSpinMultiplierPersists || i == 0;
                yield return PlaySpin(spin, true, resetMultiplier);

                if (spin.ScatterCount >= config.MinScattersForBonus)
                {
                    int extra = Mathf.Min(config.RetriggerFreeSpins, config.MaxTotalFreeSpinsPerRound - total);
                    if (extra > 0)
                    {
                        total += extra;
                        hud.SetFreeSpins(i + 1, total);
                        yield return hud.ShowBanner("+" + extra + " FREE SPINS", bannerHold * 0.7f);
                    }
                }
            }

            hud.HideFreeSpins();
            yield return hud.ShowBanner("BONUS COMPLETE", bannerHold * 0.7f);
        }

        // ---------- Helpers ----------

        private void UpdateWinHud()
        {
            long credits = runningMilli / 1000; // whole credits, rounded down like the settlement
            hud.SetWin(credits, false);
            hud.SetBalance(balanceBase + credits, false);
        }

        private void SetSpeed(float value)
        {
            speed = value;
            grid.Speed = value;
            hud.Speed = value;
        }

        private void HandleBalanceChanged(long balance)
        {
            // While a round is playing back, the counters follow the animation instead.
            if (!isBusy && isReady) hud.SetBalance(balance, false);
        }

        private void HandleReelStopped(int reel)
        {
            PlaySfx(SfxType.SlotStop);
        }

        private void Click()
        {
            PlaySfx(SfxType.ButtonClick);
        }

        private void PlaySfx(SfxType type)
        {
            if (audioManager != null) audioManager.PlaySfx(type);
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