using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorGame.Core
{
    /// <summary>
    /// Describes how one color looks and is identified.
    /// </summary>
    [Serializable]
    public class ColorDefinition
    {
        public GameColor color;
        public string id;
        public string displayName;
        public Color uiColor = Color.white;
        [Tooltip("Optional icon. Leave empty for now.")]
        public Sprite icon;
    }

    /// <summary>
    /// All configurable Color Game settings in one asset.
    /// Create it with: right-click in Project > Create > Color Game > Color Game Config.
    /// Note: the starting balance lives on VirtualWalletManager because the wallet is shared with future games.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorGameConfig", menuName = "Color Game/Color Game Config")]
    public class ColorGameConfig : ScriptableObject
    {
        /// <summary>Number of result slots (dice) per round.</summary>
        public const int ResultSlotCount = 3;

        /// <summary>The most matches possible in one round.</summary>
        public const int MaxMatches = ResultSlotCount;

        [Header("Colors")]
        [SerializeField] private List<ColorDefinition> colors = CreateDefaultColors();

        [Header("Bets")]
        [Tooltip("Selectable bet amounts, shown left to right.")]
        [SerializeField] private List<int> betAmounts = new List<int> { 1, 5, 20, 50, 200, 500 };

        [Header("Payouts")]
        [Tooltip("Element N = multiplier when the selected color appears N times. " +
                 "Winnings = bet x multiplier (this is the TOTAL returned, stake included). Element 0 must stay 0.")]
        [SerializeField] private float[] payoutMultipliers = { 0f, 1f, 2f, 3f };

        [Header("Roll Animation Timing (seconds)")]
        [Tooltip("How often a spinning slot changes color.")]
        [SerializeField] private float spinSwapInterval = 0.07f;
        [SerializeField] private float slot1StopTime = 1.0f;
        [SerializeField] private float slot2StopTime = 1.6f;
        [SerializeField] private float slot3StopTime = 2.2f;
        [Tooltip("Pause after the last slot stops, before the payout is shown.")]
        [SerializeField] private float resultRevealDelay = 0.6f;

        [Header("UI")]
        [SerializeField] private float toastDuration = 1.5f;

        [Header("History")]
        [SerializeField] private int historyLimit = 100;

        public IReadOnlyList<ColorDefinition> Colors => colors;
        public IReadOnlyList<int> BetAmounts => betAmounts;

        public float SpinSwapInterval => spinSwapInterval;
        public float Slot1StopTime => slot1StopTime;
        public float Slot2StopTime => slot2StopTime;
        public float Slot3StopTime => slot3StopTime;
        public float ResultRevealDelay => resultRevealDelay;
        public float ToastDuration => toastDuration;
        public int HistoryLimit => historyLimit;

        /// <summary>Finds the definition (name, ID, UI color) for a color.</summary>
        public bool TryGetColorDefinition(GameColor color, out ColorDefinition definition)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i] != null && colors[i].color == color)
                {
                    definition = colors[i];
                    return true;
                }
            }

            Debug.LogError($"[ColorGameConfig] No ColorDefinition found for {color}.", this);
            definition = null;
            return false;
        }

        /// <summary>Multiplier for a number of matches (0 to 3). Used by PayoutCalculator in Phase 4.</summary>
        public float GetPayoutMultiplier(int matches)
        {
            int index = Mathf.Clamp(matches, 0, MaxMatches);
            return payoutMultipliers[index];
        }

        /// <summary>True if this amount is one of the configured bets.</summary>
        public bool IsValidBetAmount(int amount)
        {
            for (int i = 0; i < betAmounts.Count; i++)
            {
                if (betAmounts[i] == amount) return true;
            }
            return false;
        }

        private void OnValidate()
        {
            // Keep the payout table exactly 4 entries: 0, 1, 2, 3 matches.
            if (payoutMultipliers == null || payoutMultipliers.Length != MaxMatches + 1)
            {
                Array.Resize(ref payoutMultipliers, MaxMatches + 1);
            }

            payoutMultipliers[0] = 0f; // zero matches must never pay
            for (int i = 1; i < payoutMultipliers.Length; i++)
            {
                payoutMultipliers[i] = Mathf.Max(0f, payoutMultipliers[i]);
            }

            for (int i = 0; i < betAmounts.Count; i++)
            {
                betAmounts[i] = Mathf.Max(1, betAmounts[i]);
            }

            historyLimit = Mathf.Max(1, historyLimit);
            toastDuration = Mathf.Max(0.5f, toastDuration);
            spinSwapInterval = Mathf.Max(0.02f, spinSwapInterval);

            if (colors.Count != Enum.GetValues(typeof(GameColor)).Length)
            {
                Debug.LogWarning("[ColorGameConfig] The number of color definitions does not match the GameColor enum.", this);
            }
        }

                [ContextMenu("Restore Default Colors")]
        private void RestoreDefaultColors()
        {
            colors = CreateDefaultColors();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private static List<ColorDefinition> CreateDefaultColors()
        {
                        // Order = the order of the color buttons (matches the reference board).
            return new List<ColorDefinition>
            {
                MakeDefinition(GameColor.Yellow,  new Color32(250, 204, 21, 255)),
                MakeDefinition(GameColor.Purple,  new Color32(168, 85, 247, 255)),
                MakeDefinition(GameColor.Orange,  new Color32(251, 146, 60, 255)),
                MakeDefinition(GameColor.Blue,    new Color32(59, 130, 246, 255)),
                MakeDefinition(GameColor.Red,     new Color32(229, 75, 75, 255)),
                MakeDefinition(GameColor.Green,   new Color32(34, 197, 94, 255))
            };
        }

        private static ColorDefinition MakeDefinition(GameColor color, Color32 uiColor)
        {
            string name = color.ToString().ToUpperInvariant();
            return new ColorDefinition
            {
                color = color,
                id = name,
                displayName = name,
                uiColor = uiColor
            };
        }
    }
}