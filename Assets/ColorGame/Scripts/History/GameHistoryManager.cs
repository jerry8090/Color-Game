using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Save;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.History
{
    /// <summary>
    /// What gets written into the save file's "ColorGame.History" section.
    /// </summary>
    [Serializable]
    public class HistorySaveData
    {
        public long lastRoundNumber;
        public List<GameRound> rounds = new List<GameRound>();
        public GameStatistics statistics = new GameStatistics();
    }

    /// <summary>
    /// Scene-facing history. Other scripts call this, never the service directly.
    /// Creates round IDs, stores finished rounds, tracks statistics, and saves/loads them.
    /// The round counter is saved, so IDs never repeat after a restart.
    /// </summary>
    public class GameHistoryManager : MonoBehaviour
    {
        public const string SaveKey = "ColorGame.History";
        private const int FallbackHistoryLimit = 100;

        [SerializeField] private ColorGameConfig config;
        [SerializeField] private SaveManager saveManager;

        /// <summary>Raised whenever rounds or statistics change (added, cleared, loaded, reset).</summary>
        public event Action OnHistoryChanged;

        private IGameHistoryService historyService;

        // Created on first use, so other scripts can safely call us at any time.
        private IGameHistoryService Service
        {
            get
            {
                if (historyService == null)
                {
                    int limit = config != null ? config.HistoryLimit : FallbackHistoryLimit;
                    historyService = new LocalGameHistoryService(limit);
                }
                return historyService;
            }
        }

        /// <summary>Oldest first. The UI reverses it to show the newest on top.</summary>
        public IReadOnlyList<GameRound> Rounds => Service.Rounds;
        public long LastRoundNumber => Service.LastRoundNumber;

        /// <summary>Read only. Never change these numbers from outside.</summary>
        public GameStatistics Statistics => Service.Statistics;

        private void Awake()
        {
            if (config == null || saveManager == null)
            {
                Debug.LogError("[GameHistoryManager] Config or Save Manager is not assigned in the Inspector.", this);
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
            // If the save file was already loaded before this object started listening,
            // the OnGameLoaded event was missed. Catch up now, so an empty history can never
            // be written back over the real one by a later save.
            if (saveManager != null && saveManager.IsReady) HandleGameLoaded();
        }

        private void OnDisable()
        {
            if (saveManager == null) return;
            saveManager.OnBeforeSave -= HandleBeforeSave;
            saveManager.OnGameLoaded -= HandleGameLoaded;
            saveManager.OnGameReset -= HandleGameReset;
        }

        /// <summary>Lets a future server-based history service replace the local one.</summary>
        public void SetHistoryService(IGameHistoryService service)
        {
            if (service == null)
            {
                Debug.LogError("[GameHistoryManager] SetHistoryService called with null.");
                return;
            }
            historyService = service;
            OnHistoryChanged?.Invoke();
        }

        /// <summary>Hands out the next unique ID, for example ROUND-000001. Never returns the same ID twice.</summary>
        public string ReserveNextRoundId()
        {
            return Service.ReserveNextRoundId();
        }

        /// <summary>Stores a finished round, updates the statistics and trims to the history limit.</summary>
        public void AddRound(GameRound round)
        {
            if (round == null)
            {
                Debug.LogError("[GameHistoryManager] AddRound called with null.");
                return;
            }

            Service.AddRound(round);
            OnHistoryChanged?.Invoke();
        }

        /// <summary>
        /// Removes all stored rounds AND resets the statistics (for a clean test run).
        /// The round counter keeps going, so IDs are never reused.
        /// </summary>
        public void ClearHistory()
        {
            Service.ClearHistory();
            OnHistoryChanged?.Invoke();
            if (saveManager != null) saveManager.SaveGame();
        }

        // Right-click the component header in Play mode. Phase 8 adds a proper button.
        [ContextMenu("TEST: Clear History")]
        private void TestClearHistory()
        {
            ClearHistory();
        }

        // ---------- Save system hooks ----------

        private void HandleBeforeSave()
        {
            HistorySaveData data = new HistorySaveData
            {
                lastRoundNumber = Service.LastRoundNumber,
                rounds = new List<GameRound>(Service.Rounds),
                statistics = Service.Statistics
            };
            saveManager.SetSection(SaveKey, data);
        }

        private void HandleGameLoaded()
        {
            if (saveManager.TryGetSection(SaveKey, out HistorySaveData data))
            {
                Service.LoadState(data.lastRoundNumber, data.rounds, data.statistics);
            }
            else
            {
                Service.ResetAll();
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