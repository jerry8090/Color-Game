using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Press feedback + gold glow for an EXISTING button. Add it to the button; nothing is replaced.
    /// - Press scale is skipped automatically when the button has an Animator (it already scales itself).
    /// - Uses pointer handlers, not onClick, so controller code that calls RemoveAllListeners cannot remove it.
    /// - The glow is one soft Image placed BEHIND the button as a sibling. It never blocks taps.
    /// - Runs a coroutine only while something is animating (idle glow keeps it alive; switch it off to stop it).
    /// </summary>
    [DisallowMultipleComponent]
    public class ScatterButtonFx : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Press")]
        [SerializeField] private bool pressScale = true;
        [SerializeField, Range(0.8f, 1f)] private float pressedScale = 0.94f;
        [Tooltip("Leave OFF for buttons with an Animator (SPIN, AUTO, SOUND) so the two do not fight.")]
        [SerializeField] private bool scaleEvenWithAnimator = false;

        [Header("Glow")]
        [SerializeField] private Color glowColor = new Color(1f, 0.82f, 0.3f, 1f);
        [SerializeField] private Color activeGlowColor = new Color(1f, 0.25f, 0.3f, 1f);
        [SerializeField] private float glowExpand = 14f;
        [SerializeField] private bool idleGlow = true;
        [SerializeField, Range(0f, 1f)] private float idleMin = 0f;
        [SerializeField, Range(0f, 1f)] private float idleMax = 0.22f;
        [SerializeField] private float idlePeriod = 2.4f;
        [SerializeField, Range(0f, 1f)] private float flashAlpha = 0.65f;

        [Header("AUTO button only")]
        [Tooltip("Tick on the AUTO button. The glow switches to the active colour while AUTO is on.")]
        [SerializeField] private bool isAutoButton = false;
        [SerializeField, Range(0f, 1f)] private float activeAlpha = 0.6f;

        [Header("Optional: punch another object on click (for example the BET value text)")]
        [SerializeField] private RectTransform punchTarget;
        [SerializeField] private float punchScale = 1.2f;

        private RectTransform self;
        private Button button;
        private Image glowImage;
        private Vector3 baseScale = Vector3.one;
        private Vector3 punchBase = Vector3.one;
        private bool useScale;
        private bool pressed;
        private bool activeOn;
        private float pressAmt, flash, punchT, activeAmt, clock;
        private Coroutine loop;

        private void Awake()
        {
            self = GetComponent<RectTransform>();

if (self == null)
{
    Debug.LogError(
        "[ScatterButtonFx] This script must be attached to a UI GameObject with RectTransform: " + name,
        this
    );

    enabled = false;
    return;
}
            button = GetComponent<Button>();
            baseScale = self.localScale;
            if (punchTarget != null) punchBase = punchTarget.localScale;

            Animator animator = GetComponent<Animator>();
            bool animated = animator != null && animator.runtimeAnimatorController != null;
            useScale = pressScale && (scaleEvenWithAnimator || !animated);

            BuildGlow();
        }

        private void OnEnable()
        {
            if (isAutoButton) ScatterFx.AutoChanged += HandleAuto;
            if (idleGlow) Kick();
        }

        private void OnDisable()
        {
            if (isAutoButton) ScatterFx.AutoChanged -= HandleAuto;
            loop = null; // Unity stops coroutines on disable
            pressed = false;
            pressAmt = flash = punchT = 0f;
            if (self != null && useScale) self.localScale = baseScale;
            if (punchTarget != null) punchTarget.localScale = punchBase;
            if (glowImage != null) glowImage.enabled = false;
        }

        private void BuildGlow()
        {
            RectTransform parent = transform.parent as RectTransform;
            if (parent == null) return;

            glowImage = ScatterFx.MakeImage(parent, "FxGlow_" + name, new Color(glowColor.r, glowColor.g, glowColor.b, 0f));
            RectTransform rt = glowImage.rectTransform;
            rt.pivot = self.pivot;
            rt.anchorMin = self.anchorMin;
            rt.anchorMax = self.anchorMax;
            rt.offsetMin = self.offsetMin - new Vector2(glowExpand, glowExpand);
            rt.offsetMax = self.offsetMax + new Vector2(glowExpand, glowExpand);
            rt.SetSiblingIndex(self.GetSiblingIndex()); // behind the button
            glowImage.enabled = false;
        }

        private bool Interactable()
        {
            return button == null || button.IsInteractable();
        }

        private void Kick()
        {
            if (loop == null && isActiveAndEnabled) loop = StartCoroutine(Run());
        }

        private void HandleAuto(bool on)
        {
            activeOn = on;
            Kick();
        }

        public void OnPointerDown(PointerEventData e) { if (!Interactable()) return; pressed = true; Kick(); }
        public void OnPointerUp(PointerEventData e) { if (!pressed) return; pressed = false; Kick(); }
        public void OnPointerExit(PointerEventData e) { if (!pressed) return; pressed = false; Kick(); }

        public void OnPointerClick(PointerEventData e)
        {
            if (!Interactable()) return;
            flash = 1f;
            if (punchTarget != null) punchT = 1f;
            Kick();
        }

        private IEnumerator Run()
        {
            while (true)
            {
                float dt = Time.unscaledDeltaTime;
                clock += dt;

                pressAmt = Mathf.MoveTowards(pressAmt, pressed ? 1f : 0f, dt / (pressed ? 0.06f : 0.16f));
                flash = Mathf.MoveTowards(flash, 0f, dt / 0.3f);
                activeAmt = Mathf.MoveTowards(activeAmt, activeOn ? 1f : 0f, dt / 0.25f);
                if (punchT > 0f) punchT = Mathf.Max(0f, punchT - dt / 0.3f);

                if (useScale)
                {
                    float k = pressAmt * pressAmt * (3f - 2f * pressAmt);
                    self.localScale = baseScale * Mathf.Lerp(1f, pressedScale, k);
                }

                if (punchTarget != null)
                {
                    float u = 1f - punchT;
                    float bump = punchT > 0f ? Mathf.Sin(Mathf.Pow(u, 0.6f) * Mathf.PI) : 0f;
                    punchTarget.localScale = punchBase * (1f + (punchScale - 1f) * bump);
                }

                if (glowImage != null)
                {
                    float idle = idleGlow
                        ? Mathf.Lerp(idleMin, idleMax, 0.5f + 0.5f * Mathf.Sin(clock * Mathf.PI * 2f / Mathf.Max(0.2f, idlePeriod)))
                        : 0f;
                    float active = activeAmt * activeAlpha * (0.8f + 0.2f * Mathf.Sin(clock * 6f));
                    float a = Mathf.Max(idle, Mathf.Max(active, flash * flashAlpha));

                    Color c = Color.Lerp(glowColor, activeGlowColor, activeAmt);
                    c.a = Interactable() ? a : 0f;
                    glowImage.color = c;
                    glowImage.enabled = c.a > 0.003f;
                }

                bool finished = !idleGlow && !pressed && pressAmt <= 0f && flash <= 0f && punchT <= 0f && !activeOn && activeAmt <= 0f;
                if (finished)
                {
                    loop = null;
                    yield break;
                }
                yield return null;
            }
        }
    }
}