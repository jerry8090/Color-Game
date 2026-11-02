using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Creates the chip (bet amount) buttons. The selected chip stays selected while you tap colors.
    /// Buttons dim when the credits still free (balance minus the bets on the table) are too low for that chip.
    /// </summary>
    public class BetSelectionUI : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private BetManager betManager;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private ColorGameManager gameManager;
        [Tooltip("The bets on the table (shows TOTAL and LEFT).")]
        [SerializeField] private BetSlip betSlip;
        [SerializeField] private BetButtonView buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TMP_Text selectedBetLabel;

        private readonly List<BetButtonView> views = new List<BetButtonView>();
        private bool isReady;

        private void Awake()
        {
            if (config == null || betManager == null || walletManager == null || gameManager == null ||
                buttonPrefab == null || buttonContainer == null || selectedBetLabel == null)
            {
                Debug.LogError("[BetSelectionUI] A reference is missing in the Inspector.", this);
                return;
            }

            selectedBetLabel.enableAutoSizing = true;
            selectedBetLabel.fontSizeMin = 16f;
            selectedBetLabel.fontSizeMax = 40f;

            BuildButtons();
            isReady = views.Count > 0;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            betManager.OnBetChanged += HandleBetChanged;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
            gameManager.OnStateChanged += HandleStateChanged;
            if (betSlip != null) betSlip.OnTotalChanged += HandleTotalChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if (betManager != null) betManager.OnBetChanged -= HandleBetChanged;
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
            if (gameManager != null) gameManager.OnStateChanged -= HandleStateChanged;
            if (betSlip != null) betSlip.OnTotalChanged -= HandleTotalChanged;
        }

        private void BuildButtons()
        {
            if (config.BetAmounts.Count == 0)
            {
                Debug.LogError("[BetSelectionUI] The config has no bet amounts.", config);
                return;
            }

            foreach (int amount in config.BetAmounts)
            {
                BetButtonView view = Instantiate(buttonPrefab, buttonContainer);
                view.name = "BetButton_" + amount;
                view.Setup(amount);
                view.OnClicked += HandleBetClicked;
                views.Add(view);
            }
        }

        private void HandleBetClicked(int amount)
        {
            betManager.TrySelectBet(amount); // shows a toast by itself if the amount is rejected
        }

        private void HandleBetChanged(int bet) { Refresh(); }
        private void HandleBalanceChanged(long balance) { Refresh(); }
        private void HandleStateChanged(GameState state) { Refresh(); }
        private void HandleTotalChanged(int total) { Refresh(); }

        private void Refresh()
        {
            bool canInteract = gameManager.State == GameState.Idle;
            long onTable = betSlip != null ? betSlip.TotalBet : 0;
            long free = walletManager.GetBalance() - onTable;

            foreach (BetButtonView view in views)
            {
                bool isSelected = betManager.HasBet && view.Amount == betManager.SelectedBet;
                view.SetSelected(isSelected);
                view.SetAffordable(free >= view.Amount);
                view.SetInteractable(canInteract);
            }

            string chip = betManager.HasBet ? betManager.SelectedBet.ToString("N0", CultureInfo.InvariantCulture) : "NONE";
            selectedBetLabel.text = betSlip == null
                ? "BET: " + chip
                : $"CHIP: {chip}\nTOTAL: {onTable.ToString("N0", CultureInfo.InvariantCulture)}   LEFT: {free.ToString("N0", CultureInfo.InvariantCulture)}";
        }
    }
}