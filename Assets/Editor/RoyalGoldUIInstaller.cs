using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using Shared.Wallet;

using ColorGame.Core;  //Fix
using ColorGame.Gameplay;
using ColorGame.History;
using ColorGame.UI;
using ColorGame.UI.RoyalGold;

/// <summary>
/// Menu: Tools > Color Game > Royal Gold > 1 - Install Royal Gold UI   (open the ColorGame scene first)
/// Builds the "COLOR GAME" Royal Gold interface under SafeAreaPanel, with EMPTY PNG slots, and connects it to the
/// existing systems. Nothing is deleted: the old top bar / result panel / info / countdown are only made invisible
/// (reversible with menu item 3). Menu item 4 removes the new UI completely.
/// </summary>
public static class RoyalGoldUIInstaller
{
    private const string RootName = "RoyalGoldUI";
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";
    private const string TypoPath = "Assets/ColorGame/UI/Typography/GameTypography.asset";

    private static readonly string[] LegacyNames = { "TopBar", "ResultPanel", "SelectionInfoPanel", "Countdown" };

    private static readonly Color Gold = new Color32(255, 213, 74, 255);
    private static readonly Color PlateColor = new Color(0.10f, 0.04f, 0.20f, 0.78f);

    private static readonly List<string> SlotLog = new List<string>();
    private static Sprite rounded;
    private static GameTypography typography;

    [MenuItem("Tools/Color Game/Royal Gold/1 - Install Royal Gold UI")]
    public static void Install()
    {
        if (EditorApplication.isPlaying) { Debug.LogWarning("[RoyalGold] Stop Play mode first."); return; }

        if (Object.FindAnyObjectByType<RoyalGoldHUD>(FindObjectsInactive.Include) != null)
        {
            Debug.Log("[RoyalGold] Already installed. Use 'Remove Royal Gold UI' first if you want to rebuild it.");
            return;
        }

        ColorGameManager manager = Find<ColorGameManager>();
        SafeAreaFitter safe = Find<SafeAreaFitter>();
        BetSlip slip = Find<BetSlip>();
        GameHistoryManager history = Find<GameHistoryManager>();
        VirtualWalletManager wallet = Find<VirtualWalletManager>();
        AutoRollController autoRoll = Find<AutoRollController>();
        ColorSelectionUI colorSelection = Find<ColorSelectionUI>();
        PlayMenuUI playMenu = Find<PlayMenuUI>();

        SerializedProperty configProperty = manager != null ? new SerializedObject(manager).FindProperty("config") : null;
        ColorGameConfig config = configProperty != null ? configProperty.objectReferenceValue as ColorGameConfig : null;

        if (manager == null || safe == null || slip == null || history == null || wallet == null || config == null)
        {
            Debug.LogError("[RoyalGold] Open the ColorGame scene first. Missing: " +
                           (manager == null ? "ColorGameManager " : "") + (safe == null ? "SafeAreaFitter " : "") +
                           (slip == null ? "BetSlip " : "") + (history == null ? "GameHistoryManager " : "") +
                           (wallet == null ? "VirtualWalletManager " : "") + (config == null ? "Config " : ""));
            return;
        }

        if (autoRoll == null) Debug.LogWarning("[RoyalGold] No AutoRollController found: the countdown will show '--'.");

        Button legacyMenu = null;
        if (playMenu != null)
        {
            SerializedProperty menuProperty = new SerializedObject(playMenu).FindProperty("menuButton");
            legacyMenu = menuProperty != null ? menuProperty.objectReferenceValue as Button : null;
        }
        if (legacyMenu == null) Debug.LogWarning("[RoyalGold] The old MENU button was not found: the new MENU button will do nothing.");

        rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        typography = AssetDatabase.LoadAssetAtPath<GameTypography>(TypoPath);
        SlotLog.Clear();

        Transform parent = safe.transform;
        Transform gameContent = parent.Find("GameContentPanel");

        // ---------- Root (drawn above the game panels, below the menu / debug / toast panels) ----------
        RectTransform root = MakeRect(RootName, parent, Vector2.zero, Vector2.one);
        Undo.RegisterCreatedObjectUndo(root.gameObject, "Install Royal Gold UI");
        root.SetSiblingIndex(gameContent != null ? gameContent.GetSiblingIndex() + 1 : parent.childCount - 1);

        Slot(root, "RoyalGold_ScreenFrame", Vector2.zero, Vector2.one,
             "Full-screen gold border/frame PNG (transparent in the middle). Raycast is off.", false);

        // ---------- Balance ----------
        RectTransform balance = MakeRect("BalancePanel", root, new Vector2(0.01f, 0.885f), new Vector2(0.19f, 0.99f));
        Plate(balance, "BalancePlate", Vector2.zero, Vector2.one);
        Slot(balance, "RoyalGold_BalanceFrame", Vector2.zero, Vector2.one, "Gold rounded frame for the balance box.", false);
        Slot(balance, "RoyalGold_BalanceIcon", new Vector2(0.04f, 0.12f), new Vector2(0.28f, 0.88f), "Gold coin / $ icon.", true);
        TextMeshProUGUI balanceLabel = Text(balance, "BalanceLabel", "BALANCE:", 30f, Gold, false, TextAlignmentOptions.Left,
                                            new Vector2(0.30f, 0.55f), new Vector2(0.97f, 0.92f), TextRole.Label);
        TextMeshProUGUI balanceText = Text(balance, "BalanceText", "\u20B11,000", 60f, Color.white, true, TextAlignmentOptions.Left,
                                           new Vector2(0.30f, 0.06f), new Vector2(0.97f, 0.58f), TextRole.Number);
        WalletUI walletUi = balance.gameObject.AddComponent<WalletUI>();
        SetRef(walletUi, "walletManager", wallet);
        SetRef(walletUi, "balanceText", balanceText);

        // ---------- Round + countdown ----------
        RectTransform round = MakeRect("RoundInformation", root, new Vector2(0.20f, 0.87f), new Vector2(0.35f, 0.995f));
        Plate(round, "RoundPlate", Vector2.zero, Vector2.one);
        Slot(round, "RoyalGold_RoundFrame", Vector2.zero, Vector2.one, "Gold oval/rounded frame behind the round number and timer.", false);
        TextMeshProUGUI roundText = Text(round, "RoundNumberText", "ROUND #1", 36f, Color.white, true, TextAlignmentOptions.Center,
                                         new Vector2(0.08f, 0.58f), new Vector2(0.92f, 0.95f), TextRole.Label);
        Slot(round, "RoyalGold_CountdownIcon", new Vector2(0.08f, 0.08f), new Vector2(0.34f, 0.56f), "Gold clock / stopwatch icon.", true);
        TextMeshProUGUI countdownText = Text(round, "CountdownText", "--", 80f, Color.white, true, TextAlignmentOptions.Center,
                                             new Vector2(0.36f, 0.04f), new Vector2(0.92f, 0.60f), TextRole.Number);

        // ---------- Title ----------
        RectTransform title = MakeRect("ColorGameTitle", root, new Vector2(0.37f, 0.875f), new Vector2(0.70f, 1.0f));
        Plate(title, "TitlePlate", Vector2.zero, Vector2.one, new Color(0f, 0f, 0f, 0.35f));
        Slot(title, "RoyalGold_TitlePlate", Vector2.zero, Vector2.one, "Gold banner/plate behind the COLOR GAME title.", false);
        Slot(title, "RoyalGold_TitleCrown", new Vector2(0.40f, 0.58f), new Vector2(0.60f, 1.0f), "Gold crown above the title.", true);
        Text(title, "TitleText", "COLOR GAME", 110f, Gold, true, TextAlignmentOptions.Center,
             new Vector2(0.08f, 0.08f), new Vector2(0.92f, 0.64f), TextRole.Title);

        // ---------- Menu button ----------
        RectTransform menu = MakeRect("MenuButton", root, new Vector2(0.86f, 0.89f), new Vector2(0.99f, 0.99f));
        Image menuPlate = menu.gameObject.AddComponent<Image>();
        StylePlate(menuPlate, PlateColor, true);
        Button menuButton = menu.gameObject.AddComponent<Button>();
        menuButton.targetGraphic = menuPlate;
        Navigation navigation = menuButton.navigation;
        navigation.mode = Navigation.Mode.None;
        menuButton.navigation = navigation;
        menu.gameObject.AddComponent<PressScale>();
        Slot(menu, "RoyalGold_MenuButton", Vector2.zero, Vector2.one, "Gold button background (normal state).", false);
        Slot(menu, "RoyalGold_MenuIcon", new Vector2(0.06f, 0.2f), new Vector2(0.30f, 0.8f), "Home icon (optional).", true);
        Text(menu, "MenuText", "MENU", 48f, Gold, true, TextAlignmentOptions.Center,
             new Vector2(0.32f, 0.1f), new Vector2(0.96f, 0.9f), TextRole.Button);

        // ---------- Result section ----------
        RectTransform result = MakeRect("ResultSection", root, new Vector2(0.01f, 0.645f), new Vector2(0.57f, 0.865f));
        Plate(result, "ResultPlate", Vector2.zero, Vector2.one);
        Slot(result, "RoyalGold_ResultFrame", Vector2.zero, Vector2.one, "Large gold frame around the whole result panel.", false);
        Slot(result, "RoyalGold_ResultBanner", new Vector2(0.02f, 0.5f), new Vector2(0.34f, 0.96f), "Ribbon/banner behind WIN! / LOSE.", false);
        TextMeshProUGUI resultTitle = Text(result, "ResultTitleText", "PLACE YOUR BETS", 90f, Color.white, true, TextAlignmentOptions.Center,
                                           new Vector2(0.03f, 0.52f), new Vector2(0.33f, 0.94f), TextRole.Title);

        RectTransform summary = MakeRect("SummaryGroup", result, new Vector2(0.02f, 0.05f), new Vector2(0.34f, 0.5f));
        CanvasGroup summaryGroup = summary.gameObject.AddComponent<CanvasGroup>();
        summaryGroup.interactable = false;
        summaryGroup.blocksRaycasts = false;
        TextMeshProUGUI matchedText = Text(summary, "MatchedCountText", string.Empty, 40f, Color.white, true, TextAlignmentOptions.Center,
                                           new Vector2(0f, 0.5f), Vector2.one, TextRole.Label);
        TextMeshProUGUI winningText = Text(summary, "WinningAmountText", string.Empty, 44f, Gold, true, TextAlignmentOptions.Center,
                                           Vector2.zero, new Vector2(1f, 0.5f), TextRole.Number);

        RectTransform slotsRect = MakeRect("ResultColorSlots", result, new Vector2(0.36f, 0.12f), new Vector2(0.98f, 0.88f));
        HorizontalLayoutGroup layout = slotsRect.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 16f;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;

        RoyalResultSlotView[] slotViews = new RoyalResultSlotView[3];
        for (int i = 0; i < slotViews.Length; i++) slotViews[i] = MakeResultSlot(slotsRect, i + 1);

        // ---------- Betting section ----------
        RectTransform betting = MakeRect("BettingSection", root, Vector2.zero, Vector2.one);
        RectTransform info = MakeRect("BettingInformation", betting, new Vector2(0.01f, 0.598f), new Vector2(0.57f, 0.640f));
        Plate(info, "InfoPlate", Vector2.zero, Vector2.one);
        Slot(info, "RoyalGold_BettingInfoFrame", Vector2.zero, Vector2.one, "Thin gold bar frame for the bets / total / left line.", false);
        TextMeshProUGUI infoText = Text(info, "BettingInfoText", "NO BETS YET", 34f, Color.white, true, TextAlignmentOptions.Center,
                                        new Vector2(0.02f, 0.08f), new Vector2(0.98f, 0.92f), TextRole.Label);

        // ---------- Effects ----------
        MakeRect("EffectsLayer", root, Vector2.zero, Vector2.one);

        // ---------- HUD controller ----------
        RoyalGoldHUD hud = root.gameObject.AddComponent<RoyalGoldHUD>();
        SerializedObject so = new SerializedObject(hud);
        so.FindProperty("config").objectReferenceValue = config;
        so.FindProperty("gameManager").objectReferenceValue = manager;
        so.FindProperty("betSlip").objectReferenceValue = slip;
        so.FindProperty("walletManager").objectReferenceValue = wallet;
        so.FindProperty("historyManager").objectReferenceValue = history;
        so.FindProperty("autoRoll").objectReferenceValue = autoRoll;
        so.FindProperty("roundText").objectReferenceValue = roundText;
        so.FindProperty("countdownText").objectReferenceValue = countdownText;
        so.FindProperty("menuButton").objectReferenceValue = menuButton;
        so.FindProperty("legacyMenuButton").objectReferenceValue = legacyMenu;
        so.FindProperty("resultTitleText").objectReferenceValue = resultTitle;
        so.FindProperty("matchedCountText").objectReferenceValue = matchedText;
        so.FindProperty("winningAmountText").objectReferenceValue = winningText;
        so.FindProperty("summaryGroup").objectReferenceValue = summaryGroup;
        so.FindProperty("bettingInfoText").objectReferenceValue = infoText;
        so.FindProperty("colorButtonsRoot").objectReferenceValue = colorSelection != null ? colorSelection.transform : null;
        SerializedProperty slotList = so.FindProperty("slots");
        slotList.arraySize = slotViews.Length;
        for (int i = 0; i < slotViews.Length; i++) slotList.GetArrayElementAtIndex(i).objectReferenceValue = slotViews[i];
        so.ApplyModifiedPropertiesWithoutUndo();

        SetLegacyVisible(false);
        EditorSceneManager.MarkSceneDirty(root.gameObject.scene);
        Selection.activeGameObject = root.gameObject;

        StringBuilder sb = new StringBuilder("[RoyalGold] Installed. Save the scene (Ctrl+S).\nEmpty PNG slots to fill (" + SlotLog.Count + "):\n");
        foreach (string line in SlotLog) sb.AppendLine("  " + line);
        Debug.Log(sb.ToString());
    }

    [MenuItem("Tools/Color Game/Royal Gold/2 - Hide Legacy UI (reversible)")]
    public static void HideLegacy() { SetLegacyVisible(false); }

    [MenuItem("Tools/Color Game/Royal Gold/3 - Show Legacy UI")]
    public static void ShowLegacy() { SetLegacyVisible(true); }

    [MenuItem("Tools/Color Game/Royal Gold/4 - Remove Royal Gold UI")]
    public static void Remove()
    {
        if (EditorApplication.isPlaying) { Debug.LogWarning("[RoyalGold] Stop Play mode first."); return; }

        RoyalGoldHUD hud = Object.FindAnyObjectByType<RoyalGoldHUD>(FindObjectsInactive.Include);
        if (hud == null) { Debug.Log("[RoyalGold] Nothing to remove."); return; }

        UnityEngine.SceneManagement.Scene scene = hud.gameObject.scene;
        Undo.DestroyObjectImmediate(hud.gameObject);
        SetLegacyVisible(true);
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("[RoyalGold] Removed. The old UI is visible again. Save the scene (Ctrl+S).");
    }

    // The old objects keep running (events, sounds, references). They are only made invisible.
    private static void SetLegacyVisible(bool visible)
    {
        RoyalGoldHUD hud = Object.FindAnyObjectByType<RoyalGoldHUD>(FindObjectsInactive.Include);
        int count = 0;

        foreach (RectTransform rect in Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include))
        {
            if (System.Array.IndexOf(LegacyNames, rect.name) < 0) continue;
            if (hud != null && rect.IsChildOf(hud.transform)) continue;

            CanvasGroup group = rect.GetComponent<CanvasGroup>();
            if (group == null) group = Undo.AddComponent<CanvasGroup>(rect.gameObject);

            Undo.RecordObject(group, "Royal Gold legacy UI");
            group.alpha = visible ? 1f : 0f;
            group.blocksRaycasts = visible;
            EditorUtility.SetDirty(group);
            count++;
        }

        Debug.Log("[RoyalGold] Old UI " + (visible ? "shown" : "hidden") + " (" + count + " objects). Save the scene (Ctrl+S).");
    }

    // ---------- Result slot ----------

    private static RoyalResultSlotView MakeResultSlot(Transform parent, int number)
    {
        GameObject go = new GameObject("Slot_" + number, typeof(RectTransform), typeof(CanvasGroup), typeof(RoyalResultSlotView));
        go.transform.SetParent(parent, false);
        CanvasGroup group = go.GetComponent<CanvasGroup>();
        group.interactable = false;
        group.blocksRaycasts = false;

        RectTransform fillRect = MakeRect("Fill", go.transform, Vector2.zero, Vector2.one);
        Image fill = fillRect.gameObject.AddComponent<Image>();
        StylePlate(fill, Color.gray, false);
        Outline outline = fillRect.gameObject.AddComponent<Outline>();
        outline.enabled = false;

        Slot(go.transform, "RoyalGold_ResultSlotFrame_" + number, Vector2.zero, Vector2.one,
             "Glossy gold/white frame over the colored pill (same PNG for all three slots).", false);

        TextMeshProUGUI label = Text(go.transform, "Label", "?", 56f, Color.white, true, TextAlignmentOptions.Center,
                                     new Vector2(0.05f, 0.1f), new Vector2(0.95f, 0.9f), TextRole.Button);

        RoyalResultSlotView view = go.GetComponent<RoyalResultSlotView>();
        SerializedObject so = new SerializedObject(view);
        so.FindProperty("fill").objectReferenceValue = fill;
        so.FindProperty("highlightOutline").objectReferenceValue = outline;
        so.FindProperty("label").objectReferenceValue = label;
        so.FindProperty("group").objectReferenceValue = group;
        so.ApplyModifiedPropertiesWithoutUndo();
        return view;
    }

    // ---------- Builders ----------

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

    // An EMPTY PNG slot: an Image with no sprite, a faint gold tint, no raycast.
    private static Image Slot(Transform parent, string name, Vector2 min, Vector2 max, string png, bool preserveAspect)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(RoyalSpriteSlot));
        go.transform.SetParent(parent, false);
        Stretch(go.GetComponent<RectTransform>(), min, max);

        Image image = go.GetComponent<Image>();
        image.sprite = null;
        image.color = RoyalSpriteSlot.PlaceholderColor;
        image.raycastTarget = false;
        image.preserveAspect = preserveAspect;

        SerializedObject so = new SerializedObject(go.GetComponent<RoyalSpriteSlot>());
        so.FindProperty("assignThisPng").stringValue = png;
        so.ApplyModifiedPropertiesWithoutUndo();

        SlotLog.Add(name + "  -  " + png);
        return image;
    }

    private static Image Plate(Transform parent, string name, Vector2 min, Vector2 max)
    {
        return Plate(parent, name, min, max, PlateColor);
    }

    // A dark translucent plate (from your RoundedRect sprite) so text stays readable before your PNGs exist.
    private static Image Plate(Transform parent, string name, Vector2 min, Vector2 max, Color color)
    {
        RectTransform rect = MakeRect(name, parent, min, max);
        Image image = rect.gameObject.AddComponent<Image>();
        StylePlate(image, color, false);
        return image;
    }

    private static void StylePlate(Image image, Color color, bool raycast)
    {
        image.sprite = rounded;
        image.type = rounded != null ? Image.Type.Sliced : Image.Type.Simple;
        image.color = color;
        image.raycastTarget = raycast;
    }

    private static TextMeshProUGUI Text(Transform parent, string name, string text, float size, Color color, bool bold,
                                        TextAlignmentOptions alignment, Vector2 min, Vector2 max, TextRole role)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        Stretch(go.GetComponent<RectTransform>(), min, max);

        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        tmp.alignment = alignment;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 14f;
        tmp.fontSizeMax = size;
        tmp.raycastTarget = false;

        if (typography != null) typography.Apply(tmp, role);
        return tmp;
    }

    private static T Find<T>() where T : Object
    {
        return Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[RoyalGold] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
