using System;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    public class BetTests
    {
        private static void AssertCovers(RouletteBet bet, params int[] expected)
        {
            CollectionAssert.AreEqual(expected, bet.Numbers);
        }

        [Test]
        public void Straight_CoversOneNumber_IncludingZero()
        {
            AssertCovers(RouletteBet.Straight(17, 10), 17);
            AssertCovers(RouletteBet.Straight(0, 10), 0);
            Assert.Catch<ArgumentException>(() => RouletteBet.Straight(37, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.Straight(-1, 10));
        }

        [Test]
        public void Split_AcceptsOnlyNeighbors()
        {
            AssertCovers(RouletteBet.Split(1, 2, 10), 1, 2);
            AssertCovers(RouletteBet.Split(1, 4, 10), 1, 4);
            AssertCovers(RouletteBet.Split(35, 36, 10), 35, 36);
            AssertCovers(RouletteBet.Split(0, 1, 10), 0, 1);
            AssertCovers(RouletteBet.Split(0, 2, 10), 0, 2);
            AssertCovers(RouletteBet.Split(0, 3, 10), 0, 3);
            AssertCovers(RouletteBet.Split(4, 1, 10), 1, 4); // order does not matter

            Assert.Catch<ArgumentException>(() => RouletteBet.Split(3, 4, 10));   // end of one row, start of the next
            Assert.Catch<ArgumentException>(() => RouletteBet.Split(1, 5, 10));   // diagonal
            Assert.Catch<ArgumentException>(() => RouletteBet.Split(1, 7, 10));   // two rows apart
            Assert.Catch<ArgumentException>(() => RouletteBet.Split(0, 4, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.Split(5, 5, 10));
        }

        [Test]
        public void Street_IsARowOfThree()
        {
            AssertCovers(RouletteBet.Street(1, 10), 1, 2, 3);
            AssertCovers(RouletteBet.Street(34, 10), 34, 35, 36);
            for (int first = 1; first <= 34; first += 3) Assert.AreEqual(3, RouletteBet.Street(first, 10).Numbers.Count);
            Assert.Catch<ArgumentException>(() => RouletteBet.Street(2, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.Street(37, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.Street(0, 10));
        }

        [Test]
        public void Corner_IsASquareOfFour()
        {
            AssertCovers(RouletteBet.Corner(1, 10), 1, 2, 4, 5);
            AssertCovers(RouletteBet.Corner(2, 10), 2, 3, 5, 6);
            AssertCovers(RouletteBet.Corner(32, 10), 32, 33, 35, 36);
            Assert.Catch<ArgumentException>(() => RouletteBet.Corner(3, 10));   // would wrap to the next row
            Assert.Catch<ArgumentException>(() => RouletteBet.Corner(33, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.Corner(34, 10));  // no row below
            Assert.Catch<ArgumentException>(() => RouletteBet.Corner(0, 10));
        }

        [Test]
        public void SixLine_IsTwoRows()
        {
            AssertCovers(RouletteBet.SixLine(1, 10), 1, 2, 3, 4, 5, 6);
            AssertCovers(RouletteBet.SixLine(31, 10), 31, 32, 33, 34, 35, 36);
            Assert.Catch<ArgumentException>(() => RouletteBet.SixLine(2, 10));
            Assert.Catch<ArgumentException>(() => RouletteBet.SixLine(34, 10));  // only one row left
        }

        [Test]
        public void RedAndBlack_CoverEighteenNumbers_NeverZero()
        {
            RouletteBet red = RouletteBet.Red(10);
            RouletteBet black = RouletteBet.Black(10);
            Assert.AreEqual(18, red.Numbers.Count);
            Assert.AreEqual(18, black.Numbers.Count);
            Assert.IsFalse(red.Covers(0));
            Assert.IsFalse(black.Covers(0));
            Assert.IsTrue(red.Covers(1));
            Assert.IsTrue(black.Covers(2));
            for (int n = 1; n <= 36; n++) Assert.AreNotEqual(red.Covers(n), black.Covers(n));
        }

        [Test]
        public void OddEvenLowHigh_AreCorrect_NeverCoverZero()
        {
            RouletteBet odd = RouletteBet.Odd(10), even = RouletteBet.Even(10), low = RouletteBet.Low(10), high = RouletteBet.High(10);
            foreach (RouletteBet bet in new[] { odd, even, low, high })
            {
                Assert.AreEqual(18, bet.Numbers.Count);
                Assert.IsFalse(bet.Covers(0));
            }
            Assert.IsTrue(odd.Covers(1) && odd.Covers(35) && !odd.Covers(2));
            Assert.IsTrue(even.Covers(2) && even.Covers(36) && !even.Covers(1));
            Assert.IsTrue(low.Covers(1) && low.Covers(18) && !low.Covers(19));
            Assert.IsTrue(high.Covers(19) && high.Covers(36) && !high.Covers(18));
        }

        [Test]
        public void Dozens_CoverTwelveEach()
        {
            AssertCovers(RouletteBet.Dozen(1, 10), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsTrue(RouletteBet.Dozen(2, 10).Covers(13) && RouletteBet.Dozen(2, 10).Covers(24));
            Assert.IsTrue(RouletteBet.Dozen(3, 10).Covers(25) && RouletteBet.Dozen(3, 10).Covers(36));
            Assert.IsFalse(RouletteBet.Dozen(1, 10).Covers(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => RouletteBet.Dozen(0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => RouletteBet.Dozen(4, 10));
        }

        [Test]
        public void Columns_CoverTwelveEach()
        {
            AssertCovers(RouletteBet.Column(1, 10), 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34);
            AssertCovers(RouletteBet.Column(2, 10), 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35);
            AssertCovers(RouletteBet.Column(3, 10), 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36);
            Assert.Throws<ArgumentOutOfRangeException>(() => RouletteBet.Column(4, 10));
        }

        [Test]
        public void InvalidAmounts_AreRejected()
        {
            Assert.Catch<ArgumentException>(() => RouletteBet.Red(0));
            Assert.Catch<ArgumentException>(() => RouletteBet.Red(-5));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Straight, new[] { 5 }, 0, out RouletteBet bet, out string error));
            Assert.IsNull(bet);
            Assert.IsNotEmpty(error);
        }

        [Test]
        public void Create_RejectsFakeOutsideBets_AndMismatchedShapes()
        {
            // A "Red" bet that secretly includes a black number, or the zero, must be refused.
            int[] almostRed = ToArray(RouletteBet.Red(10).Numbers);
            almostRed[0] = 2;
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Red, almostRed, 10, out _, out _));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Straight, new[] { 1, 2 }, 10, out _, out _));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Street, new[] { 1, 2, 4 }, 10, out _, out _));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Split, new[] { 1, 1 }, 10, out _, out _));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Straight, null, 10, out _, out _));
            Assert.IsFalse(RouletteBet.TryCreate(BetType.Straight, new int[0], 10, out _, out _));
        }

        [Test]
        public void Key_IdentifiesTheBettingArea_NotTheStake()
        {
            Assert.AreEqual(RouletteBet.Straight(17, 10).Key, RouletteBet.Straight(17, 500).Key);
            Assert.AreNotEqual(RouletteBet.Straight(17, 10).Key, RouletteBet.Straight(18, 10).Key);
            Assert.AreNotEqual(RouletteBet.Red(10).Key, RouletteBet.Black(10).Key);
            Assert.AreEqual(RouletteBet.Split(1, 2, 10).Key, RouletteBet.Split(2, 1, 10).Key);
        }

        [Test]
        public void WithAmount_KeepsTheShape_AndRejectsZero()
        {
            RouletteBet bet = RouletteBet.Corner(1, 10).WithAmount(50);
            Assert.AreEqual(50, bet.Amount);
            AssertCovers(bet, 1, 2, 4, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => bet.WithAmount(0));
        }

        private static int[] ToArray(System.Collections.Generic.IReadOnlyList<int> list)
        {
            int[] copy = new int[list.Count];
            for (int i = 0; i < copy.Length; i++) copy[i] = list[i];
            return copy;
        }
    }
}
