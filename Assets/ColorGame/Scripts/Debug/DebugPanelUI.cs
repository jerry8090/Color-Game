using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.Gameplay;
using ColorGame.History;

namespace ColorGame.DevTools
{
    /// <summary>
    /// DEVELOPMENT / TEST ONLY panel. Shows live game information and offers test buttons.
    /// It never edits numbers itself: it calls the wallet and history managers,
    /// so every action creates a normal ledger entry and is saved like any other change.
    /// The panel is hidden with a CanvasGroup so this script stays active and can open it.
    /// </summary>
    public class DebugPanelUI : MonoBehaviour
    {
        // The button labels in the scene say ADD 100 / ADD 1000. Change both if you change these.
        private const int SmallCreditAmount = 100;
        private const int LargeCreditAmount = 1000;
        private const string NoneYet = "NONE YET";

        [Header("References")]
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private GameHistoryManager historyManager;

        [Header("Panel")]
        [SerializeField] private CanvasGroup debugPanelGroup;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text statusText;

        [Header("Dev Buttons")]
        [SerializeField] private Button add100Button;
        [SerializeField] private Button add1000Button;
        [SerializeField] private Button resetBalanceButton;
        [SerializeField] private Button clearHistoryButton;

        [Header("Look")]
        [SerializeField] private Color labelColor = new Color(0.72f, 0.66f, 0.82f, 1f);
        [SerializeField] private Color gainColor = new Color(0.49f, 0.89f, 0.55f, 1f);
        [SerializeField] private Color lossColor = new Color(1f, 0.48f, 0.48f, 1f);
        [Tooltip("Where the value column starts, as a percent of the text width.")]
        [SerializeField, Range(20f, 70f)] private float valueColumnPercent = 42f;

        /// <summary>Raised with true when the panel opens and false when it closes (ColorGameAudio listens).</summary>
        public event Action<bool> OnPanelToggled;

        private bool isReady;

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            if (walletManager == null || betManager == null || gameManager == null || historyManager == null ||
                debugPanelGroup == null || openButton == null || closeButton == null ||
                infoText == null || statusText == null ||
                add100Button == null || add1000Button == null ||
                resetBalanceButton == null || clearHistoryButton == null)
            {
                Debug.LogError("[DebugPanelUI] A reference is missing in the Inspector.", this);
                return;
            }

            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(Open);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);

            add100Button.onClick.RemoveAllListeners();
            add100Button.onClick.AddListener(() => AddCredits(SmallCreditAmount));
            add1000Button.onClick.RemoveAllListeners();
            add1000Button.onClick.AddListener(() => AddCredits(LargeCreditAmount));
            resetBalanceButton.onClick.RemoveAllListeners();
            resetBalanceButton.onClick.AddListener(ResetBalance);
            clearHistoryButton.onClick.RemoveAllListeners();
            clearHistoryButton.onClick.AddListener(ClearHistory);

            SetPanelVisible(false);
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            walletManager.OnBalanceChanged += HandleBalanceChanged;
            betManager.OnBetChanged += HandleBetChanged;
            gameManager.OnStateChanged += HandleStateChanged;
            gameManager.OnSelectedColorChanged += HandleColorChanged;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            historyManager.OnHistoryChanged += HandleHistoryChanged;

            UpdateButtons();
        }

        private void OnDisable()
        {
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
            if (betManager != null) betManager.OnBetChanged -= HandleBetChanged;
            if (gameManager != null)
            {
                gameManager.OnStateChanged -= HandleStateChanged;
                gameManager.OnSelectedColorChanged -= HandleColorChanged;
                gameManager.OnRoundCompleted -= HandleRoundCompleted;
            }
            if (historyManager != null) historyManager.OnHistoryChanged -= HandleHistoryChanged;
        }

        // ---------- Open / close ----------

        public void Open()
        {
            if (!isReady || IsOpen) return;
            if (gameManager.State != GameState.Idle) return; // do not cover a running round

            IsOpen = true;
            statusText.text = "READY";
            Refresh();
            SetPanelVisible(true);
            OnPanelToggled?.Invoke(true);
        }

        public void Close()
        {
            if (!isReady || !IsOpen) return;

            IsOpen = false;
            SetPanelVisible(false);
            OnPanelToggled?.Invoke(false);
        }

        private void SetPanelVisible(bool visible)
        {
            debugPanelGroup.alpha = visible ? 1f : 0f;
            debugPanelGroup.interactable = visible;
            debugPanelGroup.blocksRaycasts = visible;
        }

        // ---------- Dev actions ----------

        private void AddCredits(int amount)
        {
            if (gameManager.State != GameState.Idle) return;

            bool ok = walletManager.AddCredits(amount, TransactionType.AdminTest);
            statusText.text = ok
                ? $"ADDED {amount.ToString("N0", CultureInfo.InvariantCulture)} CREDITS (ADMIN_TEST)"
                : "COULD NOT ADD CREDITS. SEE CONSOLE.";
        }

        private void ResetBalance()
        {
            if (gameManager.State != GameState.Idle) return;

            walletManager.ResetBalance();
            statusText.text = "BALANCE RESET TO " + Format(walletManager.StartingBalance);
        }

        private void ClearHistory()
        {
            if (gameManager.State != GameState.Idle) return;

            historyManager.ClearHistory();
            statusText.text = "HISTORY AND STATISTICS CLEARED";
        }

        // ---------- Event handlers ----------

        private void HandleBalanceChanged(long balance) { RefreshIfOpen(); }
        private void HandleBetChanged(int bet) { RefreshIfOpen(); }
        private void HandleColorChanged(bool hasColor, GameColor color) { RefreshIfOpen(); }
        private void HandleRoundCompleted(GameRound round, PayoutResult payout) { RefreshIfOpen(); }
        private void HandleHistoryChanged() { RefreshIfOpen(); }

        private void HandleStateChanged(GameState state)
        {
            UpdateButtons();
            RefreshIfOpen();
        }

        private void UpdateButtons()
        {
            bool idle = gameManager.State == GameState.Idle;

            openButton.interactable = idle;
            add100Button.interactable = idle;
            add1000Button.interactable = idle;
            resetBalanceButton.interactable = idle;
            clearHistoryButton.interactable = idle;
        }

        private void RefreshIfOpen()
        {
            if (IsOpen) Refresh();
        }

        // ---------- Drawing ----------

        private void Refresh()
        {
            GameStatistics stats = historyManager.Statistics;
            StringBuilder sb = new StringBuilder();

            AppendLine(sb, "BALANCE", Format(walletManager.GetBalance()));
            AppendLine(sb, "SELECTED COLOR", gameManager.HasSelectedColor
                ? gameManager.SelectedColor.ToString().ToUpperInvariant()
                : "NONE");
            AppendLine(sb, "SELECTED BET", betManager.HasBet ? Format(betManager.SelectedBet) : "NONE");
            AppendLine(sb, "GAME STATE", gameManager.State.ToString().ToUpperInvariant());
            AppendLine(sb, "LAST RESULT", LastResultText());
            AppendLine(sb, "LAST PAYOUT", LastPayoutText());
            AppendLine(sb, "TOTAL ROUNDS", stats != null ? Format(stats.totalRounds) : "0");
            AppendLine(sb, "TOTAL WINS", stats != null ? Format(stats.wins) : "0");
            AppendLine(sb, "TOTAL LOSSES", stats != null ? Format(stats.losses) : "0");

            infoText.text = sb.ToString();
        }

        private string LastResultText()
        {
            GameRound round = gameManager.LastRound;
            if (round == null) return NoneYet;

            return $"{round.result1} / {round.result2} / {round.result3}".ToUpperInvariant();
        }

        private string LastPayoutText()
        {
            PayoutResult payout = gameManager.LastPayout;
            if (payout == null) return NoneYet;

            long net = payout.NetResult;
            Color netColor = net > 0 ? gainColor : (net < 0 ? lossColor : Color.white);
            string netString = net.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);

            return $"WON {Format(payout.Winnings)} (NET <color=#{ColorUtility.ToHtmlStringRGB(netColor)}>{netString}</color>)";
        }

        // "<pos=..%>" jumps to a fixed column, so the values line up neatly.
        private void AppendLine(StringBuilder sb, string label, string value)
        {
            string labelHex = ColorUtility.ToHtmlStringRGB(labelColor);
            string column = valueColumnPercent.ToString("0.#", CultureInfo.InvariantCulture);
            sb.Append($"<color=#{labelHex}>{label}</color><pos={column}%>{value}\n");
        }

        private static string Format(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}