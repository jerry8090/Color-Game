using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Generates simple placeholder sound effects (plain tones) into Assets/ColorGame/Audio/SFX.
/// Replace them later with real sounds using the same file names.
/// Menu: Tools > Color Game > Generate Placeholder Sounds
/// </summary>
public static class ColorGamePlaceholderAudio
{
    private const int SampleRate = 44100;

    [MenuItem("Tools/Color Game/Generate Placeholder Sounds")]
    public static void Generate()
    {
        string folder = Path.Combine(Application.dataPath, "ColorGame/Audio/SFX");
        Directory.CreateDirectory(folder);

        // Button click: one short blip
        float[] click = new float[Samples(0.1f)];
        AddTone(click, 0f, 880f, 0.06f, 0.6f);
        Save(folder, "sfx_button_click.wav", click);

        // Color select: two quick rising blips
        float[] select = new float[Samples(0.16f)];
        AddTone(select, 0f, 660f, 0.07f, 0.6f);
        AddTone(select, 0.07f, 990f, 0.08f, 0.6f);
        Save(folder, "sfx_color_select.wav", select);

        // Roll: a ticking sound that lasts about as long as the roll animation
        float[] roll = new float[Samples(2.4f)];
        for (int i = 0; i < 22; i++)
        {
            AddTone(roll, i * 0.1f, 500f + (i % 3) * 120f, 0.05f, 0.5f);
        }
        Save(folder, "sfx_roll.wav", roll);

        // Slot stop: a low thud
        float[] stop = new float[Samples(0.25f)];
        AddTone(stop, 0f, 180f, 0.15f, 0.9f);
        AddTone(stop, 0f, 360f, 0.06f, 0.4f);
        Save(folder, "sfx_slot_stop.wav", stop);

        // Win: rising four-note arpeggio
        float[] win = new float[Samples(0.9f)];
        float[] winNotes = { 523f, 659f, 784f, 1047f };
        for (int i = 0; i < winNotes.Length; i++) AddTone(win, i * 0.1f, winNotes[i], 0.3f, 0.6f);
        Save(folder, "sfx_win.wav", win);

        // Lose: falling three-note phrase
        float[] lose = new float[Samples(0.8f)];
        float[] loseNotes = { 330f, 262f, 196f };
        for (int i = 0; i < loseNotes.Length; i++) AddTone(lose, i * 0.15f, loseNotes[i], 0.3f, 0.6f);
        Save(folder, "sfx_lose.wav", lose);

        AssetDatabase.Refresh();
        Debug.Log("Placeholder sounds ready in Assets/ColorGame/Audio/SFX");
    }

    private static int Samples(float seconds)
    {
        return Mathf.CeilToInt(seconds * SampleRate);
    }

    // Adds a sine tone with a quick attack and a smooth fade-out into the buffer.
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
            float attack = Mathf.Clamp01(t / 0.004f);
            float decay = (1f - progress) * (1f - progress);

            buffer[index] += Mathf.Sin(2f * Mathf.PI * frequency * t) * volume * attack * decay;
        }
    }

    private static void Save(string folder, string fileName, float[] samples)
    {
        string path = Path.Combine(folder, fileName);

        if (File.Exists(path))
        {
            Debug.Log("Skipped (already exists): " + fileName);
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
            writer.Write(16);                    // PCM header size
            writer.Write((short)1);              // PCM
            writer.Write((short)1);              // mono
            writer.Write(SampleRate);
            writer.Write(SampleRate * 2);        // bytes per second
            writer.Write((short)2);              // bytes per sample
            writer.Write((short)16);             // bits per sample
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(dataSize);

            foreach (float sample in samples)
            {
                writer.Write((short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue));
            }
        }
    }
}