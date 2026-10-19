using System;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// The permanent record of one finished round. Phase 5 creates it, Phase 7 stores and displays it.
    /// Timestamp is an ISO-8601 UTC string because JsonUtility cannot save DateTime.
    /// </summary>
    [Serializable]
    public class GameRound
    {
        public string roundId;            // ROUND-000001
        public GameColor selectedColor;
        public int betAmount;
        public GameColor result1;
        public GameColor result2;
        public GameColor result3;
        public int matches;
        public float payoutMultiplier;
        public long winnings;             // total returned to the player (stake included)
        public long startingBalance;      // balance before the bet was deducted
        public long endingBalance;        // balance after winnings were added
        public string timestampUtc;

        // Properties are not saved to JSON. They are calculated from the saved fields.
        public long NetResult => winnings - betAmount;
        public bool IsWin => matches > 0;

        public GameColor[] GetResults()
        {
            return new[] { result1, result2, result3 };
        }

        /// <summary>Builds a record from a finished round.</summary>
        public static GameRound Create(
            string roundId,
            GameColor selectedColor,
            int betAmount,
            GameColor[] results,
            PayoutResult payout,
            long startingBalance,
            long endingBalance)
        {
            if (results == null || results.Length != ColorGameConfig.ResultSlotCount)
            {
                UnityEngine.Debug.LogError("[GameRound] Results must contain exactly 3 colors.");
                return null;
            }

            return new GameRound
            {
                roundId = roundId,
                selectedColor = selectedColor,
                betAmount = betAmount,
                result1 = results[0],
                result2 = results[1],
                result3 = results[2],
                matches = payout.Matches,
                payoutMultiplier = payout.Multiplier,
                winnings = payout.Winnings,
                startingBalance = startingBalance,
                endingBalance = endingBalance,
                timestampUtc = DateTime.UtcNow.ToString("o")
            };
        }

        /// <summary>Turns a counter into an ID, for example 1 becomes ROUND-000001.</summary>
        public static string FormatRoundId(long roundNumber)
        {
            return "ROUND-" + roundNumber.ToString("000000");
        }
    }
}