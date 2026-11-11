using System.IO;
using UnityEditor;
using UnityEngine;
using RouletteGame.Core;

/// <summary>
/// Menu: Tools > Roulette Game > Generate Wheel Sprites
/// Draws the wheel picture and the ball (no art software needed) into Assets/RouletteGame/Sprites.
/// The wheel has 37 pockets in European order with the zero at the TOP, clockwise: exactly what WheelView expects.
/// Existing files are overwritten.
/// </summary>
public static class RouletteWheelSpriteGenerator
{
    private const string Folder = "Assets/RouletteGame/Sprites";
    private const int WheelSize = 1024;
    private const int BallSize = 64;

    [MenuItem("Tools/Roulette Game/Generate Wheel Sprites")]
    public static void Generate()
    {
        string root = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(root, Folder));

        SavePng(root, Folder + "/RouletteWheel.png", DrawWheel(), WheelSize);
        SavePng(root, Folder + "/RouletteBall.png", DrawBall(), BallSize);

        AssetDatabase.Refresh();
        Configure(Folder + "/RouletteWheel.png");
        Configure(Folder + "/RouletteBall.png");
        Debug.Log("[RouletteSprites] Generated RouletteWheel.png and RouletteBall.png in " + Folder);
    }

    private static Color32[] DrawWheel()
    {
        Color32[] px = new Color32[WheelSize * WheelSize];
        float c = WheelSize * 0.5f;
        float step = 360f / RouletteLayout.PocketCount;

        Color wood = new Color32(70, 36, 24, 255);
        Color gold = new Color32(226, 190, 92, 255);
        Color green = new Color32(0, 140, 70, 255);
        Color red = new Color32(196, 30, 42, 255);
        Color black = new Color32(24, 24, 30, 255);
        Color inner = new Color32(38, 22, 52, 255);

        for (int y = 0; y < WheelSize; y++)
        {
            for (int x = 0; x < WheelSize; x++)
            {
                float dx = (x + 0.5f - c) / c;
                float dy = (y + 0.5f - c) / c; // up is positive
                float r = Mathf.Sqrt(dx * dx + dy * dy);

                float alpha = Mathf.Clamp01((1f - r) * c);
                if (alpha <= 0f) { px[y * WheelSize + x] = new Color32(0, 0, 0, 0); continue; }

                // Clockwise angle from the top, 0 to 360.
                float angle = Mathf.Atan2(dx, dy) * Mathf.Rad2Deg;
                if (angle < 0f) angle += 360f;

                Color color;
                if (r > 0.935f)
                {
                    color = Color.Lerp(wood, wood * 1.5f, Mathf.InverseLerp(0.935f, 1f, r));
                }
                else if (r > 0.64f)
                {
                    // Pocket i is centred on i * step, so it spans half a step either side.
                    float shifted = Mathf.Repeat(angle + step * 0.5f, 360f);
                    int index = Mathf.Clamp(Mathf.FloorToInt(shifted / step), 0, RouletteLayout.PocketCount - 1);
                    PocketColor kind = RouletteLayout.ColorOf(RouletteLayout.NumberAt(index));
                    color = kind == PocketColor.Green ? green : (kind == PocketColor.Red ? red : black);

                    float local = shifted - index * step;
                    float edgeDegrees = Mathf.Min(local, step - local);
                    float edgePixels = edgeDegrees * Mathf.Deg2Rad * r * c;
                    color = Color.Lerp(color, gold, Cover(edgePixels, 1.4f));
                }
                else if (r > 0.5f)
                {
                    color = inner;
                }
                else
                {
                    color = Color.Lerp(inner * 1.4f, inner, r / 0.5f);
                    if (r < 0.12f) color = gold;
                    // four gold spokes
                    float toSpoke = Mathf.Min(Mathf.Min(Mathf.Abs(dx), Mathf.Abs(dy)), 1f) * c;
                    if (r > 0.12f) color = Color.Lerp(color, gold, Cover(toSpoke, 5f));
                }

                // thin gold rings between the zones
                color = Color.Lerp(color, gold, Cover(Mathf.Abs(r - 0.935f) * c, 2.5f));
                color = Color.Lerp(color, gold, Cover(Mathf.Abs(r - 0.64f) * c, 2f));
                color = Color.Lerp(color, gold, Cover(Mathf.Abs(r - 0.5f) * c, 2.5f));
                color = Color.Lerp(color, gold, Cover(Mathf.Abs(r - 0.3f) * c, 2f));

                px[y * WheelSize + x] = new Color32(
                    (byte)(Mathf.Clamp01(color.r) * 255f), (byte)(Mathf.Clamp01(color.g) * 255f),
                    (byte)(Mathf.Clamp01(color.b) * 255f), (byte)(alpha * 255f));
            }
        }
        return px;
    }

    private static Color32[] DrawBall()
    {
        Color32[] px = new Color32[BallSize * BallSize];
        float c = BallSize * 0.5f;

        for (int y = 0; y < BallSize; y++)
        {
            for (int x = 0; x < BallSize; x++)
            {
                float dx = (x + 0.5f - c) / c;
                float dy = (y + 0.5f - c) / c;
                float r = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01((1f - r) * c);

                // a shiny ball: bright spot toward the top left
                float light = Mathf.Clamp01(1f - Mathf.Sqrt((dx + 0.3f) * (dx + 0.3f) + (dy - 0.3f) * (dy - 0.3f)) * 0.9f);
                float v = Mathf.Lerp(0.55f, 1f, light);
                px[y * BallSize + x] = new Color32((byte)(v * 255f), (byte)(v * 255f), (byte)(Mathf.Min(1f, v * 1.02f) * 255f), (byte)(alpha * 255f));
            }
        }
        return px;
    }

    // 1 inside a line of the given half width, fading to 0 over one pixel.
    private static float Cover(float distanceInPixels, float halfWidth)
    {
        return Mathf.Clamp01(halfWidth + 0.5f - distanceInPixels);
    }

    private static void SavePng(string root, string assetPath, Color32[] pixels, int size)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply();
        File.WriteAllBytes(Path.Combine(root, assetPath), texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }

    private static void Configure(string assetPath)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null) { Debug.LogError("[RouletteSprites] Could not configure " + assetPath); return; }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.filterMode = FilterMode.Bilinear;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();
    }
}
