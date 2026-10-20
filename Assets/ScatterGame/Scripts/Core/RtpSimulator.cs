using System;

namespace ScatterGame.Core
{
    public sealed class SimulationReport
    {
        public long Rounds;
        public long TotalBetCredits;
        public long TotalWinMilli;
        public long TotalWinCreditsSettled;
        public long RoundsWithWin;
        public long BonusRounds;
        public long TotalFreeSpins;
        public double LargestWinInBets;
        public int MaxMultiplierSeen;
        public int MaxCascadesInOneSpin;
        public long CascadeLimitHits;

        /// <summary>Return to player using exact milli-credits (no rounding loss).</summary>
        public double Rtp => TotalBetCredits == 0 ? 0 : TotalWinMilli / (1000.0 * TotalBetCredits);
        
        /// <summary>RTP after flooring each round to whole credits, which is what players actually receive.</summary>
        public double SettledRtp => TotalBetCredits == 0 ? 0 : (double)TotalWinCreditsSettled / TotalBetCredits;

        public double HitFrequency => Rounds == 0 ? 0 : (double)RoundsWithWin / Rounds;
        public double BonusFrequency => Rounds == 0 ? 0 : (double)BonusRounds / Rounds;

        public override string ToString()
        {
            return "Rounds " + Rounds + " | RTP " + (Rtp * 100).ToString("F2") + "% (settled " + (SettledRtp * 100).ToString("F2") + "%) | Hit " + (HitFrequency * 100).ToString("F2") +
                   "% | Bonus every " + (BonusRounds == 0 ? "never" : (Rounds / (double)BonusRounds).ToString("F1") + " rounds") +
                   " | Largest win " + LargestWinInBets.ToString("F1") + "x bet | Max multiplier " + MaxMultiplierSeen +
                   " | Cascade limit hits " + CascadeLimitHits;
        }
    }

    /// <summary>Measures a configuration by playing many rounds. For tuning only. It never influences real outcomes.</summary>
    public static class RtpSimulator
    {
        public static SimulationReport Run(SlotConfig config, int rounds, int seed, long bet = 100)
        {
            config.ValidateOrThrow();
            WeightedSymbolSource source = new WeightedSymbolSource(config, seed);
            SimulationReport report = new SimulationReport();

            for (int i = 0; i < rounds; i++)
            {
                RoundResult round = RoundResolver.Resolve(config, bet, source, false);

                report.Rounds++;
                report.TotalBetCredits += bet;
                report.TotalWinMilli += round.TotalWinMilli;
                report.TotalWinCreditsSettled += round.TotalWinCredits;
                if (round.TotalWinCredits > 0) report.RoundsWithWin++;
                if (round.BonusTriggered) report.BonusRounds++;
                report.TotalFreeSpins += round.FreeSpins.Count;

                double inBets = round.TotalWinMilli / (1000.0 * bet);
                if (inBets > report.LargestWinInBets) report.LargestWinInBets = inBets;
                if (round.MaxMultiplier > report.MaxMultiplierSeen) report.MaxMultiplierSeen = round.MaxMultiplier;

                Track(report, round.PaidSpin);
                foreach (SpinResult spin in round.FreeSpins) Track(report, spin);
            }

            return report;
        }

        private static void Track(SimulationReport report, SpinResult spin)
        {
            if (spin.Steps.Count > report.MaxCascadesInOneSpin) report.MaxCascadesInOneSpin = spin.Steps.Count;
            if (spin.HitCascadeLimit) report.CascadeLimitHits++;
        }
    }
}