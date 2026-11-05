using System.Collections.Generic;
using UnityEngine;

namespace ColorGame.UI
{
    /// <summary>
    /// The bills lying on ONE color. Bills are pooled, laid out inside this rectangle (bottom up, slight
    /// rotation, two columns when crowded) and scaled to fit. Only the newest Max Visible Bills are drawn;
    /// the BET total label on the color button always shows the real amount.
    /// </summary>
    public class BillStackView : MonoBehaviour
    {
        [SerializeField] private BetBillView billPrefab;
        [SerializeField] private int maxVisibleBills = 12;
        [SerializeField] private float maxRotation = 9f;
        [SerializeField] private float popDuration = 0.14f;

        private readonly List<int> amounts = new List<int>();
        private readonly List<BetBillView> shown = new List<BetBillView>();
        private readonly Stack<BetBillView> pool = new Stack<BetBillView>();

        private RectTransform Area => (RectTransform)transform;

        public void AddBill(int denomination)
        {
            amounts.Add(denomination);
            Relayout(true);
        }

        public void SetBills(IReadOnlyList<int> denominations)
        {
            Clear();
            foreach (int d in denominations) amounts.Add(d);
            Relayout(false);
        }

        public void Clear()
        {
            foreach (BetBillView bill in shown) ReturnToPool(bill);
            shown.Clear();
            amounts.Clear();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (isActiveAndEnabled && shown.Count > 0) Relayout(false);
        }

        private void Relayout(bool animateNewest)
        {
            if (billPrefab == null)
            {
                Debug.LogError("[BillStackView] Bill Prefab is not assigned.", this);
                return;
            }

            int visible = Mathf.Min(amounts.Count, Mathf.Max(1, maxVisibleBills));
            int first = amounts.Count - visible;

            while (shown.Count < visible) shown.Add(Rent());
            while (shown.Count > visible)
            {
                ReturnToPool(shown[shown.Count - 1]);
                shown.RemoveAt(shown.Count - 1);
            }

            Rect area = Area.rect;
            RectTransform prefabRect = (RectTransform)billPrefab.transform;
            float bw = prefabRect.sizeDelta.x;
            float bh = prefabRect.sizeDelta.y;

            float scale = area.width > 1f && area.height > 1f
                ? Mathf.Min(1f, area.width * 0.8f / bw, area.height * 0.6f / bh)
                : 1f;

            int cols = visible > 6 ? 2 : 1;
            int rows = Mathf.CeilToInt(visible / (float)cols);
            float scaledW = bw * scale;
            float scaledH = bh * scale;
            float freeH = Mathf.Max(0f, area.height - scaledH - 4f);
            float step = rows > 1 ? Mathf.Min(scaledH * 0.3f, freeH / (rows - 1)) : 0f;
            float colOffset = cols == 2 ? scaledW * 0.22f : 0f;

            for (int i = 0; i < visible; i++)
            {
                BetBillView bill = shown[i];
                bill.Setup(amounts[first + i]);

                int col = i % cols;
                int row = i / cols;
                int seed = first + i;

                float x = (cols == 2 ? (col == 0 ? -colOffset : colOffset) : 0f) + Jitter(seed, 1) * scaledW * 0.05f;
                float y = -area.height * 0.5f + scaledH * 0.5f + 2f + row * step;

                RectTransform rt = (RectTransform)bill.transform;
                rt.anchoredPosition = new Vector2(x, y);
                rt.localRotation = Quaternion.Euler(0f, 0f, Jitter(seed, 2) * maxRotation);
                rt.localScale = Vector3.one * scale;
                rt.SetSiblingIndex(i);
            }

            if (animateNewest && visible > 0) shown[visible - 1].PlayPop(scale, popDuration);
        }

        private BetBillView Rent()
        {
            BetBillView bill = pool.Count > 0 ? pool.Pop() : Instantiate(billPrefab, transform);
            RectTransform rt = (RectTransform)bill.transform;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            bill.gameObject.SetActive(true);
            return bill;
        }

        private void ReturnToPool(BetBillView bill)
        {
            if (bill == null) return;
            bill.ResetState();
            bill.gameObject.SetActive(false);
            pool.Push(bill);
        }

        // Stable pseudo-random number from -1 to 1, so a bill never jumps around.
        private static float Jitter(int seed, int salt)
        {
            float v = Mathf.Sin(seed * 12.9898f + salt * 78.233f) * 43758.5453f;
            return (v - Mathf.Floor(v)) * 2f - 1f;
        }
    }
}