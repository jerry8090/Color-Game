using System;
using System.Collections.Generic;
using NUnit.Framework;
using ScatterGame.Core;

namespace ScatterGame.Tests
{
    internal sealed class FakeWallet : IScatterWallet
    {
        public long Balance;
        public bool RefuseSpend;
        public bool RefuseCredit;
        public int SpendCalls;
        public int CreditCalls;
        public long LastSpend;
        public long LastCredit;
        public readonly List<string> Calls = new List<string>();

        public FakeWallet(long balance) { Balance = balance; }

        public long GetBalance() { return Balance; }

        public bool TrySpend(long amount, string roundId)
        {
            if (RefuseSpend || amount <= 0 || Balance < amount) return false;
            Balance -= amount; SpendCalls++; LastSpend = amount; Calls.Add("spend:" + roundId);
            return true;
        }

        public bool TryCredit(long amount, string roundId)
        {
            if (RefuseCredit || amount <= 0) return false;
            Balance += amount; CreditCalls++; LastCredit = amount; Calls.Add("credit:" + roundId);
            return true;
        }
    }

    public class RoundSettlerTests
    {
        private SlotConfig config;
        private FakeWallet wallet;
        private RoundSettler settler;

        [SetUp]
        public void SetUp()
        {
            config = DefaultSlotConfig.Create();
            wallet = new FakeWallet(1000);
            settler = new RoundSettler(config, wallet, new[] { 10, 20, 50, 100, 200, 500 });
        }

        private static ScriptedSymbolSource NoWinSource()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.NoWin());
            return source;
        }

        private static ScriptedSymbolSource TenWinSource()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeTenReels());
            TestGrids.EnqueueQuietRefill(source);
            return source;
        }

        [Test]
        public void NoWin_ChargesBetOnce_AndNeverCredits()
        {
            SettlementResult result = settler.Play(100, "R1", NoWinSource());

            Assert.AreEqual(SettlementStatus.Completed, result.Status);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(0, wallet.CreditCalls);
            Assert.AreEqual(900, wallet.Balance);
            Assert.AreEqual(0, result.CreditsPaid);
        }

        [Test]
        public void Win_ChargesOnce_AndCreditsOnce()
        {
            SettlementResult result = settler.Play(100, "R1", TenWinSource());

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(100, wallet.LastSpend);
            Assert.AreEqual(64, wallet.LastCredit);
            Assert.AreEqual(964, wallet.Balance);
            Assert.AreEqual(1000, result.BalanceBefore);
            Assert.AreEqual(964, result.BalanceAfter);
            CollectionAssert.AreEqual(new[] { "spend:R1", "credit:R1" }, wallet.Calls); // charge always comes first
        }

        [Test]
        public void BonusRound_ChargesOnce_AndCreditsTheWholeRoundOnce()
        {
            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            source.EnqueueGrid(TestGrids.ThreeTenReels());   // free spin 1 wins at the free-spin 2x multiplier
            TestGrids.EnqueueQuietRefill(source);
            for (int i = 0; i < 9; i++) source.EnqueueGrid(TestGrids.NoWin());

            SettlementResult result = settler.Play(100, "R1", source);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(10, result.Round.FreeSpins.Count);
            Assert.AreEqual(1, wallet.SpendCalls);   // free spins never charge a bet
            Assert.AreEqual(1, wallet.CreditCalls);  // one payment for the whole round
            Assert.AreEqual(128, wallet.LastCredit);
            Assert.AreEqual(1000 - 100 + 128, wallet.Balance);
        }

        [Test]
        public void InsufficientCredits_ChangesNothing_AndDoesNotUseTheSource()
        {
            wallet.Balance = 50;
            ScriptedSymbolSource source = NoWinSource();

            SettlementResult result = settler.Play(100, "R1", source);

            Assert.AreEqual(SettlementStatus.InsufficientCredits, result.Status);
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(50, wallet.Balance);
            Assert.AreEqual(20, source.Remaining);
        }

        [Test]
        public void InvalidBets_AreRejected()
        {
            foreach (long bet in new long[] { 0, -10, 15, 1000 })
            {
                Assert.AreEqual(SettlementStatus.InvalidBet, settler.Play(bet, "R" + bet, NoWinSource()).Status);
            }
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(1000, wallet.Balance);
        }

        [Test]
        public void EmptyRoundId_IsRejected()
        {
            Assert.AreEqual(SettlementStatus.InvalidRoundId, settler.Play(100, "", NoWinSource()).Status);
            Assert.AreEqual(SettlementStatus.InvalidRoundId, settler.Play(100, null, NoWinSource()).Status);
            Assert.AreEqual(0, wallet.SpendCalls);
        }

        [Test]
        public void DuplicateRoundId_IsRejected_WithoutSecondCharge()
        {
            settler.Play(100, "R1", NoWinSource());
            ScriptedSymbolSource second = NoWinSource();

            SettlementResult result = settler.Play(100, "R1", second);

            Assert.AreEqual(SettlementStatus.DuplicateRound, result.Status);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(900, wallet.Balance);
            Assert.AreEqual(20, second.Remaining);
        }

        [Test]
        public void DifferentRoundIds_AreAllSettledSeparately()
        {
            settler.Play(100, "R1", NoWinSource());
            settler.Play(100, "R2", TenWinSource());
            Assert.AreEqual(2, wallet.SpendCalls);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(1000 - 100 - 100 + 64, wallet.Balance);
        }

        [Test]
        public void SpendRefused_ChargesNothing_AndTheIdCanBeRetried()
        {
            wallet.RefuseSpend = true;
            SettlementResult failed = settler.Play(100, "R1", TenWinSource());
            Assert.AreEqual(SettlementStatus.SpendFailed, failed.Status);
            Assert.AreEqual(0, wallet.CreditCalls);
            Assert.AreEqual(1000, wallet.Balance);
            Assert.IsNull(failed.Round);

            wallet.RefuseSpend = false;
            Assert.IsTrue(settler.Play(100, "R1", TenWinSource()).Success);
        }

        [Test]
        public void CreditRefused_ReportsTheOwedAmount()
        {
            wallet.RefuseCredit = true;
            SettlementResult result = settler.Play(100, "R1", TenWinSource());

            Assert.AreEqual(SettlementStatus.CreditFailed, result.Status);
            Assert.AreEqual(64, result.OwedCredits);
            Assert.AreEqual(0, result.CreditsPaid);
            Assert.AreEqual(900, wallet.Balance);
        }

        [Test]
        public void SourceFailure_HappensBeforeAnyCharge_AndKeepsTheIdFree()
        {
            Assert.Throws<InvalidOperationException>(() => settler.Play(100, "R1", new ScriptedSymbolSource()));
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(1000, wallet.Balance);

            Assert.IsTrue(settler.Play(100, "R1", NoWinSource()).Success);
        }

        [Test]
        public void Constructor_RejectsBadInput()
        {
            Assert.Throws<ArgumentNullException>(() => new RoundSettler(null, wallet, new[] { 10 }));
            Assert.Throws<ArgumentNullException>(() => new RoundSettler(config, null, new[] { 10 }));
            Assert.Throws<ArgumentException>(() => new RoundSettler(config, wallet, new int[0]));
            SlotConfig bad = DefaultSlotConfig.Create();
            bad.PaidMultiplierLadder = new[] { 1, 11 };
            Assert.Throws<InvalidOperationException>(() => new RoundSettler(bad, wallet, new[] { 10 }));
        }
    }
}