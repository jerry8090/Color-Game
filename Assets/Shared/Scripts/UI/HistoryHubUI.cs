using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;

namespace Shared.UI
{
    /// <summary>
    /// The HISTORY screen of the main menu: one button per game.
    /// The COLOR GAME button is the Open Button of the Color Game history panel (GameHistoryUI), so that panel opens
    /// by itself and shows over this one. The SCATTER button stays disabled (COMING SOON) until a Scatter history
    /// screen exists: then set that screen's Open Button to this button and enable it.
    /// This script only shows and hides the chooser. Put it on an always-active object, NOT on the hub panel.
    /// </summary>
    public class HistoryHubUI : MonoBehaviour, IBackNavigable
    {
        [SerializeField] private Button openButton;
        [SerializeField] private GameObject hubPanel;
        [SerializeField] private Button backButton;

        [Header("Optional")]
        [SerializeField] private AudioManager audioManager;

        private bool isReady;

        public bool IsOpen => hubPanel != null && hubPanel.activeSelf;

        private void Awake()
        {
            if (openButton == null || hubPanel == null || backButton == null)
            {
                Debug.LogError("[HistoryHubUI] A reference is missing in the Inspector " +
                               "(Open Button, Hub Panel, Back Button).", this);
                return;
            }

            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(Open);
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(Close);

            hubPanel.SetActive(false);
            isReady = true;
        }

        public void Open()
        {
            if (!isReady || IsOpen) return;
            Click();
            hubPanel.SetActive(true);
        }

        public void Close()
        {
            if (!isReady || !IsOpen) return;
            Click();
            hubPanel.SetActive(false);
        }

        private void Click()
        {
            if (audioManager != null) audioManager.PlaySfx(SfxType.ButtonClick);
        }
    }
}
