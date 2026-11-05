using System;
using System.Collections;
using UnityEngine;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Fade + slight scale for a panel.
    ///  - Opening: plays automatically whenever the panel is activated (SetActive(true)) if Play On Enable is ticked.
    ///  - Closing: call PlayHide() yourself. Existing code that calls SetActive(false) still works, it just closes instantly.
    /// Fade Group = the whole panel. Scale Target = the card you want to grow (so a full-screen dim backdrop does not scale).
    /// Do NOT use on panels whose CanvasGroup alpha is driven by another script (GameHistoryUI): use PlayShow() from code there.
    /// </summary>
    public class CasinoPanelTransition : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [Tooltip("Optional. Leave empty to use (or add) a CanvasGroup on this object.")]
        [SerializeField] private CanvasGroup fadeGroup;
        [Tooltip("Optional. Leave empty to scale this object.")]
        [SerializeField] private RectTransform scaleTarget;
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool deactivateAfterHide = true;

        private Vector3 baseScale = Vector3.one;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (controller == null)
            {
                Debug.LogError("[CasinoPanelTransition] No CasinoUIAnimationController in the scene.", this);
                return;
            }

            if (fadeGroup == null) fadeGroup = GetComponent<CanvasGroup>();
            if (fadeGroup == null) fadeGroup = gameObject.AddComponent<CanvasGroup>();
            if (scaleTarget == null) scaleTarget = (RectTransform)transform;

            baseScale = scaleTarget.localScale;
            ready = true;
        }

        private void OnEnable()
        {
            if (ready && playOnEnable) PlayShow();
        }

        private void OnDisable()
        {
            routine = null;
            if (!ready) return;
            fadeGroup.alpha = 1f;
            scaleTarget.localScale = baseScale;
        }

        public void PlayShow() { Run(true, null); }
        public void PlayHide(Action onDone = null) { Run(false, onDone); }

        private void Run(bool show, Action onDone)
        {
            if (!ready || !isActiveAndEnabled)
            {
                onDone?.Invoke();
                return;
            }

            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Animate(show, onDone));
        }

        private IEnumerator Animate(bool show, Action onDone)
        {
            float duration = Mathf.Max(0.05f, controller.PanelFadeDuration);
            float small = controller.PanelStartScale;
            float fromAlpha = show ? 0f : fadeGroup.alpha;
            float toAlpha = show ? 1f : 0f;
            float fromScale = show ? small : 1f;
            float toScale = show ? 1f : small;
            float t = 0f;

            fadeGroup.alpha = fromAlpha;
            scaleTarget.localScale = baseScale * fromScale;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                float e = CasinoUIAnimationController.EaseOutCubic(t);
                fadeGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, e);
                scaleTarget.localScale = baseScale * Mathf.Lerp(fromScale, toScale, e);
                yield return null;
            }

            fadeGroup.alpha = toAlpha;
            scaleTarget.localScale = baseScale * toScale;
            routine = null;
            onDone?.Invoke();

            if (!show && deactivateAfterHide) gameObject.SetActive(false);
        }
    }
}