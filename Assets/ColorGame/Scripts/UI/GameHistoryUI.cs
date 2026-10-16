using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using ColorGame.Core;
using ColorGame.Gameplay;
using ColorGame.History;

namespace ColorGame.UI
{
    /// <summary>
    /// The history screen: statistics on top, a scrollable list of rounds below (newest first).
    /// Display only. It reads from GameHistoryManager and never changes game data.
    /// The panel is hidden with a CanvasGroup so this script stays active and can open it.
    /// The Game Manager reference is OPTIONAL: in the Main Menu scene there is no game running,
    /// so the panel can open at any time. In a play scene it still waits for the Idle state.
    /// </summary>
    public class GameHistoryUI : MonoBehaviour, IBackNavigable
    {
        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private GameHistoryManager historyManager;
        [Tooltip("Optional. Leave empty in the Main Menu scene.")]
        [SerializeField] private ColorGameManager gameManager;

        [Header("Panel")]
        [SerializeField] private CanvasGroup panelGroup;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        [Header("History List")]
        [SerializeField] private HistoryRowView rowPrefab;
        [SerializeField] private RectTransform rowContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject emptyMessage;

        [Header("Statistics Texts")]
        [SerializeField] private TMP_Text roundsText;
        [SerializeField] private TMP_Text winsText;
        [SerializeField] private TMP_Text lossesText;
        [SerializeField] private TMP_Text creditsBetText;
        [SerializeField] private TMP_Text creditsWonText;
        [SerializeField] private TMP_Text netText;

        [Header("Look")]
        [SerializeField] private Color gainColor = new Color(0.49f, 0.89f, 0.55f, 1f);
        [SerializeField] private Color lossColor = new Color(1f, 0.48f, 0.48f, 1f);

        /// <summary>Raised with true when the panel opens and false when it closes (ColorGameAudio listens).</summary>
        public event Action<bool> OnPanelToggled;

        private readonly List<HistoryRowView> rows = new List<HistoryRowView>();
        private bool isReady;

        public bool IsOpen { get; private set; }

        private void Awake()
        {
            if (config == null || historyManager == null ||
                panelGroup == null || openButton == null || closeButton == null ||
                rowPrefab == null || rowContainer == null || scrollRect == null || emptyMessage == null ||
                roundsText == null || winsText == null || lossesText == null ||
                creditsBetText == null || creditsWonText == null || netText == null)
            {
                Debug.LogError("[GameHistoryUI] A reference is missing in the Inspector.", this);
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
            if (!isReady) return;

            historyManager.OnHistoryChanged += HandleHistoryChanged;
            if (gameManager != null) gameManager.OnStateChanged += HandleStateChanged;
            UpdateOpenButton();
        }

        private void OnDisable()
        {
            if (historyManager != null) historyManager.OnHistoryChanged -= HandleHistoryChanged;
            if (gameManager != null) gameManager.OnStateChanged -= HandleStateChanged;
        }

        public void Open()
        {
            if (!isReady || IsOpen) return;

            // In a play scene, do not cover the result screen while a round is running.
            if (gameManager != null && gameManager.State != GameState.Idle) return;

            IsOpen = true;
            Refresh();
            SetPanelVisible(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(rowContainer);
            scrollRect.verticalNormalizedPosition = 1f; // start at the newest round

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

        private void HandleStateChanged(GameState state)
        {
            UpdateOpenButton();
        }

        private void UpdateOpenButton()
        {
            openButton.interactable = gameManager == null || gameManager.State == GameState.Idle;
        }

        private void SetPanelVisible(bool visible)
        {
            panelGroup.alpha = visible ? 1f : 0f;
            panelGroup.interactable = visible;
            panelGroup.blocksRaycasts = visible;
        }

        // ---------- Drawing ----------

        private void Refresh()
        {
            RefreshStatistics();
            RefreshRows();
        }

        private void RefreshRows()
        {
            IReadOnlyList<GameRound> rounds = historyManager.Rounds;
            int count = rounds.Count;

            // Reuse row objects instead of destroying and recreating them.
            while (rows.Count < count)
            {
                HistoryRowView row = Instantiate(rowPrefab, rowContainer);
                row.name = "HistoryRow" + (rows.Count + 1);
                rows.Add(row);
            }

            for (int i = 0; i < rows.Count; i++)
            {
                if (i < count)
                {
                    rows[i].gameObject.SetActive(true);
                    rows[i].Bind(rounds[count - 1 - i], config); // newest first
                }
                else
                {
                    rows[i].gameObject.SetActive(false);
                }
            }

            emptyMessage.SetActive(count == 0);
        }

        private void RefreshStatistics()
        {
            GameStatistics stats = historyManager.Statistics;
            if (stats == null) return;

            roundsText.text = Cell("ROUNDS", Format(stats.totalRounds));
            winsText.text = Cell("WINS", Format(stats.wins));
            lossesText.text = Cell("LOSSES", Format(stats.losses));
            creditsBetText.text = Cell("CREDITS BET", Format(stats.creditsBet));
            creditsWonText.text = Cell("CREDITS WON", Format(stats.creditsWon));

            long net = stats.NetCredits;
            Color netColor = net > 0 ? gainColor : (net < 0 ? lossColor : Color.white);
            string netString = net.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);
            netText.text = Cell("NET", $"<color=#{ColorUtility.ToHtmlStringRGB(netColor)}>{netString}</color>");
        }

        // Small label on top, big value underneath.
        private static string Cell(string label, string value)
        {
            return $"<size=55%>{label}</size>\n{value}";
        }

        private static string Format(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}