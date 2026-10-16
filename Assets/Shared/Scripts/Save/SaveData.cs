using System;
using System.Collections.Generic;
using Shared.Wallet;

namespace Shared.Save
{
    /// <summary>
    /// One named block of game-specific data stored as JSON text.
    /// Example: key "ColorGame.History". This keeps SaveManager independent of any single game.
    /// </summary>
    [Serializable]
    public class SaveSection
    {
        public string key;
        public string json;
    }

    /// <summary>Player settings, saved with everything else. AudioManager will use these in Phase 6.</summary>
    [Serializable]
    public class SettingsData
    {
        public float musicVolume = 0.7f;
        public float sfxVolume = 1f;
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
    }

    /// <summary>Everything that gets written to the save file.</summary>
    [Serializable]
    public class SaveData
    {
        public int saveVersion = 1;
        public string savedAtUtc;
        public long balance;
        public List<WalletTransaction> transactions = new List<WalletTransaction>();
        public List<SaveSection> sections = new List<SaveSection>();
        public SettingsData settings = new SettingsData();
    }
}