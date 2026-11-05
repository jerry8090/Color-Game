using System;
using System.Collections.Generic;
using ScatterGame.Core;

namespace ScatterGame.Runtime
{
    /// <summary>One winning combination (or a scatter pay) as it was awarded.</summary>
    public sealed class WinEntry
    {
        public int SpinNumber;        // 0 = the paid spin, 1 and up = free spins
        public bool IsFreeSpin;
        public int CascadeIndex;
        public int SymbolId;
        public int Reels;             // 0 for a scatter pay
        public long Ways;
        public int Multiplier;
        public long AmountMilli;      // already includes the multiplier of that cascade
        public long SpinTotalMilli;   // this spin, after this win
        public long RoundTotalMilli;  // the whole round, after this win
    }

    /// <summary>
    /// Single source of truth for what a round has won so far. Pure logic, no Unity objects.
    /// Each cascade is added exactly once, using the engine's own numbers. Totals are never recalculated.
    /// </summary>
    public sealed class SpinWinLedger
    {
        private readonly List<WinEntry> entries = new List<WinEntry>();
        private readonly HashSet<long> recorded = new HashSet<long>();

        public IReadOnlyList<WinEntry> Entries => entries;
        public long RoundTotalMilli { get; private set; }
        public long SpinTotalMilli { get; private set; }
        public long LastCascadeMilli { get; private set; }
        public int SpinNumber { get; private set; }
        public bool IsFreeSpin { get; private set; }

        /// <summary>A genuinely new paid round: everything starts at zero.</summary>
        public void BeginRound()
        {
            entries.Clear();
            recorded.Clear();
            RoundTotalMilli = 0;
            SpinTotalMilli = 0;
            LastCascadeMilli = 0;
            SpinNumber = 0;
            IsFreeSpin = false;
        }

        /// <summary>The next spin of the round (0 = paid spin). The round total keeps running, the spin total restarts.</summary>
        public void BeginSpin(int spinNumber, bool isFreeSpin)
        {
            SpinNumber = spinNumber;
            IsFreeSpin = isFreeSpin;
            SpinTotalMilli = 0;
            LastCascadeMilli = 0;
        }

        /// <summary>
        /// Records every winning combination of one cascade. Returns the new entries (empty if this cascade was already recorded).
        /// </summary>
        public List<WinEntry> AddCascade(CascadeStep step, long bet)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (bet <= 0) throw new ArgumentException("The bet must be greater than zero.");

            List<WinEntry> added = new List<WinEntry>();
            if (step.Wins == null || !recorded.Add(KeyFor(step.Index))) return added;

            long cascadeTotal = 0;
            foreach (SymbolWin win in step.Wins)
            {
                long amount = win.BaseWinMilli(bet) * step.Multiplier; // the multiplier is applied exactly once
                cascadeTotal += amount;
                SpinTotalMilli += amount;
                RoundTotalMilli += amount;

                WinEntry entry = new WinEntry
                {
                    SpinNumber = SpinNumber,
                    IsFreeSpin = IsFreeSpin,
                    CascadeIndex = step.Index,
                    SymbolId = win.SymbolId,
                    Reels = win.ReelsMatched,
                    Ways = win.Ways,
                    Multiplier = step.Multiplier,
                    AmountMilli = amount,
                    SpinTotalMilli = SpinTotalMilli,
                    RoundTotalMilli = RoundTotalMilli
                };
                entries.Add(entry);
                added.Add(entry);
            }

            LastCascadeMilli = cascadeTotal;
            return added;
        }

        /// <summary>Records the optional scatter pay of a spin (0 by default, so normally nothing happens).</summary>
        public List<WinEntry> AddScatterPay(int scatterSymbolId, long milli)
        {
            List<WinEntry> added = new List<WinEntry>();
            if (milli <= 0 || !recorded.Add(KeyFor(int.MaxValue))) return added;

            SpinTotalMilli += milli;
            RoundTotalMilli += milli;
            LastCascadeMilli = milli;

            WinEntry entry = new WinEntry
            {
                SpinNumber = SpinNumber,
                IsFreeSpin = IsFreeSpin,
                CascadeIndex = -1,
                SymbolId = scatterSymbolId,
                Reels = 0,
                Ways = 0,
                Multiplier = 1,
                AmountMilli = milli,
                SpinTotalMilli = SpinTotalMilli,
                RoundTotalMilli = RoundTotalMilli
            };
            entries.Add(entry);
            added.Add(entry);
            return added;
        }

        private long KeyFor(int cascadeIndex)
        {
            return ((long)SpinNumber << 32) | (uint)cascadeIndex;
        }
    }
}