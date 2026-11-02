using UnityEditor;
using UnityEngine;
using ScatterGame.Core;
using ScatterGame.Runtime;

/// <summary>
/// Menu: Tools > Scatter Game > Create Default Settings Asset
/// Creates Assets/ScatterGame/ScriptableObjects/ScatterGameSettings.asset filled with the tuned prototype values.
/// It never overwrites an existing asset.
/// </summary>
public static class ScatterSettingsCreator
{
    private const string Folder = "Assets/ScatterGame/ScriptableObjects";
    private const string AssetPath = Folder + "/ScatterGameSettings.asset";

    [MenuItem("Tools/Scatter Game/Create Default Settings Asset")]
    public static void Create()
    {
        ScatterGameSettings existing = AssetDatabase.LoadAssetAtPath<ScatterGameSettings>(AssetPath);
        if (existing != null)
        {
            Debug.Log("[ScatterSettings] The asset already exists. Nothing was changed.");
            Selection.activeObject = existing;
            return;
        }

        if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets/ScatterGame", "ScriptableObjects");

        ScatterGameSettings asset = ScriptableObject.CreateInstance<ScatterGameSettings>();
        asset.CopyFrom(TunedSlotConfig.Create(), new[] { 10, 20, 50, 100, 200, 500 });

        AssetDatabase.CreateAsset(asset, AssetPath);
        AssetDatabase.SaveAssets();
        Selection.activeObject = asset;

        Debug.Log("[ScatterSettings] Created " + AssetPath + ". Sprites are empty on purpose: they come with the UI stage.");
    }
}