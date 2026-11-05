using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Result-panel animation. DISPLAY ONLY. It never touches cubes, results, payouts or credits.
    ///   OnRollStarted    : the result section dims a little while the cubes roll.
    ///   OnResultsShown   : (the cubes have REALLY stopped) fade up + scale 92% to 100% + gold flash.
    ///   OnRoundCompleted : WIN = winnings count-up, a small bounce and gold sparks on the matching boxes.
    ///                      LOSE = nothing extra.
    /// Events are subscribed in Start(), AFTER GameResultUI subscribed in OnEnable, so GameResultUI writes its
    /// text first and the count-up then animates over it in the same frame, without a flicker.
    /// The count-up uses the exact text format GameResultUI uses: "WINNINGS: 30  |  NET +10".
    /// Result Section and Slots Container may be the same object (ResultPanel). The reveal and the bounce share
    /// one scale routine, so they can never fight over that scale.
    /// Put this on an always-active object (for example the CasinoUI object).
    /// </summary>
    public class CasinoResultAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private ColorGameManager gameManager;
        [Tooltip("The panel that holds the three result boxes (ResultPanel).")]
        [SerializeField] private RectTransform resultSection;
        [Tooltip("The object that bounces on a win. May be the same ResultPanel.")]
        [SerializeField] private RectTransform slotsContainer;
        [Tooltip("EffectsLayer (the gold sparks live here). Empty = no sparks.")]
        [SerializeField] private RectTransform effectsLayer;
        [Tooltip("WinningsText (the text GameResultUI writes). Empty = no count-up.")]
        [SerializeField] private TMP_Text winningAmountText;

        [Header("Optional")]
        [Tooltip("RoundedRect gives the gold flash rounded corners.")]
        [SerializeField] private Sprite flashSprite;

        private CanvasGroup sectionGroup;
        private Image flash;
        private Vector3 sectionBase = Vector3.one;
        private Vector3 slotsBase = Vector3.one;

        private Coroutine fadeRoutine;
        private Coroutine scaleRoutine; // reveal OR bounce, never both
        private Coroutine countRoutine;
        private Coroutine burstRoutine;

        private Image[] pool;
        private Vector2[] velocity;
        private Color[] baseColor;
        private float[] life;
        private float[] maxLife;
        private bool[] live;
        private int liveCount;

        private bool ready;
        private bool started;
        private bool subscribed;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();

            if (controller == null || gameManager == null || resultSection == null || slotsContainer == null)
            {
                Debug.LogError("[CasinoResultAnimator] A reference is missing in the Inspector.", this);
                return;
            }

            sectionGroup = resultSection.GetComponent<CanvasGroup>();
            if (sectionGroup == null) sectionGroup = resultSection.gameObject.AddComponent<CanvasGroup>();

            sectionBase = resultSection.localScale;
            slotsBase = slotsContainer.localScale;

            Color clear = controller.Gold;
            clear.a = 0f;
            flash = CasinoUIAnimationController.CreateOverlay(resultSection, "CasinoResultFlash", flashSprite, clear);

            BuildPool();
            ready = true;
        }

        private void Start()
        {
            started = true;
            Subscribe();
        }

        private void OnEnable()
        {
            if (started) Subscribe();
        }

        // True only while every Unity object this script touches still exists.
        // During a scene change Unity can destroy the CanvasGroup / panels BEFORE this component's OnDisable runs.
        private bool UiAlive()
        {
            return sectionGroup != null && resultSection != null && slotsContainer != null && flash != null;
        }

        private void OnDisable()
        {
            Unsubscribe();
            StopAllCoroutines();
            fadeRoutine = null;
            scaleRoutine = null;
            countRoutine = null;
            burstRoutine = null;
            if (!ready || !UiAlive()) return;

            sectionGroup.alpha = 1f;
            ResetScales();
            flash.color = new Color(1f, 1f, 1f, 0f);
            ClearParticles();
        }

        private void Subscribe()
        {
            if (!ready || subscribed) return;
            gameManager.OnRollStarted += HandleRollStarted;
            gameManager.OnBettingWindowOpened += HandleRestore;
            gameManager.OnRoundVoided += HandleVoided;
            gameManager.OnResultsShown += HandleResultsShown;
            gameManager.OnNoBetResultShown += HandleNoBetResultShown;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!subscribed || gameManager == null) return;
            gameManager.OnRollStarted -= HandleRollStarted;
            gameManager.OnBettingWindowOpened -= HandleRestore;
            gameManager.OnRoundVoided -= HandleVoided;
            gameManager.OnResultsShown -= HandleResultsShown;
            gameManager.OnNoBetResultShown -= HandleNoBetResultShown;
            gameManager.OnRoundCompleted -= HandleRoundCompleted;
            subscribed = false;
        }

        // ---------- Event handlers ----------

        private void HandleRollStarted(GameColor[] results) { FadeTo(controller.DimWhileRolling); }
        private void HandleRestore() { FadeTo(1f); }
        private void HandleVoided(string roundId) { FadeTo(1f); }

        private void HandleResultsShown(GameColor[] results, PayoutResult payout)
        {
            StartScale(RevealRoutine(payout != null && payout.IsWin));
        }

        private void HandleNoBetResultShown(GameColor[] results)
        {
            StartScale(RevealRoutine(false));
        }

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            if (!ready || !UiAlive()) return;
            if (payout == null || !payout.IsWin) return; // a loss gets only the subtle reveal

            if (winningAmountText != null && payout.Winnings > 0)
            {
                string net = payout.NetResult.ToString("+#,0;-#,0;0", CultureInfo.InvariantCulture);
                if (countRoutine != null) StopCoroutine(countRoutine);
                countRoutine = StartCoroutine(CountUpRoutine(payout.Winnings, net));
            }

            StartScale(BounceRoutine());

            if (!controller.GoldParticlesEnabled || pool == null) return;

            // Find the real result boxes, so the sparks start on the right ones.
            ResultSlotView[] slots = slotsContainer.GetComponentsInChildren<ResultSlotView>(false);
            int count = Mathf.Min(controller.ParticlesPerSlot, pool.Length);
            for (int i = 0; i < payout.SlotMatches.Count && i < slots.Length; i++)
            {
                if (payout.SlotMatches[i]) Burst(slots[i].transform.position, count);
            }
        }

        // ---------- Scale: reveal and bounce share ONE routine ----------

        private void StartScale(IEnumerator routine)
        {
            if (!ready || !UiAlive()) return;
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            ResetScales();
            scaleRoutine = StartCoroutine(routine);
        }

        private void ResetScales()
        {
            if (resultSection != null) resultSection.localScale = sectionBase;
            if (slotsContainer != null) slotsContainer.localScale = slotsBase;
        }

        // ---------- Section animations ----------

        private void FadeTo(float target)
        {
            if (!ready || !UiAlive()) return;
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeRoutine(target));
        }

        private IEnumerator FadeRoutine(float target)
        {
            if (!UiAlive()) { fadeRoutine = null; yield break; }
            float from = sectionGroup.alpha;
            float t = 0f;
            while (t < 1f)
            {
                if (!UiAlive()) { fadeRoutine = null; yield break; }
                t += Time.unscaledDeltaTime / 0.3f;
                sectionGroup.alpha = Mathf.Lerp(from, target, CasinoUIAnimationController.EaseOutCubic(t));
                yield return null;
            }
            if (UiAlive()) sectionGroup.alpha = target;
            fadeRoutine = null;
        }

        private IEnumerator RevealRoutine(bool win)
        {
            if (fadeRoutine != null) { StopCoroutine(fadeRoutine); fadeRoutine = null; }
            if (!UiAlive()) { scaleRoutine = null; yield break; }

            float duration = Mathf.Max(0.1f, controller.ResultDuration);
            float startScale = win ? controller.ResultStartScale : Mathf.Lerp(controller.ResultStartScale, 1f, 0.6f);
            float startAlpha = sectionGroup.alpha;
            float peak = controller.ResultFlashAlpha * (win ? 1f : 0.4f);
            Color tint = controller.Gold;
            float t = 0f;

            while (t < 1f)
            {
                if (!UiAlive()) { scaleRoutine = null; yield break; }
                t += Time.unscaledDeltaTime / duration;
                float k = Mathf.Clamp01(t);
                sectionGroup.alpha = Mathf.Lerp(startAlpha, 1f, CasinoUIAnimationController.EaseOutCubic(k));
                resultSection.localScale = sectionBase * Mathf.LerpUnclamped(startScale, 1f, CasinoUIAnimationController.EaseOutBack(k));
                tint.a = peak * (1f - k);
                flash.color = tint;
                yield return null;
            }

            if (UiAlive())
            {
                sectionGroup.alpha = 1f;
                resultSection.localScale = sectionBase;
                tint.a = 0f;
                flash.color = tint;
            }
            scaleRoutine = null;
        }

        private IEnumerator BounceRoutine()
        {
            // If a reveal was cut short, finish its alpha and flash cleanly.
            if (fadeRoutine != null) { StopCoroutine(fadeRoutine); fadeRoutine = null; }
            if (!UiAlive()) { scaleRoutine = null; yield break; }
            sectionGroup.alpha = 1f;
            Color clear = controller.Gold;
            clear.a = 0f;
            flash.color = clear;

            const float duration = 0.35f;
            float peak = controller.WinBounceScale;
            float t = 0f;
            while (t < 1f)
            {
                if (!UiAlive()) { scaleRoutine = null; yield break; }
                t += Time.unscaledDeltaTime / duration;
                slotsContainer.localScale = slotsBase * Mathf.Lerp(1f, peak, Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI));
                yield return null;
            }
            if (slotsContainer != null) slotsContainer.localScale = slotsBase;
            scaleRoutine = null;
        }

        // Same text format as GameResultUI: "WINNINGS: 30  |  NET +10". Only the winnings number counts up.
        private IEnumerator CountUpRoutine(long target, string netText)
        {
            float duration = Mathf.Max(0.1f, controller.WinCountUpDuration);
            float t = 0f;

            while (t < 1f)
            {
                if (winningAmountText == null) { countRoutine = null; yield break; }
                t += Time.unscaledDeltaTime / duration;
                long value = (long)Math.Round(target * (double)CasinoUIAnimationController.EaseOutCubic(t));
                winningAmountText.text = "WINNINGS: " + value.ToString("N0", CultureInfo.InvariantCulture) + "  |  NET " + netText;
                yield return null;
            }

            if (winningAmountText != null)
            {
                winningAmountText.text = "WINNINGS: " + target.ToString("N0", CultureInfo.InvariantCulture) + "  |  NET " + netText;
            }
            countRoutine = null;
        }

        // ---------- Gold sparks (fixed pool: no Instantiate or Destroy during play) ----------

        private void BuildPool()
        {
            if (effectsLayer == null) return;

            int size = controller.ParticlePoolSize;
            pool = new Image[size];
            velocity = new Vector2[size];
            baseColor = new Color[size];
            life = new float[size];
            maxLife = new float[size];
            live = new bool[size];

            for (int i = 0; i < size; i++)
            {
                GameObject go = new GameObject("GoldSpark", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(effectsLayer, false);
                Image image = go.GetComponent<Image>();
                image.sprite = CasinoSprites.SoftDot;
                image.raycastTarget = false;
                go.SetActive(false);
                pool[i] = image;
            }
        }

        private void Burst(Vector3 worldPosition, int count)
        {
            for (int i = 0; i < pool.Length && count > 0; i++)
            {
                if (live[i] || pool[i] == null) continue;

                float size = UnityEngine.Random.Range(controller.ParticleSize.x, controller.ParticleSize.y);
                float angle = UnityEngine.Random.Range(20f, 160f) * Mathf.Deg2Rad;
                float speed = controller.ParticleSpeed * UnityEngine.Random.Range(0.5f, 1f);

                RectTransform rect = pool[i].rectTransform;
                rect.sizeDelta = new Vector2(size, size);
                rect.position = worldPosition;

                velocity[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                maxLife[i] = controller.ParticleLife * UnityEngine.Random.Range(0.7f, 1f);
                life[i] = maxLife[i];
                baseColor[i] = Color.Lerp(controller.Gold, Color.white, UnityEngine.Random.value * 0.5f);
                pool[i].color = baseColor[i];
                pool[i].gameObject.SetActive(true);
                live[i] = true;
                liveCount++;
                count--;
            }

            if (burstRoutine == null && liveCount > 0) burstRoutine = StartCoroutine(BurstLoop());
        }

        private IEnumerator BurstLoop()
        {
            while (liveCount > 0)
            {
                float dt = Time.unscaledDeltaTime;

                for (int i = 0; i < pool.Length; i++)
                {
                    if (!live[i]) continue;

                    if (pool[i] == null) // destroyed with its parent
                    {
                        live[i] = false;
                        liveCount--;
                        continue;
                    }

                    life[i] -= dt;
                    if (life[i] <= 0f)
                    {
                        live[i] = false;
                        liveCount--;
                        pool[i].gameObject.SetActive(false);
                        continue;
                    }

                    velocity[i].y -= controller.ParticleGravity * dt;
                    pool[i].rectTransform.anchoredPosition += velocity[i] * dt;

                    Color c = baseColor[i];
                    c.a = Mathf.Clamp01(life[i] / (maxLife[i] * 0.4f));
                    pool[i].color = c;
                }

                yield return null;
            }

            burstRoutine = null;
        }

        private void ClearParticles()
        {
            if (pool == null) return;
            for (int i = 0; i < pool.Length; i++)
            {
                live[i] = false;
                if (pool[i] != null) pool[i].gameObject.SetActive(false);
            }
            liveCount = 0;
        }
    }
}