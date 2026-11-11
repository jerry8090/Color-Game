using System;
using System.Collections.Generic;
using NUnit.Framework;
using RouletteGame.Core;
using UnityEngine;

namespace RouletteGame.Tests
{
    public class TextAndHistoryTests
    {
        private static RouletteRoundRecord Record(string id, long bet, long returned, int number = 5)
        {
            return new RouletteRoundRecord { roundId = id, winningNumber = number, totalBet = bet, totalReturned = returned, net = returned - bet };
        }

        private static RouletteSettlementResult Play(FakeWallet wallet, int number, params RouletteBet[] bets)
        {
            return new RouletteRoundSettler(wallet).Play(bets, "ROULETTE-000001", new ScriptedOutcomeSource(number));
        }

        // ---------- Text ----------

        [Test]
        public void Describe_NamesEveryBetType()
        {
            Assert.AreEqual("STRAIGHT 7", RouletteText.Describe(RouletteBet.Straight(7, 1)));
            Assert.AreEqual("SPLIT 1-2", RouletteText.Describe(RouletteBet.Split(1, 2, 1)));
            Assert.AreEqual("STREET 4-6", RouletteText.Describe(RouletteBet.Street(4, 1)));
            Assert.AreEqual("CORNER 1-2-4-5", RouletteText.Describe(RouletteBet.Corner(1, 1)));
            Assert.AreEqual("SIX LINE 1-6", RouletteText.Describe(RouletteBet.SixLine(1, 1)));
            Assert.AreEqual("RED", RouletteText.Describe(RouletteBet.Red(1)));
            Assert.AreEqual("BLACK", RouletteText.Describe(RouletteBet.Black(1)));
            Assert.AreEqual("ODD", RouletteText.Describe(RouletteBet.Odd(1)));
            Assert.AreEqual("EVEN", RouletteText.Describe(RouletteBet.Even(1)));
            Assert.AreEqual("LOW 1-18", RouletteText.Describe(RouletteBet.Low(1)));
            Assert.AreEqual("HIGH 19-36", RouletteText.Describe(RouletteBet.High(1)));
            Assert.AreEqual("1ST DOZEN", RouletteText.Describe(RouletteBet.Dozen(1, 1)));
            Assert.AreEqual("2ND DOZEN", RouletteText.Describe(RouletteBet.Dozen(2, 1)));
            Assert.AreEqual("3RD DOZEN", RouletteText.Describe(RouletteBet.Dozen(3, 1)));
            Assert.AreEqual("COLUMN 3", RouletteText.Describe(RouletteBet.Column(3, 1)));
            Assert.Throws<ArgumentNullException>(() => RouletteText.Describe(null));
        }

        [Test]
        public void ColorNamesAndAmounts_AreFormatted()
        {
            Assert.AreEqual("GREEN", RouletteText.ColorName(PocketColor.Green));
            Assert.AreEqual("RED", RouletteText.ColorName(PocketColor.Red));
            Assert.AreEqual("BLACK", RouletteText.ColorName(PocketColor.Black));
            Assert.AreEqual("1,234", RouletteText.Credits(1234));
            Assert.AreEqual("+350", RouletteText.Signed(350));
            Assert.AreEqual("-50", RouletteText.Signed(-50));
            Assert.AreEqual("0", RouletteText.Signed(0));
        }

        // ---------- History ----------

        [Test]
        public void RoundIds_AreUniqueAndPadded()
        {
            RouletteHistoryService service = new RouletteHistoryService(10);
            Assert.AreEqual("ROULETTE-000001", service.ReserveNextRoundId());
            Assert.AreEqual("ROULETTE-000002", service.ReserveNextRoundId());
            Assert.AreEqual(2, service.LastRoundNumber);
        }

        [Test]
        public void AddRound_UpdatesStatistics()
        {
            RouletteHistoryService service = new RouletteHistoryService(10);
            service.AddRound(Record("ROULETTE-000001", 100, 0, 0));      // lost, zero
            service.AddRound(Record("ROULETTE-000002", 100, 360, 17));   // won, +260
            service.AddRound(Record("ROULETTE-000003", 50, 100, 2));     // won, +50

            RouletteStatistics s = service.Statistics;
            Assert.AreEqual(3, s.totalRounds);
            Assert.AreEqual(2, s.winningRounds);
            Assert.AreEqual(250, s.totalWagered);
            Assert.AreEqual(460, s.totalReturned);
            Assert.AreEqual(210, s.NetCredits);
            Assert.AreEqual(260, s.largestWin);
            Assert.AreEqual(1, s.zeroCount);
        }

        [Test]
        public void Trim_RemovesOldestRounds_ButKeepsStatistics()
        {
            RouletteHistoryService service = new RouletteHistoryService(3);
            for (int i = 1; i <= 5; i++) service.AddRound(Record("ROULETTE-00000" + i, 10, 0));

            Assert.AreEqual(3, service.Rounds.Count);
            Assert.AreEqual("ROULETTE-000003", service.Rounds[0].roundId);
            Assert.AreEqual(5, service.Statistics.totalRounds);
        }

        [Test]
        public void RecentNumbers_AreNewestFirst_AndLimited()
        {
            RouletteHistoryService service = new RouletteHistoryService(10);
            foreach (int n in new[] { 1, 2, 3, 4, 5 }) service.AddRound(Record("ROULETTE-00000" + n, 10, 0, n));

            CollectionAssert.AreEqual(new[] { 5, 4, 3 }, service.RecentNumbers(3));
            Assert.AreEqual(5, service.RecentNumbers(99).Count);
            Assert.IsEmpty(new RouletteHistoryService(5).RecentNumbers(3));
        }

        [Test]
        public void ClearHistory_ResetsRoundsAndStats_ButKeepsTheIdCounter()
        {
            RouletteHistoryService service = new RouletteHistoryService(10);
            service.ReserveNextRoundId();
            service.AddRound(Record("ROULETTE-000001", 100, 0));

            service.ClearHistory();

            Assert.AreEqual(0, service.Rounds.Count);
            Assert.AreEqual(0, service.Statistics.totalRounds);
            Assert.AreEqual("ROULETTE-000002", service.ReserveNextRoundId());
        }

        [Test]
        public void LoadState_NeverRepeatsAnId_EvenWithAStaleCounter()
        {
            RouletteHistoryService service = new RouletteHistoryService(10);
            service.LoadState(0, new List<RouletteRoundRecord> { Record("ROULETTE-000007", 10, 0) }, null);
            Assert.AreEqual("ROULETTE-000008", service.ReserveNextRoundId());
            Assert.IsNotNull(service.Statistics);
        }

        [Test]
        public void EmptyOrOlderSaveData_LoadsWithSafeDefaults()
        {
            RouletteHistorySaveData data = JsonUtility.FromJson<RouletteHistorySaveData>("{}");
            RouletteHistoryService service = new RouletteHistoryService(10);

            Assert.DoesNotThrow(() => service.LoadState(data.lastRoundNumber, data.rounds, data.statistics));
            Assert.AreEqual(0, service.Rounds.Count);
            Assert.AreEqual("ROULETTE-000001", service.ReserveNextRoundId());
        }

        [Test]
        public void FromSettlement_RecordsAWinningRoundCorrectly()
        {
            FakeWallet wallet = new FakeWallet(1000);
            RouletteSettlementResult settlement = Play(wallet, 17, RouletteBet.Straight(17, 10), RouletteBet.Red(50));

            RouletteRoundRecord record = RouletteHistoryService.FromSettlement(settlement, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual("ROULETTE-000001", record.roundId);
            Assert.AreEqual(17, record.winningNumber);
            Assert.AreEqual((int)PocketColor.Black, record.color);
            Assert.AreEqual(60, record.totalBet);
            Assert.AreEqual(360, record.totalReturned);
            Assert.AreEqual(300, record.net);
            Assert.AreEqual(2, record.bets.Count);
            Assert.IsTrue(record.bets[0].won);
            Assert.AreEqual("STRAIGHT 17", record.bets[0].label);
            Assert.AreEqual(350, record.bets[0].netWinnings);
            Assert.AreEqual(360, record.bets[0].totalReturn);
            Assert.IsFalse(record.bets[1].won);
            Assert.AreEqual("RED", record.bets[1].label);
            Assert.AreEqual(0, record.bets[1].totalReturn);
        }

        [Test]
        public void FromSettlement_StillRecordsARoundWhoseCreditWasRefused()
        {
            FakeWallet wallet = new FakeWallet(1000) { RefuseCredit = true };
            RouletteSettlementResult settlement = Play(wallet, 17, RouletteBet.Straight(17, 10));
            Assert.AreEqual(RouletteSettlementStatus.CreditFailed, settlement.Status);

            RouletteRoundRecord record = RouletteHistoryService.FromSettlement(settlement, DateTime.UtcNow);
            Assert.AreEqual(360, record.totalReturned); // the stake was charged and the round was played
        }

        [Test]
        public void FromSettlement_RejectsRoundsThatWereNeverCharged()
        {
            FakeWallet wallet = new FakeWallet(5);
            RouletteSettlementResult refused = Play(wallet, 17, RouletteBet.Straight(17, 10));
            Assert.AreEqual(RouletteSettlementStatus.InsufficientCredits, refused.Status);

            Assert.Throws<ArgumentException>(() => RouletteHistoryService.FromSettlement(refused, DateTime.UtcNow));
            Assert.Throws<ArgumentNullException>(() => RouletteHistoryService.FromSettlement(null, DateTime.UtcNow));
        }

        [Test]
        public void SaveData_SurvivesAJsonRoundTrip()
        {
            RouletteHistoryService original = new RouletteHistoryService(10);
            original.ReserveNextRoundId();
            original.AddRound(RouletteHistoryService.FromSettlement(Play(new FakeWallet(1000), 17, RouletteBet.Straight(17, 10)), DateTime.UtcNow));

            RouletteHistorySaveData data = new RouletteHistorySaveData
            {
                lastRoundNumber = original.LastRoundNumber,
                rounds = new List<RouletteRoundRecord>(original.Rounds),
                statistics = original.Statistics
            };
            RouletteHistorySaveData loaded = JsonUtility.FromJson<RouletteHistorySaveData>(JsonUtility.ToJson(data));

            RouletteHistoryService restored = new RouletteHistoryService(10);
            restored.LoadState(loaded.lastRoundNumber, loaded.rounds, loaded.statistics);

            Assert.AreEqual(1, restored.Rounds.Count);
            RouletteRoundRecord a = original.Rounds[0], b = restored.Rounds[0];
            Assert.AreEqual(a.roundId, b.roundId);
            Assert.AreEqual(a.winningNumber, b.winningNumber);
            Assert.AreEqual(a.totalReturned, b.totalReturned);
            Assert.AreEqual(a.bets.Count, b.bets.Count);
            Assert.AreEqual(a.bets[0].label, b.bets[0].label);
            Assert.AreEqual(original.Statistics.totalReturned, restored.Statistics.totalReturned);
            Assert.AreEqual("ROULETTE-000002", restored.ReserveNextRoundId());
        }
    }
}
