using NUnit.Framework;
using ScatterGame.Core;

namespace ScatterGame.Tests
{
    public class CascadeTests
    {
        private SlotConfig config;

        [SetUp]
        public void SetUp() { config = DefaultSlotConfig.Create(); }

        private SpinResult Spin(ScriptedSymbolSource source, long bet = 100)
        {
            MultiplierController multiplier = new MultiplierController(config.PaidMultiplierLadder, config.MaxMultiplier);
            return SpinResolver.Resolve(config, bet, source, multiplier);
        }

        [Test]
        public void NoWin_ProducesNoSteps()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.NoWin());
            SpinResult result = Spin(source);
            Assert.AreEqual(0, result.Steps.Count);
            Assert.AreEqual(0, result.TotalWinMilli);
        }

        [Test]
        public void SingleWin_RemovesCells_RefillsCorrectCells_AndPaysOnce()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            TestGrids.EnqueueQuietRefill(source);

            SpinResult result = Spin(source);

            Assert.AreEqual(1, result.Steps.Count);
            CascadeStep step = result.Steps[0];
            Assert.AreEqual(1, step.Multiplier);
            Assert.AreEqual(12, step.Removed.Count);
            Assert.AreEqual(12, step.NewSymbols.Count);
            Assert.AreEqual(64000, step.WinMilli);
            Assert.AreEqual(64000, result.TotalWinMilli);
            for (int row = 0; row < 4; row++)
            {
                Assert.AreEqual(1, step.GridAfter.Get(0, row));
                Assert.AreEqual(2, step.GridAfter.Get(1, row));
                Assert.AreEqual(3, step.GridAfter.Get(2, row));
                Assert.AreEqual(3, step.GridAfter.Get(3, row)); // untouched reels stay
                Assert.AreEqual(4, step.GridAfter.Get(4, row));
            }
            Assert.AreEqual(0, source.Remaining);
        }

        [Test]
        public void Cascade_ContinuesWhileWinsRemain_AndMultiplierAppliesPerCascade()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            // First refill: all Jack on reels 1-3 -> a second win.
            source.Enqueue(0, 1, 1, 1, 1);
            source.Enqueue(1, 1, 1, 1, 1);
            source.Enqueue(2, 1, 1, 1, 1);
            // Second refill: quiet (Ten, Queen, King).
            source.Enqueue(0, 0, 0, 0, 0);
            source.Enqueue(1, 2, 2, 2, 2);
            source.Enqueue(2, 3, 3, 3, 3);

            SpinResult result = Spin(source);

            Assert.AreEqual(2, result.Steps.Count);
            Assert.AreEqual(1, result.Steps[0].Multiplier);
            Assert.AreEqual(2, result.Steps[1].Multiplier);
            Assert.AreEqual(64000, result.Steps[0].WinMilli);          // 10 x 64 x 100 x 1
            Assert.AreEqual(12L * 64 * 100 * 2, result.Steps[1].WinMilli); // 153,600
            Assert.AreEqual(217600, result.TotalWinMilli);             // each cascade counted once, never twice
            Assert.IsFalse(result.HitCascadeLimit);
        }

        [Test]
        public void GoldenSymbol_BecomesWild_AndStays_ForNextCascade()
        {
            SlotGrid grid = TestGrids.NoWin();
            TestGrids.SetColumn(grid, 0, TestGrids.GoldenTen);
            TestGrids.SetColumn(grid, 1, TestGrids.Ten);
            TestGrids.SetColumn(grid, 2, TestGrids.Ten);

            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(grid);
            source.Enqueue(1, 1, 1, 1, 1, 2, 2, 2, 2); // reel 2: Jack refill, then quiet refill
            source.Enqueue(2, 1, 1, 1, 1, 3, 3, 3, 3);
            source.Enqueue(0, 1, 1, 1, 1);             // reel 1 is only refilled in cascade 2

            SpinResult result = Spin(source);

            CascadeStep first = result.Steps[0];
            Assert.AreEqual(4, first.TransformedToWild.Count);
            Assert.AreEqual(8, first.Removed.Count);
            Assert.AreEqual(64000, first.WinMilli); // paid as Ten, before the transformation
            for (int row = 0; row < 4; row++) Assert.AreEqual(TestGrids.Wild, first.GridAfter.Get(0, row));

            Assert.AreEqual(2, result.Steps.Count);
            Assert.AreEqual(TestGrids.Jack, result.Steps[1].Wins[0].SymbolId); // the new Wild helps a Jack win
            Assert.AreEqual(2, result.Steps[1].Multiplier);
        }

        [Test]
        public void CascadeLimit_StopsEndlessChains()
        {
            config.MaxCascadesPerSpin = 3;
            MultiplierController multiplier = new MultiplierController(config.PaidMultiplierLadder, config.MaxMultiplier);
            SpinResult result = SpinResolver.Resolve(config, 100, new ConstantSource(TestGrids.Ten), multiplier);
            Assert.AreEqual(3, result.Steps.Count);
            Assert.IsTrue(result.HitCascadeLimit);
        }

        [Test]
        public void Multiplier_ReachesTenAndNeverExceedsIt_InCascades()
        {
            config.MaxCascadesPerSpin = 20;
            MultiplierController multiplier = new MultiplierController(config.PaidMultiplierLadder, config.MaxMultiplier);
            SpinResult result = SpinResolver.Resolve(config, 100, new ConstantSource(TestGrids.Ten), multiplier);
            Assert.AreEqual(10, result.Steps[5].Multiplier);
            foreach (CascadeStep step in result.Steps) Assert.LessOrEqual(step.Multiplier, 10);
            Assert.AreEqual(10, result.MaxMultiplier);
        }

        [Test]
        public void SameSeed_GivesSameResult()
        {
            RoundResult a = RoundResolver.Resolve(config, 100, new WeightedSymbolSource(config, 777));
            RoundResult b = RoundResolver.Resolve(config, 100, new WeightedSymbolSource(config, 777));
            Assert.AreEqual(a.TotalWinMilli, b.TotalWinMilli);
            Assert.IsTrue(a.PaidSpin.InitialGrid.SameAs(b.PaidSpin.InitialGrid));
        }
    }
}