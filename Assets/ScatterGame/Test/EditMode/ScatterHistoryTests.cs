using System;
using System.Collections.Generic;
using NUnit.Framework;
using ScatterGame.Core;
using ScatterGame.Runtime;
using UnityEngine;

namespace ScatterGame.Tests
{
    public class ScatterHistoryTests
    {
        private static ScatterRoundRecord Record(string id, long bet, long win, bool bonus = false, int awarded = 0, int played = 0)
        {
            return new ScatterRoundRecord
            {
                roundId = id, bet = bet, winCredits = win, netCredits = win - bet,
                bonusTriggered = bonus, freeSpinsAwarded = awarded, freeSpinsPlayed = played
            };
        }

        private static SettlementResult PlayBonusRound()
        {
            FakeWallet wallet = new FakeWallet(1000);
            RoundSettler settler = new RoundSettler(DefaultSlotConfig.Create(), wallet, new[] { 100 });

            ScriptedSymbolSource source = new ScriptedSymbolSource();
            source.EnqueueGrid(TestGrids.ThreeScatters());
            source.EnqueueGrid(TestGrids.ThreeTenReels()); // free spin 1 wins at the 2x free-spin multiplier
            TestGrids.EnqueueQuietRefill(source);
            for (int i = 0; i < 9; i++) source.EnqueueGrid(TestGrids.NoWin());

            return settler.Play(100, "SCATTER-000001", source);
        }

        [Test]
        public void RoundIds_AreUniqueAndPadded()
        {
            ScatterHistoryService service = new ScatterHistoryService(10);
            Assert.AreEqual("SCATTER-000001", service.ReserveNextRoundId());
            Assert.AreEqual("SCATTER-000002", service.ReserveNextRoundId());
            Assert.AreEqual(2, service.LastRoundNumber);
        }

        [Test]
        public void AddRound_UpdatesStatistics()
        {
            ScatterHistoryService service = new ScatterHistoryService(10);
            service.AddRound(Record("SCATTER-000001", 100, 0));
            service.AddRound(Record("SCATTER-000002", 100, 250, true, 10, 10));
            service.AddRound(Record("SCATTER-000003", 50, 20));

            ScatterStatistics s = service.Statistics;
            Assert.AreEqual(3, s.totalRounds);
            Assert.AreEqual(2, s.wins);
            Assert.AreEqual(250, s.totalBet);
            Assert.AreEqual(270, s.totalWon);
            Assert.AreEqual(20, s.NetCredits);
            Assert.AreEqual(1, s.bonusRounds);
            Assert.AreEqual(10, s.totalFreeSpinsAwarded);
            Assert.AreEqual(10, s.totalFreeSpinsPlayed);
            Assert.AreEqual(250, s.largestWin);
        }

        [Test]
        public void Trim_RemovesOldestRounds_ButKeepsStatistics()
        {
            ScatterHistoryService service = new ScatterHistoryService(3);
            for (int i = 1; i <= 5; i++) service.AddRound(Record("SCATTER-00000" + i, 10, 0));

            Assert.AreEqual(3, service.Rounds.Count);
            Assert.AreEqual("SCATTER-000003", service.Rounds[0].roundId);
            Assert.AreEqual(5, service.Statistics.totalRounds);
        }

        [Test]
        public void ClearHistory_ResetsRoundsAndStats_ButKeepsTheIdCounter()
        {
            ScatterHistoryService service = new ScatterHistoryService(10);
            service.ReserveNextRoundId();
            service.AddRound(Record("SCATTER-000001", 100, 0));

            service.ClearHistory();

            Assert.AreEqual(0, service.Rounds.Count);
            Assert.AreEqual(0, service.Statistics.totalRounds);
            Assert.AreEqual("SCATTER-000002", service.ReserveNextRoundId());
        }

        [Test]
        public void LoadState_NeverRepeatsAnId_EvenWithAStaleCounter()
        {
            ScatterHistoryService service = new ScatterHistoryService(10);
            service.LoadState(0, new List<ScatterRoundRecord> { Record("SCATTER-000007", 10, 0) }, null);
            Assert.AreEqual("SCATTER-000008", service.ReserveNextRoundId());
            Assert.IsNotNull(service.Statistics);
        }

        [Test]
        public void EmptyOrOlderSaveData_LoadsWithSafeDefaults()
        {
            ScatterHistorySaveData data = JsonUtility.FromJson<ScatterHistorySaveData>("{}");
            ScatterHistoryService service = new ScatterHistoryService(10);

            Assert.DoesNotThrow(() => service.LoadState(data.lastRoundNumber, data.rounds, data.statistics));
            Assert.AreEqual(0, service.Rounds.Count);
            Assert.IsNotNull(service.Statistics);
            Assert.AreEqual("SCATTER-000001", service.ReserveNextRoundId());
        }

        [Test]
        public void FromSettlement_RecordsABonusRoundCorrectly()
        {
            SettlementResult settlement = PlayBonusRound();
            Assert.IsTrue(settlement.Success);

            ScatterRoundRecord record = ScatterHistoryService.FromSettlement(settlement, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual("SCATTER-000001", record.roundId);
            Assert.AreEqual(100, record.bet);
            Assert.AreEqual(128, record.winCredits);
            Assert.AreEqual(28, record.netCredits);
            Assert.IsTrue(record.bonusTriggered);
            Assert.AreEqual(3, record.scatterCount);
            Assert.AreEqual(10, record.freeSpinsAwarded);
            Assert.AreEqual(10, record.freeSpinsPlayed);
            Assert.AreEqual(1, record.cascades);
            Assert.AreEqual(2, record.maxMultiplier);

            Assert.AreEqual(11, record.spins.Count);
            Assert.AreEqual("paid", record.spins[0].kind);
            Assert.AreEqual("free", record.spins[1].kind);
            Assert.AreEqual("6,0,0,0,1,1,1,1,2,6,2,2,3,3,3,3,4,4,6,4", record.spins[0].gridCsv);

            Assert.AreEqual(1, record.wins.Count);
            ScatterWinRecord win = record.wins[0];
            Assert.AreEqual(1, win.spin);
            Assert.AreEqual(0, win.cascade);
            Assert.AreEqual(2, win.multiplier);
            Assert.AreEqual(TestGrids.Ten, win.symbolId);
            Assert.AreEqual(3, win.reels);
            Assert.AreEqual(64, win.ways);
            Assert.AreEqual(128000, win.winMilli);
        }

        [Test]
        public void FromSettlement_RejectsAFailedSettlement()
        {
            Assert.Throws<ArgumentException>(() =>
                ScatterHistoryService.FromSettlement(new SettlementResult { Status = SettlementStatus.InsufficientCredits }, DateTime.UtcNow));
            Assert.Throws<ArgumentNullException>(() => ScatterHistoryService.FromSettlement(null, DateTime.UtcNow));
        }

        [Test]
        public void WinRecords_AreCapped_AndFlagged()
        {
            FakeWallet wallet = new FakeWallet(1000);
            RoundSettler settler = new RoundSettler(DefaultSlotConfig.Create(), wallet, new[] { 100 });
            SettlementResult settlement = settler.Play(100, "SCATTER-000001", new ConstantSource(TestGrids.Ten)); // 50 cascades

            ScatterRoundRecord record = ScatterHistoryService.FromSettlement(settlement, DateTime.UtcNow);

            Assert.IsTrue(record.hitCascadeLimit);
            Assert.AreEqual(ScatterHistoryService.MaxWinRecordsPerRound, record.wins.Count);
            Assert.IsTrue(record.winsTruncated);
        }

        [Test]
        public void SaveData_SurvivesAJsonRoundTrip()
        {
            ScatterHistoryService original = new ScatterHistoryService(10);
            original.ReserveNextRoundId();
            original.AddRound(ScatterHistoryService.FromSettlement(PlayBonusRound(), DateTime.UtcNow));

            ScatterHistorySaveData data = new ScatterHistorySaveData
            {
                lastRoundNumber = original.LastRoundNumber,
                rounds = new List<ScatterRoundRecord>(original.Rounds),
                statistics = original.Statistics
            };
            string json = JsonUtility.ToJson(data);

            ScatterHistorySaveData loaded = JsonUtility.FromJson<ScatterHistorySaveData>(json);
            ScatterHistoryService restored = new ScatterHistoryService(10);
            restored.LoadState(loaded.lastRoundNumber, loaded.rounds, loaded.statistics);

            Assert.AreEqual(1, restored.Rounds.Count);
            ScatterRoundRecord a = original.Rounds[0];
            ScatterRoundRecord b = restored.Rounds[0];
            Assert.AreEqual(a.roundId, b.roundId);
            Assert.AreEqual(a.winCredits, b.winCredits);
            Assert.AreEqual(a.spins.Count, b.spins.Count);
            Assert.AreEqual(a.spins[0].gridCsv, b.spins[0].gridCsv);
            Assert.AreEqual(a.wins.Count, b.wins.Count);
            Assert.AreEqual(a.wins[0].winMilli, b.wins[0].winMilli);
            Assert.AreEqual(original.Statistics.totalWon, restored.Statistics.totalWon);
            Assert.AreEqual("SCATTER-000002", restored.ReserveNextRoundId());
        }
    }
}