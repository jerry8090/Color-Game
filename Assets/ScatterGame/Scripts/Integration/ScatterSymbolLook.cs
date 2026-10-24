using System;
using UnityEngine;
using ScatterGame.Core;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Label and colour of a symbol while it has no sprite. When a sprite is assigned in the settings asset,
    /// the view uses the sprite and this only supplies the golden marker.
    /// </summary>
    public static class ScatterSymbolLook
    {
        private static readonly Color[] RegularColors =
        {
            new Color32(30, 136, 229, 255),
            new Color32(67, 160, 71, 255),
            new Color32(251, 140, 0, 255),
            new Color32(0, 172, 193, 255),
            new Color32(216, 27, 96, 255)
        };

        private static readonly Color WildColor = new Color32(142, 36, 170, 255);
        private static readonly Color ScatterColor = new Color32(183, 28, 28, 255);
        private static readonly Color GoldenColor = new Color32(255, 193, 7, 255);

        public static readonly Color DarkText = new Color32(42, 27, 61, 255);

        public static bool IsGolden(ScatterGameSettings settings, int symbolId)
        {
            SymbolEntry entry = settings != null ? settings.GetEntry(symbolId) : null;
            return entry != null && entry.goldenOfSymbolId >= 0;
        }

        public static string Label(ScatterGameSettings settings, int symbolId)
        {
            SymbolEntry entry = settings != null ? settings.GetEntry(symbolId) : null;
            if (entry == null) return symbolId.ToString();
            if (entry.category == SymbolCategory.Wild) return "WILD";
            if (entry.category == SymbolCategory.Scatter) return "SCAT";

            SymbolEntry baseEntry = entry;
            if (entry.goldenOfSymbolId >= 0)
            {
                SymbolEntry found = settings.GetEntry(entry.goldenOfSymbolId);
                if (found != null) baseEntry = found;
            }

            string name = baseEntry.displayName;
            if (string.IsNullOrEmpty(name)) return baseEntry.id.ToString();
            if (string.Equals(name, "Ten", StringComparison.OrdinalIgnoreCase)) return "10";
            return name.Substring(0, 1).ToUpperInvariant();
        }

        public static Color Tint(ScatterGameSettings settings, int symbolId)
        {
            SymbolEntry entry = settings != null ? settings.GetEntry(symbolId) : null;
            if (entry == null) return Color.gray;
            if (entry.category == SymbolCategory.Wild) return WildColor;
            if (entry.category == SymbolCategory.Scatter) return ScatterColor;
            if (entry.goldenOfSymbolId >= 0) return GoldenColor;
            return RegularColors[Mathf.Abs(entry.id) % RegularColors.Length];
        }

        public static Color TextColor(ScatterGameSettings settings, int symbolId)
        {
            return IsGolden(settings, symbolId) ? DarkText : Color.white;
        }
    }
}