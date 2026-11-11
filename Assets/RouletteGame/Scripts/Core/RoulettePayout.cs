using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    /// <summary>The result of one bet. All values are whole credits.</summary>
    public sealed class BetOutcome
    {
        public RouletteBet Bet { get; }
        public bool Won { get; }

        /// <summary>Profit only: stake x multiplier (for example 350 on a winning straight-up of 10). 0 when the bet lost.</summary>
        public long NetWinnings { get; }

        /// <summary>The original stake given back. Only when the bet won.</summary>
        public long StakeReturned { get; }

        /// <summary>What the wallet receives for this bet: stake returned plus winnings. 0 when the bet lost.</summary>
        public long TotalReturn => NetWinnings + StakeReturned;

        public BetOutcome(RouletteBet bet, bool won, long netWinnings, long stakeReturned)
        {
            Bet = bet;
            Won = won;
            NetWinnings = netWinnings;
            StakeReturned = stakeReturned;
        }
    }

    /// <summary>The result of a whole round.</summary>
    public sealed class RoundPayout
    {
        public int WinningNumber { get; }
        public IReadOnlyList<BetOutcome> Outcomes { get; }

        /// <summary>Sum of all stakes (what was taken from the wallet).</summary>
        public long TotalStake { get; }

        /// <summary>Sum of all returns (what is credited to the wallet, once).</summary>
        public long TotalReturned { get; }

        public int WinningBetCount { get; }

        /// <summary>Returned minus staked. Negative = the player lost credits.</summary>
        public long NetResult => TotalReturned - TotalStake;

        public RoundPayout(int winningNumber, IReadOnlyList<BetOutcome> outcomes, long totalStake, long totalReturned, int winningBetCount)
        {
            WinningNumber = winningNumber;
            Outcomes = outcomes;
            TotalStake = totalStake;
            TotalReturned = totalReturned;
            WinningBetCount = winningBetCount;
        }
    }

    /// <summary>
    /// The ONLY place that decides what a round pays. Integer math only, overflow checked.
    /// European rules: the payout ratio is the profit. The winning stake is returned on top of it.
    /// </summary>
    public static class RoulettePayout
    {
        /// <summary>Profit per 1 credit staked when the bet wins (35 means 35 to 1).</summary>
        public static int MultiplierFor(BetType type)
        {
            switch (type)
            {
                case BetType.Straight: return 35;
                case BetType.Split: return 17;
                case BetType.Street: return 11;
                case BetType.Corner: return 8;
                case BetType.SixLine: return 5;
                case BetType.Dozen:
                case BetType.Column: return 2;
                case BetType.Red:
                case BetType.Black:
                case BetType.Odd:
                case BetType.Even:
                case BetType.Low:
                case BetType.High: return 1;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown bet type.");
            }
        }

        public static BetOutcome EvaluateBet(RouletteBet bet, int winningNumber)
        {
            if (bet == null) throw new ArgumentNullException(nameof(bet));
            if (!RouletteLayout.IsValidNumber(winningNumber)) throw new ArgumentOutOfRangeException(nameof(winningNumber));

            if (!bet.Covers(winningNumber)) return new BetOutcome(bet, false, 0, 0);

            long net = checked(bet.Amount * bet.Multiplier);
            return new BetOutcome(bet, true, net, bet.Amount);
        }

        public static RoundPayout Evaluate(IReadOnlyList<RouletteBet> bets, int winningNumber)
        {
            if (bets == null) throw new ArgumentNullException(nameof(bets));
            if (!RouletteLayout.IsValidNumber(winningNumber)) throw new ArgumentOutOfRangeException(nameof(winningNumber));

            List<BetOutcome> outcomes = new List<BetOutcome>(bets.Count);
            long stake = 0;
            long returned = 0;
            int wins = 0;

            checked
            {
                foreach (RouletteBet bet in bets)
                {
                    BetOutcome outcome = EvaluateBet(bet, winningNumber);
                    outcomes.Add(outcome);
                    stake += bet.Amount;
                    returned += outcome.TotalReturn;
                    if (outcome.Won) wins++;
                }
            }

            return new RoundPayout(winningNumber, outcomes, stake, returned, wins);
        }

        /// <summary>The best possible total return of these bets over all 37 numbers (for the "potential win" display).</summary>
        public static long MaxPossibleReturn(IReadOnlyList<RouletteBet> bets)
        {
            if (bets == null) throw new ArgumentNullException(nameof(bets));

            long best = 0;
            for (int number = 0; number <= RouletteLayout.MaxNumber; number++)
            {
                long total = Evaluate(bets, number).TotalReturned;
                if (total > best) best = total;
            }
            return best;
        }
    }
}
