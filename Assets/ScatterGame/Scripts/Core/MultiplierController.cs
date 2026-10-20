using System;

namespace ScatterGame.Core
{
    /// <summary>
    /// Cascade multiplier. Every winning cascade uses the next ladder value, never more than the maximum.
    /// The multiplier applies to THAT cascade's win only. It is never applied to the running total.
    /// </summary>
    public sealed class MultiplierController
    {
        private readonly int[] ladder;
        private readonly int maxMultiplier;

        public int WinsRegistered { get; private set; }
        public int HighestApplied { get; private set; }

        public MultiplierController(int[] ladder, int maxMultiplier)
        {
            if (ladder == null || ladder.Length == 0) throw new ArgumentException("The multiplier ladder cannot be empty.");
            if (maxMultiplier < 1) throw new ArgumentException("The maximum multiplier must be at least 1.");

            this.ladder = (int[])ladder.Clone();
            this.maxMultiplier = maxMultiplier;
        }

        /// <summary>What the UI should show now: the last applied value, or the first step before any win.</summary>
        public int Current => WinsRegistered == 0 ? Step(0) : Step(WinsRegistered - 1);

        /// <summary>The multiplier the next winning cascade will use.</summary>
        public int NextToApply => Step(WinsRegistered);

        /// <summary>Call exactly once per winning cascade. Returns the multiplier to apply to that win.</summary>
        public int RegisterWin()
        {
            int applied = Step(WinsRegistered);
            WinsRegistered++;
            if (applied > HighestApplied) HighestApplied = applied;
            return applied;
        }

        public void Reset()
        {
            WinsRegistered = 0;
            HighestApplied = 0;
        }

        private int Step(int winIndex)
        {
            int value = ladder[Math.Min(winIndex, ladder.Length - 1)];
            if (value < 1) value = 1;
            return Math.Min(value, maxMultiplier);
        }
    }
}