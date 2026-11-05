using System;
using System.Collections.Generic;
using ScatterGame.Core;

namespace ScatterGame.Runtime
{
    /// <summary>
    /// Pure rules for WHICH sounds play (no Unity audio here, so it is unit tested).
    /// </summary>
    public static class ScatterSoundPlanner
    {
        /// <summary>
        /// The winning symbols that should play their sound for one cascade: highest paying first, at most 'limit'.
        /// A symbol without a sound is skipped and does not use up a place.
        /// </summary>
        public static List<int> PickWinSymbols(IReadOnlyList<SymbolWin> wins, Func<int, bool> hasSound, int limit)
        {
            List<int> picked = new List<int>();
            if (wins == null || hasSound == null) return picked;

            List<SymbolWin> ordered = new List<SymbolWin>(wins);
            ordered.Sort((a, b) =>
            {
                int byPay = b.BaseWinMilli(1).CompareTo(a.BaseWinMilli(1));
                return byPay != 0 ? byPay : a.SymbolId.CompareTo(b.SymbolId);
            });

            int max = Math.Max(1, limit);
            foreach (SymbolWin win in ordered)
            {
                if (picked.Count >= max) break;
                if (picked.Contains(win.SymbolId) || !hasSound(win.SymbolId)) continue;
                picked.Add(win.SymbolId);
            }
            return picked;
        }

        /// <summary>True if a Wild cell (in the grid BEFORE the cascade) is part of any winning combination.</summary>
        public static bool WildTookPart(IReadOnlyList<SymbolWin> wins, SlotGrid gridBefore, int wildId)
        {
            if (wins == null || gridBefore == null || wildId < 0) return false;

            foreach (SymbolWin win in wins)
            {
                foreach (GridPosition position in win.Positions)
                {
                    if (gridBefore.Get(position) == wildId) return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Decides when a multiplier sound should play: once per NEW level, never again while the level stays the same.
    /// Call Reset at the start of every spin that resets its multiplier.
    /// </summary>
    public sealed class MultiplierSoundTracker
    {
        public int Announced { get; private set; }

        public void Reset()
        {
            Announced = 0;
        }

        public bool ShouldPlay(int multiplier)
        {
            if (multiplier < 1 || multiplier == Announced) return false;
            Announced = multiplier;
            return true;
        }
    }
}