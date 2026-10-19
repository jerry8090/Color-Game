using System;

namespace Shared.Wallet
{
    /// <summary>
    /// Every kind of balance change that is allowed to happen.
    /// </summary>
    public enum TransactionType
    {
        StartingBalance,
        Bet,
        Win,
        Bonus,
        AdminTest,
        Reset,
        Refund,
    }

    /// <summary>
    /// One line in the wallet ledger. Records are never edited after creation.
    /// Note: JsonUtility cannot save DateTime, so the timestamp is stored as an ISO-8601 string (UTC).
    /// </summary>
    [Serializable]
    public class WalletTransaction
    {
        public string transactionId;   // e.g. TXN-000001
        public TransactionType type;
        public long amount;            // negative for bets, positive for wins
        public long balanceBefore;
        public long balanceAfter;
        public string timestampUtc;    // e.g. 2026-10-05T08:30:00.0000000Z
        public string gameRoundId;     // empty when the transaction is not tied to a round
    }
}