using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shared.Audio;

/// <summary>
/// Menu: Tools > Color Game > Generate and Assign Match Sounds   (open the game scene first)
/// Creates PLACEHOLDER synthesized sounds (not recordings) for Match2 and Match3 and fills EMPTY slots only.
/// Replace them any time by dragging your own clips into the AudioManager list.
/// </summary>
public static class ColorGameMatchAudioSetup
{
    private const int SampleRate = 44100;
    private const string Folder = "Assets/ColorGame/Audio/SFX";

    [MenuItem("Tools/Color Game/Generate and Assign Match Sounds")]
    public static void Run()
    {
        if (EditorApplication.isPlaying) { Debug.LogWarning("[MatchAudio] Stop Play mode first."); return; }

        Directory.CreateDirectory(Path.Combine(Directory.GetParent(Application.dataPath).FullName, Folder));

        // Two matches: quick rising arpeggio with a sparkle.
        float[] two = new float[Samples(0.8f)];
        AddTone(two, 0.00f, 523f, 0.30f, 0.50f);
        AddTone(two, 0.09f, 659f, 0.30f, 0.50f);
        AddTone(two, 0.18f, 784f, 0.40f, 0.55f);
        AddTone(two, 0.28f, 1568f, 0.30f, 0.20f);
        Save("sfx_match2.wav", two);

        // Three matches: longer fanfare ending in a bright chord.
        float[] three = new float[Samples(1.9f)];
        float[] run = { 523f, 659f, 784f, 1047f, 784f, 1047f, 1319f, 1568f };
        for (int i = 0; i < run.Length; i++) AddTone(three, i * 0.08f, run[i], 0.30f, 0.45f);
        foreach (float f in new[] { 1047f, 1319f, 1568f, 2093f }) AddTone(three, 0.7f, f, 1.1f, 0.30f);
        AddTone(three, 0.7f, 262f, 1.1f, 0.35f);
        Save("sfx_match3.wav", three);

        AssetDatabase.Refresh();
        Assign();
    }

    private static void Assign()
    {
        AudioManager audio = UnityEngine.Object.FindAnyObjectByType<AudioManager>(FindObjectsInactive.Include);
        if (audio == null) { Debug.LogWarning("[MatchAudio] No AudioManager in the open scene."); return; }

        SerializedObject so = new SerializedObject(audio);
        SerializedProperty list = so.FindProperty("sfxEntries");

        foreach (SfxType type in Enum.GetValues(typeof(SfxType)))
        {
            if (FindRow(list, type) >= 0) continue;
            list.arraySize++;
            SerializedProperty row = list.GetArrayElementAtIndex(list.arraySize - 1);
            row.FindPropertyRelative("type").enumValueIndex = (int)type;
            row.FindPropertyRelative("clip").objectReferenceValue = null;
            row.FindPropertyRelative("volume").floatValue = 1f;
        }

        Fill(list, SfxType.Match2, "sfx_match2.wav");
        Fill(list, SfxType.Match3, "sfx_match3.wav");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(audio);
        EditorSceneManager.MarkSceneDirty(audio.gameObject.scene);
        Debug.Log("[MatchAudio] Match2 / Match3 sounds ready. Save the scene (Ctrl+S).");
    }

    private static void Fill(SerializedProperty list, SfxType type, string file)
    {
        int index = FindRow(list, type);
        if (index < 0) return;
        SerializedProperty clip = list.GetArrayElementAtIndex(index).FindPropertyRelative("clip");
        if (clip.objectReferenceValue != null) return; // keep clips you chose yourself
        clip.objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioClip>(Folder + "/" + file);
    }

    private static int FindRow(SerializedProperty list, SfxType type)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            if (list.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex == (int)type) return i;
        }
        return -1;
    }

    private static int Samples(float seconds) { return Mathf.CeilToInt(seconds * SampleRate); }

    private static void AddTone(float[] buffer, float start, float freq, float duration, float volume)
    {
        int s = Mathf.RoundToInt(start * SampleRate);
        int count = Samples(duration);
        for (int i = 0; i < count; i++)
        {
            int index = s + i;
            if (index >= buffer.Length) break;
            float t = i / (float)SampleRate;
            float p = i / (float)count;
            buffer[index] += Mathf.Sin(2f * Mathf.PI * freq * t) * volume * Mathf.Clamp01(t / 0.004f) * (1f - p) * (1f - p);
        }
    }

    private static void Save(string fileName, float[] samples)
    {
        string path = Path.Combine(Application.dataPath, "ColorGame/Audio/SFX", fileName);
        if (File.Exists(path)) return;

        int dataSize = samples.Length * 2;
        using (FileStream stream = new FileStream(path, FileMode.Create))
        using (BinaryWriter w = new BinaryWriter(stream))
        {
            w.Write(Encoding.ASCII.GetBytes("RIFF")); w.Write(36 + dataSize);
            w.Write(Encoding.ASCII.GetBytes("WAVE")); w.Write(Encoding.ASCII.GetBytes("fmt "));
            w.Write(16); w.Write((short)1); w.Write((short)1);
            w.Write(SampleRate); w.Write(SampleRate * 2); w.Write((short)2); w.Write((short)16);
            w.Write(Encoding.ASCII.GetBytes("data")); w.Write(dataSize);
            foreach (float sample in samples) w.Write((short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue));
        }
    }
}