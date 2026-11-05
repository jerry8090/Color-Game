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
    ///   OnRoundCompleted : WIN = winnings count-up, slot bounce and gold burst. LOSE = nothing extra.
    /// Events are subscribed in Start(), AFTER RoyalGoldHUD subscribed in OnEnable, so the HUD writes its
    /// text first and this script then animates over it without a flicker.
    /// Put this on an always-active object (for example the CasinoUI object).
    /// </summary>
    public class CasinoResultAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CasinoUIAnimationController controller;
        [SerializeField] private ColorGameManager gameManager;
        [Tooltip("RoyalGoldUI > ResultSection")]
        [SerializeField] private RectTransform resultSection;
        [Tooltip("RoyalGoldUI > ResultSection > ResultColorSlots (its 3 children are the result pills)")]
        [SerializeField] private RectTransform slotsContainer;
        [Tooltip("RoyalGoldUI > EffectsLayer (particles live here). Empty = no particles.")]
        [SerializeField] private RectTransform effectsLayer;
        [Tooltip("RoyalGoldUI > ResultSection > SummaryGroup > WinningAmountText. Empty = no count-up.")]
        [SerializeField] private TMP_Text winningAmountText;

        [Header("Optional")]
        [Tooltip("RoundedRect.png gives the gold flash rounded corners.")]
        [SerializeField] private Sprite flashSprite;
        [Tooltip("Must match the text RoyalGoldHUD writes.")]
        [SerializeField] private string winningsPrefix = "WINNINGS: \u20B1";

        private CanvasGroup sectionGroup;
        private Image flash;
        private Vector3 sectionBase = Vector3.one;
        private Vector3 slotsBase = Vector3.one;

        private Coroutine fadeRoutine;
        private Coroutine revealRoutine;
        private Coroutine countRoutine;
        private Coroutine bounceRoutine;
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

        private void OnDisable()
        {
            Unsubscribe();
            fadeRoutine = revealRoutine = countRoutine = bounceRoutine = burstRoutine = null;
            if (!ready) return;

            sectionGroup.alpha = 1f;
            resultSection.localScale = sectionBase;
            slotsContainer.localScale = slotsBase;
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
            Restart(ref revealRoutine, RevealRoutine(payout != null && payout.IsWin));
        }

        private void HandleNoBetResultShown(GameColor[] results)
        {
            Restart(ref revealRoutine, RevealRoutine(false));
        }

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            if (payout == null || !payout.IsWin) return; // a loss gets only the subtle reveal

            if (winningAmountText != null && payout.Winnings > 0)
            {
                Restart(ref countRoutine, CountUpRoutine(payout.Winnings));
            }

            Restart(ref bounceRoutine, BounceRoutine());

            if (!controller.GoldParticlesEnabled || pool == null) return;

            int count = Mathf.Min(controller.ParticlesPerSlot, pool.Length);
            for (int i = 0; i < payout.SlotMatches.Count && i < slotsContainer.childCount; i++)
            {
                if (payout.SlotMatches[i]) Burst(slotsContainer.GetChild(i).position, count);
            }
        }

        // ---------- Section animations ----------

        private void FadeTo(float target)
        {
            Restart(ref fadeRoutine, FadeRoutine(target));
        }

        private IEnumerator FadeRoutine(float target)
        {
            float from = sectionGroup.alpha;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / 0.3f;
                sectionGroup.alpha = Mathf.Lerp(from, target, CasinoUIAnimationController.EaseOutCubic(t));
                yield return null;
            }
            sectionGroup.alpha = target;
            fadeRoutine = null;
        }

        private IEnumerator RevealRoutine(bool win)
        {
            if (fadeRoutine != null) { StopCoroutine(fadeRoutine); fadeRoutine = null; }

            float duration = Mathf.Max(0.1f, controller.ResultDuration);
            float startScale = win ? controller.ResultStartScale : Mathf.Lerp(controller.ResultStartScale, 1f, 0.6f);
            float startAlpha = sectionGroup.alpha;
            float peak = controller.ResultFlashAlpha * (win ? 1f : 0.4f);
            Color tint = controller.Gold;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                float k = Mathf.Clamp01(t);
                sectionGroup.alpha = Mathf.Lerp(startAlpha, 1f, CasinoUIAnimationController.EaseOutCubic(k));
                resultSection.localScale = sectionBase * Mathf.LerpUnclamped(startScale, 1f, CasinoUIAnimationController.EaseOutBack(k));
                tint.a = peak * (1f - k);
                flash.color = tint;
                yield return null;
            }

            sectionGroup.alpha = 1f;
            resultSection.localScale = sectionBase;
            tint.a = 0f;
            flash.color = tint;
            revealRoutine = null;
        }

        private IEnumerator BounceRoutine()
        {
            const float duration = 0.35f;
            float peak = controller.WinBounceScale;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                slotsContainer.localScale = slotsBase * Mathf.Lerp(1f, peak, Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI));
                yield return null;
            }
            slotsContainer.localScale = slotsBase;
            bounceRoutine = null;
        }

        private IEnumerator CountUpRoutine(long target)
        {
            float duration = Mathf.Max(0.1f, controller.WinCountUpDuration);
            float t = 0f;
            winningAmountText.text = winningsPrefix + "0";

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / duration;
                long value = (long)Math.Round(target * (double)CasinoUIAnimationController.EaseOutCubic(t));
                winningAmountText.text = winningsPrefix + value.ToString("N0", CultureInfo.InvariantCulture);
                yield return null;
            }

            winningAmountText.text = winningsPrefix + target.ToString("N0", CultureInfo.InvariantCulture);
            countRoutine = null;
        }

        // ---------- Gold particles (fixed pool: no Instantiate or Destroy during play) ----------

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
                if (live[i]) continue;

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

        private void Restart(ref Coroutine slot, IEnumerator routine)
        {
            if (slot != null) StopCoroutine(slot);
            slot = StartCoroutine(routine);
        }
    }
}