using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;

namespace ColorGame.UI
{
    /// <summary>
    /// One button that turns all sound (music + effects) on or off. The choice is saved.
    /// </summary>
    public class AudioToggleUI : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        private bool isReady;

        private void Awake()
        {
            if (audioManager == null || button == null || label == null)
            {
                Debug.LogError("[AudioToggleUI] A reference is missing in the Inspector.", this);
                return;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClicked);
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;
            audioManager.OnSettingsChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (audioManager != null) audioManager.OnSettingsChanged -= Refresh;
        }

        private void HandleClicked()
        {
            bool turnOn = !audioManager.IsSoundOn;
            audioManager.SetSoundOn(turnOn);

            // Confirm with a click, but only when sound has just been turned ON.
            if (turnOn) audioManager.PlaySfx(SfxType.ButtonClick);
        }

        private void Refresh()
        {
            label.text = audioManager.IsSoundOn ? "SOUND: ON" : "SOUND: OFF";
        }
    }
}