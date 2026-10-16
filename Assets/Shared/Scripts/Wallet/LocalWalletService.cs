using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Core;

namespace Shared.Wallet
{
    /// <summary>
    /// Local (offline) wallet for the virtual-credit prototype.
    /// Holds the balance and the transaction ledger in memory.
    /// </summary>
    public class LocalWalletService : IWalletService
    {
        private readonly List<WalletTransaction> transactions = new List<WalletTransaction>();
        private readonly int maxTransactions;

        private long balance;
        private long nextTransactionNumber = 1;

        public LocalWalletService(int maxStoredTransactions = 1000)
        {
            maxTransactions = Mathf.Max(10, maxStoredTransactions);
        }

        public long GetBalance()
        {
            return balance;
        }

        public bool CanAfford(long amount)
        {
            return amount > 0 && balance >= amount;
        }

        public IReadOnlyList<WalletTransaction> GetTransactions()
        {
            return transactions;
        }

        public WalletTransaction InitializeNewWallet(long startingBalance)
        {
            transactions.Clear();
            balance = 0;
            nextTransactionNumber = 1;
            return Record(TransactionType.StartingBalance, startingBalance, string.Empty);
        }

        public bool SpendCredits(long amount, string gameRoundId, out WalletTransaction transaction)
        {
            transaction = null;

            if (amount <= 0)
            {
                Debug.LogError($"[Wallet] SpendCredits called with invalid amount: {amount}");
                return false;
            }

            if (balance < amount)
            {
                // Normal gameplay situation, not a developer error.
                Debug.LogWarning($"[Wallet] Insufficient credits. Balance {balance}, needed {amount}.");
                return false;
            }

            transaction = Record(TransactionType.Bet, -amount, gameRoundId);
            return true;
        }

        public bool AddCredits(long amount, TransactionType type, string gameRoundId, out WalletTransaction transaction)
        {
            transaction = null;

            if (amount <= 0)
            {
                Debug.LogError($"[Wallet] AddCredits called with invalid amount: {amount}");
                return false;
            }

            // Bets, starting balance and resets have their own dedicated methods.
            if (type == TransactionType.Bet ||
                type == TransactionType.StartingBalance ||
                type == TransactionType.Reset)
            {
                Debug.LogError($"[Wallet] AddCredits cannot be used for transaction type {type}.");
                return false;
            }

            transaction = Record(type, amount, gameRoundId);
            return true;
        }

        public WalletTransaction ResetBalance(long startingBalance)
        {
            // Amount can be negative (balance was higher) or positive (balance was lower).
            return Record(TransactionType.Reset, startingBalance - balance, string.Empty);
        }

        public bool LoadState(long savedBalance, IEnumerable<WalletTransaction> savedTransactions)
        {
            if (savedBalance < 0)
            {
                Debug.LogError($"[Wallet] Cannot load a negative balance: {savedBalance}");
                return false;
            }

            transactions.Clear();
            long highestNumber = 0;

            if (savedTransactions != null)
            {
                foreach (WalletTransaction tx in savedTransactions)
                {
                    if (tx == null) continue;
                    transactions.Add(tx);
                    highestNumber = Math.Max(highestNumber, ParseTransactionNumber(tx.transactionId));
                }
            }

            balance = savedBalance;
            nextTransactionNumber = highestNumber + 1;
            return true;
        }

        // Single place where the balance changes. Every change creates a ledger record.
        private WalletTransaction Record(TransactionType type, long amount, string gameRoundId)
        {
            long before = balance;
            balance += amount;

            WalletTransaction tx = new WalletTransaction
            {
                transactionId = "TXN-" + nextTransactionNumber.ToString("000000"),
                type = type,
                amount = amount,
                balanceBefore = before,
                balanceAfter = balance,
                timestampUtc = DateTime.UtcNow.ToString("o"),
                gameRoundId = gameRoundId ?? string.Empty
            };

            nextTransactionNumber++;
            transactions.Add(tx);

            // Keep the ledger from growing forever in the prototype.
            while (transactions.Count > maxTransactions)
            {
                transactions.RemoveAt(0);
            }

            return tx;
        }

        private static long ParseTransactionNumber(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId)) return 0;
            int dash = transactionId.LastIndexOf('-');
            if (dash < 0) return 0;
            return long.TryParse(transactionId.Substring(dash + 1), out long number) ? number : 0;
        }
    }
}