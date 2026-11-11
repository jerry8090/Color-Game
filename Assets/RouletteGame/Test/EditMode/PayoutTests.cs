using System;
using System.Collections.Generic;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    public class PayoutTests
    {
        private static RoundPayout Play(int winningNumber, params RouletteBet[] bets)
        {
            return RoulettePayout.Evaluate(bets, winningNumber);
        }

        [Test]
        public void StraightUp_Pays35To1_AndReturnsTheStake()
        {
            BetOutcome win = RoulettePayout.EvaluateBet(RouletteBet.Straight(17, 10), 17);
            Assert.IsTrue(win.Won);
            Assert.AreEqual(350, win.NetWinnings);
            Assert.AreEqual(10, win.StakeReturned);
            Assert.AreEqual(360, win.TotalReturn);

            BetOutcome loss = RoulettePayout.EvaluateBet(RouletteBet.Straight(17, 10), 18);
            Assert.IsFalse(loss.Won);
            Assert.AreEqual(0, loss.TotalReturn);
        }

        [Test]
        public void Split_Pays17To1()
        {
            BetOutcome win = RoulettePayout.EvaluateBet(RouletteBet.Split(8, 9, 10), 9);
            Assert.AreEqual(170, win.NetWinnings);
            Assert.AreEqual(180, win.TotalReturn);
            Assert.IsFalse(RoulettePayout.EvaluateBet(RouletteBet.Split(8, 9, 10), 10).Won);
        }

        [Test]
        public void Street_Pays11To1()
        {
            BetOutcome win = RoulettePayout.EvaluateBet(RouletteBet.Street(7, 10), 8);
            Assert.AreEqual(110, win.NetWinnings);
            Assert.AreEqual(120, win.TotalReturn);
        }

        [Test]
        public void Corner_Pays8To1()
        {
            BetOutcome win = RoulettePayout.EvaluateBet(RouletteBet.Corner(8, 10), 12);
            Assert.AreEqual(80, win.NetWinnings);
            Assert.AreEqual(90, win.TotalReturn);
        }

        [Test]
        public void SixLine_Pays5To1()
        {
            BetOutcome win = RoulettePayout.EvaluateBet(RouletteBet.SixLine(13, 10), 18);
            Assert.AreEqual(50, win.NetWinnings);
            Assert.AreEqual(60, win.TotalReturn);
        }

        [Test]
        public void EvenMoneyBets_Pay1To1()
        {
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.Red(10), 1).TotalReturn);
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.Black(10), 2).TotalReturn);
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.Odd(10), 3).TotalReturn);
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.Even(10), 4).TotalReturn);
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.Low(10), 18).TotalReturn);
            Assert.AreEqual(20, RoulettePayout.EvaluateBet(RouletteBet.High(10), 19).TotalReturn);
            Assert.AreEqual(10, RoulettePayout.EvaluateBet(RouletteBet.Red(10), 1).NetWinnings);
        }

        [Test]
        public void DozensAndColumns_Pay2To1()
        {
            BetOutcome dozen = RoulettePayout.EvaluateBet(RouletteBet.Dozen(2, 10), 20);
            Assert.AreEqual(20, dozen.NetWinnings);
            Assert.AreEqual(30, dozen.TotalReturn);

            BetOutcome column = RoulettePayout.EvaluateBet(RouletteBet.Column(3, 10), 36);
            Assert.AreEqual(20, column.NetWinnings);
            Assert.AreEqual(30, column.TotalReturn);

            Assert.IsFalse(RoulettePayout.EvaluateBet(RouletteBet.Dozen(2, 10), 12).Won);
            Assert.IsFalse(RoulettePayout.EvaluateBet(RouletteBet.Column(3, 10), 35).Won);
        }

        [Test]
        public void Zero_DefeatsEveryOrdinaryOutsideBet()
        {
            RouletteBet[] outside =
            {
                RouletteBet.Red(10), RouletteBet.Black(10), RouletteBet.Odd(10), RouletteBet.Even(10),
                RouletteBet.Low(10), RouletteBet.High(10),
                RouletteBet.Dozen(1, 10), RouletteBet.Dozen(2, 10), RouletteBet.Dozen(3, 10),
                RouletteBet.Column(1, 10), RouletteBet.Column(2, 10), RouletteBet.Column(3, 10)
            };

            RoundPayout payout = RoulettePayout.Evaluate(outside, 0);
            Assert.AreEqual(0, payout.WinningBetCount);
            Assert.AreEqual(0, payout.TotalReturned);
            Assert.AreEqual(120, payout.TotalStake);
            Assert.AreEqual(-120, payout.NetResult);
        }

        [Test]
        public void Zero_StillPaysStraightAndZeroSplits()
        {
            Assert.AreEqual(360, RoulettePayout.EvaluateBet(RouletteBet.Straight(0, 10), 0).TotalReturn);
            Assert.AreEqual(180, RoulettePayout.EvaluateBet(RouletteBet.Split(0, 2, 10), 0).TotalReturn);
        }

        [Test]
        public void MixedRound_AddsUpCorrectly()
        {
            // Winning number 17 (black, odd, low, 2nd dozen, 2nd column).
            RoundPayout payout = Play(17,
                RouletteBet.Straight(17, 10),   // wins: returns 360
                RouletteBet.Black(100),         // wins: returns 200
                RouletteBet.Red(50),            // loses
                RouletteBet.Dozen(2, 20),       // wins: returns 60
                RouletteBet.Corner(1, 5));      // loses

            Assert.AreEqual(185, payout.TotalStake);
            Assert.AreEqual(620, payout.TotalReturned);
            Assert.AreEqual(435, payout.NetResult);
            Assert.AreEqual(3, payout.WinningBetCount);
            Assert.AreEqual(5, payout.Outcomes.Count);
        }

        [Test]
        public void NoBets_ProducesAnEmptyResult()
        {
            RoundPayout payout = Play(5);
            Assert.AreEqual(0, payout.TotalStake);
            Assert.AreEqual(0, payout.TotalReturned);
            Assert.AreEqual(0, payout.NetResult);
        }

        [Test]
        public void EveryBetType_HasTheSameHouseEdge_ForAllNumbers()
        {
            // Covered numbers x (multiplier + 1) must be 36 for every bet. That is the European 36/37 return.
            List<RouletteBet> samples = new List<RouletteBet>
            {
                RouletteBet.Straight(0, 1), RouletteBet.Straight(17, 1), RouletteBet.Split(1, 2, 1), RouletteBet.Split(0, 3, 1),
                RouletteBet.Street(1, 1), RouletteBet.Corner(1, 1), RouletteBet.SixLine(1, 1),
                RouletteBet.Red(1), RouletteBet.Black(1), RouletteBet.Odd(1), RouletteBet.Even(1), RouletteBet.Low(1), RouletteBet.High(1),
                RouletteBet.Dozen(1, 1), RouletteBet.Column(1, 1)
            };

            foreach (RouletteBet bet in samples)
            {
                long totalReturnOverAllNumbers = 0;
                for (int n = 0; n <= 36; n++) totalReturnOverAllNumbers += RoulettePayout.EvaluateBet(bet, n).TotalReturn;
                Assert.AreEqual(36, totalReturnOverAllNumbers, bet.Key);
            }
        }

        [Test]
        public void Multipliers_MatchTheRules()
        {
            Assert.AreEqual(35, RoulettePayout.MultiplierFor(BetType.Straight));
            Assert.AreEqual(17, RoulettePayout.MultiplierFor(BetType.Split));
            Assert.AreEqual(11, RoulettePayout.MultiplierFor(BetType.Street));
            Assert.AreEqual(8, RoulettePayout.MultiplierFor(BetType.Corner));
            Assert.AreEqual(5, RoulettePayout.MultiplierFor(BetType.SixLine));
            Assert.AreEqual(2, RoulettePayout.MultiplierFor(BetType.Dozen));
            Assert.AreEqual(2, RoulettePayout.MultiplierFor(BetType.Column));
            Assert.AreEqual(1, RoulettePayout.MultiplierFor(BetType.Red));
        }

        [Test]
        public void MaxPossibleReturn_FindsTheBestNumber()
        {
            RouletteBet[] bets = { RouletteBet.Straight(17, 10), RouletteBet.Black(100) };
            // Best case is 17 (black): 360 + 200.
            Assert.AreEqual(560, RoulettePayout.MaxPossibleReturn(bets));
            Assert.AreEqual(0, RoulettePayout.MaxPossibleReturn(new RouletteBet[0]));
        }

        [Test]
        public void InvalidInput_IsRejected()
        {
            Assert.Throws<ArgumentNullException>(() => RoulettePayout.Evaluate(null, 1));
            Assert.Throws<ArgumentNullException>(() => RoulettePayout.EvaluateBet(null, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => RoulettePayout.Evaluate(new RouletteBet[0], 37));
            Assert.Throws<ArgumentOutOfRangeException>(() => RoulettePayout.EvaluateBet(RouletteBet.Red(1), -1));
        }

        [Test]
        public void HugeStakes_ThrowInsteadOfOverflowing()
        {
            RouletteBet huge = RouletteBet.Straight(1, long.MaxValue / 2);
            Assert.Throws<OverflowException>(() => RoulettePayout.EvaluateBet(huge, 1));
        }
    }
}
