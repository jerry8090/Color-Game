
using UnityEngine;
using UnityEngine.UI;
using Shared.Audio;

namespace ColorGame.UI
{
    /// <summary>
    /// One button that turns all sound (music + effects) on or off.
    /// Uses sprites instead of TMP text.
    /// </summary>
    public class AudioToggleUI : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Button button;

        [Header("Sprite Settings")]
        [SerializeField] private Image soundImage;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;

        private bool isReady;

        private void Awake()
        {
            if (audioManager == null ||
                button == null ||
                soundImage == null ||
                soundOnSprite == null ||
                soundOffSprite == null)
            {
                Debug.LogError(
                    "[AudioToggleUI] A reference is missing in the Inspector.",
                    this
                );
                return;
            }

            button.onClick.RemoveListener(HandleClicked);
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
            if (audioManager != null)
                audioManager.OnSettingsChanged -= Refresh;
        }

        private void HandleClicked()
        {
            bool turnOn = !audioManager.IsSoundOn;

            audioManager.SetSoundOn(turnOn);

            // Play click sound only when sound is turned ON.
            if (turnOn)
                audioManager.PlaySfx(SfxType.ButtonClick);
        }

        private void Refresh()
        {
            soundImage.sprite = audioManager.IsSoundOn
                ? soundOnSprite
                : soundOffSprite;
        }
    }
}
