namespace ScatterGame.Core
{
    /// <summary>
    /// The only thing the slot engine needs from a wallet.
    /// Stage 4 implements it on top of the existing VirtualWalletManager, so there is still ONE balance in the project.
    /// </summary>
    public interface IScatterWallet
    {
        long GetBalance();

        /// <summary>Removes the bet. Returns false (and changes nothing) if it cannot be done.</summary>
        bool TrySpend(long amount, string roundId);

        /// <summary>Adds winnings. Returns false if it cannot be done.</summary>
        bool TryCredit(long amount, string roundId);
    }
}