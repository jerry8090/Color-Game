using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RouletteGame.Core;

namespace RouletteGame.UI
{
    /// <summary>
    /// The result card shown after the ball has stopped: winning number and color, every winning and losing bet,
    /// the total payout and the net profit or loss. Display only: it shows the numbers the settlement already decided.
    /// Put this script on an always-active object. The card itself (Panel Root) is switched on and off.
    /// </summary>
    public class RouletteResultPanel : MonoBehaviour
    {
        private const int MaxBetLines = 12;

        [SerializeField] private GameObject panelRoot;
        [SerializeField] private Image numberBadge;
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private TMP_Text colorText;
        [SerializeField] private TMP_Text summaryText;
        [SerializeField] private TMP_Text betsText;
        [SerializeField] private TMP_Text payoutText;
        [SerializeField] private TMP_Text netText;
        [SerializeField] private Button continueButton;

        [Header("Look")]
        [SerializeField] private Color winColor = new Color32(124, 226, 140, 255);
        [SerializeField] private Color loseColor = new Color32(255, 122, 122, 255);
        [SerializeField] private Color evenColor = Color.white;

        /// <summary>The player pressed CONTINUE.</summary>
        public event Action OnContinue;

        private bool isReady;

        public bool IsOpen => panelRoot != null && panelRoot.activeSelf;

        private void Awake()
        {
            if (panelRoot == null || numberBadge == null || numberText == null || colorText == null || summaryText == null ||
                betsText == null || payoutText == null || netText == null || continueButton == null)
            {
                Debug.LogError("[RouletteResultPanel] A reference is missing in the Inspector.", this);
                return;
            }

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => OnContinue?.Invoke());
            panelRoot.SetActive(false);
            isReady = true;
        }

        public void Show(RouletteSettlementResult result)
        {
            if (!isReady || result == null || result.Payout == null) return;

            RoundPayout payout = result.Payout;
            PocketColor pocket = RouletteLayout.ColorOf(payout.WinningNumber);

            numberBadge.color = RouletteUi.ColorFor(pocket);
            numberText.text = payout.WinningNumber.ToString();
            colorText.text = RouletteText.ColorName(pocket);
            summaryText.text = "WINNING BETS " + payout.WinningBetCount + "     LOSING BETS " + (payout.Outcomes.Count - payout.WinningBetCount);
            betsText.text = BuildBetLines(payout);

            long net = payout.NetResult;
            Color netColor = net > 0 ? winColor : (net < 0 ? loseColor : evenColor);
            payoutText.text = "PAYOUT  \u20B1" + RouletteText.Credits(payout.TotalReturned);
            netText.text = (net >= 0 ? "PROFIT  " : "LOSS  ") + "\u20B1" + RouletteText.Credits(Math.Abs(net));
            netText.color = netColor;

            panelRoot.SetActive(true);
        }

        public void Hide()
        {
            if (isReady) panelRoot.SetActive(false);
        }

        private string BuildBetLines(RoundPayout payout)
        {
            string win = ColorUtility.ToHtmlStringRGB(winColor);
            string lose = ColorUtility.ToHtmlStringRGB(loseColor);
            StringBuilder sb = new StringBuilder();

            int shown = Math.Min(payout.Outcomes.Count, MaxBetLines);
            for (int i = 0; i < shown; i++)
            {
                BetOutcome o = payout.Outcomes[i];
                if (o.Won)
                {
                    sb.Append("<color=#").Append(win).Append(">WIN   ").Append(RouletteText.Describe(o.Bet))
                      .Append("   ").Append(RouletteText.Signed(o.NetWinnings)).Append("</color>\n");
                }
                else
                {
                    sb.Append("<color=#").Append(lose).Append(">LOSE  ").Append(RouletteText.Describe(o.Bet))
                      .Append("   ").Append(RouletteText.Signed(-o.Bet.Amount)).Append("</color>\n");
                }
            }

            if (payout.Outcomes.Count > shown) sb.Append("+").Append(payout.Outcomes.Count - shown).Append(" MORE");
            return sb.ToString().TrimEnd('\n');
        }
    }
}
