using System;
using System.Collections.Generic;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    internal sealed class FakeWallet : IRouletteWallet
    {
        public long Balance;
        public bool RefuseSpend;
        public bool RefuseCredit;
        public int SpendCalls;
        public int CreditCalls;
        public long LastSpend;
        public long LastCredit;
        public Action OnSpend;
        public readonly List<string> Calls = new List<string>();

        public FakeWallet(long balance) { Balance = balance; }

        public long GetBalance() { return Balance; }

        public bool TrySpend(long amount, string roundId)
        {
            if (RefuseSpend || amount <= 0 || Balance < amount) return false;
            OnSpend?.Invoke();
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

    internal sealed class FixedSource : IOutcomeSource
    {
        private readonly int number;
        public FixedSource(int number) { this.number = number; }
        public int NextNumber() { return number; }
    }

    public class SettlerTests
    {
        private FakeWallet wallet;
        private RouletteRoundSettler settler;

        [SetUp]
        public void SetUp()
        {
            wallet = new FakeWallet(1000);
            settler = new RouletteRoundSettler(wallet);
        }

        private static RouletteBet[] Bets(params RouletteBet[] bets) { return bets; }

        [Test]
        public void SuccessfulBet_WinningStraight_ChargesOnce_CreditsStakePlusWinnings()
        {
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(17));

            Assert.AreEqual(RouletteSettlementStatus.Completed, r.Status);
            Assert.AreEqual(17, r.WinningNumber);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(10, wallet.LastSpend);
            Assert.AreEqual(360, wallet.LastCredit);   // 350 winnings + 10 stake back
            Assert.AreEqual(1350, wallet.Balance);
            Assert.AreEqual(1000, r.BalanceBefore);
            Assert.AreEqual(1350, r.BalanceAfter);
            Assert.AreEqual(350, r.Payout.NetResult);
            CollectionAssert.AreEqual(new[] { "spend:R1", "credit:R1" }, wallet.Calls); // charge always first
        }

        [Test]
        public void LosingBet_ChargesOnce_NeverCredits()
        {
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(18));

            Assert.IsTrue(r.Success);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(0, wallet.CreditCalls);
            Assert.AreEqual(990, wallet.Balance);
            Assert.AreEqual(0, r.CreditsPaid);
        }

        [Test]
        public void MultipleBets_AreChargedAsOneTotal_AndPaidAsOneTotal()
        {
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Black(100), RouletteBet.Red(50)), "R1", new ScriptedOutcomeSource(17));

            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(150, wallet.LastSpend);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(200, wallet.LastCredit);
            Assert.AreEqual(1050, wallet.Balance);
            Assert.AreEqual(150, r.CreditsCharged);
            Assert.AreEqual(200, r.CreditsPaid);
        }

        [Test]
        public void Zero_TakesEveryOutsideBet_NoCredit()
        {
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Red(10), RouletteBet.Even(10), RouletteBet.Low(10)), "R1", new ScriptedOutcomeSource(0));

            Assert.IsTrue(r.Success);
            Assert.AreEqual(0, wallet.CreditCalls);
            Assert.AreEqual(970, wallet.Balance);
        }

        [Test]
        public void InsufficientCredits_ChangesNothing_AndDoesNotDrawANumber()
        {
            wallet.Balance = 5;
            ScriptedOutcomeSource source = new ScriptedOutcomeSource(17);

            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", source);

            Assert.AreEqual(RouletteSettlementStatus.InsufficientCredits, r.Status);
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(5, wallet.Balance);
            Assert.AreEqual(1, source.Remaining);
            Assert.IsFalse(settler.IsRoundIdUsed("R1"));
        }

        [Test]
        public void InvalidRequests_AreRejected_WithoutCharging()
        {
            ScriptedOutcomeSource source = new ScriptedOutcomeSource(1, 2, 3, 4, 5);

            Assert.AreEqual(RouletteSettlementStatus.NoBets, settler.Play(Bets(), "R1", source).Status);
            Assert.AreEqual(RouletteSettlementStatus.NoBets, settler.Play(null, "R2", source).Status);
            Assert.AreEqual(RouletteSettlementStatus.InvalidBet, settler.Play(Bets(RouletteBet.Red(10), null), "R3", source).Status);
            Assert.AreEqual(RouletteSettlementStatus.InvalidRoundId, settler.Play(Bets(RouletteBet.Red(10)), "", source).Status);
            Assert.AreEqual(RouletteSettlementStatus.InvalidRoundId, settler.Play(Bets(RouletteBet.Red(10)), null, source).Status);

            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(1000, wallet.Balance);
            Assert.AreEqual(5, source.Remaining);
        }

        [Test]
        public void DuplicateRoundId_IsRefused_NoSecondChargeOrPayout()
        {
            settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(17));
            ScriptedOutcomeSource second = new ScriptedOutcomeSource(17);

            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", second);

            Assert.AreEqual(RouletteSettlementStatus.DuplicateRound, r.Status);
            Assert.AreEqual(1, wallet.SpendCalls);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(1350, wallet.Balance);
            Assert.AreEqual(1, second.Remaining);
        }

        [Test]
        public void DifferentRoundIds_AreSettledSeparately()
        {
            settler.Play(Bets(RouletteBet.Red(100)), "R1", new ScriptedOutcomeSource(1));  // wins
            settler.Play(Bets(RouletteBet.Red(100)), "R2", new ScriptedOutcomeSource(2));  // loses
            Assert.AreEqual(2, wallet.SpendCalls);
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(1000 - 100 + 200 - 100, wallet.Balance);
        }

        [Test]
        public void SpendRefused_ChargesNothing_AndTheIdCanBeRetried()
        {
            wallet.RefuseSpend = true;
            RouletteSettlementResult failed = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(17));

            Assert.AreEqual(RouletteSettlementStatus.SpendFailed, failed.Status);
            Assert.AreEqual(0, wallet.CreditCalls);
            Assert.AreEqual(1000, wallet.Balance);
            Assert.IsNull(failed.Payout);
            Assert.IsFalse(settler.IsRoundIdUsed("R1"));

            wallet.RefuseSpend = false;
            Assert.IsTrue(settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(17)).Success);
        }

        [Test]
        public void CreditRefused_RemembersTheDebt_AndRecoversItExactlyOnce()
        {
            wallet.RefuseCredit = true;
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Straight(17, 10)), "R1", new ScriptedOutcomeSource(17));

            Assert.AreEqual(RouletteSettlementStatus.CreditFailed, r.Status);
            Assert.AreEqual(360, r.OwedCredits);
            Assert.AreEqual(360, settler.OwedFor("R1"));
            Assert.AreEqual(990, wallet.Balance);

            Assert.IsFalse(settler.TryRecoverOwed("R1", out _));   // still refused: debt stays
            wallet.RefuseCredit = false;
            Assert.IsTrue(settler.TryRecoverOwed("R1", out long credited));
            Assert.AreEqual(360, credited);
            Assert.AreEqual(1350, wallet.Balance);

            Assert.IsFalse(settler.TryRecoverOwed("R1", out _));   // never paid twice
            Assert.AreEqual(1, wallet.CreditCalls);
            Assert.AreEqual(0, settler.OwedFor("R1"));
            Assert.AreEqual(RouletteSettlementStatus.DuplicateRound, settler.Play(Bets(RouletteBet.Red(10)), "R1", new ScriptedOutcomeSource(1)).Status);
        }

        [Test]
        public void SourceFailure_HappensBeforeAnyCharge_AndKeepsTheIdFree()
        {
            Assert.Throws<InvalidOperationException>(() => settler.Play(Bets(RouletteBet.Red(10)), "R1", new ScriptedOutcomeSource()));
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.AreEqual(1000, wallet.Balance);
            Assert.IsFalse(settler.IsRoundIdUsed("R1"));
            Assert.IsTrue(settler.Play(Bets(RouletteBet.Red(10)), "R1", new ScriptedOutcomeSource(1)).Success);
        }

        [Test]
        public void SourceReturningAnImpossibleNumber_IsRefused_BeforeAnyCharge()
        {
            RouletteSettlementResult r = settler.Play(Bets(RouletteBet.Red(10)), "R1", new FixedSource(99));
            Assert.AreEqual(RouletteSettlementStatus.InvalidOutcome, r.Status);
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.IsFalse(settler.IsRoundIdUsed("R1"));
        }

        [Test]
        public void TableLimits_AreEnforced_PerAreaAndInTotal()
        {
            RouletteRoundSettler limited = new RouletteRoundSettler(wallet, new RouletteLimits(100, 150));

            Assert.AreEqual(RouletteSettlementStatus.LimitExceeded, limited.Play(Bets(RouletteBet.Straight(1, 200)), "R1", new ScriptedOutcomeSource(1)).Status);
            Assert.AreEqual(RouletteSettlementStatus.LimitExceeded, limited.Play(Bets(RouletteBet.Straight(1, 60), RouletteBet.Straight(1, 60)), "R2", new ScriptedOutcomeSource(1)).Status);
            Assert.AreEqual(RouletteSettlementStatus.LimitExceeded, limited.Play(Bets(RouletteBet.Red(100), RouletteBet.Black(100)), "R3", new ScriptedOutcomeSource(1)).Status);
            Assert.AreEqual(0, wallet.SpendCalls);
            Assert.IsTrue(limited.Play(Bets(RouletteBet.Red(100), RouletteBet.Black(50)), "R4", new ScriptedOutcomeSource(1)).Success);
        }

        [Test]
        public void ReentrantPlay_DuringTheCharge_IsRefusedAsBusy()
        {
            RouletteSettlementResult inner = null;
            wallet.OnSpend = () => inner = settler.Play(Bets(RouletteBet.Red(10)), "R2", new ScriptedOutcomeSource(1));

            RouletteSettlementResult outer = settler.Play(Bets(RouletteBet.Red(10)), "R1", new ScriptedOutcomeSource(1));

            Assert.AreEqual(RouletteSettlementStatus.Busy, inner.Status);
            Assert.IsTrue(outer.Success);
            Assert.AreEqual(1, wallet.SpendCalls);
        }

        [Test]
        public void PlayingAfterSettlement_IsAllowedWithANewId()
        {
            Assert.IsTrue(settler.Play(Bets(RouletteBet.Red(10)), "R1", new ScriptedOutcomeSource(2)).Success);
            Assert.IsTrue(settler.Play(Bets(RouletteBet.Red(10)), "R2", new ScriptedOutcomeSource(1)).Success);
        }

        [Test]
        public void Constructor_RejectsNullWallet_AndBadLimits()
        {
            Assert.Throws<ArgumentNullException>(() => new RouletteRoundSettler(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RouletteLimits(0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RouletteLimits(10, 0));
            Assert.Throws<ArgumentNullException>(() => settler.Play(Bets(RouletteBet.Red(10)), "R1", null));
        }
    }
}
