using System;

namespace RouletteGame.Core
{
    /// <summary>One moment of the spin. Angles are unwrapped degrees, clockwise from the top (12 o'clock).</summary>
    public readonly struct WheelState
    {
        /// <summary>Where the wheel's zero pocket is.</summary>
        public readonly double WheelAngle;

        /// <summary>Where the ball is.</summary>
        public readonly double BallAngle;

        /// <summary>0 = running on the rim, 1 = down in the pocket.</summary>
        public readonly double DropProgress;

        public readonly bool Finished;

        public WheelState(double wheelAngle, double ballAngle, double dropProgress, bool finished)
        {
            WheelAngle = wheelAngle;
            BallAngle = ballAngle;
            DropProgress = dropProgress;
            Finished = finished;
        }
    }

    /// <summary>
    /// The whole spin as a pure function of time. The winning number is an INPUT, so the ball cannot land anywhere else.
    ///
    /// Phase 1 (rim): the ball runs counter-clockwise on the rim and slows down. The wheel turns clockwise and slows down.
    /// Phase 2 (drop): the ball moves relative to the wheel and eases out until it rests exactly in the winning pocket.
    ///   Its position is always (wheel angle + a relative angle), and the relative angle ends on the pocket's angle,
    ///   so at the end the pocket under the ball IS the winning number, whatever the wheel's own angle is.
    /// No physics, no randomness, no frame-rate dependence: the same inputs always give the same spin.
    /// </summary>
    public sealed class WheelTrajectory
    {
        private const double Step = 360.0 / RouletteLayout.PocketCount;

        private readonly double wheelStart, wheelSpeed0, wheelSpeed1;
        private readonly double ballStart, ballSpeed0, ballSpeed1;
        private readonly double total, dropTime;
        private readonly double relativeAtDrop, relativeTravel;

        public int WinningNumber { get; }
        public double Duration => total;
        public double DropTime => dropTime;

        private WheelTrajectory(int number, WheelSpinSettings s, double ballStartAngle, double wheelStartAngle)
        {
            WinningNumber = number;
            total = s.TotalDuration;
            dropTime = total * s.DropFraction;
            wheelStart = wheelStartAngle;
            wheelSpeed0 = s.WheelStartSpeed;
            wheelSpeed1 = s.WheelEndSpeed;
            ballStart = ballStartAngle;
            ballSpeed0 = s.BallStartSpeed;
            ballSpeed1 = s.BallDropSpeed;

            // Where the ball is, relative to the wheel, when it leaves the rim.
            double ballAtDrop = ballStart - (ballSpeed0 + ballSpeed1) * 0.5 * dropTime;
            relativeAtDrop = ballAtDrop - WheelAt(dropTime);

            // How far (backwards, relative to the wheel) it must travel to end on the pocket: the shortest distance
            // plus whole turns, picked so the ball keeps roughly the speed it had on the rim (no sudden acceleration).
            double pocket = RouletteLayout.PocketAngleDegrees(number);
            double needed = Normalize360(relativeAtDrop - pocket);
            double duration = total - dropTime;
            double relativeSpeed = ballSpeed1 + WheelSpeedAt(dropTime);
            double ideal = relativeSpeed * duration * 0.5;
            double turns = Math.Max(0.0, Math.Round((ideal - needed) / 360.0));
            double travel = needed + 360.0 * turns;
            if (travel < 45.0) travel += 360.0; // always a visible run into the pocket
            relativeTravel = travel;
        }

        /// <param name="winningNumber">The number the ball must land on (0 to 36). Decided elsewhere.</param>
        /// <param name="ballStartAngle">Only the look of the start. It can never change where the ball ends.</param>
        /// <param name="wheelStartAngle">Where the wheel is when the spin begins (so a new spin continues smoothly).</param>
        public static WheelTrajectory Create(int winningNumber, WheelSpinSettings settings, double ballStartAngle = 0.0, double wheelStartAngle = 0.0)
        {
            if (!RouletteLayout.IsValidNumber(winningNumber)) throw new ArgumentOutOfRangeException(nameof(winningNumber));
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (double.IsNaN(ballStartAngle) || double.IsInfinity(ballStartAngle)) ballStartAngle = 0.0;
            if (double.IsNaN(wheelStartAngle) || double.IsInfinity(wheelStartAngle)) wheelStartAngle = 0.0;

            return new WheelTrajectory(winningNumber, settings.Sanitized(), ballStartAngle, wheelStartAngle);
        }

        public WheelState Evaluate(double time)
        {
            if (double.IsNaN(time) || time < 0.0) time = 0.0;

            if (time >= total)
            {
                double wheelEnd = WheelAt(total);
                return new WheelState(wheelEnd, wheelEnd + relativeAtDrop - relativeTravel, 1.0, true);
            }

            double wheel = WheelAt(time);

            if (time <= dropTime)
            {
                double orbit = ballStart - (ballSpeed0 * time - (ballSpeed0 - ballSpeed1) * time * time / (2.0 * dropTime));
                return new WheelState(wheel, orbit, 0.0, false);
            }

            double u = (time - dropTime) / (total - dropTime);
            double ease = 1.0 - (1.0 - u) * (1.0 - u);
            double relative = relativeAtDrop - relativeTravel * ease;
            double smooth = u * u * (3.0 - 2.0 * u);
            return new WheelState(wheel, wheel + relative, smooth, false);
        }

        /// <summary>The number whose pocket is under the ball. At the end of the spin this is always WinningNumber.</summary>
        public static int PocketUnderBall(double wheelAngle, double ballAngle)
        {
            double relative = Normalize360(ballAngle - wheelAngle);
            int index = (int)Math.Round(relative / Step) % RouletteLayout.PocketCount;
            return RouletteLayout.NumberAt(index);
        }

        public static double Normalize360(double degrees)
        {
            double r = degrees % 360.0;
            if (r < 0.0) r += 360.0;
            if (r >= 360.0) r -= 360.0;
            return r;
        }

        private double WheelAt(double t)
        {
            if (t < 0.0) t = 0.0;
            if (t > total) t = total;
            return wheelStart + wheelSpeed0 * t - (wheelSpeed0 - wheelSpeed1) * t * t / (2.0 * total);
        }

        private double WheelSpeedAt(double t)
        {
            return wheelSpeed0 - (wheelSpeed0 - wheelSpeed1) * t / total;
        }
    }
}
