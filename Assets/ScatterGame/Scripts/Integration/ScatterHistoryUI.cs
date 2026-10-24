using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// The Scatter history screen: statistics on top, a scrollable list of rounds below (newest first).
    /// Display only. It reads from ScatterHistoryManager and never changes game data.
    /// Rows are created by code, so no row prefab is needed. Separate from the Color Game history.
    /// </summary>
    public class ScatterHistoryUI : MonoBehaviour, IBackNavigable
    {
        [Header("References")]
        [SerializeField] private ScatterHistoryManager historyManager;
        [SerializeField] private CanvasGroup panelGroup;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform rowContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject emptyMessage;
        [SerializeField] private TMP_Text statsText;

        [Header("Look")]
        [SerializeField] private Sprite rowSprite;
        [SerializeField] private float rowHeight = 170f;
        [SerializeField] private Color winRowColor = new Color(0.17f, 0.33f, 0.22f, 1f);
        [SerializeField] private Color loseRowColor = new Color(0.33f, 0.18f, 0.2f, 1f);
        [SerializeField] private Color bonusRowColor = new Color(0.30f, 0.17f, 0.40f, 1f);
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
                Debug.LogError("[ScatterHistoryUI] A reference is missing in the Inspector.", this);
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

        public void SetOpenEnabled(bool enabledState)
        {
            if (isReady) openButton.interactable = enabledState;
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

            IReadOnlyList<ScatterRoundRecord> records = historyManager.Rounds;
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
            ScatterStatistics s = historyManager.Statistics;
            if (s == null) return;

            statsText.text =
                "ROUNDS " + N(s.totalRounds) + "    WINS " + N(s.wins) + "\n" +
                "BET " + N(s.totalBet) + "    WON " + N(s.totalWon) + "    NET " + Signed(s.NetCredits) + "\n" +
                "FREE SPINS " + N(s.totalFreeSpinsPlayed) + "    LARGEST WIN " + N(s.largestWin);
        }

        private void Bind(Row row, ScatterRoundRecord r)
        {
            string time = string.Empty;
            if (DateTime.TryParse(r.timestampUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime utc))
            {
                time = utc.ToLocalTime().ToString("MMM dd HH:mm", CultureInfo.InvariantCulture);
            }

            string bonus = r.bonusTriggered ? "   <color=#E1BEE7>BONUS</color>" : string.Empty;

            row.text.text =
                "<b>" + r.roundId + "</b>   <size=70%>" + time + "</size>" + bonus + "\n" +
                "BET " + N(r.bet) + "   WIN " + N(r.winCredits) + "   NET " + Signed(r.netCredits) + "\n" +
                "<size=75%>MAX x" + r.maxMultiplier + "   CASCADES " + r.cascades + "   FREE SPINS " + r.freeSpinsPlayed + "</size>";

            row.background.color = r.bonusTriggered ? bonusRowColor : (r.winCredits > 0 ? winRowColor : loseRowColor);
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
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" +
                   value.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture) + "</color>";
        }

        private static string N(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}