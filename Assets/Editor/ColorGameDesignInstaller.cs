using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using ColorGame.DevTools;
using ColorGame.Gameplay;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Design > 3 - Apply Design System   (run it in the MainMenu scene AND the game scene)
/// - restyles the prefabs (ColorButton, BetBill, BetButton, ResultSlot, HistoryRow)
/// - applies the typography (font + outline presets) to every TMP text in the open scene
/// - restyles buttons and panels, adds press feedback
/// - in the game scene: adds the FORCE RESULT button to the dev panel
/// Safe to run twice. Run steps 1 and 2 first, and "Install Bet Slip + Bills + Orientation" before this.
/// </summary>
public static class ColorGameDesignInstaller
{
    private const string TypoPath = "Assets/ColorGame/UI/Typography/GameTypography.asset";
    private const string LibraryPath = "Assets/ColorGame/UI/Bills/BillSpriteLibrary.asset";
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string FramePath = "Assets/ColorGame/UI/AreaFrame.png";
    private const string HighlightPath = "Assets/ColorGame/UI/AreaHighlight.png";
    private const string GlossPath = "Assets/ColorGame/UI/Gloss.png";
    private const string BillPrefab = "Assets/ColorGame/Prefabs/BetBill.prefab";
    private const string ColorButtonPrefab = "Assets/ColorGame/Prefabs/ColorButton.prefab";
    private static readonly string[] TextPrefabs =
    {
        "Assets/ColorGame/Prefabs/ColorButton.prefab", "Assets/ColorGame/Prefabs/BetButton.prefab",
        "Assets/ColorGame/Prefabs/ResultSlot.prefab", "Assets/ColorGame/Prefabs/HistoryRow.prefab",
        "Assets/ColorGame/Prefabs/BetBill.prefab"
    };

    [MenuItem("Tools/Color Game/Design/3 - Apply Design System")]
    public static void Install()
    {
        if (EditorApplication.isPlaying) { Debug.LogWarning("[Design] Stop Play mode first."); return; }

        GameTypography typo = AssetDatabase.LoadAssetAtPath<GameTypography>(TypoPath);
        BillSpriteLibrary library = AssetDatabase.LoadAssetAtPath<BillSpriteLibrary>(LibraryPath);
        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        Sprite frame = AssetDatabase.LoadAssetAtPath<Sprite>(FramePath);
        Sprite highlight = AssetDatabase.LoadAssetAtPath<Sprite>(HighlightPath);
        Sprite gloss = AssetDatabase.LoadAssetAtPath<Sprite>(GlossPath);

        if (typo == null || library == null || rounded == null || frame == null || highlight == null || gloss == null)
        {
            Debug.LogError("[Design] Missing assets. Run Tools > Color Game > Design > 1 and 2 first (and Generate UI Sprites).");
            return;
        }

        PatchBillPrefab(typo, library);
        PatchColorButtonPrefab(frame, highlight, gloss, rounded);
        foreach (string path in TextPrefabs) StyleTextInPrefab(path, typo);

        ColorGameManager manager = Object.FindAnyObjectByType<ColorGameManager>(FindObjectsInactive.Include);
        if (manager != null) AddForceButton(manager, rounded);

        StyleScene(typo, rounded);

        AssetDatabase.SaveAssets();
        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("[Design] Design system applied. Save the scene (Ctrl+S).");
    }

    // ---------- Prefabs ----------

    private static void PatchBillPrefab(GameTypography typo, BillSpriteLibrary library)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(BillPrefab) == null)
        {
            Debug.LogError("[Design] BetBill.prefab not found. Run 'Install Bet Slip + Bills + Orientation' first.");
            return;
        }

        GameObject root = PrefabUtility.LoadPrefabContents(BillPrefab);
        try
        {
            ((RectTransform)root.transform).sizeDelta = new Vector2(128f, 60f);

            Image art = root.GetComponent<Image>();
            art.type = Image.Type.Simple;
            art.preserveAspect = true;
            art.color = Color.white;
            art.raycastTarget = false;

            Transform inner = root.transform.Find("Inner");
            if (inner != null) inner.gameObject.SetActive(false);

            Transform labelT = root.transform.Find("Label");
            if (labelT != null)
            {
                RectTransform r = (RectTransform)labelT;
                r.anchorMin = new Vector2(0.40f, 0.12f);
                r.anchorMax = new Vector2(0.94f, 0.88f);
                r.offsetMin = r.offsetMax = Vector2.zero;

                TextMeshProUGUI t = labelT.GetComponent<TextMeshProUGUI>();
                t.enableAutoSizing = true;
                t.fontSizeMin = 14f;
                t.fontSizeMax = 40f;
                t.color = Color.white;
                typo.Apply(t, TextRole.Bill);
            }

            SerializedObject so = new SerializedObject(root.GetComponent<BetBillView>());
            so.FindProperty("library").objectReferenceValue = library;
            so.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(root, BillPrefab);
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
    }

    private static void PatchColorButtonPrefab(Sprite frame, Sprite highlight, Sprite gloss, Sprite rounded)
    {
        GameObject root = PrefabUtility.LoadPrefabContents(ColorButtonPrefab);
        try
        {
            ColorButtonView view = root.GetComponent<ColorButtonView>();
            SerializedObject so = new SerializedObject(view);
            SerializedProperty frameProp = so.FindProperty("frame");
            SerializedProperty showName = so.FindProperty("showColorName");
            if (frameProp == null || showName == null)
            {
                Debug.LogError("[Design] ColorButtonView has no 'frame' field. Install the new ColorButtonView.cs first.");
                return;
            }

            if (frameProp.objectReferenceValue == null)
            {
                if (root.GetComponent<Shadow>() == null)
                {
                    Shadow shadow = root.AddComponent<Shadow>();
                    shadow.effectColor = new Color(0f, 0f, 0f, 0.4f);
                    shadow.effectDistance = new Vector2(0f, -8f);
                }

                MakeOverlay(root.transform, "Gloss", gloss, 0);
                Image frameImage = MakeOverlay(root.transform, "Frame", frame, 1);
                Image highlightImage = MakeOverlay(root.transform, "Highlight", highlight, 2);
                Image flash = MakeOverlay(root.transform, "TapFlash", rounded, 3);
                flash.color = new Color(1f, 1f, 1f, 0f);

                so.FindProperty("frame").objectReferenceValue = frameImage;
                so.FindProperty("highlight").objectReferenceValue = highlightImage;
                so.FindProperty("tapFlash").objectReferenceValue = flash;
            }

            showName.boolValue = false; // the color and the bills identify the area

            // Names are hidden, so the bills get more room.
            Transform area = root.transform.Find("BillArea");
            if (area != null)
            {
                RectTransform r = (RectTransform)area;
                r.anchorMin = new Vector2(0.05f, 0.2f);
                r.anchorMax = new Vector2(0.95f, 0.92f);
                r.offsetMin = r.offsetMax = Vector2.zero;
            }

            so.ApplyModifiedPropertiesWithoutUndo();
            PrefabUtility.SaveAsPrefabAsset(root, ColorButtonPrefab);
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
    }

    private static Image MakeOverlay(Transform parent, string name, Sprite sprite, int siblingIndex)
    {
        Transform existing = parent.Find(name);
        GameObject go = existing != null ? existing.gameObject : new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        RectTransform rect = (RectTransform)go.transform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        Image image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = Color.white;
        image.raycastTarget = false; // overlays must never block taps
        go.transform.SetSiblingIndex(siblingIndex);
        return image;
    }

    private static void StyleTextInPrefab(string path, GameTypography typo)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null) return;

        GameObject root = PrefabUtility.LoadPrefabContents(path);
        try
        {
            foreach (TMP_Text text in root.GetComponentsInChildren<TMP_Text>(true))
            {
                if (path.EndsWith("BetBill.prefab")) continue; // done in PatchBillPrefab
                typo.Apply(text, RoleFor(text));
            }
            PrefabUtility.SaveAsPrefabAsset(root, path);
        }
        finally { PrefabUtility.UnloadPrefabContents(root); }
    }

    // ---------- Scene ----------

    private static void StyleScene(GameTypography typo, Sprite rounded)
    {
        foreach (TMP_Text text in Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include))
        {
            typo.Apply(text, RoleFor(text));
            EditorUtility.SetDirty(text);
        }

        foreach (Button button in Object.FindObjectsByType<Button>(FindObjectsInactive.Include))
        {
            StyleButton(button, rounded);
        }

        foreach (Image image in Object.FindObjectsByType<Image>(FindObjectsInactive.Include))
        {
            StylePanel(image, rounded);
        }
    }

    private static TextRole RoleFor(TMP_Text t)
    {
        string n = t.name.ToLowerInvariant();
        string p = t.transform.parent != null ? t.transform.parent.name.ToLowerInvariant() : string.Empty;

        if (n.Contains("title") || n.Contains("countdown")) return TextRole.Title;
        if (n.Contains("value") || n.Contains("total") || n.Contains("wintext") || n.Contains("winnings") ||
            p.Contains("betbutton") || p.Contains("statssection")) return TextRole.Number;
        if (t.GetComponentInParent<Button>() != null || p.Contains("resultslot")) return TextRole.Button;
        if (n.Contains("label") || n.Contains("note") || n.Contains("info") || n.Contains("status") || n.Contains("header")) return TextRole.Label;
        return TextRole.Body;
    }

    private static void StyleButton(Button button, Sprite rounded)
    {
        RectTransform rect = (RectTransform)button.transform;
        bool fullScreen = rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one;
        TMP_Text label = button.GetComponentInChildren<TMP_Text>(true);

        // Skip the full-screen backdrop, the color and bet buttons (their prefabs have their own look) and unlabeled buttons.
        if (fullScreen || label == null || button.GetComponent<ColorButtonView>() != null ||
            button.GetComponent<BetButtonView>() != null) return;

        Image image = button.targetGraphic as Image;
        if (image == null) return;

        string n = button.name.ToLowerInvariant();
        bool danger = n.Contains("close") || n.Contains("back") || n.Contains("quit") || n.Contains("clear") || n.Contains("reset");
        bool primary = n.Contains("colorgame");

        image.sprite = rounded;
        image.type = Image.Type.Sliced;
        image.color = danger ? GameTheme.Danger : (primary ? GameTheme.Gold : GameTheme.Violet);
        label.color = primary ? GameTheme.VioletDark : Color.white;

        if (button.GetComponent<Shadow>() == null)
        {
            Shadow shadow = button.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
            shadow.effectDistance = new Vector2(0f, -6f);
        }

        if (button.GetComponent<PressScale>() == null) button.gameObject.AddComponent<PressScale>();

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.96f, 0.96f, 0.96f, 1f);
        colors.pressedColor = new Color(0.78f, 0.78f, 0.78f, 1f);
        colors.selectedColor = Color.white;
        colors.disabledColor = new Color(0.6f, 0.6f, 0.6f, 0.55f);
        colors.fadeDuration = 0.08f;
        button.colors = colors;

        EditorUtility.SetDirty(button);
    }

    private static void StylePanel(Image image, Sprite rounded)
    {
        if (image.sprite != rounded || image.GetComponent<Button>() != null) return;

        string n = image.name.ToLowerInvariant();
        if (!(n.EndsWith("panel") || n.EndsWith("section") || n.EndsWith("card"))) return;

        image.color = GameTheme.Panel;
        if (image.GetComponent<Outline>() == null)
        {
            Outline outline = image.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(GameTheme.Gold.r, GameTheme.Gold.g, GameTheme.Gold.b, 0.45f);
            outline.effectDistance = new Vector2(2f, -2f);
            outline.useGraphicAlpha = false;
        }
        EditorUtility.SetDirty(image);
    }

    // ---------- Force Result button ----------

    private static void AddForceButton(ColorGameManager manager, Sprite rounded)
    {
        if (Object.FindAnyObjectByType<ForceResultUI>(FindObjectsInactive.Include) != null) return;

        RectTransform section = null;
        foreach (RectTransform rect in Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include))
        {
            if (rect.name == "ButtonsSection") { section = rect; break; }
        }

        if (section == null)
        {
            Debug.LogWarning("[Design] 'ButtonsSection' (dev panel) not found: the FORCE RESULT button was not added.");
            return;
        }

        GameObject go = new GameObject("ForceResultButton", typeof(RectTransform), typeof(Image), typeof(Button), typeof(ForceResultUI));
        go.transform.SetParent(section, false); // the grid layout sets its size

        Image image = go.GetComponent<Image>();
        image.sprite = rounded;
        image.type = Image.Type.Sliced;
        image.color = new Color32(230, 81, 0, 255);

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;

        GameObject labelGo = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        labelGo.transform.SetParent(go.transform, false);
        RectTransform lr = (RectTransform)labelGo.transform;
        lr.anchorMin = Vector2.zero;
        lr.anchorMax = Vector2.one;
        lr.offsetMin = lr.offsetMax = Vector2.zero;

        TextMeshProUGUI label = labelGo.GetComponent<TextMeshProUGUI>();
        label.text = "FORCE: OFF";
        label.alignment = TextAlignmentOptions.Center;
        label.enableAutoSizing = true;
        label.fontSizeMin = 20f;
        label.fontSizeMax = 44f;
        label.fontStyle = FontStyles.Bold;
        label.color = Color.white;
        label.raycastTarget = false;

        SerializedObject so = new SerializedObject(go.GetComponent<ForceResultUI>());
        so.FindProperty("gameManager").objectReferenceValue = manager;
        so.FindProperty("button").objectReferenceValue = button;
        so.FindProperty("label").objectReferenceValue = label;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}