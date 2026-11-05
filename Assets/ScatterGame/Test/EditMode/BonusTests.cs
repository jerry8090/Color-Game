using NUnit.Framework;
using ScatterGame.Core;

namespace ScatterGame.Tests
{
    public class BonusTests
    {
        private SlotConfig config;

        [SetUp]
        public void SetUp() { config = DefaultSlotConfig.Create(); }

        private static void QueueNoWin(ScriptedSymbolSource source, int count)
        {
            for (int i = 0; i < count; i++) source.EnqueueGrid(TestGrids.NoWin());
        }

        private static void QueueTenWinWithQuietRefill(ScriptedSymbolSource source)
        {
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            TestGrids.EnqueueQuietRefill(source);
        }

        [Test]
        public void FreeSpinTable_FollowsConfig()
        {
            Assert.AreEqual(0, config.FreeSpinsFor(2));
            Assert.AreEqual(10, config.FreeSpinsFor(3));
            Assert.AreEqual(12, config.FreeSpinsFor(4));
            Assert.AreEqual(15, config.FreeSpinsFor(5));
            Assert.AreEqual(15, config.FreeSpinsFor(8));
        }

        [Test]
        public void NormalGrid_DoesNotTriggerBonus()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.NoWin());
            RoundResult round = RoundResolver.Resolve(config, 100, source);
            Assert.IsFalse(round.BonusTriggered);
            Assert.AreEqual(0, round.FreeSpins.Count);
        }

        [Test]
        public void ThreeScatters_TriggerTenFreeSpins_AllConsumed()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            QueueNoWin(source, 10);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.IsTrue(round.BonusTriggered);
            Assert.AreEqual(3, round.PaidSpin.ScatterCount);
            Assert.AreEqual(10, round.FreeSpinsAwardedInitial);
            Assert.AreEqual(10, round.FreeSpins.Count);
            Assert.AreEqual(10, round.TotalFreeSpinsAwarded);
            Assert.AreEqual(0, round.TotalWinCredits);
            Assert.AreEqual(-100, round.NetCredits); // the paid bet only: free spins add no further cost
            Assert.AreEqual(0, source.Remaining);
        }

        [Test]
        public void Retrigger_AddsConfiguredSpins()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            source.EnqueueGrid(TestGrids.ThreeScatters()); // free spin 1 retriggers
            QueueNoWin(source, 14);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(1, round.FreeSpinRetriggers);
            Assert.AreEqual(15, round.TotalFreeSpinsAwarded);
            Assert.AreEqual(15, round.FreeSpins.Count);
        }

        [Test]
        public void Retrigger_IsCapped_AndEnds()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            for (int i = 0; i < 31; i++) source.EnqueueGrid(TestGrids.ThreeScatters());

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(30, round.TotalFreeSpinsAwarded);
            Assert.AreEqual(30, round.FreeSpins.Count);
        }

        [Test]
        public void FreeSpin_UsesFreeSpinLadder_AndAccumulatesIntoRoundTotal()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            QueueTenWinWithQuietRefill(source); // free spin 1: one win
            QueueNoWin(source, 9);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(2, round.FreeSpins[0].Steps[0].Multiplier);   // free ladder starts at 2x
            Assert.AreEqual(128000, round.FreeSpins[0].TotalWinMilli);    // 10 x 64 x 100 x 2
            Assert.AreEqual(128, round.TotalWinCredits);
            Assert.AreEqual(2, round.MaxMultiplier);
        }

        [Test]
        public void FreeSpinMultiplier_ResetsEachSpin_ByDefault()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            QueueTenWinWithQuietRefill(source);
            QueueTenWinWithQuietRefill(source);
            QueueNoWin(source, 8);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(2, round.FreeSpins[0].Steps[0].Multiplier);
            Assert.AreEqual(2, round.FreeSpins[1].Steps[0].Multiplier);
        }

        [Test]
        public void FreeSpinMultiplier_Persists_WhenConfigured()
        {
            config.FreeSpinMultiplierPersists = true;
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            QueueTenWinWithQuietRefill(source);
            QueueTenWinWithQuietRefill(source);
            QueueNoWin(source, 8);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(2, round.FreeSpins[0].Steps[0].Multiplier);
            Assert.AreEqual(4, round.FreeSpins[1].Steps[0].Multiplier);
        }

        [Test]
        public void ScatterPay_IsPaidOnce_AndNotMultiplied()
        {
            config.ScatterPayMilli = new[] { 2000, 5000, 10000 };
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            QueueNoWin(source, 10);

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            Assert.AreEqual(200, round.TotalWinCredits); // 2.0 x bet
        }

        [Test]
        public void Settlement_RoundsDownToWholeCreditsOnce()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            QueueTenWinWithQuietRefill(source);
            RoundResult round = RoundResolver.Resolve(config, 1, source);
            Assert.AreEqual(640, round.TotalWinMilli);
            Assert.AreEqual(0, round.TotalWinCredits);

            source = new ScriptedSymbolSource();
            QueueTenWinWithQuietRefill(source);
            round = RoundResolver.Resolve(config, 3, source);
            Assert.AreEqual(1920, round.TotalWinMilli);
            Assert.AreEqual(1, round.TotalWinCredits);
        }

        [Test]
        public void InvalidInput_IsRejected()
        {
            Assert.Throws<System.ArgumentException>(() => RoundResolver.Resolve(config, 0, new ConstantSource(0)));
            SlotConfig bad = DefaultSlotConfig.Create();
            bad.PaidMultiplierLadder = new[] { 1, 11 };
            Assert.Throws<System.InvalidOperationException>(() => RoundResolver.Resolve(bad, 100, new ConstantSource(0)));
        }
    }
}