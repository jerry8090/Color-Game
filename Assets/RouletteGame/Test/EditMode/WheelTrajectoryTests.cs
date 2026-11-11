using System;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    public class WheelTrajectoryTests
    {
        private static readonly double[] StartAngles = { 0, 13.7, 180, 359.9, -75, 1234.5 };

        private static WheelSpinSettings[] SettingVariants()
        {
            return new[]
            {
                new WheelSpinSettings(),
                new WheelSpinSettings { TotalDuration = 3f, DropFraction = 0.5f, WheelStartSpeed = 0f, WheelEndSpeed = 0f, BallStartSpeed = 300f, BallDropSpeed = 50f },
                new WheelSpinSettings { TotalDuration = 12f, DropFraction = 0.85f, WheelStartSpeed = 200f, WheelEndSpeed = 40f, BallStartSpeed = 1500f, BallDropSpeed = 400f },
                new WheelSpinSettings { TotalDuration = 2f, DropFraction = 0.3f, WheelStartSpeed = 360f, WheelEndSpeed = 0f, BallStartSpeed = 2000f, BallDropSpeed = 10f }
            };
        }

        [Test]
        public void BallEndsInTheWinningPocket_ForAll37Numbers_AndEveryStartAndSetting()
        {
            foreach (WheelSpinSettings settings in SettingVariants())
            {
                foreach (double ballStart in StartAngles)
                {
                    foreach (double wheelStart in StartAngles)
                    {
                        for (int number = 0; number <= 36; number++)
                        {
                            WheelTrajectory t = WheelTrajectory.Create(number, settings, ballStart, wheelStart);
                            WheelState end = t.Evaluate(t.Duration);

                            Assert.IsTrue(end.Finished);
                            Assert.AreEqual(number, WheelTrajectory.PocketUnderBall(end.WheelAngle, end.BallAngle), "number " + number);

                            double relative = WheelTrajectory.Normalize360(end.BallAngle - end.WheelAngle);
                            double wanted = RouletteLayout.PocketAngleDegrees(number);
                            double diff = Math.Abs(relative - wanted);
                            Assert.Less(Math.Min(diff, 360.0 - diff), 1e-6, "number " + number);
                        }
                    }
                }
            }
        }

        [Test]
        public void ResultStaysTheSame_AfterTheEnd_AndForBadTimes()
        {
            WheelTrajectory t = WheelTrajectory.Create(26, new WheelSpinSettings(), 40, 10);
            WheelState end = t.Evaluate(t.Duration);

            WheelState later = t.Evaluate(t.Duration + 100);
            Assert.AreEqual(end.WheelAngle, later.WheelAngle, 1e-9);
            Assert.AreEqual(end.BallAngle, later.BallAngle, 1e-9);

            WheelState start = t.Evaluate(0);
            Assert.AreEqual(start.BallAngle, t.Evaluate(-5).BallAngle, 1e-9);
            Assert.AreEqual(start.BallAngle, t.Evaluate(double.NaN).BallAngle, 1e-9);
            Assert.IsFalse(start.Finished);
        }

        [Test]
        public void TheSpin_IsDeterministic()
        {
            WheelTrajectory a = WheelTrajectory.Create(17, new WheelSpinSettings(), 55, 12);
            WheelTrajectory b = WheelTrajectory.Create(17, new WheelSpinSettings(), 55, 12);
            for (double time = 0; time <= 7; time += 0.25)
            {
                Assert.AreEqual(a.Evaluate(time).BallAngle, b.Evaluate(time).BallAngle);
                Assert.AreEqual(a.Evaluate(time).WheelAngle, b.Evaluate(time).WheelAngle);
            }
        }

        [Test]
        public void Motion_IsSmooth_NoJumpsBetweenFrames()
        {
            const double dt = 1.0 / 120.0;
            foreach (WheelSpinSettings settings in SettingVariants())
            {
                WheelSpinSettings safe = settings.Sanitized();
                // A frame can never move the ball more than its fastest speed allows (plus slack for the drop phase).
                double limit = Math.Max(safe.BallStartSpeed, 720.0) * dt * 1.5 + 1.0;

                for (int number = 0; number <= 36; number += 6)
                {
                    WheelTrajectory t = WheelTrajectory.Create(number, settings, 33, 71);
                    WheelState previous = t.Evaluate(0);
                    for (double time = dt; time <= t.Duration + dt; time += dt)
                    {
                        WheelState now = t.Evaluate(time);
                        Assert.Less(Math.Abs(now.BallAngle - previous.BallAngle), limit, "number " + number + " at " + time);
                        Assert.Less(Math.Abs(now.WheelAngle - previous.WheelAngle), safe.WheelStartSpeed * dt + 1e-6);
                        previous = now;
                    }
                }
            }
        }

        [Test]
        public void Ball_SlowsDownOnTheRim_AndRunsBackwardsOnlyIntoThePocket()
        {
            WheelSpinSettings settings = new WheelSpinSettings();
            WheelTrajectory t = WheelTrajectory.Create(5, settings, 0, 0);

            // Rim phase: the ball keeps moving counter-clockwise, and each second covers less ground.
            double lastTravel = double.MaxValue;
            for (double time = 0; time + 1.0 <= t.DropTime; time += 1.0)
            {
                double travel = t.Evaluate(time).BallAngle - t.Evaluate(time + 1.0).BallAngle;
                Assert.Greater(travel, 0.0);
                Assert.Less(travel, lastTravel);
                lastTravel = travel;
            }

            // Drop phase: relative to the wheel the ball only moves one way and never turns back.
            double previousRelative = t.Evaluate(t.DropTime).BallAngle - t.Evaluate(t.DropTime).WheelAngle;
            for (double time = t.DropTime; time <= t.Duration; time += 0.02)
            {
                WheelState s = t.Evaluate(time);
                double relative = s.BallAngle - s.WheelAngle;
                Assert.LessOrEqual(relative, previousRelative + 1e-9);
                previousRelative = relative;
            }
        }

        [Test]
        public void DropProgress_RunsFromZeroToOne_WithoutGoingBack()
        {
            WheelTrajectory t = WheelTrajectory.Create(9, new WheelSpinSettings(), 0, 0);
            Assert.AreEqual(0.0, t.Evaluate(0).DropProgress);
            Assert.AreEqual(0.0, t.Evaluate(t.DropTime).DropProgress, 1e-9);
            Assert.AreEqual(1.0, t.Evaluate(t.Duration).DropProgress);

            double previous = 0;
            for (double time = 0; time <= t.Duration; time += 0.05)
            {
                double p = t.Evaluate(time).DropProgress;
                Assert.GreaterOrEqual(p, previous);
                previous = p;
            }
        }

        [Test]
        public void TheWheelComesToAFullStop_WhenItsEndSpeedIsZero()
        {
            WheelTrajectory t = WheelTrajectory.Create(1, new WheelSpinSettings { WheelEndSpeed = 0f }, 0, 0);
            double a = t.Evaluate(t.Duration - 1.0 / 120.0).WheelAngle;
            double b = t.Evaluate(t.Duration).WheelAngle;
            Assert.Less(Math.Abs(b - a), 0.05);
        }

        [Test]
        public void TheBallAlwaysRunsAVisibleDistanceIntoThePocket()
        {
            // Even when the ball happens to drop right next to its pocket, it still travels at least 45 degrees.
            for (int number = 0; number <= 36; number++)
            {
                WheelTrajectory t = WheelTrajectory.Create(number, new WheelSpinSettings(), 0, 0);
                WheelState atDrop = t.Evaluate(t.DropTime);
                WheelState end = t.Evaluate(t.Duration);
                double travelled = (atDrop.BallAngle - atDrop.WheelAngle) - (end.BallAngle - end.WheelAngle);
                Assert.GreaterOrEqual(travelled, 45.0 - 1e-6, "number " + number);
            }
        }

        [Test]
        public void SettingsOutOfRange_AreReported_AndSanitizedSafely()
        {
            Assert.IsEmpty(new WheelSpinSettings().GetProblems());

            WheelSpinSettings bad = new WheelSpinSettings
            {
                TotalDuration = 0f, DropFraction = 2f, WheelStartSpeed = -5f, WheelEndSpeed = 999f,
                BallStartSpeed = float.NaN, BallDropSpeed = 99999f
            };
            Assert.IsNotEmpty(bad.GetProblems());

            WheelSpinSettings safe = bad.Sanitized();
            Assert.IsEmpty(safe.GetProblems());
            Assert.GreaterOrEqual(safe.TotalDuration, WheelSpinSettings.MinDuration);
            Assert.LessOrEqual(safe.WheelEndSpeed, safe.WheelStartSpeed);
            Assert.LessOrEqual(safe.BallDropSpeed, safe.BallStartSpeed);

            // A broken settings object still gives a correct, finished spin.
            WheelTrajectory t = WheelTrajectory.Create(14, bad, double.NaN, double.PositiveInfinity);
            WheelState end = t.Evaluate(t.Duration);
            Assert.AreEqual(14, WheelTrajectory.PocketUnderBall(end.WheelAngle, end.BallAngle));
            Assert.IsFalse(double.IsNaN(end.BallAngle));
        }

        [Test]
        public void InvalidInput_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => WheelTrajectory.Create(37, new WheelSpinSettings()));
            Assert.Throws<ArgumentOutOfRangeException>(() => WheelTrajectory.Create(-1, new WheelSpinSettings()));
            Assert.Throws<ArgumentNullException>(() => WheelTrajectory.Create(1, null));
        }

        [Test]
        public void Normalize360_StaysInRange()
        {
            Assert.AreEqual(0.0, WheelTrajectory.Normalize360(0));
            Assert.AreEqual(0.0, WheelTrajectory.Normalize360(360));
            Assert.AreEqual(350.0, WheelTrajectory.Normalize360(-10), 1e-9);
            Assert.AreEqual(10.0, WheelTrajectory.Normalize360(730), 1e-9);
            Assert.Less(WheelTrajectory.Normalize360(-1e-20), 360.0);
        }

        // ---------- The whole chain: outcome -> settlement -> wheel ----------

        [Test]
        public void WheelLandsOnTheGeneratedResult_ThroughTheSettler_ForAllNumbers()
        {
            for (int number = 0; number <= 36; number++)
            {
                FakeWallet wallet = new FakeWallet(1000);
                RouletteRoundSettler settler = new RouletteRoundSettler(wallet);

                RouletteSettlementResult result = settler.Play(new[] { RouletteBet.Red(10) }, "R" + number, new ScriptedOutcomeSource(number));
                Assert.IsTrue(result.Success);
                Assert.AreEqual(number, result.WinningNumber);

                WheelTrajectory t = WheelTrajectory.Create(result.WinningNumber, new WheelSpinSettings(), 17 * number, 5 * number);
                WheelState end = t.Evaluate(t.Duration);
                Assert.AreEqual(result.WinningNumber, WheelTrajectory.PocketUnderBall(end.WheelAngle, end.BallAngle));
            }
        }
    }
}
