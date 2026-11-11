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
using RouletteGame.Data;
using RouletteGame.Integration;
using RouletteGame.UI;

/// <summary>
/// Menu: Tools > Roulette Game > Install Roulette History Into Main Menu   (open the MainMenu scene first)
///  - adds a ROULETTE button to the HISTORY chooser
///  - adds a Roulette history panel and connects that button to it
///  - adds the panel to the Back (Esc) list
/// It does NOT touch the game buttons of the menu, the Color Game history or the Scatter history.
/// Safe to run twice. Run Tools > Color Game > Install History Hub first if the HISTORY chooser does not exist yet.
/// </summary>
public static class RouletteMenuInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string SettingsPath = "Assets/RouletteGame/ScriptableObjects/RouletteSettings.asset";
    private const string RouletteButtonName = "RouletteHistoryButton";

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color Red = new Color32(176, 0, 32, 255);

    [MenuItem("Tools/Roulette Game/Install Roulette History Into Main Menu")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[RouletteMenu] Stop Play mode first.");
            return;
        }

        MainMenuController menu = Find<MainMenuController>();
        if (menu == null)
        {
            Debug.LogError("[RouletteMenu] Open the MainMenu scene first. MainMenuController was not found.");
            return;
        }

        if (Find<RouletteHistoryUI>() != null)
        {
            Debug.Log("[RouletteMenu] The Roulette history is already installed. Nothing to do.");
            return;
        }

        GameHistoryUI colorHistory = Find<GameHistoryUI>();
        HistoryHubUI hub = Find<HistoryHubUI>();
        if (colorHistory == null || hub == null)
        {
            Debug.LogError("[RouletteMenu] The History hub was not found. Run Tools > Color Game > Install History Hub first.");
            return;
        }

        SaveManager save = ResolveSaveManager();
        if (save == null)
        {
            Debug.LogError("[RouletteMenu] No SaveManager was found in the MainMenu scene.");
            return;
        }

        SerializedProperty hubPanelProperty = new SerializedObject(hub).FindProperty("hubPanel");
        GameObject hubPanel = hubPanelProperty != null ? hubPanelProperty.objectReferenceValue as GameObject : null;
        if (hubPanel == null)
        {
            Debug.LogError("[RouletteMenu] The History hub has no Hub Panel assigned.");
            return;
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Transform parent = colorHistory.transform.parent;

        // ---- The ROULETTE button in the chooser (below SCATTER GAME, above BACK) ----
        Button rouletteButton = MakeButton(hubPanel.transform, RouletteButtonName, "ROULETTE", new Color32(0, 121, 107, 255), Color.white, rounded,
                                           new Vector2(0.1f, 0.26f), new Vector2(0.9f, 0.38f));

        // ---- Panel: drawn just above the Scatter history panel when there is one, so above the hub and below Settings ----
        ScatterHistoryUI scatterHistory = Find<ScatterHistoryUI>();
        int siblingIndex = (scatterHistory != null && scatterHistory.transform.parent == parent)
            ? FindPanelIndex(parent, "ScatterHistoryPanel", colorHistory.transform.GetSiblingIndex()) + 1
            : colorHistory.transform.GetSiblingIndex() + 1;

        RectTransform panel = MakeRect("RouletteHistoryPanel", parent, Vector2.zero, Vector2.one);
        panel.SetSiblingIndex(siblingIndex);
        Image panelImage = panel.gameObject.AddComponent<Image>();
        panelImage.color = new Color(0.105f, 0.063f, 0.19f, 1f);
        CanvasGroup group = panel.gameObject.AddComponent<CanvasGroup>();

        TextMeshProUGUI title = MakeText(panel, "Title", "ROULETTE HISTORY", 64f, Gold, true);
        Stretch(title.rectTransform, new Vector2(0.04f, 0.925f), new Vector2(0.70f, 0.99f));
        Button closeButton = MakeButton(panel, "CloseButton", "CLOSE", Red, Color.white, rounded, new Vector2(0.74f, 0.935f), new Vector2(0.96f, 0.985f));

        RectTransform statsPanel = MakeRect("StatsPanel", panel, new Vector2(0.03f, 0.80f), new Vector2(0.97f, 0.915f));
        Style(statsPanel.gameObject.AddComponent<Image>(), rounded, new Color(0f, 0f, 0f, 0.35f));
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
        GameObject managerObject = new GameObject("RouletteHistoryManager");
        RouletteHistoryManager historyManager = managerObject.AddComponent<RouletteHistoryManager>();
        SetRef(historyManager, "saveManager", save);
        RouletteSettings settings = AssetDatabase.LoadAssetAtPath<RouletteSettings>(SettingsPath);
        if (settings != null) SetRef(historyManager, "settings", settings); // optional: the history limit comes from it

        GameObject uiObject = new GameObject("RouletteHistoryUI");
        RouletteHistoryUI historyUi = uiObject.AddComponent<RouletteHistoryUI>();
        SetRef(historyUi, "historyManager", historyManager);
        SetRef(historyUi, "panelGroup", group);
        SetRef(historyUi, "openButton", rouletteButton);
        SetRef(historyUi, "closeButton", closeButton);
        SetRef(historyUi, "rowContainer", content);
        SetRef(historyUi, "scrollRect", scrollRect);
        SetRef(historyUi, "emptyMessage", empty.gameObject);
        SetRef(historyUi, "statsText", statsText);
        SetRef(historyUi, "rowSprite", rounded);

        // ---- Back (Esc): the panel closes before the chooser ----
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

            int hubIndex = panels.IndexOf(hub);
            if (hubIndex >= 0) panels.Insert(hubIndex, historyUi);
            else panels.Add(historyUi);

            back.arraySize = panels.Count;
            for (int i = 0; i < panels.Count; i++) back.GetArrayElementAtIndex(i).objectReferenceValue = panels[i];
            menuObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogWarning("[RouletteMenu] MainMenuController has no 'Back Panels' list. Esc will not close the Roulette history.");
        }

        EditorSceneManager.MarkSceneDirty(menu.gameObject.scene);
        Debug.Log("[RouletteMenu] Done. HISTORY now has a ROULETTE button. Save the scene (Ctrl+S).");
    }

    // ---------- Helpers ----------

    private static int FindPanelIndex(Transform parent, string name, int fallback)
    {
        Transform found = parent.Find(name);
        return found != null ? found.GetSiblingIndex() : fallback;
    }

    private static SaveManager ResolveSaveManager()
    {
        // Use the same SaveManager the other histories use, so all histories save together.
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

    private static void Style(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;
        image.raycastTarget = false;
    }

    private static TextMeshProUGUI MakeText(Transform parent, string name, string text, float size, Color color, bool bold)
    {
        return RouletteUi.MakeText(parent, name, text, size, color, null, bold);
    }

    private static Button MakeButton(Transform parent, string name, string label, Color color, Color labelColor, Sprite sprite, Vector2 min, Vector2 max)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;

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
            Debug.LogError("[RouletteMenu] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
