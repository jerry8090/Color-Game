using System;
using System.Collections.Generic;

namespace ScatterGame.Runtime
{
    /// <summary>One winning combination inside a round. spin 0 = the paid spin, 1 and up = free spins.</summary>
    [Serializable]
    public class ScatterWinRecord
    {
        public int spin;
        public int cascade;
        public int multiplier;
        public int symbolId;
        public int reels;
        public long ways;
        public long winMilli; // pay x ways x bet x multiplier, in thousandths of a credit
    }

    /// <summary>Summary of one spin (the paid spin or one free spin).</summary>
    [Serializable]
    public class ScatterSpinRecord
    {
        public string kind;      // "paid" or "free"
        public string gridCsv;   // the starting 5x4 grid, reel by reel, top to bottom
        public int scatterCount;
        public int cascades;
        public int maxMultiplier;
        public long winMilli;
    }

    /// <summary>One finished paid round, including every free spin it earned.</summary>
    [Serializable]
    public class ScatterRoundRecord
    {
        public string roundId;
        public string timestampUtc;
        public long bet;
        public long winCredits;
        public long netCredits;
        public bool bonusTriggered;
        public int scatterCount;       // on the paid spin
        public int freeSpinsAwarded;   // including retriggers
        public int freeSpinsPlayed;
        public int retriggers;
        public int cascades;           // all spins together
        public int maxMultiplier;
        public bool hitCascadeLimit;
        public bool winsTruncated;
        public List<ScatterSpinRecord> spins = new List<ScatterSpinRecord>();
        public List<ScatterWinRecord> wins = new List<ScatterWinRecord>();
    }

    /// <summary>Informational numbers only. They never influence any outcome.</summary>
    [Serializable]
    public class ScatterStatistics
    {
        public long totalRounds;
        public long wins;
        public long totalBet;
        public long totalWon;
        public long bonusRounds;
        public long totalFreeSpinsAwarded;
        public long totalFreeSpinsPlayed;
        public long largestWin;

        public long NetCredits => totalWon - totalBet;
    }

    /// <summary>What is written into the save file's "ScatterGame.History" section.</summary>
    [Serializable]
    public class ScatterHistorySaveData
    {
        public long lastRoundNumber;
        public List<ScatterRoundRecord> rounds = new List<ScatterRoundRecord>();
        public ScatterStatistics statistics = new ScatterStatistics();
    }
}