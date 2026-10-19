using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using Shared.Wallet;

namespace ColorGame.UI
{
    /// <summary>
    /// Displays the current balance. Display only: it never changes the balance itself.
    /// Bets and resets show instantly. Winnings, bonuses and test credits count up with a small pop.
    /// </summary>
    public class WalletUI : MonoBehaviour
    {
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private TMP_Text balanceText;

        [Header("Gain Animation")]
        [SerializeField] private float countUpDuration = 0.6f;
        [SerializeField] private float popScale = 1.15f;

        private Coroutine animationRoutine;
        private bool isReady;

        private void OnEnable()
        {
            if (walletManager == null || balanceText == null)
            {
                Debug.LogError("[WalletUI] Wallet Manager or Balance Text is not assigned.", this);
                return;
            }

            isReady = true;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
            walletManager.OnTransactionAdded += HandleTransactionAdded;
            ShowInstant(walletManager.GetBalance());
        }

        private void OnDisable()
        {
            if (walletManager != null)
            {
                walletManager.OnBalanceChanged -= HandleBalanceChanged;
                walletManager.OnTransactionAdded -= HandleTransactionAdded;
            }

            StopAnimation();
            if (balanceText != null) balanceText.transform.localScale = Vector3.one;
        }

        // Fires first on every change. Always shows the true balance immediately.
        private void HandleBalanceChanged(long balance)
        {
            if (!isReady) return;
            StopAnimation();
            ShowInstant(balance);
        }

        // Fires right after. If it was a gain, replay the number as a count-up.
        private void HandleTransactionAdded(WalletTransaction tx)
        {
            if (!isReady || !isActiveAndEnabled) return;

            bool isGain = tx.type == TransactionType.Win ||
                          tx.type == TransactionType.Bonus ||
                          tx.type == TransactionType.AdminTest;

            if (!isGain || tx.balanceAfter <= tx.balanceBefore) return;

            StopAnimation();
            animationRoutine = StartCoroutine(CountUpRoutine(tx.balanceBefore, tx.balanceAfter));
        }

        private IEnumerator CountUpRoutine(long from, long to)
        {
            float elapsed = 0f;

            while (elapsed < countUpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / countUpDuration);
                float eased = 1f - (1f - t) * (1f - t); // fast at first, slows at the end

                long shown = from + (long)System.Math.Round((to - from) * (double)eased);
                balanceText.text = Format(shown);
                balanceText.transform.localScale = Vector3.one * (1f + (popScale - 1f) * Mathf.Sin(t * Mathf.PI));
                yield return null;
            }

            ShowInstant(to);
            animationRoutine = null;
        }

        private void ShowInstant(long balance)
        {
            balanceText.text = Format(balance);
            balanceText.transform.localScale = Vector3.one;
        }

        private void StopAnimation()
        {
            if (animationRoutine == null) return;
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }

        // "N0" gives thousands separators: 1,000 / 10,000 / 100,000
        private static string Format(long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}