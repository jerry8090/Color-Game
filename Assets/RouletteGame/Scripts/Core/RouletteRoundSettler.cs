using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    public enum RouletteSettlementStatus
    {
        Completed,
        InvalidRoundId,
        NoBets,
        InvalidBet,
        LimitExceeded,
        DuplicateRound,
        InsufficientCredits,
        InvalidOutcome,
        SpendFailed,
        CreditFailed,
        Busy
    }

    public sealed class RouletteSettlementResult
    {
        public RouletteSettlementStatus Status;
        public string RoundId;
        public int WinningNumber = -1;

        /// <summary>Null unless the stake was charged.</summary>
        public RoundPayout Payout;

        public long CreditsCharged;
        public long CreditsPaid;

        /// <summary>Winnings that could not be credited (only with CreditFailed). Recover with TryRecoverOwed.</summary>
        public long OwedCredits;

        public long BalanceBefore;
        public long BalanceAfter;

        public bool Success => Status == RouletteSettlementStatus.Completed;
    }

    /// <summary>
    /// Plays one paid round against a wallet: validate everything, draw the number, work out the payout,
    /// THEN charge the stake once and credit the return once.
    /// Nothing can fail after the charge except the credit, and an unpaid credit is remembered and can be recovered.
    /// A round id works once. A second attempt is refused and never charges or pays again.
    /// The wallet is settled before any animation, so closing the app mid-spin can never lose or duplicate credits.
    /// </summary>
    public sealed class RouletteRoundSettler
    {
        private readonly IRouletteWallet wallet;
        private readonly RouletteLimits limits;
        private readonly HashSet<string> usedRoundIds = new HashSet<string>();
        private readonly Dictionary<string, long> owed = new Dictionary<string, long>();
        private bool playing;

        public RouletteRoundSettler(IRouletteWallet wallet, RouletteLimits limits = null)
        {
            this.wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            this.limits = limits ?? RouletteLimits.Default;
        }

        public RouletteSettlementResult Play(IReadOnlyList<RouletteBet> bets, string roundId, IOutcomeSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            RouletteSettlementResult result = new RouletteSettlementResult
            {
                RoundId = roundId,
                BalanceBefore = wallet.GetBalance()
            };

            if (playing) return Finish(result, RouletteSettlementStatus.Busy);
            playing = true;

            try
            {
                if (string.IsNullOrWhiteSpace(roundId)) return Finish(result, RouletteSettlementStatus.InvalidRoundId);

                // Work on a private copy: bets are immutable, so nothing can change them after this line.
                List<RouletteBet> snapshot = bets == null ? new List<RouletteBet>() : new List<RouletteBet>(bets);

                RouletteSettlementStatus check = CheckBets(snapshot, out long total);
                if (check != RouletteSettlementStatus.Completed) return Finish(result, check);

                if (usedRoundIds.Contains(roundId)) return Finish(result, RouletteSettlementStatus.DuplicateRound);
                if (result.BalanceBefore < total) return Finish(result, RouletteSettlementStatus.InsufficientCredits);

                // Draw and evaluate BEFORE charging. If anything throws here, nothing was spent and the id is still free.
                int number = source.NextNumber();
                if (!RouletteLayout.IsValidNumber(number)) return Finish(result, RouletteSettlementStatus.InvalidOutcome);
                RoundPayout payout = RoulettePayout.Evaluate(snapshot, number);

                usedRoundIds.Add(roundId);
                if (!wallet.TrySpend(total, roundId))
                {
                    usedRoundIds.Remove(roundId); // nothing was charged, so the id may be used again
                    return Finish(result, RouletteSettlementStatus.SpendFailed);
                }

                result.WinningNumber = number;
                result.Payout = payout;
                result.CreditsCharged = total;

                long returned = payout.TotalReturned; // stake included, only for winning bets
                if (returned > 0)
                {
                    if (!wallet.TryCredit(returned, roundId))
                    {
                        owed[roundId] = returned;
                        result.OwedCredits = returned;
                        return Finish(result, RouletteSettlementStatus.CreditFailed);
                    }
                    result.CreditsPaid = returned;
                }

                return Finish(result, RouletteSettlementStatus.Completed);
            }
            finally
            {
                playing = false;
            }
        }

        public bool IsRoundIdUsed(string roundId)
        {
            return roundId != null && usedRoundIds.Contains(roundId);
        }

        public long OwedFor(string roundId)
        {
            return roundId != null && owed.TryGetValue(roundId, out long amount) ? amount : 0;
        }

        /// <summary>Retries an unpaid credit. Pays at most once: after success the debt is gone.</summary>
        public bool TryRecoverOwed(string roundId, out long credited)
        {
            credited = 0;
            if (roundId == null || !owed.TryGetValue(roundId, out long amount)) return false;
            if (!wallet.TryCredit(amount, roundId)) return false;

            owed.Remove(roundId);
            credited = amount;
            return true;
        }

        private RouletteSettlementStatus CheckBets(List<RouletteBet> list, out long total)
        {
            total = 0;
            if (list.Count == 0) return RouletteSettlementStatus.NoBets;

            Dictionary<string, long> perArea = new Dictionary<string, long>();
            try
            {
                checked
                {
                    foreach (RouletteBet bet in list)
                    {
                        if (bet == null || bet.Amount <= 0) return RouletteSettlementStatus.InvalidBet;

                        perArea.TryGetValue(bet.Key, out long sum);
                        sum += bet.Amount;
                        if (sum > limits.MaxBetPerArea) return RouletteSettlementStatus.LimitExceeded;
                        perArea[bet.Key] = sum;

                        total += bet.Amount;
                        if (total > limits.MaxTotalBet) return RouletteSettlementStatus.LimitExceeded;
                    }
                }
            }
            catch (OverflowException)
            {
                return RouletteSettlementStatus.InvalidBet;
            }

            return RouletteSettlementStatus.Completed;
        }

        private RouletteSettlementResult Finish(RouletteSettlementResult result, RouletteSettlementStatus status)
        {
            result.Status = status;
            result.BalanceAfter = wallet.GetBalance();
            return result;
        }
    }
}
