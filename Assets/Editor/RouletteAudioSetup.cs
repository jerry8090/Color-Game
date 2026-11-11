using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shared.Audio;
using RouletteGame.Integration;
using RouletteGame.Wheel;

/// <summary>
/// Menu: Tools > Roulette Game > Generate Sounds and Install Audio   (open the Roulette scene first)
/// Creates PLACEHOLDER sounds (plain synthesized tones and noise, NOT recorded audio) for chips, bet confirmation,
/// the spinning wheel with its rattling ball, and the ball dropping into its pocket.
/// Then it fills the EMPTY slots of the AudioManager in the open scene and adds the RouletteAudio listener if it is missing.
/// It never overwrites a file or a clip you already assigned, so you can replace the sounds later with real ones
/// using the same file names.
/// Needs the five new SfxType entries (ChipPlace, ChipRemove, BetConfirm, WheelSpin, BallDrop) in AudioManager.cs.
/// </summary>
public static class RouletteAudioSetup
{
    private const int SampleRate = 44100;
    private const string Folder = "Assets/RouletteGame/Audio";

    private static readonly (SfxType type, string file)[] Files =
    {
        (SfxType.ChipPlace, "sfx_chip_place.wav"),
        (SfxType.ChipRemove, "sfx_chip_remove.wav"),
        (SfxType.BetConfirm, "sfx_bet_confirm.wav"),
        (SfxType.WheelSpin, "sfx_wheel_spin.wav"),
        (SfxType.BallDrop, "sfx_ball_drop.wav")
    };

    [MenuItem("Tools/Roulette Game/Generate Sounds and Install Audio")]
    public static void Run()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[RouletteAudio] Stop Play mode first.");
            return;
        }

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(projectRoot, Folder));

        // Chip placed: a short plastic click-clack.
        float[] place = new float[Samples(0.12f)];
        AddNoise(place, 0f, 0.015f, 0.5f, 11);
        AddTone(place, 0f, 2400f, 0.04f, 0.45f);
        AddTone(place, 0.02f, 1300f, 0.06f, 0.4f);
        Save("sfx_chip_place.wav", place);

        // Chip removed: lower and softer.
        float[] remove = new float[Samples(0.12f)];
        AddNoise(remove, 0f, 0.012f, 0.3f, 12);
        AddTone(remove, 0f, 900f, 0.05f, 0.35f);
        AddTone(remove, 0.03f, 600f, 0.06f, 0.3f);
        Save("sfx_chip_remove.wav", remove);

        // Bets confirmed: two rising notes and a sparkle.
        float[] confirm = new float[Samples(0.55f)];
        AddTone(confirm, 0f, 660f, 0.25f, 0.4f);
        AddTone(confirm, 0.1f, 880f, 0.3f, 0.4f);
        AddTone(confirm, 0.2f, 1320f, 0.3f, 0.25f);
        Save("sfx_bet_confirm.wav", confirm);

        // Wheel spinning: a low whir that fades out, and a ball whose clicks get slower and slower.
        float[] spin = new float[Samples(8f)];
        AddWhir(spin);
        float time = 0.2f;
        float interval = 0.04f;
        int seed = 100;
        while (time < 7.4f)
        {
            AddTone(spin, time, 2200f, 0.012f, 0.35f);
            AddNoise(spin, time, 0.008f, 0.3f, seed++);
            interval *= 1.045f;
            time += interval;
        }
        Save("sfx_wheel_spin.wav", spin);

        // Ball drops into a pocket: a clack and three small bounces.
        float[] drop = new float[Samples(0.6f)];
        AddTone(drop, 0f, 1800f, 0.05f, 0.6f);
        AddTone(drop, 0f, 900f, 0.08f, 0.45f);
        AddNoise(drop, 0f, 0.02f, 0.5f, 21);
        AddTone(drop, 0.13f, 1500f, 0.04f, 0.35f);
        AddTone(drop, 0.23f, 1300f, 0.035f, 0.22f);
        AddTone(drop, 0.31f, 1100f, 0.03f, 0.14f);
        Save("sfx_ball_drop.wav", drop);

        AssetDatabase.Refresh();
        Assign();
    }

    private static void Assign()
    {
        AudioManager audio = UnityEngine.Object.FindAnyObjectByType<AudioManager>(FindObjectsInactive.Include);
        if (audio == null)
        {
            Debug.LogWarning("[RouletteAudio] Sounds were created, but no AudioManager is in the open scene. " +
                             "Open the Roulette scene and run this menu item again.");
            return;
        }

        SerializedObject so = new SerializedObject(audio);
        SerializedProperty list = so.FindProperty("sfxEntries");
        if (list == null)
        {
            Debug.LogError("[RouletteAudio] AudioManager has no 'sfxEntries' list.");
            return;
        }

        // Make sure every SfxType has a row.
        foreach (SfxType type in Enum.GetValues(typeof(SfxType)))
        {
            if (FindRow(list, type) < 0)
            {
                list.arraySize++;
                SerializedProperty row = list.GetArrayElementAtIndex(list.arraySize - 1);
                row.FindPropertyRelative("type").enumValueIndex = (int)type;
                row.FindPropertyRelative("clip").objectReferenceValue = null;
                row.FindPropertyRelative("volume").floatValue = 1f;
            }
        }

        int assigned = 0;
        foreach ((SfxType type, string file) in Files)
        {
            int index = FindRow(list, type);
            if (index < 0) continue;

            SerializedProperty clipProperty = list.GetArrayElementAtIndex(index).FindPropertyRelative("clip");
            if (clipProperty.objectReferenceValue != null) continue; // keep clips you chose yourself

            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(Folder + "/" + file);
            if (clip == null) continue;

            clipProperty.objectReferenceValue = clip;
            assigned++;
        }

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(audio);
        EditorSceneManager.MarkSceneDirty(audio.gameObject.scene);

        string listener = InstallListener(audio);
        Debug.Log("[RouletteAudio] Placeholder sounds ready in " + Folder + ". Assigned " + assigned + " empty slot(s) on AudioManager. " +
                  listener + " These are synthesized placeholders, not recordings. Save the scene (Ctrl+S).");
    }

    // Adds the listener next to the game controller when the scene has none (for example a scene built before this phase).
    private static string InstallListener(AudioManager audio)
    {
        if (UnityEngine.Object.FindAnyObjectByType<RouletteAudio>(FindObjectsInactive.Include) != null) return "RouletteAudio was already in the scene.";

        RouletteGameController controller = UnityEngine.Object.FindAnyObjectByType<RouletteGameController>(FindObjectsInactive.Include);
        WheelView wheel = UnityEngine.Object.FindAnyObjectByType<WheelView>(FindObjectsInactive.Include);
        if (controller == null || wheel == null) return "No RouletteGameController or WheelView here, so the RouletteAudio listener was not added.";

        RouletteAudio listener = controller.gameObject.AddComponent<RouletteAudio>();
        SerializedObject so = new SerializedObject(listener);
        so.FindProperty("audioManager").objectReferenceValue = audio;
        so.FindProperty("controller").objectReferenceValue = controller;
        so.FindProperty("wheel").objectReferenceValue = wheel;
        so.ApplyModifiedPropertiesWithoutUndo();
        return "RouletteAudio was added next to the controller.";
    }

    private static int FindRow(SerializedProperty list, SfxType type)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            if (list.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex == (int)type) return i;
        }
        return -1;
    }

    // ---------- Tone and file helpers ----------

    private static int Samples(float seconds)
    {
        return Mathf.CeilToInt(seconds * SampleRate);
    }

    private static void AddTone(float[] buffer, float startSeconds, float frequency, float durationSeconds, float volume)
    {
        int start = Mathf.RoundToInt(startSeconds * SampleRate);
        int count = Samples(durationSeconds);

        for (int i = 0; i < count; i++)
        {
            int index = start + i;
            if (index >= buffer.Length) break;

            float t = i / (float)SampleRate;
            float progress = i / (float)count;
            float attack = Mathf.Clamp01(t / 0.002f);
            float decay = (1f - progress) * (1f - progress);

            buffer[index] += Mathf.Sin(2f * Mathf.PI * frequency * t) * volume * attack * decay;
        }
    }

    // Fixed seed, so the generated file is always the same.
    private static void AddNoise(float[] buffer, float startSeconds, float durationSeconds, float volume, int seed)
    {
        System.Random random = new System.Random(seed);
        int start = Mathf.RoundToInt(startSeconds * SampleRate);
        int count = Samples(durationSeconds);

        for (int i = 0; i < count; i++)
        {
            int index = start + i;
            if (index >= buffer.Length) break;

            float progress = i / (float)count;
            float decay = (1f - progress) * (1f - progress);
            buffer[index] += ((float)random.NextDouble() * 2f - 1f) * volume * decay;
        }
    }

    // A low whirring wheel: the pitch sinks slowly and the sound fades out over the last 1.5 seconds.
    private static void AddWhir(float[] buffer)
    {
        double phase = 0.0;
        float length = buffer.Length / (float)SampleRate;

        for (int i = 0; i < buffer.Length; i++)
        {
            float t = i / (float)SampleRate;
            float frequency = Mathf.Lerp(120f, 55f, t / length);
            phase += 2.0 * Math.PI * frequency / SampleRate;

            float tremolo = 0.75f + 0.25f * Mathf.Sin(2f * Mathf.PI * 9f * t);
            float fade = Mathf.Clamp01((length - t) / 1.5f) * Mathf.Clamp01(t / 0.1f);
            buffer[i] += (float)Math.Sin(phase) * 0.16f * tremolo * fade;
        }
    }

    private static void Save(string fileName, float[] samples)
    {
        string path = Path.Combine(Application.dataPath, "RouletteGame/Audio", fileName);

        if (File.Exists(path))
        {
            Debug.Log("[RouletteAudio] Skipped (already exists): " + fileName);
            return;
        }

        int dataSize = samples.Length * 2;

        using (FileStream stream = new FileStream(path, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(36 + dataSize);
            writer.Write(Encoding.ASCII.GetBytes("WAVE"));
            writer.Write(Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)1);
            writer.Write(SampleRate);
            writer.Write(SampleRate * 2);
            writer.Write((short)2);
            writer.Write((short)16);
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(dataSize);

            foreach (float sample in samples)
            {
                writer.Write((short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue));
            }
        }
    }
}
