using System;
using System.Collections.Generic;
using System.Globalization;

namespace ScatterGame.Runtime
{
    /// <summary>How win amounts and combinations are written on screen. Pure text logic, so it is unit tested.</summary>
    public static class ScatterWinFormat
    {
        /// <summary>Milli-credits as credits with two decimals (rounded down), for example 64000 -> "64.00".</summary>
        public static string Credits(long milli)
        {
            if (milli < 0) milli = 0;
            long hundredths = milli / 10;
            return (hundredths / 100).ToString("N0", CultureInfo.InvariantCulture) + "." +
                   (hundredths % 100).ToString("00", CultureInfo.InvariantCulture);
        }

        public static string Signed(long milli)
        {
            return "+" + Credits(milli);
        }

        /// <summary>For example "3x JACK +10.00   4x QUEEN +200.00". Shows at most maxShown combinations, then "+N MORE".</summary>
        public static string BuildComboText(IReadOnlyList<WinEntry> entries, Func<int, string> nameOf, int maxShown)
        {
            if (entries == null || entries.Count == 0) return string.Empty;

            int shown = Math.Min(entries.Count, Math.Max(1, maxShown));
            List<string> parts = new List<string>();

            for (int i = 0; i < shown; i++)
            {
                WinEntry entry = entries[i];
                string name = nameOf != null ? nameOf(entry.SymbolId) : entry.SymbolId.ToString(CultureInfo.InvariantCulture);
                string prefix = entry.Reels > 0 ? entry.Reels + "x " : string.Empty;
                parts.Add(prefix + name + " " + Signed(entry.AmountMilli));
            }

            if (entries.Count > shown) parts.Add("+" + (entries.Count - shown) + " MORE");
            return string.Join("   ", parts);
        }
    }
}