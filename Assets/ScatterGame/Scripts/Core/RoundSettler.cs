using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    public enum SettlementStatus
    {
        Completed,
        InvalidRoundId,
        InvalidBet,
        DuplicateRound,
        InsufficientCredits,
        SpendFailed,
        CreditFailed,
        Busy
    }

    public sealed class SettlementResult
    {
        public SettlementStatus Status;
        public string RoundId;
        public long Bet;

        /// <summary>The full resolved round (paid spin and every free spin). Null if nothing was charged.</summary>
        public RoundResult Round;

        public long CreditsCharged;
        public long CreditsPaid;

        /// <summary>Winnings that could not be credited (only with CreditFailed). Must be handled by the caller.</summary>
        public long OwedCredits;

        public long BalanceBefore;
        public long BalanceAfter;

        public bool Success => Status == SettlementStatus.Completed;
    }

    /// <summary>
    /// Plays one whole paid round against a wallet: validate, resolve, charge the bet once, credit the winnings once.
    /// Free spins are part of the same round and never charge a bet.
    /// The round is resolved BEFORE the bet is charged, so a failure can never leave a charge that needs refunding.
    /// A round id can be used once. Duplicates are rejected.
    /// </summary>
    public sealed class RoundSettler
    {
        private readonly SlotConfig config;
        private readonly IScatterWallet wallet;
        private readonly HashSet<long> allowedBets = new HashSet<long>();
        private readonly HashSet<string> usedRoundIds = new HashSet<string>();
        private bool playing;

        public RoundSettler(SlotConfig config, IScatterWallet wallet, IEnumerable<int> allowedBets)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (wallet == null) throw new ArgumentNullException(nameof(wallet));
            if (allowedBets == null) throw new ArgumentNullException(nameof(allowedBets));

            config.ValidateOrThrow();
            this.config = config;
            this.wallet = wallet;
            foreach (int bet in allowedBets)
            {
                if (bet > 0) this.allowedBets.Add(bet);
            }
            if (this.allowedBets.Count == 0) throw new ArgumentException("At least one positive bet amount is required.");
        }

        public bool IsBetAllowed(long bet)
        {
            return allowedBets.Contains(bet);
        }

        public SettlementResult Play(long bet, string roundId, ISymbolSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            SettlementResult result = new SettlementResult
            {
                RoundId = roundId,
                Bet = bet,
                BalanceBefore = wallet.GetBalance()
            };

            if (playing) return Finish(result, SettlementStatus.Busy);
            playing = true;

            try
            {
                if (string.IsNullOrWhiteSpace(roundId)) return Finish(result, SettlementStatus.InvalidRoundId);
                if (!allowedBets.Contains(bet)) return Finish(result, SettlementStatus.InvalidBet);
                if (usedRoundIds.Contains(roundId)) return Finish(result, SettlementStatus.DuplicateRound);
                if (result.BalanceBefore < bet) return Finish(result, SettlementStatus.InsufficientCredits);

                // Resolve first. If this throws, nothing has been spent and the round id is still free.
                RoundResult round = RoundResolver.Resolve(config, bet, source, false);

                usedRoundIds.Add(roundId);
                if (!wallet.TrySpend(bet, roundId))
                {
                    usedRoundIds.Remove(roundId); // nothing was charged, so the id may be used again
                    return Finish(result, SettlementStatus.SpendFailed);
                }

                result.Round = round;
                result.CreditsCharged = bet;

                long win = round.TotalWinCredits; // whole credits, rounded down once
                if (win > 0)
                {
                    if (!wallet.TryCredit(win, roundId))
                    {
                        result.OwedCredits = win;
                        return Finish(result, SettlementStatus.CreditFailed);
                    }
                    result.CreditsPaid = win;
                }

                return Finish(result, SettlementStatus.Completed);
            }
            finally
            {
                playing = false;
            }
        }

        private SettlementResult Finish(SettlementResult result, SettlementStatus status)
        {
            result.Status = status;
            result.BalanceAfter = wallet.GetBalance();
            return result;
        }
    }
}