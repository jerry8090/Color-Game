using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    /// <summary>Where new symbols come from. The engine never calls a random function directly.</summary>
    public interface ISymbolSource
    {
        int NextSymbol(int reel);
    }

    /// <summary>Weighted random symbols from the SlotConfig. A fixed seed gives a repeatable sequence.</summary>
    public sealed class WeightedSymbolSource : ISymbolSource
    {
        private readonly Random rng;
        private readonly int[][] ids = new int[SlotConfig.Reels][];
        private readonly int[][] cumulative = new int[SlotConfig.Reels][];

        public WeightedSymbolSource(SlotConfig config, int seed) : this(config, new Random(seed)) { }

        public WeightedSymbolSource(SlotConfig config, Random random)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            rng = random ?? throw new ArgumentNullException(nameof(random));

            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                List<int> reelIds = new List<int>();
                List<int> reelCumulative = new List<int>();
                int running = 0;

                foreach (SymbolDefinition symbol in config.Symbols)
                {
                    int weight = symbol.ReelWeights[reel];
                    if (weight <= 0) continue;
                    running += weight;
                    reelIds.Add(symbol.Id);
                    reelCumulative.Add(running);
                }

                if (running <= 0) throw new InvalidOperationException("Reel " + (reel + 1) + " has no symbol weight.");
                ids[reel] = reelIds.ToArray();
                cumulative[reel] = reelCumulative.ToArray();
            }
        }

        public int NextSymbol(int reel)
        {
            if (reel < 0 || reel >= SlotConfig.Reels) throw new ArgumentOutOfRangeException(nameof(reel));

            int[] cum = cumulative[reel];
            int roll = rng.Next(cum[cum.Length - 1]);
            for (int i = 0; i < cum.Length; i++)
            {
                if (roll < cum[i]) return ids[reel][i];
            }
            return ids[reel][ids[reel].Length - 1];
        }
    }

    /// <summary>
    /// Test source: you decide every symbol. Values are queued per reel and used in the order the engine asks.
    /// Enqueue in the same order the engine consumes: initial grid first, then each refill, then the next spin.
    /// </summary>
    public sealed class ScriptedSymbolSource : ISymbolSource
    {
        private readonly Queue<int>[] queues = new Queue<int>[SlotConfig.Reels];

        public ScriptedSymbolSource()
        {
            for (int reel = 0; reel < SlotConfig.Reels; reel++) queues[reel] = new Queue<int>();
        }

        public void Enqueue(int reel, params int[] symbolIds)
        {
            foreach (int id in symbolIds) queues[reel].Enqueue(id);
        }

        /// <summary>Queues a whole grid as the next spin's starting grid (top to bottom on every reel).</summary>
        public void EnqueueGrid(SlotGrid grid)
        {
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                for (int row = 0; row < SlotConfig.Rows; row++) queues[reel].Enqueue(grid.Get(reel, row));
            }
        }

        public int Remaining
        {
            get
            {
                int total = 0;
                foreach (Queue<int> queue in queues) total += queue.Count;
                return total;
            }
        }

        public int NextSymbol(int reel)
        {
            if (queues[reel].Count == 0)
            {
                throw new InvalidOperationException("The scripted source ran out of symbols for reel " + (reel + 1) + ".");
            }
            return queues[reel].Dequeue();
        }
    }

    /// <summary>Generates the starting grid of a spin: reel by reel, top to bottom.</summary>
    public static class GridGenerator
    {
        public static SlotGrid Generate(ISymbolSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            SlotGrid grid = new SlotGrid();
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                for (int row = 0; row < SlotConfig.Rows; row++) grid.Set(reel, row, source.NextSymbol(reel));
            }
            return grid;
        }
    }
}