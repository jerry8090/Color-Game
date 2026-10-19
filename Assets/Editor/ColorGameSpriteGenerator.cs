using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Creates two UI sprites so you do not need art software:
///  - RoundedRect.png : a white rounded rectangle (9-sliced, tint it with the Image color)
///  - BackgroundGradient.png : a purple vertical gradient for the screen background
/// Menu: Tools > Color Game > Generate UI Sprites
/// </summary>
public static class ColorGameSpriteGenerator
{
    private const string OutputFolder = "Assets/ColorGame/UI";

    [MenuItem("Tools/Color Game/Generate UI Sprites")]
    public static void Generate()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(projectRoot, OutputFolder));

        CreateRoundedRect(projectRoot, OutputFolder + "/RoundedRect.png");
        CreateGradient(projectRoot, OutputFolder + "/BackgroundGradient.png");

        AssetDatabase.Refresh();
        Debug.Log("Color Game UI sprites generated in " + OutputFolder);
    }

    private static void CreateRoundedRect(string projectRoot, string assetPath)
    {
        const int size = 128;
        const float radius = 40f; // must stay smaller than the 48 px slice border below

        Color32[] pixels = new Color32[size * size];
        float half = size * 0.5f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float px = x + 0.5f - half;
                float py = y + 0.5f - half;

                // Signed distance to a rounded box. Negative = inside.
                float qx = Mathf.Abs(px) - (half - radius);
                float qy = Mathf.Abs(py) - (half - radius);
                float outside = new Vector2(Mathf.Max(qx, 0f), Mathf.Max(qy, 0f)).magnitude;
                float inside = Mathf.Min(Mathf.Max(qx, qy), 0f);
                float distance = outside + inside - radius;

                float alpha = Mathf.Clamp01(0.5f - distance); // 1 px antialiased edge
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
            }
        }

        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply();

        SavePng(texture, projectRoot, assetPath);
        ConfigureSprite(assetPath, new Vector4(48, 48, 48, 48));
    }

    private static void CreateGradient(string projectRoot, string assetPath)
    {
        const int width = 4;
        const int height = 256;

        Color32 top = new Color32(59, 34, 96, 255);     // lighter purple
        Color32 bottom = new Color32(23, 13, 38, 255);  // very dark purple

        Color32[] pixels = new Color32[width * height];
        for (int y = 0; y < height; y++)
        {
            float t = y / (float)(height - 1); // y = 0 is the bottom of the texture
            Color32 color = Color32.Lerp(bottom, top, t);
            for (int x = 0; x < width; x++) pixels[y * width + x] = color;
        }

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.SetPixels32(pixels);
        texture.Apply();

        SavePng(texture, projectRoot, assetPath);
        ConfigureSprite(assetPath, Vector4.zero);
    }

    private static void SavePng(Texture2D texture, string projectRoot, string assetPath)
    {
        byte[] png = texture.EncodeToPNG();
        Object.DestroyImmediate(texture);

        File.WriteAllBytes(Path.Combine(projectRoot, assetPath), png);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }

    private static void ConfigureSprite(string assetPath, Vector4 border)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("Could not configure sprite at " + assetPath);
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spriteBorder = border;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();
    }
}