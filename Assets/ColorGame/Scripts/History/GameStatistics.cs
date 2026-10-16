using System;
using System.Collections.Generic;
using ColorGame.Gameplay;

namespace ColorGame.History
{
    /// <summary>
    /// Lifetime totals for testing. Stored separately from the (trimmed) round list,
    /// so the numbers stay correct even after old rounds are removed from the history.
    /// Fields are saved to JSON. The properties below are calculated and not saved.
    /// </summary>
    [Serializable]
    public class GameStatistics
    {
        public long totalRounds;
        public long wins;
        public long losses;
        public long creditsBet;
        public long creditsWon;   // total returned to the player (stake included)

        /// <summary>Credits won minus credits bet. Negative means the player is down.</summary>
        public long NetCredits => creditsWon - creditsBet;

        public float WinRatePercent => totalRounds > 0 ? 100f * wins / totalRounds : 0f;

        /// <summary>Credits won divided by credits bet. Handy when tuning the payout table.</summary>
        public float ReturnToPlayerPercent => creditsBet > 0 ? 100f * creditsWon / creditsBet : 0f;

        /// <summary>Adds one finished round to the totals.</summary>
        public void Record(GameRound round)
        {
            if (round == null) return;

            totalRounds++;
            if (round.IsWin) wins++; else losses++;
            creditsBet += round.betAmount;
            creditsWon += round.winnings;
        }

        public void Clear()
        {
            totalRounds = 0;
            wins = 0;
            losses = 0;
            creditsBet = 0;
            creditsWon = 0;
        }

        /// <summary>Recounts everything from a list of rounds (used to upgrade old saves).</summary>
        public void RebuildFrom(IEnumerable<GameRound> rounds)
        {
            Clear();
            if (rounds == null) return;

            foreach (GameRound round in rounds)
            {
                Record(round);
            }
        }
    }
}