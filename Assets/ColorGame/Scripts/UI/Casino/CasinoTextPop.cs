using System.Collections;
using TMPro;
using UnityEngine;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Pops a TMP text (scale + gold flash) whenever its text changes. Use on the ROUND text.
    /// It only compares two strings per frame in LateUpdate (no allocation), and ignores the first 0.5 s
    /// so the starting value does not pop.
    /// </summary>
    public class CasinoTextPop : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private TMP_Text text;

        private string last;
        private Color baseColor;
        private Vector3 baseScale = Vector3.one;
        private float startTime;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (text == null) text = GetComponent<TMP_Text>();
            if (controller == null || text == null)
            {
                Debug.LogError("[CasinoTextPop] Needs a CasinoUIAnimationController in the scene and a TMP_Text.", this);
                enabled = false;
                return;
            }

            baseScale = text.rectTransform.localScale;
            baseColor = text.color;
            last = text.text;
            ready = true;
        }

        private void OnEnable()
        {
            startTime = Time.unscaledTime;
        }

        private void OnDisable()
        {
            routine = null;
            if (!ready) return;
            text.rectTransform.localScale = baseScale;
            text.color = baseColor;
        }

        private void LateUpdate()
        {
            if (!ready || text.text == last) return;

            last = text.text;
            if (Time.unscaledTime - startTime < 0.5f) return;

            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Pop());
        }

        private IEnumerator Pop()
        {
            float duration = Mathf.Max(0.1f, controller.TextPopDuration);
            float peak = controller.TextPopScale;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                float k = Mathf.Clamp01(t);
                text.rectTransform.localScale = baseScale * Mathf.Lerp(1f, peak, Mathf.Sin(k * Mathf.PI));
                text.color = Color.Lerp(controller.Gold, baseColor, CasinoUIAnimationController.EaseOutCubic(k));
                yield return null;
            }

            text.rectTransform.localScale = baseScale;
            text.color = baseColor;
            routine = null;
        }
    }
}