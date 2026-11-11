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
using RouletteGame.Data;
using RouletteGame.Integration;
using RouletteGame.UI;
using RouletteGame.Wheel;

/// <summary>
/// Menu: Tools > Roulette Game > Build Roulette Scene
/// Builds the whole Roulette play scene (portrait 1080x1920) into Assets/RouletteGame/Scenes/RouletteGame.unity,
/// with its own wallet, save, audio and history managers that use the SAME save file as the other games.
/// Creates the settings asset and the wheel sprites if they are missing, and adds the scene to the build list.
/// Safe to run again: it asks before replacing the scene. Nothing in the other scenes is touched.
/// </summary>
public static class RouletteSceneBuilder
{
    private const string ScenePath = "Assets/RouletteGame/Scenes/RouletteGame.unity";
    private const string SettingsFolder = "Assets/RouletteGame/ScriptableObjects";
    private const string SettingsPath = SettingsFolder + "/RouletteSettings.asset";
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string GradientPath = "Assets/ColorGame/UI/BackgroundGradient.png";
    private const string WheelPath = "Assets/RouletteGame/Sprites/RouletteWheel.png";
    private const string BallPath = "Assets/RouletteGame/Sprites/RouletteBall.png";
    private const string SfxFolder = "Assets/ColorGame/Audio/SFX";

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color Dark = new Color32(42, 27, 61, 255);
    private static readonly Color Purple = new Color32(94, 53, 177, 255);
    private static readonly Color Red = new Color32(176, 0, 32, 255);
    private static readonly Color Soft = new Color32(208, 200, 224, 255);
    private static readonly Color Panel = new Color(0f, 0f, 0f, 0.4f);

    private static readonly (SfxType type, string file)[] SfxFiles =
    {
        (SfxType.ButtonClick, "sfx_button_click.wav"),
        (SfxType.Select, "sfx_color_select.wav"),
        (SfxType.Roll, "sfx_roll.wav"),
        (SfxType.SlotStop, "sfx_slot_stop.wav"),
        (SfxType.Win, "sfx_win.wav"),
        (SfxType.Lose, "sfx_lose.wav")
    };

    [MenuItem("Tools/Roulette Game/Create Default Settings Asset")]
    public static void CreateSettings()
    {
        RouletteSettings settings = EnsureSettings();
        Selection.activeObject = settings;
    }

    [MenuItem("Tools/Roulette Game/Build Roulette Scene")]
    public static void Build()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[RouletteBuilder] Stop Play mode first.");
            return;
        }

        RouletteSettings settings = EnsureSettings();
        if (settings == null) return;

        if (!EditorUtility.DisplayDialog("Build Roulette scene",
                "This builds " + ScenePath + ".\nAnything already in that scene will be replaced.", "Build", "Cancel"))
        {
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        // The wheel pictures are generated on demand.
        if (AssetDatabase.LoadAssetAtPath<Sprite>(WheelPath) == null || AssetDatabase.LoadAssetAtPath<Sprite>(BallPath) == null)
        {
            RouletteWheelSpriteGenerator.Generate();
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Sprite gradient = AssetDatabase.LoadAssetAtPath<Sprite>(GradientPath);
        Sprite wheelSprite = AssetDatabase.LoadAssetAtPath<Sprite>(WheelPath);
        Sprite ballSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BallPath);
        if (rounded == null || gradient == null)
        {
            Debug.LogWarning("[RouletteBuilder] UI sprites are missing. Run Tools > Color Game > Generate UI Sprites, then build again for the full look.");
        }
        if (wheelSprite == null || ballSprite == null)
        {
            Debug.LogError("[RouletteBuilder] The wheel sprites could not be created. Run Tools > Roulette Game > Generate Wheel Sprites and build again.");
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ---- Camera ----
        GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(23, 13, 38, 255);

        // ---- Managers (same code and same save file as the other games) ----
        GameObject managers = new GameObject("----Managers----");

        GameObject walletObject = new GameObject("WalletManager");
        walletObject.transform.SetParent(managers.transform);
        VirtualWalletManager wallet = walletObject.AddComponent<VirtualWalletManager>();

        GameObject saveObject = new GameObject("SaveManager");
        saveObject.transform.SetParent(managers.transform);
        SaveManager save = saveObject.AddComponent<SaveManager>();
        SetRef(save, "walletManager", wallet);

        GameObject audioObject = new GameObject("AudioManager");
        audioObject.transform.SetParent(managers.transform);
        AudioManager audio = audioObject.AddComponent<AudioManager>();
        AudioSource sfxSource = MakeAudioSource(audioObject.transform, "SFXSource", false);
        AudioSource musicSource = MakeAudioSource(audioObject.transform, "MusicSource", true);
        SetRef(audio, "saveManager", save);
        SetRef(audio, "sfxSource", sfxSource);
        SetRef(audio, "musicSource", musicSource);
        AssignSounds(audio);

        GameObject historyObject = new GameObject("RouletteHistory");
        historyObject.transform.SetParent(managers.transform);
        RouletteHistoryManager history = historyObject.AddComponent<RouletteHistoryManager>();
        SetRef(history, "saveManager", save);
        SetRef(history, "settings", settings);

        // ---- Event system ----
        GameObject eventSystem = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
#if ENABLE_INPUT_SYSTEM
        eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif

        // ---- Canvas ----
        GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(ResponsiveCanvasScaler));
        canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        RectTransform background = MakeRect("Background", canvasObject.transform, Vector2.zero, Vector2.one);
        Image backgroundImage = background.gameObject.AddComponent<Image>();
        backgroundImage.sprite = gradient;
        backgroundImage.color = gradient != null ? Color.white : Dark;
        backgroundImage.raycastTarget = false;

        RectTransform safe = MakeRect("SafeAreaPanel", canvasObject.transform, Vector2.zero, Vector2.one);
        safe.gameObject.AddComponent<SafeAreaFitter>();

        // ---- Top bar ----
        Button homeButton = MakeButton(safe, "HomeButton", "HOME", Purple, Color.white, rounded, V(0.03f, 0.935f), V(0.23f, 0.985f), out _);
        TextMeshProUGUI title = MakeText(safe, "Title", "ROULETTE", 80f, Gold);
        Anchor(title.rectTransform, V(0.25f, 0.935f), V(0.75f, 0.985f));
        Button soundButton = MakeButton(safe, "SoundButton", "SOUND: ON", Purple, Color.white, rounded, V(0.77f, 0.935f), V(0.97f, 0.985f), out TextMeshProUGUI soundLabel);
        AudioToggleUI toggle = soundButton.gameObject.AddComponent<AudioToggleUI>();
        SetRef(toggle, "audioManager", audio);
        SetRef(toggle, "button", soundButton);
        SetRef(toggle, "label", soundLabel);

        // ---- Balance, total bet, maximum payout ----
        StatPanel(safe, "BalancePanel", "BALANCE", V(0.03f, 0.875f), V(0.36f, 0.930f), rounded, Gold, out TextMeshProUGUI balanceValue);
        StatPanel(safe, "TotalBetPanel", "TOTAL BET", V(0.37f, 0.875f), V(0.64f, 0.930f), rounded, Color.white, out TextMeshProUGUI totalBetValue);
        StatPanel(safe, "PotentialPanel", "MAX PAYOUT", V(0.65f, 0.875f), V(0.97f, 0.930f), rounded, new Color32(124, 226, 140, 255), out TextMeshProUGUI potentialValue);

        // ---- Last numbers ----
        PanelRect(safe, "LastNumbersPanel", V(0.03f, 0.835f), V(0.97f, 0.870f), rounded, Panel);
        RectTransform lastNumbers = MakeRect("LastNumbers", safe, V(0.04f, 0.835f), V(0.96f, 0.870f));
        HorizontalLayoutGroup lastLayout = lastNumbers.gameObject.AddComponent<HorizontalLayoutGroup>();
        lastLayout.spacing = 8f;
        lastLayout.childAlignment = TextAnchor.MiddleLeft;
        lastLayout.childControlWidth = false;
        lastLayout.childControlHeight = false;
        lastLayout.childForceExpandWidth = false;
        lastLayout.childForceExpandHeight = false;

        // ---- Wheel ----
        RectTransform wheelArea = MakeRect("WheelArea", safe, V(0.03f, 0.600f), V(0.97f, 0.830f));
        RectTransform wheelFrame = MakeRect("WheelFrame", wheelArea, Vector2.zero, Vector2.one);
        AspectRatioFitter fitter = wheelFrame.gameObject.AddComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = 1f;

        RectTransform wheelRect = MakeRect("Wheel", wheelFrame, Vector2.zero, Vector2.one);
        Image wheelImage = wheelRect.gameObject.AddComponent<Image>();
        wheelImage.sprite = wheelSprite;
        wheelImage.preserveAspect = true;
        wheelImage.raycastTarget = false;

        RectTransform ballRect = MakeRect("Ball", wheelFrame, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        ballRect.sizeDelta = new Vector2(26f, 26f);
        ballRect.anchoredPosition = Vector2.zero;
        Image ballImage = ballRect.gameObject.AddComponent<Image>();
        ballImage.sprite = ballSprite;
        ballImage.raycastTarget = false;

        WheelView wheelView = wheelFrame.gameObject.AddComponent<WheelView>();
        SetRef(wheelView, "wheel", wheelRect);
        SetRef(wheelView, "ball", ballRect);
        SetFloat(wheelView, "labelSize", 20f);

        // ---- Status line ----
        TextMeshProUGUI status = MakeText(safe, "StatusText", "PLACE YOUR BETS", 40f, Soft);
        Anchor(status.rectTransform, V(0.05f, 0.565f), V(0.95f, 0.600f));

        // ---- Betting table (the view builds the areas and tap spots itself) ----
        PanelRect(safe, "TableBackground", V(0.04f, 0.150f), V(0.96f, 0.560f), rounded, Panel);
        RectTransform tableArea = MakeRect("TableArea", safe, V(0.04f, 0.150f), V(0.96f, 0.560f));
        RectTransform chipLayer = MakeRect("ChipLayer", safe, V(0.04f, 0.150f), V(0.96f, 0.560f));

        // ---- Chips ----
        RectTransform chipRow = MakeRect("ChipRow", safe, V(0.04f, 0.095f), V(0.96f, 0.145f));
        HorizontalLayoutGroup chipLayout = chipRow.gameObject.AddComponent<HorizontalLayoutGroup>();
        chipLayout.spacing = 18f;
        chipLayout.childAlignment = TextAnchor.MiddleCenter;
        chipLayout.childControlWidth = false;
        chipLayout.childControlHeight = false;
        chipLayout.childForceExpandWidth = false;
        chipLayout.childForceExpandHeight = false;

        // ---- Buttons ----
        Button undoButton = MakeButton(safe, "UndoButton", "UNDO", Purple, Color.white, rounded, V(0.03f, 0.050f), V(0.26f, 0.092f), out _);
        Button clearButton = MakeButton(safe, "ClearButton", "CLEAR", Red, Color.white, rounded, V(0.27f, 0.050f), V(0.50f, 0.092f), out _);
        Button repeatButton = MakeButton(safe, "RepeatButton", "REPEAT", Purple, Color.white, rounded, V(0.51f, 0.050f), V(0.74f, 0.092f), out _);
        Button removeButton = MakeButton(safe, "RemoveButton", "REMOVE", Purple, Color.white, rounded, V(0.75f, 0.050f), V(0.97f, 0.092f), out TextMeshProUGUI removeLabel);
        Button confirmButton = MakeButton(safe, "ConfirmButton", "CONFIRM BETS", new Color32(255, 179, 0, 255), Dark, rounded, V(0.03f, 0.004f), V(0.49f, 0.046f), out TextMeshProUGUI confirmLabel);
        Button spinButton = MakeButton(safe, "SpinButton", "SPIN", new Color32(52, 168, 83, 255), Color.white, rounded, V(0.51f, 0.004f), V(0.97f, 0.046f), out TextMeshProUGUI spinLabel);

        // ---- Result card (switched off until a round ends) ----
        RectTransform resultRoot = MakeRect("ResultPanel", safe, Vector2.zero, Vector2.one);
        Image dim = resultRoot.gameObject.AddComponent<Image>();
        dim.color = new Color(0f, 0f, 0f, 0.78f); // also blocks taps on the table behind it

        RectTransform card = PanelRect(resultRoot, "ResultCard", V(0.06f, 0.22f), V(0.94f, 0.78f), rounded, new Color32(26, 15, 48, 245));
        TextMeshProUGUI resultTitle = MakeText(card, "Title", "ROUND RESULT", 64f, Gold);
        Anchor(resultTitle.rectTransform, V(0.05f, 0.90f), V(0.95f, 0.98f));

        RectTransform badge = PanelRect(card, "NumberBadge", V(0.37f, 0.69f), V(0.63f, 0.89f), rounded, Color.gray);
        Image badgeImage = badge.GetComponent<Image>();
        TextMeshProUGUI numberText = MakeText(badge, "Number", "0", 120f, Color.white);
        Anchor(numberText.rectTransform, V(0.05f, 0.05f), V(0.95f, 0.95f));

        TextMeshProUGUI colorText = MakeText(card, "ColorText", "GREEN", 44f, Color.white);
        Anchor(colorText.rectTransform, V(0.05f, 0.62f), V(0.95f, 0.68f));
        TextMeshProUGUI summaryText = MakeText(card, "Summary", string.Empty, 32f, Soft);
        Anchor(summaryText.rectTransform, V(0.05f, 0.555f), V(0.95f, 0.615f));

        TextMeshProUGUI betsText = MakeText(card, "BetLines", string.Empty, 32f, Color.white);
        betsText.alignment = TextAlignmentOptions.TopLeft;
        betsText.fontStyle = FontStyles.Normal;
        Anchor(betsText.rectTransform, V(0.08f, 0.27f), V(0.92f, 0.55f));

        TextMeshProUGUI payoutText = MakeText(card, "Payout", string.Empty, 46f, Gold);
        Anchor(payoutText.rectTransform, V(0.05f, 0.19f), V(0.95f, 0.26f));
        TextMeshProUGUI netText = MakeText(card, "Net", string.Empty, 60f, Color.white);
        Anchor(netText.rectTransform, V(0.05f, 0.11f), V(0.95f, 0.19f));
        Button continueButton = MakeButton(card, "ContinueButton", "CONTINUE", new Color32(255, 179, 0, 255), Dark, rounded, V(0.25f, 0.02f), V(0.75f, 0.10f), out _);
        resultRoot.gameObject.SetActive(false);

        // ---- Logic object ----
        GameObject logic = new GameObject("RouletteGame");

        BettingTableView tableView = logic.AddComponent<BettingTableView>();
        SetRef(tableView, "tableArea", tableArea);
        SetRef(tableView, "chipLayer", chipLayer);
        SetRef(tableView, "cellSprite", rounded);

        ChipSelectorUI chipSelector = logic.AddComponent<ChipSelectorUI>();
        SetRef(chipSelector, "settings", settings);
        SetRef(chipSelector, "container", chipRow);
        SetRef(chipSelector, "chipSprite", rounded);

        RouletteHUD hud = logic.AddComponent<RouletteHUD>();
        SetRef(hud, "settings", settings);
        SetRef(hud, "balanceText", balanceValue);
        SetRef(hud, "totalBetText", totalBetValue);
        SetRef(hud, "potentialText", potentialValue);
        SetRef(hud, "statusText", status);
        SetRef(hud, "lastNumbersContainer", lastNumbers);
        SetRef(hud, "badgeSprite", rounded);

        RouletteResultPanel resultPanel = logic.AddComponent<RouletteResultPanel>();
        SetRef(resultPanel, "panelRoot", resultRoot.gameObject);
        SetRef(resultPanel, "numberBadge", badgeImage);
        SetRef(resultPanel, "numberText", numberText);
        SetRef(resultPanel, "colorText", colorText);
        SetRef(resultPanel, "summaryText", summaryText);
        SetRef(resultPanel, "betsText", betsText);
        SetRef(resultPanel, "payoutText", payoutText);
        SetRef(resultPanel, "netText", netText);
        SetRef(resultPanel, "continueButton", continueButton);

        RouletteGameController controller = logic.AddComponent<RouletteGameController>();
        SetRef(controller, "settings", settings);
        SetRef(controller, "walletManager", wallet);
        SetRef(controller, "saveManager", save);
        SetRef(controller, "historyManager", history);
        SetRef(controller, "wheel", wheelView);
        SetRef(controller, "table", tableView);
        SetRef(controller, "chips", chipSelector);
        SetRef(controller, "hud", hud);
        SetRef(controller, "resultPanel", resultPanel);
        SetRef(controller, "undoButton", undoButton);
        SetRef(controller, "clearButton", clearButton);
        SetRef(controller, "repeatButton", repeatButton);
        SetRef(controller, "removeButton", removeButton);
        SetRef(controller, "removeLabel", removeLabel);
        SetRef(controller, "confirmButton", confirmButton);
        SetRef(controller, "confirmLabel", confirmLabel);
        SetRef(controller, "spinButton", spinButton);
        SetRef(controller, "spinLabel", spinLabel);
        SetRef(controller, "homeButton", homeButton);

        RouletteAudio rouletteAudio = logic.AddComponent<RouletteAudio>();
        SetRef(rouletteAudio, "audioManager", audio);
        SetRef(rouletteAudio, "controller", controller);
        SetRef(rouletteAudio, "wheel", wheelView);

        // ---- Orientation: portrait, like the main menu and Scatter ----
        GameObject orientation = new GameObject("SceneOrientation");
        orientation.AddComponent<SceneOrientation>(); // Portrait is the default mode

        EditorSceneManager.SaveScene(scene, ScenePath);

        // ---- Build list: keep every scene, add this one ----
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!scenes.Exists(s => s.path == ScenePath)) scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();

        if (!scenes.Exists(s => s.enabled && Path.GetFileNameWithoutExtension(s.path) == "MainMenu"))
        {
            Debug.LogWarning("[RouletteBuilder] 'MainMenu' is not in the build list. The HOME button needs it (File > Build Profiles > Scene List).");
        }

        Debug.Log("[RouletteBuilder] Built " + ScenePath + " and added it to the build list. Open it and press Play. " +
                  "Optional: run Tools > Color Game > Design > 3 - Apply Design System in this scene for the shared fonts.");
    }

    // ---------- Settings ----------

    private static RouletteSettings EnsureSettings()
    {
        RouletteSettings settings = AssetDatabase.LoadAssetAtPath<RouletteSettings>(SettingsPath);
        if (settings != null) return settings;

        string root = Directory.GetParent(Application.dataPath).FullName;
        Directory.CreateDirectory(Path.Combine(root, SettingsFolder));

        settings = ScriptableObject.CreateInstance<RouletteSettings>();
        AssetDatabase.CreateAsset(settings, SettingsPath);
        AssetDatabase.SaveAssets();
        Debug.Log("[RouletteBuilder] Created " + SettingsPath + " with the default chips (10, 20, 50, 100, 500).");
        return settings;
    }

    // ---------- Audio ----------

    private static void AssignSounds(AudioManager audio)
    {
        SerializedObject audioSerialized = new SerializedObject(audio);
        SerializedProperty list = audioSerialized.FindProperty("sfxEntries");
        if (list == null)
        {
            Debug.LogError("[RouletteBuilder] AudioManager has no 'sfxEntries' list.");
            return;
        }

        int missing = 0;
        foreach ((SfxType type, string file) in SfxFiles)
        {
            int index = -1;
            for (int i = 0; i < list.arraySize; i++)
            {
                if (list.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex == (int)type) index = i;
            }

            if (index < 0)
            {
                list.arraySize++;
                index = list.arraySize - 1;
                SerializedProperty row = list.GetArrayElementAtIndex(index);
                row.FindPropertyRelative("type").enumValueIndex = (int)type;
                row.FindPropertyRelative("volume").floatValue = 1f;
            }

            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(SfxFolder + "/" + file);
            if (clip == null) { missing++; continue; }
            list.GetArrayElementAtIndex(index).FindPropertyRelative("clip").objectReferenceValue = clip;
        }

        audioSerialized.ApplyModifiedPropertiesWithoutUndo();
        if (missing > 0)
        {
            Debug.LogWarning("[RouletteBuilder] " + missing + " sound file(s) were not found in " + SfxFolder +
                             ". Run Tools > Color Game > Generate Placeholder Sounds, then build again.");
        }
    }

    // ---------- UI helpers ----------

    private static Vector2 V(float x, float y) { return new Vector2(x, y); }

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
        Anchor(rect, min, max);
        return rect;
    }

    private static void Anchor(RectTransform rect, Vector2 min, Vector2 max)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static RectTransform PanelRect(Transform parent, string name, Vector2 min, Vector2 max, Sprite sprite, Color color)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;
        image.raycastTarget = false;
        return rect;
    }

    private static TextMeshProUGUI MakeText(Transform parent, string name, string text, float size, Color color)
    {
        return RouletteUi.MakeText(parent, name, text, size, color, null, true);
    }

    private static void StatPanel(Transform parent, string name, string label, Vector2 min, Vector2 max, Sprite sprite, Color valueColor, out TextMeshProUGUI value)
    {
        RectTransform panel = PanelRect(parent, name, min, max, sprite, Panel);
        TextMeshProUGUI labelText = MakeText(panel, "Label", label, 24f, Color.white);
        labelText.fontStyle = FontStyles.Normal;
        Anchor(labelText.rectTransform, V(0f, 0.55f), Vector2.one);
        value = MakeText(panel, "Value", "0", 44f, valueColor);
        Anchor(value.rectTransform, Vector2.zero, V(1f, 0.6f));
    }

    private static Button MakeButton(Transform parent, string name, string label, Color color, Color labelColor, Sprite sprite,
                                     Vector2 min, Vector2 max, out TextMeshProUGUI text)
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

        text = MakeText(rect, "Label", label, 44f, labelColor);
        Anchor(text.rectTransform, V(0.04f, 0.05f), V(0.96f, 0.95f));
        return button;
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[RouletteBuilder] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetFloat(Object target, string field, float value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[RouletteBuilder] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.floatValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
