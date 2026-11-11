using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    /// <summary>
    /// Tunable numbers of the spin animation. Plain serializable data, so it shows in the Inspector.
    /// Angles are in degrees, speeds in degrees per second. The wheel turns clockwise, the ball counter-clockwise.
    /// Out-of-range values are never fatal: Sanitized() returns a safe copy and GetProblems() explains what was wrong.
    /// </summary>
    [Serializable]
    public class WheelSpinSettings
    {
        public const float MinDuration = 2f, MaxDuration = 30f;
        public const float MinDropFraction = 0.3f, MaxDropFraction = 0.9f;
        public const float MaxWheelSpeed = 360f;
        public const float MinBallSpeed = 30f, MaxBallSpeed = 2000f;
        public const float MinBallDropSpeed = 10f;

        /// <summary>Seconds from the start of the spin until everything has stopped.</summary>
        public float TotalDuration = 7f;

        /// <summary>Fraction of the spin after which the ball leaves the rim and falls toward the pockets.</summary>
        public float DropFraction = 0.68f;

        /// <summary>Wheel speed at the start (clockwise).</summary>
        public float WheelStartSpeed = 70f;

        /// <summary>Wheel speed at the very end. 0 = the wheel comes to a full stop.</summary>
        public float WheelEndSpeed = 0f;

        /// <summary>Ball speed at the start (counter-clockwise, on the rim).</summary>
        public float BallStartSpeed = 640f;

        /// <summary>Ball speed at the moment it leaves the rim.</summary>
        public float BallDropSpeed = 140f;

        public WheelSpinSettings Sanitized()
        {
            float wheelStart = Clamp(WheelStartSpeed, 0f, MaxWheelSpeed);
            float ballStart = Clamp(BallStartSpeed, MinBallSpeed, MaxBallSpeed);

            return new WheelSpinSettings
            {
                TotalDuration = Clamp(TotalDuration, MinDuration, MaxDuration),
                DropFraction = Clamp(DropFraction, MinDropFraction, MaxDropFraction),
                WheelStartSpeed = wheelStart,
                WheelEndSpeed = Clamp(WheelEndSpeed, 0f, wheelStart),
                BallStartSpeed = ballStart,
                BallDropSpeed = Clamp(BallDropSpeed, MinBallDropSpeed, ballStart)
            };
        }

        public List<string> GetProblems()
        {
            List<string> problems = new List<string>();
            WheelSpinSettings safe = Sanitized();

            if (!Same(TotalDuration, safe.TotalDuration)) problems.Add("Total Duration must be " + MinDuration + " to " + MaxDuration + ".");
            if (!Same(DropFraction, safe.DropFraction)) problems.Add("Drop Fraction must be " + MinDropFraction + " to " + MaxDropFraction + ".");
            if (!Same(WheelStartSpeed, safe.WheelStartSpeed)) problems.Add("Wheel Start Speed must be 0 to " + MaxWheelSpeed + ".");
            if (!Same(WheelEndSpeed, safe.WheelEndSpeed)) problems.Add("Wheel End Speed must be 0 to the Wheel Start Speed.");
            if (!Same(BallStartSpeed, safe.BallStartSpeed)) problems.Add("Ball Start Speed must be " + MinBallSpeed + " to " + MaxBallSpeed + ".");
            if (!Same(BallDropSpeed, safe.BallDropSpeed)) problems.Add("Ball Drop Speed must be " + MinBallDropSpeed + " to the Ball Start Speed.");
            return problems;
        }

        private static bool Same(float a, float b)
        {
            return !float.IsNaN(a) && Math.Abs(a - b) < 1e-6f;
        }

        // NaN becomes the minimum, so a broken value can never reach the maths.
        private static float Clamp(float value, float min, float max)
        {
            if (float.IsNaN(value)) return min;
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
