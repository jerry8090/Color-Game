using System;
using System.Collections.Generic;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    public class LayoutAndOutcomeTests
    {
        // Independent copy of the official European wheel order, so the test does not trust the code under test.
        private static readonly int[] ExpectedOrder =
        {
            0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26
        };

        private static readonly HashSet<int> ExpectedReds = new HashSet<int> { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

        [Test]
        public void AllThirtySevenNumbersAreValid_AndOthersAreNot()
        {
            for (int n = 0; n <= 36; n++) Assert.IsTrue(RouletteLayout.IsValidNumber(n));
            Assert.IsFalse(RouletteLayout.IsValidNumber(-1));
            Assert.IsFalse(RouletteLayout.IsValidNumber(37));
            Assert.Throws<ArgumentOutOfRangeException>(() => RouletteLayout.ColorOf(37));
        }

        [Test]
        public void Wheel_HasNoDuplicateOrMissingPocket()
        {
            Assert.AreEqual(37, RouletteLayout.WheelOrder.Count);
            HashSet<int> seen = new HashSet<int>(RouletteLayout.WheelOrder);
            Assert.AreEqual(37, seen.Count);
            for (int n = 0; n <= 36; n++) Assert.IsTrue(seen.Contains(n), "Missing " + n);
        }

        [Test]
        public void Wheel_FollowsTheEuropeanOrder_AndMappingRoundTrips()
        {
            for (int i = 0; i < 37; i++)
            {
                Assert.AreEqual(ExpectedOrder[i], RouletteLayout.NumberAt(i));
                Assert.AreEqual(i, RouletteLayout.IndexOf(ExpectedOrder[i]));
            }
        }

        [Test]
        public void Zero_IsGreen_AndTheOnlyGreen()
        {
            Assert.AreEqual(PocketColor.Green, RouletteLayout.ColorOf(0));
            int greens = 0;
            for (int n = 0; n <= 36; n++) if (RouletteLayout.ColorOf(n) == PocketColor.Green) greens++;
            Assert.AreEqual(1, greens);
        }

        [Test]
        public void RedAndBlack_AreCorrect_EighteenEach()
        {
            int red = 0, black = 0;
            for (int n = 1; n <= 36; n++)
            {
                bool shouldBeRed = ExpectedReds.Contains(n);
                Assert.AreEqual(shouldBeRed ? PocketColor.Red : PocketColor.Black, RouletteLayout.ColorOf(n), "Number " + n);
                if (shouldBeRed) red++; else black++;
            }
            Assert.AreEqual(18, red);
            Assert.AreEqual(18, black);
        }

        [Test]
        public void Properties_ZeroBelongsToNoOutsideGroup()
        {
            Assert.IsFalse(RouletteLayout.IsOdd(0));
            Assert.IsFalse(RouletteLayout.IsEven(0));
            Assert.IsFalse(RouletteLayout.IsLow(0));
            Assert.IsFalse(RouletteLayout.IsHigh(0));
            Assert.AreEqual(0, RouletteLayout.DozenOf(0));
            Assert.AreEqual(0, RouletteLayout.ColumnOf(0));
        }

        [Test]
        public void Properties_DozensColumnsLowHigh_AreCorrect()
        {
            Assert.AreEqual(1, RouletteLayout.DozenOf(12));
            Assert.AreEqual(2, RouletteLayout.DozenOf(13));
            Assert.AreEqual(2, RouletteLayout.DozenOf(24));
            Assert.AreEqual(3, RouletteLayout.DozenOf(25));
            Assert.AreEqual(1, RouletteLayout.ColumnOf(1));
            Assert.AreEqual(1, RouletteLayout.ColumnOf(34));
            Assert.AreEqual(2, RouletteLayout.ColumnOf(35));
            Assert.AreEqual(3, RouletteLayout.ColumnOf(36));
            Assert.IsTrue(RouletteLayout.IsLow(18));
            Assert.IsTrue(RouletteLayout.IsHigh(19));
            Assert.IsTrue(RouletteLayout.IsOdd(35));
            Assert.IsTrue(RouletteLayout.IsEven(36));
        }

        [Test]
        public void PocketAngles_AreEvenlySpaced_StartingAtZero()
        {
            Assert.AreEqual(0.0, RouletteLayout.PocketAngleDegrees(0), 1e-9);
            Assert.AreEqual(360.0 / 37.0, RouletteLayout.PocketAngleDegrees(32), 1e-9);
            Assert.AreEqual(36 * 360.0 / 37.0, RouletteLayout.PocketAngleDegrees(26), 1e-9);
        }

        // ---------- Outcome sources ----------

        [Test]
        public void CryptoSource_StaysInRange_AndEveryNumberAppears()
        {
            int[] counts = new int[37];
            using (CryptoOutcomeSource source = new CryptoOutcomeSource())
            {
                for (int i = 0; i < 37000; i++)
                {
                    int n = source.NextNumber();
                    Assert.IsTrue(n >= 0 && n <= 36);
                    counts[n]++;
                }
            }
            // Expected 1000 each (standard deviation about 31). A wide band keeps this test from ever failing by chance.
            for (int n = 0; n <= 36; n++) Assert.That(counts[n], Is.InRange(700, 1300), "Number " + n);
        }

        [Test]
        public void SeededSource_IsRepeatable()
        {
            SeededOutcomeSource a = new SeededOutcomeSource(42);
            SeededOutcomeSource b = new SeededOutcomeSource(42);
            for (int i = 0; i < 100; i++) Assert.AreEqual(a.NextNumber(), b.NextNumber());
        }

        [Test]
        public void ScriptedSource_ReturnsInOrder_AndRefusesWhenEmpty()
        {
            ScriptedOutcomeSource source = new ScriptedOutcomeSource(17, 0, 36);
            Assert.AreEqual(17, source.NextNumber());
            Assert.AreEqual(0, source.NextNumber());
            Assert.AreEqual(36, source.NextNumber());
            Assert.Throws<InvalidOperationException>(() => source.NextNumber());
            Assert.Throws<ArgumentOutOfRangeException>(() => source.Enqueue(37));
        }
    }
}
