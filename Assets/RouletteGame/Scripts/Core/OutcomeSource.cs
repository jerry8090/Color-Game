using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RouletteGame.Core
{
    /// <summary>Where the winning number comes from. Nothing else in the game may choose a number.</summary>
    public interface IOutcomeSource
    {
        /// <summary>Returns a winning number from 0 to 36.</summary>
        int NextNumber();
    }

    /// <summary>
    /// The real source: operating-system cryptographic randomness with rejection sampling,
    /// so all 37 numbers have exactly the same probability (no modulo bias).
    /// Prototype note: this is strong for a local game, but a real-money version must draw on a server.
    /// </summary>
    public sealed class CryptoOutcomeSource : IOutcomeSource, IDisposable
    {
        private const ulong Space = 1UL << 32;
        private const ulong Cutoff = Space - (Space % RouletteLayout.PocketCount); // largest multiple of 37 that fits

        private readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        private readonly byte[] buffer = new byte[4];
        private readonly object gate = new object();

        /// <summary>
        /// Turns one random 32-bit value into a number from 0 to 36. Returns false for the few values at the very top
        /// of the range, which are thrown away: 2^32 is not a multiple of 37, so using them would favour the low numbers.
        /// Public so the fairness rule itself can be tested.
        /// </summary>
        public static bool TryMap(uint raw, out int number)
        {
            if (raw >= Cutoff)
            {
                number = -1;
                return false;
            }

            number = (int)(raw % RouletteLayout.PocketCount);
            return true;
        }

        public int NextNumber()
        {
            lock (gate)
            {
                while (true)
                {
                    rng.GetBytes(buffer);
                    if (TryMap(BitConverter.ToUInt32(buffer, 0), out int number)) return number;
                }
            }
        }

        public void Dispose()
        {
            rng.Dispose();
        }
    }

    /// <summary>Repeatable numbers for simulations. A fixed seed gives the same sequence. Not for real play.</summary>
    public sealed class SeededOutcomeSource : IOutcomeSource
    {
        private readonly Random random;

        public SeededOutcomeSource(int seed)
        {
            random = new Random(seed);
        }

        public int NextNumber()
        {
            return random.Next(RouletteLayout.PocketCount);
        }
    }

    /// <summary>Test source: you decide every winning number, in order.</summary>
    public sealed class ScriptedOutcomeSource : IOutcomeSource
    {
        private readonly Queue<int> queue = new Queue<int>();

        public ScriptedOutcomeSource(params int[] numbers)
        {
            Enqueue(numbers);
        }

        public int Remaining => queue.Count;

        public void Enqueue(params int[] numbers)
        {
            foreach (int number in numbers)
            {
                if (!RouletteLayout.IsValidNumber(number)) throw new ArgumentOutOfRangeException(nameof(numbers), number, "A roulette number must be 0 to 36.");
                queue.Enqueue(number);
            }
        }

        public int NextNumber()
        {
            if (queue.Count == 0) throw new InvalidOperationException("The scripted outcome source ran out of numbers.");
            return queue.Dequeue();
        }
    }
}
