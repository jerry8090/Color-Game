using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RouletteGame.Data;

namespace RouletteGame.UI
{
    /// <summary>
    /// The row of chips. The amounts and colors come from RouletteSettings, so changing the asset changes the chips.
    /// The selected chip stays selected while the player taps the table. Chips dim when the free credits are too low.
    /// </summary>
    public class ChipSelectorUI : MonoBehaviour
    {
        [SerializeField] private RouletteSettings settings;
        [Tooltip("The chips are created inside this rectangle (give it a Horizontal Layout Group).")]
        [SerializeField] private RectTransform container;
        [SerializeField] private Sprite chipSprite;
        [SerializeField] private TMP_FontAsset font;
        [SerializeField] private float chipSize = 92f;
        [SerializeField] private float selectedScale = 1.12f;
        [SerializeField, Range(0f, 1f)] private float unaffordableAlpha = 0.35f;

        /// <summary>Raised with the chip amount when the player picks another chip.</summary>
        public event Action<long> OnSelectedChanged;

        private sealed class Chip
        {
            public long amount;
            public Button button;
            public CanvasGroup group;
            public Outline outline;
            public RectTransform rect;
        }

        private readonly List<Chip> chips = new List<Chip>();
        private int selectedIndex;
        private bool isReady;

        public long SelectedAmount => isReady ? chips[selectedIndex].amount : 0;

        private void Awake()
        {
            if (settings == null || container == null || settings.ChipAmounts.Count == 0)
            {
                Debug.LogError("[ChipSelectorUI] Settings (with at least one chip) or Container is missing in the Inspector.", this);
                return;
            }

            for (int i = 0; i < settings.ChipAmounts.Count; i++) chips.Add(CreateChip(i, settings.ChipAmounts[i]));

            selectedIndex = Mathf.Clamp(settings.DefaultChipIndex, 0, chips.Count - 1);
            isReady = true;
            ApplySelection();
        }

        public void Select(int index)
        {
            if (!isReady || index < 0 || index >= chips.Count || index == selectedIndex) return;
            selectedIndex = index;
            ApplySelection();
            OnSelectedChanged?.Invoke(SelectedAmount);
        }

        /// <summary>Dims the chips the player cannot afford right now (free credits = balance minus the bets on the table).</summary>
        public void Refresh(long freeCredits)
        {
            if (!isReady) return;
            foreach (Chip chip in chips) chip.group.alpha = chip.amount <= freeCredits ? 1f : unaffordableAlpha;
        }

        public void SetInteractable(bool interactable)
        {
            if (!isReady) return;
            foreach (Chip chip in chips) chip.button.interactable = interactable;
        }

        private Chip CreateChip(int index, int amount)
        {
            GameObject go = new GameObject("Chip_" + amount, typeof(RectTransform), typeof(Image), typeof(Button), typeof(CanvasGroup), typeof(Outline));
            go.transform.SetParent(container, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.sizeDelta = new Vector2(chipSize, chipSize);

            Color color = settings.ChipColor(index);
            Image image = go.GetComponent<Image>();
            image.sprite = chipSprite;
            image.type = chipSprite != null ? Image.Type.Sliced : Image.Type.Simple;
            image.color = color;

            Button button = go.GetComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.None;
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;

            Outline outline = go.GetComponent<Outline>();
            outline.effectColor = RouletteUi.Gold;
            outline.effectDistance = new Vector2(5f, 5f);
            outline.enabled = false;

            RouletteUi.MakeText(go.transform, "Amount", amount.ToString("N0", CultureInfo.InvariantCulture), 34f, RouletteUi.TextOn(color), font);

            int captured = index;
            button.onClick.AddListener(() => Select(captured));

            return new Chip { amount = amount, button = button, group = go.GetComponent<CanvasGroup>(), outline = outline, rect = rect };
        }

        private void ApplySelection()
        {
            for (int i = 0; i < chips.Count; i++)
            {
                bool selected = i == selectedIndex;
                chips[i].outline.enabled = selected;
                chips[i].rect.localScale = Vector3.one * (selected ? selectedScale : 1f);
            }
        }
    }
}
