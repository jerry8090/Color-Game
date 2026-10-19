using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;

namespace ColorGame.UI
{
    /// <summary>
    /// One color button. Display and click only: it never decides anything.
    /// </summary>
    public class ColorButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image background;
        [SerializeField] private Outline selectionOutline;
        [SerializeField] private TMP_Text label;
        [Tooltip("Optional. Leave empty if you have no icons.")]
        [SerializeField] private Image iconImage;

        [Header("Selected Look")]
        [SerializeField] private Color selectedOutlineColor = new Color(1f, 0.84f, 0.29f, 1f); // gold
        [SerializeField] private Vector2 selectedOutlineDistance = new Vector2(8f, 8f);
        [SerializeField] private float selectedScale = 1.08f;

        /// <summary>Raised with this button's color when it is clicked.</summary>
        public event Action<GameColor> OnClicked;

        public GameColor AssignedColor { get; private set; }

        public void Setup(ColorDefinition definition)
        {
            if (button == null || background == null || selectionOutline == null || label == null)
            {
                Debug.LogError("[ColorButtonView] A reference is missing on the ColorButton prefab.", this);
                return;
            }

            AssignedColor = definition.color;
            background.color = definition.uiColor;

            label.text = definition.displayName.ToUpperInvariant();

            // Dark text on bright colors (like yellow), white text on dark colors.
            //float brightness = 0.299f * definition.uiColor.r 
            //                 + 0.587f * definition.uiColor.g 
            //                 + 0.114f * definition.uiColor.b;
            //label.color = brightness > 0.7f ? new Color(0.15f, 0.1f, 0.2f) : Color.white;
            
             // Always use black text on every color button.
              label.color = Color.black;

            if (iconImage != null)
            {
                iconImage.sprite = definition.icon;
                iconImage.enabled = definition.icon != null;
            }

            selectionOutline.effectColor = selectedOutlineColor;
            selectionOutline.effectDistance = selectedOutlineDistance;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClicked?.Invoke(AssignedColor));

            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            selectionOutline.enabled = selected;
            transform.localScale = selected ? Vector3.one * selectedScale : Vector3.one;
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
}