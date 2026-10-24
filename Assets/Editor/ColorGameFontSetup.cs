using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using Shared.UI;

/// <summary>
/// Menu: Tools > Color Game > Design > 2 - Create Font + Typography
/// Needs a .ttf in Assets/ColorGame/Fonts. Builds a dynamic TMP font asset, six shared material presets
/// (outline and shadow per text role) and the GameTypography asset.
/// </summary>
public static class ColorGameFontSetup
{
    private const string FontFolder = "Assets/ColorGame/Fonts";
    private const string TypoFolder = "Assets/ColorGame/UI/Typography";
    private const string TypoPath = TypoFolder + "/GameTypography.asset";

    [MenuItem("Tools/Color Game/Design/2 - Create Font + Typography")]
    public static void Run()
    {
        string[] guids = AssetDatabase.FindAssets("t:Font", new[] { FontFolder });
        if (guids.Length == 0)
        {
            Debug.LogError("[Font] Put a .ttf file in " + FontFolder + " first (see the steps).");
            return;
        }

        Directory.CreateDirectory(Path.Combine(Directory.GetParent(Application.dataPath).FullName, TypoFolder));
        ShaderUtilities.GetShaderPropertyIDs();

        Font font = AssetDatabase.LoadAssetAtPath<Font>(AssetDatabase.GUIDToAssetPath(guids[0]));
        string fontAssetPath = TypoFolder + "/GameFont SDF.asset";
        TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontAssetPath);

        if (fontAsset == null)
        {
            fontAsset = TMP_FontAsset.CreateFontAsset(font); // dynamic atlas, filled as characters are needed
            fontAsset.name = "GameFont SDF";
            AssetDatabase.CreateAsset(fontAsset, fontAssetPath);
            fontAsset.material.name = "GameFont SDF Material";
            AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
            foreach (Texture2D atlas in fontAsset.atlasTextures)
            {
                atlas.name = "GameFont SDF Atlas";
                AssetDatabase.AddObjectToAsset(atlas, fontAsset);
            }
            fontAsset.TryAddCharacters("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 .,:;!?+-/%()|$\u20B1", out string missing);
            if (!string.IsNullOrEmpty(missing)) Debug.LogWarning("[Font] This font has no glyphs for: " + missing);
            EditorUtility.SetDirty(fontAsset);
        }

        Color outline = GameTheme.TextOutline;
        Color shadow = new Color(0f, 0f, 0f, 0.55f);

        Material title = Preset(fontAsset, "Title", 0.24f, outline, true, shadow, -0.7f, 0.25f);
        Material number = Preset(fontAsset, "Number", 0.20f, outline, true, shadow, -0.6f, 0.2f);
        Material button = Preset(fontAsset, "Button", 0.14f, outline, true, new Color(0f, 0f, 0f, 0.35f), -0.5f, 0.2f);
        Material label = Preset(fontAsset, "Label", 0.10f, outline, true, new Color(0f, 0f, 0f, 0.35f), -0.4f, 0.3f);
        Material body = Preset(fontAsset, "Body", 0.05f, outline, false, shadow, 0f, 0f);
        Material bill = Preset(fontAsset, "Bill", 0.22f, new Color32(20, 10, 40, 255), false, shadow, 0f, 0f);

        GameTypography typography = AssetDatabase.LoadAssetAtPath<GameTypography>(TypoPath);
        if (typography == null)
        {
            typography = ScriptableObject.CreateInstance<GameTypography>();
            AssetDatabase.CreateAsset(typography, TypoPath);
        }

        SerializedObject so = new SerializedObject(typography);
        so.FindProperty("font").objectReferenceValue = fontAsset;
        so.FindProperty("titleMaterial").objectReferenceValue = title;
        so.FindProperty("numberMaterial").objectReferenceValue = number;
        so.FindProperty("buttonMaterial").objectReferenceValue = button;
        so.FindProperty("labelMaterial").objectReferenceValue = label;
        so.FindProperty("bodyMaterial").objectReferenceValue = body;
        so.FindProperty("billMaterial").objectReferenceValue = bill;
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorUtility.SetDirty(typography);
        AssetDatabase.SaveAssets();
        Debug.Log("[Font] Typography ready. Peso sign in this font: " + fontAsset.HasCharacter('\u20B1', true));
    }

    private static Material Preset(TMP_FontAsset fontAsset, string name, float outlineWidth, Color outlineColor,
                                   bool underlay, Color underlayColor, float offsetY, float softness)
    {
        string path = TypoFolder + "/GameFont_" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            material = new Material(fontAsset.material);
            AssetDatabase.CreateAsset(material, path);
        }

        material.SetFloat(ShaderUtilities.ID_OutlineWidth, outlineWidth);
        material.SetColor(ShaderUtilities.ID_OutlineColor, outlineColor);

        if (underlay)
        {
            material.EnableKeyword(ShaderUtilities.Keyword_Underlay);
            material.SetColor(ShaderUtilities.ID_UnderlayColor, underlayColor);
            material.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, 0f);
            material.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, offsetY);
            material.SetFloat(ShaderUtilities.ID_UnderlaySoftness, softness);
            material.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0f);
        }
        else
        {
            material.DisableKeyword(ShaderUtilities.Keyword_Underlay);
        }

        EditorUtility.SetDirty(material);
        return material;
    }
}