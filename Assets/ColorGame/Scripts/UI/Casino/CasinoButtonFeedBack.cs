using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Press feedback: the Target shrinks slightly while pressed, then eases back.
    /// Point Target at a CHILD (icon or label), never at an object whose scale another script controls.
    /// Button OnClick events are untouched. This only listens to pointer events.
    /// </summary>
    public class CasinoButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private RectTransform target;

        private Selectable selectable;
        private Vector3 baseScale = Vector3.one;
        private float current = 1f;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (target == null) target = (RectTransform)transform;
            if (controller == null)
            {
                Debug.LogError("[CasinoButtonFeedback] No CasinoUIAnimationController in the scene.", this);
                return;
            }

            selectable = GetComponentInParent<Selectable>();
            baseScale = target.localScale;
            ready = true;
        }

        private void OnDisable()
        {
            routine = null;
            current = 1f;
            if (target != null) target.localScale = baseScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!ready || (selectable != null && !selectable.IsInteractable())) return;
            AnimateTo(controller.ButtonPressScale);
        }

        public void OnPointerUp(PointerEventData eventData) { if (ready) AnimateTo(1f); }
        public void OnPointerExit(PointerEventData eventData) { if (ready) AnimateTo(1f); }

        private void AnimateTo(float to)
        {
            if (!isActiveAndEnabled) return;
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Run(to));
        }

        private IEnumerator Run(float to)
        {
            float speed = 4f / Mathf.Max(0.03f, controller.ButtonAnimDuration);

            while (Mathf.Abs(current - to) > 0.002f)
            {
                current = Mathf.Lerp(current, to, 1f - Mathf.Exp(-speed * Time.unscaledDeltaTime));
                target.localScale = baseScale * current;
                yield return null;
            }

            current = to;
            target.localScale = baseScale * current;
            routine = null;
        }
    }
}