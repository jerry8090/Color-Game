using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Gameplay;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Final-countdown effect. Each second of the last countdown seconds it:
    ///  - flashes the ROUND box (gold turning crimson as time runs out) with a tiny scale pulse,
    ///  - and, if assigned, pulses the countdown text itself a little harder.
    /// It listens to AutoRollController.OnCountdownTick (the same event the sounds use).
    /// It never writes the countdown text, the timer or the sounds. It only changes scale and an overlay.
    /// </summary>
    public class CasinoCountdownFx : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private AutoRollController autoRoll;
        [Tooltip("TopBar > RoundImage")]
        [SerializeField] private RectTransform roundBox;
        [Tooltip("Optional. TopBar > RoundImage > Countdown (the text AutoRollController writes). Empty = no text pulse.")]
        [SerializeField] private RectTransform countdownText;
        [SerializeField] private Sprite flashSprite;
        [Tooltip("Same value as Countdown Sound Seconds on AutoRollController.")]
        [SerializeField] private int tickSeconds = 5;

        private Image flash;
        private Vector3 baseScale = Vector3.one;
        private Vector3 textBaseScale = Vector3.one;
        private Coroutine routine;
        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (controller == null || autoRoll == null || roundBox == null)
            {
                Debug.LogError("[CasinoCountdownFx] A reference is missing in the Inspector.", this);
                return;
            }

            baseScale = roundBox.localScale;
            if (countdownText != null) textBaseScale = countdownText.localScale;
            flash = CasinoUIAnimationController.CreateOverlay(roundBox, "CasinoCountdownFlash", flashSprite, new Color(1f, 1f, 1f, 0f));
            ready = true;
        }

        private void OnEnable()
        {
            if (ready) autoRoll.OnCountdownTick += HandleTick;
        }

        private void OnDisable()
        {
            if (autoRoll != null) autoRoll.OnCountdownTick -= HandleTick;
            routine = null;
            if (!ready) return;
            roundBox.localScale = baseScale;
            if (countdownText != null) countdownText.localScale = textBaseScale;
            flash.color = new Color(1f, 1f, 1f, 0f);
        }

        private void HandleTick(int secondsLeft)
        {
            // danger: 0 at the first tick, 1 at the last second
            float danger = 1f - Mathf.Clamp01((secondsLeft - 1) / (float)Mathf.Max(1, tickSeconds - 1));
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Pulse(danger));
        }

        private IEnumerator Pulse(float danger)
        {
            const float duration = 0.45f;
            Color tint = Color.Lerp(controller.Gold, controller.Crimson, danger);
            float peakAlpha = Mathf.Lerp(0.18f, 0.5f, danger);
            float peakScale = Mathf.Lerp(0.02f, 0.07f, danger);
            float peakTextScale = Mathf.Lerp(0.08f, 0.22f, danger);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                float k = Mathf.Clamp01(t);
                float wave = Mathf.Sin(k * Mathf.PI);

                tint.a = peakAlpha * (1f - k);
                flash.color = tint;
                roundBox.localScale = baseScale * (1f + peakScale * wave);
                if (countdownText != null) countdownText.localScale = textBaseScale * (1f + peakTextScale * wave);
                yield return null;
            }

            tint.a = 0f;
            flash.color = tint;
            roundBox.localScale = baseScale;
            if (countdownText != null) countdownText.localScale = textBaseScale;
            routine = null;
        }
    }
}
