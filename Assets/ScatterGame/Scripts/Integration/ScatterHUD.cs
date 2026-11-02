using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace ScatterGame.Integration
{
    /// <summary>
    /// All the numbers and messages on the play screen. Display only.
    /// Balance and win count up smoothly. The controller decides what they should show.
    /// </summary>
    public class ScatterHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text balanceText;
        [SerializeField] private TMP_Text winText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private TMP_Text betText;
        [SerializeField] private TMP_Text freeSpinText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text bannerText;
        [SerializeField] private GameObject freeSpinBar;
        [SerializeField] private CanvasGroup bannerGroup;
        [Tooltip("How fast the counters catch up (higher is faster).")]
        [SerializeField] private float countRate = 6f;

        /// <summary>1 = normal. The controller raises it to fast-forward.</summary>
        public float Speed { get; set; } = 1f;

        private long targetBalance, shownBalance, targetWin, shownWin;
        private Coroutine popRoutine;
        private bool isReady;

        private void Awake()
        {
            if (balanceText == null || winText == null || multiplierText == null || betText == null ||
                freeSpinText == null || statusText == null || bannerText == null ||
                freeSpinBar == null || bannerGroup == null)
            {
                Debug.LogError("[ScatterHUD] A reference is missing in the Inspector.", this);
                return;
            }

            balanceText.text = "0";
            winText.text = "0";
            statusText.text = string.Empty;
            freeSpinBar.SetActive(false);
            bannerGroup.alpha = 0f;
            bannerGroup.blocksRaycasts = false;
            bannerGroup.interactable = false;
            isReady = true;
        }

        private void Update()
        {
            if (!isReady) return;

            if (shownBalance != targetBalance)
            {
                shownBalance = Step(shownBalance, targetBalance);
                balanceText.text = Format(shownBalance);
            }

            if (shownWin != targetWin)
            {
                shownWin = Step(shownWin, targetWin);
                winText.text = Format(shownWin);
            }
        }

        public void SetBalance(long value, bool instant)
        {
            if (!isReady) return;
            targetBalance = value;
            if (instant)
            {
                shownBalance = value;
                balanceText.text = Format(value);
            }
        }

        public void SetWin(long value, bool instant)
        {
            if (!isReady) return;
            targetWin = value;
            if (instant)
            {
                shownWin = value;
                winText.text = Format(value);
            }
        }

        public void SetBet(int bet)
        {
            if (isReady) betText.text = "BET  " + Format(bet);
        }

        public void SetMultiplier(int multiplier, bool pop)
        {
            if (!isReady) return;
            multiplierText.text = "x" + multiplier;
            if (!pop) return;

            if (popRoutine != null) StopCoroutine(popRoutine);
            popRoutine = StartCoroutine(Pop());
        }

        public void SetFreeSpins(int current, int total)
        {
            if (!isReady) return;
            freeSpinBar.SetActive(true);
            freeSpinText.text = "FREE SPIN " + current + " / " + total;
        }

        public void HideFreeSpins()
        {
            if (isReady) freeSpinBar.SetActive(false);
        }

        public void SetStatus(string message)
        {
            if (isReady) statusText.text = message ?? string.Empty;
        }

        /// <summary>A short full-width announcement: fades in, stays, fades out.</summary>
        public IEnumerator ShowBanner(string message, float holdSeconds)
        {
            if (!isReady) yield break;

            bannerText.text = message;
            RectTransform rect = bannerText.rectTransform;
            bannerGroup.alpha = 0f;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * Speed / 0.2f;
                float k = Mathf.Clamp01(t);
                bannerGroup.alpha = k;
                rect.localScale = Vector3.one * Mathf.Lerp(0.6f, 1f, k);
                yield return null;
            }

            float held = 0f;
            while (held < holdSeconds)
            {
                held += Time.deltaTime * Speed;
                yield return null;
            }

            t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime * Speed / 0.25f;
                bannerGroup.alpha = Mathf.Clamp01(t);
                yield return null;
            }

            bannerGroup.alpha = 0f;
            rect.localScale = Vector3.one;
        }

        private IEnumerator Pop()
        {
            RectTransform rect = multiplierText.rectTransform;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * Speed / 0.3f;
                rect.localScale = Vector3.one * Mathf.Lerp(1.6f, 1f, Mathf.Clamp01(t));
                yield return null;
            }
            rect.localScale = Vector3.one;
            popRoutine = null;
        }

        private long Step(long current, long target)
        {
            long diff = target - current;
            long step = Math.Max(1L, (long)(Math.Abs(diff) * countRate * Time.deltaTime));
            return diff > 0 ? Math.Min(target, current + step) : Math.Max(target, current - step);
        }

        private static string Format(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}