using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    public sealed class SymbolWin
    {
        public int SymbolId;
        public int ReelsMatched;
        public long Ways;
        public int PayMilli;
        public List<GridPosition> Positions = new List<GridPosition>();

        /// <summary>Win before the cascade multiplier, in milli-credits.</summary>
        public long BaseWinMilli(long totalBet)
        {
            return (long)PayMilli * Ways * totalBet;
        }
    }

    public sealed class EvaluationResult
    {
        public readonly List<SymbolWin> Wins = new List<SymbolWin>();
        public bool HasWin => Wins.Count > 0;
    }

    /// <summary>
    /// 1,024-ways evaluator for 5x4. Pure logic, no UI.
    /// For each regular symbol: starting at reel 1, every consecutive reel must contain the symbol, its golden variant,
    /// or a Wild (if that symbol allows it). Ways = product of the matching cell counts per reel.
    /// Needs 3 or more reels. Scatters are never part of this evaluation.
    /// </summary>
    public static class WinEvaluator
    {
        public static EvaluationResult Evaluate(SlotConfig config, SlotGrid grid)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (grid == null) throw new ArgumentNullException(nameof(grid));

            EvaluationResult result = new EvaluationResult();

            foreach (SymbolDefinition symbol in config.BaseRegularSymbols())
            {
                long ways = 1;
                int reelsMatched = 0;
                List<GridPosition> positions = new List<GridPosition>();

                for (int reel = 0; reel < SlotConfig.Reels; reel++)
                {
                    int count = 0;
                    for (int row = 0; row < SlotConfig.Rows; row++)
                    {
                        if (Matches(config, grid.Get(reel, row), symbol))
                        {
                            positions.Add(new GridPosition(reel, row));
                            count++;
                        }
                    }

                    if (count == 0) break; // the chain must be unbroken from reel 1
                    ways *= count;
                    reelsMatched++;
                }

                if (reelsMatched < SlotConfig.MinWinReels) continue;

                int pay = symbol.PayMilli[reelsMatched - SlotConfig.MinWinReels];
                if (pay <= 0) continue;

                result.Wins.Add(new SymbolWin
                {
                    SymbolId = symbol.Id,
                    ReelsMatched = reelsMatched,
                    Ways = ways,
                    PayMilli = pay,
                    Positions = positions
                });
            }

            return result;
        }

        public static int CountScatters(SlotConfig config, SlotGrid grid)
        {
            int count = 0;
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    if (config.GetSymbol(grid.Get(reel, row)).IsScatter) count++;
                }
            }
            return count;
        }

        private static bool Matches(SlotConfig config, int cellId, SymbolDefinition target)
        {
            if (cellId == target.Id) return true;

            SymbolDefinition cell = config.GetSymbol(cellId);
            if (cell.IsGolden) return cell.GoldenOfSymbolId == target.Id;
            return cell.IsWild && target.WildCanSubstitute;
        }
    }
}