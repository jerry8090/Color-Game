using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using RouletteGame.Core;
using RouletteGame.Integration;

namespace RouletteGame.UI
{
    /// <summary>
    /// The Roulette history screen: statistics on top, a scrollable list of rounds below (newest first).
    /// Display only. It reads from RouletteHistoryManager and never changes game data.
    /// Rows are created by code, so no row prefab is needed. Separate from the Color Game and Scatter histories.
    /// The panel is hidden with a CanvasGroup so this script stays active and can open it.
    /// </summary>
    public class RouletteHistoryUI : MonoBehaviour, IBackNavigable
    {
        private const int MaxBetsShown = 3;

        [Header("References")]
        [SerializeField] private RouletteHistoryManager historyManager;
        [SerializeField] private CanvasGroup panelGroup;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform rowContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject emptyMessage;
        [SerializeField] private TMP_Text statsText;

        [Header("Look")]
        [SerializeField] private Sprite rowSprite;
        [SerializeField] private float rowHeight = 190f;
        [SerializeField] private Color winRowColor = new Color(0.17f, 0.33f, 0.22f, 1f);
        [SerializeField] private Color loseRowColor = new Color(0.33f, 0.18f, 0.2f, 1f);
        [SerializeField] private Color evenRowColor = new Color(0.22f, 0.2f, 0.32f, 1f);
        [SerializeField] private Color gainColor = new Color(0.49f, 0.89f, 0.55f, 1f);
        [SerializeField] private Color lossColor = new Color(1f, 0.48f, 0.48f, 1f);

        /// <summary>Raised with true when the panel opens and false when it closes.</summary>
        public event Action<bool> OnPanelToggled;

        private sealed class Row
        {
            public GameObject root;
            public Image background;
            public TMP_Text text;
        }

        private readonly List<Row> rows = new List<Row>();
        private bool isReady;

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            if (historyManager == null || panelGroup == null || openButton == null || closeButton == null ||
                rowContainer == null || scrollRect == null || emptyMessage == null || statsText == null)
            {
                Debug.LogError("[RouletteHistoryUI] A reference is missing in the Inspector.", this);
                return;
            }

            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(Open);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);

            SetPanelVisible(false);
            isReady = true;
        }

        private void OnEnable()
        {
            if (isReady) historyManager.OnHistoryChanged += HandleHistoryChanged;
        }

        private void OnDisable()
        {
            if (historyManager != null) historyManager.OnHistoryChanged -= HandleHistoryChanged;
        }

        public void Open()
        {
            if (!isReady || IsOpen) return;

            IsOpen = true;
            Refresh();
            SetPanelVisible(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rowContainer);
            scrollRect.verticalNormalizedPosition = 1f; // newest round first
            OnPanelToggled?.Invoke(true);
        }

        public void Close()
        {
            if (!isReady || !IsOpen) return;

            IsOpen = false;
            SetPanelVisible(false);
            OnPanelToggled?.Invoke(false);
        }

        private void HandleHistoryChanged()
        {
            if (IsOpen) Refresh();
        }

        private void SetPanelVisible(bool visible)
        {
            panelGroup.alpha = visible ? 1f : 0f;
            panelGroup.interactable = visible;
            panelGroup.blocksRaycasts = visible;
        }

        private void Refresh()
        {
            RefreshStatistics();

            IReadOnlyList<RouletteRoundRecord> records = historyManager.Rounds;
            int count = records.Count;

            while (rows.Count < count) rows.Add(CreateRow());

            for (int i = 0; i < rows.Count; i++)
            {
                if (i < count)
                {
                    rows[i].root.SetActive(true);
                    Bind(rows[i], records[count - 1 - i]); // newest first
                }
                else
                {
                    rows[i].root.SetActive(false);
                }
            }

            emptyMessage.SetActive(count == 0);
        }

        private void RefreshStatistics()
        {
            RouletteStatistics s = historyManager.Statistics;
            if (s == null) return;

            statsText.text =
                "ROUNDS " + N(s.totalRounds) + "    WINNING ROUNDS " + N(s.winningRounds) + "\n" +
                "WAGERED " + N(s.totalWagered) + "    RETURNED " + N(s.totalReturned) + "    NET " + Signed(s.NetCredits) + "\n" +
                "LARGEST WIN " + N(s.largestWin) + "    ZEROS " + N(s.zeroCount);
        }

        private void Bind(Row row, RouletteRoundRecord r)
        {
            string time = string.Empty;
            if (DateTime.TryParse(r.timestampUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime utc))
            {
                time = utc.ToLocalTime().ToString("MMM dd HH:mm", CultureInfo.InvariantCulture);
            }

            PocketColor pocket = RouletteLayout.IsValidNumber(r.winningNumber) ? RouletteLayout.ColorOf(r.winningNumber) : PocketColor.Black;
            Color numberColor = Color.Lerp(RouletteUi.ColorFor(pocket), Color.white, 0.45f); // lightened so it stays readable

            StringBuilder bets = new StringBuilder();
            int shown = Math.Min(r.bets.Count, MaxBetsShown);
            for (int i = 0; i < shown; i++)
            {
                RouletteBetRecord b = r.bets[i];
                if (i > 0) bets.Append("  |  ");
                bets.Append(b.label).Append(' ').Append(b.won ? Plain(b.netWinnings) : "-" + Plain(b.amount));
            }
            if (r.bets.Count > shown) bets.Append("  |  +").Append(r.bets.Count - shown).Append(" MORE");

            row.text.text =
                "<b>" + r.roundId + "</b>   <size=70%>" + time + "</size>\n" +
                "<color=#" + ColorUtility.ToHtmlStringRGB(numberColor) + "><b>" + r.winningNumber + " " + RouletteText.ColorName(pocket) + "</b></color>" +
                "   BET " + N(r.totalBet) + "   RETURN " + N(r.totalReturned) + "   NET " + Signed(r.net) + "\n" +
                "<size=75%>" + bets + "</size>";

            row.background.color = r.net > 0 ? winRowColor : (r.net < 0 ? loseRowColor : evenRowColor);
        }

        private Row CreateRow()
        {
            GameObject root = new GameObject("Row" + (rows.Count + 1), typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            root.transform.SetParent(rowContainer, false);

            Image background = root.GetComponent<Image>();
            background.sprite = rowSprite;
            background.type = rowSprite != null ? Image.Type.Sliced : Image.Type.Simple;
            background.raycastTarget = false;

            LayoutElement layout = root.GetComponent<LayoutElement>();
            layout.minHeight = rowHeight;
            layout.preferredHeight = rowHeight;

            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(root.transform, false);
            RectTransform rect = (RectTransform)textObject.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(20f, 8f);
            rect.offsetMax = new Vector2(-20f, -8f);

            TextMeshProUGUI tmp = textObject.GetComponent<TextMeshProUGUI>();
            tmp.fontSize = 32f;
            tmp.enableAutoSizing = true;
            tmp.fontSizeMin = 14f;
            tmp.fontSizeMax = 32f;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.color = Color.white;
            tmp.raycastTarget = false;

            return new Row { root = root, background = background, text = tmp };
        }

        private string Signed(long value)
        {
            Color color = value > 0 ? gainColor : (value < 0 ? lossColor : Color.white);
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + RouletteText.Signed(value) + "</color>";
        }

        private static string Plain(long value)
        {
            return RouletteText.Credits(value);
        }

        private static string N(long value)
        {
            return RouletteText.Credits(value);
        }
    }
}
