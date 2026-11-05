using System;
using System.Collections.Generic;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>The total bet on one color in one round.</summary>
    [Serializable]
    public class ColorBetEntry
    {
        public GameColor color;
        public int amount;
    }

    /// <summary>
    /// The permanent record of one finished round.
    /// betAmount is the TOTAL wager. colorBets lists every color bet (empty in old saves).
    /// selectedColor is the color with the biggest bet (kept for old screens and old saves).
    /// </summary>
    [Serializable]
    public class GameRound
    {
        public string roundId;
        public GameColor selectedColor;
        public int betAmount;
        public GameColor result1;
        public GameColor result2;
        public GameColor result3;
        public int matches;               // cubes that showed any bet color
        public float payoutMultiplier;    // winnings / total bet
        public long winnings;             // total returned (stake included)
        public long startingBalance;
        public long endingBalance;
        public string timestampUtc;
        public List<ColorBetEntry> colorBets = new List<ColorBetEntry>();

        public long NetResult => winnings - betAmount;
        public bool IsWin => matches > 0;

        public GameColor[] GetResults()
        {
            return new[] { result1, result2, result3 };
        }

        /// <summary>Builds a record for a round with one or more color bets.</summary>
        public static GameRound Create(
            string roundId,
            IReadOnlyList<ColorBetEntry> bets,
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

            if (bets == null || bets.Count == 0)
            {
                UnityEngine.Debug.LogError("[GameRound] A wagered round needs at least one color bet.");
                return null;
            }

            GameRound round = new GameRound
            {
                roundId = roundId,
                betAmount = payout.BetAmount,
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

            ColorBetEntry biggest = bets[0];
            foreach (ColorBetEntry bet in bets)
            {
                round.colorBets.Add(new ColorBetEntry { color = bet.color, amount = bet.amount });
                if (bet.amount > biggest.amount) biggest = bet;
            }
            round.selectedColor = biggest.color;
            return round;
        }

        /// <summary>Old single-color signature, kept so nothing else breaks.</summary>
        public static GameRound Create(string roundId, GameColor selectedColor, int betAmount, GameColor[] results,
                                       PayoutResult payout, long startingBalance, long endingBalance)
        {
            List<ColorBetEntry> bets = new List<ColorBetEntry>
            {
                new ColorBetEntry { color = selectedColor, amount = betAmount }
            };
            return Create(roundId, bets, results, payout, startingBalance, endingBalance);
        }

        public static string FormatRoundId(long roundNumber)
        {
            return "ROUND-" + roundNumber.ToString("000000");
        }
    }
}