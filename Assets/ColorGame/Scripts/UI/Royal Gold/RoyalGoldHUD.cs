using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.Gameplay;
using ColorGame.History;

namespace ColorGame.UI.RoyalGold
{
    /// <summary>
    /// The "COLOR GAME" Royal Gold HUD. DISPLAY ONLY.
    /// It listens to the existing systems (ColorGameManager, BetSlip, wallet, AutoRollController) and shows:
    /// round number, countdown, result colors, matched count, winnings, bet summary, and a gold glow on bet colors.
    /// It never changes credits, results, payouts or timing. The balance number is shown by the existing WalletUI
    /// component (added to the new BalancePanel by the installer).
    /// </summary>
    public class RoyalGoldHUD : MonoBehaviour
    {
        private const string Peso = "\u20B1";

        [Header("Game systems (existing)")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private BetSlip betSlip;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private GameHistoryManager historyManager;
        [Tooltip("Optional. Needed for the countdown number.")]
        [SerializeField] private AutoRollController autoRoll;

        [Header("Top area")]
        [SerializeField] private TMP_Text roundText;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private Button menuButton;
        [Tooltip("The OLD menu button. The new MENU button simply presses it, so PlayMenuUI keeps working.")]
        [SerializeField] private Button legacyMenuButton;

        [Header("Result area")]
        [SerializeField] private TMP_Text resultTitleText;
        [SerializeField] private TMP_Text matchedCountText;
        [SerializeField] private TMP_Text winningAmountText;
        [SerializeField] private CanvasGroup summaryGroup;
        [Tooltip("Exactly 3, left to right.")]
        [SerializeField] private RoyalResultSlotView[] slots;

        [Header("Bet summary")]
        [SerializeField] private TMP_Text bettingInfoText;

        [Header("Gold glow on bet colors")]
        [Tooltip("The existing ColorSelectionPanel (its buttons get a pulsing gold outline while they hold bets).")]
        [SerializeField] private Transform colorButtonsRoot;
        [SerializeField] private Color glowColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private float glowDistance = 7f;
        [SerializeField] private float glowPulseSpeed = 5f;

        [Header("Countdown")]
        [SerializeField] private int lowTimeSeconds = 5;
        [SerializeField] private float pulseAmount = 0.18f;
        [SerializeField] private float pulseSpeed = 8f;
        [SerializeField] private Color countdownColor = Color.white;
        [SerializeField] private Color lowTimeColor = new Color(1f, 0.35f, 0.3f, 1f);

        [Header("Result look")]
        [SerializeField] private float revealStagger = 0.12f;
        [SerializeField] private Color neutralColor = Color.white;
        [SerializeField] private Color winColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private Color loseColor = new Color(1f, 0.45f, 0.45f, 1f);
        [SerializeField] private Color noBetColor = new Color(0.82f, 0.78f, 0.9f, 1f);

        private readonly Dictionary<GameColor, Outline> glows = new Dictionary<GameColor, Outline>();
        private GameColor[] lastResults;
        private Coroutine revealRoutine;
        private Coroutine popRoutine;
        private Coroutine summaryRoutine;
        private long shownRound = long.MinValue;
        private int shownSeconds = -2;
        private bool anyGlow;
        private bool isReady;

        private void Awake()
        {
            bool missing = config == null || gameManager == null || betSlip == null || walletManager == null ||
                           historyManager == null || roundText == null || countdownText == null ||
                           resultTitleText == null || matchedCountText == null || winningAmountText == null ||
                           bettingInfoText == null || slots == null || slots.Length != ColorGameConfig.ResultSlotCount;

            if (!missing)
            {
                foreach (RoyalResultSlotView slot in slots)
                {
                    if (slot == null) missing = true;
                }
            }

            if (missing)
            {
                Debug.LogError("[RoyalGoldHUD] A reference is missing in the Inspector (Slots needs exactly 3 entries).", this);
                return;
            }

            foreach (RoyalResultSlotView slot in slots) slot.Init();

            if (menuButton != null) menuButton.onClick.AddListener(HandleMenuClicked);
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnBettingWindowOpened += HandleBettingWindowOpened;
            gameManager.OnRoundSkipped += HandleRoundSkipped;
            gameManager.OnRoundStarted += HandleRoundStarted;
            gameManager.OnRoundVoided += HandleRoundVoided;
            gameManager.OnResultsShown += HandleResultsShown;
            gameManager.OnNoBetResultShown += HandleNoBetResultShown;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            gameManager.OnStateChanged += HandleStateChanged;
            betSlip.OnTotalChanged += HandleTotalChanged;
            betSlip.OnChipPlaced += HandleChipPlaced;
            betSlip.OnSlipCleared += HandleSlipCleared;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnBettingWindowOpened -= HandleBettingWindowOpened;
                gameManager.OnRoundSkipped -= HandleRoundSkipped;
                gameManager.OnRoundStarted -= HandleRoundStarted;
                gameManager.OnRoundVoided -= HandleRoundVoided;
                gameManager.OnResultsShown -= HandleResultsShown;
                gameManager.OnNoBetResultShown -= HandleNoBetResultShown;
                gameManager.OnRoundCompleted -= HandleRoundCompleted;
                gameManager.OnStateChanged -= HandleStateChanged;
            }
            if (betSlip != null)
            {
                betSlip.OnTotalChanged -= HandleTotalChanged;
                betSlip.OnChipPlaced -= HandleChipPlaced;
                betSlip.OnSlipCleared -= HandleSlipCleared;
            }
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;

            revealRoutine = null;
            popRoutine = null;
            summaryRoutine = null;
        }

        private void OnDestroy()
        {
            if (menuButton != null) menuButton.onClick.RemoveListener(HandleMenuClicked);
        }

        private void Start()
        {
            if (!isReady) return;

            BuildGlows();
            ShowIdle();
            RefreshBettingInfo();
            RefreshGlows();
        }

        private void Update()
        {
            if (!isReady) return;

            UpdateRoundNumber();
            UpdateCountdown();
            UpdateGlows();
        }

        // ---------- Menu ----------

        private void HandleMenuClicked()
        {
            if (legacyMenuButton == null)
            {
                Debug.LogWarning("[RoyalGoldHUD] No Legacy Menu Button assigned, so MENU does nothing.", this);
                return;
            }
            legacyMenuButton.onClick.Invoke();
        }

        // ---------- Round number and countdown ----------

        // Wagered rounds reserve their ID when they start, so before that the NEXT number is shown.
        private void UpdateRoundNumber()
        {
            long number = historyManager.LastRoundNumber + (gameManager.HasActiveWager ? 0 : 1);
            if (number == shownRound) return;

            shownRound = number;
            roundText.text = "ROUND #" + number.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateCountdown()
        {
            bool open = autoRoll != null && autoRoll.IsBettingOpen;

            if (!open)
            {
                if (shownSeconds != -1)
                {
                    shownSeconds = -1;
                    countdownText.text = "--";
                    countdownText.color = countdownColor;
                    countdownText.rectTransform.localScale = Vector3.one;
                }
                return;
            }

            int seconds = Mathf.Max(0, Mathf.CeilToInt(autoRoll.RemainingSeconds));
            if (seconds != shownSeconds)
            {
                shownSeconds = seconds;
                countdownText.text = seconds.ToString(CultureInfo.InvariantCulture);
            }

            bool low = seconds <= lowTimeSeconds;
            countdownText.color = low ? lowTimeColor : countdownColor;
            float scale = low ? 1f + pulseAmount * (0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed)) : 1f;
            countdownText.rectTransform.localScale = Vector3.one * scale;
        }

        // ---------- Game events ----------

        private void HandleBettingWindowOpened()
        {
            ShowIdle();
            RefreshBettingInfo();
        }

        private void HandleRoundSkipped()
        {
            StopAnimations();
            lastResults = null;
            SetTitle("NO BET", noBetColor);
            matchedCountText.text = "ROUND CONTINUES";
            winningAmountText.text = string.Empty;
            SetSummaryAlpha(1f);
            foreach (RoyalResultSlotView slot in slots) slot.ShowUnknown();
        }

        private void HandleRoundStarted(string roundId)
        {
            StopAnimations();
            lastResults = null;
            SetTitle("ROLLING...", neutralColor);
            matchedCountText.text = roundId;
            winningAmountText.text = string.Empty;
            SetSummaryAlpha(1f);
            foreach (RoyalResultSlotView slot in slots) slot.ShowUnknown();
            RefreshBettingInfo();
        }

        private void HandleRoundVoided(string roundId)
        {
            StopAnimations();
            lastResults = null;
            SetTitle("ROLL CANCELLED", neutralColor);
            matchedCountText.text = "BET REFUNDED";
            winningAmountText.text = string.Empty;
            SetSummaryAlpha(1f);
            foreach (RoyalResultSlotView slot in slots) slot.ShowUnknown();
        }

        // The real cube colors are known. Reveal them left to right, then highlight the matches.
        private void HandleResultsShown(GameColor[] results, PayoutResult payout)
        {
            if (results == null) return;

            StopReveal();
            lastResults = (GameColor[])results.Clone();
            matchedCountText.text = "MATCHED: " + payout.Matches.ToString(CultureInfo.InvariantCulture);
            revealRoutine = StartCoroutine(RevealRoutine(lastResults, payout));
        }

        // A round without a wager: show the colors only (no highlights, no win or loss).
        private void HandleNoBetResultShown(GameColor[] results)
        {
            if (results == null) return;

            StopReveal();
            lastResults = (GameColor[])results.Clone();
            revealRoutine = StartCoroutine(RevealRoutine(lastResults, null));
        }

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            // Make sure the final state is on screen even if the reveal was still running.
            if (revealRoutine != null)
            {
                StopReveal();
                if (lastResults != null)
                {
                    ShowAllSlots(lastResults, false);
                    ApplyHighlights(payout);
                }
            }

            if (payout.IsWin)
            {
                SetTitle("WIN!", winColor);
            }
            else
            {
                SetTitle("LOSE", loseColor);
            }

            matchedCountText.text = "MATCHED: " + payout.Matches.ToString(CultureInfo.InvariantCulture);
            winningAmountText.text = "WINNINGS: " + Peso + payout.Winnings.ToString("N0", CultureInfo.InvariantCulture);

            StopCoroutineSafe(ref popRoutine);
            StopCoroutineSafe(ref summaryRoutine);
            if (isActiveAndEnabled)
            {
                popRoutine = StartCoroutine(PopRoutine(resultTitleText.transform));
                if (summaryGroup != null) summaryRoutine = StartCoroutine(FadeSummaryRoutine());
            }

            RefreshBettingInfo();
        }

        private void HandleStateChanged(GameState state)
        {
            RefreshBettingInfo();
            if (menuButton != null && legacyMenuButton != null) menuButton.interactable = legacyMenuButton.interactable;
        }

        private void HandleTotalChanged(int total) { RefreshBettingInfo(); }
        private void HandleBalanceChanged(long balance) { RefreshBettingInfo(); }
        private void HandleChipPlaced(GameColor color, int amount, int total) { RefreshGlows(); }
        private void HandleSlipCleared() { RefreshGlows(); }

        // ---------- Result display ----------

        private void ShowIdle()
        {
            StopAnimations();
            lastResults = null;
            SetTitle("PLACE YOUR BETS", neutralColor);
            matchedCountText.text = string.Empty;
            winningAmountText.text = string.Empty;
            SetSummaryAlpha(1f);
            foreach (RoyalResultSlotView slot in slots) slot.ShowUnknown();
        }

        private void SetTitle(string text, Color color)
        {
            resultTitleText.text = text;
            resultTitleText.color = color;
            resultTitleText.transform.localScale = Vector3.one;
        }

        private void ShowAllSlots(GameColor[] results, bool pop)
        {
            for (int i = 0; i < slots.Length && i < results.Length; i++)
            {
                if (config.TryGetColorDefinition(results[i], out ColorDefinition definition))
                {
                    slots[i].ShowColor(definition, pop);
                }
            }
        }

        // The matches were decided by PayoutCalculator. This only shows them.
        private void ApplyHighlights(PayoutResult payout)
        {
            if (payout == null) return;

            for (int i = 0; i < slots.Length && i < payout.SlotMatches.Count; i++)
            {
                bool matched = payout.SlotMatches[i];
                slots[i].SetHighlighted(matched);
                slots[i].SetDimmed(payout.IsWin && !matched);
            }
        }

        private IEnumerator RevealRoutine(GameColor[] results, PayoutResult payout)
        {
            for (int i = 0; i < slots.Length && i < results.Length; i++)
            {
                slots[i].ClearHighlight();
                if (config.TryGetColorDefinition(results[i], out ColorDefinition definition))
                {
                    slots[i].ShowColor(definition, true);
                }
                yield return new WaitForSeconds(Mathf.Max(0f, revealStagger));
            }

            ApplyHighlights(payout);
            revealRoutine = null;
        }

        // Title grows past full size, then settles.
        private IEnumerator PopRoutine(Transform target)
        {
            const float duration = 0.4f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float scale = t < 0.6f
                    ? Mathf.Lerp(0.5f, 1.25f, t / 0.6f)
                    : Mathf.Lerp(1.25f, 1f, (t - 0.6f) / 0.4f);
                target.localScale = Vector3.one * scale;
                yield return null;
            }

            target.localScale = Vector3.one;
            popRoutine = null;
        }

        private IEnumerator FadeSummaryRoutine()
        {
            const float duration = 0.35f;
            float elapsed = 0f;
            summaryGroup.alpha = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                summaryGroup.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }

            summaryGroup.alpha = 1f;
            summaryRoutine = null;
        }

        private void SetSummaryAlpha(float alpha)
        {
            if (summaryGroup != null) summaryGroup.alpha = alpha;
        }

        private void StopReveal()
        {
            StopCoroutineSafe(ref revealRoutine);
        }

        private void StopAnimations()
        {
            StopCoroutineSafe(ref revealRoutine);
            StopCoroutineSafe(ref popRoutine);
            StopCoroutineSafe(ref summaryRoutine);
        }

        private void StopCoroutineSafe(ref Coroutine routine)
        {
            if (routine == null) return;
            StopCoroutine(routine);
            routine = null;
        }

        // ---------- Bet summary ----------

        private void RefreshBettingInfo()
        {
            int colorsWithBets = 0;
            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition != null && betSlip.GetTotal(definition.color) > 0) colorsWithBets++;
            }

            long total = betSlip.TotalBet;
            long balance = walletManager.GetBalance();

            // While a round runs the bet is already deducted, so it must not be subtracted twice.
            long left = gameManager.State == GameState.Idle ? balance - total : balance;
            if (left < 0) left = 0;

            string head = colorsWithBets == 0
                ? "NO BETS YET"
                : "BETS ON " + colorsWithBets.ToString(CultureInfo.InvariantCulture) + (colorsWithBets == 1 ? " COLOR" : " COLORS");

            bettingInfoText.text = head +
                                   "   |   TOTAL: " + Peso + total.ToString("N0", CultureInfo.InvariantCulture) +
                                   "   |   LEFT: " + Peso + left.ToString("N0", CultureInfo.InvariantCulture);
        }

        // ---------- Gold glow on colors that hold bets ----------

        private void BuildGlows()
        {
            if (colorButtonsRoot == null) return;

            foreach (ColorButtonView view in colorButtonsRoot.GetComponentsInChildren<ColorButtonView>(true))
            {
                Graphic graphic = view.GetComponent<Graphic>();
                if (graphic == null) graphic = view.GetComponentInChildren<Graphic>(true);
                if (graphic == null || glows.ContainsKey(view.AssignedColor)) continue;

                Outline outline = graphic.gameObject.AddComponent<Outline>();
                outline.effectDistance = new Vector2(glowDistance, -glowDistance);
                outline.effectColor = glowColor;
                outline.useGraphicAlpha = false;
                outline.enabled = false;
                glows[view.AssignedColor] = outline;
            }
        }

        private void RefreshGlows()
        {
            anyGlow = false;
            foreach (KeyValuePair<GameColor, Outline> pair in glows)
            {
                bool on = betSlip.GetTotal(pair.Key) > 0;
                if (pair.Value != null) pair.Value.enabled = on;
                anyGlow |= on;
            }
        }

        private void UpdateGlows()
        {
            if (!anyGlow) return;

            float k = 0.55f + 0.45f * (0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * glowPulseSpeed));
            Color c = glowColor;
            c.a = glowColor.a * k;

            foreach (KeyValuePair<GameColor, Outline> pair in glows)
            {
                if (pair.Value != null && pair.Value.enabled) pair.Value.effectColor = c;
            }
        }
    }
}
