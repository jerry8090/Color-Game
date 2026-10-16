
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// One-click Android setup and app icon generator.
/// Menu: Tools > Color Game > Apply Android Settings
///       Tools > Color Game > Generate App Icon
/// </summary>
public static class ColorGameAndroidSetup
{
    private const string ScenePath = "Assets/ColorGame/Scenes/ColorGame.unity";
    private const string IconPath = "Assets/ColorGame/UI/AppIcon.png";

    // ---------- Android settings ----------

    [MenuItem("Tools/Color Game/Apply Android Settings")]
    public static void ApplyAndroidSettings()
    {
        PlayerSettings.productName = "Color Game";
        PlayerSettings.bundleVersion = "0.1.0";

        if (PlayerSettings.Android.bundleVersionCode < 1)
            PlayerSettings.Android.bundleVersionCode = 1;

        // Portrait only.
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        // IL2CPP + 64-bit is required by Google Play.
        PlayerSettings.SetScriptingBackend(
            NamedBuildTarget.Android,
            ScriptingImplementation.IL2CPP
        );

        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        // Draw behind the notch. SafeAreaFitter keeps UI out of it.
        PlayerSettings.Android.renderOutsideSafeArea = true;

        // Add the Color Game scene without removing MainMenu
        // or any other scenes already in the build list.
        if (File.Exists(ScenePath))
        {
            List<EditorBuildSettingsScene> scenes =
                new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            if (!scenes.Exists(s => s.path == ScenePath))
            {
                scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }
        else
        {
            Debug.LogError(
                "[AndroidSetup] Scene not found at " + ScenePath +
                ". Save your scene there, then run this again."
            );
        }

        Debug.Log(
            "[AndroidSetup] Done. Package name is: " +
            PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android) +
            " (change it in Project Settings > Player > Other Settings " +
            "if you want a different one)."
        );
    }

    // ---------- App icon ----------

    [MenuItem("Tools/Color Game/Generate App Icon")]
    public static void GenerateIcon()
    {
        const int size = 512;
        const int margin = 48;
        const int gap = 20;
        const float radius = 26f;

        Color32 background = new Color32(42, 27, 61, 255);

        Color32[] palette =
        {
            new Color32(229, 57, 53, 255),   // red
            new Color32(67, 160, 71, 255),   // green
            new Color32(30, 136, 229, 255),  // blue
            new Color32(253, 216, 53, 255),  // yellow
            new Color32(142, 36, 170, 255),  // purple
            new Color32(251, 140, 0, 255)    // orange
        };

        Color32[] pixels = new Color32[size * size];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = background;
        }

        int tile = (size - 2 * margin - 2 * gap) / 3;
        int totalWidth = 3 * tile + 2 * gap;
        int totalHeight = 2 * tile + gap;
        int startX = (size - totalWidth) / 2;
        int startY = (size - totalHeight) / 2;

        for (int row = 0; row < 2; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                int x0 = startX + column * (tile + gap);
                int y0 = startY + row * (tile + gap);

                DrawRoundedRect(
                    pixels,
                    size,
                    x0,
                    y0,
                    tile,
                    tile,
                    radius,
                    palette[row * 3 + column]
                );
            }
        }

        Texture2D texture = new Texture2D(
            size,
            size,
            TextureFormat.RGBA32,
            false
        );

        texture.SetPixels32(pixels);
        texture.Apply();

        byte[] png = texture.EncodeToPNG();
        Object.DestroyImmediate(texture);

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string fullPath = Path.Combine(projectRoot, IconPath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllBytes(fullPath, png);

        AssetDatabase.ImportAsset(
            IconPath,
            ImportAssetOptions.ForceUpdate
        );

        TextureImporter importer =
            AssetImporter.GetAtPath(IconPath) as TextureImporter;

        if (importer != null)
        {
            importer.textureType = TextureImporterType.Default;
            importer.mipmapEnabled = false;
            importer.textureCompression =
                TextureImporterCompression.Uncompressed;

            importer.SaveAndReimport();
        }

        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);

        if (icon == null)
        {
            Debug.LogError(
                "[AndroidSetup] The icon file was created but could not be loaded: " +
                IconPath
            );
            return;
        }

        // NamedBuildTarget.Unknown = the Default Icon slot.
        PlayerSettings.SetIcons(
            NamedBuildTarget.Unknown,
            new[] { icon },
            IconKind.Application
        );

        Debug.Log(
            "[AndroidSetup] App icon created at " +
            IconPath +
            " and set as the default icon."
        );
    }

    // Draws an anti-aliased rounded rectangle into the pixel buffer.
    private static void DrawRoundedRect(
        Color32[] pixels,
        int size,
        int x0,
        int y0,
        int width,
        int height,
        float radius,
        Color32 color
    )
    {
        float halfW = width * 0.5f;
        float halfH = height * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int px = x0 + x;
                int py = y0 + y;

                if (px < 0 || py < 0 || px >= size || py >= size)
                    continue;

                float localX = x + 0.5f - halfW;
                float localY = y + 0.5f - halfH;

                // Signed distance to a rounded box. Negative = inside.
                float qx = Mathf.Abs(localX) - (halfW - radius);
                float qy = Mathf.Abs(localY) - (halfH - radius);

                float outside = new Vector2(
                    Mathf.Max(qx, 0f),
                    Mathf.Max(qy, 0f)
                ).magnitude;

                float inside = Mathf.Min(Mathf.Max(qx, qy), 0f);
                float distance = outside + inside - radius;

                float alpha = Mathf.Clamp01(0.5f - distance);

                if (alpha <= 0f)
                    continue;

                int index = py * size + px;

                pixels[index] = Color32.Lerp(
                    pixels[index],
                    color,
                    alpha
                );
            }
        }
    }
}
