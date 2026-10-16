using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shared.UI;
using ColorGame.Gameplay;
using ColorGame.Machine;
using ColorGame.Sound;

/// <summary>
/// Menu: Tools > Color Game > Install Betting Cycle UI + Audio Links   (open the ColorGame scene first)
///  - creates the countdown text on top of the machine view and assigns it to AutoRollController
///  - fills the optional Machine and Auto Roll slots on ColorGameAudio
/// It does NOT switch Auto Roll on. Safe to run twice. Place in Assets/Editor/.
/// </summary>
public static class ColorGameBettingCycleInstaller
{
    private static readonly Color Gold = new Color32(255, 213, 74, 255);

    [MenuItem("Tools/Color Game/Install Betting Cycle UI + Audio Links")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[BettingCycle] Stop Play mode first.");
            return;
        }

        AutoRollController autoRoll = Object.FindFirstObjectByType<AutoRollController>(FindObjectsInactive.Include);
        ColorGameAudio audio = Object.FindFirstObjectByType<ColorGameAudio>(FindObjectsInactive.Include);
        Machine3DPresenter machine = Object.FindFirstObjectByType<Machine3DPresenter>(FindObjectsInactive.Include);

        if (autoRoll == null)
        {
            Debug.LogError("[BettingCycle] AutoRollController was not found. Open the ColorGame scene first.");
            return;
        }

        // ---- Countdown text ----
        SerializedObject autoObject = new SerializedObject(autoRoll);
        SerializedProperty textProperty = autoObject.FindProperty("countdownText");

        if (textProperty != null && textProperty.objectReferenceValue == null)
        {
            Transform parent = FindMachineView();
            if (parent == null)
            {
                SafeAreaFitter safe = Object.FindFirstObjectByType<SafeAreaFitter>(FindObjectsInactive.Include);
                parent = safe != null ? safe.transform : null;
            }

            if (parent == null)
            {
                Debug.LogError("[BettingCycle] Could not find MachineView or SafeAreaPanel for the countdown text.");
            }
            else
            {
                GameObject go = new GameObject("CountdownText", typeof(RectTransform), typeof(TextMeshProUGUI));
                go.transform.SetParent(parent, false);

                RectTransform rect = (RectTransform)go.transform;
                rect.anchorMin = new Vector2(0.1f, 0.86f);
                rect.anchorMax = new Vector2(0.9f, 0.98f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
                text.text = string.Empty;
                text.fontSize = 56f;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Center;
                text.color = Gold;
                text.enableAutoSizing = true;
                text.fontSizeMin = 20f;
                text.fontSizeMax = 56f;
                text.raycastTarget = false; // it must never block taps

                textProperty.objectReferenceValue = text;
                autoObject.ApplyModifiedPropertiesWithoutUndo();
                Debug.Log("[BettingCycle] Countdown text created under " + parent.name + " and assigned.");
            }
        }

        // ---- Audio links ----
        if (audio != null)
        {
            SerializedObject audioObject = new SerializedObject(audio);
            Link(audioObject, "autoRoll", autoRoll);
            if (machine != null) Link(audioObject, "machine", machine);
            audioObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(audio);
        }
        else
        {
            Debug.LogWarning("[BettingCycle] ColorGameAudio was not found. Assign Machine and Auto Roll on it by hand.");
        }

        EditorSceneManager.MarkSceneDirty(autoRoll.gameObject.scene);
        Debug.Log("[BettingCycle] Done. Save the scene (Ctrl+S). Auto Roll Enabled is still " +
                  (autoRoll.AutoRollEnabled ? "ON" : "OFF") + ".");
    }

    private static void Link(SerializedObject owner, string field, Object value)
    {
        SerializedProperty property = owner.FindProperty(field);
        if (property == null)
        {
            Debug.LogError($"[BettingCycle] Field '{field}' was not found on ColorGameAudio. Is the new ColorGameAudio.cs installed?");
            return;
        }
        property.objectReferenceValue = value;
    }

    private static Transform FindMachineView()
    {
        foreach (RectTransform rect in Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (rect.name == "MachineView") return rect;
        }
        return null;
    }
}
