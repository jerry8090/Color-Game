
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI
{
    /// <summary>
    /// One bet button.
    /// Displays a denomination-specific sprite instead of a plain color.
    /// Handles selection, affordability, and interactability.
    /// </summary>
    public class BetButtonView : MonoBehaviour
    {
        [Header("Button References")]
        [SerializeField] private Button button;
        [SerializeField] private Image chipImage;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Outline selectionOutline;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Denomination Sprites")]
        [SerializeField] private Sprite chip1;
        [SerializeField] private Sprite chip5;
        [SerializeField] private Sprite chip20;
        [SerializeField] private Sprite chip50;
        [SerializeField] private Sprite chip200;
        [SerializeField] private Sprite chip500;

        [Header("Label Options")]
        [Tooltip("Enable if the denomination number is already inside the sprite.")]
        [SerializeField] private bool numberIsInSprite = false;

        [Header("Selection Look")]
        [SerializeField]
        private Color selectedOutlineColor =
            new Color(1f, 0.84f, 0.29f, 1f);

        [SerializeField]
        private Vector2 selectedOutlineDistance =
            new Vector2(6f, 6f);

        [SerializeField] private float selectedScale = 1.08f;

        [Header("Affordability")]
        [SerializeField, Range(0f, 1f)]
        private float unaffordableAlpha = 0.35f;

        public event Action<int> OnClicked;

        public int Amount { get; private set; }

        private Vector3 originalScale = Vector3.one;

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        public void Setup(int amount)
        {
            if (button == null ||
                chipImage == null ||
                canvasGroup == null ||
                selectionOutline == null)
            {
                Debug.LogError(
                    "[BetButtonView] Assign Button, Chip Image, " +
                    "Selection Outline, and Canvas Group in the prefab.",
                    this);

                return;
            }

            Amount = amount;

            // Display the amount as text unless the sprite contains it.
            if (label != null)
            {
                label.text = amount.ToString(
                    "N0", CultureInfo.InvariantCulture);

                label.gameObject.SetActive(!numberIsInSprite);
            }

            // Assign the correct image for this denomination.
            ApplyDenominationSprite(amount);

            // Configure the existing gold selection outline.
            selectionOutline.effectColor = selectedOutlineColor;
            selectionOutline.effectDistance = selectedOutlineDistance;
            selectionOutline.enabled = false;

            // Register the click event.
            button.onClick.RemoveListener(HandleButtonClicked);
            button.onClick.AddListener(HandleButtonClicked);

            SetSelected(false);
            SetAffordable(true);
        }

        private void ApplyDenominationSprite(int amount)
        {
            Sprite sprite = GetDenominationSprite(amount);

            if (sprite == null)
            {
                Debug.LogError(
                    $"[BetButtonView] No sprite assigned for " +
                    $"denomination {amount}. Check the prefab Inspector.",
                    this);

                chipImage.enabled = false;
                return;
            }

            // Display the sprite without applying a color tint.
            chipImage.sprite = sprite;
            chipImage.color = Color.white;
            chipImage.preserveAspect = true;
            chipImage.enabled = true;
        }

        private Sprite GetDenominationSprite(int amount)
        {
            switch (amount)
            {
                case 1:
                    return chip1;

                case 5:
                    return chip5;

                case 20:
                    return chip20;

                case 50:
                    return chip50;

                case 200:
                    return chip200;

                case 500:
                    return chip500;

                default:
                    return null;
            }
        }

        private void HandleButtonClicked()
        {
            OnClicked?.Invoke(Amount);
        }

        public void SetSelected(bool selected)
        {
            if (selectionOutline != null)
                selectionOutline.enabled = selected;

            transform.localScale = selected
                ? originalScale * selectedScale
                : originalScale;
        }

        public void SetAffordable(bool affordable)
        {
            if (canvasGroup != null)
                canvasGroup.alpha = affordable ? 1f : unaffordableAlpha;
        }

        public void SetInteractable(bool interactable)
        {
            if (button != null)
                button.interactable = interactable;
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(HandleButtonClicked);
        }
    }
}
