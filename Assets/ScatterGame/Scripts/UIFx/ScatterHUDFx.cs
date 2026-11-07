using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Extra feedback for the EXISTING HUD texts. It never sets text, only colour and scale of the objects you assign.
    /// - Multiplier: gold colour flash only (ScatterHUD already does the scale pop; this avoids fighting it).
    /// - Win / Balance: colour flash on wins and a scale pop when the round is credited.
    /// - Big / Mega win: short, soft gold screen flash.
    /// - Scene start: a black curtain that fades out (hides the hard cut from the menu).
    /// </summary>
    public class ScatterHudFx : MonoBehaviour
    {
        [Header("Existing HUD texts (assign the 'Value' objects)")]
        [SerializeField] private TMP_Text winText;
        [SerializeField] private TMP_Text balanceText;
        [SerializeField] private TMP_Text multiplierText;

        [Header("Look")]
        [SerializeField] private Color flashColor = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private float winPopAmount = 0.25f;
        [SerializeField] private float balancePopAmount = 0.12f;

        [Header("Big win screen flash")]
        [SerializeField] private bool bigWinFlash = true;
        [Tooltip("Keep equal to ScatterGameController > Big / Mega Win Multiple.")]
        [SerializeField] private float bigWinMultiple = 10f;
        [SerializeField] private float megaWinMultiple = 25f;
        [SerializeField, Range(0f, 0.5f)] private float bigFlashAlpha = 0.18f;
        [SerializeField, Range(0f, 0.5f)] private float megaFlashAlpha = 0.3f;
        [SerializeField] private float flashSeconds = 0.55f;

        [Header("Scene fade-in")]
        [SerializeField] private bool fadeInOnStart = true;
        [SerializeField] private float curtainHold = 0.05f;
        [SerializeField] private float curtainFade = 0.35f;

        private sealed class Pulse
        {
            public TMP_Text text;
            public RectTransform rt;
            public Vector3 baseScale;
            public Color baseColor;
            public float scaleT, colorT, amp;
        }

        private Pulse win, balance, mult;
        private Image curtain, flash;
        private float curtainClock, flashT, flashPeak;
        private bool curtainActive;
        private Coroutine loop;
        private bool initialized;

        private void Awake()
        {
            win = Make(winText);
            balance = Make(balanceText);
            mult = Make(multiplierText);

            TMP_Text any = winText != null ? winText : (balanceText != null ? balanceText : multiplierText);
            Canvas canvas = any != null ? any.GetComponentInParent<Canvas>() : null;
            if (canvas == null)
            {
                Debug.LogError("[ScatterHudFx] Assign at least one HUD text.", this);
                enabled = false;
                return;
            }

            RectTransform layer = ScatterFx.GetLayer(canvas.rootCanvas.transform);

            flash = ScatterFx.MakeImage(layer, "FxFlash", new Color(1f, 0.82f, 0.3f, 0f));
            flash.sprite = null; // plain full-screen colour
            ScatterFx.Stretch(flash.rectTransform, 0f);
            flash.enabled = false;

            if (fadeInOnStart)
            {
                curtain = ScatterFx.MakeImage(layer, "FxCurtain", Color.black);
                curtain.sprite = null;
                ScatterFx.Stretch(curtain.rectTransform, 0f);
                curtain.rectTransform.SetAsLastSibling();
            }

            initialized = true;
        }

        private static Pulse Make(TMP_Text t)
        {
            if (t == null) return null;
            return new Pulse { text = t, rt = t.rectTransform, baseScale = t.rectTransform.localScale, baseColor = t.color };
        }

        private void OnEnable()
        {
            if (!initialized) return;
            ScatterFx.CascadeWin += HandleCascadeWin;
            ScatterFx.RoundSettled += HandleRoundSettled;
        }

        private void OnDisable()
        {
            ScatterFx.CascadeWin -= HandleCascadeWin;
            ScatterFx.RoundSettled -= HandleRoundSettled;
            if (!initialized) return;

            loop = null;
            Reset(win); Reset(balance); Reset(mult);
            if (curtain != null) curtain.enabled = false;
            if (flash != null) flash.enabled = false;
        }

        private void Start()
        {
            if (curtain != null)
            {
                curtainActive = true;
                curtainClock = 0f;
                Kick();
            }
        }

        private static void Reset(Pulse p)
        {
            if (p == null || p.text == null) return;
            p.rt.localScale = p.baseScale;
            p.text.color = p.baseColor;
            p.scaleT = p.colorT = 0f;
        }

        private void HandleCascadeWin(System.Collections.Generic.IReadOnlyCollection<ScatterGame.Core.GridPosition> cells, int multiplier, long winMilli, long bet)
        {
            Fire(mult, 0f, 1f);
            Fire(win, 0f, 0.8f);
        }

        private void HandleRoundSettled(long creditsPaid, long bet)
        {
            if (creditsPaid <= 0) return;

            Fire(win, winPopAmount, 1f);
            Fire(balance, balancePopAmount, 0.8f);

            if (bigWinFlash && bet > 0 && flash != null)
            {
                if (creditsPaid >= bet * megaWinMultiple) StartFlash(megaFlashAlpha);
                else if (creditsPaid >= bet * bigWinMultiple) StartFlash(bigFlashAlpha);
            }
        }

        private void StartFlash(float peak)
        {
            flashPeak = peak;
            flashT = 1f;
            flash.rectTransform.SetAsLastSibling();
            flash.enabled = true;
            if (curtain != null) curtain.rectTransform.SetAsLastSibling();
            Kick();
        }

        private void Fire(Pulse p, float amp, float color)
        {
            if (p == null || p.text == null) return;
            if (amp > 0f) { p.amp = amp; p.scaleT = 1f; }
            if (color > 0f) p.colorT = Mathf.Max(p.colorT, color);
            Kick();
        }

        private void Kick()
        {
            if (loop == null && isActiveAndEnabled) loop = StartCoroutine(Run());
        }

        private bool Step(Pulse p, float dt)
        {
            if (p == null || p.text == null) return false;
            bool active = false;

            if (p.scaleT > 0f)
            {
                p.scaleT = Mathf.Max(0f, p.scaleT - dt / 0.35f);
                if (p.scaleT > 0f)
                {
                    float u = 1f - p.scaleT;
                    p.rt.localScale = p.baseScale * (1f + p.amp * Mathf.Sin(Mathf.Pow(u, 0.6f) * Mathf.PI));
                    active = true;
                }
                else p.rt.localScale = p.baseScale;
            }

            if (p.colorT > 0f)
            {
                p.colorT = Mathf.Max(0f, p.colorT - dt / 0.5f);
                p.text.color = Color.Lerp(p.baseColor, flashColor, p.colorT);
                active |= p.colorT > 0f;
            }
            return active;
        }

        private IEnumerator Run()
        {
            while (true)
            {
                float dt = Time.unscaledDeltaTime;
                bool any = false;
                any |= Step(win, dt);
                any |= Step(balance, dt);
                any |= Step(mult, dt);

                if (curtainActive)
                {
                    curtainClock += dt;
                    float k = Mathf.Clamp01((curtainClock - curtainHold) / Mathf.Max(0.05f, curtainFade));
                    Color c = curtain.color;
                    c.a = 1f - k;
                    curtain.color = c;
                    if (k >= 1f) { curtain.enabled = false; curtainActive = false; }
                    else any = true;
                }

                if (flashT > 0f)
                {
                    flashT = Mathf.Max(0f, flashT - dt / Mathf.Max(0.1f, flashSeconds));
                    float a = flashPeak * Mathf.Sin((1f - flashT) * Mathf.PI);
                    flash.color = new Color(1f, 0.82f, 0.3f, a);
                    if (flashT <= 0f) flash.enabled = false;
                    else any = true;
                }

                if (!any)
                {
                    loop = null;
                    yield break;
                }
                yield return null;
            }
        }
    }
}