using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// One row of the history list. Display only: it shows the values that were recorded
    /// in the GameRound (matches, winnings) and never works out a win itself.
    /// </summary>
    public class HistoryRowView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image colorChip;
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text resultsText;
        [SerializeField] private TMP_Text detailsText;
        [SerializeField] private TMP_Text winText;

        [Header("Look")]
        [SerializeField] private Color winTint = new Color(0.17f, 0.33f, 0.22f, 1f);
        [SerializeField] private Color loseTint = new Color(0.33f, 0.18f, 0.20f, 1f);
        [SerializeField] private Color gainTextColor = new Color(0.49f, 0.89f, 0.55f, 1f);
        [SerializeField] private Color evenTextColor = Color.white;
        [SerializeField] private Color lossTextColor = new Color(1f, 0.48f, 0.48f, 1f);
        [SerializeField] private Color mutedTextColor = new Color(0.72f, 0.66f, 0.82f, 1f);
        [Tooltip("Color names are lightened so dark colors (like purple) stay readable on the dark row.")]
        [SerializeField, Range(0f, 1f)] private float colorLighten = 0.35f;

        private bool referencesChecked;
        private bool referencesOk;

        public void Bind(GameRound round, ColorGameConfig config)
        {
            if (round == null || config == null)
            {
                Debug.LogError("[HistoryRowView] Bind needs a round and a config.", this);
                return;
            }

            if (!ReferencesAreValid()) return;

            // Background tint and the selected-color chip.
            background.color = round.IsWin ? winTint : loseTint;

            if (config.TryGetColorDefinition(round.selectedColor, out ColorDefinition selectedDefinition))
            {
                colorChip.color = selectedDefinition.uiColor;
            }

            string mutedHex = ColorUtility.ToHtmlStringRGB(mutedTextColor);
            headerText.text = $"{round.roundId}   <size=75%><color=#{mutedHex}>{FormatTime(round.timestampUtc)}</color></size>";

            resultsText.text = $"{ColorWord(config, round.result1)}  /  {ColorWord(config, round.result2)}  /  {ColorWord(config, round.result3)}";

            string bet = round.betAmount.ToString("N0", CultureInfo.InvariantCulture);
            string won = round.winnings.ToString("N0", CultureInfo.InvariantCulture);
            detailsText.text = $"BET {ColorWord(config, round.selectedColor)} {bet}  |  MATCHES {round.matches}  |  WON {won}";

            // NET = winnings minus the bet. A 1-match round at 1x honestly shows 0.
            long net = round.NetResult;
            Color netColor = net > 0 ? gainTextColor : (net < 0 ? lossTextColor : evenTextColor);
            string netString = net.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);
            winText.text = $"<size=45%>NET</size>\n<color=#{ColorUtility.ToHtmlStringRGB(netColor)}>{netString}</color>";
        }

        private string ColorWord(ColorGameConfig config, GameColor color)
        {
            if (!config.TryGetColorDefinition(color, out ColorDefinition definition))
            {
                return color.ToString().ToUpperInvariant();
            }

            Color lightened = Color.Lerp(definition.uiColor, Color.white, colorLighten);
            return $"<color=#{ColorUtility.ToHtmlStringRGB(lightened)}>{definition.displayName.ToUpperInvariant()}</color>";
        }

        // Timestamps are saved as UTC ISO-8601 text. Show them in the player's local time.
        private static string FormatTime(string isoUtc)
        {
            if (DateTime.TryParse(isoUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime time))
            {
                return time.ToLocalTime().ToString("MMM d, HH:mm", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        private bool ReferencesAreValid()
        {
            if (referencesChecked) return referencesOk;

            referencesChecked = true;
            referencesOk = background != null && colorChip != null && headerText != null &&
                           resultsText != null && detailsText != null && winText != null;

            if (!referencesOk)
            {
                Debug.LogError("[HistoryRowView] A reference is missing on the HistoryRow prefab.", this);
            }
            return referencesOk;
        }
    }
}