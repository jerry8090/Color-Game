using System;
using System.Globalization;

namespace RouletteGame.Core
{
    /// <summary>How bets, colors and amounts are written on screen. Pure text logic, so it is unit tested.</summary>
    public static class RouletteText
    {
        public static string ColorName(PocketColor color)
        {
            switch (color)
            {
                case PocketColor.Green: return "GREEN";
                case PocketColor.Red: return "RED";
                default: return "BLACK";
            }
        }

        public static string Describe(RouletteBet bet)
        {
            if (bet == null) throw new ArgumentNullException(nameof(bet));
            int first = bet.Numbers[0];
            int last = bet.Numbers[bet.Numbers.Count - 1];

            switch (bet.Type)
            {
                case BetType.Straight: return "STRAIGHT " + first;
                case BetType.Split: return "SPLIT " + first + "-" + last;
                case BetType.Street: return "STREET " + first + "-" + last;
                case BetType.Corner: return "CORNER " + string.Join("-", bet.Numbers);
                case BetType.SixLine: return "SIX LINE " + first + "-" + last;
                case BetType.Red: return "RED";
                case BetType.Black: return "BLACK";
                case BetType.Odd: return "ODD";
                case BetType.Even: return "EVEN";
                case BetType.Low: return "LOW 1-18";
                case BetType.High: return "HIGH 19-36";
                case BetType.Dozen: return first == 1 ? "1ST DOZEN" : (first == 13 ? "2ND DOZEN" : "3RD DOZEN");
                case BetType.Column: return "COLUMN " + first;
                default: return bet.Type.ToString().ToUpperInvariant();
            }
        }

        public static string Credits(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static string Signed(long value)
        {
            return value.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);
        }
    }
}
