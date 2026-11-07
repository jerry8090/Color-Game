using System.Collections;
using UnityEngine;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Staggered fade/slide/scale-in for EXISTING UI objects (for example the menu title and buttons).
    /// Adds a CanvasGroup if missing and restores the original position and scale afterwards.
    /// </summary>
    public class ScatterStaggerIn : MonoBehaviour
    {
        [SerializeField] private RectTransform[] items;
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private float initialDelay = 0.15f;
        [SerializeField] private float stagger = 0.08f;
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float slideY = -40f;
        [SerializeField, Range(0.8f, 1f)] private float startScale = 0.92f;

        private CanvasGroup[] groups;
        private Vector2[] basePos;
        private Vector3[] baseScale;
        private Coroutine routine;

        private void Awake()
        {
            int n = items != null ? items.Length : 0;
            groups = new CanvasGroup[n];
            basePos = new Vector2[n];
            baseScale = new Vector3[n];
            for (int i = 0; i < n; i++)
            {
                if (items[i] == null) continue;
                CanvasGroup g = items[i].GetComponent<CanvasGroup>();
                groups[i] = g != null ? g : items[i].gameObject.AddComponent<CanvasGroup>();
                basePos[i] = items[i].anchoredPosition;
                baseScale[i] = items[i].localScale;
            }
        }

        private void OnEnable() { if (playOnEnable) Play(); }

        private void OnDisable()
        {
            if (groups == null) return;
            for (int i = 0; i < groups.Length; i++) Restore(i);
            routine = null;
        }

        public void Play()
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Run());
        }

        private void Restore(int i)
        {
            if (items[i] == null) return;
            groups[i].alpha = 1f;
            items[i].anchoredPosition = basePos[i];
            items[i].localScale = baseScale[i];
        }

        private IEnumerator Run()
        {
            int n = items.Length;
            for (int i = 0; i < n; i++)
            {
                if (items[i] == null) continue;
                groups[i].alpha = 0f; // hidden immediately, before the first frame is drawn
            }

            float total = initialDelay + (n - 1) * stagger + duration;
            float t = 0f;
            while (t < total)
            {
                t += Time.unscaledDeltaTime;
                for (int i = 0; i < n; i++)
                {
                    if (items[i] == null) continue;
                    float k = Mathf.Clamp01((t - initialDelay - i * stagger) / Mathf.Max(0.05f, duration));
                    float e = ScatterFx.EaseOutCubic(k);
                    groups[i].alpha = k;
                    items[i].anchoredPosition = basePos[i] + new Vector2(0f, Mathf.Lerp(slideY, 0f, e));
                    float s = Mathf.LerpUnclamped(startScale, 1f, ScatterFx.EaseOutBack(k));
                    items[i].localScale = baseScale[i] * s;
                }
                yield return null;
            }

            for (int i = 0; i < n; i++) Restore(i);
            routine = null;
        }
    }
}