using ScatterGame.Core;

namespace ScatterGame.Tests
{
    /// <summary>Shared fixed grids. Ids: 0 Ten, 1 Jack, 2 Queen, 3 King, 4 Ace, 5 Wild, 6 Scatter, 7 Golden Ten.</summary>
    internal static class TestGrids
    {
        public const int Ten = 0, Jack = 1, Queen = 2, King = 3, Ace = 4, Wild = 5, Scatter = 6, GoldenTen = 7;

        /// <summary>Every row is Ten Jack Queen King Ace, so each reel holds one symbol type. No symbol wins.</summary>
        public static SlotGrid NoWin()
        {
            return SlotGrid.FromRows(
                new[] { 0, 1, 2, 3, 4 },
                new[] { 0, 1, 2, 3, 4 },
                new[] { 0, 1, 2, 3, 4 },
                new[] { 0, 1, 2, 3, 4 });
        }

        public static SlotGrid SetColumn(SlotGrid grid, int reel, int symbolId)
        {
            for (int row = 0; row < SlotConfig.Rows; row++) grid.Set(reel, row, symbolId);
            return grid;
        }

        /// <summary>Ten on reels 1-3: 4x4x4 = 64 ways.</summary>
        public static SlotGrid ThreeTenReels()
        {
            SlotGrid grid = NoWin();
            SetColumn(grid, 0, Ten);
            SetColumn(grid, 1, Ten);
            SetColumn(grid, 2, Ten);
            return grid;
        }

        /// <summary>Three scatters, no regular win.</summary>
        public static SlotGrid ThreeScatters()
        {
            SlotGrid grid = NoWin();
            grid.Set(0, 0, Scatter);
            grid.Set(2, 1, Scatter);
            grid.Set(4, 2, Scatter);
            return grid;
        }

        /// <summary>Refill that creates no win after the Ten win: reel1 Jack, reel2 Queen, reel3 King.</summary>
        public static void EnqueueQuietRefill(ScriptedSymbolSource source)
        {
            source.Enqueue(0, 1, 1, 1, 1);
            source.Enqueue(1, 2, 2, 2, 2);
            source.Enqueue(2, 3, 3, 3, 3);
        }
    }

    /// <summary>Returns the same symbol forever. Every grid and refill is that symbol, so every cascade wins.</summary>
    internal sealed class ConstantSource : ISymbolSource
    {
        private readonly int symbolId;
        public ConstantSource(int symbolId) { this.symbolId = symbolId; }
        public int NextSymbol(int reel) { return symbolId; }
    }
}