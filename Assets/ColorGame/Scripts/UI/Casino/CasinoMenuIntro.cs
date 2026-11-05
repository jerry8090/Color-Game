using System.Collections;
using UnityEngine;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Main-menu entrance: the listed items (logo/title first, then buttons) fade and scale in one after another.
    /// Positions and sizes are never changed. Items get a CanvasGroup if they have none.
    /// While an item is still invisible its CanvasGroup is non-interactive, so you cannot tap an invisible button.
    /// </summary>
    public class CasinoMenuIntro : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [Tooltip("In reveal order: Title, Subtitle, then the buttons.")]
        [SerializeField] private RectTransform[] items;
        [SerializeField] private float initialDelay = 0.1f;
        [SerializeField] private float stagger = 0.09f;
        [SerializeField] private float itemDuration = 0.35f;
        [SerializeField, Range(0.5f, 1f)] private float startScale = 0.9f;

        private CanvasGroup[] groups;
        private Vector3[] baseScales;
        private bool[] wasInteractable;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (controller == null || items == null || items.Length == 0)
            {
                Debug.LogError("[CasinoMenuIntro] Controller or Items missing in the Inspector.", this);
                return;
            }

            groups = new CanvasGroup[items.Length];
            baseScales = new Vector3[items.Length];
            wasInteractable = new bool[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null) continue;
                CanvasGroup g = items[i].GetComponent<CanvasGroup>();
                if (g == null) g = items[i].gameObject.AddComponent<CanvasGroup>();
                groups[i] = g;
                baseScales[i] = items[i].localScale;
                wasInteractable[i] = g.interactable;
            }

            ready = true;
        }

        private void OnEnable()
        {
            if (ready) routine = StartCoroutine(Play());
        }

        private void OnDisable()
        {
            routine = null;
            if (ready) Finish();
        }

        private IEnumerator Play()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (groups[i] == null) continue;
                groups[i].alpha = 0f;
                groups[i].interactable = false;
                items[i].localScale = baseScales[i] * startScale;
            }

            float elapsed = 0f;
            float total = initialDelay + stagger * items.Length + itemDuration;

            while (elapsed < total)
            {
                elapsed += Time.unscaledDeltaTime;

                for (int i = 0; i < items.Length; i++)
                {
                    if (groups[i] == null) continue;
                    float k = Mathf.Clamp01((elapsed - initialDelay - i * stagger) / Mathf.Max(0.05f, itemDuration));
                    groups[i].alpha = CasinoUIAnimationController.EaseOutCubic(k);
                    items[i].localScale = baseScales[i] * Mathf.LerpUnclamped(startScale, 1f, CasinoUIAnimationController.EaseOutBack(k));
                    if (k >= 1f) groups[i].interactable = wasInteractable[i];
                }

                yield return null;
            }

            Finish();
            routine = null;
        }

        private void Finish()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (groups[i] == null) continue;
                groups[i].alpha = 1f;
                groups[i].interactable = wasInteractable[i];
                items[i].localScale = baseScales[i];
            }
        }
    }
}