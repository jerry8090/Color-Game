using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    /// <summary>
    /// Every tunable rule of the slot. Nothing about symbols, pays, multipliers or bonus is hard-coded elsewhere.
    /// Win = PayMilli x ways x totalBet x cascadeMultiplier, in milli-credits (1000 = 1 credit).
    /// </summary>
    public sealed class SlotConfig
    {
        public const int Reels = 5;
        public const int Rows = 4;
        public const int MinWinReels = 3;
        public const int PayTiers = Reels - MinWinReels + 1; // 3, 4, 5 reels

        public List<SymbolDefinition> Symbols = new List<SymbolDefinition>();

        public int MaxMultiplier = 10;
        public int[] PaidMultiplierLadder = { 1, 2, 3, 5, 7, 10 };
        public int[] FreeSpinMultiplierLadder = { 2, 4, 6, 10 };

        /// <summary>false: each free spin starts its ladder again. true: the ladder continues across free spins.</summary>
        public bool FreeSpinMultiplierPersists = false;

        public int MaxCascadesPerSpin = 50;
        public bool GoldenTransformEnabled = true;

        public int MinScattersForBonus = 3;

        /// <summary>Free spins for MinScatters, MinScatters+1, ... (last value is used for any higher count).</summary>
        public int[] FreeSpinsAwarded = { 10, 12, 15 };

        /// <summary>Extra free spins when a free spin shows MinScattersForBonus or more scatters.</summary>
        public int RetriggerFreeSpins = 5;

        public int MaxTotalFreeSpinsPerRound = 30;

        /// <summary>Optional scatter pay (thousandths of total bet), same tiering as FreeSpinsAwarded. Not multiplied.</summary>
        public int[] ScatterPayMilli = { 0, 0, 0 };

        private Dictionary<int, SymbolDefinition> index;
        private int indexedCount = -1;

        private void EnsureIndex()
        {
            if (index != null && indexedCount == Symbols.Count) return;

            index = new Dictionary<int, SymbolDefinition>();
            foreach (SymbolDefinition symbol in Symbols)
            {
                if (symbol != null && !index.ContainsKey(symbol.Id)) index.Add(symbol.Id, symbol);
            }
            indexedCount = Symbols.Count;
        }

        public SymbolDefinition GetSymbol(int id)
        {
            EnsureIndex();
            if (!index.TryGetValue(id, out SymbolDefinition symbol))
            {
                throw new ArgumentException("Unknown symbol id " + id + ".");
            }
            return symbol;
        }

        public bool TryGetSymbol(int id, out SymbolDefinition symbol)
        {
            EnsureIndex();
            return index.TryGetValue(id, out symbol);
        }

        /// <summary>Id of the first Wild symbol, or -1 if there is none.</summary>
        public int WildSymbolId
        {
            get
            {
                foreach (SymbolDefinition symbol in Symbols)
                {
                    if (symbol != null && symbol.IsWild) return symbol.Id;
                }
                return -1;
            }
        }

        /// <summary>The symbols that can start a win: Regular and not golden.</summary>
        public IEnumerable<SymbolDefinition> BaseRegularSymbols()
        {
            foreach (SymbolDefinition symbol in Symbols)
            {
                if (symbol != null && symbol.Category == SymbolCategory.Regular && !symbol.IsGolden) yield return symbol;
            }
        }

        /// <summary>Free spins awarded by a scatter count (0 if the count does not trigger the bonus).</summary>
        public int FreeSpinsFor(int scatterCount)
        {
            if (scatterCount < MinScattersForBonus || FreeSpinsAwarded == null || FreeSpinsAwarded.Length == 0) return 0;
            int tier = Math.Min(scatterCount - MinScattersForBonus, FreeSpinsAwarded.Length - 1);
            return FreeSpinsAwarded[tier];
        }

        public int ScatterPayMilliFor(int scatterCount)
        {
            if (scatterCount < MinScattersForBonus || ScatterPayMilli == null || ScatterPayMilli.Length == 0) return 0;
            int tier = Math.Min(scatterCount - MinScattersForBonus, ScatterPayMilli.Length - 1);
            return ScatterPayMilli[tier];
        }

        /// <summary>Returns a list of problems. An empty list means the configuration is usable.</summary>
        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (Symbols == null || Symbols.Count == 0)
            {
                errors.Add("No symbols are defined.");
                return errors;
            }

            HashSet<int> ids = new HashSet<int>();
            bool hasScatter = false;
            bool hasWild = false;
            bool hasGolden = false;

            foreach (SymbolDefinition symbol in Symbols)
            {
                if (symbol == null)
                {
                    errors.Add("A symbol entry is null.");
                    continue;
                }

                if (!ids.Add(symbol.Id)) errors.Add("Duplicate symbol id " + symbol.Id + ".");

                if (symbol.ReelWeights == null || symbol.ReelWeights.Length != Reels)
                {
                    errors.Add("Symbol " + symbol.Id + " must have exactly " + Reels + " reel weights.");
                }
                else
                {
                    foreach (int weight in symbol.ReelWeights)
                    {
                        if (weight < 0) errors.Add("Symbol " + symbol.Id + " has a negative weight.");
                    }
                }

                if (symbol.IsScatter) hasScatter = true;
                if (symbol.IsWild) hasWild = true;
                if (symbol.IsGolden) hasGolden = true;

                if (symbol.Category == SymbolCategory.Regular && !symbol.IsGolden)
                {
                    if (symbol.PayMilli == null || symbol.PayMilli.Length != PayTiers)
                    {
                        errors.Add("Symbol " + symbol.Id + " must have " + PayTiers + " pay values (3, 4, 5 reels).");
                    }
                    else
                    {
                        foreach (int pay in symbol.PayMilli)
                        {
                            if (pay < 0) errors.Add("Symbol " + symbol.Id + " has a negative pay value.");
                        }
                    }
                }
            }

            foreach (SymbolDefinition symbol in Symbols)
            {
                if (symbol == null || !symbol.IsGolden) continue;

                SymbolDefinition baseSymbol = null;
                foreach (SymbolDefinition candidate in Symbols)
                {
                    if (candidate != null && candidate.Id == symbol.GoldenOfSymbolId) baseSymbol = candidate;
                }

                if (baseSymbol == null) errors.Add("Golden symbol " + symbol.Id + " refers to unknown symbol " + symbol.GoldenOfSymbolId + ".");
                else if (baseSymbol.Category != SymbolCategory.Regular || baseSymbol.IsGolden)
                {
                    errors.Add("Golden symbol " + symbol.Id + " must refer to a regular, non-golden symbol.");
                }
            }

            if (!hasScatter) errors.Add("No Scatter symbol is defined.");
            if (GoldenTransformEnabled && hasGolden && !hasWild) errors.Add("Golden transformation needs a Wild symbol.");

            for (int reel = 0; reel < Reels; reel++)
            {
                long total = 0;
                foreach (SymbolDefinition symbol in Symbols)
                {
                    if (symbol != null && symbol.ReelWeights != null && symbol.ReelWeights.Length == Reels && symbol.ReelWeights[reel] > 0)
                    {
                        total += symbol.ReelWeights[reel];
                    }
                }
                if (total <= 0) errors.Add("Reel " + (reel + 1) + " has no symbol weight.");
            }

            if (MaxMultiplier < 1) errors.Add("MaxMultiplier must be at least 1.");
            ValidateLadder(PaidMultiplierLadder, "Paid multiplier ladder", errors);
            ValidateLadder(FreeSpinMultiplierLadder, "Free-spin multiplier ladder", errors);

            if (MaxCascadesPerSpin < 1) errors.Add("MaxCascadesPerSpin must be at least 1.");
            if (MinScattersForBonus < 1 || MinScattersForBonus > Reels * Rows) errors.Add("MinScattersForBonus is out of range.");

            if (FreeSpinsAwarded == null || FreeSpinsAwarded.Length == 0) errors.Add("FreeSpinsAwarded is empty.");
            else foreach (int spins in FreeSpinsAwarded) if (spins <= 0) errors.Add("FreeSpinsAwarded values must be positive.");

            if (RetriggerFreeSpins < 0) errors.Add("RetriggerFreeSpins cannot be negative.");
            if (MaxTotalFreeSpinsPerRound < 1) errors.Add("MaxTotalFreeSpinsPerRound must be at least 1.");

            if (ScatterPayMilli == null) errors.Add("ScatterPayMilli is null.");
            else foreach (int pay in ScatterPayMilli) if (pay < 0) errors.Add("ScatterPayMilli cannot be negative.");

            return errors;
        }

        public void ValidateOrThrow()
        {
            List<string> errors = Validate();
            if (errors.Count > 0)
            {
                throw new InvalidOperationException("Invalid SlotConfig: " + string.Join(" | ", errors));
            }
        }

        private void ValidateLadder(int[] ladder, string label, List<string> errors)
        {
            if (ladder == null || ladder.Length == 0)
            {
                errors.Add(label + " is empty.");
                return;
            }

            foreach (int value in ladder)
            {
                if (value < 1) errors.Add(label + " contains a value below 1.");
                if (value > MaxMultiplier) errors.Add(label + " contains " + value + ", which is above MaxMultiplier " + MaxMultiplier + ".");
            }
        }
    }
}