using System.Collections.Generic;
using UnityEngine;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// The outcome of one round. Winnings is the TOTAL returned (stake included).
    /// Matches = cubes that showed any bet color. BestMatchCount = the most cubes showing ONE bet color (the payout tier, 0 to 3).
    /// </summary>
    public sealed class PayoutResult
    {
        public int Matches { get; }
        public int BestMatchCount { get; }
        public float Multiplier { get; }
        public long Winnings { get; }
        public int BetAmount { get; }
        public IReadOnlyList<bool> SlotMatches { get; }

        public long NetResult => Winnings - BetAmount;
        public bool IsWin => Matches > 0;

        public PayoutResult(int matches, float multiplier, long winnings, int betAmount, bool[] slotMatches, int bestMatchCount = -1)
        {
            Matches = matches;
            BestMatchCount = bestMatchCount < 0 ? matches : bestMatchCount;
            Multiplier = multiplier;
            Winnings = winnings;
            BetAmount = betAmount;
            SlotMatches = slotMatches;
        }

        public static PayoutResult Empty(int betAmount)
        {
            return new PayoutResult(0, 0f, 0, Mathf.Max(0, betAmount), new bool[ColorGameConfig.ResultSlotCount]);
        }
    }

    /// <summary>
    /// The ONLY place that decides how much a round pays. Rules come from ColorGameConfig.
    /// Every bet color is paid separately: bet x multiplier(how many cubes show that color).
    /// </summary>
    public class PayoutCalculator
    {
        private readonly ColorGameConfig config;

        public PayoutCalculator(ColorGameConfig config)
        {
            this.config = config;
            if (config == null) Debug.LogError("[PayoutCalculator] Config is null. Payouts will be zero.");
        }

        /// <summary>Single-color overload (kept for compatibility).</summary>
        public PayoutResult Calculate(GameColor[] results, GameColor selectedColor, int betAmount)
        {
            return Calculate(results, new List<ColorBetEntry> { new ColorBetEntry { color = selectedColor, amount = betAmount } });
        }

        public PayoutResult Calculate(GameColor[] results, IReadOnlyList<ColorBetEntry> bets)
        {
            if (config == null) return PayoutResult.Empty(0);

            if (results == null || results.Length != ColorGameConfig.ResultSlotCount)
            {
                Debug.LogError($"[PayoutCalculator] Results must contain exactly {ColorGameConfig.ResultSlotCount} colors.");
                return PayoutResult.Empty(0);
            }

            if (bets == null || bets.Count == 0)
            {
                Debug.LogError("[PayoutCalculator] No color bets were given.");
                return PayoutResult.Empty(0);
            }

            int totalBet = 0;
            foreach (ColorBetEntry bet in bets)
            {
                if (bet == null || bet.amount <= 0 || !System.Enum.IsDefined(typeof(GameColor), bet.color))
                {
                    Debug.LogError("[PayoutCalculator] A color bet is invalid (amount must be above zero).");
                    return PayoutResult.Empty(0);
                }
                totalBet += bet.amount;
            }

            bool[] slotMatches = new bool[results.Length];
            long winnings = 0;
            int best = 0;

            foreach (ColorBetEntry bet in bets)
            {
                int count = 0;
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i] != bet.color) continue;
                    slotMatches[i] = true;
                    count++;
                }

                best = Mathf.Max(best, count);
                float multiplier = config.GetPayoutMultiplier(count);
                winnings += (long)System.Math.Round((double)bet.amount * multiplier, System.MidpointRounding.AwayFromZero);
            }

            int matches = 0;
            foreach (bool matched in slotMatches) if (matched) matches++;

            float effective = totalBet > 0 ? (float)winnings / totalBet : 0f;
            return new PayoutResult(matches, effective, winnings, totalBet, slotMatches, best);
        }
    }
}