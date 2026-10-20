using NUnit.Framework;
using ScatterGame.Core;
using UnityEngine;

namespace ScatterGame.Tests
{
    public class SimulationTests
    {
        [Test]
        public void Simulation_RunsCleanly_AndRespectsHardLimits()
        {
            SlotConfig config = DefaultSlotConfig.Create();
            SimulationReport report = RtpSimulator.Run(config, 20000, 12345, 100);

            Debug.Log("[ScatterSim] " + report);

            Assert.AreEqual(20000, report.Rounds);
            Assert.LessOrEqual(report.MaxMultiplierSeen, 10);
            Assert.LessOrEqual(report.MaxCascadesInOneSpin, config.MaxCascadesPerSpin);
            Assert.GreaterOrEqual(report.TotalWinMilli, 0);
            Assert.LessOrEqual(report.TotalWinCreditsSettled * 1000, report.TotalWinMilli);
        }
    }
}