using UnityEngine;
using UnityEngine.EventSystems;

namespace Shared.UI
{
    /// <summary>A small press feedback for buttons: shrinks while pressed, then returns. No effect while idle.</summary>
    public class PressScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float pressedScale = 0.96f;
        [SerializeField] private float speed = 22f;

        private float target = 1f;

        private void Awake() { enabled = false; }
        private void OnDisable() { transform.localScale = Vector3.one; target = 1f; }

        public void OnPointerDown(PointerEventData e) { target = pressedScale; enabled = true; }
        public void OnPointerUp(PointerEventData e) { target = 1f; enabled = true; }
        public void OnPointerExit(PointerEventData e) { target = 1f; enabled = true; }

        private void Update()
        {
            float s = Mathf.Lerp(transform.localScale.x, target, 1f - Mathf.Exp(-speed * Time.unscaledDeltaTime));
            if (Mathf.Abs(s - target) < 0.002f)
            {
                s = target;
                if (target >= 1f) enabled = false; // idle again: no per-frame work
            }
            transform.localScale = Vector3.one * s;
        }
    }
}