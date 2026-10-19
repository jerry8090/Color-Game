using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor-only helpers. Menu: Tools > Color Game
/// </summary>
public static class ColorGameSaveTools
{
    [MenuItem("Tools/Color Game/Open Save Folder")]
    public static void OpenSaveFolder()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("Tools/Color Game/Delete Save Files")]
    public static void DeleteSaveFiles()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("Stop Play mode before deleting save files.");
            return;
        }

        string basePath = Path.Combine(Application.persistentDataPath, "colorgame_save.json");
        string[] files = { basePath, basePath + ".bak", basePath + ".tmp", basePath + ".corrupt" };

        foreach (string file in files)
        {
            if (File.Exists(file)) File.Delete(file);
        }

        Debug.Log("Color Game save files deleted.");
    }
}