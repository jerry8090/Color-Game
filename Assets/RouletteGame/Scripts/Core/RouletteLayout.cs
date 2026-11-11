using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    public enum PocketColor
    {
        Green,
        Red,
        Black
    }

    /// <summary>
    /// European (single zero) roulette: 37 pockets, the real wheel order, colors and number properties.
    /// Pure data and rules. No Unity objects, no randomness.
    /// Table layout: 12 rows of 3. Row r holds 3r+1, 3r+2, 3r+3. Column 1 = 1,4,7..34, column 2 = 2,5..35, column 3 = 3,6..36.
    /// </summary>
    public static class RouletteLayout
    {
        public const int PocketCount = 37;
        public const int MaxNumber = 36;

        // Clockwise wheel order, starting at the zero.
        private static readonly int[] WheelNumbers =
        {
            0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26
        };

        private static readonly int[] RedNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

        private static readonly int[] IndexByNumber = new int[PocketCount];
        private static readonly bool[] RedFlags = new bool[PocketCount];
        private static readonly IReadOnlyList<int> WheelOrderView = Array.AsReadOnly(WheelNumbers);

        static RouletteLayout()
        {
            for (int i = 0; i < WheelNumbers.Length; i++) IndexByNumber[WheelNumbers[i]] = i;
            foreach (int number in RedNumbers) RedFlags[number] = true;
        }

        /// <summary>The 37 numbers in clockwise wheel order, starting with 0.</summary>
        public static IReadOnlyList<int> WheelOrder => WheelOrderView;

        public static bool IsValidNumber(int number)
        {
            return number >= 0 && number <= MaxNumber;
        }

        public static PocketColor ColorOf(int number)
        {
            Require(number);
            if (number == 0) return PocketColor.Green;
            return RedFlags[number] ? PocketColor.Red : PocketColor.Black;
        }

        public static bool IsRed(int number) { return ColorOf(number) == PocketColor.Red; }
        public static bool IsBlack(int number) { return ColorOf(number) == PocketColor.Black; }

        // Zero is neither odd nor even, neither low nor high, and in no dozen or column.
        public static bool IsOdd(int number) { Require(number); return number > 0 && number % 2 == 1; }
        public static bool IsEven(int number) { Require(number); return number > 0 && number % 2 == 0; }
        public static bool IsLow(int number) { Require(number); return number >= 1 && number <= 18; }
        public static bool IsHigh(int number) { Require(number); return number >= 19; }

        /// <summary>1, 2 or 3 (1-12, 13-24, 25-36). 0 for the zero.</summary>
        public static int DozenOf(int number)
        {
            Require(number);
            return number == 0 ? 0 : (number - 1) / 12 + 1;
        }

        /// <summary>1, 2 or 3. 0 for the zero.</summary>
        public static int ColumnOf(int number)
        {
            Require(number);
            return number == 0 ? 0 : (number - 1) % 3 + 1;
        }

        /// <summary>Position of a number on the wheel (0 to 36, clockwise from the zero).</summary>
        public static int IndexOf(int number)
        {
            Require(number);
            return IndexByNumber[number];
        }

        public static int NumberAt(int wheelIndex)
        {
            if (wheelIndex < 0 || wheelIndex >= PocketCount) throw new ArgumentOutOfRangeException(nameof(wheelIndex));
            return WheelNumbers[wheelIndex];
        }

        /// <summary>Angle of the pocket's center in degrees, clockwise from the zero pocket (0 to under 360).</summary>
        public static double PocketAngleDegrees(int number)
        {
            return IndexOf(number) * (360.0 / PocketCount);
        }

        private static void Require(int number)
        {
            if (!IsValidNumber(number)) throw new ArgumentOutOfRangeException(nameof(number), number, "A roulette number must be 0 to 36.");
        }
    }
}
