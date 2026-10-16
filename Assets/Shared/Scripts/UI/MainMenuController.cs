using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Shared.Audio;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Shared.UI
{
    /// <summary>One game on the main menu: what to show and which scene to open.</summary>
    [Serializable]
    public class MenuGameEntry
    {
        public string displayName = "GAME";
        [Tooltip("The scene FILE name (for example ColorGame). The scene must be in File > Build Profiles > Scene List.")]
        public string sceneName;
        public Button button;
        public TMP_Text label;
    }

    /// <summary>
    /// The main menu. It only navigates: it opens a game scene, shows settings, or quits.
    /// A game whose scene is empty or missing from the build list is shown as COMING SOON and cannot be pressed.
    /// Each scene has its own managers, so nothing is duplicated when scenes change (a scene load destroys the old one).
    /// Balance, history and sound settings are shared through the single save file.
    /// Overlay panels on this scene (for example History) open themselves; this script only lets Back close them.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MenuGameEntry[] games;

        [Header("Settings and quit")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button settingsCloseButton;
        [SerializeField] private Button quitButton;

        [Header("Optional")]
        [Tooltip("Plays a click on every menu button.")]
        [SerializeField] private AudioManager audioManager;
        [Tooltip("Panels that the Back button should close before anything else (for example the History panel). " +
                 "Each one must be a script that implements IBackNavigable.")]
        [SerializeField] private MonoBehaviour[] backPanels;

        private readonly List<IBackNavigable> backNavigables = new List<IBackNavigable>();
        private bool isLoading;
        private bool isReady;

        private void Awake()
        {
            if (games == null || settingsButton == null || settingsPanel == null ||
                settingsCloseButton == null || quitButton == null)
            {
                Debug.LogError("[MainMenuController] A reference is missing in the Inspector.", this);
                return;
            }

            backNavigables.Clear();
            if (backPanels != null)
            {
                foreach (MonoBehaviour panel in backPanels)
                {
                    if (panel == null) continue;

                    if (panel is IBackNavigable navigable) backNavigables.Add(navigable);
                    else Debug.LogError($"[MainMenuController] '{panel.name}' does not implement IBackNavigable.", this);
                }
            }

            foreach (MenuGameEntry entry in games)
            {
                if (entry == null || entry.button == null || entry.label == null)
                {
                    Debug.LogError("[MainMenuController] A game entry is missing its Button or Label.", this);
                    continue;
                }

                MenuGameEntry captured = entry;
                bool available = !string.IsNullOrWhiteSpace(captured.sceneName) &&
                                 Application.CanStreamedLevelBeLoaded(captured.sceneName);

                captured.label.text = available
                    ? captured.displayName
                    : captured.displayName + "\n<size=45%>COMING SOON</size>";
                captured.button.interactable = available;

                if (!available && !string.IsNullOrWhiteSpace(captured.sceneName))
                {
                    Debug.LogWarning($"[MainMenuController] Scene '{captured.sceneName}' is not in the build list. " +
                                     $"'{captured.displayName}' is shown as COMING SOON.", this);
                }

                captured.button.onClick.RemoveAllListeners();
                captured.button.onClick.AddListener(() => OpenGame(captured.sceneName));
            }

            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(() => SetSettingsOpen(true));
            settingsCloseButton.onClick.RemoveAllListeners();
            settingsCloseButton.onClick.AddListener(() => SetSettingsOpen(false));
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(Quit);

            settingsPanel.SetActive(false);
            isReady = true;
        }

        private void Update()
        {
            if (!isReady || isLoading || !BackPressed()) return;

            // Android Back: close an open overlay panel (History) first.
            foreach (IBackNavigable panel in backNavigables)
            {
                if (panel.IsOpen)
                {
                    panel.Close();
                    return;
                }
            }

            // Then settings, otherwise leave the app.
            if (settingsPanel.activeSelf) SetSettingsOpen(false);
            else Quit();
        }

        private void OpenGame(string sceneName)
        {
            if (isLoading) return;

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"[MainMenuController] Scene '{sceneName}' cannot be loaded. Add it to the build list.", this);
                return;
            }

            isLoading = true;
            Click();
            SceneManager.LoadScene(sceneName); // single mode: the menu scene is destroyed, nothing is duplicated
        }

        private void SetSettingsOpen(bool open)
        {
            if (isLoading) return;
            Click();
            settingsPanel.SetActive(open);
        }

        private void Quit()
        {
            if (isLoading) return;
            Click();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Click()
        {
            if (audioManager != null) audioManager.PlaySfx(SfxType.ButtonClick);
        }

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