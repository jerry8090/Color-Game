using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;
using Shared.Save;
using Shared.UI;
using ColorGame.DevTools;
using ColorGame.Gameplay;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Install Play Screen Menu   (open the ColorGame scene first)
/// Adds a MENU button to the play screen and moves HISTORY, SOUND and DEV TEST into its panel (plus MAIN MENU).
/// Nothing is deleted and every existing reference keeps working, because the buttons are only re-parented.
/// Safe to run twice. Place in Assets/Editor/.
/// </summary>
public static class ColorGamePlayMenuInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";

    [MenuItem("Tools/Color Game/Install Play Screen Menu")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[PlayMenu] Stop Play mode first.");
            return;
        }

        if (Object.FindFirstObjectByType<PlayMenuUI>(FindObjectsInactive.Include) != null)
        {
            Debug.Log("[PlayMenu] Already installed. Nothing to do.");
            return;
        }

        ColorGameManager gameManager = Find<ColorGameManager>();
        SaveManager saveManager = Find<SaveManager>();
        GameHistoryUI history = Find<GameHistoryUI>();
        DebugPanelUI debug = Find<DebugPanelUI>();
        AudioToggleUI sound = Find<AudioToggleUI>();
        AudioManager audio = Find<AudioManager>();
        SafeAreaFitter safe = Find<SafeAreaFitter>();

        if (gameManager == null || saveManager == null || history == null || debug == null || sound == null || safe == null)
        {
            Debug.LogError("[PlayMenu] Open the ColorGame scene first. Missing: " +
                           (gameManager == null ? "ColorGameManager " : "") + (saveManager == null ? "SaveManager " : "") +
                           (history == null ? "GameHistoryUI " : "") + (debug == null ? "DebugPanelUI " : "") +
                           (sound == null ? "AudioToggleUI " : "") + (safe == null ? "SafeAreaFitter" : ""));
            return;
        }

        Button historyButton = GetButton(history, "openButton");
        Button debugButton = GetButton(debug, "openButton");
        Button soundButton = sound.GetComponent<Button>();
        if (historyButton == null || debugButton == null || soundButton == null)
        {
            Debug.LogError("[PlayMenu] Could not find the HISTORY, DEV TEST or SOUND button.");
            return;
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Transform parent = safe.transform;

        // The MENU button takes the place of the SOUND button (top right), copying its rectangle.
        RectTransform soundRect = (RectTransform)soundButton.transform;
        Transform soundOldParent = soundRect.parent;
        Button menuButton = MakeButton(soundOldParent, "MenuButton", "MENU", new Color(0f, 0f, 0f, 0.4f), rounded);
        RectTransform menuRect = (RectTransform)menuButton.transform;
        menuRect.anchorMin = soundRect.anchorMin;
        menuRect.anchorMax = soundRect.anchorMax;
        menuRect.offsetMin = soundRect.offsetMin;
        menuRect.offsetMax = soundRect.offsetMax;
        menuRect.SetSiblingIndex(soundRect.GetSiblingIndex());

        // Full-screen dim backdrop (a button, so tapping outside the card closes the menu).
        GameObject panel = new GameObject("PlayMenuPanel", typeof(RectTransform), typeof(Image), typeof(Button));
        panel.transform.SetParent(parent, false);
        Stretch((RectTransform)panel.transform, Vector2.zero, Vector2.one);
        Image dim = panel.GetComponent<Image>();
        dim.color = new Color(0f, 0f, 0f, 0.65f);
        Button backdrop = panel.GetComponent<Button>();
        backdrop.targetGraphic = dim;
        SetNoNavigation(backdrop);
        if (history.transform.parent == parent) panel.transform.SetSiblingIndex(history.transform.GetSiblingIndex());

        // The card with the four buttons.
        GameObject card = new GameObject("MenuCard", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup));
        card.transform.SetParent(panel.transform, false);
        Stretch((RectTransform)card.transform, new Vector2(0.1f, 0.30f), new Vector2(0.9f, 0.70f));
        Image cardImage = card.GetComponent<Image>();
        cardImage.sprite = rounded;
        cardImage.type = rounded != null ? Image.Type.Sliced : Image.Type.Simple;
        cardImage.color = new Color32(42, 27, 61, 255);
        VerticalLayoutGroup layout = card.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(30, 30, 30, 30);
        layout.spacing = 20f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;

        // Re-parent the existing buttons (references and click handlers keep working).
        historyButton.transform.SetParent(card.transform, false);
        soundButton.transform.SetParent(card.transform, false);
        debugButton.transform.SetParent(card.transform, false);
        Button mainMenuButton = MakeButton(card.transform, "MainMenuButton", "MAIN MENU", new Color32(57, 73, 171, 255), rounded);

        foreach (Button b in new[] { historyButton, soundButton, debugButton, mainMenuButton })
        {
            TMP_Text label = b.GetComponentInChildren<TMP_Text>(true);
            if (label == null) continue;
            label.enableAutoSizing = true;
            label.fontSizeMin = 24f;
            label.fontSizeMax = 44f;
        }

        // The controller lives on an always-active object.
        GameObject controllerGo = new GameObject("PlayMenuController");
        controllerGo.transform.SetParent(parent, false);
        PlayMenuUI playMenu = controllerGo.AddComponent<PlayMenuUI>();
        SerializedObject so = new SerializedObject(playMenu);
        so.FindProperty("gameManager").objectReferenceValue = gameManager;
        so.FindProperty("saveManager").objectReferenceValue = saveManager;
        so.FindProperty("historyUI").objectReferenceValue = history;
        so.FindProperty("debugPanel").objectReferenceValue = debug;
        so.FindProperty("menuButton").objectReferenceValue = menuButton;
        so.FindProperty("menuPanel").objectReferenceValue = panel;
        so.FindProperty("backdropButton").objectReferenceValue = backdrop;
        so.FindProperty("mainMenuButton").objectReferenceValue = mainMenuButton;
        so.FindProperty("mainMenuSceneName").stringValue = "MainMenu";
        if (audio != null) so.FindProperty("audioManager").objectReferenceValue = audio;
        so.ApplyModifiedPropertiesWithoutUndo();

        panel.SetActive(false);
        EditorSceneManager.MarkSceneDirty(controllerGo.scene);
        Debug.Log("[PlayMenu] Installed. HISTORY, SOUND and DEV TEST are now inside the MENU panel. Save the scene (Ctrl+S).");
    }

    private static T Find<T>() where T : Object
    {
        return Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);
    }

    private static Button GetButton(Object owner, string field)
    {
        SerializedProperty property = new SerializedObject(owner).FindProperty(field);
        return property != null ? property.objectReferenceValue as Button : null;
    }

    private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void SetNoNavigation(Button button)
    {
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;
    }

    private static Button MakeButton(Transform parent, string name, string label, Color color, Sprite sprite)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        Image image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;
        SetNoNavigation(button);

        GameObject textGo = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        textGo.transform.SetParent(go.transform, false);
        Stretch((RectTransform)textGo.transform, Vector2.zero, Vector2.one);
        TextMeshProUGUI text = textGo.GetComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 40f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.raycastTarget = false;
        return button;
    }
}
