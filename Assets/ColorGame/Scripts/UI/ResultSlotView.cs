using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;

namespace ColorGame.UI
{
    /// <summary>
    /// One of the three result boxes. Display only.
    /// </summary>
    public class ResultSlotView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Outline highlightOutline;
        [SerializeField] private TMP_Text label;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Look")]
        [SerializeField] private Color unknownColor = new Color(0.23f, 0.23f, 0.29f, 1f);
        [SerializeField] private Color highlightColor = new Color(1f, 0.84f, 0.29f, 1f); // gold
        [SerializeField] private Vector2 highlightDistance = new Vector2(10f, 10f);
        [SerializeField] private float highlightedScale = 1.1f;
        [SerializeField] private float dimmedAlpha = 0.55f;
        [SerializeField] private float popScale = 1.15f;
        [SerializeField] private float popDuration = 0.18f;

        private float baseScale = 1f;
        private Coroutine popRoutine;
        private bool isReady;

        public void Init()
        {
            if (background == null || highlightOutline == null || label == null || canvasGroup == null)
            {
                Debug.LogError("[ResultSlotView] A reference is missing on the ResultSlot prefab.", this);
                return;
            }

            highlightOutline.effectColor = highlightColor;
            highlightOutline.effectDistance = highlightDistance;
            isReady = true;
            ShowUnknown();
        }

        /// <summary>Shows the "?" box used before the first roll.</summary>
        public void ShowUnknown()
        {
            if (!isReady) return;
            background.color = unknownColor;
            label.text = "?";
            label.color = Color.white;
            ClearHighlight();
        }

        /// <summary>Shows a color. Set pop = true for the little bounce when a slot stops.</summary>
        public void ShowColor(ColorDefinition definition, bool pop)
        {
            if (!isReady || definition == null) return;

            background.color = definition.uiColor;
            label.text = definition.displayName.ToUpperInvariant();

            // Dark text on bright colors (like yellow), white text on dark colors.
            float brightness = 0.299f * definition.uiColor.r + 0.587f * definition.uiColor.g + 0.114f * definition.uiColor.b;
            label.color = brightness > 0.7f ? new Color(0.15f, 0.1f, 0.2f) : Color.white;

            if (pop && gameObject.activeInHierarchy)
            {
                if (popRoutine != null) StopCoroutine(popRoutine);
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
            canvasGroup.alpha = dimmed ? dimmedAlpha : 1f;
        }

        public void ClearHighlight()
        {
            SetHighlighted(false);
            SetDimmed(false);
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