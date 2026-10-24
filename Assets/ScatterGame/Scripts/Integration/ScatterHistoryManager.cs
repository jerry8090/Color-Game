using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Save;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Scene-facing Scatter history. Saves into the "ScatterGame.History" section of the shared save file.
    /// Completely separate from the Color Game history. Same save hooks as GameHistoryManager.
    /// </summary>
    public class ScatterHistoryManager : MonoBehaviour
    {
        public const string SaveKey = "ScatterGame.History";

        [SerializeField] private SaveManager saveManager;
        [SerializeField] private int historyLimit = 100;

        /// <summary>Raised whenever rounds or statistics change.</summary>
        public event Action OnHistoryChanged;

        private ScatterHistoryService service;

        private ScatterHistoryService Service
        {
            get
            {
                if (service == null) service = new ScatterHistoryService(historyLimit);
                return service;
            }
        }

        /// <summary>Oldest first. The UI reverses it to show the newest on top.</summary>
        public IReadOnlyList<ScatterRoundRecord> Rounds => Service.Rounds;
        public ScatterStatistics Statistics => Service.Statistics;
        public long LastRoundNumber => Service.LastRoundNumber;

        private void Awake()
        {
            if (saveManager == null)
            {
                Debug.LogError("[ScatterHistoryManager] Save Manager is not assigned in the Inspector.", this);
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

        public void AddRound(ScatterRoundRecord record)
        {
            if (record == null)
            {
                Debug.LogError("[ScatterHistoryManager] AddRound called with null.");
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

        [ContextMenu("TEST: Clear Scatter History")]
        private void TestClearHistory()
        {
            ClearHistory();
        }

        // ---------- Save hooks ----------

        private void HandleBeforeSave()
        {
            ScatterHistorySaveData data = new ScatterHistorySaveData
            {
                lastRoundNumber = Service.LastRoundNumber,
                rounds = new List<ScatterRoundRecord>(Service.Rounds),
                statistics = Service.Statistics
            };
            saveManager.SetSection(SaveKey, data);
        }

        private void HandleGameLoaded()
        {
            if (saveManager.TryGetSection(SaveKey, out ScatterHistorySaveData data))
            {
                Service.LoadState(data.lastRoundNumber, data.rounds, data.statistics);
            }
            else
            {
                Service.ResetAll(); // older save files have no Scatter section
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