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
using ScatterGame.Core;
using ScatterGame.Integration;
using ScatterGame.Runtime;

/// <summary>
/// Menu: Tools > Scatter Game > Build Scatter Scene
/// Builds the whole Ace Scatter play scene (portrait 1080x1920) into Assets/ScatterGame/Scene/ScatterGame.unity,
/// with its own wallet, save, audio, history and round managers that use the SAME save file as the Color Game.
/// It also adds the scene to the build list. Safe to run again: it asks before replacing the scene.
/// </summary>
public static class ScatterSceneBuilder
{
    private const string ScenePath = "Assets/ScatterGame/Scene/ScatterGame.unity";
    private const string SettingsPath = "Assets/ScatterGame/ScriptableObjects/ScatterGameSettings.asset";
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string GradientPath = "Assets/ColorGame/UI/BackgroundGradient.png";
    private const string SfxFolder = "Assets/ColorGame/Audio/SFX";

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color Dark = new Color32(42, 27, 61, 255);
    private static readonly Color Purple = new Color32(94, 53, 177, 255);
    private static readonly Color Red = new Color32(176, 0, 32, 255);
    private static readonly Color Soft = new Color32(208, 200, 224, 255);

    private static readonly (SfxType type, string file)[] SfxFiles =
    {
        (SfxType.ButtonClick, "sfx_button_click.wav"),
        (SfxType.Select, "sfx_color_select.wav"),
        (SfxType.Roll, "sfx_roll.wav"),
        (SfxType.SlotStop, "sfx_slot_stop.wav"),
        (SfxType.Win, "sfx_win.wav"),
        (SfxType.Lose, "sfx_lose.wav")
    };

    [MenuItem("Tools/Scatter Game/Build Scatter Scene")]
    public static void Build()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[ScatterBuilder] Stop Play mode first.");
            return;
        }

        ScatterGameSettings settings = AssetDatabase.LoadAssetAtPath<ScatterGameSettings>(SettingsPath);
        if (settings == null)
        {
            ScatterSettingsCreator.Create();
            settings = AssetDatabase.LoadAssetAtPath<ScatterGameSettings>(SettingsPath);
        }
        if (settings == null)
        {
            Debug.LogError("[ScatterBuilder] Could not find or create " + SettingsPath);
            return;
        }

        if (!EditorUtility.DisplayDialog("Build Ace Scatter scene",
                "This builds " + ScenePath + ".\nAnything already in that scene will be replaced.", "Build", "Cancel"))
        {
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Sprite gradient = AssetDatabase.LoadAssetAtPath<Sprite>(GradientPath);
        if (rounded == null || gradient == null)
        {
            Debug.LogWarning("[ScatterBuilder] UI sprites are missing. Run Tools > Color Game > Generate UI Sprites, then build again for the full look.");
        }

        // ---- Camera ----
        GameObject cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(23, 13, 38, 255);

        // ---- Managers ----
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

        GameObject historyObject = new GameObject("ScatterHistory");
        historyObject.transform.SetParent(managers.transform);
        ScatterHistoryManager historyManager = historyObject.AddComponent<ScatterHistoryManager>();
        SetRef(historyManager, "saveManager", save);

        GameObject roundObject = new GameObject("ScatterRound");
        roundObject.transform.SetParent(managers.transform);
        ScatterRoundService roundService = roundObject.AddComponent<ScatterRoundService>();
        SetRef(roundService, "settings", settings);
        SetRef(roundService, "walletManager", wallet);
        SetRef(roundService, "saveManager", save);
        SetRef(roundService, "historyManager", historyManager);

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
        Button homeButton = MakeButton(safe, "HomeButton", "HOME", Purple, Color.white, rounded,
                                       new Vector2(0.03f, 0.93f), new Vector2(0.23f, 0.985f), out _);
        TextMeshProUGUI title = MakeText(safe, "Title", "ACE SCATTER", 80f, Gold, true);
        Stretch(title.rectTransform, new Vector2(0.25f, 0.93f), new Vector2(0.75f, 0.985f));
        Button soundButton = MakeButton(safe, "SoundButton", "SOUND: ON", Purple, Color.white, rounded,
                                        new Vector2(0.77f, 0.93f), new Vector2(0.97f, 0.985f), out TextMeshProUGUI soundLabel);
        AudioToggleUI toggle = soundButton.gameObject.AddComponent<AudioToggleUI>();
        SetRef(toggle, "audioManager", audio);
        SetRef(toggle, "button", soundButton);
        SetRef(toggle, "label", soundLabel);

        // ---- Balance, win, multiplier ----
        RectTransform balancePanel = Panel(safe, "BalancePanel", new Vector2(0.2f, 0.865f), new Vector2(0.8f, 0.925f), rounded, new Color(0f, 0f, 0f, 0.4f));
        TextMeshProUGUI balanceLabel = MakeText(balancePanel, "Label", "VIRTUAL CREDITS", 26f, Color.white, false);
        Stretch(balanceLabel.rectTransform, new Vector2(0f, 0.58f), Vector2.one);
        TextMeshProUGUI balanceValue = MakeText(balancePanel, "Value", "0", 52f, Gold, true);
        Stretch(balanceValue.rectTransform, Vector2.zero, new Vector2(1f, 0.62f));

        RectTransform winPanel = Panel(safe, "WinPanel", new Vector2(0.04f, 0.785f), new Vector2(0.49f, 0.855f), rounded, new Color(0f, 0f, 0f, 0.4f));
        TextMeshProUGUI winLabel = MakeText(winPanel, "Label", "WIN", 26f, Color.white, false);
        Stretch(winLabel.rectTransform, new Vector2(0f, 0.58f), Vector2.one);
        TextMeshProUGUI winValue = MakeText(winPanel, "Value", "0", 52f, new Color32(124, 226, 140, 255), true);
        Stretch(winValue.rectTransform, Vector2.zero, new Vector2(1f, 0.62f));

        RectTransform multiplierPanel = Panel(safe, "MultiplierPanel", new Vector2(0.51f, 0.785f), new Vector2(0.96f, 0.855f), rounded, new Color(0f, 0f, 0f, 0.4f));
        TextMeshProUGUI multiplierLabel = MakeText(multiplierPanel, "Label", "MULTIPLIER", 26f, Color.white, false);
        Stretch(multiplierLabel.rectTransform, new Vector2(0f, 0.58f), Vector2.one);
        TextMeshProUGUI multiplierValue = MakeText(multiplierPanel, "Value", "x1", 52f, Gold, true);
        Stretch(multiplierValue.rectTransform, Vector2.zero, new Vector2(1f, 0.62f));

        // Free-spin bar (only visible during the bonus)
        RectTransform freeSpinBar = Panel(safe, "FreeSpinBar", new Vector2(0.15f, 0.745f), new Vector2(0.85f, 0.782f), rounded, new Color32(142, 36, 170, 255));
        TextMeshProUGUI freeSpinText = MakeText(freeSpinBar, "Text", "FREE SPIN 1 / 10", 36f, Color.white, true);
        Stretch(freeSpinText.rectTransform, Vector2.zero, Vector2.one);

        // ---- Grid ----
        RectTransform gridArea = MakeRect("GridArea", safe, new Vector2(0.03f, 0.31f), new Vector2(0.97f, 0.742f));
        RectTransform frame = MakeRect("GridFrame", gridArea, Vector2.zero, Vector2.one);
        Image frameImage = frame.gameObject.AddComponent<Image>();
        Style(frameImage, rounded, new Color(0f, 0f, 0f, 0.5f), false);
        AspectRatioFitter fitter = frame.gameObject.AddComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = (float)SlotConfig.Reels / SlotConfig.Rows;

        RectTransform mask = MakeRect("GridMask", frame, Vector2.zero, Vector2.one);
        mask.offsetMin = new Vector2(14f, 14f);
        mask.offsetMax = new Vector2(-14f, -14f);
        mask.gameObject.AddComponent<RectMask2D>();

        SlotSymbolView[] cells = new SlotSymbolView[SlotConfig.Reels * SlotConfig.Rows];
        for (int reel = 0; reel < SlotConfig.Reels; reel++)
        {
            for (int row = 0; row < SlotConfig.Rows; row++)
            {
                cells[reel * SlotConfig.Rows + row] = MakeCell(mask, reel, row, rounded);
            }
        }

        SlotGridView gridView = frame.gameObject.AddComponent<SlotGridView>();
        SerializedObject gridObject = new SerializedObject(gridView);
        SerializedProperty cellList = gridObject.FindProperty("cells");
        cellList.arraySize = cells.Length;
        for (int i = 0; i < cells.Length; i++) cellList.GetArrayElementAtIndex(i).objectReferenceValue = cells[i];
        gridObject.ApplyModifiedPropertiesWithoutUndo();

        // ---- Status text, bet controls ----
        TextMeshProUGUI status = MakeText(safe, "StatusText", string.Empty, 38f, new Color32(255, 138, 128, 255), true);
        Stretch(status.rectTransform, new Vector2(0.1f, 0.272f), new Vector2(0.9f, 0.305f));

        Button betDown = MakeButton(safe, "BetDownButton", "-", Purple, Color.white, rounded,
                                    new Vector2(0.12f, 0.20f), new Vector2(0.30f, 0.265f), out _);
        RectTransform betPanel = Panel(safe, "BetPanel", new Vector2(0.32f, 0.20f), new Vector2(0.68f, 0.265f), rounded, new Color(0f, 0f, 0f, 0.4f));
        TextMeshProUGUI betText = MakeText(betPanel, "Value", "BET  10", 48f, Gold, true);
        Stretch(betText.rectTransform, Vector2.zero, Vector2.one);
        Button betUp = MakeButton(safe, "BetUpButton", "+", Purple, Color.white, rounded,
                                  new Vector2(0.70f, 0.20f), new Vector2(0.88f, 0.265f), out _);

        // ---- Spin, auto, history ----
                // ---- Spin and auto ----
        Button autoButton = MakeButton(safe, "AutoButton", "AUTO", Purple, Color.white, rounded,
                                       new Vector2(0.10f, 0.085f), new Vector2(0.36f, 0.165f), out TextMeshProUGUI autoLabel);
        Button spinButton = MakeButton(safe, "SpinButton", "SPIN", new Color32(255, 179, 0, 255), Dark, rounded,
                                       new Vector2(0.42f, 0.065f), new Vector2(0.90f, 0.185f), out TextMeshProUGUI spinLabel);

        // ---- Banner (announcements) ----
        RectTransform banner = Panel(safe, "BannerPanel", new Vector2(0.04f, 0.42f), new Vector2(0.96f, 0.62f), rounded, new Color(0.06f, 0.03f, 0.12f, 0.92f));
        CanvasGroup bannerGroup = banner.gameObject.AddComponent<CanvasGroup>();
        TextMeshProUGUI bannerText = MakeText(banner, "Text", string.Empty, 120f, Gold, true);
        Stretch(bannerText.rectTransform, new Vector2(0.03f, 0.05f), new Vector2(0.97f, 0.95f));

        

        // ---- Components ----
        GameObject logic = new GameObject("ScatterGame");

        ScatterHUD hud = logic.AddComponent<ScatterHUD>();
        SetRef(hud, "balanceText", balanceValue);
        SetRef(hud, "winText", winValue);
        SetRef(hud, "multiplierText", multiplierValue);
        SetRef(hud, "betText", betText);
        SetRef(hud, "freeSpinText", freeSpinText);
        SetRef(hud, "statusText", status);
        SetRef(hud, "bannerText", bannerText);
        SetRef(hud, "freeSpinBar", freeSpinBar.gameObject);
        SetRef(hud, "bannerGroup", bannerGroup);


        ScatterGameController controller = logic.AddComponent<ScatterGameController>();
        SetRef(controller, "roundService", roundService);
        SetRef(controller, "walletManager", wallet);
        SetRef(controller, "saveManager", save);
        SetRef(controller, "audioManager", audio);
        SetRef(controller, "grid", gridView);
        SetRef(controller, "hud", hud);
        SetRef(controller, "spinButton", spinButton);
        SetRef(controller, "betDownButton", betDown);
        SetRef(controller, "betUpButton", betUp);
        SetRef(controller, "autoButton", autoButton);
        SetRef(controller, "homeButton", homeButton);
        SetRef(controller, "spinLabel", spinLabel);
        SetRef(controller, "autoLabel", autoLabel);

        EditorSceneManager.SaveScene(scene, ScenePath);

        // ---- Build list: keep every scene, add this one ----
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!scenes.Exists(s => s.path == ScenePath)) scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();

        Debug.Log("[ScatterBuilder] Built " + ScenePath + " and added it to the build list. Press Play to try it.");
    }

    // ---------- Grid cell ----------

    private static SlotSymbolView MakeCell(Transform parent, int reel, int row, Sprite rounded)
    {
        RectTransform cell = MakeRect("Cell_" + reel + "_" + row, parent,
            new Vector2(reel / (float)SlotConfig.Reels, (SlotConfig.Rows - 1 - row) / (float)SlotConfig.Rows),
            new Vector2((reel + 1) / (float)SlotConfig.Reels, (SlotConfig.Rows - row) / (float)SlotConfig.Rows));
        cell.offsetMin = new Vector2(4f, 4f);
        cell.offsetMax = new Vector2(-4f, -4f);

        RectTransform content = MakeRect("Content", cell, Vector2.zero, Vector2.one);
        Image background = content.gameObject.AddComponent<Image>();
        Style(background, rounded, Color.gray, false);

        Outline goldBorder = content.gameObject.AddComponent<Outline>();
        goldBorder.effectColor = new Color(1f, 0.84f, 0.25f, 1f);
        goldBorder.effectDistance = new Vector2(3f, 3f);
        goldBorder.enabled = false;

        Outline highlight = content.gameObject.AddComponent<Outline>();
        highlight.effectColor = new Color(1f, 0.97f, 0.6f, 1f);
        highlight.effectDistance = new Vector2(6f, 6f);
        highlight.enabled = false;
        
        RectTransform iconRect = MakeRect("Icon", content, Vector2.zero, Vector2.one);
        //RectTransform iconRect = MakeRect("Icon", content, new Vector2(0.08f, 0.08f), new Vector2(0.92f, 0.92f));
        Image icon = iconRect.gameObject.AddComponent<Image>();
        icon.preserveAspect = true;
        icon.raycastTarget = false;
        icon.enabled = false;

        TextMeshProUGUI label = MakeText(content, "Label", string.Empty, 110f, Color.white, true);
        Stretch(label.rectTransform, new Vector2(0.04f, 0.04f), new Vector2(0.96f, 0.96f));

        SlotSymbolView view = cell.gameObject.AddComponent<SlotSymbolView>();
        SetRef(view, "content", content);
        SetRef(view, "background", background);
        SetRef(view, "icon", icon);
        SetRef(view, "label", label);
        SetRef(view, "goldBorder", goldBorder);
        SetRef(view, "highlight", highlight);
        return view;
    }

    // ---------- Audio ----------

    private static void AssignSounds(AudioManager audio)
    {
        SerializedObject audioObject = new SerializedObject(audio);
        SerializedProperty list = audioObject.FindProperty("sfxEntries");
        if (list == null)
        {
            Debug.LogError("[ScatterBuilder] AudioManager has no 'sfxEntries' list.");
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

        audioObject.ApplyModifiedPropertiesWithoutUndo();
        if (missing > 0)
        {
            Debug.LogWarning("[ScatterBuilder] " + missing + " sound file(s) were not found in " + SfxFolder +
                             ". Run Tools > Color Game > Generate Placeholder Sounds, then build again.");
        }
    }

    // ---------- UI helpers ----------

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

    private static RectTransform Panel(Transform parent, string name, Vector2 min, Vector2 max, Sprite sprite, Color color)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        Style(image, sprite, color, false);
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

        text = MakeText(rect, "Label", label, 64f, labelColor, true);
        Stretch(text.rectTransform, new Vector2(0.04f, 0.05f), new Vector2(0.96f, 0.95f));
        return button;
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[ScatterBuilder] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}