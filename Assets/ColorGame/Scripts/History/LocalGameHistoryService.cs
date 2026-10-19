using System;
using System.Collections.Generic;
using UnityEngine;
using ColorGame.Gameplay;

namespace ColorGame.History
{
    /// <summary>
    /// Local (offline) history store for the virtual-credit prototype.
    /// A plain C# class: it is created by GameHistoryManager, never attached to an object.
    /// </summary>
    public class LocalGameHistoryService : IGameHistoryService
    {
        private readonly List<GameRound> rounds = new List<GameRound>();
        private readonly int historyLimit;

        private long lastRoundNumber;
        private GameStatistics statistics = new GameStatistics();

        public LocalGameHistoryService(int historyLimit)
        {
            this.historyLimit = Mathf.Max(1, historyLimit);
        }

        public IReadOnlyList<GameRound> Rounds => rounds;
        public long LastRoundNumber => lastRoundNumber;
        public GameStatistics Statistics => statistics;

        public string ReserveNextRoundId()
        {
            lastRoundNumber++;
            return GameRound.FormatRoundId(lastRoundNumber);
        }

        public void AddRound(GameRound round)
        {
            if (round == null)
            {
                Debug.LogError("[History] AddRound called with null.");
                return;
            }

            rounds.Add(round);
            statistics.Record(round);
            TrimToLimit();
        }

        public void ClearHistory()
        {
            rounds.Clear();
            statistics.Clear();
        }

        public void ResetAll()
        {
            rounds.Clear();
            statistics = new GameStatistics();
            lastRoundNumber = 0;
        }

        public void LoadState(long savedLastRoundNumber, IEnumerable<GameRound> savedRounds, GameStatistics savedStatistics)
        {
            rounds.Clear();
            lastRoundNumber = Math.Max(0, savedLastRoundNumber);

            if (savedRounds != null)
            {
                foreach (GameRound round in savedRounds)
                {
                    if (round == null) continue;
                    rounds.Add(round);

                    // Safety: the counter can never be lower than an existing round ID.
                    lastRoundNumber = Math.Max(lastRoundNumber, ParseRoundNumber(round.roundId));
                }
            }

            TrimToLimit();

            // Old saves (before Phase 7) have rounds but no statistics. Rebuild them from the rounds.
            bool statsMissing = savedStatistics == null ||
                                (savedStatistics.totalRounds == 0 && rounds.Count > 0);

            if (statsMissing)
            {
                statistics = new GameStatistics();
                statistics.RebuildFrom(rounds);
            }
            else
            {
                statistics = savedStatistics;
            }
        }

        private void TrimToLimit()
        {
            while (rounds.Count > historyLimit)
            {
                rounds.RemoveAt(0); // remove the oldest
            }
        }

        private static long ParseRoundNumber(string roundId)
        {
            if (string.IsNullOrEmpty(roundId)) return 0;
            int dash = roundId.LastIndexOf('-');
            if (dash < 0) return 0;
            return long.TryParse(roundId.Substring(dash + 1), out long number) ? number : 0;
        }
    }
}