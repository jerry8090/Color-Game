using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Displays the betting and result messages. It only shows what the manager reports and never decides who wins.
    ///   Betting open      : PLACE YOUR BET
    ///   Round without bet : NO BET - ROUND CONTINUES (no win or loss is ever shown for it)
    ///   Wagered round     : ROLLING... then WIN! / LOSE, or ROLL CANCELLED if the cubes could not settle.
    /// "Winnings" is the total returned (stake included). "Net" is winnings minus the bet.
    /// </summary>
    public class GameResultUI : MonoBehaviour
    {
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text matchesText;
        [SerializeField] private TMP_Text winningsText;

        [Header("Colors")]
        [SerializeField] private Color neutralColor = Color.white;
        [SerializeField] private Color winColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private Color loseColor = new Color(1f, 0.45f, 0.45f, 1f);
        [SerializeField] private Color noBetColor = new Color(0.82f, 0.78f, 0.9f, 1f);

        private Coroutine popRoutine;
        private bool isReady;

        private void Awake()
        {
            if (gameManager == null || titleText == null || matchesText == null || winningsText == null)
            {
                Debug.LogError("[GameResultUI] A reference is missing in the Inspector.", this);
                return;
            }

            isReady = true;
            ShowReady();
        }

        private void OnEnable()
        {
            if (!isReady) return;
            gameManager.OnRoundStarted += HandleRoundStarted;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            gameManager.OnRoundVoided += HandleRoundVoided;
            gameManager.OnBettingWindowOpened += HandleBettingWindowOpened;
            gameManager.OnRoundSkipped += HandleRoundSkipped;
        }

        private void OnDisable()
        {
            if (gameManager == null) return;
            gameManager.OnRoundStarted -= HandleRoundStarted;
            gameManager.OnRoundCompleted -= HandleRoundCompleted;
            gameManager.OnRoundVoided -= HandleRoundVoided;
            gameManager.OnBettingWindowOpened -= HandleBettingWindowOpened;
            gameManager.OnRoundSkipped -= HandleRoundSkipped;
        }

        private void ShowReady()
        {
            StopPop();
            titleText.text = "PLACE YOUR BET";
            titleText.color = neutralColor;
            matchesText.text = string.Empty;
            winningsText.text = string.Empty;
            titleText.transform.localScale = Vector3.one;
        }

        private void HandleBettingWindowOpened()
        {
            ShowReady();
        }

        // Deadline passed without a color AND a bet: no wager, the machine runs anyway.
        private void HandleRoundSkipped()
        {
            StopPop();
            titleText.text = "NO BET";
            titleText.color = noBetColor;
            titleText.transform.localScale = Vector3.one;
            matchesText.text = "ROUND CONTINUES";
            winningsText.text = string.Empty;
        }

        private void HandleRoundStarted(string roundId)
        {
            StopPop();
            titleText.text = "ROLLING...";
            titleText.color = neutralColor;
            titleText.transform.localScale = Vector3.one;
            matchesText.text = roundId;
            winningsText.text = string.Empty;
        }

        private void HandleRoundVoided(string roundId)
        {
            StopPop();
            titleText.text = "ROLL CANCELLED";
            titleText.color = neutralColor;
            titleText.transform.localScale = Vector3.one;
            matchesText.text = "BET REFUNDED";
            winningsText.text = string.Empty;
        }

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            string winnings = payout.Winnings.ToString("N0", CultureInfo.InvariantCulture);
            string net = payout.NetResult.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);

            if (payout.IsWin)
            {
                titleText.text = "WIN!";
                titleText.color = winColor;
                matchesText.text = "MATCHED: " + payout.Matches;
            }
            else
            {
                titleText.text = "LOSE";
                titleText.color = loseColor;
                matchesText.text = "Better luck next time!";
            }

            winningsText.text = $"WINNINGS: {winnings}  |  NET {net}";

            StopPop();
            if (gameObject.activeInHierarchy) popRoutine = StartCoroutine(PopRoutine());
        }

        private void StopPop()
        {
            if (popRoutine == null) return;
            StopCoroutine(popRoutine);
            popRoutine = null;
        }

        // Title grows past full size, then settles.
        private IEnumerator PopRoutine()
        {
            const float duration = 0.4f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float scale = t < 0.6f
                    ? Mathf.Lerp(0.5f, 1.25f, t / 0.6f)
                    : Mathf.Lerp(1.25f, 1f, (t - 0.6f) / 0.4f);
                titleText.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            titleText.transform.localScale = Vector3.one;
            popRoutine = null;
        }
    }
}
