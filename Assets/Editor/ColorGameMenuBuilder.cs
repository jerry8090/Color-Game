using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Shared.Audio;
using Shared.Save;
using Shared.UI;
using Shared.Wallet;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Create Main Menu Scene
/// Builds Assets/Scenes/MainMenu.unity (purple theme, responsive, with its own wallet/save/audio managers
/// that read the SAME save file), and puts it first in the build list. Place in Assets/Editor/.
/// </summary>
public static class ColorGameMenuBuilder
{
    private const string MenuScenePath = "Assets/Scenes/MainMenu.unity";
    private const string GameScenePath = "Assets/ColorGame/Scenes/ColorGame.unity";
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string GradientPath = "Assets/ColorGame/UI/BackgroundGradient.png";
    private const string ClickPath = "Assets/ColorGame/Audio/SFX/sfx_button_click.wav";

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color Dark = new Color32(42, 27, 61, 255);

    [MenuItem("Tools/Color Game/Create Main Menu Scene")]
    public static void CreateMainMenuScene()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[MenuBuilder] Stop Play mode first.");
            return;
        }

        if (File.Exists(MenuScenePath) &&
            !EditorUtility.DisplayDialog("Main Menu exists", "Assets/Scenes/MainMenu.unity already exists. Replace it?", "Replace", "Cancel"))
        {
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        Directory.CreateDirectory(Path.GetDirectoryName(MenuScenePath));
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Sprite gradient = AssetDatabase.LoadAssetAtPath<Sprite>(GradientPath);
        if (rounded == null || gradient == null)
        {
            Debug.LogWarning("[MenuBuilder] UI sprites missing. Run Tools > Color Game > Generate UI Sprites, then run this again for the full look.");
        }

        // ---- Camera (needed for the audio listener and a solid background) ----
        GameObject cam = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cam.tag = "MainCamera";
        Camera camera = cam.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(23, 13, 38, 255);

        // ---- Managers (same code and same save file as the Color Game scene) ----
        GameObject managers = new GameObject("----Managers----");
        GameObject walletGo = new GameObject("WalletManager");
        walletGo.transform.SetParent(managers.transform);
        VirtualWalletManager wallet = walletGo.AddComponent<VirtualWalletManager>();

        GameObject saveGo = new GameObject("SaveManager");
        saveGo.transform.SetParent(managers.transform);
        SaveManager save = saveGo.AddComponent<SaveManager>();
        SetRef(save, "walletManager", wallet);

        GameObject audioGo = new GameObject("AudioManager");
        audioGo.transform.SetParent(managers.transform);
        AudioManager audio = audioGo.AddComponent<AudioManager>();
        AudioSource sfx = MakeAudioSource(audioGo.transform, "SFXSource", false);
        AudioSource music = MakeAudioSource(audioGo.transform, "MusicSource", true);
        SetRef(audio, "saveManager", save);
        SetRef(audio, "sfxSource", sfx);
        SetRef(audio, "musicSource", music);

        AudioClip click = AssetDatabase.LoadAssetAtPath<AudioClip>(ClickPath);
        SerializedObject audioObject = new SerializedObject(audio);
        SerializedProperty entries = audioObject.FindProperty("sfxEntries");
        if (click != null && entries != null && entries.arraySize > 0)
        {
            entries.GetArrayElementAtIndex(0).FindPropertyRelative("clip").objectReferenceValue = click;
            audioObject.ApplyModifiedPropertiesWithoutUndo();
        }

        // ---- Event system ----
        GameObject eventSystem = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
#if ENABLE_INPUT_SYSTEM
        eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif

        // ---- Canvas ----
        GameObject canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(ResponsiveCanvasScaler));
        canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Background fills the whole screen (also behind notches).
        RectTransform background = MakeRect("Background", canvasGo.transform, Vector2.zero, Vector2.one);
        Image backgroundImage = background.gameObject.AddComponent<Image>();
        backgroundImage.sprite = gradient;
        backgroundImage.color = gradient != null ? Color.white : Dark;
        backgroundImage.raycastTarget = false;

        RectTransform safe = MakeRect("SafeAreaPanel", canvasGo.transform, Vector2.zero, Vector2.one);
        safe.gameObject.AddComponent<SafeAreaFitter>();

        // ---- Balance (shows the shared wallet) ----
        RectTransform balancePanel = MakeRect("BalancePanel", safe, new Vector2(0.2f, 0.915f), new Vector2(0.8f, 0.985f));
        Image balanceImage = balancePanel.gameObject.AddComponent<Image>();
        Style(balanceImage, rounded, new Color(0f, 0f, 0f, 0.4f), false);
        TextMeshProUGUI balanceLabel = MakeText(balancePanel, "BalanceLabel", "VIRTUAL CREDITS", 28, Color.white, false);
        Stretch(balanceLabel.rectTransform, new Vector2(0f, 0.55f), Vector2.one);
        TextMeshProUGUI balanceValue = MakeText(balancePanel, "BalanceValue", "0", 56, Gold, true);
        Stretch(balanceValue.rectTransform, Vector2.zero, new Vector2(1f, 0.6f));
        WalletUI walletUi = balancePanel.gameObject.AddComponent<WalletUI>();
        SetRef(walletUi, "walletManager", wallet);
        SetRef(walletUi, "balanceText", balanceValue);

        // ---- Title ----
        TextMeshProUGUI title = MakeText(safe, "Title", "GAME ROOM", 120, Gold, true);
        Stretch(title.rectTransform, new Vector2(0.05f, 0.73f), new Vector2(0.95f, 0.88f));
        TextMeshProUGUI subtitle = MakeText(safe, "Subtitle", "VIRTUAL CREDITS ONLY  -  NO REAL MONEY", 32, new Color32(208, 200, 224, 255), false);
        Stretch(subtitle.rectTransform, new Vector2(0.05f, 0.68f), new Vector2(0.95f, 0.73f));

        // ---- Buttons ----
        Button colorButton = MakeButton(safe, "ColorGameButton", "COLOR GAME", new Color32(255, 179, 0, 255), Dark, rounded,
                                        new Vector2(0.1f, 0.50f), new Vector2(0.9f, 0.63f), out TextMeshProUGUI colorLabel);
        Button scatterButton = MakeButton(safe, "ScatterButton", "SCATTER GAME", new Color32(142, 36, 170, 255), Color.white, rounded,
                                          new Vector2(0.1f, 0.35f), new Vector2(0.9f, 0.48f), out TextMeshProUGUI scatterLabel);
        Button settingsButton = MakeButton(safe, "SettingsButton", "SETTINGS", new Color32(94, 53, 177, 255), Color.white, rounded,
                                           new Vector2(0.1f, 0.22f), new Vector2(0.9f, 0.32f), out _);
        Button quitButton = MakeButton(safe, "QuitButton", "QUIT", new Color32(176, 0, 32, 255), Color.white, rounded,
                                       new Vector2(0.25f, 0.11f), new Vector2(0.75f, 0.19f), out _);

        // ---- Settings panel ----
        RectTransform settingsRect = MakeRect("SettingsPanel", safe, Vector2.zero, Vector2.one);
        Image settingsImage = settingsRect.gameObject.AddComponent<Image>();
        settingsImage.color = new Color(0.06f, 0.04f, 0.10f, 0.97f);

        TextMeshProUGUI settingsTitle = MakeText(settingsRect, "SettingsTitle", "SETTINGS", 90, Gold, true);
        Stretch(settingsTitle.rectTransform, new Vector2(0.05f, 0.78f), new Vector2(0.95f, 0.90f));

        Button soundButton = MakeButton(settingsRect, "SoundButton", "SOUND: ON", new Color32(94, 53, 177, 255), Color.white, rounded,
                                        new Vector2(0.15f, 0.55f), new Vector2(0.85f, 0.65f), out TextMeshProUGUI soundLabel);
        AudioToggleUI toggle = soundButton.gameObject.AddComponent<AudioToggleUI>();
        SetRef(toggle, "audioManager", audio);
        SetRef(toggle, "button", soundButton);
        SetRef(toggle, "label", soundLabel);

        TextMeshProUGUI note = MakeText(settingsRect, "Note", "Sound settings are saved on this device and shared by every game.", 32, new Color32(208, 200, 224, 255), false);
        Stretch(note.rectTransform, new Vector2(0.1f, 0.42f), new Vector2(0.9f, 0.53f));

        Button backButton = MakeButton(settingsRect, "BackButton", "BACK", new Color32(176, 0, 32, 255), Color.white, rounded,
                                       new Vector2(0.25f, 0.15f), new Vector2(0.75f, 0.24f), out _);

        // ---- Controller ----
        GameObject controllerGo = new GameObject("MainMenuController");
        MainMenuController controller = controllerGo.AddComponent<MainMenuController>();
        SerializedObject controllerObject = new SerializedObject(controller);
        SerializedProperty games = controllerObject.FindProperty("games");
        games.arraySize = 2;
        SetEntry(games.GetArrayElementAtIndex(0), "COLOR GAME", "ColorGame", colorButton, colorLabel);
        SetEntry(games.GetArrayElementAtIndex(1), "SCATTER GAME", "", scatterButton, scatterLabel); // set the real name + scene in the Inspector
        controllerObject.FindProperty("settingsButton").objectReferenceValue = settingsButton;
        controllerObject.FindProperty("settingsPanel").objectReferenceValue = settingsRect.gameObject;
        controllerObject.FindProperty("settingsCloseButton").objectReferenceValue = backButton;
        controllerObject.FindProperty("quitButton").objectReferenceValue = quitButton;
        controllerObject.FindProperty("audioManager").objectReferenceValue = audio;
        controllerObject.ApplyModifiedPropertiesWithoutUndo();

        settingsRect.gameObject.SetActive(false);

        EditorSceneManager.SaveScene(scene, MenuScenePath);

        // ---- Build list: MainMenu first, keep every other scene ----
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.RemoveAll(s => s.path == MenuScenePath);
        scenes.Insert(0, new EditorBuildSettingsScene(MenuScenePath, true));
        if (File.Exists(GameScenePath) && !scenes.Exists(s => s.path == GameScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(GameScenePath, true));
        }
        EditorBuildSettings.scenes = scenes.ToArray();

        Debug.Log("[MenuBuilder] Created " + MenuScenePath + " and put it first in the build list. " +
                  "Check SaveManager > Save File Name matches the Color Game scene. " +
                  "Select MainMenuController and set the Scatter entry's Display Name and Scene Name.");
    }

    // ---------- Helpers ----------

    private static AudioSource MakeAudioSource(Transform parent, string name, bool loop)
    {
        GameObject go = new GameObject(name, typeof(AudioSource));
        go.transform.SetParent(parent, false);
        AudioSource source = go.GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = loop;
        return source;
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
                                     Vector2 min, Vector2 max, out TextMeshProUGUI text)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        Style(image, sprite, color, true);

        Button button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        Navigation navigation = button.navigation;
        navigation.mode = Navigation.Mode.None;
        button.navigation = navigation;

        text = MakeText(rect, "Label", label, 64, labelColor, true);
        Stretch(text.rectTransform, new Vector2(0.04f, 0.05f), new Vector2(0.96f, 0.95f));
        return button;
    }

    private static void SetEntry(SerializedProperty entry, string displayName, string sceneName, Button button, TMP_Text label)
    {
        entry.FindPropertyRelative("displayName").stringValue = displayName;
        entry.FindPropertyRelative("sceneName").stringValue = sceneName;
        entry.FindPropertyRelative("button").objectReferenceValue = button;
        entry.FindPropertyRelative("label").objectReferenceValue = label;
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError($"[MenuBuilder] Field '{field}' was not found on {target.GetType().Name}.");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
