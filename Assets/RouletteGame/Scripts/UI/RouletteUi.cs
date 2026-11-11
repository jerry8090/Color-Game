using TMPro;
using UnityEngine;
using RouletteGame.Core;

namespace RouletteGame.UI
{
    /// <summary>Small shared helpers for the Roulette views.</summary>
    public static class RouletteUi
    {
        public static readonly Color Gold = new Color32(255, 213, 74, 255);
        public static readonly Color Dark = new Color32(42, 27, 61, 255);

        public static Color ColorFor(PocketColor color)
        {
            switch (color)
            {
                case PocketColor.Green: return new Color32(0, 140, 70, 255);
                case PocketColor.Red: return new Color32(196, 30, 42, 255);
                default: return new Color32(30, 30, 38, 255);
            }
        }

        public static void Stretch(RectTransform rect, Vector2 offsetMin, Vector2 offsetMax)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
        }

        /// <summary>A text that fills its parent, auto-sizes down to fit, and never blocks taps.</summary>
        public static TextMeshProUGUI MakeText(Transform parent, string name, string text, float size, Color color, TMP_FontAsset font, bool bold = true)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            Stretch((RectTransform)go.transform, Vector2.zero, Vector2.zero);

            TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
            if (font != null) tmp.font = font;
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableAutoSizing = true;
            tmp.fontSizeMin = 10f;
            tmp.fontSizeMax = size;
            tmp.raycastTarget = false;
            return tmp;
        }

        /// <summary>Dark text on light colors, white text on dark colors.</summary>
        public static Color TextOn(Color background)
        {
            float brightness = 0.299f * background.r + 0.587f * background.g + 0.114f * background.b;
            return brightness > 0.6f ? Dark : Color.white;
        }
    }
}
