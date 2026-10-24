using System;
using System.Collections.Generic;
using ScatterGame.Core;

namespace ScatterGame.Runtime
{
    /// <summary>
    /// Pure history logic: round ids, stored rounds, statistics, trimming, and building a record from a settlement.
    /// No Unity objects and no save file here, so it is fully testable. ScatterHistoryManager connects it to SaveManager.
    /// </summary>
    public sealed class ScatterHistoryService
    {
        public const string IdPrefix = "SCATTER-";
        public const int MaxWinRecordsPerRound = 40;

        private readonly List<ScatterRoundRecord> rounds = new List<ScatterRoundRecord>();
        private readonly int limit;

        public long LastRoundNumber { get; private set; }
        public ScatterStatistics Statistics { get; private set; } = new ScatterStatistics();

        /// <summary>Oldest first.</summary>
        public IReadOnlyList<ScatterRoundRecord> Rounds => rounds;

        public ScatterHistoryService(int historyLimit)
        {
            limit = Math.Max(1, historyLimit);
        }

        /// <summary>Hands out the next id, for example SCATTER-000001. Never the same id twice.</summary>
        public string ReserveNextRoundId()
        {
            LastRoundNumber++;
            return IdPrefix + LastRoundNumber.ToString("000000");
        }

        public void AddRound(ScatterRoundRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            rounds.Add(record);

            ScatterStatistics s = Statistics;
            s.totalRounds++;
            if (record.winCredits > 0) s.wins++;
            s.totalBet += record.bet;
            s.totalWon += record.winCredits;
            if (record.bonusTriggered) s.bonusRounds++;
            s.totalFreeSpinsAwarded += record.freeSpinsAwarded;
            s.totalFreeSpinsPlayed += record.freeSpinsPlayed;
            if (record.winCredits > s.largestWin) s.largestWin = record.winCredits;

            while (rounds.Count > limit) rounds.RemoveAt(0);
        }

        /// <summary>Removes stored rounds and statistics. The id counter keeps going, so ids are never reused.</summary>
        public void ClearHistory()
        {
            rounds.Clear();
            Statistics = new ScatterStatistics();
        }

        public void ResetAll()
        {
            ClearHistory();
            LastRoundNumber = 0;
        }

        /// <summary>Restores saved data. Every input may be null (older or empty save files).</summary>
        public void LoadState(long savedLastRoundNumber, List<ScatterRoundRecord> savedRounds, ScatterStatistics savedStatistics)
        {
            rounds.Clear();
            long highest = Math.Max(0, savedLastRoundNumber);

            if (savedRounds != null)
            {
                foreach (ScatterRoundRecord record in savedRounds)
                {
                    if (record == null) continue;
                    rounds.Add(record);
                    highest = Math.Max(highest, ParseNumber(record.roundId));
                }
            }

            while (rounds.Count > limit) rounds.RemoveAt(0);

            // Even if the saved counter is behind, an id that is already in the history is never handed out again.
            LastRoundNumber = highest;
            Statistics = savedStatistics ?? new ScatterStatistics();
        }

        // ---------- Building a record ----------

        public static ScatterRoundRecord FromSettlement(SettlementResult settlement, DateTime utcNow)
        {
            if (settlement == null) throw new ArgumentNullException(nameof(settlement));
            if (!settlement.Success || settlement.Round == null)
            {
                throw new ArgumentException("Only a completed settlement can be recorded.");
            }

            RoundResult round = settlement.Round;
            ScatterRoundRecord record = new ScatterRoundRecord
            {
                roundId = settlement.RoundId,
                timestampUtc = utcNow.ToString("o"),
                bet = settlement.Bet,
                winCredits = settlement.CreditsPaid,
                netCredits = settlement.CreditsPaid - settlement.Bet,
                bonusTriggered = round.BonusTriggered,
                scatterCount = round.PaidSpin.ScatterCount,
                freeSpinsAwarded = round.TotalFreeSpinsAwarded,
                freeSpinsPlayed = round.FreeSpins.Count,
                retriggers = round.FreeSpinRetriggers,
                cascades = round.TotalCascades,
                maxMultiplier = round.MaxMultiplier
            };

            AddSpin(record, round.PaidSpin, 0, "paid", settlement.Bet);
            for (int i = 0; i < round.FreeSpins.Count; i++)
            {
                AddSpin(record, round.FreeSpins[i], i + 1, "free", settlement.Bet);
            }

            return record;
        }

        private static void AddSpin(ScatterRoundRecord record, SpinResult spin, int spinIndex, string kind, long bet)
        {
            record.spins.Add(new ScatterSpinRecord
            {
                kind = kind,
                gridCsv = string.Join(",", spin.InitialGrid.ToArray()),
                scatterCount = spin.ScatterCount,
                cascades = spin.Steps.Count,
                maxMultiplier = spin.MaxMultiplier,
                winMilli = spin.TotalWinMilli
            });

            if (spin.HitCascadeLimit) record.hitCascadeLimit = true;

            foreach (CascadeStep step in spin.Steps)
            {
                foreach (SymbolWin win in step.Wins)
                {
                    if (record.wins.Count >= MaxWinRecordsPerRound)
                    {
                        record.winsTruncated = true;
                        return;
                    }

                    record.wins.Add(new ScatterWinRecord
                    {
                        spin = spinIndex,
                        cascade = step.Index,
                        multiplier = step.Multiplier,
                        symbolId = win.SymbolId,
                        reels = win.ReelsMatched,
                        ways = win.Ways,
                        winMilli = win.BaseWinMilli(bet) * step.Multiplier
                    });
                }
            }
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