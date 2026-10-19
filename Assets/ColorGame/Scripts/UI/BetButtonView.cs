using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI
{
    /// <summary>
    /// One bet button. Display and click only.
    /// "Unaffordable" buttons are dimmed but still clickable, so tapping one can show "INSUFFICIENT CREDITS".
    /// "Not interactable" (during a round) fully disables the button.
    /// </summary>
    public class BetButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Outline selectionOutline;
        [SerializeField] private TMP_Text label;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Look")]
        [SerializeField] private Color selectedOutlineColor = new Color(1f, 0.84f, 0.29f, 1f); // gold
        [SerializeField] private Vector2 selectedOutlineDistance = new Vector2(6f, 6f);
        [SerializeField] private float selectedScale = 1.08f;
        [SerializeField] private float unaffordableAlpha = 0.35f;

        /// <summary>Raised with this button's bet amount when it is clicked.</summary>
        public event Action<int> OnClicked;

        public int Amount { get; private set; }

        public void Setup(int amount)
        {
            if (button == null || selectionOutline == null || label == null || canvasGroup == null)
            {
                Debug.LogError("[BetButtonView] A reference is missing on the BetButton prefab.", this);
                return;
            }

            Amount = amount;
            label.text = amount.ToString("N0", CultureInfo.InvariantCulture);

            selectionOutline.effectColor = selectedOutlineColor;
            selectionOutline.effectDistance = selectedOutlineDistance;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClicked?.Invoke(Amount));

            SetSelected(false);
            SetAffordable(true);
        }

        public void SetSelected(bool selected)
        {
            selectionOutline.enabled = selected;
            transform.localScale = selected ? Vector3.one * selectedScale : Vector3.one;
        }

        public void SetAffordable(bool affordable)
        {
            canvasGroup.alpha = affordable ? 1f : unaffordableAlpha;
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
}