using System;
using System.Collections.Generic;

namespace ScatterGame.Core
{
    public sealed class RoundResult
    {
        public long Bet;
        public SpinResult PaidSpin;
        public List<SpinResult> FreeSpins = new List<SpinResult>();

        public bool BonusTriggered;
        public int FreeSpinsAwardedInitial;
        public int FreeSpinRetriggers;
        public int TotalFreeSpinsAwarded;

        public long TotalWinMilli;

        /// <summary>The only amount that should ever be credited to the wallet: whole credits, rounded down once.</summary>
        public long TotalWinCredits => TotalWinMilli / 1000;

        public long NetCredits => TotalWinCredits - Bet;

        public int MaxMultiplier;
        public int TotalCascades;
    }

    /// <summary>
    /// Resolves a whole paid round: the paid spin plus every free spin it earns, all at once.
    /// The wallet adapter will charge the bet once before calling this and credit TotalWinCredits once after.
    /// Free spins never charge a bet.
    /// </summary>
    public static class RoundResolver
    {
        public static RoundResult Resolve(SlotConfig config, long bet, ISymbolSource source, bool validateConfig = true)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (bet <= 0) throw new ArgumentException("The bet must be greater than zero.");
            if (validateConfig) config.ValidateOrThrow();

            RoundResult round = new RoundResult { Bet = bet };

            MultiplierController paidMultiplier = new MultiplierController(config.PaidMultiplierLadder, config.MaxMultiplier);
            round.PaidSpin = SpinResolver.Resolve(config, bet, source, paidMultiplier);
            round.TotalWinMilli = round.PaidSpin.TotalWinMilli;
            round.MaxMultiplier = round.PaidSpin.MaxMultiplier;
            round.TotalCascades = round.PaidSpin.Steps.Count;

            int awarded = Math.Min(config.FreeSpinsFor(round.PaidSpin.ScatterCount), config.MaxTotalFreeSpinsPerRound);
            if (awarded <= 0) return round;

            round.BonusTriggered = true;
            round.FreeSpinsAwardedInitial = awarded;
            round.TotalFreeSpinsAwarded = awarded;

            int remaining = awarded;
            MultiplierController freeMultiplier = new MultiplierController(config.FreeSpinMultiplierLadder, config.MaxMultiplier);

            while (remaining > 0)
            {
                remaining--;
                if (!config.FreeSpinMultiplierPersists) freeMultiplier.Reset();

                SpinResult spin = SpinResolver.Resolve(config, bet, source, freeMultiplier);
                round.FreeSpins.Add(spin);
                round.TotalWinMilli += spin.TotalWinMilli;
                round.TotalCascades += spin.Steps.Count;
                if (spin.MaxMultiplier > round.MaxMultiplier) round.MaxMultiplier = spin.MaxMultiplier;

                // Retrigger: extra spins, limited by the per-round cap. The cap guarantees the loop ends.
                if (spin.ScatterCount >= config.MinScattersForBonus)
                {
                    int room = config.MaxTotalFreeSpinsPerRound - round.TotalFreeSpinsAwarded;
                    int extra = Math.Min(config.RetriggerFreeSpins, room);
                    if (extra > 0)
                    {
                        remaining += extra;
                        round.TotalFreeSpinsAwarded += extra;
                        round.FreeSpinRetriggers++;
                    }
                }
            }

            return round;
        }
    }
}