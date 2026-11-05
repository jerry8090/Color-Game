using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI
{
    /// <summary>
    /// One betting bill. The artwork (with the peso sign) comes from the library by the REAL bet amount,
    /// the digits are text on top. Display only.
    /// </summary>
    public class BetBillView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image inner;
        [SerializeField] private TMP_Text label;
        [SerializeField] private BillSpriteLibrary library;

        private Coroutine popRoutine;

        public int Denomination { get; private set; }

        public void Setup(int denomination)
        {
            if (background == null || label == null)
            {
                Debug.LogError("[BetBillView] A reference is missing on the BetBill prefab.", this);
                return;
            }

            Denomination = denomination;
            if (inner != null) inner.enabled = false;

            Sprite art = library != null ? library.Get(denomination) : null;
            if (art == null)
            {
                Debug.LogError($"[BetBillView] No bill artwork for {denomination}. Run Tools > Color Game > Design > 1.", this);
                background.sprite = null;
                background.color = new Color(0.4f, 0.3f, 0.6f, 1f);
            }
            else
            {
                background.sprite = art;
                background.color = Color.white;
            }

            label.text = denomination.ToString("N0", CultureInfo.InvariantCulture);
        }

        public void PlayPop(float targetScale, float duration)
        {
            ResetState();
            if (isActiveAndEnabled) popRoutine = StartCoroutine(Pop(targetScale, duration));
        }

        public void ResetState()
        {
            if (popRoutine != null)
            {
                StopCoroutine(popRoutine);
                popRoutine = null;
            }
        }

        private IEnumerator Pop(float targetScale, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localScale = Vector3.one * Mathf.Lerp(targetScale * 1.5f, targetScale, t);
                yield return null;
            }
            transform.localScale = Vector3.one * targetScale;
            popRoutine = null;
        }
    }
}