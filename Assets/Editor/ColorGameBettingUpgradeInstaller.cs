using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.Gameplay;
using ColorGame.UI;

/// <summary>
/// Menu: Tools > Color Game > Install Bet Slip + Bills + Orientation
/// Run it once in EACH scene: the MainMenu scene (adds Portrait) and the game scene (adds the bet slip, bills, Landscape).
/// It creates BetBill.prefab, adds a bill area and a BET total to ColorButton.prefab, and wires the scene. Safe to run twice.
/// </summary>
public static class ColorGameBettingUpgradeInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string BillPath = "Assets/ColorGame/Prefabs/BetBill.prefab";
    private const string ButtonPath = "Assets/ColorGame/Prefabs/ColorButton.prefab";

    [MenuItem("Tools/Color Game/Install Bet Slip + Bills + Orientation")]
    public static void Install()
    {
        if (EditorApplication.isPlaying) { Debug.LogWarning("[Upgrade] Stop Play mode first."); return; }

        MainMenuController menu = Object.FindAnyObjectByType<MainMenuController>(FindObjectsInactive.Include);
        ColorGameManager manager = Object.FindAnyObjectByType<ColorGameManager>(FindObjectsInactive.Include);

        if (menu != null)
        {
            AddOrientation(menu.gameObject.scene, OrientationMode.Portrait);
            Debug.Log("[Upgrade] Main menu: Portrait orientation added. Save the scene (Ctrl+S).");
            return;
        }

        if (manager == null)
        {
            Debug.LogError("[Upgrade] Open the MainMenu scene or the game scene first.");
            return;
        }

        BetManager betManager = Find<BetManager>();
        VirtualWalletManager wallet = Find<VirtualWalletManager>();
        ColorSelectionUI colors = Find<ColorSelectionUI>();
        BetSelectionUI bets = Find<BetSelectionUI>();
        ColorGameConfig config = new SerializedObject(manager).FindProperty("config").objectReferenceValue as ColorGameConfig;

        if (betManager == null || wallet == null || colors == null || bets == null || config == null)
        {
            Debug.LogError("[Upgrade] Missing in this scene: BetManager, VirtualWalletManager, ColorSelectionUI, BetSelectionUI or the manager's Config.");
            return;
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        BetBillView billPrefab = EnsureBillPrefab(rounded);
        if (billPrefab == null) return;
        PatchColorButton(billPrefab);

        BetSlip slip = Find<BetSlip>();
        if (slip == null) slip = new GameObject("BetSlip").AddComponent<BetSlip>();
        Link(slip, "config", config);
        Link(slip, "betManager", betManager);
        Link(slip, "walletManager", wallet);

        Link(manager, "betSlip", slip);
        Link(colors, "betSlip", slip);
        Link(bets, "betSlip", slip);

        AddOrientation(manager.gameObject.scene, OrientationMode.Landscape);
        EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        Debug.Log("[Upgrade] Bet slip, bills and Landscape orientation installed. Save the scene (Ctrl+S).");
    }

    // ---------- Bill prefab ----------

    private static BetBillView EnsureBillPrefab(Sprite rounded)
    {
        BetBillView existing = AssetDatabase.LoadAssetAtPath<BetBillView>(BillPath);
        if (existing != null) return existing;

        GameObject root = new GameObject("BetBill", typeof(RectTransform), typeof(Image), typeof(BetBillView));
        RectTransform rt = (RectTransform)root.transform;
        rt.sizeDelta = new Vector2(120f, 56f);

        Image background = root.GetComponent<Image>();
        StyleImage(background, rounded, Color.white);

        GameObject innerGo = new GameObject("Inner", typeof(RectTransform), typeof(Image));
        innerGo.transform.SetParent(root.transform, false);
        Stretch((RectTransform)innerGo.transform, new Vector2(5f, 5f), new Vector2(-5f, -5f));
        Image inner = innerGo.GetComponent<Image>();
        StyleImage(inner, rounded, Color.white);

        GameObject labelGo = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
        labelGo.transform.SetParent(root.transform, false);
        Stretch((RectTransform)labelGo.transform, new Vector2(6f, 4f), new Vector2(-6f, -4f));
        TextMeshProUGUI label = labelGo.GetComponent<TextMeshProUGUI>();
        label.text = "P100";
        label.fontStyle = FontStyles.Bold;
        label.alignment = TextAlignmentOptions.Center;
        label.color = new Color32(40, 28, 52, 255);
        label.enableAutoSizing = true;
        label.fontSizeMin = 12f;
        label.fontSizeMax = 34f;
        label.raycastTarget = false;

        SerializedObject so = new SerializedObject(root.GetComponent<BetBillView>());
        so.FindProperty("background").objectReferenceValue = background;
        so.FindProperty("inner").objectReferenceValue = inner;
        so.FindProperty("label").objectReferenceValue = label;
        so.ApplyModifiedPropertiesWithoutUndo();

        GameObject saved = PrefabUtility.SaveAsPrefabAsset(root, BillPath);
        Object.DestroyImmediate(root);
        return saved != null ? saved.GetComponent<BetBillView>() : null;
    }

    // ---------- ColorButton prefab: name on top, bills in the middle, BET total at the bottom ----------

    private static void PatchColorButton(BetBillView billPrefab)
    {
        GameObject root = PrefabUtility.LoadPrefabContents(ButtonPath);
        try
        {
            ColorButtonView view = root.GetComponent<ColorButtonView>();
            if (view == null)
            {
                Debug.LogError("[Upgrade] ColorButtonView was not found on the root of ColorButton.prefab.");
                return;
            }

            SerializedObject so = new SerializedObject(view);
            if (so.FindProperty("billStack").objectReferenceValue != null) return; // already patched

            Transform name = root.transform.Find("Label");
            if (name != null)
            {
                RectTransform r = (RectTransform)name;
                r.anchorMin = new Vector2(0f, 0.62f);
                r.anchorMax = Vector2.one;
                r.offsetMin = r.offsetMax = Vector2.zero;
                TextMeshProUGUI t = name.GetComponent<TextMeshProUGUI>();
                if (t != null) { t.enableAutoSizing = true; t.fontSizeMin = 20f; t.fontSizeMax = 54f; }
            }

            GameObject area = new GameObject("BillArea", typeof(RectTransform), typeof(BillStackView));
            area.transform.SetParent(root.transform, false);
            RectTransform ar = (RectTransform)area.transform;
            ar.anchorMin = new Vector2(0.04f, 0.2f);
            ar.anchorMax = new Vector2(0.96f, 0.62f);
            ar.offsetMin = ar.offsetMax = Vector2.zero;
            SerializedObject stackObject = new SerializedObject(area.GetComponent<BillStackView>());
            stackObject.FindProperty("billPrefab").objectReferenceValue = billPrefab;
            stackObject.ApplyModifiedPropertiesWithoutUndo();

            GameObject totalGo = new GameObject("BetTotal", typeof(RectTransform), typeof(TextMeshProUGUI));
            totalGo.transform.SetParent(root.transform, false);
            RectTransform tr = (RectTransform)totalGo.transform;
            tr.anchorMin = new Vector2(0.05f, 0.01f);
            tr.anchorMax = new Vector2(0.95f, 0.2f);
            tr.offsetMin = tr.offsetMax = Vector2.zero;
            TextMeshProUGUI total = totalGo.GetComponent<TextMeshProUGUI>();
            total.text = string.Empty;
            total.fontStyle = FontStyles.Bold;
            total.alignment = TextAlignmentOptions.Center;
            total.color = Color.black;
            total.enableAutoSizing = true;
            total.fontSizeMin = 16f;
            total.fontSizeMax = 38f;
            total.raycastTarget = false;

            so.FindProperty("billStack").objectReferenceValue = area.GetComponent<BillStackView>();
            so.FindProperty("betTotalLabel").objectReferenceValue = total;
            so.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(root, ButtonPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    // ---------- Helpers ----------

    private static void AddOrientation(UnityEngine.SceneManagement.Scene scene, OrientationMode mode)
    {
        SceneOrientation existing = Object.FindAnyObjectByType<SceneOrientation>(FindObjectsInactive.Include);
        SceneOrientation target = existing != null ? existing : new GameObject("SceneOrientation").AddComponent<SceneOrientation>();

        SerializedObject so = new SerializedObject(target);
        so.FindProperty("mode").enumValueIndex = (int)mode;
        so.ApplyModifiedPropertiesWithoutUndo();
        EditorSceneManager.MarkSceneDirty(scene);
    }

    private static void StyleImage(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
        image.pixelsPerUnitMultiplier = 3.5f; // smaller rounded corners on a small bill
        image.color = color;
        image.raycastTarget = false;          // bills must never block taps on the color
    }

    private static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    private static T Find<T>() where T : Object
    {
        return Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);
    }

    private static void Link(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null) { Debug.LogError($"[Upgrade] Field '{field}' not found on {target.GetType().Name}."); return; }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}