using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Shared.Wallet;

namespace Shared.Save
{
    /// <summary>
    /// Saves and loads the prototype data to a JSON file in Application.persistentDataPath.
    /// Safety features: writes to a temp file first, keeps a .bak backup, keeps a copy of a corrupted file.
    /// Prototype only: a real-money version must keep the wallet on a server, not in a local file.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VirtualWalletManager walletManager;

        [Header("Settings")]
        [SerializeField] private string saveFileName = "colorgame_save.json";
        [SerializeField] private bool loadOnStart = true;
        [SerializeField] private bool saveOnEveryTransaction = true;

        /// <summary>Raised just before writing, so other systems can push their data via SetSection.</summary>
        public event Action OnBeforeSave;

        /// <summary>Raised after a save file was loaded, so other systems can read their sections.</summary>
        public event Action OnGameLoaded;

        /// <summary>Raised after a full game reset, so other systems can clear themselves.</summary>
        public event Action OnGameReset;

        private readonly Dictionary<string, string> sections = new Dictionary<string, string>();
        private SettingsData settings = new SettingsData();

        // Prevents saving an empty wallet over a real save before the load has happened.
        private bool isReady;
        private bool suppressAutoSave;

        public SettingsData Settings => settings;
        public bool IsReady => isReady;

        private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);
        private string BackupPath => SavePath + ".bak";
        private string TempPath => SavePath + ".tmp";
        private string CorruptPath => SavePath + ".corrupt";

        private void Awake()
        {
            if (walletManager == null)
            {
                Debug.LogError("[SaveManager] Wallet Manager is not assigned in the Inspector.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (walletManager != null) walletManager.OnTransactionAdded += HandleTransactionAdded;
        }

        private void OnDisable()
        {
            if (walletManager != null) walletManager.OnTransactionAdded -= HandleTransactionAdded;
        }

        private void Start()
        {
            if (!loadOnStart) return;

            // First launch (no file) or unreadable file: start a fresh game.
            if (!LoadGame())
            {
                ResetGame();
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused) SaveGame(); // Android: the app goes to the background
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        // ---------- Sections (used by other systems such as game history) ----------

        public void SetSection<T>(string key, T value) where T : class
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                Debug.LogError("[SaveManager] SetSection needs a key and a value.");
                return;
            }
            sections[key] = JsonUtility.ToJson(value);
        }

        public bool TryGetSection<T>(string key, out T value) where T : class
        {
            value = null;
            if (!sections.TryGetValue(key, out string json) || string.IsNullOrEmpty(json)) return false;

            try
            {
                value = JsonUtility.FromJson<T>(json);
                return value != null;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Could not read section '{key}': {e.Message}");
                return false;
            }
        }

        // ---------- Save / Load / Reset ----------

        [ContextMenu("Save Game")]
        public bool SaveGame()
        {
            if (walletManager == null) return false;

            if (!isReady)
            {
                Debug.LogWarning("[SaveManager] Save skipped: game has not been loaded yet.");
                return false;
            }

            try
            {
                OnBeforeSave?.Invoke();

                string json = JsonUtility.ToJson(BuildSaveData(), true);

                File.WriteAllText(TempPath, json);                              // 1. write temp file
                if (File.Exists(SavePath)) File.Copy(SavePath, BackupPath, true); // 2. back up the old save
                File.Copy(TempPath, SavePath, true);                            // 3. replace the save
                File.Delete(TempPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Save failed: {e.Message}");
                return false;
            }
        }

        [ContextMenu("Load Game")]
        public bool LoadGame()
        {
            if (walletManager == null)
            {
                Debug.LogError("[SaveManager] Cannot load: Wallet Manager is missing.");
                return false;
            }

            SaveData data = ReadFile(SavePath);

            if (data == null && File.Exists(BackupPath))
            {
                Debug.LogWarning("[SaveManager] Main save unreadable. Trying backup file.");
                data = ReadFile(BackupPath);
            }

            if (data == null)
            {
                Debug.Log("[SaveManager] No usable save file found.");
                return false;
            }

            if (!walletManager.LoadState(data.balance, data.transactions))
            {
                return false;
            }

            sections.Clear();
            if (data.sections != null)
            {
                foreach (SaveSection section in data.sections)
                {
                    if (section != null && !string.IsNullOrEmpty(section.key))
                    {
                        sections[section.key] = section.json;
                    }
                }
            }

            settings = data.settings ?? new SettingsData();
            isReady = true;

            OnGameLoaded?.Invoke();
            Debug.Log($"[SaveManager] Loaded save. Balance: {data.balance}");
            return true;
        }

        /// <summary>Wipes balance, transactions, sections and settings, then saves a fresh game.</summary>
        [ContextMenu("Reset Game (wipe everything)")]
        public void ResetGame()
        {
            if (walletManager == null)
            {
                Debug.LogError("[SaveManager] Cannot reset: Wallet Manager is missing.");
                return;
            }

            suppressAutoSave = true;
            isReady = true; // allow saving from now on

            sections.Clear();
            settings = new SettingsData();
            walletManager.InitializeNewWallet();
            OnGameReset?.Invoke();

            suppressAutoSave = false;
            SaveGame();
            Debug.Log("[SaveManager] Game reset. New wallet created.");
        }

        // ---------- Helpers ----------

        private void HandleTransactionAdded(WalletTransaction transaction)
        {
            if (saveOnEveryTransaction && !suppressAutoSave)
            {
                SaveGame();
            }
        }

        private SaveData BuildSaveData()
        {
            SaveData data = new SaveData
            {
                saveVersion = 1,
                savedAtUtc = DateTime.UtcNow.ToString("o"),
                balance = walletManager.GetBalance(),
                transactions = new List<WalletTransaction>(walletManager.Transactions),
                settings = settings
            };

            foreach (KeyValuePair<string, string> pair in sections)
            {
                data.sections.Add(new SaveSection { key = pair.Key, json = pair.Value });
            }

            return data;
        }

        private SaveData ReadFile(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                if (data == null) throw new InvalidDataException("File was empty or invalid.");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to read '{path}': {e.Message}");

                // Keep a copy so a corrupted file is never silently lost.
                try { File.Copy(path, CorruptPath, true); } catch (Exception) { /* ignore */ }
                return null;
            }
        }
    }
}