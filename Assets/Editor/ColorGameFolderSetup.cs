using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// One-click helper that creates the project folder structure.
/// Run it from the menu: Tools > Color Game > Create Folder Structure.
/// </summary>
public static class ColorGameFolderSetup
{
    private static readonly string[] Folders =
    {
        "Shared/Scripts/Core",
        "Shared/Scripts/Wallet",
        "Shared/Scripts/Save",
        "Shared/Scripts/Audio",
        "ColorGame/Scripts/Core",
        "ColorGame/Scripts/Gameplay",
        "ColorGame/Scripts/History",
        "ColorGame/Scripts/UI",
        "ColorGame/Scripts/Debug",
        "ColorGame/Prefabs",
        "ColorGame/Scenes",
        "ColorGame/ScriptableObjects",
        "ColorGame/Sprites",
        "ColorGame/Audio/SFX",
        "ColorGame/Audio/Music",
        "ColorGame/Materials",
        "ColorGame/UI"
    };

    [MenuItem("Tools/Color Game/Create Folder Structure")]
    public static void CreateFolders()
    {
        foreach (string folder in Folders)
        {
            string fullPath = Path.Combine(Application.dataPath, folder);
            Directory.CreateDirectory(fullPath); // Does nothing if it already exists
        }

        AssetDatabase.Refresh();
        Debug.Log("Color Game folder structure created.");
    }
}