using UnityEngine;
using UnityEngine.UI;

namespace Shared.UI
{
    /// <summary>
    /// Keeps the whole reference area (for example 1080 x 1920) visible on every screen shape.
    /// Screen taller than the reference (most modern phones): match WIDTH, so the canvas is 1080 wide and taller than 1920.
    /// Screen wider than the reference (tablets, foldables): match HEIGHT, so the canvas is 1920 tall and wider than 1080.
    /// Result: the canvas is never smaller than the reference size in either direction.
    /// Put this on the Canvas, next to the Canvas Scaler.
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    public class ResponsiveCanvasScaler : MonoBehaviour
    {
        private CanvasScaler scaler;
        private int lastWidth;
        private int lastHeight;

        private void Awake()
        {
            scaler = GetComponent<CanvasScaler>();
        }

        private void OnEnable()
        {
            Apply(true);
        }

        // Runs every frame but only does work when the screen size changed
        // (window resized in the Editor, split-screen or foldable on Android).
        private void Update()
        {
            Apply(false);
        }

        private void Apply(bool force)
        {
            if (scaler == null)
            {
                Debug.LogError("[ResponsiveCanvasScaler] No Canvas Scaler found on this object.", this);
                enabled = false;
                return;
            }

            if (Screen.width <= 0 || Screen.height <= 0) return;
            if (!force && Screen.width == lastWidth && Screen.height == lastHeight) return;

            lastWidth = Screen.width;
            lastHeight = Screen.height;

            Vector2 reference = scaler.referenceResolution;
            if (reference.x <= 0f || reference.y <= 0f)
            {
                Debug.LogError("[ResponsiveCanvasScaler] Reference Resolution must be greater than zero.", this);
                return;
            }

            float referenceAspect = reference.x / reference.y;
            float screenAspect = (float)Screen.width / Screen.height;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = screenAspect >= referenceAspect ? 1f : 0f;
        }
    }
}