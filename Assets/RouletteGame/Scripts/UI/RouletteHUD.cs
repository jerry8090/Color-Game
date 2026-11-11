using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RouletteGame.Core;
using RouletteGame.Data;

namespace RouletteGame.UI
{
    /// <summary>
    /// All the numbers and messages on the play screen. Display only.
    /// The balance counts up smoothly. The controller decides what every value should show,
    /// so the balance can stay behind the real wallet while the ball is still rolling.
    /// </summary>
    public class RouletteHUD : MonoBehaviour
    {
        [SerializeField] private RouletteSettings settings;
        [SerializeField] private TMP_Text balanceText;
        [SerializeField] private TMP_Text totalBetText;
        [SerializeField] private TMP_Text potentialText;
        [SerializeField] private TMP_Text statusText;

        [Header("Last numbers")]
        [SerializeField] private RectTransform lastNumbersContainer;
        [SerializeField] private Sprite badgeSprite;
        [SerializeField] private TMP_FontAsset font;
        [SerializeField] private float badgeSize = 56f;

        [Header("Look")]
        [SerializeField] private Color statusColor = Color.white;
        [SerializeField] private Color messageColor = new Color32(255, 138, 128, 255);
        [Tooltip("How fast the balance catches up (higher is faster).")]
        [SerializeField] private float countRate = 6f;

        private sealed class Badge
        {
            public GameObject root;
            public Image image;
            public TMP_Text text;
        }

        private readonly List<Badge> badges = new List<Badge>();
        private long targetBalance, shownBalance;
        private string baseStatus = string.Empty;
        private Coroutine messageRoutine;
        private bool isReady;

        private void Awake()
        {
            if (settings == null || balanceText == null || totalBetText == null || potentialText == null ||
                statusText == null || lastNumbersContainer == null)
            {
                Debug.LogError("[RouletteHUD] A reference is missing in the Inspector.", this);
                return;
            }

            for (int i = 0; i < settings.LastNumbersShown; i++) badges.Add(CreateBadge(i));
            SetLastNumbers(new List<int>());
            statusText.color = statusColor;
            isReady = true;
        }

        private void Update()
        {
            if (!isReady || shownBalance == targetBalance) return;

            long diff = targetBalance - shownBalance;
            long step = Math.Max(1L, (long)(Math.Abs(diff) * countRate * Time.deltaTime));
            shownBalance = diff > 0 ? Math.Min(targetBalance, shownBalance + step) : Math.Max(targetBalance, shownBalance - step);
            balanceText.text = Money(shownBalance);
        }

        public void SetBalance(long value, bool instant)
        {
            if (!isReady) return;
            targetBalance = value;
            if (instant)
            {
                shownBalance = value;
                balanceText.text = Money(value);
            }
        }

        public void SetTotalBet(long value)
        {
            if (isReady) totalBetText.text = Money(value);
        }

        public void SetPotential(long value)
        {
            if (isReady) potentialText.text = Money(value);
        }

        /// <summary>The steady line under the wheel (for example PLACE YOUR BETS).</summary>
        public void SetStatus(string message)
        {
            if (!isReady) return;
            baseStatus = message ?? string.Empty;
            if (messageRoutine == null)
            {
                statusText.text = baseStatus;
                statusText.color = statusColor;
            }
        }

        /// <summary>A short warning (for example INSUFFICIENT CREDITS). The steady line returns afterwards.</summary>
        public void ShowMessage(string message, float seconds = 1.6f)
        {
            if (!isReady || !isActiveAndEnabled) return;
            if (messageRoutine != null) StopCoroutine(messageRoutine);
            messageRoutine = StartCoroutine(MessageRoutine(message, seconds));
        }

        /// <summary>Shows the newest winning numbers, newest first (left).</summary>
        public void SetLastNumbers(IReadOnlyList<int> newestFirst)
        {
            if (!isReady && badges.Count == 0) return;

            for (int i = 0; i < badges.Count; i++)
            {
                bool has = newestFirst != null && i < newestFirst.Count;
                badges[i].root.SetActive(has);
                if (!has) continue;

                int number = newestFirst[i];
                badges[i].image.color = RouletteUi.ColorFor(RouletteLayout.ColorOf(number));
                badges[i].text.text = number.ToString();
            }
        }

        private IEnumerator MessageRoutine(string message, float seconds)
        {
            statusText.text = message;
            statusText.color = messageColor;
            yield return new WaitForSeconds(seconds);
            statusText.text = baseStatus;
            statusText.color = statusColor;
            messageRoutine = null;
        }

        private Badge CreateBadge(int index)
        {
            GameObject go = new GameObject("Last_" + index, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(lastNumbersContainer, false);
            ((RectTransform)go.transform).sizeDelta = new Vector2(badgeSize, badgeSize);

            Image image = go.GetComponent<Image>();
            image.sprite = badgeSprite;
            image.type = badgeSprite != null ? Image.Type.Sliced : Image.Type.Simple;
            image.raycastTarget = false;

            Outline outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 1f, 1f, index == 0 ? 0.9f : 0.25f); // the newest number stands out
            outline.effectDistance = new Vector2(2f, -2f);

            TextMeshProUGUI text = RouletteUi.MakeText(go.transform, "Number", "0", 30f, Color.white, font);
            return new Badge { root = go, image = image, text = text };
        }

        private static string Money(long value)
        {
            return "\u20B1" + value.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
