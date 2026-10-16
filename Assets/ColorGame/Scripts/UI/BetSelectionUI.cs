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
    /// Creates the bet buttons from the config, dims the ones the player cannot afford,
    /// and shows the selected bet. Clicks are forwarded to BetManager.
    /// </summary>
    public class BetSelectionUI : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private BetManager betManager;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private ColorGameManager gameManager;
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

            BuildButtons();
            isReady = views.Count > 0;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            betManager.OnBetChanged += HandleBetChanged;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
            gameManager.OnStateChanged += HandleStateChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if (betManager != null) betManager.OnBetChanged -= HandleBetChanged;
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
            if (gameManager != null) gameManager.OnStateChanged -= HandleStateChanged;
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
            betManager.TrySelectBet(amount); // shows a toast by itself if the bet is rejected
        }

        private void HandleBetChanged(int bet) { Refresh(); }
        private void HandleBalanceChanged(long balance) { Refresh(); }
        private void HandleStateChanged(GameState state) { Refresh(); }

        private void Refresh()
        {
            bool canInteract = gameManager.State == GameState.Idle;

            foreach (BetButtonView view in views)
            {
                bool isSelected = betManager.HasBet && view.Amount == betManager.SelectedBet;
                view.SetSelected(isSelected);
                view.SetAffordable(walletManager.CanAfford(view.Amount));
                view.SetInteractable(canInteract);
            }

            selectedBetLabel.text = betManager.HasBet
                ? "BET: " + betManager.SelectedBet.ToString("N0", CultureInfo.InvariantCulture)
                : "BET: NONE";
        }
    }
}