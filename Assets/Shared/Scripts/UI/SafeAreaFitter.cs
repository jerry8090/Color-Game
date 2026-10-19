using UnityEngine;

namespace Shared.UI
{
    /// <summary>
    /// Shrinks this RectTransform to the phone's safe area (the part not covered by a notch,
    /// punch-hole camera, rounded corners, or the gesture bar).
    /// Put this on SafeAreaPanel. Everything inside it stays visible and tappable.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        [Header("Which edges respect the safe area")]
        [SerializeField] private bool applyLeft = true;
        [SerializeField] private bool applyRight = true;
        [SerializeField] private bool applyTop = true;
        [SerializeField] private bool applyBottom = true;

        private RectTransform rectTransform;
        private Rect lastSafeArea;
        private int lastWidth;
        private int lastHeight;

        private void Awake()
        {
            rectTransform = (RectTransform)transform;
        }

        private void OnEnable()
        {
            Apply(true);
        }

        // Cheap check every frame: only does work when the safe area or screen size changed.
        private void Update()
        {
            Apply(false);
        }

        private void Apply(bool force)
        {
            if (rectTransform == null) return;
            if (Screen.width <= 0 || Screen.height <= 0) return;

            Rect safeArea = Screen.safeArea;

            if (!force && safeArea == lastSafeArea &&
                Screen.width == lastWidth && Screen.height == lastHeight)
            {
                return;
            }

            lastSafeArea = safeArea;
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            float left = applyLeft ? safeArea.xMin : 0f;
            float right = applyRight ? safeArea.xMax : Screen.width;
            float bottom = applyBottom ? safeArea.yMin : 0f;
            float top = applyTop ? safeArea.yMax : Screen.height;

            // Anchors are fractions (0 to 1) of the parent, so this works at any resolution.
            rectTransform.anchorMin = new Vector2(left / Screen.width, bottom / Screen.height);
            rectTransform.anchorMax = new Vector2(right / Screen.width, top / Screen.height);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}