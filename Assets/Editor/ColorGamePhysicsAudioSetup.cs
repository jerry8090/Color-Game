using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shared.Audio;

/// <summary>
/// Menu: Tools > Color Game > Generate and Assign Physics + Countdown Sounds
/// Creates PLACEHOLDER sounds (plain synthesized tones and noise, NOT recorded audio) for the cube impacts,
/// settling, rack and betting countdown, then fills the empty slots of the AudioManager in the open scene.
/// It never overwrites a file or a clip you already assigned, so you can replace them with real sounds later
/// using the same file names. Place in Assets/Editor/.
/// </summary>
public static class ColorGamePhysicsAudioSetup
{
    private const int SampleRate = 44100;
    private const string Folder = "Assets/ColorGame/Audio/SFX";

    private static readonly (SfxType type, string file)[] Files =
    {
        (SfxType.CubeHitCube,   "sfx_cube_hit_cube.wav"),
        (SfxType.CubeHitFloor,  "sfx_cube_hit_floor.wav"),
        (SfxType.CubeHitWall,   "sfx_cube_hit_wall.wav"),
        (SfxType.CubeSettle,    "sfx_cube_settle.wav"),
        (SfxType.RackMove,      "sfx_rack_move.wav"),
        (SfxType.CountdownOpen, "sfx_count_open.wav"),
        (SfxType.CountdownTick, "sfx_count_tick.wav"),
        (SfxType.CountdownFinal,"sfx_count_final.wav")
    };

    [MenuItem("Tools/Color Game/Generate and Assign Physics + Countdown Sounds")]
    public static void Run()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[PhysicsAudio] Stop Play mode first.");
            return;
        }

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(projectRoot, Folder));

        // Wooden clack between two cubes: short, bright.
        float[] hitCube = new float[Samples(0.12f)];
        AddTone(hitCube, 0f, 1500f, 0.07f, 0.5f);
        AddTone(hitCube, 0f, 2300f, 0.04f, 0.3f);
        AddNoise(hitCube, 0f, 0.025f, 0.45f, 1);
        Save("sfx_cube_hit_cube.wav", hitCube);

        // Cube on the floor: lower, heavier thud.
        float[] hitFloor = new float[Samples(0.22f)];
        AddTone(hitFloor, 0f, 130f, 0.18f, 0.9f);
        AddTone(hitFloor, 0f, 270f, 0.1f, 0.45f);
        AddNoise(hitFloor, 0f, 0.03f, 0.4f, 2);
        Save("sfx_cube_hit_floor.wav", hitFloor);

        // Cube on a side wall or rail: mid knock.
        float[] hitWall = new float[Samples(0.15f)];
        AddTone(hitWall, 0f, 520f, 0.1f, 0.6f);
        AddTone(hitWall, 0f, 910f, 0.06f, 0.35f);
        AddNoise(hitWall, 0f, 0.02f, 0.35f, 3);
        Save("sfx_cube_hit_wall.wav", hitWall);

        // Settling: a tiny soft contact.
        float[] settle = new float[Samples(0.1f)];
        AddTone(settle, 0f, 320f, 0.07f, 0.35f);
        Save("sfx_cube_settle.wav", settle);

        // Rack: low mechanical rumble.
        float[] rack = new float[Samples(0.7f)];
        AddTone(rack, 0f, 70f, 0.65f, 0.5f);
        AddTone(rack, 0.05f, 105f, 0.5f, 0.25f);
        AddNoise(rack, 0f, 0.6f, 0.12f, 4);
        Save("sfx_rack_move.wav", rack);

        // Betting window opens: soft two-note chime.
        float[] open = new float[Samples(0.5f)];
        AddTone(open, 0f, 880f, 0.3f, 0.35f);
        AddTone(open, 0.12f, 1320f, 0.35f, 0.3f);
        Save("sfx_count_open.wav", open);

        // Tick: one short blip.
        float[] tick = new float[Samples(0.1f)];
        AddTone(tick, 0f, 1000f, 0.06f, 0.6f);
        Save("sfx_count_tick.wav", tick);

        // Final warning: two higher blips.
        float[] final = new float[Samples(0.35f)];
        AddTone(final, 0f, 1500f, 0.1f, 0.7f);
        AddTone(final, 0.15f, 1500f, 0.14f, 0.7f);
        Save("sfx_count_final.wav", final);

        AssetDatabase.Refresh();
        Assign();
    }

    private static void Assign()
    {
        AudioManager audio = UnityEngine.Object.FindFirstObjectByType<AudioManager>(FindObjectsInactive.Include);
        if (audio == null)
        {
            Debug.LogWarning("[PhysicsAudio] Sounds were created, but no AudioManager is in the open scene. " +
                             "Open the ColorGame scene and run this menu item again.");
            return;
        }

        SerializedObject so = new SerializedObject(audio);
        SerializedProperty list = so.FindProperty("sfxEntries");
        if (list == null)
        {
            Debug.LogError("[PhysicsAudio] AudioManager has no 'sfxEntries' list. Is the new AudioManager.cs installed?");
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
        Debug.Log($"[PhysicsAudio] Placeholder sounds ready in {Folder}. Assigned {assigned} empty slot(s) on AudioManager. " +
                  "These are synthesized placeholders, not realistic recordings. Save the scene (Ctrl+S).");
    }

    private static int FindRow(SerializedProperty list, SfxType type)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty typeProperty = list.GetArrayElementAtIndex(i).FindPropertyRelative("type");
            if (typeProperty.enumValueIndex == (int)type) return i;
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

    private static void Save(string fileName, float[] samples)
    {
        string path = Path.Combine(Application.dataPath, "ColorGame/Audio/SFX", fileName);

        if (File.Exists(path))
        {
            Debug.Log("[PhysicsAudio] Skipped (already exists): " + fileName);
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
