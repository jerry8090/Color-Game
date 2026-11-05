using NUnit.Framework;
using ScatterGame.Core;

namespace ScatterGame.Tests
{
    public class MultiplierTests
    {
        [Test]
        public void Progression_FollowsLadder_AndReachesTen()
        {
            MultiplierController controller = new MultiplierController(new[] { 1, 2, 3, 5, 7, 10 }, 10);
            int[] expected = { 1, 2, 3, 5, 7, 10, 10, 10 };
            foreach (int value in expected) Assert.AreEqual(value, controller.RegisterWin());
            Assert.AreEqual(10, controller.HighestApplied);
        }

        [Test]
        public void NeverExceedsMaximum_EvenWithBadLadder()
        {
            MultiplierController controller = new MultiplierController(new[] { 1, 50 }, 10);
            controller.RegisterWin();
            Assert.AreEqual(10, controller.RegisterWin());
            Assert.AreEqual(10, controller.RegisterWin());
        }

        [Test]
        public void CurrentAndNext_AreReportedCorrectly()
        {
            MultiplierController controller = new MultiplierController(new[] { 1, 2, 3 }, 10);
            Assert.AreEqual(1, controller.Current);
            Assert.AreEqual(1, controller.NextToApply);
            controller.RegisterWin();
            Assert.AreEqual(1, controller.Current);
            Assert.AreEqual(2, controller.NextToApply);
        }

        [Test]
        public void Reset_StartsAgain()
        {
            MultiplierController controller = new MultiplierController(new[] { 1, 2, 3 }, 10);
            controller.RegisterWin(); controller.RegisterWin();
            controller.Reset();
            Assert.AreEqual(1, controller.RegisterWin());
        }

        [Test]
        public void Constructor_RejectsEmptyLadder()
        {
            Assert.Throws<System.ArgumentException>(() => new MultiplierController(new int[0], 10));
        }
    }
}