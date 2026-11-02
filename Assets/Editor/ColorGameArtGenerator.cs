using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ColorGame.Core;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Design > 1 - Generate Art
/// Generates fictional game-money bill sprites (one per bet amount in your config, each with a drawn peso sign),
/// plus AreaFrame, AreaHighlight and Gloss sprites for the color betting areas, and fills BillSpriteLibrary.
/// Existing files are overwritten. Put your own artwork in the library afterwards if you prefer.
/// </summary>
public static class ColorGameArtGenerator
{
    private const string UiFolder = "Assets/ColorGame/UI";
    private const string BillFolder = "Assets/ColorGame/UI/Bills";
    private const string ConfigPath = "Assets/ColorGame/ScriptableObjects/ColorGameConfig.asset";
    private const string LibraryPath = "Assets/ColorGame/UI/Bills/BillSpriteLibrary.asset";
    private const int BillW = 320;
    private const int BillH = 150;

    private struct BillStyle
    {
        public Color top, bottom, border, line, medal;
        public int pattern;
    }

    private static Color C(int r, int g, int b) { return new Color32((byte)r, (byte)g, (byte)b, 255); }

    private static readonly BillStyle[] Styles =
    {
        new BillStyle { top = C(226,196,150), bottom = C(184,142,92),  border = C(96,64,34),   line = C(250,236,204), medal = C(246,228,190), pattern = 0 },
        new BillStyle { top = C(255,184,96),  bottom = C(236,128,34),  border = C(130,62,10),  line = C(255,226,170), medal = C(255,222,160), pattern = 1 },
        new BillStyle { top = C(244,108,108), bottom = C(190,40,52),   border = C(100,16,28),  line = C(255,200,200), medal = C(255,214,214), pattern = 2 },
        new BillStyle { top = C(176,136,236), bottom = C(108,68,190),  border = C(255,213,74), line = C(255,236,170), medal = C(255,226,120), pattern = 3 },
        new BillStyle { top = C(116,210,134), bottom = C(36,146,86),   border = C(255,213,74), line = C(220,255,226), medal = C(255,226,120), pattern = 4 },
        new BillStyle { top = C(52,38,98),    bottom = C(22,14,50),    border = C(255,213,74), line = C(255,213,74), medal = C(255,213,74),  pattern = 5 }
    };

    [MenuItem("Tools/Color Game/Design/1 - Generate Art (frames, gloss, bills)")]
    public static void Generate()
    {
        string root = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(root, BillFolder));

        MakeFrame(root, UiFolder + "/AreaFrame.png");
        MakeHighlight(root, UiFolder + "/AreaHighlight.png");
        MakeGloss(root, UiFolder + "/Gloss.png");

        ColorGameConfig config = AssetDatabase.LoadAssetAtPath<ColorGameConfig>(ConfigPath);
        if (config == null) { Debug.LogError("[Art] ColorGameConfig not found at " + ConfigPath); return; }

        List<int> amounts = new List<int>();
        foreach (int a in config.BetAmounts) if (!amounts.Contains(a)) amounts.Add(a);
        amounts.Sort();

        List<string> paths = new List<string>();
        for (int i = 0; i < amounts.Count; i++)
        {
            string path = BillFolder + "/Bill_" + amounts[i] + ".png";
            MakeBill(root, path, Styles[i % Styles.Length]);
            paths.Add(path);
        }

        AssetDatabase.Refresh();
        for (int i = 0; i < paths.Count; i++) ConfigureSprite(paths[i], Vector4.zero, 100f);

        BillSpriteLibrary library = AssetDatabase.LoadAssetAtPath<BillSpriteLibrary>(LibraryPath);
        if (library == null)
        {
            library = ScriptableObject.CreateInstance<BillSpriteLibrary>();
            AssetDatabase.CreateAsset(library, LibraryPath);
        }

        SerializedObject so = new SerializedObject(library);
        SerializedProperty list = so.FindProperty("entries");
        list.arraySize = amounts.Count;
        for (int i = 0; i < amounts.Count; i++)
        {
            SerializedProperty entry = list.GetArrayElementAtIndex(i);
            entry.FindPropertyRelative("denomination").intValue = amounts[i];
            entry.FindPropertyRelative("sprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>(paths[i]);
        }
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();

        Debug.Log("[Art] Generated frame, highlight, gloss and " + amounts.Count + " bill sprites in " + UiFolder);
    }

    // ---------- Frames ----------

    private static void MakeFrame(string root, string path)
    {
        const int size = 128;
        Color32[] px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = BoxD(x + 0.5f - 64f, y + 0.5f - 64f, 64f, 64f, 40f);
                float alpha = Mathf.Clamp01(0.5f - d) * Mathf.Clamp01(0.5f + (d + 7f)); // a 7 px ring
                px[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
            }
        }
        Save(root, path, px, size, size);
        ConfigureSprite(path, new Vector4(48, 48, 48, 48), 100f);
    }

    private static void MakeHighlight(string root, string path)
    {
        const int size = 128;
        Color32[] px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = BoxD(x + 0.5f - 64f, y + 0.5f - 64f, 64f, 64f, 40f);
                float band = Mathf.Clamp01(1.5f - Mathf.Abs(d + 9f));       // a thin inner line
                float fade = Mathf.Lerp(0.25f, 1f, y / (float)(size - 1));  // brighter at the top
                px[y * size + x] = new Color32(255, 255, 255, (byte)(band * fade * 255f));
            }
        }
        Save(root, path, px, size, size);
        ConfigureSprite(path, new Vector4(48, 48, 48, 48), 100f);
    }

    private static void MakeGloss(string root, string path)
    {
        const int size = 128;
        Color32[] px = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = BoxD(x + 0.5f - 64f, y + 0.5f - 64f, 64f, 64f, 40f);
                float shape = Mathf.Clamp01(0.5f - (d + 8f));
                float fade = Mathf.Clamp01((y / (float)(size - 1) - 0.45f) / 0.55f) * 0.32f;
                px[y * size + x] = new Color32(255, 255, 255, (byte)(shape * fade * 255f));
            }
        }
        Save(root, path, px, size, size);
        ConfigureSprite(path, new Vector4(48, 48, 48, 48), 100f);
    }

    // ---------- Bills ----------

    private static void MakeBill(string root, string path, BillStyle s)
    {
        Color32[] px = new Color32[BillW * BillH];
        float mx = BillW * 0.18f;
        float my = BillH * 0.5f;
        float radius = BillH * 0.36f;
        Color glyph = new Color(0.14f, 0.08f, 0.2f, 1f);

        for (int y = 0; y < BillH; y++)
        {
            for (int x = 0; x < BillW; x++)
            {
                float d = BoxD(x + 0.5f - BillW * 0.5f, y + 0.5f - BillH * 0.5f, BillW * 0.5f, BillH * 0.5f, 18f);
                float alpha = Mathf.Clamp01(0.5f - d);
                if (alpha <= 0f) { px[y * BillW + x] = new Color32(0, 0, 0, 0); continue; }

                Color c = Color.Lerp(s.bottom, s.top, y / (float)(BillH - 1));
                c = Pattern(c, s, x, y, mx, my);

                // Border band and a thin inner line.
                c = Color.Lerp(c, s.border, Mathf.Clamp01(0.5f + (d + 6f)));
                c = Color.Lerp(c, s.line, Mathf.Clamp01(1f - Mathf.Abs(d + 10f)) * 0.8f);

                // Soft dark plate behind the digits, so white digits stay readable on every bill.
                float plate = BoxD(x + 0.5f - BillW * 0.665f, y + 0.5f - BillH * 0.5f, BillW * 0.285f, BillH * 0.32f, 14f);
                c = Color.Lerp(c, Color.black, Mathf.Clamp01(0.5f - plate) * 0.3f);

                // Medallion with the peso sign.
                float dx = x + 0.5f - mx;
                float dy = y + 0.5f - my;
                float md = Mathf.Sqrt(dx * dx + dy * dy) - radius;
                float fill = Mathf.Clamp01(0.5f - md);
                c = Color.Lerp(c, s.medal, fill);
                c = Color.Lerp(c, s.border, Mathf.Clamp01(1f - Mathf.Abs(md + 3f)));
                c = Color.Lerp(c, glyph, PesoCoverage(dx, dy, radius * 1.1f) * fill);

                px[y * BillW + x] = new Color32(
                    (byte)(Mathf.Clamp01(c.r) * 255f), (byte)(Mathf.Clamp01(c.g) * 255f),
                    (byte)(Mathf.Clamp01(c.b) * 255f), (byte)(alpha * 255f));
            }
        }
        Save(root, path, px, BillW, BillH);
    }

    private static Color Pattern(Color c, BillStyle s, int x, int y, float mx, float my)
    {
        bool lighten = false;
        float amount = 0.15f;

        switch (s.pattern)
        {
            case 1: lighten = Mathf.Sin((x + y) * 0.35f) > 0.85f; amount = 0.12f; break;
            case 2:
                {
                    int gx = x % 16 - 8, gy = y % 16 - 8;
                    lighten = gx * gx + gy * gy < 6;
                    break;
                }
            case 3:
                {
                    float band = Mathf.Abs(Mathf.Repeat(y + 6f * Mathf.Sin(x * 0.05f), 9f) - 4.5f);
                    lighten = band < 0.8f; amount = 0.18f;
                    break;
                }
            case 4:
                {
                    float a = Mathf.Abs(Mathf.Repeat(x + y, 18f) - 9f);
                    float b = Mathf.Abs(Mathf.Repeat(x - y, 18f) - 9f);
                    lighten = Mathf.Min(a, b) < 0.9f; amount = 0.16f;
                    break;
                }
            case 5: lighten = Mathf.Sin(Mathf.Atan2(y - my, x - mx) * 14f) > 0.7f; amount = 0.14f; break;
        }

        return lighten ? Color.Lerp(c, Color.white, amount) : c;
    }

    // A "P" with two horizontal bars, drawn with simple distance shapes. Unit = glyph height in pixels.
    private static float PesoCoverage(float x, float y, float unit)
    {
        float u = x / unit;
        float v = y / unit;

        float stem = Mathf.Max(Mathf.Abs(u + 0.17f) - 0.075f, Mathf.Abs(v) - 0.5f);
        float ring = Mathf.Abs(Mathf.Sqrt((u + 0.12f) * (u + 0.12f) + (v - 0.2f) * (v - 0.2f)) - 0.235f) - 0.07f;
        ring = Mathf.Max(ring, -(u + 0.12f)); // right half of the bowl only
        float bar1 = Mathf.Max(Mathf.Abs(u - 0.02f) - 0.40f, Mathf.Abs(v - 0.11f) - 0.035f);
        float bar2 = Mathf.Max(Mathf.Abs(u - 0.02f) - 0.40f, Mathf.Abs(v - 0.27f) - 0.035f);

        float dist = Mathf.Min(Mathf.Min(stem, ring), Mathf.Min(bar1, bar2));
        return Mathf.Clamp01(0.5f - dist * unit);
    }

    // ---------- Helpers ----------

    private static float BoxD(float px, float py, float halfW, float halfH, float radius)
    {
        float qx = Mathf.Abs(px) - (halfW - radius);
        float qy = Mathf.Abs(py) - (halfH - radius);
        float outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)).magnitude;
        float inside = Mathf.Min(Mathf.Max(qx, qy), 0f);
        return outside + inside - radius;
    }

    private static void Save(string root, string assetPath, Color32[] pixels, int w, int h)
    {
        Texture2D texture = new Texture2D(w, h, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply();
        File.WriteAllBytes(Path.Combine(root, assetPath), texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }

    private static void ConfigureSprite(string assetPath, Vector4 border, float pixelsPerUnit)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) { Debug.LogError("[Art] Could not configure " + assetPath); return; }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spriteBorder = border;
        importer.spritePixelsPerUnit = pixelsPerUnit;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.filterMode = FilterMode.Bilinear;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();
    }
}