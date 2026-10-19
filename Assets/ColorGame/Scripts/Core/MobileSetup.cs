using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using ColorGame.DevTools;
using ColorGame.UI;

namespace ColorGame.Mobile
{
    /// <summary>
    /// Small phone-specific behaviors in one place:
    ///  - frame rate and "keep the screen awake"
    ///  - the Android Back button closes the History or Dev panel
    ///  - development-only objects (the DEV TEST button) are hidden in non-development builds
    /// It contains no game rules.
    /// </summary>
    public class MobileSetup : MonoBehaviour
    {
        [Header("Screen")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool keepScreenAwake = true;

        [Header("Back Button (optional)")]
        [SerializeField] private GameHistoryUI historyUI;
        [SerializeField] private DebugPanelUI debugPanel;

        [Header("Development-only objects")]
        [Tooltip("These objects are switched off unless this is the Editor or a Development Build.")]
        [SerializeField] private GameObject[] devOnlyObjects;
        [Tooltip("Tick to see how a release build looks while still in the Editor.")]
        [SerializeField] private bool previewReleaseBuild = false;

        private void OnValidate()
        {
            targetFrameRate = Mathf.Clamp(targetFrameRate, 30, 120);
        }

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;

            if (keepScreenAwake)
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            // Debug.isDebugBuild is true in the Editor and in Development Builds only.
            bool hideDevObjects = previewReleaseBuild || !Debug.isDebugBuild;

            if (hideDevObjects && devOnlyObjects != null)
            {
                foreach (GameObject devObject in devOnlyObjects)
                {
                    if (devObject != null) devObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (!BackPressed()) return;

            // Close the top-most panel first.
            if (debugPanel != null && debugPanel.IsOpen)
            {
                debugPanel.Close();
            }
            else if (historyUI != null && historyUI.IsOpen)
            {
                historyUI.Close();
            }
        }

        // The Android Back button arrives as the Escape key.
        private static bool BackPressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Escape);
#else
            return false;
#endif
        }
    }
}