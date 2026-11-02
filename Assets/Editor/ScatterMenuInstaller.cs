using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.Save;
using Shared.UI;
using ColorGame.History;
using ColorGame.UI;
using ScatterGame.Integration;

/// <summary>
/// Menu: Tools > Scatter Game > Install Scatter Into Main Menu   (open the MainMenu scene first)
///  - makes the SCATTER GAME button on the menu open the ScatterGame scene
///  - adds a Scatter history panel to the menu and connects the hub's SCATTER GAME button to it
///  - adds the panel to the Back (Esc) list, and the Scatter scene to the build list
/// Nothing is deleted and the Color Game history is not touched. Safe to run twice. Place in Assets/Editor/.
/// </summary>
public static class ScatterMenuInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string ScatterScenePath = "Assets/ScatterGame/Scene/ScatterGame.unity";
    private const string ScatterSceneName = "ScatterGame";
    private const string MenuSceneName = "MainMenu";
    private const string ScatterButtonName = "ScatterHistoryButton";

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color Red = new Color32(176, 0, 32, 255);

    [MenuItem("Tools/Scatter Game/Install Scatter Into Main Menu")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[ScatterMenu] Stop Play mode first.");
            return;
        }

        MainMenuController menu = Find<MainMenuController>();
        if (menu == null)
        {
            Debug.LogError("[ScatterMenu] Open the MainMenu scene first. MainMenuController was not found.");
            return;
        }

        bool entryChanged = EnableMenuEntry(menu);
        bool historyChanged = InstallHistory(menu);
        EnsureBuildList();

        if (entryChanged || historyChanged)
        {
            EditorSceneManager.MarkSceneDirty(menu.gameObject.scene);
            Debug.Log("[ScatterMenu] Done. Save the scene (Ctrl+S).");
        }
        else
        {
            Debug.Log("[ScatterMenu] Already installed. Nothing to do.");
        }
    }

    // ---------- Menu entry ----------

    private static bool EnableMenuEntry(MainMenuController menu)
    {
        SerializedObject menuObject = new SerializedObject(menu);
        SerializedProperty games = menuObject.FindProperty("games");
        if (games == null)
        {
            Debug.LogError("[ScatterMenu] MainMenuController has no 'games' list.");
            return false;
        }

        for (int i = 0; i < games.arraySize; i++)
        {
            SerializedProperty entry = games.GetArrayElementAtIndex(i);
            SerializedProperty scene = entry.FindPropertyRelative("sceneName");
            string displayName = entry.FindPropertyRelative("displayName").stringValue ?? string.Empty;

            if (scene.stringValue == ScatterSceneName) return false; // already set

            if (string.IsNullOrWhiteSpace(scene.stringValue) && displayName.ToUpperInvariant().Contains("SCATTER"))
            {
                scene.stringValue = ScatterSceneName;
                menuObject.ApplyModifiedPropertiesWithoutUndo();
                return true;
            }
        }

        Debug.LogWarning("[ScatterMenu] No empty SCATTER entry was found in MainMenuController > Games. " +
                         "Set its Scene Name to '" + ScatterSceneName + "' by hand.");
        return false;
    }

    // ---------- History panel ----------

    private static bool InstallHistory(MainMenuController menu)
    {
        if (Find<ScatterHistoryUI>() != null) return false;

        GameHistoryUI colorHistory = Find<GameHistoryUI>();
        HistoryHubUI hub = Find<HistoryHubUI>();
        Button scatterButton = FindButton(ScatterButtonName);

        if (colorHistory == null || scatterButton == null)
        {
            Debug.LogError("[ScatterMenu] The History hub was not found. Run Tools > Color Game > Install History Hub first " +
                           "(it creates the SCATTER GAME button in the hub).");
            return false;
        }

        SaveManager save = ResolveSaveManager();
        if (save == null)
        {
            Debug.LogError("[ScatterMenu] No SaveManager was found in the MainMenu scene.");
            return false;
        }

        if (Object.FindObjectsByType<SaveManager>(FindObjectsInactive.Include).Length > 1)
        {
            Debug.LogWarning("[ScatterMenu] The MainMenu scene has MORE THAN ONE SaveManager. This works, but each one keeps its own " +
                             "copy of the settings, so the one that saves last can overwrite the other's sound setting. " +
                             "Keep only one and point everything at it.");
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Transform parent = colorHistory.transform.parent;

        // ---- Panel: drawn just above the Color Game history panel (so above the hub, below Settings) ----
        RectTransform panel = MakeRect("ScatterHistoryPanel", parent, Vector2.zero, Vector2.one);
        panel.SetSiblingIndex(colorHistory.transform.GetSiblingIndex() + 1);
        Image panelImage = panel.gameObject.AddComponent<Image>();
        panelImage.color = new Color(0.105f, 0.063f, 0.19f, 1f);
        CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();

        TextMeshProUGUI title = MakeText(panel, "Title", "SCATTER HISTORY", 64f, Gold, true);
        Stretch(title.rectTransform, new Vector2(0.04f, 0.925f), new Vector2(0.70f, 0.99f));
        Button closeButton = MakeButton(panel, "CloseButton", "CLOSE", Red, Color.white, rounded,
                                        new Vector2(0.74f, 0.935f), new Vector2(0.96f, 0.985f));

        RectTransform statsPanel = MakeRect("StatsPanel", panel, new Vector2(0.03f, 0.80f), new Vector2(0.97f, 0.915f));
        Style(statsPanel.gameObject.AddComponent<Image>(), rounded, new Color(0f, 0f, 0f, 0.35f), false);
        TextMeshProUGUI statsText = MakeText(statsPanel, "Text", string.Empty, 38f, Color.white, false);
        Stretch(statsText.rectTransform, new Vector2(0.02f, 0.05f), new Vector2(0.98f, 0.95f));

        RectTransform scrollRoot = MakeRect("Scroll View", panel, new Vector2(0.03f, 0.02f), new Vector2(0.97f, 0.785f));
        scrollRoot.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.25f);
        ScrollRect scrollRect = scrollRoot.gameObject.AddComponent<ScrollRect>();

        RectTransform viewport = MakeRect("Viewport", scrollRoot, Vector2.zero, Vector2.one);
        viewport.gameObject.AddComponent<RectMask2D>();

        RectTransform content = MakeRect("Content", viewport, new Vector2(0f, 1f), Vector2.one);
        content.pivot = new Vector2(0f, 1f);
        VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.spacing = 10f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scrollRect.viewport = viewport;
        scrollRect.content = content;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.scrollSensitivity = 40f;

        TextMeshProUGUI empty = MakeText(panel, "EmptyMessage", "NO ROUNDS PLAYED YET", 44f, new Color32(158, 158, 158, 255), true);
        Stretch(empty.rectTransform, new Vector2(0.05f, 0.35f), new Vector2(0.95f, 0.55f));

        // ---- Managers (always-active objects, NOT on the panel) ----
        GameObject managerObject = new GameObject("ScatterHistoryManager");
        ScatterHistoryManager historyManager = managerObject.AddComponent<ScatterHistoryManager>();
        SetRef(historyManager, "saveManager", save);

        GameObject uiObject = new GameObject("ScatterHistoryUI");
        ScatterHistoryUI historyUi = uiObject.AddComponent<ScatterHistoryUI>();
        SetRef(historyUi, "historyManager", historyManager);
        SetRef(historyUi, "panelGroup", group);
        SetRef(historyUi, "openButton", scatterButton);
        SetRef(historyUi, "closeButton", closeButton);
        SetRef(historyUi, "rowContainer", content);
        SetRef(historyUi, "scrollRect", scrollRect);
        SetRef(historyUi, "emptyMessage", empty.gameObject);
        SetRef(historyUi, "statsText", statsText);
        SetRef(historyUi, "rowSprite", rounded);

        // ---- The hub's SCATTER GAME button is now live ----
        scatterButton.interactable = true;
        TMP_Text label = scatterButton.GetComponentInChildren<TMP_Text>(true);
        if (label != null) label.text = "SCATTER GAME";
        EditorUtility.SetDirty(scatterButton);
        if (label != null) EditorUtility.SetDirty(label);

        // ---- Back (Esc): Color history, Scatter history, then the hub ----
        SerializedObject menuObject = new SerializedObject(menu);
        SerializedProperty back = menuObject.FindProperty("backPanels");
        if (back != null)
        {
            List<Object> panels = new List<Object>();
            for (int i = 0; i < back.arraySize; i++)
            {
                Object existing = back.GetArrayElementAtIndex(i).objectReferenceValue;
                if (existing != null) panels.Add(existing);
            }

            int hubIndex = hub != null ? panels.IndexOf(hub) : -1;
            if (hubIndex >= 0) panels.Insert(hubIndex, historyUi);
            else panels.Add(historyUi);

            back.arraySize = panels.Count;
            for (int i = 0; i < panels.Count; i++) back.GetArrayElementAtIndex(i).objectReferenceValue = panels[i];
            menuObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogWarning("[ScatterMenu] MainMenuController has no 'Back Panels' list. Esc will not close the Scatter history.");
        }

        return true;
    }

    // ---------- Build list ----------

    private static void EnsureBuildList()
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        if (!System.IO.File.Exists(ScatterScenePath))
        {
            Debug.LogWarning("[ScatterMenu] " + ScatterScenePath + " does not exist yet. Run Tools > Scatter Game > Build Scatter Scene first.");
        }
        else if (!scenes.Exists(s => s.path == ScatterScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(ScatterScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log("[ScatterMenu] Added the Scatter scene to the build list.");
        }

        if (!scenes.Exists(s => s.enabled && System.IO.Path.GetFileNameWithoutExtension(s.path) == MenuSceneName))
        {
            Debug.LogWarning("[ScatterMenu] The '" + MenuSceneName + "' scene is not in the build list. Scatter's HOME button needs it " +
                             "(File > Build Profiles > Scene List).");
        }
    }

    // ---------- Helpers ----------

    private static SaveManager ResolveSaveManager()
    {
        // Use the same SaveManager the Color Game history uses, so both histories save together.
        GameHistoryManager colorManager = Find<GameHistoryManager>();
        if (colorManager != null)
        {
            SerializedProperty property = new SerializedObject(colorManager).FindProperty("saveManager");
            SaveManager fromColor = property != null ? property.objectReferenceValue as SaveManager : null;
            if (fromColor != null) return fromColor;
        }
        return Find<SaveManager>();
    }

    private static T Find<T>() where T : Object
    {
        return Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);
    }

    private static Button FindButton(string name)
    {
        foreach (Button button in Object.FindObjectsByType<Button>(FindObjectsInactive.Include))
        {
            if (button.name == name) return button;
        }
        return null;
    }

    private static RectTransform MakeRect(string name, Transform parent, Vector2 min, Vector2 max)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        Stretch(rect, min, max);
        return rect;
    }

    private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void Style(Image image, Sprite sprite, Color color, bool raycast)
    {
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;
        image.raycastTarget = raycast;
    }

    private static TextMeshProUGUI MakeText(Transform parent, string name, string text, float size, Color color, bool bold)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 14f;
        tmp.fontSizeMax = size;
        tmp.raycastTarget = false;
        return tmp;
    }

    private static Button MakeButton(Transform parent, string name, string label, Color color, Color labelColor, Sprite sprite,
                                     Vector2 min, Vector2 max)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        Style(image, sprite, color, true);

        Button button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;

        TextMeshProUGUI text = MakeText(rect, "Label", label, 64f, labelColor, true);
        Stretch(text.rectTransform, new Vector2(0.04f, 0.05f), new Vector2(0.96f, 0.95f));
        return button;
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[ScatterMenu] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}