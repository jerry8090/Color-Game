using System.Reflection;
using NUnit.Framework;
using ScatterGame.Core;
using ScatterGame.Runtime;
using UnityEngine;

namespace ScatterGame.Tests
{
    public class ScatterSettingsTests
    {
        private ScatterGameSettings settings;

        [SetUp]
        public void SetUp()
        {
            settings = ScriptableObject.CreateInstance<ScatterGameSettings>();
            settings.CopyFrom(TunedSlotConfig.Create(), new[] { 10, 20, 50, 100, 200, 500 });
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(settings);
        }

        [Test]
        public void DefaultSettings_HaveNoProblems()
        {
            Assert.IsEmpty(settings.GetProblems());
            Assert.AreEqual(6, settings.BetAmounts.Count);
            Assert.AreEqual(0, settings.DefaultBetIndex);
        }

        [Test]
        public void BuiltConfig_PlaysExactlyLikeTheTunedConfig()
        {
            SimulationReport fromAsset = RtpSimulator.Run(settings.BuildSlotConfig(), 3000, 99, 100);
            SimulationReport direct = RtpSimulator.Run(TunedSlotConfig.Create(), 3000, 99, 100);

            Assert.AreEqual(direct.TotalWinMilli, fromAsset.TotalWinMilli);
            Assert.AreEqual(direct.BonusRounds, fromAsset.BonusRounds);
            Assert.AreEqual(10, settings.BuildSlotConfig().MaxMultiplier);
        }

        [Test]
        public void BrokenSettings_AreReported()
        {
            typeof(ScatterGameSettings).GetField("maxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(settings, 3);
            Assert.IsNotEmpty(settings.GetProblems()); // the ladders contain values above 3

            typeof(ScatterGameSettings).GetField("maxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(settings, 10);
            typeof(ScatterGameSettings).GetField("betAmounts", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(settings, new[] { 10, 10, -5 });
            Assert.IsNotEmpty(settings.GetProblems());
        }
    }
}