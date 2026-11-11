using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    public class SlipTests
    {
        private RouletteBetSlip slip;
        private string error;

        [SetUp]
        public void SetUp() { slip = new RouletteBetSlip(); }

        [Test]
        public void PlacingChips_AddsAndMergesOnTheSameArea()
        {
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Straight(17, 1), 10, 1000, out error));
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Straight(17, 1), 20, 1000, out error));
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error));

            Assert.AreEqual(2, slip.Bets.Count);
            Assert.AreEqual(30, slip.GetAmount(RouletteBet.Straight(17, 1).Key));
            Assert.AreEqual(80, slip.TotalBet);
        }

        [Test]
        public void PlacingMoreThanTheBalance_IsRejected_WithoutChange()
        {
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Red(1), 80, 100, out error));
            Assert.IsFalse(slip.TryPlaceChip(RouletteBet.Black(1), 30, 100, out error));
            Assert.AreEqual(RouletteBetSlip.InsufficientCreditsMessage, error);
            Assert.AreEqual(80, slip.TotalBet);
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Black(1), 20, 100, out error)); // exactly the balance is fine
            Assert.AreEqual(100, slip.TotalBet);
        }

        [Test]
        public void InvalidChips_AreRejected()
        {
            Assert.IsFalse(slip.TryPlaceChip(RouletteBet.Red(1), 0, 1000, out _));
            Assert.IsFalse(slip.TryPlaceChip(RouletteBet.Red(1), -10, 1000, out _));
            Assert.IsFalse(slip.TryPlaceChip(null, 10, 1000, out _));
            Assert.IsFalse(slip.HasBets);
        }

        [Test]
        public void Limits_AreEnforced()
        {
            RouletteBetSlip limited = new RouletteBetSlip(new RouletteLimits(100, 150));
            Assert.IsTrue(limited.TryPlaceChip(RouletteBet.Red(1), 100, 10000, out error));
            Assert.IsFalse(limited.TryPlaceChip(RouletteBet.Red(1), 10, 10000, out error));    // area limit
            Assert.IsTrue(limited.TryPlaceChip(RouletteBet.Black(1), 50, 10000, out error));
            Assert.IsFalse(limited.TryPlaceChip(RouletteBet.Odd(1), 10, 10000, out error));    // table limit
            Assert.AreEqual(150, limited.TotalBet);
        }

        [Test]
        public void RemoveChip_ReducesThenRemovesTheArea()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Red(1), 20, 1000, out error);
            string key = RouletteBet.Red(1).Key;

            Assert.IsTrue(slip.TryRemoveChip(key, 20, out error));
            Assert.AreEqual(50, slip.GetAmount(key));
            Assert.IsTrue(slip.TryRemoveChip(key, 50, out error));
            Assert.IsFalse(slip.HasBets);
            Assert.IsFalse(slip.TryRemoveChip(key, 10, out error));
            Assert.AreEqual("NO BET THERE", error);
        }

        [Test]
        public void RemoveArea_RemovesTheWholeBet()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Black(1), 20, 1000, out error);
            Assert.IsTrue(slip.TryRemoveArea(RouletteBet.Red(1).Key, out error));
            Assert.AreEqual(1, slip.Bets.Count);
            Assert.AreEqual(20, slip.TotalBet);
            Assert.IsFalse(slip.TryRemoveArea(RouletteBet.Red(1).Key, out error));
        }

        [Test]
        public void Undo_TakesBackPlace_Remove_AndClear_InOrder()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Red(1), 20, 1000, out error);
            slip.TryRemoveArea(RouletteBet.Red(1).Key, out error);
            Assert.AreEqual(0, slip.TotalBet);

            Assert.IsTrue(slip.Undo());                       // undo the removal
            Assert.AreEqual(70, slip.TotalBet);
            slip.Clear();
            Assert.AreEqual(0, slip.TotalBet);
            Assert.IsTrue(slip.Undo());                       // undo the clear
            Assert.AreEqual(70, slip.TotalBet);
            Assert.IsTrue(slip.Undo());                       // undo the second chip
            Assert.AreEqual(50, slip.TotalBet);
            Assert.IsTrue(slip.Undo());
            Assert.AreEqual(0, slip.TotalBet);
            Assert.IsFalse(slip.Undo());                      // nothing left
        }

        [Test]
        public void Clear_RemovesEverything_AndDoesNothingWhenEmpty()
        {
            Assert.IsFalse(slip.Clear());
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Corner(1, 1), 10, 1000, out error);
            Assert.IsTrue(slip.Clear());
            Assert.IsFalse(slip.HasBets);
        }

        [Test]
        public void Confirm_NeedsABet_AndLocksTheTable()
        {
            Assert.IsFalse(slip.TryConfirm(out error));
            Assert.AreEqual("PLACE A BET FIRST", error);

            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            Assert.IsTrue(slip.TryConfirm(out error));
            Assert.IsTrue(slip.IsLocked);
            Assert.IsFalse(slip.TryConfirm(out error));
        }

        [Test]
        public void LockedSlip_RejectsEveryChange()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryConfirm(out error);
            string key = RouletteBet.Red(1).Key;

            Assert.IsFalse(slip.TryPlaceChip(RouletteBet.Black(1), 10, 1000, out error));
            Assert.AreEqual(RouletteBetSlip.ClosedMessage, error);
            Assert.IsFalse(slip.TryRemoveChip(key, 10, out _));
            Assert.IsFalse(slip.TryRemoveArea(key, out _));
            Assert.IsFalse(slip.Clear());
            Assert.IsFalse(slip.Undo());
            Assert.IsFalse(slip.TryRepeatLast(1000, out _));
            Assert.AreEqual(50, slip.TotalBet);
        }

        [Test]
        public void Unlock_ReopensTheTable()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryConfirm(out error);
            slip.Unlock();
            Assert.IsFalse(slip.IsLocked);
            Assert.IsTrue(slip.TryPlaceChip(RouletteBet.Black(1), 10, 1000, out error));
        }

        [Test]
        public void FinishRound_ClearsTable_ThenRepeatRestoresIt()
        {
            slip.TryPlaceChip(RouletteBet.Red(1), 50, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Straight(7, 1), 10, 1000, out error);
            slip.TryConfirm(out error);
            slip.FinishRound();

            Assert.IsFalse(slip.HasBets);
            Assert.IsFalse(slip.IsLocked);
            Assert.IsTrue(slip.HasLastRound);
            Assert.IsFalse(slip.CanUndo);

            Assert.IsTrue(slip.TryRepeatLast(1000, out error));
            Assert.AreEqual(60, slip.TotalBet);
            Assert.AreEqual(2, slip.Bets.Count);
            Assert.IsTrue(slip.Undo());                   // repeat can be undone
            Assert.IsFalse(slip.HasBets);
        }

        [Test]
        public void Repeat_NeedsAPreviousRound_AndEnoughCredits()
        {
            Assert.IsFalse(slip.TryRepeatLast(1000, out error));
            Assert.AreEqual("NO PREVIOUS BET", error);

            slip.TryPlaceChip(RouletteBet.Red(1), 500, 1000, out error);
            slip.FinishRound();
            Assert.IsFalse(slip.TryRepeatLast(100, out error));
            Assert.AreEqual(RouletteBetSlip.InsufficientCreditsMessage, error);
            Assert.IsFalse(slip.HasBets);
        }

        [Test]
        public void Snapshot_IsACopy_AndMaxPossibleReturnIsReported()
        {
            slip.TryPlaceChip(RouletteBet.Straight(17, 1), 10, 1000, out error);
            slip.TryPlaceChip(RouletteBet.Black(1), 100, 1000, out error);

            RouletteBet[] copy = slip.Snapshot();
            slip.Clear();
            Assert.AreEqual(2, copy.Length);
            Assert.AreEqual(560, RoulettePayout.MaxPossibleReturn(copy));

            slip.Undo();
            Assert.AreEqual(560, slip.MaxPossibleReturn());
        }

        [Test]
        public void ChangedEvent_FiresOnEveryRealChange()
        {
            int count = 0;
            slip.Changed += () => count++;

            slip.TryPlaceChip(RouletteBet.Red(1), 10, 1000, out error);   // 1
            slip.TryPlaceChip(RouletteBet.Red(1), 0, 1000, out error);    // rejected: no event
            slip.Undo();                                                  // 2
            slip.Undo();                                                  // nothing to undo: no event
            Assert.AreEqual(2, count);
        }
    }
}
