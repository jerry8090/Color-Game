using System.Collections.Generic;
using UnityEngine;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// The outcome of one round: how many matches, what multiplier, and what was won.
    /// Winnings is the TOTAL returned to the player (stake included).
    /// Example: bet 100, 2 matches at 2x -> Winnings 200, NetResult +100.
    /// </summary>
    public sealed class PayoutResult
    {
        public int Matches { get; }
        public float Multiplier { get; }
        public long Winnings { get; }
        public int BetAmount { get; }
        public IReadOnlyList<bool> SlotMatches { get; }

        /// <summary>Winnings minus the bet. Negative means the player lost credits this round.</summary>
        public long NetResult => Winnings - BetAmount;

        /// <summary>A round is a WIN when the selected color appeared at least once.</summary>
        public bool IsWin => Matches > 0;

        public PayoutResult(int matches, float multiplier, long winnings, int betAmount, bool[] slotMatches)
        {
            Matches = matches;
            Multiplier = multiplier;
            Winnings = winnings;
            BetAmount = betAmount;
            SlotMatches = slotMatches;
        }

        /// <summary>A safe "nothing won" result, returned when the input was invalid.</summary>
        public static PayoutResult Empty(int betAmount)
        {
            return new PayoutResult(0, 0f, 0, Mathf.Max(0, betAmount), new bool[ColorGameConfig.ResultSlotCount]);
        }
    }

    /// <summary>
    /// The ONLY place that decides how much a round pays.
    /// Rules come from ColorGameConfig, so nothing is hard-coded here.
    /// In a real-money version this logic would run on the server.
    /// </summary>
    public class PayoutCalculator
    {
        private readonly ColorGameConfig config;

        public PayoutCalculator(ColorGameConfig config)
        {
            this.config = config;

            if (config == null)
            {
                Debug.LogError("[PayoutCalculator] Config is null. Payouts will be zero.");
            }
        }

        public PayoutResult Calculate(GameColor[] results, GameColor selectedColor, int betAmount)
        {
            if (config == null) return PayoutResult.Empty(betAmount);

            if (results == null || results.Length != ColorGameConfig.ResultSlotCount)
            {
                Debug.LogError($"[PayoutCalculator] Results must contain exactly {ColorGameConfig.ResultSlotCount} colors.");
                return PayoutResult.Empty(betAmount);
            }

            if (betAmount <= 0)
            {
                Debug.LogError($"[PayoutCalculator] Bet must be greater than zero. Got {betAmount}.");
                return PayoutResult.Empty(betAmount);
            }

            // Count matches and remember which slots matched (the UI uses this for highlighting).
            bool[] slotMatches = new bool[results.Length];
            int matches = 0;

            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] == selectedColor)
                {
                    slotMatches[i] = true;
                    matches++;
                }
            }

            float multiplier = config.GetPayoutMultiplier(matches);

            // Round half up so a multiplier like 1.5x on a bet of 10 is always 15.
            long winnings = (long)System.Math.Round((double)betAmount * multiplier, System.MidpointRounding.AwayFromZero);

            return new PayoutResult(matches, multiplier, winnings, betAmount, slotMatches);
        }
    }
}