using System.Collections.Generic;
using Shared.Wallet;

namespace Shared.Core
{
    /// <summary>
    /// Everything the game needs from a wallet. The prototype uses LocalWalletService.
    /// A future backend version (for example ServerWalletService) would implement this
    /// same interface, so the game code does not need to be rewritten.
    /// </summary>
    public interface IWalletService
    {
        long GetBalance();
        bool CanAfford(long amount);
        IReadOnlyList<WalletTransaction> GetTransactions();

        /// <summary>Clears everything and creates a STARTING_BALANCE transaction.</summary>
        WalletTransaction InitializeNewWallet(long startingBalance);

        /// <summary>Removes credits and records a BET transaction. Returns false if invalid or unaffordable.</summary>
        bool SpendCredits(long amount, string gameRoundId, out WalletTransaction transaction);

        /// <summary>Adds credits. Only Win, Bonus and AdminTest are allowed here.</summary>
        bool AddCredits(long amount, TransactionType type, string gameRoundId, out WalletTransaction transaction);

        /// <summary>Sets the balance back to the starting balance and records a RESET transaction.</summary>
        WalletTransaction ResetBalance(long startingBalance);

        /// <summary>Restores a previously saved balance and ledger.</summary>
        bool LoadState(long balance, IEnumerable<WalletTransaction> savedTransactions);
    }
}