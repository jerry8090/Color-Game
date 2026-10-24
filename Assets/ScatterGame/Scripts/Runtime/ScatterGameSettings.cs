using System;
using System.Collections.Generic;
using UnityEngine;
using ScatterGame.Core;

namespace ScatterGame.Runtime
{
    [Serializable]
    public class SymbolEntry
    {
        public int id;
        public string displayName;
        public SymbolCategory category;
        public Sprite sprite;

        [Tooltip("Generation weight on reels 1 to 5. 0 = never lands on that reel.")]
        public int[] reelWeights = new int[SlotConfig.Reels];

        [Tooltip("Pay per way, in thousandths of the total bet, for 3, 4 and 5 reels. Regular, non-golden symbols only.")]
        public int[] payMilli = new int[SlotConfig.PayTiers];

        [Tooltip("-1 for normal symbols. For a golden symbol: the id of the regular symbol it pays as.")]
        public int goldenOfSymbolId = -1;

        [Tooltip("Regular symbols only: may a Wild stand in for this symbol?")]
        public bool wildCanSubstitute = true;

        [Tooltip("Optional sound for this symbol when it is part of a win.")]
        public AudioClip winSound;
    }

    /// <summary>
    /// Every tunable rule of Ace Scatter, editable in the Inspector.
    /// BuildSlotConfig() turns it into the plain SlotConfig the engine uses.
    /// All numbers are prototype values. They are not the original game's numbers.
    /// </summary>
    [CreateAssetMenu(fileName = "ScatterGameSettings", menuName = "Scatter Game/Settings")]
    public class ScatterGameSettings : ScriptableObject
    {
        [Header("Symbols")]
        [SerializeField] private List<SymbolEntry> symbols = new List<SymbolEntry>();

        [Header("Multipliers (win = pay x ways x bet x multiplier)")]
        [SerializeField] private int maxMultiplier = 10;
        [SerializeField] private int[] paidMultiplierLadder = { 1, 2, 3, 5, 7, 10 };
        [SerializeField] private int[] freeSpinMultiplierLadder = { 2, 4, 6, 10 };
        [SerializeField] private bool freeSpinMultiplierPersists = false;

        [Header("Cascades")]
        [SerializeField] private int maxCascadesPerSpin = 50;
        [SerializeField] private bool goldenTransformEnabled = true;

        [Header("Scatter bonus")]
        [SerializeField] private int minScattersForBonus = 3;
        [Tooltip("Free spins for 3, 4, 5+ scatters (the last value is used for any higher count).")]
        [SerializeField] private int[] freeSpinsAwarded = { 10, 12, 15 };
        [SerializeField] private int retriggerFreeSpins = 5;
        [SerializeField] private int maxTotalFreeSpinsPerRound = 30;
        [Tooltip("Optional scatter pay in thousandths of the total bet. Same tiers as free spins. Not multiplied.")]
        [SerializeField] private int[] scatterPayMilli = { 0, 0, 0 };

        [Header("Betting (virtual credits)")]
        [SerializeField] private int[] betAmounts = { 10, 20, 50, 100, 200, 500 };
        [SerializeField] private int defaultBetIndex = 0;

        public IReadOnlyList<int> BetAmounts => betAmounts;
        public int DefaultBetIndex => Mathf.Clamp(defaultBetIndex, 0, Mathf.Max(0, betAmounts.Length - 1));
        public IReadOnlyList<SymbolEntry> Symbols => symbols;

        public SymbolEntry GetEntry(int id)
        {
            foreach (SymbolEntry entry in symbols)
            {
                if (entry != null && entry.id == id) return entry;
            }
            return null;
        }

        public SlotConfig BuildSlotConfig()
        {
            SlotConfig config = new SlotConfig
            {
                MaxMultiplier = maxMultiplier,
                PaidMultiplierLadder = Copy(paidMultiplierLadder),
                FreeSpinMultiplierLadder = Copy(freeSpinMultiplierLadder),
                FreeSpinMultiplierPersists = freeSpinMultiplierPersists,
                MaxCascadesPerSpin = maxCascadesPerSpin,
                GoldenTransformEnabled = goldenTransformEnabled,
                MinScattersForBonus = minScattersForBonus,
                FreeSpinsAwarded = Copy(freeSpinsAwarded),
                RetriggerFreeSpins = retriggerFreeSpins,
                MaxTotalFreeSpinsPerRound = maxTotalFreeSpinsPerRound,
                ScatterPayMilli = Copy(scatterPayMilli)
            };

            foreach (SymbolEntry entry in symbols)
            {
                if (entry == null) { config.Symbols.Add(null); continue; }

                config.Symbols.Add(new SymbolDefinition
                {
                    Id = entry.id,
                    Name = entry.displayName,
                    Category = entry.category,
                    ReelWeights = Copy(entry.reelWeights),
                    PayMilli = Copy(entry.payMilli),
                    GoldenOfSymbolId = entry.goldenOfSymbolId,
                    WildCanSubstitute = entry.wildCanSubstitute
                });
            }

            return config;
        }

        /// <summary>Everything wrong with these settings. An empty list means they are usable.</summary>
        public List<string> GetProblems()
        {
            List<string> problems = BuildSlotConfig().Validate();

            if (betAmounts == null || betAmounts.Length == 0)
            {
                problems.Add("No bet amounts are defined.");
            }
            else
            {
                HashSet<int> seen = new HashSet<int>();
                foreach (int bet in betAmounts)
                {
                    if (bet <= 0) problems.Add("Bet amounts must be positive.");
                    else if (!seen.Add(bet)) problems.Add("Bet amount " + bet + " is listed twice.");
                }
                if (defaultBetIndex < 0 || defaultBetIndex >= betAmounts.Length) problems.Add("Default Bet Index is out of range.");
            }

            return problems;
        }

        /// <summary>Fills the asset from a SlotConfig (used by the editor menu that creates the default asset).</summary>
        public void CopyFrom(SlotConfig config, int[] bets)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            maxMultiplier = config.MaxMultiplier;
            paidMultiplierLadder = Copy(config.PaidMultiplierLadder);
            freeSpinMultiplierLadder = Copy(config.FreeSpinMultiplierLadder);
            freeSpinMultiplierPersists = config.FreeSpinMultiplierPersists;
            maxCascadesPerSpin = config.MaxCascadesPerSpin;
            goldenTransformEnabled = config.GoldenTransformEnabled;
            minScattersForBonus = config.MinScattersForBonus;
            freeSpinsAwarded = Copy(config.FreeSpinsAwarded);
            retriggerFreeSpins = config.RetriggerFreeSpins;
            maxTotalFreeSpinsPerRound = config.MaxTotalFreeSpinsPerRound;
            scatterPayMilli = Copy(config.ScatterPayMilli);
            betAmounts = Copy(bets);
            defaultBetIndex = 0;

            symbols = new List<SymbolEntry>();
            foreach (SymbolDefinition symbol in config.Symbols)
            {
                symbols.Add(new SymbolEntry
                {
                    id = symbol.Id,
                    displayName = symbol.Name,
                    category = symbol.Category,
                    reelWeights = Copy(symbol.ReelWeights),
                    payMilli = Copy(symbol.PayMilli),
                    goldenOfSymbolId = symbol.GoldenOfSymbolId,
                    wildCanSubstitute = symbol.WildCanSubstitute
                });
            }
        }

        private void OnValidate()
        {
            // Keep the Inspector arrays the right length so a typo cannot break the engine.
            foreach (SymbolEntry entry in symbols)
            {
                if (entry == null) continue;
                if (entry.reelWeights == null || entry.reelWeights.Length != SlotConfig.Reels) Array.Resize(ref entry.reelWeights, SlotConfig.Reels);
                if (entry.payMilli == null || entry.payMilli.Length != SlotConfig.PayTiers) Array.Resize(ref entry.payMilli, SlotConfig.PayTiers);
            }

            List<string> problems = GetProblems();
            if (problems.Count > 0)
            {
                Debug.LogWarning("[ScatterGameSettings] " + name + ": " + string.Join(" | ", problems), this);
            }
        }

        private static int[] Copy(int[] source)
        {
            return source == null ? null : (int[])source.Clone();
        }
    }
}