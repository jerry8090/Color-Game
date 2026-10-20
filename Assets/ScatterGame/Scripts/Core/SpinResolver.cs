using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    /// <summary>One winning cascade. The UI replays these to animate. All values are already final.</summary>
    public sealed class CascadeStep
    {
        public int Index;
        public int Multiplier;
        public SlotGrid GridBefore;
        public List<SymbolWin> Wins;
        public long BaseWinMilli;
        public long WinMilli;

        /// <summary>Cells that disappear.</summary>
        public List<GridPosition> Removed = new List<GridPosition>();

        /// <summary>Golden cells that stay and become Wild.</summary>
        public List<GridPosition> TransformedToWild = new List<GridPosition>();

        /// <summary>Cells filled with new symbols after the fall (in GridAfter).</summary>
        public List<GridPosition> NewSymbols = new List<GridPosition>();

        public SlotGrid GridAfter;
    }

    public sealed class SpinResult
    {
        public SlotGrid InitialGrid;
        public List<CascadeStep> Steps = new List<CascadeStep>();
        public int ScatterCount;
        public long ScatterWinMilli;
        public bool HitCascadeLimit;

        public long CascadeWinMilli
        {
            get
            {
                long total = 0;
                foreach (CascadeStep step in Steps) total += step.WinMilli;
                return total;
            }
        }

        public long TotalWinMilli => CascadeWinMilli + ScatterWinMilli;

        public int MaxMultiplier
        {
            get
            {
                int max = 0;
                foreach (CascadeStep step in Steps) if (step.Multiplier > max) max = step.Multiplier;
                return max;
            }
        }
    }

    /// <summary>
    /// Resolves one spin completely and instantly: starting grid, then cascades until no win remains
    /// (or the safety limit is hit). No animation, no wallet. Deterministic for a given symbol source.
    /// </summary>
    public static class SpinResolver
    {
        public static SpinResult Resolve(SlotConfig config, long totalBet, ISymbolSource source, MultiplierController multiplier)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (multiplier == null) throw new ArgumentNullException(nameof(multiplier));
            if (totalBet <= 0) throw new ArgumentException("The bet must be greater than zero.");

            SlotGrid grid = GridGenerator.Generate(source);

            SpinResult result = new SpinResult { InitialGrid = grid.Clone() };
            result.ScatterCount = WinEvaluator.CountScatters(config, grid);
            result.ScatterWinMilli = (long)config.ScatterPayMilliFor(result.ScatterCount) * totalBet;

            int wildId = config.WildSymbolId;

            while (true)
            {
                EvaluationResult evaluation = WinEvaluator.Evaluate(config, grid);
                if (!evaluation.HasWin) break;

                if (result.Steps.Count >= config.MaxCascadesPerSpin)
                {
                    result.HitCascadeLimit = true;
                    break;
                }

                int applied = multiplier.RegisterWin();

                long baseWin = 0;
                foreach (SymbolWin win in evaluation.Wins) baseWin += win.BaseWinMilli(totalBet);

                CascadeStep step = new CascadeStep
                {
                    Index = result.Steps.Count,
                    Multiplier = applied,
                    GridBefore = grid.Clone(),
                    Wins = evaluation.Wins,
                    BaseWinMilli = baseWin,
                    WinMilli = baseWin * applied // the multiplier is applied exactly once, to this cascade only
                };

                // Every cell used by any win (a Wild can be shared by several wins, so collect without duplicates).
                HashSet<GridPosition> winning = new HashSet<GridPosition>();
                foreach (SymbolWin win in evaluation.Wins)
                {
                    foreach (GridPosition position in win.Positions) winning.Add(position);
                }

                List<GridPosition> ordered = new List<GridPosition>(winning);
                ordered.Sort((a, b) => a.Reel != b.Reel ? a.Reel.CompareTo(b.Reel) : a.Row.CompareTo(b.Row));

                bool[,] removedFlags = new bool[SlotConfig.Reels, SlotConfig.Rows];
                foreach (GridPosition position in ordered)
                {
                    SymbolDefinition symbol = config.GetSymbol(grid.Get(position));
                    if (config.GoldenTransformEnabled && symbol.IsGolden && wildId >= 0)
                    {
                        // The payout above is already settled. The cell only changes for the next evaluation.
                        grid.Set(position, wildId);
                        step.TransformedToWild.Add(position);
                    }
                    else
                    {
                        removedFlags[position.Reel, position.Row] = true;
                        step.Removed.Add(position);
                    }
                }

                ApplyGravityAndRefill(grid, removedFlags, source, step.NewSymbols);
                step.GridAfter = grid.Clone();
                result.Steps.Add(step);
            }

            return result;
        }

        // Survivors keep their order and fall to the bottom. New symbols fill the top of each reel.
        private static void ApplyGravityAndRefill(SlotGrid grid, bool[,] removed, ISymbolSource source, List<GridPosition> newSymbols)
        {
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                List<int> survivors = new List<int>();
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    if (!removed[reel, row]) survivors.Add(grid.Get(reel, row));
                }

                int empty = SlotConfig.Rows - survivors.Count;

                for (int row = 0; row < empty; row++)
                {
                    grid.Set(reel, row, source.NextSymbol(reel));
                    newSymbols.Add(new GridPosition(reel, row));
                }

                for (int i = 0; i < survivors.Count; i++) grid.Set(reel, empty + i, survivors[i]);
            }
        }
    }
}