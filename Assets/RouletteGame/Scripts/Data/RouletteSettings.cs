using System.Collections.Generic;
using UnityEngine;
using RouletteGame.Core;

namespace RouletteGame.Data
{
    /// <summary>
    /// Every tunable number of the Roulette game, editable in the Inspector.
    /// Bad values are reported as warnings. Nothing here decides an outcome.
    /// </summary>
    [CreateAssetMenu(fileName = "RouletteSettings", menuName = "Roulette Game/Settings")]
    public class RouletteSettings : ScriptableObject
    {
        [Header("Chips (virtual credits)")]
        [SerializeField] private int[] chipAmounts = { 10, 20, 50, 100, 500 };
        [Tooltip("One color per chip, in the same order as the amounts.")]
        [SerializeField] private Color[] chipColors =
        {
            new Color32(66, 133, 244, 255), new Color32(52, 168, 83, 255), new Color32(234, 67, 53, 255),
            new Color32(142, 36, 170, 255), new Color32(40, 40, 48, 255)
        };
        [SerializeField] private int defaultChipIndex = 0;

        [Header("Table limits")]
        [SerializeField] private long maxBetPerArea = 100000;
        [SerializeField] private long maxTotalBet = 1000000;

        [Header("Wheel")]
        [SerializeField] private WheelSpinSettings spin = new WheelSpinSettings();
        [Tooltip("Animation speed after SPIN is pressed during a spin.")]
        [SerializeField] private float fastForwardSpeed = 4f;

        [Header("Round")]
        [Tooltip("The result panel closes by itself after this many seconds. 0 = the player closes it.")]
        [SerializeField] private float resultAutoCloseSeconds = 0f;

        [Header("History")]
        [SerializeField] private int historyLimit = 100;
        [SerializeField, Range(1, 20)] private int lastNumbersShown = 10;

        public IReadOnlyList<int> ChipAmounts => chipAmounts;
        public int DefaultChipIndex => Mathf.Clamp(defaultChipIndex, 0, Mathf.Max(0, chipAmounts.Length - 1));
        public RouletteLimits Limits => new RouletteLimits(maxBetPerArea, maxTotalBet);
        public WheelSpinSettings Spin => spin;
        public float FastForwardSpeed => Mathf.Max(1f, fastForwardSpeed);
        public float ResultAutoCloseSeconds => Mathf.Max(0f, resultAutoCloseSeconds);
        public int HistoryLimit => Mathf.Max(1, historyLimit);
        public int LastNumbersShown => Mathf.Clamp(lastNumbersShown, 1, 20);

        public Color ChipColor(int index)
        {
            if (chipColors == null || chipColors.Length == 0) return new Color32(94, 53, 177, 255);
            return chipColors[Mathf.Clamp(index, 0, chipColors.Length - 1)];
        }

        /// <summary>Everything wrong with these settings. An empty list means they are usable.</summary>
        public List<string> GetProblems()
        {
            List<string> problems = new List<string>();

            if (maxBetPerArea <= 0) problems.Add("Max Bet Per Area must be above zero.");
            if (maxTotalBet <= 0) problems.Add("Max Total Bet must be above zero.");

            if (chipAmounts == null || chipAmounts.Length == 0)
            {
                problems.Add("No chip amounts are defined.");
            }
            else
            {
                HashSet<int> seen = new HashSet<int>();
                foreach (int chip in chipAmounts)
                {
                    if (chip <= 0) problems.Add("Chip amounts must be positive.");
                    else if (!seen.Add(chip)) problems.Add("Chip " + chip + " is listed twice.");
                    else if (chip > maxBetPerArea) problems.Add("Chip " + chip + " is above Max Bet Per Area.");
                }
            }

            if (spin == null) problems.Add("Spin settings are missing.");
            else problems.AddRange(spin.GetProblems());
            return problems;
        }

        private void OnValidate()
        {
            List<string> problems = GetProblems();
            if (problems.Count > 0) Debug.LogWarning("[RouletteSettings] " + name + ": " + string.Join(" | ", problems), this);
        }
    }
}
