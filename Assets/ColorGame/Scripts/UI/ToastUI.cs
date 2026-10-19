using System.Collections;
using TMPro;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Shows short messages such as "INSUFFICIENT CREDITS" that fade in and out.
    /// Uses a CanvasGroup (alpha) instead of SetActive so the coroutine always works.
    /// </summary>
    public class ToastUI : MonoBehaviour
    {
        [SerializeField] private BetManager betManager;
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.2f;

        private Coroutine activeRoutine;
        private bool isReady;

        private void Awake()
        {
            if (betManager == null || config == null || messageText == null || canvasGroup == null)
            {
                Debug.LogError("[ToastUI] A reference is missing in the Inspector.", this);
                return;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false; // a toast must never block button taps
            canvasGroup.interactable = false;
            isReady = true;
        }

        private void OnEnable()
        {
            if (isReady) betManager.OnBetRejected += Show;
        }

        private void OnDisable()
        {
            if (betManager != null) betManager.OnBetRejected -= Show;
        }

        public void Show(string message)
        {
            if (!isReady) return;

            if (activeRoutine != null) StopCoroutine(activeRoutine);
            activeRoutine = StartCoroutine(ShowRoutine(message));
        }

        private IEnumerator ShowRoutine(string message)
        {
            messageText.text = message;

            yield return Fade(canvasGroup.alpha, 1f);
            yield return new WaitForSeconds(config.ToastDuration);
            yield return Fade(1f, 0f);

            activeRoutine = null;
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }
}