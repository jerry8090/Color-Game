using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Core;

namespace Shared.Wallet
{
    /// <summary>
    /// Scene-facing wallet. Other scripts call this, never the service directly.
    /// Raises events whenever the balance changes so the UI can update itself.
    /// </summary>
    public class VirtualWalletManager : MonoBehaviour
    {
        [Header("Wallet Settings")]
        [SerializeField] private long startingBalance = 1000;
        [SerializeField] private int maxStoredTransactions = 5000;

        /// <summary>Raised with the new balance after any change.</summary>
        public event Action<long> OnBalanceChanged;

        /// <summary>Raised with the new ledger entry after any transaction.</summary>
        public event Action<WalletTransaction> OnTransactionAdded;

        private IWalletService walletService;

        public long StartingBalance => startingBalance;
        public IReadOnlyList<WalletTransaction> Transactions => Service.GetTransactions();

        // Created on first use, so other scripts can safely call us during Awake/OnEnable.
        private IWalletService Service
        {
            get
            {
                if (walletService == null)
                {
                    walletService = new LocalWalletService(maxStoredTransactions);
                }
                return walletService;
            }
        }

        private void OnValidate()
        {
            if (startingBalance < 1) startingBalance = 1;
            if (maxStoredTransactions < 10) maxStoredTransactions = 10;
        }

        /// <summary>Lets a future ServerWalletService replace the local one.</summary>
        public void SetWalletService(IWalletService service)
        {
            if (service == null)
            {
                Debug.LogError("[WalletManager] SetWalletService called with null.");
                return;
            }
            walletService = service;
            OnBalanceChanged?.Invoke(walletService.GetBalance());
        }

        public long GetBalance()
        {
            return Service.GetBalance();
        }

        public bool CanAfford(long amount)
        {
            return Service.CanAfford(amount);
        }

        /// <summary>Removes credits for a bet. Returns false if the bet is invalid or too expensive.</summary>
        public bool SpendCredits(long amount, string gameRoundId)
        {
            if (!Service.SpendCredits(amount, gameRoundId, out WalletTransaction tx)) return false;
            RaiseChanged(tx);
            return true;
        }

        /// <summary>Adds credits (Win, Bonus or AdminTest).</summary>
        public bool AddCredits(long amount, TransactionType type = TransactionType.Bonus, string gameRoundId = "")
        {
            if (!Service.AddCredits(amount, type, gameRoundId, out WalletTransaction tx)) return false;
            RaiseChanged(tx);
            return true;
        }

        /// <summary>Wipes the wallet and starts again with the starting balance.</summary>
        public void InitializeNewWallet()
        {
            WalletTransaction tx = Service.InitializeNewWallet(startingBalance);
            RaiseChanged(tx);
        }

        /// <summary>Sets the balance back to the starting balance (history of transactions is kept).</summary>
        public void ResetBalance()
        {
            WalletTransaction tx = Service.ResetBalance(startingBalance);
            RaiseChanged(tx);
        }

        /// <summary>Called by SaveManager when loading a save file.</summary>
        public bool LoadState(long savedBalance, List<WalletTransaction> savedTransactions)
        {
            if (!Service.LoadState(savedBalance, savedTransactions)) return false;
            OnBalanceChanged?.Invoke(Service.GetBalance());
            return true;
        }

        private void RaiseChanged(WalletTransaction tx)
        {
            OnBalanceChanged?.Invoke(Service.GetBalance());
            OnTransactionAdded?.Invoke(tx);
        }
    }
}