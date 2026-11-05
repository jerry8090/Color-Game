using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;

namespace ColorGame.UI
{
    /// <summary>
    /// One color betting area: fill, outline frame, highlight, the bills lying on it and its BET total.
    /// There is NO persistent selected look: the bills show where the bets are. A tap gives a short flash.
    /// The color name is hidden by default (the color itself identifies the area). The color is still known internally.
    /// </summary>
    public class ColorButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image background;
        [SerializeField] private Outline selectionOutline; // kept for old prefabs. Never shown.
        [SerializeField] private TMP_Text label;
        [Tooltip("Optional. Leave empty if you have no icons.")]
        [SerializeField] private Image iconImage;

        [Header("Look")]
        [SerializeField] private Image frame;
        [SerializeField] private Image highlight;
        [SerializeField] private Image tapFlash;
        [Tooltip("Untick = the color name is not shown on the betting area.")]
        [SerializeField] private bool showColorName = false;

        [Header("Bets on this color")]
        [SerializeField] private BillStackView billStack;
        [SerializeField] private TMP_Text betTotalLabel;

        // Old selected-look fields: kept so existing prefabs keep their serialized values.
        [SerializeField, HideInInspector] private Color selectedOutlineColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField, HideInInspector] private Vector2 selectedOutlineDistance = new Vector2(8f, 8f);
        //[SerializeField, HideInInspector] private float selectedScale = 1.08f;

        public event Action<GameColor> OnClicked;

        public GameColor AssignedColor { get; private set; }

        private Coroutine tapRoutine;

        public void Setup(ColorDefinition definition)
        {
            if (button == null || background == null || label == null)
            {
                Debug.LogError("[ColorButtonView] A reference is missing on the ColorButton prefab.", this);
                return;
            }

            AssignedColor = definition.color;
            background.color = definition.uiColor;

            label.text = showColorName ? definition.displayName.ToUpperInvariant() : string.Empty;
            label.enabled = showColorName;
            label.color = Color.black;

            if (iconImage != null)
            {
                iconImage.sprite = definition.icon;
                iconImage.enabled = definition.icon != null;
            }

            if (selectionOutline != null) selectionOutline.enabled = false;

            Derive(definition.uiColor, out Color outline, out Color light);
            if (frame != null) frame.color = outline;
            if (highlight != null) highlight.color = new Color(light.r, light.g, light.b, 0.9f);
            if (tapFlash != null) tapFlash.color = new Color(1f, 1f, 1f, 0f);

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClicked?.Invoke(AssignedColor));

            ClearBills();
        }

        /// <summary>No visual effect any more (the bills show the bet). Kept so old callers still compile.</summary>
        public void SetSelected(bool selected) { }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        /// <summary>A short flash and tiny bounce that is gone after 0.2 seconds. Not a selected state.</summary>
        public void PlayTapFeedback()
        {
            if (!isActiveAndEnabled) return;
            if (tapRoutine != null) StopCoroutine(tapRoutine);
            tapRoutine = StartCoroutine(TapRoutine());
        }

        public void AddBill(int denomination, int newTotal)
        {
            if (billStack != null) billStack.AddBill(denomination);
            SetBetTotal(newTotal);
        }

        public void SetBills(IReadOnlyList<int> denominations, int total)
        {
            if (billStack != null) billStack.SetBills(denominations);
            SetBetTotal(total);
        }

        public void ClearBills()
        {
            if (billStack != null) billStack.Clear();
            SetBetTotal(0);
        }

        private void OnDisable()
        {
            tapRoutine = null;
            transform.localScale = Vector3.one;
            if (tapFlash != null) tapFlash.color = new Color(1f, 1f, 1f, 0f);
        }

        private IEnumerator TapRoutine()
        {
            const float duration = 0.2f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                if (tapFlash != null) tapFlash.color = new Color(1f, 1f, 1f, 0.4f * (1f - t));
                transform.localScale = Vector3.one * (1f + 0.04f * Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            if (tapFlash != null) tapFlash.color = new Color(1f, 1f, 1f, 0f);
            transform.localScale = Vector3.one;
            tapRoutine = null;
        }

        private void SetBetTotal(int total)
        {
            if (betTotalLabel != null)
            {
                betTotalLabel.text = total > 0 ? "BET " + total.ToString("N0", CultureInfo.InvariantCulture) : string.Empty;
            }
        }

        // A darker, slightly warmer edge and a lighter highlight from the area's own color.
        private static void Derive(Color baseColor, out Color outline, out Color light)
        {
            Color.RGBToHSV(baseColor, out float h, out float s, out float v);
            outline = Color.HSVToRGB(Mathf.Repeat(h - 0.012f, 1f), Mathf.Clamp01(s * 1.1f + 0.05f), v * 0.5f);
            light = Color.Lerp(baseColor, Color.white, 0.6f);
        }
    }
}