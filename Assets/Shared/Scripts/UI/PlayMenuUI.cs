using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Shared.Audio;
using Shared.Save;
using ColorGame.DevTools;
using ColorGame.Gameplay;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ColorGame.UI
{
    /// <summary>
    /// The small MENU button on the play screen and its panel (SOUND, DEV TEST, MAIN MENU).
    /// History now lives in the main menu, so History UI is optional here.
    /// MAIN MENU is available at any time, except while a bet is in play (leaving then would lose the bet).
    /// Android Back: closes the menu panel, otherwise returns to the main menu (same rule).
    /// Put this on an always-active object, NOT on the menu panel itself.
    /// </summary>
    public class PlayMenuUI : MonoBehaviour
    {
        [Header("Required")]
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private Button menuButton;
        [SerializeField] private GameObject menuPanel;
        [Tooltip("The dark full-screen button behind the card. Tapping it closes the menu.")]
        [SerializeField] private Button backdropButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        [Header("Optional")]
        [Tooltip("Leave empty: the history screen is in the main menu now.")]
        [SerializeField] private MonoBehaviour historyUI;
        [SerializeField] private DebugPanelUI debugPanel;
        [SerializeField] private AudioManager audioManager;

        private TMP_Text mainMenuLabel;
        private string mainMenuText = "MAIN MENU";
        private bool isReady;
        private bool isLeaving;
        private bool panelsOpenLastFrame;

        private void Awake()
        {
            List<string> missing = new List<string>();
            if (gameManager == null) missing.Add("Game Manager");
            if (saveManager == null) missing.Add("Save Manager");
            if (menuButton == null) missing.Add("Menu Button");
            if (menuPanel == null) missing.Add("Menu Panel");
            if (backdropButton == null) missing.Add("Backdrop Button");
            if (mainMenuButton == null) missing.Add("Main Menu Button");

            if (missing.Count > 0)
            {
                Debug.LogError("[PlayMenuUI] Missing in the Inspector: " + string.Join(", ", missing), this);
                return;
            }

            mainMenuLabel = mainMenuButton.GetComponentInChildren<TMP_Text>(true);
            if (mainMenuLabel != null) mainMenuText = mainMenuLabel.text;

            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(ToggleMenu);
            backdropButton.onClick.RemoveAllListeners();
            backdropButton.onClick.AddListener(() => CloseMenu(true));
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);

            menuPanel.SetActive(false);
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;
            if (debugPanel != null) debugPanel.OnPanelToggled += HandlePanelToggled;
        }

        private void OnDisable()
        {
            if (debugPanel != null) debugPanel.OnPanelToggled -= HandlePanelToggled;
        }

        private void Update()
        {
            if (isReady && menuPanel.activeSelf) RefreshMainMenuButton();
        }

        private void LateUpdate()
        {
            if (!isReady || isLeaving) return;

            // LateUpdate runs after every Update. If the Dev panel was open at the end of the last frame,
            // MobileSetup has just used this Back press to close it, so we ignore it.
            if (BackPressed() && !panelsOpenLastFrame)
            {
                if (menuPanel.activeSelf) CloseMenu(true);
                else GoToMainMenu();
            }

            panelsOpenLastFrame = debugPanel != null && debugPanel.IsOpen;
        }

        private void ToggleMenu()
        {
            if (!isReady || isLeaving) return;

            if (menuPanel.activeSelf)
            {
                CloseMenu(true);
                return;
            }

            Click();
            RefreshMainMenuButton();
            menuPanel.SetActive(true);
        }

        private void CloseMenu(bool withSound)
        {
            if (!menuPanel.activeSelf) return;
            if (withSound) Click();
            menuPanel.SetActive(false);
        }

        private void HandlePanelToggled(bool isOpen)
        {
            if (isOpen) CloseMenu(false); // the Dev panel opened from the menu: close the menu behind it
        }

        // While a bet is in play the button shows why it cannot be used.
        private void RefreshMainMenuButton()
        {
            bool canLeave = !gameManager.HasActiveWager;
            mainMenuButton.interactable = canLeave;
            if (mainMenuLabel != null) mainMenuLabel.text = canLeave ? mainMenuText : "FINISHING ROUND...";
        }

        private void GoToMainMenu()
        {
            if (!isReady || isLeaving) return;

            // A spent bet belongs to a running round. Wait for it to finish.
            if (gameManager.HasActiveWager) return;

            if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                Debug.LogError($"[PlayMenuUI] Scene '{mainMenuSceneName}' is not in the build list " +
                               "(File > Build Profiles > Scene List).", this);
                return;
            }

            isLeaving = true;
            Click();
            saveManager.SaveGame();
            SceneManager.LoadScene(mainMenuSceneName);
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
