
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;

namespace ColorGame.UI
{
    /// <summary>
    /// Shared audio toggle for Color Game (Text)
    /// and Scatter Game (Sprites).
    /// </summary>
    public class AudioToggleUI : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button button;

        [Header("Text Display - Color Game")]
        [SerializeField] private TMP_Text label;

        [Header("Sprite Display - Scatter Game")]
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;

        private void Awake()
        {
            if (audioManager == null || button == null)
            {
                Debug.LogError(
                    "[AudioToggleUI] AudioManager or Button is missing.", this);
                enabled = false;
                return;
            }

            if (label == null && buttonImage == null)
            {
                Debug.LogError(
                    "[AudioToggleUI] Assign either a TMP label or Button Image.", this);
                enabled = false;
                return;
            }

            if (buttonImage != null &&
                (soundOnSprite == null || soundOffSprite == null))
            {
                Debug.LogError(
                    "[AudioToggleUI] Sound ON/OFF sprites are missing.", this);
                enabled = false;
                return;
            }

            button.onClick.AddListener(HandleClicked);
        }

        private void OnEnable()
        {
            if (audioManager == null) return;

            audioManager.OnSettingsChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (audioManager != null)
                audioManager.OnSettingsChanged -= Refresh;
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(HandleClicked);
        }

        private void HandleClicked()
        {
            bool turnOn = !audioManager.IsSoundOn;

            audioManager.SetSoundOn(turnOn);

            // Play confirmation only when sound is turned ON.
            if (turnOn)
                audioManager.PlaySfx(SfxType.ButtonClick);
        }

        private void Refresh()
        {
            bool soundOn = audioManager.IsSoundOn;

            // Color Game: Text display
            if (label != null)
            {
                label.text = soundOn ? "SOUND: ON" : "SOUND: OFF";
            }

            // Scatter Game: Sprite display
            if (buttonImage != null)
            {
                buttonImage.sprite = soundOn
                    ? soundOnSprite
                    : soundOffSprite;
            }
        }
    }
}
