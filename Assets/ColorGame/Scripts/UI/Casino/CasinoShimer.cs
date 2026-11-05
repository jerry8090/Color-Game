using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// A soft gold light bar sweeping across this UI element every few seconds.
    /// Builds its own child (ShimmerMask > ShimmerBar). Your sprites are never modified.
    /// The sweep is clipped to the rectangle of this object (RectMask2D).
    /// </summary>
    public class CasinoShimmer : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private float startDelay = 0.5f;
        [SerializeField] private bool randomizeStart = true;

        private RectTransform host;
        private RectTransform bar;
        private Image barImage;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (controller == null)
            {
                Debug.LogError("[CasinoShimmer] No CasinoUIAnimationController in the scene.", this);
                return;
            }

            host = (RectTransform)transform;

            GameObject maskGo = new GameObject("ShimmerMask", typeof(RectTransform), typeof(RectMask2D), typeof(LayoutElement));
            maskGo.GetComponent<LayoutElement>().ignoreLayout = true; // never disturb a Layout Group
            RectTransform mask = (RectTransform)maskGo.transform;
            mask.SetParent(host, false);
            mask.anchorMin = Vector2.zero;
            mask.anchorMax = Vector2.one;
            mask.offsetMin = Vector2.zero;
            mask.offsetMax = Vector2.zero;

            GameObject barGo = new GameObject("ShimmerBar", typeof(RectTransform), typeof(Image));
            bar = (RectTransform)barGo.transform;
            bar.SetParent(mask, false);
            bar.anchorMin = bar.anchorMax = new Vector2(0.5f, 0.5f);
            bar.pivot = new Vector2(0.5f, 0.5f);

            barImage = barGo.GetComponent<Image>();
            barImage.sprite = CasinoSprites.SoftBar;
            barImage.raycastTarget = false;
            barImage.enabled = false;

            ready = true;
        }

        private void OnEnable()
        {
            if (ready) routine = StartCoroutine(Loop());
        }

        private void OnDisable()
        {
            routine = null;
            if (barImage != null) barImage.enabled = false;
        }

        private IEnumerator Loop()
        {
            float first = startDelay + (randomizeStart ? Random.value * controller.ShimmerInterval : 0f);
            yield return new WaitForSecondsRealtime(first);

            while (true)
            {
                if (controller.IdleAnimationsEnabled) yield return Sweep();
                yield return new WaitForSecondsRealtime(controller.ShimmerInterval);
            }
        }

        private IEnumerator Sweep()
        {
            Rect r = host.rect;
            if (r.width < 2f || r.height < 2f) yield break;

            float w = r.width * controller.ShimmerWidth;
            bar.sizeDelta = new Vector2(w, r.height * 1.6f);
            bar.localRotation = Quaternion.Euler(0f, 0f, 12f);

            Color c = controller.Gold;
            c.a = controller.ShimmerIntensity;
            barImage.color = c;
            barImage.enabled = true;

            float from = -(r.width * 0.5f + w);
            float to = r.width * 0.5f + w;
            float duration = Mathf.Max(0.1f, controller.ShimmerDuration);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                bar.anchoredPosition = new Vector2(Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t)), 0f);
                yield return null;
            }

            barImage.enabled = false;
        }
    }
}