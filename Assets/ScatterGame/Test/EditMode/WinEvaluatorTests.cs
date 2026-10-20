using NUnit.Framework;
using ScatterGame.Core;

namespace ScatterGame.Tests
{
    public class WinEvaluatorTests
    {
        private SlotConfig config;

        [SetUp]
        public void SetUp() { config = DefaultSlotConfig.Create(); }

        [Test]
        public void Grid_HasTwentyCells()
        {
            Assert.AreEqual(20, new SlotGrid().CellCount);
            SlotGrid generated = GridGenerator.Generate(new WeightedSymbolSource(config, 1));
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
                for (int row = 0; row < SlotConfig.Rows; row++)
                    Assert.IsTrue(config.TryGetSymbol(generated.Get(reel, row), out _));
        }

        [Test]
        public void NoMatchingCombination_NoWin()
        {
            Assert.IsFalse(WinEvaluator.Evaluate(config, TestGrids.NoWin()).HasWin);
        }

        [Test]
        public void TwoReels_AreNotEnough()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.Ten);
            TestGrids.SetColumn(grid, 1, TestGrids.Ten);
            Assert.IsFalse(WinEvaluator.Evaluate(config, grid).HasWin);
        }

        [Test]
        public void ThreeReels_64Ways_CorrectPayout()
        {
            EvaluationResult result = WinEvaluator.Evaluate(config, TestGrids.ThreeTenReels());
            Assert.AreEqual(1, result.Wins.Count);
            SymbolWin win = result.Wins[0];
            Assert.AreEqual(TestGrids.Ten, win.SymbolId);
            Assert.AreEqual(3, win.ReelsMatched);
            Assert.AreEqual(64, win.Ways);
            Assert.AreEqual(12, win.Positions.Count);
            Assert.AreEqual(10L * 64 * 100, win.BaseWinMilli(100)); // 64,000 milli = 64 credits at bet 100
        }

        [Test]
        public void FourReels_256Ways()
        {
            SlotGrid grid = TestGrids.ThreeTenReels();
            TestGrids.SetColumn(grid, 3, TestGrids.Ten);
            SymbolWin win = WinEvaluator.Evaluate(config, grid).Wins[0];
            Assert.AreEqual(4, win.ReelsMatched);
            Assert.AreEqual(256, win.Ways);
            Assert.AreEqual(30L * 256 * 100, win.BaseWinMilli(100));
        }

        [Test]
        public void FiveReels_1024Ways()
        {
            SlotGrid grid = TestGrids.ThreeTenReels();
            TestGrids.SetColumn(grid, 3, TestGrids.Ten);
            TestGrids.SetColumn(grid, 4, TestGrids.Ten);
            SymbolWin win = WinEvaluator.Evaluate(config, grid).Wins[0];
            Assert.AreEqual(5, win.ReelsMatched);
            Assert.AreEqual(1024, win.Ways);
            Assert.AreEqual(80L * 1024 * 10, win.BaseWinMilli(10));
        }

        [Test]
        public void Wild_SubstitutesForRegularSymbol()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.Ten);
            TestGrids.SetColumn(grid, 1, TestGrids.Wild);
            TestGrids.SetColumn(grid, 2, TestGrids.Ten);
            SymbolWin win = WinEvaluator.Evaluate(config, grid).Wins[0];
            Assert.AreEqual(3, win.ReelsMatched);
            Assert.AreEqual(64, win.Ways);
        }

        [Test]
        public void Wild_CountsOnlyCellsThatMatch_PartialReels()
        {
            SlotGrid grid = TestGrids.NoWin();
            // Reel 1: Ten, Jack, Jack, Jack. Reel 2: Wild, King, King, King. Reel 3: Ten, Queen, Queen, Queen.
            grid.Set(0, 0, TestGrids.Ten); grid.Set(0, 1, 1); grid.Set(0, 2, 1); grid.Set(0, 3, 1);
            grid.Set(1, 0, TestGrids.Wild); grid.Set(1, 1, 3); grid.Set(1, 2, 3); grid.Set(1, 3, 3);
            grid.Set(2, 0, TestGrids.Ten); grid.Set(2, 1, 2); grid.Set(2, 2, 2); grid.Set(2, 3, 2);
            EvaluationResult result = WinEvaluator.Evaluate(config, grid);
            SymbolWin ten = result.Wins.Find(w => w.SymbolId == TestGrids.Ten);
            Assert.IsNotNull(ten);
            Assert.AreEqual(1, ten.Ways);
            Assert.AreEqual(3, ten.Positions.Count);
        }

        [Test]
        public void Wild_DoesNotSubstitute_WhenSymbolForbidsIt()
        {
            config.GetSymbol(TestGrids.Ten).WildCanSubstitute = false;
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.Ten);
            TestGrids.SetColumn(grid, 1, TestGrids.Wild);
            TestGrids.SetColumn(grid, 2, TestGrids.Ten);
            Assert.IsFalse(WinEvaluator.Evaluate(config, grid).HasWin);
        }

        [Test]
        public void Wild_NeverSubstitutesForScatter()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.Wild);
            TestGrids.SetColumn(grid, 1, TestGrids.Wild);
            TestGrids.SetColumn(grid, 2, TestGrids.Wild);
            Assert.AreEqual(0, WinEvaluator.CountScatters(config, grid));
        }

        [Test]
        public void Scatter_IsNotPaidAsRegularSymbol_AndBreaksChains()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.Scatter);
            TestGrids.SetColumn(grid, 1, TestGrids.Scatter);
            TestGrids.SetColumn(grid, 2, TestGrids.Scatter);
            Assert.IsFalse(WinEvaluator.Evaluate(config, grid).HasWin);
            Assert.AreEqual(12, WinEvaluator.CountScatters(config, grid));
        }

        [Test]
        public void GoldenSymbol_MatchesItsBaseSymbol()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.GoldenTen);
            TestGrids.SetColumn(grid, 1, TestGrids.Ten);
            TestGrids.SetColumn(grid, 2, TestGrids.GoldenTen);
            SymbolWin win = WinEvaluator.Evaluate(config, grid).Wins[0];
            Assert.AreEqual(TestGrids.Ten, win.SymbolId);
            Assert.AreEqual(64, win.Ways);
        }

        [Test]
        public void MultipleSimultaneousWins_EachCountedOnce()
        {
            SlotGrid grid = SlotGrid.FromRows(
                new[] { 0, 0, 0, 4, 4 },
                new[] { 1, 1, 1, 4, 4 },
                new[] { 2, 2, 2, 4, 4 },
                new[] { 3, 3, 3, 4, 4 });
            EvaluationResult result = WinEvaluator.Evaluate(config, grid);
            Assert.AreEqual(4, result.Wins.Count);
            long total = 0;
            foreach (SymbolWin win in result.Wins) { Assert.AreEqual(1, win.Ways); total += win.BaseWinMilli(100); }
            Assert.AreEqual((10 + 12 + 16 + 20) * 100L, total);
        }

        [Test]
        public void InvalidConfigurations_AreReported()
        {
            Assert.IsEmpty(config.Validate());

            SlotConfig badLadder = DefaultSlotConfig.Create();
            badLadder.PaidMultiplierLadder = new[] { 1, 11 };
            Assert.IsNotEmpty(badLadder.Validate());

            SlotConfig duplicate = DefaultSlotConfig.Create();
            duplicate.GetSymbol(1).Id = 0;
            Assert.IsNotEmpty(duplicate.Validate());

            SlotConfig noWeight = DefaultSlotConfig.Create();
            foreach (SymbolDefinition symbol in noWeight.Symbols) symbol.ReelWeights[2] = 0;
            Assert.IsNotEmpty(noWeight.Validate());

            SlotConfig noScatter = DefaultSlotConfig.Create();
            noScatter.Symbols.RemoveAll(s => s.IsScatter);
            Assert.IsNotEmpty(noScatter.Validate());

            SlotConfig badGolden = DefaultSlotConfig.Create();
            badGolden.GetSymbol(7).GoldenOfSymbolId = 99;
            Assert.IsNotEmpty(badGolden.Validate());

            SlotConfig badWeights = DefaultSlotConfig.Create();
            badWeights.GetSymbol(0).ReelWeights = new[] { 1, 1 };
            Assert.IsNotEmpty(badWeights.Validate());
        }
    }
}