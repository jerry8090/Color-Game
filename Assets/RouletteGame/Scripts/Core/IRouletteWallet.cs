namespace RouletteGame.Core
{
    /// <summary>
    /// The only thing the roulette engine needs from a wallet.
    /// UnityRouletteWallet implements it on top of the existing VirtualWalletManager, so there is still ONE balance in the project.
    /// </summary>
    public interface IRouletteWallet
    {
        long GetBalance();

        /// <summary>Removes the stake. Returns false (and changes nothing) if it cannot be done.</summary>
        bool TrySpend(long amount, string roundId);

        /// <summary>Adds winnings (stake included). Returns false if it cannot be done.</summary>
        bool TryCredit(long amount, string roundId);
    }
}
