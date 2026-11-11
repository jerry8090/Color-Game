using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    public enum BetType
    {
        Straight,
        Split,
        Street,
        Corner,
        SixLine,
        Red,
        Black,
        Odd,
        Even,
        Low,
        High,
        Dozen,
        Column
    }

    /// <summary>
    /// One bet: a type, the exact numbers it covers, and a stake in whole credits. Immutable.
    /// Every bet is validated when created, so an impossible shape (for example a split of 3 and 4) can never exist.
    /// The covered numbers are the single source of truth for winning. Zero is never covered by an outside bet.
    /// </summary>
    public sealed class RouletteBet
    {
        private readonly int[] numbers;

        public BetType Type { get; }
        public long Amount { get; }
        public IReadOnlyList<int> Numbers { get; }

        /// <summary>Same key = same betting area (the bet slip merges chips with equal keys).</summary>
        public string Key { get; }

        public int Multiplier => RoulettePayout.MultiplierFor(Type);

        private RouletteBet(BetType type, int[] sortedNumbers, long amount)
        {
            Type = type;
            numbers = sortedNumbers;
            Numbers = Array.AsReadOnly(numbers);
            Amount = amount;
            Key = type + ":" + string.Join("-", numbers);
        }

        public bool Covers(int number)
        {
            return Array.BinarySearch(numbers, number) >= 0;
        }

        public RouletteBet WithAmount(long amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "The bet amount must be greater than zero.");
            return new RouletteBet(Type, numbers, amount);
        }

        // ---------- Factories ----------

        public static RouletteBet Straight(int number, long amount) { return Create(BetType.Straight, new[] { number }, amount); }
        public static RouletteBet Split(int a, int b, long amount) { return Create(BetType.Split, new[] { a, b }, amount); }

        /// <summary>A row of three. Pass the first number of the row: 1, 4, 7 ... 34.</summary>
        public static RouletteBet Street(int firstNumber, long amount)
        {
            return Create(BetType.Street, Run(firstNumber, 3), amount);
        }

        /// <summary>Four numbers in a square. Pass the lowest number: for example 1 covers 1, 2, 4, 5.</summary>
        public static RouletteBet Corner(int lowestNumber, long amount)
        {
            return Create(BetType.Corner, new[] { lowestNumber, lowestNumber + 1, lowestNumber + 3, lowestNumber + 4 }, amount);
        }

        /// <summary>Two rows (six numbers). Pass the first number of the first row: 1, 4 ... 31.</summary>
        public static RouletteBet SixLine(int firstNumber, long amount)
        {
            return Create(BetType.SixLine, Run(firstNumber, 6), amount);
        }

        public static RouletteBet Red(long amount) { return Create(BetType.Red, OutsideSet(BetType.Red), amount); }
        public static RouletteBet Black(long amount) { return Create(BetType.Black, OutsideSet(BetType.Black), amount); }
        public static RouletteBet Odd(long amount) { return Create(BetType.Odd, OutsideSet(BetType.Odd), amount); }
        public static RouletteBet Even(long amount) { return Create(BetType.Even, OutsideSet(BetType.Even), amount); }
        public static RouletteBet Low(long amount) { return Create(BetType.Low, OutsideSet(BetType.Low), amount); }
        public static RouletteBet High(long amount) { return Create(BetType.High, OutsideSet(BetType.High), amount); }

        /// <summary>Dozen 1 = 1-12, 2 = 13-24, 3 = 25-36.</summary>
        public static RouletteBet Dozen(int dozen, long amount)
        {
            if (dozen < 1 || dozen > 3) throw new ArgumentOutOfRangeException(nameof(dozen), "A dozen is 1, 2 or 3.");
            return Create(BetType.Dozen, Run((dozen - 1) * 12 + 1, 12), amount);
        }

        /// <summary>Column 1 = 1,4..34, 2 = 2,5..35, 3 = 3,6..36.</summary>
        public static RouletteBet Column(int column, long amount)
        {
            if (column < 1 || column > 3) throw new ArgumentOutOfRangeException(nameof(column), "A column is 1, 2 or 3.");
            int[] set = new int[12];
            for (int i = 0; i < set.Length; i++) set[i] = column + i * 3;
            return Create(BetType.Column, set, amount);
        }

        public static RouletteBet Create(BetType type, IReadOnlyList<int> coveredNumbers, long amount)
        {
            if (!TryCreate(type, coveredNumbers, amount, out RouletteBet bet, out string error)) throw new ArgumentException(error);
            return bet;
        }

        /// <summary>Safe version for UI and loaded data: never throws, explains why a bet is refused.</summary>
        public static bool TryCreate(BetType type, IReadOnlyList<int> coveredNumbers, long amount, out RouletteBet bet, out string error)
        {
            bet = null;

            if (!Enum.IsDefined(typeof(BetType), type)) { error = "Unknown bet type."; return false; }
            if (amount <= 0) { error = "The bet amount must be greater than zero."; return false; }
            if (coveredNumbers == null || coveredNumbers.Count == 0) { error = "A bet must cover at least one number."; return false; }

            int[] sorted = new int[coveredNumbers.Count];
            for (int i = 0; i < sorted.Length; i++) sorted[i] = coveredNumbers[i];
            Array.Sort(sorted);

            if (!Validate(type, sorted, out error)) return false;

            bet = new RouletteBet(type, sorted, amount);
            return true;
        }

        // ---------- Validation ----------

        private static bool Validate(BetType type, int[] n, out string error)
        {
            error = null;

            foreach (int number in n)
            {
                if (!RouletteLayout.IsValidNumber(number)) { error = "Number " + number + " is not on the wheel."; return false; }
            }
            for (int i = 1; i < n.Length; i++)
            {
                if (n[i] == n[i - 1]) { error = "A bet cannot list the same number twice."; return false; }
            }

            bool ok;
            switch (type)
            {
                case BetType.Straight:
                    ok = n.Length == 1;
                    break;
                case BetType.Split:
                    ok = n.Length == 2 && IsAdjacentPair(n[0], n[1]);
                    break;
                case BetType.Street:
                    ok = n.Length == 3 && n[0] >= 1 && n[0] <= 34 && n[0] % 3 == 1 && IsRun(n, 1);
                    break;
                case BetType.Corner:
                    ok = n.Length == 4 && n[0] >= 1 && n[0] <= 32 && n[0] % 3 != 0 &&
                         n[1] == n[0] + 1 && n[2] == n[0] + 3 && n[3] == n[0] + 4;
                    break;
                case BetType.SixLine:
                    ok = n.Length == 6 && n[0] >= 1 && n[0] <= 31 && n[0] % 3 == 1 && IsRun(n, 1);
                    break;
                case BetType.Dozen:
                    ok = n.Length == 12 && (n[0] == 1 || n[0] == 13 || n[0] == 25) && IsRun(n, 1);
                    break;
                case BetType.Column:
                    ok = n.Length == 12 && n[0] >= 1 && n[0] <= 3 && IsRun(n, 3);
                    break;
                default:
                    ok = SameSet(n, OutsideSet(type));
                    break;
            }

            if (!ok) error = "These numbers are not a valid " + type + " bet.";
            return ok;
        }

        // Neighbors on the table: side by side in a row, or one above the other. The zero touches 1, 2 and 3.
        private static bool IsAdjacentPair(int a, int b)
        {
            if (a == 0) return b >= 1 && b <= 3;
            return b - a == 3 || (b - a == 1 && a % 3 != 0);
        }

        private static bool IsRun(int[] n, int step)
        {
            for (int i = 1; i < n.Length; i++)
            {
                if (n[i] - n[i - 1] != step) return false;
            }
            return true;
        }

        private static int[] Run(int first, int count)
        {
            int[] set = new int[count];
            for (int i = 0; i < count; i++) set[i] = first + i;
            return set;
        }

        private static bool SameSet(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        /// <summary>The numbers an even-money bet covers (sorted). Zero is never included.</summary>
        private static int[] OutsideSet(BetType type)
        {
            List<int> list = new List<int>();
            for (int n = 1; n <= RouletteLayout.MaxNumber; n++)
            {
                bool include;
                switch (type)
                {
                    case BetType.Red: include = RouletteLayout.IsRed(n); break;
                    case BetType.Black: include = RouletteLayout.IsBlack(n); break;
                    case BetType.Odd: include = RouletteLayout.IsOdd(n); break;
                    case BetType.Even: include = RouletteLayout.IsEven(n); break;
                    case BetType.Low: include = RouletteLayout.IsLow(n); break;
                    case BetType.High: include = RouletteLayout.IsHigh(n); break;
                    default: include = false; break;
                }
                if (include) list.Add(n);
            }
            return list.ToArray();
        }
    }
}
