using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    /// <summary>One bet inside a finished round.</summary>
    [Serializable]
    public class RouletteBetRecord
    {
        public int type;          // BetType as a number (saved data stores the number, so the enum order must never change)
        public string label;
        public long amount;
        public bool won;
        public long netWinnings;  // profit only, 0 when the bet lost
        public long totalReturn;  // stake + winnings, 0 when the bet lost
    }

    /// <summary>One finished round.</summary>
    [Serializable]
    public class RouletteRoundRecord
    {
        public string roundId;
        public string timestampUtc;   // ISO-8601, because JsonUtility cannot save DateTime
        public int winningNumber;
        public int color;             // PocketColor as a number
        public long totalBet;
        public long totalReturned;    // stake included
        public long net;              // returned minus bet
        public List<RouletteBetRecord> bets = new List<RouletteBetRecord>();
    }

    /// <summary>Informational totals. They never influence any outcome.</summary>
    [Serializable]
    public class RouletteStatistics
    {
        public long totalRounds;
        public long winningRounds;    // rounds where at least one bet won
        public long totalWagered;
        public long totalReturned;
        public long largestWin;       // biggest single-round profit
        public long zeroCount;

        public long NetCredits => totalReturned - totalWagered;
    }

    /// <summary>What is written into the save file's "RouletteGame.History" section.</summary>
    [Serializable]
    public class RouletteHistorySaveData
    {
        public long lastRoundNumber;
        public List<RouletteRoundRecord> rounds = new List<RouletteRoundRecord>();
        public RouletteStatistics statistics = new RouletteStatistics();
    }

    /// <summary>
    /// Pure history logic: round ids, stored rounds, statistics, trimming and building a record from a settlement.
    /// No Unity objects and no save file here, so it is fully testable. RouletteHistoryManager connects it to SaveManager.
    /// </summary>
    public sealed class RouletteHistoryService
    {
        public const string IdPrefix = "ROULETTE-";

        private readonly List<RouletteRoundRecord> rounds = new List<RouletteRoundRecord>();
        private readonly int limit;

        public long LastRoundNumber { get; private set; }
        public RouletteStatistics Statistics { get; private set; } = new RouletteStatistics();

        /// <summary>Oldest first.</summary>
        public IReadOnlyList<RouletteRoundRecord> Rounds => rounds;

        public RouletteHistoryService(int historyLimit)
        {
            limit = Math.Max(1, historyLimit);
        }

        /// <summary>Hands out the next id, for example ROULETTE-000001. Never the same id twice.</summary>
        public string ReserveNextRoundId()
        {
            LastRoundNumber++;
            return IdPrefix + LastRoundNumber.ToString("000000");
        }

        public void AddRound(RouletteRoundRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            rounds.Add(record);

            RouletteStatistics s = Statistics;
            s.totalRounds++;
            if (record.totalReturned > 0) s.winningRounds++;
            s.totalWagered += record.totalBet;
            s.totalReturned += record.totalReturned;
            if (record.net > s.largestWin) s.largestWin = record.net;
            if (record.winningNumber == 0) s.zeroCount++;

            while (rounds.Count > limit) rounds.RemoveAt(0);
        }

        /// <summary>The newest winning numbers, newest first.</summary>
        public List<int> RecentNumbers(int count)
        {
            List<int> list = new List<int>();
            for (int i = rounds.Count - 1; i >= 0 && list.Count < count; i--) list.Add(rounds[i].winningNumber);
            return list;
        }

        /// <summary>Removes stored rounds and statistics. The id counter keeps going, so ids are never reused.</summary>
        public void ClearHistory()
        {
            rounds.Clear();
            Statistics = new RouletteStatistics();
        }

        public void ResetAll()
        {
            ClearHistory();
            LastRoundNumber = 0;
        }

        /// <summary>Restores saved data. Every input may be null (older or empty save files).</summary>
        public void LoadState(long savedLastRoundNumber, List<RouletteRoundRecord> savedRounds, RouletteStatistics savedStatistics)
        {
            rounds.Clear();
            long highest = Math.Max(0, savedLastRoundNumber);

            if (savedRounds != null)
            {
                foreach (RouletteRoundRecord record in savedRounds)
                {
                    if (record == null) continue;
                    rounds.Add(record);
                    highest = Math.Max(highest, ParseNumber(record.roundId));
                }
            }

            while (rounds.Count > limit) rounds.RemoveAt(0);

            // Even if the saved counter is behind, an id that is already in the history is never handed out again.
            LastRoundNumber = highest;
            Statistics = savedStatistics ?? new RouletteStatistics();
        }

        /// <summary>Builds a record from a settlement. Works for every round whose stake was charged (also an unpaid credit).</summary>
        public static RouletteRoundRecord FromSettlement(RouletteSettlementResult settlement, DateTime utcNow)
        {
            if (settlement == null) throw new ArgumentNullException(nameof(settlement));
            if (settlement.Payout == null) throw new ArgumentException("Only a round that was charged and played can be recorded.");

            RoundPayout payout = settlement.Payout;
            RouletteRoundRecord record = new RouletteRoundRecord
            {
                roundId = settlement.RoundId,
                timestampUtc = utcNow.ToString("o"),
                winningNumber = payout.WinningNumber,
                color = (int)RouletteLayout.ColorOf(payout.WinningNumber),
                totalBet = payout.TotalStake,
                totalReturned = payout.TotalReturned,
                net = payout.NetResult
            };

            foreach (BetOutcome outcome in payout.Outcomes)
            {
                record.bets.Add(new RouletteBetRecord
                {
                    type = (int)outcome.Bet.Type,
                    label = RouletteText.Describe(outcome.Bet),
                    amount = outcome.Bet.Amount,
                    won = outcome.Won,
                    netWinnings = outcome.NetWinnings,
                    totalReturn = outcome.TotalReturn
                });
            }
            return record;
        }

        private static long ParseNumber(string roundId)
        {
            if (string.IsNullOrEmpty(roundId)) return 0;
            int dash = roundId.LastIndexOf('-');
            if (dash < 0) return 0;
            return long.TryParse(roundId.Substring(dash + 1), out long number) ? number : 0;
        }
    }
}
