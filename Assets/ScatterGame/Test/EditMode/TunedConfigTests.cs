using NUnit.Framework;
using ScatterGame.Core;
using UnityEngine;

namespace ScatterGame.Tests
{
    public class TunedConfigTests
    {
        [Test]
        public void TunedConfig_IsValid()
        {
            Assert.IsEmpty(TunedSlotConfig.Create().Validate());
        }

        [Test]
        public void TunedConfig_Rtp_IsInSaneRange_AndRespectsLimits()
        {
            SlotConfig config = TunedSlotConfig.Create();
            SimulationReport report = RtpSimulator.Run(config, 60000, 2024, 100);
            Debug.Log("[ScatterSim tuned] " + report);

            // Wide band on purpose: a 60,000-round sample moves a few points between seeds.
            Assert.Greater(report.Rtp, 0.80);
            Assert.Less(report.Rtp, 1.12);
            Assert.LessOrEqual(report.MaxMultiplierSeen, 10);
            Assert.LessOrEqual(report.MaxCascadesInOneSpin, config.MaxCascadesPerSpin);
        }
    }
}