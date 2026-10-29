using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Save;

namespace Shared.Audio
{
    /// <summary>Every sound effect the games can ask for. Add new entries at the END of the list.</summary>
    public enum SfxType
    {
        ButtonClick,
        Select,
        Roll,
        SlotStop,
        Win,
        Lose,
        CubeHitCube,
        CubeHitFloor,
        CubeHitWall,
        CubeSettle,
        RackMove,
        CountdownOpen,
        CountdownTick,
        CountdownFinal,
        Match2,
        Match3
    }

    /// <summary>Which volume slider and playback path a sound uses.</summary>
    public enum SfxGroup
    {
        Interface,   // buttons, results: one shared AudioSource
        Physics,     // cube impacts: a small pool of voices (pitch per hit)
        Countdown,   // betting countdown: its own voice, can be stopped instantly
        Machine      // rack movement: its own voice
    }

        /// <summary>Dedicated voices for sounds that are not in the SfxType list (Scatter symbol wins and multipliers).</summary>
    public enum SfxChannel
    {
        Symbol,
        Multiplier
    }

    /// <summary>One sound effect: which type, which clip, how loud.</summary>
    [Serializable]
    public class SfxEntry
    {
        public SfxType type;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    /// <summary>
    /// The single place that plays sound. Other scripts call PlaySfx / PlayImpact and never touch AudioSources.
    /// Master volume and mute come from the save file (SaveManager.Settings: sfxEnabled, sfxVolume, music...).
    /// Group volumes (physics, countdown, machine) are set here in the Inspector.
    /// A missing clip logs ONE warning and is then silently ignored.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Sound Effects")]
        [SerializeField] private List<SfxEntry> sfxEntries = new List<SfxEntry>
        {
            new SfxEntry { type = SfxType.ButtonClick },
            new SfxEntry { type = SfxType.Select },
            new SfxEntry { type = SfxType.Roll },
            new SfxEntry { type = SfxType.SlotStop },
            new SfxEntry { type = SfxType.Win },
            new SfxEntry { type = SfxType.Lose }
        };

        [Header("Group volumes (multiplied with the master SFX volume and mute)")]
        [Range(0f, 1f)] [SerializeField] private float physicsVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float countdownVolume = 0.8f;
        [Range(0f, 1f)] [SerializeField] private float machineVolume = 0.6f;

        [Header("Physics sounds")]
        [Tooltip("How many impact sounds can play at the same time. Voices are created once and reused.")]
        [SerializeField] private int physicsVoices = 8;
        [Tooltip("Minimum seconds between two sounds of the same type.")]
        [SerializeField] private float physicsMinInterval = 0.04f;
        [Tooltip("Random pitch change per impact (0.08 = plus or minus 8 percent).")]
        [SerializeField] private float pitchVariation = 0.08f;
        [Tooltip("Limit for the combined loudness of all impact sounds playing at once.")]
        [SerializeField] private float physicsMaxCombined = 1.5f;

        [Header("Music (optional)")]
        [SerializeField] private AudioClip musicClip;
        [Header("Scatter game: symbol and multiplier sounds (multiplied with the master SFX volume and mute)")]
        [Range(0f, 1f)] [SerializeField] private float symbolWinVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float multiplierVolume = 1f;

        /// <summary>Raised when volume or on/off settings change (the sound button listens to this).</summary>
        public event Action OnSettingsChanged;

        private readonly Dictionary<SfxType, SfxEntry> lookup = new Dictionary<SfxType, SfxEntry>();
        private readonly HashSet<SfxType> warned = new HashSet<SfxType>();
        private readonly Dictionary<SfxType, float> lastPhysicsPlay = new Dictionary<SfxType, float>();
        private readonly SettingsData fallbackSettings = new SettingsData();

        private AudioSource[] physicsPool;
        private AudioSource countdownSource;
        private AudioSource machineSource;
        private AudioSource symbolSource;
        private AudioSource multiplierSource;
        private bool isReady;

        // Always read the settings fresh: SaveManager replaces the object when the game is reset.
        private SettingsData Settings =>
            saveManager != null && saveManager.Settings != null ? saveManager.Settings : fallbackSettings;

        public bool IsSoundOn => Settings.musicEnabled || Settings.sfxEnabled;

        private void OnValidate()
        {
            physicsVoices = Mathf.Clamp(physicsVoices, 2, 16);
            physicsMinInterval = Mathf.Max(0f, physicsMinInterval);
            pitchVariation = Mathf.Clamp(pitchVariation, 0f, 0.3f);
            physicsMaxCombined = Mathf.Max(0.2f, physicsMaxCombined);
            EnsureEntries();
        }

        // Makes sure every SfxType has a row in the list, so new sounds appear in the Inspector by themselves.
        [ContextMenu("Add Missing Sound Entries")]
        private void EnsureEntries()
        {
            if (sfxEntries == null) sfxEntries = new List<SfxEntry>();

            foreach (SfxType type in Enum.GetValues(typeof(SfxType)))
            {
                bool found = false;
                foreach (SfxEntry entry in sfxEntries)
                {
                    if (entry != null && entry.type == type)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) sfxEntries.Add(new SfxEntry { type = type });
            }
        }

        private void Awake()
        {
            if (sfxSource == null || musicSource == null)
            {
                Debug.LogError("[AudioManager] SFX Source or Music Source is not assigned in the Inspector.", this);
                return;
            }

            sfxSource.playOnAwake = false;
            musicSource.playOnAwake = false;
            musicSource.loop = true;

            foreach (SfxEntry entry in sfxEntries)
            {
                if (entry == null) continue;

                if (lookup.ContainsKey(entry.type))
                {
                    Debug.LogWarning($"[AudioManager] Sound type {entry.type} is listed twice. Using the first one.", this);
                    continue;
                }
                lookup.Add(entry.type, entry);
            }

            // Voices are created once here and reused for every sound (no AudioSource per collision).
            physicsPool = new AudioSource[Mathf.Clamp(physicsVoices, 2, 16)];
            for (int i = 0; i < physicsPool.Length; i++) physicsPool[i] = CreateVoice("PhysicsVoice" + i);
            countdownSource = CreateVoice("CountdownVoice");
            machineSource = CreateVoice("MachineVoice");
            symbolSource = CreateVoice("SymbolWinVoice");
            multiplierSource = CreateVoice("MultiplierVoice");

            isReady = true;
        }

        private AudioSource CreateVoice(string voiceName)
        {
            GameObject go = new GameObject(voiceName);
            go.transform.SetParent(transform, false);

            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f; // 2D: works the same in the Editor and on Android
            return source;
        }

        private void OnEnable()
        {
            if (saveManager == null) return;
            saveManager.OnGameLoaded += HandleSettingsReloaded;
            saveManager.OnGameReset += HandleSettingsReloaded;
        }

        private void OnDisable()
        {
            if (saveManager == null) return;
            saveManager.OnGameLoaded -= HandleSettingsReloaded;
            saveManager.OnGameReset -= HandleSettingsReloaded;
        }

        private void Start()
        {
            // If the save is not loaded yet, wait for the OnGameLoaded event instead,
            // so music never starts when the saved setting says it is off.
            if (saveManager == null || saveManager.IsReady)
            {
                ApplyMusic();
                OnSettingsChanged?.Invoke();
            }
        }

        // ---------- Sound effects ----------

        /// <summary>Plays a sound at its normal strength (buttons, results, countdown, rack...).</summary>
        public void PlaySfx(SfxType type)
        {
            Play(type, 1f, 0f);
        }

        /// <summary>Plays an impact. Strength 0 to 1 decides the loudness, and the pitch varies slightly.</summary>
        public void PlayImpact(SfxType type, float strength01)
        {
            Play(type, Mathf.Clamp01(strength01), pitchVariation);
        }

        /// <summary>Stops the betting countdown sounds immediately.</summary>
        public void StopCountdownSounds()
        {
            if (countdownSource != null) countdownSource.Stop();
        }

        /// <summary>Stops the rack / machine sounds immediately.</summary>
        public void StopMachineSounds()
        { 
            
            if (machineSource != null) machineSource.Stop();
        }

                /// <summary>
        /// Plays any clip on a dedicated voice (Scatter symbol win sounds and multiplier sounds).
        /// Obeys the saved master SFX volume and mute. A missing clip is ignored.
        /// Symbol and multiplier sounds use separate voices, so a symbol sound never cuts off a multiplier sound.
        /// </summary>
        public void PlayClip(AudioClip clip, SfxChannel channel, float clipVolume = 1f)
        {
            if (clip == null || !isReady || !Settings.sfxEnabled) return;

            float groupVolume = channel == SfxChannel.Multiplier ? multiplierVolume : symbolWinVolume;
            float volume = Mathf.Clamp01(clipVolume * groupVolume * Settings.sfxVolume);
            if (volume < 0.003f) return;

            AudioSource source = channel == SfxChannel.Multiplier ? multiplierSource : symbolSource;
            if (source != null) source.PlayOneShot(clip, volume);
        }


        private void Play(SfxType type, float strength, float pitchRandom)
        {
            if (!isReady || !Settings.sfxEnabled) return;

            if (!lookup.TryGetValue(type, out SfxEntry entry) || entry.clip == null)
            {
                if (warned.Add(type))
                {
                    Debug.LogWarning($"[AudioManager] No clip assigned for {type}. This sound will be silent.", this);
                }
                return;
            }

            SfxGroup group = GetGroup(type);
            float volume = Mathf.Clamp01(entry.volume * strength * GroupVolume(group) * Settings.sfxVolume);
            if (volume < 0.003f) return;

            switch (group)
            {
                case SfxGroup.Physics:
                    PlayOnPool(type, entry.clip, volume, pitchRandom);
                    break;
                case SfxGroup.Countdown:
                    PlayOnVoice(countdownSource, entry.clip, volume);
                    break;
                case SfxGroup.Machine:
                    PlayOnVoice(machineSource, entry.clip, volume);
                    break;
                default:
                    sfxSource.PlayOneShot(entry.clip, volume);
                    break;
            }
        }

        private static SfxGroup GetGroup(SfxType type)
        {
            switch (type)
            {
                case SfxType.CubeHitCube:
                case SfxType.CubeHitFloor:
                case SfxType.CubeHitWall:
                case SfxType.CubeSettle:
                    return SfxGroup.Physics;
                case SfxType.CountdownOpen:
                case SfxType.CountdownTick:
                case SfxType.CountdownFinal:
                    return SfxGroup.Countdown;
                case SfxType.RackMove:
                    return SfxGroup.Machine;
                default:
                    return SfxGroup.Interface;
            }
        }

        private float GroupVolume(SfxGroup group)
        {
            switch (group)
            {
                case SfxGroup.Physics: return physicsVolume;
                case SfxGroup.Countdown: return countdownVolume;
                case SfxGroup.Machine: return machineVolume;
                default: return 1f;
            }
        }

        private static void PlayOnVoice(AudioSource source, AudioClip clip, float volume)
        {
            if (source == null) return;
            source.Stop();
            source.clip = clip;
            source.volume = volume;
            source.pitch = 1f;
            source.Play();
        }

        private void PlayOnPool(SfxType type, AudioClip clip, float volume, float pitchRandom)
        {
            // Same sound type too soon after the last one: skip it.
            float now = Time.unscaledTime;
            if (lastPhysicsPlay.TryGetValue(type, out float last) && now - last < physicsMinInterval) return;

            // Limit the combined loudness when several cubes collide at once.
            float playing = 0f;
            AudioSource free = null;
            AudioSource quietest = null;

            foreach (AudioSource voice in physicsPool)
            {
                if (voice.isPlaying)
                {
                    playing += voice.volume;
                    if (quietest == null || voice.volume < quietest.volume) quietest = voice;
                }
                else if (free == null)
                {
                    free = voice;
                }
            }

            float room = physicsMaxCombined - playing;
            if (room <= 0.05f) return;
            volume = Mathf.Min(volume, room);

            AudioSource chosen = free;
            if (chosen == null)
            {
                // All voices busy: replace the quietest one, but only if the new sound is louder.
                if (quietest == null || quietest.volume >= volume) return;
                chosen = quietest;
            }

            lastPhysicsPlay[type] = now;

            chosen.Stop();
            chosen.clip = clip;
            chosen.volume = volume;
            chosen.pitch = 1f + UnityEngine.Random.Range(-pitchRandom, pitchRandom);
            chosen.Play();
        }

        private void StopRuntimeVoices()
        {
            if (physicsPool != null)
            {
                foreach (AudioSource voice in physicsPool)
                {
                    if (voice != null) voice.Stop();
                }
            }
            StopCountdownSounds();
            if (symbolSource != null) symbolSource.Stop();
            if (multiplierSource != null) multiplierSource.Stop();
            StopMachineSounds();
            
        }

        // ---------- Settings ----------

        public void SetMusicEnabled(bool enabledState)
        {
            Settings.musicEnabled = enabledState;
            ApplyMusic();
            SettingsChanged();
        }

        public void SetSfxEnabled(bool enabledState)
        {
            Settings.sfxEnabled = enabledState;
            if (!enabledState) StopRuntimeVoices();
            SettingsChanged();
        }

        /// <summary>Turns both music and sound effects on or off together.</summary>
        public void SetSoundOn(bool on)
        {
            Settings.musicEnabled = on;
            Settings.sfxEnabled = on;
            if (!on) StopRuntimeVoices();
            ApplyMusic();
            SettingsChanged();
        }

        // ---------- Helpers ----------

        private void HandleSettingsReloaded()
        {
            ApplyMusic();
            OnSettingsChanged?.Invoke();
        }

        private void SettingsChanged()
        {
            if (saveManager != null && saveManager.IsReady) saveManager.SaveGame();
            OnSettingsChanged?.Invoke();
        }

        private void ApplyMusic()
        {
            if (!isReady || musicClip == null) return;

            musicSource.volume = Mathf.Clamp01(Settings.musicVolume);

            if (Settings.musicEnabled)
            {
                if (musicSource.clip != musicClip) musicSource.clip = musicClip;
                if (!musicSource.isPlaying) musicSource.Play();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }
}
