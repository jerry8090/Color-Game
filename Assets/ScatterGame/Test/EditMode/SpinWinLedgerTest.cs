using System.Collections.Generic;
using NUnit.Framework;
using ScatterGame.Core;
using ScatterGame.Runtime;

namespace ScatterGame.Tests
{
    public class SpinWinLedgerTests
    {
        private SlotConfig config;

        [SetUp]
        public void SetUp() { config = DefaultSlotConfig.Create(); }

        private SpinResult Resolve(ISymbolSource source)
        {
            return SpinResolver.Resolve(config, 100, source, new MultiplierController(config.PaidMultiplierLadder, config.MaxMultiplier));
        }

        private static ScriptedSymbolSource TwoCascadeSource()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            source.Enqueue(0, 1, 1, 1, 1); // first refill: Jacks on reels 1-3, a second win
            source.Enqueue(1, 1, 1, 1, 1);
            source.Enqueue(2, 1, 1, 1, 1);
            source.Enqueue(0, 0, 0, 0, 0); // second refill: quiet
            source.Enqueue(1, 2, 2, 2, 2);
            source.Enqueue(2, 3, 3, 3, 3);
            return source;
        }

        [Test]
        public void NoWins_StaysAtZero()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.NoWin());
            SpinResult spin = Resolve(source);

            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);
            foreach (CascadeStep step in spin.Steps) ledger.AddCascade(step, 100);

            Assert.AreEqual(0, ledger.RoundTotalMilli);
            Assert.IsEmpty(ledger.Entries);
        }

        [Test]
        public void SingleWin_RecordsTheCombination()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            TestGrids.EnqueueQuietRefill(source);
            SpinResult spin = Resolve(source);

            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);
            List<WinEntry> added = ledger.AddCascade(spin.Steps[0], 100);

            Assert.AreEqual(1, added.Count);
            WinEntry entry = added[0];
            Assert.AreEqual(TestGrids.Ten, entry.SymbolId);
            Assert.AreEqual(3, entry.Reels);
            Assert.AreEqual(64, entry.Ways);
            Assert.AreEqual(1, entry.Multiplier);
            Assert.AreEqual(64000, entry.AmountMilli);
            Assert.AreEqual(64000, entry.RoundTotalMilli);
            Assert.AreEqual(64000, ledger.LastCascadeMilli);
        }

        [Test]
        public void TwoCascades_Accumulate_WithoutMultiplyingEarlierWinsAgain()
        {
            SpinResult spin = Resolve(TwoCascadeSource());
            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);

            WinEntry first = ledger.AddCascade(spin.Steps[0], 100)[0];
            WinEntry second = ledger.AddCascade(spin.Steps[1], 100)[0];

            Assert.AreEqual(64000, first.AmountMilli);            // x1
            Assert.AreEqual(153600, second.AmountMilli);          // x2, only this win
            Assert.AreEqual(2, second.Multiplier);
            Assert.AreEqual(64000, first.RoundTotalMilli);
            Assert.AreEqual(217600, second.RoundTotalMilli);      // 64,000 + 153,600
            Assert.AreEqual(64000, first.AmountMilli);            // the first win was never touched again
            Assert.AreEqual(spin.TotalWinMilli, ledger.RoundTotalMilli);
        }

        [Test]
        public void UpToTheTenTimesCap_EveryWinIsCalculatedOnce()
        {
            config.MaxCascadesPerSpin = 20;
            SpinResult spin = Resolve(new ConstantSource(TestGrids.Ten));

            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);

            long previous = 0;
            for (int i = 0; i < spin.Steps.Count; i++)
            {
                List<WinEntry> added = ledger.AddCascade(spin.Steps[i], 100);
                Assert.AreEqual(1, added.Count);
                Assert.AreEqual(spin.Steps[i].WinMilli, added[0].AmountMilli);
                Assert.AreEqual(previous + added[0].AmountMilli, added[0].RoundTotalMilli);
                Assert.LessOrEqual(added[0].Multiplier, 10);
                previous = added[0].RoundTotalMilli;
            }

            Assert.AreEqual(spin.CascadeWinMilli, ledger.RoundTotalMilli);
            Assert.AreEqual(10, ledger.Entries[ledger.Entries.Count - 1].Multiplier);
        }

        [Test]
        public void TheSameCascade_IsNeverAddedTwice()
        {
            SpinResult spin = Resolve(TwoCascadeSource());
            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);

            ledger.AddCascade(spin.Steps[0], 100);
            List<WinEntry> again = ledger.AddCascade(spin.Steps[0], 100);

            Assert.IsEmpty(again);
            Assert.AreEqual(64000, ledger.RoundTotalMilli);
            Assert.AreEqual(1, ledger.Entries.Count);
        }

        [Test]
        public void BeginRound_ResetsEverything()
        {
            SpinResult spin = Resolve(TwoCascadeSource());
            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);
            ledger.AddCascade(spin.Steps[0], 100);

            ledger.BeginRound();

            Assert.AreEqual(0, ledger.RoundTotalMilli);
            Assert.AreEqual(0, ledger.SpinTotalMilli);
            Assert.IsEmpty(ledger.Entries);

            ledger.BeginSpin(0, false);
            Assert.AreEqual(1, ledger.AddCascade(spin.Steps[0], 100).Count); // the same cascade may be recorded again in a NEW round
        }

        [Test]
        public void BeginSpin_RestartsTheSpinTotal_ButKeepsTheRoundTotal()
        {
            SpinResult spin = Resolve(TwoCascadeSource());
            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);
            ledger.AddCascade(spin.Steps[0], 100);

            ledger.BeginSpin(1, true);

            Assert.AreEqual(0, ledger.SpinTotalMilli);
            Assert.AreEqual(64000, ledger.RoundTotalMilli);
            Assert.IsTrue(ledger.IsFreeSpin);
        }

        [Test]
        public void FullBonusRound_MatchesTheEngineTotal_WithSeparateSpinAccounting()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            source.EnqueueGrid(TestGrids.ThreeTenReels());   // free spin 1 wins at the free-spin 2x multiplier
            TestGrids.EnqueueQuietRefill(source);
            for (int i = 0; i < 9; i++) source.EnqueueGrid(TestGrids.NoWin());

            RoundResult round = RoundResolver.Resolve(config, 100, source);

            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);
            foreach (CascadeStep step in round.PaidSpin.Steps) ledger.AddCascade(step, 100);

            long freeSpinOneTotal = -1;
            for (int i = 0; i < round.FreeSpins.Count; i++)
            {
                ledger.BeginSpin(i + 1, true);
                foreach (CascadeStep step in round.FreeSpins[i].Steps) ledger.AddCascade(step, 100);
                if (i == 0) freeSpinOneTotal = ledger.SpinTotalMilli;
            }

            Assert.AreEqual(round.TotalWinMilli, ledger.RoundTotalMilli);
            Assert.AreEqual(128000, freeSpinOneTotal);
            Assert.AreEqual(1, ledger.Entries.Count);
            Assert.AreEqual(1, ledger.Entries[0].SpinNumber);
            Assert.IsTrue(ledger.Entries[0].IsFreeSpin);
            Assert.AreEqual(2, ledger.Entries[0].Multiplier);
        }

        [Test]
        public void ScatterPay_IsAddedOnce_AndZeroIsIgnored()
        {
            SpinWinLedger ledger = new SpinWinLedger();
            ledger.BeginRound();
            ledger.BeginSpin(0, false);

            Assert.IsEmpty(ledger.AddScatterPay(TestGrids.Scatter, 0));
            Assert.AreEqual(1, ledger.AddScatterPay(TestGrids.Scatter, 200000).Count);
            Assert.IsEmpty(ledger.AddScatterPay(TestGrids.Scatter, 200000)); // duplicate event

            Assert.AreEqual(200000, ledger.RoundTotalMilli);
        }

        [Test]
        public void Credits_AreWrittenWithTwoDecimals_RoundedDown()
        {
            Assert.AreEqual("64.00", ScatterWinFormat.Credits(64000));
            Assert.AreEqual("0.01", ScatterWinFormat.Credits(10));
            Assert.AreEqual("0.00", ScatterWinFormat.Credits(5));
            Assert.AreEqual("0.00", ScatterWinFormat.Credits(-50));
            Assert.AreEqual("1,234.56", ScatterWinFormat.Credits(1234567));
            Assert.AreEqual("+0.20", ScatterWinFormat.Signed(200));
        }

        [Test]
        public void ComboText_ListsEveryCombination_AndTruncatesLongLists()
        {
            List<WinEntry> two = new List<WinEntry>
            {
                new WinEntry { SymbolId = 1, Reels = 3, AmountMilli = 10000 },
                new WinEntry { SymbolId = 2, Reels = 4, AmountMilli = 200000 }
            };
            System.Func<int, string> names = id => id == 1 ? "JACK" : "QUEEN";

            Assert.AreEqual("3x JACK +10.00   4x QUEEN +200.00", ScatterWinFormat.BuildComboText(two, names, 3));
            Assert.AreEqual(string.Empty, ScatterWinFormat.BuildComboText(null, names, 3));

            List<WinEntry> five = new List<WinEntry>();
            for (int i = 0; i < 5; i++) five.Add(new WinEntry { SymbolId = 1, Reels = 3, AmountMilli = 1000 });
            StringAssert.EndsWith("+2 MORE", ScatterWinFormat.BuildComboText(five, names, 3));
        }
    }
}