using System;

namespace RouletteGame.Core
{
    /// <summary>Table limits, in whole credits. Used by the bet slip AND checked again by the settler.</summary>
    public sealed class RouletteLimits
    {
        public long MaxBetPerArea { get; }
        public long MaxTotalBet { get; }

        public RouletteLimits(long maxBetPerArea, long maxTotalBet)
        {
            if (maxBetPerArea <= 0) throw new ArgumentOutOfRangeException(nameof(maxBetPerArea));
            if (maxTotalBet <= 0) throw new ArgumentOutOfRangeException(nameof(maxTotalBet));
            MaxBetPerArea = maxBetPerArea;
            MaxTotalBet = maxTotalBet;
        }

        public static RouletteLimits Default => new RouletteLimits(100000, 1000000);
    }
}
