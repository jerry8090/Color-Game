using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;

namespace ColorGame.UI.RoyalGold
{
    /// <summary>
    /// One of the three colored result pills of the Royal Gold design. Display only:
    /// it never decides what is a win. Highlight/dim values come from the PayoutResult.
    /// </summary>
    public class RoyalResultSlotView : MonoBehaviour
    {
        [SerializeField] private Image fill;
        [Tooltip("Gold outline shown when this cube matched a bet color.")]
        [SerializeField] private Outline highlightOutline;
        [SerializeField] private TMP_Text label;
        [SerializeField] private CanvasGroup group;

        [Header("Look")]
        [SerializeField] private Color unknownColor = new Color(0.16f, 0.09f, 0.27f, 0.9f);
        [SerializeField] private Color highlightColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private Vector2 highlightDistance = new Vector2(6f, 6f);
        [SerializeField] private float highlightedScale = 1.08f;
        [SerializeField, Range(0f, 1f)] private float dimmedAlpha = 0.55f;
        [SerializeField] private float popScale = 1.15f;
        [SerializeField] private float popDuration = 0.18f;

        private float baseScale = 1f;
        private Coroutine popRoutine;
        private bool isReady;

        public void Init()
        {
            if (fill == null || highlightOutline == null || label == null || group == null)
            {
                Debug.LogError("[RoyalResultSlotView] A reference is missing on " + name + ".", this);
                return;
            }

            highlightOutline.effectColor = highlightColor;
            highlightOutline.effectDistance = highlightDistance;
            isReady = true;
            ShowUnknown();
        }

        public void ShowUnknown()
        {
            if (!isReady) return;
            StopPop();
            fill.color = unknownColor;
            label.text = "?";
            label.color = Color.white;
            ClearHighlight();
        }

        public void ShowColor(ColorDefinition definition, bool pop)
        {
            if (!isReady || definition == null) return;

            fill.color = definition.uiColor;
            label.text = definition.displayName.ToUpperInvariant();

            float brightness = 0.299f * definition.uiColor.r + 0.587f * definition.uiColor.g + 0.114f * definition.uiColor.b;
            label.color = brightness > 0.7f ? new Color(0.15f, 0.1f, 0.2f) : Color.white;

            if (pop && isActiveAndEnabled)
            {
                StopPop();
                popRoutine = StartCoroutine(PopRoutine());
            }
        }

        public void SetHighlighted(bool highlighted)
        {
            if (!isReady) return;
            StopPop();
            baseScale = highlighted ? highlightedScale : 1f;
            highlightOutline.enabled = highlighted;
            transform.localScale = Vector3.one * baseScale;
        }

        public void SetDimmed(bool dimmed)
        {
            if (!isReady) return;
            group.alpha = dimmed ? dimmedAlpha : 1f;
        }

        public void ClearHighlight()
        {
            SetHighlighted(false);
            SetDimmed(false);
        }

        private void OnDisable()
        {
            popRoutine = null;
        }

        private void StopPop()
        {
            if (popRoutine == null) return;
            StopCoroutine(popRoutine);
            popRoutine = null;
        }

        private IEnumerator PopRoutine()
        {
            float elapsed = 0f;
            while (elapsed < popDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / popDuration);
                transform.localScale = Vector3.one * Mathf.Lerp(popScale, baseScale, t);
                yield return null;
            }

            transform.localScale = Vector3.one * baseScale;
            popRoutine = null;
        }
    }
}
