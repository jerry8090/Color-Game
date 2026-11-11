using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Save;
using RouletteGame.Core;
using RouletteGame.Data;

namespace RouletteGame.Integration
{
    /// <summary>
    /// Scene-facing Roulette history. Saves into the "RouletteGame.History" section of the shared save file.
    /// Completely separate from the Color Game and Scatter histories. Same save hooks as ScatterHistoryManager.
    /// </summary>
    public class RouletteHistoryManager : MonoBehaviour
    {
        public const string SaveKey = "RouletteGame.History";
        private const int FallbackLimit = 100;

        [SerializeField] private SaveManager saveManager;
        [SerializeField] private RouletteSettings settings;

        /// <summary>Raised whenever rounds or statistics change.</summary>
        public event Action OnHistoryChanged;

        private RouletteHistoryService service;

        private RouletteHistoryService Service
        {
            get
            {
                if (service == null) service = new RouletteHistoryService(settings != null ? settings.HistoryLimit : FallbackLimit);
                return service;
            }
        }

        /// <summary>Oldest first. The UI reverses it to show the newest on top.</summary>
        public IReadOnlyList<RouletteRoundRecord> Rounds => Service.Rounds;
        public RouletteStatistics Statistics => Service.Statistics;
        public long LastRoundNumber => Service.LastRoundNumber;

        public List<int> RecentNumbers(int count)
        {
            return Service.RecentNumbers(count);
        }

        private void Awake()
        {
            if (saveManager == null)
            {
                Debug.LogError("[RouletteHistoryManager] Save Manager is not assigned in the Inspector.", this);
            }
        }

        private void OnEnable()
        {
            if (saveManager == null) return;
            saveManager.OnBeforeSave += HandleBeforeSave;
            saveManager.OnGameLoaded += HandleGameLoaded;
            saveManager.OnGameReset += HandleGameReset;
        }

        private void Start()
        {
            // The save may have loaded before this object started listening. Catch up, so an empty history
            // can never be written back over the real one.
            if (saveManager != null && saveManager.IsReady) HandleGameLoaded();
        }

        private void OnDisable()
        {
            if (saveManager == null) return;
            saveManager.OnBeforeSave -= HandleBeforeSave;
            saveManager.OnGameLoaded -= HandleGameLoaded;
            saveManager.OnGameReset -= HandleGameReset;
        }

        public string ReserveNextRoundId()
        {
            return Service.ReserveNextRoundId();
        }

        public void AddRound(RouletteRoundRecord record)
        {
            if (record == null)
            {
                Debug.LogError("[RouletteHistoryManager] AddRound called with null.");
                return;
            }

            Service.AddRound(record);
            OnHistoryChanged?.Invoke();
        }

        public void ClearHistory()
        {
            Service.ClearHistory();
            OnHistoryChanged?.Invoke();
            if (saveManager != null) saveManager.SaveGame();
        }

        [ContextMenu("TEST: Clear Roulette History")]
        private void TestClearHistory()
        {
            ClearHistory();
        }

        // ---------- Save hooks ----------

        private void HandleBeforeSave()
        {
            RouletteHistorySaveData data = new RouletteHistorySaveData
            {
                lastRoundNumber = Service.LastRoundNumber,
                rounds = new List<RouletteRoundRecord>(Service.Rounds),
                statistics = Service.Statistics
            };
            saveManager.SetSection(SaveKey, data);
        }

        private void HandleGameLoaded()
        {
            if (saveManager.TryGetSection(SaveKey, out RouletteHistorySaveData data))
            {
                Service.LoadState(data.lastRoundNumber, data.rounds, data.statistics);
            }
            else
            {
                Service.ResetAll(); // older save files have no Roulette section
            }

            OnHistoryChanged?.Invoke();
        }

        private void HandleGameReset()
        {
            Service.ResetAll();
            OnHistoryChanged?.Invoke();
        }
    }
}
