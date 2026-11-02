using System.Collections.Generic;
using NUnit.Framework;
using ScatterGame.Core;
using ScatterGame.Runtime;
using UnityEngine;

namespace ScatterGame.Tests
{
    public class ScatterSoundTests
    {
        private ScatterGameSettings settings;
        private readonly List<AudioClip> clips = new List<AudioClip>();

        [SetUp]
        public void SetUp()
        {
            settings = ScriptableObject.CreateInstance<ScatterGameSettings>();
            settings.CopyFrom(TunedSlotConfig.Create(), new[] { 10, 20 });
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(settings);
            foreach (AudioClip clip in clips) Object.DestroyImmediate(clip);
            clips.Clear();
        }

        private AudioClip NewClip()
        {
            AudioClip clip = AudioClip.Create("test", 441, 1, 44100, false);
            clips.Add(clip);
            return clip;
        }

        private static SymbolWin Win(int id, int payMilli, long ways)
        {
            return new SymbolWin { SymbolId = id, PayMilli = payMilli, Ways = ways };
        }

        // ---------- Settings ----------

        [Test]
        public void MultiplierSounds_HaveOneRowPerLevel_OneToMax()
        {
            Assert.AreEqual(10, settings.MultiplierSounds.Count);
            for (int i = 0; i < 10; i++) Assert.AreEqual(i + 1, settings.MultiplierSounds[i].multiplier);
        }

        [Test]
        public void MultiplierSound_IsFoundForThatLevelOnly()
        {
            AudioClip clip = NewClip();
            ((MultiplierSoundEntry)settings.MultiplierSounds[2]).clip = clip; // x3

            Assert.IsTrue(settings.TryGetMultiplierSound(3, out AudioClip found, out _));
            Assert.AreSame(clip, found);
            Assert.IsFalse(settings.TryGetMultiplierSound(4, out _, out _)); // row exists, no clip
            Assert.IsFalse(settings.TryGetMultiplierSound(0, out _, out _));
            Assert.IsFalse(settings.TryGetMultiplierSound(11, out _, out _)); // no such level
        }

        [Test]
        public void MissingMultiplierSound_NeverThrows()
        {
            Assert.DoesNotThrow(() => settings.TryGetMultiplierSound(999, out _, out _));
            Assert.DoesNotThrow(() => settings.TryGetMultiplierSound(-5, out _, out _));
        }

        [Test]
        public void WinSound_IsLookedUpBySymbolId()
        {
            AudioClip clip = NewClip();
            SymbolEntry jack = settings.GetEntry(DefaultSlotConfig.Jack);
            jack.winSound = clip;
            jack.winSoundVolume = 0.5f;

            Assert.IsTrue(settings.TryGetWinSound(DefaultSlotConfig.Jack, out AudioClip found, out float volume));
            Assert.AreSame(clip, found);
            Assert.AreEqual(0.5f, volume, 0.0001f);
            Assert.IsFalse(settings.TryGetWinSound(DefaultSlotConfig.Queen, out _, out _)); // no clip
            Assert.IsFalse(settings.TryGetWinSound(12345, out _, out _));                   // unknown symbol
        }

        [Test]
        public void CopyFrom_KeepsExistingSounds()
        {
            AudioClip clip = NewClip();
            settings.GetEntry(DefaultSlotConfig.Queen).winSound = clip;

            settings.CopyFrom(TunedSlotConfig.Create(), new[] { 10, 20 });

            Assert.AreSame(clip, settings.GetEntry(DefaultSlotConfig.Queen).winSound);
        }

        // ---------- Which symbols play ----------

        [Test]
        public void PickWinSymbols_HighestPayFirst_AndCapped()
        {
            List<SymbolWin> wins = new List<SymbolWin> { Win(0, 1, 1), Win(1, 2, 1), Win(3, 10, 4) }; // 1, 2, 40 milli
            List<int> picked = ScatterSoundPlanner.PickWinSymbols(wins, id => true, 2);
            CollectionAssert.AreEqual(new[] { 3, 1 }, picked);
        }

        [Test]
        public void PickWinSymbols_SilentSymbolsDoNotUseUpAPlace()
        {
            List<SymbolWin> wins = new List<SymbolWin> { Win(0, 1, 1), Win(1, 2, 1), Win(3, 10, 4) };
            List<int> picked = ScatterSoundPlanner.PickWinSymbols(wins, id => id != 3, 2);
            CollectionAssert.AreEqual(new[] { 1, 0 }, picked);
        }

        [Test]
        public void PickWinSymbols_LimitBelowOneStillPlaysOne_AndNullIsSafe()
        {
            List<SymbolWin> wins = new List<SymbolWin> { Win(1, 2, 1), Win(3, 10, 4) };
            CollectionAssert.AreEqual(new[] { 3 }, ScatterSoundPlanner.PickWinSymbols(wins, id => true, 0));
            Assert.IsEmpty(ScatterSoundPlanner.PickWinSymbols(null, id => true, 2));
            Assert.IsEmpty(ScatterSoundPlanner.PickWinSymbols(wins, null, 2));
        }

        [Test]
        public void WildTookPart_OnlyWhenAWildCellIsInAWin()
        {
            SlotGrid grid = TestGrids.NoWin();
            grid.Set(1, 0, TestGrids.Wild);

            SymbolWin withWild = Win(0, 1, 1);
            withWild.Positions = new List<GridPosition> { new GridPosition(0, 0), new GridPosition(1, 0), new GridPosition(2, 0) };
            SymbolWin withoutWild = Win(0, 1, 1);
            withoutWild.Positions = new List<GridPosition> { new GridPosition(0, 1), new GridPosition(1, 1), new GridPosition(2, 1) };

            Assert.IsTrue(ScatterSoundPlanner.WildTookPart(new List<SymbolWin> { withWild }, grid, TestGrids.Wild));
            Assert.IsFalse(ScatterSoundPlanner.WildTookPart(new List<SymbolWin> { withoutWild }, grid, TestGrids.Wild));
            Assert.IsFalse(ScatterSoundPlanner.WildTookPart(new List<SymbolWin> { withWild }, grid, -1));
        }

        // ---------- When multiplier sounds play ----------

        [Test]
        public void Tracker_PlaysOncePerNewLevel_NotWhileItStaysTheSame()
        {
            MultiplierSoundTracker tracker = new MultiplierSoundTracker();
            int[] steps = { 1, 1, 2, 2, 3, 5, 10, 10, 10 };
            List<int> played = new List<int>();
            foreach (int level in steps) if (tracker.ShouldPlay(level)) played.Add(level);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 10 }, played); // the skipped levels play nothing
        }

        [Test]
        public void Tracker_Reset_LetsTheFirstLevelPlayAgain()
        {
            MultiplierSoundTracker tracker = new MultiplierSoundTracker();
            Assert.IsTrue(tracker.ShouldPlay(1));
            Assert.IsFalse(tracker.ShouldPlay(1));
            tracker.Reset();
            Assert.IsTrue(tracker.ShouldPlay(1));
            Assert.IsFalse(tracker.ShouldPlay(0));
        }
    }
}