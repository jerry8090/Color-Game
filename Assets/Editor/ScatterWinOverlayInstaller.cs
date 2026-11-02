using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Shared.UI;
using ScatterGame.Integration;

/// <summary>
/// Menu: Tools > Scatter Game > Add Win Overlay To Open Scene   (open the ScatterGame scene first)
/// Adds the floating win overlay under SafeAreaPanel and connects it to ScatterGameController.
/// Nothing existing is moved or changed. Safe to run twice. Also used by Build Scatter Scene.
/// </summary>
public static class ScatterWinOverlayInstaller
{
    private const string RoundedPath = "Assets/ColorGame/UI/RoundedRect.png";

    [MenuItem("Tools/Scatter Game/Add Win Overlay To Open Scene")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[ScatterOverlay] Stop Play mode first.");
            return;
        }

        ScatterGameController controller = Object.FindAnyObjectByType<ScatterGameController>(FindObjectsInactive.Include);
        SafeAreaFitter safe = Object.FindAnyObjectByType<SafeAreaFitter>(FindObjectsInactive.Include);
        if (controller == null || safe == null)
        {
            Debug.LogError("[ScatterOverlay] Open the ScatterGame scene first (ScatterGameController or SafeAreaPanel not found).");
            return;
        }

        if (Object.FindAnyObjectByType<ScatterWinOverlay>(FindObjectsInactive.Include) != null)
        {
            Debug.Log("[ScatterOverlay] The overlay already exists. Nothing to do.");
            return;
        }

        Sprite rounded = AssetDatabase.LoadAssetAtPath<Sprite>(RoundedPath);
        ScatterWinOverlay overlay = CreateOverlay(safe.transform, rounded);
        SetRef(controller, "winOverlay", overlay);

        EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
        Debug.Log("[ScatterOverlay] Added the win overlay. Save the scene (Ctrl+S).");
    }

    /// <summary>Builds the overlay under the given parent. The position is on the WinOverlay RectTransform if you want to move it.</summary>
    public static ScatterWinOverlay CreateOverlay(Transform parent, Sprite rounded)
    {
        GameObject root = new GameObject("WinOverlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasGroup));
        root.transform.SetParent(parent, false);

        RectTransform rect = root.GetComponent<RectTransform>();
        Stretch(rect, new Vector2(0.07f, 0.285f), new Vector2(0.93f, 0.415f));

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.overrideSorting = true;   // draws above the gameplay
        canvas.sortingOrder = 50;

        CanvasGroup group = root.GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;    // never blocks taps

        // Dark, semi-transparent background. Lower the alpha of this colour to make it more transparent.
        Image background = root.AddComponent<Image>();
        background.sprite = rounded;
        background.type = rounded != null ? Image.Type.Sliced : Image.Type.Simple;
        background.color = new Color(0.04f, 0.02f, 0.09f, 0.55f);
        background.raycastTarget = false;

        TextMeshProUGUI header = MakeText(root.transform, "Header", "TOTAL WIN", 30f, new Color32(208, 200, 224, 255), true,
                                          TextAlignmentOptions.Center, new Vector2(0.03f, 0.76f), new Vector2(0.97f, 0.98f));

        TextMeshProUGUI total = MakeText(root.transform, "Total", "0.00", 110f, Color.white, true,
                                         TextAlignmentOptions.Center, new Vector2(0.03f, 0.38f), new Vector2(0.97f, 0.78f));
        total.enableVertexGradient = true; // colourful: gold at the top, orange and pink below
        total.colorGradient = new VertexGradient(new Color32(255, 240, 130, 255), new Color32(255, 240, 130, 255),
                                                 new Color32(255, 140, 0, 255), new Color32(255, 80, 120, 255));

        TextMeshProUGUI latest = MakeText(root.transform, "Latest", "LATEST WIN  +0.00", 36f, new Color32(124, 226, 140, 255), true,
                                          TextAlignmentOptions.Left, new Vector2(0.04f, 0.19f), new Vector2(0.55f, 0.39f));

        TextMeshProUGUI multiplier = MakeText(root.transform, "Multiplier", "MULTIPLIER  x1", 36f, new Color32(255, 213, 74, 255), true,
                                              TextAlignmentOptions.Right, new Vector2(0.55f, 0.19f), new Vector2(0.96f, 0.39f));

        TextMeshProUGUI combos = MakeText(root.transform, "Combinations", string.Empty, 28f, Color.white, false,
                                          TextAlignmentOptions.Center, new Vector2(0.03f, 0.01f), new Vector2(0.97f, 0.21f));

        ScatterWinOverlay overlay = root.AddComponent<ScatterWinOverlay>();
        SetRef(overlay, "group", group);
        SetRef(overlay, "headerText", header);
        SetRef(overlay, "totalText", total);
        SetRef(overlay, "latestText", latest);
        SetRef(overlay, "multiplierText", multiplier);
        SetRef(overlay, "comboText", combos);
        return overlay;
    }

    // ---------- Helpers ----------

    private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static TextMeshProUGUI MakeText(Transform parent, string name, string text, float size, Color color, bool bold,
                                            TextAlignmentOptions alignment, Vector2 min, Vector2 max)
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
        return tmp;
    }

    private static void SetRef(Object target, string field, Object value)
    {
        SerializedObject so = new SerializedObject(target);
        SerializedProperty property = so.FindProperty(field);
        if (property == null)
        {
            Debug.LogError("[ScatterOverlay] Field '" + field + "' was not found on " + target.GetType().Name + ".");
            return;
        }
        property.objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}