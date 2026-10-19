using System;
using UnityEngine;
using Shared.Wallet;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// Holds the currently selected bet and checks it against the wallet.
    /// It does NOT spend credits. ColorGameManager does that when a round starts (Phase 5).
    /// </summary>
    public class BetManager : MonoBehaviour
    {
        public const string InsufficientCreditsMessage = "INSUFFICIENT CREDITS";

        [SerializeField] private ColorGameConfig config;
        [SerializeField] private VirtualWalletManager walletManager;

        /// <summary>Raised with the new bet amount, or 0 when the selection was cleared.</summary>
        public event Action<int> OnBetChanged;

        /// <summary>Raised with a message to show the player (for example "INSUFFICIENT CREDITS").</summary>
        public event Action<string> OnBetRejected;

        private int selectedBet;
        private bool isLocked;
        private bool isReady;

        public int SelectedBet => selectedBet;
        public bool HasBet => selectedBet > 0;
        public bool IsLocked => isLocked;

        private void Awake()
        {
            if (config == null || walletManager == null)
            {
                Debug.LogError("[BetManager] Config or Wallet Manager is not assigned in the Inspector.", this);
                return;
            }
            isReady = true;
        }

        private void OnEnable()
        {
            if (walletManager != null) walletManager.OnBalanceChanged += HandleBalanceChanged;
        }

        private void OnDisable()
        {
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;
        }

        /// <summary>True if the amount is a configured bet and the player can afford it.</summary>
        public bool CanSelectBet(int amount)
        {
            return isReady && config.IsValidBetAmount(amount) && walletManager.CanAfford(amount);
        }

        /// <summary>Tries to select a bet. Returns true on success.</summary>
        public bool TrySelectBet(int amount)
        {
            if (!isReady) return false;

            if (isLocked)
            {
                Debug.LogWarning("[BetManager] Bet cannot be changed while a round is running.");
                return false;
            }

            if (amount <= 0)
            {
                Debug.LogError($"[BetManager] Invalid bet amount: {amount}");
                return false;
            }

            if (!config.IsValidBetAmount(amount))
            {
                Debug.LogError($"[BetManager] {amount} is not one of the configured bet amounts.");
                return false;
            }

            if (!walletManager.CanAfford(amount))
            {
                OnBetRejected?.Invoke(InsufficientCreditsMessage);
                return false;
            }

            selectedBet = amount;
            OnBetChanged?.Invoke(selectedBet);
            return true;
        }

        public void ClearBet()
        {
            if (selectedBet == 0) return;
            selectedBet = 0;
            OnBetChanged?.Invoke(selectedBet);
        }

        /// <summary>
        /// Locked while a round runs. While locked, the bet cannot change and is not auto-cleared
        /// (the bet has already been deducted by then, so the balance is lower).
        /// </summary>
        public void SetLocked(bool locked)
        {
            if (isLocked == locked) return;
            isLocked = locked;

            if (!isLocked) ValidateSelectedBet();
        }

        private void HandleBalanceChanged(long newBalance)
        {
            if (!isLocked) ValidateSelectedBet();
        }

        // If the player can no longer afford the selected bet, clear it and tell them.
        private void ValidateSelectedBet()
        {
            if (!HasBet || walletManager.CanAfford(selectedBet)) return;

            ClearBet();
            OnBetRejected?.Invoke(InsufficientCreditsMessage);
        }
    }
}