using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// One persistent, semi-transparent floating overlay that shows the winnings of the current round.
    /// It is hidden until a win occurs, updates with every further win, and fades out a configurable time after the round ends.
    /// Display only: the controller tells it what to show, it never calculates a payout.
    /// It does not block taps (CanvasGroup and the background ignore raycasts) and has no Update loop.
    /// </summary>
    public class ScatterWinOverlay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text totalText;
        [SerializeField] private TMP_Text latestText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private TMP_Text comboText;

        [Header("Behaviour")]
        [Tooltip("Draw order of the overlay's own canvas. Higher = on top of the gameplay.")]
        [SerializeField] private int sortingOrder = 50;
        [Tooltip("Seconds the overlay stays visible after the round has finished and the final win is shown.")]
        [SerializeField, Min(0f)] private float hideDelay = 2.5f;
        [SerializeField, Min(0.01f)] private float fadeInTime = 0.15f;
        [SerializeField, Min(0.01f)] private float fadeOutTime = 0.35f;
        [Tooltip("How fast the TOTAL WIN counter catches up (higher is faster).")]
        [SerializeField] private float countRate = 6f;
        [SerializeField] private float popScale = 1.15f;

        /// <summary>1 = normal. The controller raises it to fast-forward the counting.</summary>
        public float Speed { get; set; } = 1f;

        private long shownMilli, targetMilli;
        private bool hasWins, counting, isReady;
        private Coroutine countRoutine, fadeRoutine, hideRoutine, popRoutine;

        private void Awake()
        {
            if (group == null || headerText == null || totalText == null ||
                latestText == null || multiplierText == null || comboText == null)
            {
                Debug.LogError("[ScatterWinOverlay] A reference is missing in the Inspector.", this);
                return;
            }

            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = sortingOrder;
            }

            group.interactable = false;
            group.blocksRaycasts = false; // never blocks the buttons or reels
            isReady = true;
            ResetForNewRound();
        }

        private void OnDisable()
        {
            if (!isReady) return;
            StopAllCoroutines();
            counting = false;
            countRoutine = fadeRoutine = hideRoutine = popRoutine = null;
            group.alpha = 0f;
        }

        /// <summary>A new paid spin starts: forget the previous result and hide at once.</summary>
        public void ResetForNewRound()
        {
            if (!isReady) return;

            StopAllCoroutines();
            counting = false;
            countRoutine = fadeRoutine = hideRoutine = popRoutine = null;

            hasWins = false;
            shownMilli = 0;
            targetMilli = 0;
            group.alpha = 0f;
            headerText.text = string.Empty;
            totalText.text = string.Empty;
            latestText.text = string.Empty;
            multiplierText.text = string.Empty;
            comboText.text = string.Empty;
            totalText.rectTransform.localScale = Vector3.one;
        }

        /// <summary>A win was confirmed. Shows the overlay (first win) or updates it (later wins of the same round).</summary>
        public void ShowWin(string header, long latestMilli, int multiplier, long totalMilli, string combos)
        {
            if (!isReady) return;

            CancelHide();
            hasWins = true;

            headerText.text = header ?? string.Empty;
            latestText.text = "LATEST WIN  " + ScatterWinFormat.Signed(latestMilli);
            multiplierText.text = "MULTIPLIER  x" + multiplier;
            comboText.text = combos ?? string.Empty;

            targetMilli = totalMilli;
            if (countRoutine != null) StopCoroutine(countRoutine);
            countRoutine = StartCoroutine(CountRoutine());

            if (popRoutine != null) StopCoroutine(popRoutine);
            popRoutine = StartCoroutine(PopRoutine());

            if (group.alpha < 1f) Fade(1f, fadeInTime);
        }

        /// <summary>The round is over and the payout was credited. Keeps the overlay for the hide delay, then fades it out.</summary>
        public void Finish(long creditedCredits)
        {
            if (!isReady || !hasWins) return; // a round without wins never shows the overlay

            comboText.text = "CREDITED TO BALANCE  " + creditedCredits.ToString("N0", CultureInfo.InvariantCulture);

            CancelHide();
            hideRoutine = StartCoroutine(HideAfterDelay());
        }

        // ---------- Animations (each one ends by itself) ----------

        private IEnumerator CountRoutine()
        {
            counting = true;
            while (shownMilli != targetMilli)
            {
                long diff = targetMilli - shownMilli;
                long step = Math.Max(1L, (long)(Math.Abs(diff) * countRate * Time.deltaTime * Mathf.Max(0.1f, Speed)));
                shownMilli = diff > 0 ? Math.Min(targetMilli, shownMilli + step) : Math.Max(targetMilli, shownMilli - step);
                totalText.text = ScatterWinFormat.Credits(shownMilli);
                yield return null;
            }

            totalText.text = ScatterWinFormat.Credits(shownMilli);
            counting = false;
        }

        private IEnumerator PopRoutine()
        {
            RectTransform rect = totalText.rectTransform;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * Mathf.Max(0.1f, Speed) / 0.25f;
                rect.localScale = Vector3.one * Mathf.Lerp(popScale, 1f, Mathf.Clamp01(t));
                yield return null;
            }
            rect.localScale = Vector3.one;
        }

        private IEnumerator HideAfterDelay()
        {
            while (counting) yield return null; // never hide before the final amount has been reached
            yield return new WaitForSeconds(hideDelay);
            Fade(0f, fadeOutTime);
        }

        private void CancelHide()
        {
            if (hideRoutine != null) StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        private void Fade(float target, float seconds)
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeRoutine(target, seconds));
        }

        private IEnumerator FadeRoutine(float target, float seconds)
        {
            float start = group.alpha;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / seconds;
                group.alpha = Mathf.Lerp(start, target, Mathf.Clamp01(t));
                yield return null;
            }
            group.alpha = target;
        }
    }
}