using System;
using System.Collections.Generic;
using NUnit.Framework;
using RouletteGame.Core;

namespace RouletteGame.Tests
{
    /// <summary>
    /// Fairness and long-run checks. Every random source here uses a FIXED seed, so these tests give the same answer
    /// on every machine and every run: they can never fail "by bad luck".
    /// </summary>
    public class FairnessAndSoakTests
    {
        // ---------- The random source ----------

        [Test]
        public void UniformMapping_ThrowsAwayTheBiasedTail_AndKeepsAnExactMultipleOf37()
        {
            const ulong space = 1UL << 32;
            ulong cutoff = space - (space % 37);

            Assert.AreEqual(0UL, cutoff % 37, "The accepted range must be an exact multiple of 37.");
            Assert.Less(space - cutoff, 37UL, "Only a tiny tail (fewer than 37 values) may be rejected.");

            Assert.IsTrue(CryptoOutcomeSource.TryMap(0, out int n));
            Assert.AreEqual(0, n);
            Assert.IsTrue(CryptoOutcomeSource.TryMap(36, out n));
            Assert.AreEqual(36, n);
            Assert.IsTrue(CryptoOutcomeSource.TryMap(37, out n));
            Assert.AreEqual(0, n);
            Assert.IsTrue(CryptoOutcomeSource.TryMap((uint)(cutoff - 1), out n));
            Assert.AreEqual(36, n);

            Assert.IsFalse(CryptoOutcomeSource.TryMap((uint)cutoff, out n));
            Assert.AreEqual(-1, n);
            Assert.IsFalse(CryptoOutcomeSource.TryMap(uint.MaxValue, out _));
        }

        [Test]
        public void UniformMapping_GivesEveryNumberTheSameShareOfValues()
        {
            // Walk a block of accepted values: every number must come up exactly as often as every other.
            int[] counts = new int[37];
            for (uint raw = 0; raw < 37u * 1000u; raw++)
            {
                Assert.IsTrue(CryptoOutcomeSource.TryMap(raw, out int n));
                counts[n]++;
            }
            for (int n = 0; n <= 36; n++) Assert.AreEqual(1000, counts[n], "Number " + n);
        }

        [Test]
        public void SeededSource_PassesAChiSquareUniformityCheck()
        {
            const int draws = 74000;
            const double expected = draws / 37.0;
            int[] counts = new int[37];
            SeededOutcomeSource source = new SeededOutcomeSource(12345);
            for (int i = 0; i < draws; i++) counts[source.NextNumber()]++;

            double chi = 0;
            foreach (int count in counts) chi += (count - expected) * (count - expected) / expected;

            // 36 degrees of freedom: 68 is the 0.1% limit for a fair wheel. 80 leaves a wide safety margin.
            Assert.Less(chi, 80.0, "chi-square " + chi);
        }

        // ---------- Every bet that exists ----------

        private static List<RouletteBet> EveryValidBet()
        {
            List<RouletteBet> list = new List<RouletteBet>();
            for (int n = 0; n <= 36; n++) list.Add(RouletteBet.Straight(n, 1));

            for (int a = 0; a <= 36; a++)
            {
                for (int b = a + 1; b <= 36; b++)
                {
                    if (RouletteBet.TryCreate(BetType.Split, new[] { a, b }, 1, out RouletteBet split, out _)) list.Add(split);
                }
            }

            for (int first = 1; first <= 34; first += 3) list.Add(RouletteBet.Street(first, 1));
            for (int lowest = 1; lowest <= 32; lowest++)
            {
                if (RouletteBet.TryCreate(BetType.Corner, new[] { lowest, lowest + 1, lowest + 3, lowest + 4 }, 1, out RouletteBet corner, out _)) list.Add(corner);
            }
            for (int first = 1; first <= 31; first += 3) list.Add(RouletteBet.SixLine(first, 1));

            list.Add(RouletteBet.Red(1)); list.Add(RouletteBet.Black(1)); list.Add(RouletteBet.Odd(1));
            list.Add(RouletteBet.Even(1)); list.Add(RouletteBet.Low(1)); list.Add(RouletteBet.High(1));
            for (int d = 1; d <= 3; d++) { list.Add(RouletteBet.Dozen(d, 1)); list.Add(RouletteBet.Column(d, 1)); }
            return list;
        }

        [Test]
        public void TheRulesAllowExactlyTheStandardNumberOfBets_FoundByBruteForce()
        {
            // Try EVERY possible set of numbers and count how many the rules accept. A wrong validator shows up here.
            int splits = 0, streets = 0, corners = 0;

            for (int a = 0; a <= 36; a++)
                for (int b = a + 1; b <= 36; b++)
                    if (RouletteBet.TryCreate(BetType.Split, new[] { a, b }, 1, out _, out _)) splits++;

            for (int a = 0; a <= 36; a++)
                for (int b = a + 1; b <= 36; b++)
                    for (int c = b + 1; c <= 36; c++)
                        if (RouletteBet.TryCreate(BetType.Street, new[] { a, b, c }, 1, out _, out _)) streets++;

            for (int a = 0; a <= 36; a++)
                for (int b = a + 1; b <= 36; b++)
                    for (int c = b + 1; c <= 36; c++)
                        for (int d = c + 1; d <= 36; d++)
                            if (RouletteBet.TryCreate(BetType.Corner, new[] { a, b, c, d }, 1, out _, out _)) corners++;

            Assert.AreEqual(60, splits);   // 24 side by side + 33 above each other + 3 with the zero
            Assert.AreEqual(12, streets);
            Assert.AreEqual(22, corners);
            Assert.AreEqual(37 + 60 + 12 + 22 + 11 + 6 + 3 + 3, EveryValidBet().Count);
        }

        [Test]
        public void EveryValidBet_ReturnsExactly36ForEvery37Numbers()
        {
            foreach (RouletteBet bet in EveryValidBet())
            {
                long total = 0;
                for (int n = 0; n <= 36; n++) total += RoulettePayout.EvaluateBet(bet, n).TotalReturn;
                Assert.AreEqual(36, total, bet.Key); // 36 returned per 37 spins of 1 credit = the 1/37 house edge, for all bets
            }
        }

        // ---------- Long runs ----------

        [Test]
        public void SimulatedReturnToPlayer_MatchesTheoryForEveryBetType()
        {
            const int rounds = 200000;
            const double theory = 36.0 / 37.0;

            RouletteBet[] samples =
            {
                RouletteBet.Straight(17, 1), RouletteBet.Split(8, 9, 1), RouletteBet.Street(7, 1), RouletteBet.Corner(8, 1),
                RouletteBet.SixLine(13, 1), RouletteBet.Red(1), RouletteBet.Black(1), RouletteBet.Odd(1), RouletteBet.Even(1),
                RouletteBet.Low(1), RouletteBet.High(1), RouletteBet.Dozen(2, 1), RouletteBet.Column(3, 1)
            };

            SeededOutcomeSource source = new SeededOutcomeSource(2024);
            long[] returned = new long[samples.Length];
            for (int i = 0; i < rounds; i++)
            {
                int number = source.NextNumber();
                for (int b = 0; b < samples.Length; b++) returned[b] += RoulettePayout.EvaluateBet(samples[b], number).TotalReturn;
            }

            for (int b = 0; b < samples.Length; b++)
            {
                double p = samples[b].Numbers.Count / 37.0;
                double payoff = samples[b].Multiplier + 1;
                double sd = Math.Sqrt(p * payoff * payoff - theory * theory);
                double tolerance = 6.0 * sd / Math.Sqrt(rounds); // six standard errors

                double actual = returned[b] / (double)rounds;
                Assert.AreEqual(theory, actual, tolerance, samples[b].Key);
            }
        }

        [Test]
        public void SoakTest_WalletAndSettlement_StayConsistentOver20000Rounds()
        {
            const int rounds = 20000;
            const long start = 100000000;

            List<RouletteBet> pool = EveryValidBet();
            System.Random choose = new System.Random(7);
            FakeWallet wallet = new FakeWallet(start);
            RouletteRoundSettler settler = new RouletteRoundSettler(wallet);
            SeededOutcomeSource played = new SeededOutcomeSource(99);
            SeededOutcomeSource predicted = new SeededOutcomeSource(99); // the same numbers, drawn independently

            long expected = start;
            int expectedCredits = 0;

            for (int round = 1; round <= rounds; round++)
            {
                int count = choose.Next(1, 5);
                List<RouletteBet> bets = new List<RouletteBet>();
                for (int i = 0; i < count; i++) bets.Add(pool[choose.Next(pool.Count)].WithAmount(choose.Next(1, 51)));

                RoundPayout predictedPayout = RoulettePayout.Evaluate(bets, predicted.NextNumber());
                expected += predictedPayout.TotalReturned - predictedPayout.TotalStake;
                if (predictedPayout.TotalReturned > 0) expectedCredits++;

                RouletteSettlementResult result = settler.Play(bets, "SOAK-" + round, played);
                Assert.AreEqual(RouletteSettlementStatus.Completed, result.Status, "round " + round);
                Assert.AreEqual(predictedPayout.WinningNumber, result.WinningNumber, "round " + round);
            }

            Assert.AreEqual(expected, wallet.Balance);
            Assert.AreEqual(rounds, wallet.SpendCalls);        // exactly one charge per round
            Assert.AreEqual(expectedCredits, wallet.CreditCalls); // exactly one payout per winning round, none for losing rounds

            // Every round id was charged once and paid at most once, and the charge always came first.
            HashSet<string> spent = new HashSet<string>();
            HashSet<string> paid = new HashSet<string>();
            foreach (string call in wallet.Calls)
            {
                string id = call.Substring(call.IndexOf(':') + 1);
                if (call.StartsWith("spend:")) Assert.IsTrue(spent.Add(id), "charged twice: " + id);
                else
                {
                    Assert.IsTrue(spent.Contains(id), "paid before it was charged: " + id);
                    Assert.IsTrue(paid.Add(id), "paid twice: " + id);
                }
            }
        }
    }
}
