using Shared.Save;
using Shared.Wallet;
using ScatterGame.Core;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Connects the slot engine to the EXISTING virtual wallet. There is still only one balance in the project.
    /// Every call creates a normal ledger transaction (Bet / Win), which SaveManager saves automatically.
    /// </summary>
    public sealed class UnityScatterWallet : IScatterWallet
    {
        private readonly VirtualWalletManager wallet;
        private readonly SaveManager saveManager;

        public UnityScatterWallet(VirtualWalletManager wallet, SaveManager saveManager)
        {
            this.wallet = wallet ?? throw new System.ArgumentNullException(nameof(wallet));
            this.saveManager = saveManager;
        }

        public long GetBalance()
        {
            return wallet.GetBalance();
        }

        public bool TrySpend(long amount, string roundId)
        {
            // Never take credits before the save file has been loaded: that state could not be saved safely.
            if (saveManager != null && !saveManager.IsReady) return false;
            return wallet.SpendCredits(amount, roundId);
        }

        public bool TryCredit(long amount, string roundId)
        {
            return wallet.AddCredits(amount, TransactionType.Win, roundId);
        }
    }
}