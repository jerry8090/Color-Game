using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;
using Shared.UI;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Install History Hub   (open the MainMenu scene first)
/// Adds a HISTORY chooser (COLOR GAME / SCATTER GAME - coming soon) in front of the existing Color Game history panel:
///  - the existing HISTORY button on the menu now opens the chooser
///  - the COLOR GAME button in the chooser opens the Color Game history panel
///  - Back (Esc) closes the history panel first, then the chooser
/// Nothing is deleted. Safe to run twice. Place in Assets/Editor/.
/// </summary>
public static class ColorGameHistoryHubInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";

    [MenuItem("Tools/Color Game/Install History Hub")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[HistoryHub] Stop Play mode first.");
            return;
        }

        if (Object.FindFirstObjectByType<HistoryHubUI>(FindObjectsInactive.Include) != null)
        {
            Debug.Log("[HistoryHub] Already installed. Nothing to do.");
            return;
        }

        GameHistoryUI history = Object.FindFirstObjectByType<GameHistoryUI>(FindObjectsInactive.Include);
        MainMenuController menu = Object.FindFirstObjectByType<MainMenuController>(FindObjectsInactive.Include);
        if (history == null || menu == null)
        {
            Debug.LogError("[HistoryHub] Open the MainMenu scene first. It needs the History panel (GameHistoryUI) and MainMenuController.");
            return;
        }

        SerializedObject historyObject = new SerializedObject(history);
        SerializedProperty openProperty = historyObject.FindProperty("openButton");
        Button menuHistoryButton = openProperty != null ? openProperty.objectReferenceValue as Button : null;
        if (menuHistoryButton == null)
        {
            Debug.LogError("[HistoryHub] The History panel has no Open Button assigned. Assign your menu HISTORY button to it first.");
            return;
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Transform parent = history.transform.parent;

        // ---- Chooser panel (drawn just below the history panel, above the menu) ----
        GameObject hub = new GameObject("HistoryHubPanel", typeof(RectTransform), typeof(Image));
        hub.transform.SetParent(parent, false);
        Stretch((RectTransform)hub.transform, Vector2.zero, Vector2.one);
        hub.GetComponent<Image>().color = new Color(0.06f, 0.04f, 0.10f, 0.97f);
        hub.transform.SetSiblingIndex(history.transform.GetSiblingIndex());

        TextMeshProUGUI title = MakeText(hub.transform, "Title", "HISTORY", 90f, new Color32(255, 213, 74, 255), true);
        Stretch(title.rectTransform, new Vector2(0.05f, 0.78f), new Vector2(0.95f, 0.90f));

        TextMeshProUGUI note = MakeText(hub.transform, "Note", "Choose a game to see its rounds.", 34f, new Color32(208, 200, 224, 255), false);
        Stretch(note.rectTransform, new Vector2(0.1f, 0.70f), new Vector2(0.9f, 0.77f));

        Button colorButton = MakeButton(hub.transform, "ColorGameHistoryButton", "COLOR GAME", new Color32(255, 179, 0, 255),
                                        new Color32(42, 27, 61, 255), rounded, new Vector2(0.1f, 0.55f), new Vector2(0.9f, 0.67f));

        Button scatterButton = MakeButton(hub.transform, "ScatterHistoryButton",
                                          "SCATTER GAME\n<size=45%>COMING SOON</size>", new Color32(142, 36, 170, 255),
                                          Color.white, rounded, new Vector2(0.1f, 0.40f), new Vector2(0.9f, 0.52f));
        scatterButton.interactable = false; // enable it when a Scatter history screen exists

        Button backButton = MakeButton(hub.transform, "BackButton", "BACK", new Color32(176, 0, 32, 255),
                                       Color.white, rounded, new Vector2(0.25f, 0.15f), new Vector2(0.75f, 0.24f));

        // ---- Controller ----
        GameObject controllerGo = new GameObject("HistoryHub");
        HistoryHubUI hubUi = controllerGo.AddComponent<HistoryHubUI>();

        SerializedObject hubObject = new SerializedObject(hubUi);
        hubObject.FindProperty("openButton").objectReferenceValue = menuHistoryButton;
        hubObject.FindProperty("hubPanel").objectReferenceValue = hub;
        hubObject.FindProperty("backButton").objectReferenceValue = backButton;
        AudioManager audio = Object.FindFirstObjectByType<AudioManager>(FindObjectsInactive.Include);
        if (audio != null) hubObject.FindProperty("audioManager").objectReferenceValue = audio;
        hubObject.ApplyModifiedPropertiesWithoutUndo();

        // The Color Game history panel is now opened by the chooser's COLOR GAME button.
        openProperty.objectReferenceValue = colorButton;
        historyObject.ApplyModifiedPropertiesWithoutUndo();

        // Back closes the history panel first, then the chooser.
        SerializedObject menuObject = new SerializedObject(menu);
        SerializedProperty back = menuObject.FindProperty("backPanels");
        if (back != null)
        {
            back.arraySize = 2;
            back.GetArrayElementAtIndex(0).objectReferenceValue = history;
            back.GetArrayElementAtIndex(1).objectReferenceValue = hubUi;
            menuObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogWarning("[HistoryHub] MainMenuController has no 'Back Panels' list. Install the last MainMenuController update.");
        }

        hub.SetActive(false);
        EditorSceneManager.MarkSceneDirty(controllerGo.scene);
        Debug.Log("[HistoryHub] Installed. HISTORY now opens the chooser. Save the scene (Ctrl+S).");
    }

    private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
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

    private static Button MakeButton(Transform parent, string name, string label, Color color, Color labelColor,
                                     Sprite sprite, Vector2 min, Vector2 max)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        Stretch((RectTransform)go.transform, min, max);

        Image image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;

        TextMeshProUGUI text = MakeText(go.transform, "Label", label, 64f, labelColor, true);
        Stretch(text.rectTransform, new Vector2(0.04f, 0.05f), new Vector2(0.96f, 0.95f));
        return button;
    }
}
