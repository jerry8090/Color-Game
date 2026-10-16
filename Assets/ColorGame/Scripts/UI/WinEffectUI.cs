using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Visual celebration only. On WIN: gold screen flash + confetti (more matches = more confetti).
    /// On LOSE: red flash + the result tray shakes.
    /// It reacts to the PayoutResult it is given and never decides who wins.
    /// </summary>
    public class WinEffectUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [Tooltip("The object that shakes on a loss, and where confetti starts (the ResultPanel).")]
        [SerializeField] private RectTransform shakeTarget;
        [SerializeField] private Image flashImage;
        [SerializeField] private RectTransform particleContainer;
        [Tooltip("Optional. Leave empty for plain squares, or use the generated RoundedRect sprite.")]
        [SerializeField] private Sprite particleSprite;

        [Header("Flash")]
        [SerializeField] private Color winFlashColor = new Color(1f, 0.84f, 0.29f, 0.35f);
        [SerializeField] private Color loseFlashColor = new Color(0.9f, 0.1f, 0.1f, 0.28f);
        [SerializeField] private float flashDuration = 0.5f;

        [Header("Shake (lose)")]
        [SerializeField] private float shakeDuration = 0.35f;
        [SerializeField] private float shakeMagnitude = 18f;

        [Header("Confetti (win)")]
        [SerializeField] private int poolSize = 60;
        [SerializeField] private int particlesPerMatch = 20;
        [SerializeField] private float minSize = 16f;
        [SerializeField] private float maxSize = 34f;
        [SerializeField] private float minSpeed = 500f;
        [SerializeField] private float maxSpeed = 1100f;
        [SerializeField] private float gravity = 1800f;
        [SerializeField] private float minLife = 0.9f;
        [SerializeField] private float maxLife = 1.6f;
        [SerializeField] private float spinSpeed = 360f;

        private class Particle
        {
            public RectTransform rect;
            public Image image;
            public Vector2 velocity;
            public float spin;
            public float life;
            public float maxLife;
            public Color baseColor;
            public bool active;
        }

        private readonly List<Particle> pool = new List<Particle>();
        private Coroutine flashRoutine;
        private Coroutine shakeRoutine;
        private Coroutine confettiRoutine;
        private Vector2 shakeOrigin;
        private bool isReady;

        private void OnValidate()
        {
            poolSize = Mathf.Clamp(poolSize, 10, 200);
            particlesPerMatch = Mathf.Max(1, particlesPerMatch);
            maxSize = Mathf.Max(minSize, maxSize);
            maxSpeed = Mathf.Max(minSpeed, maxSpeed);
            maxLife = Mathf.Max(minLife, maxLife);
            flashDuration = Mathf.Max(0.1f, flashDuration);
            shakeDuration = Mathf.Max(0.1f, shakeDuration);
        }

        private void Awake()
        {
            if (config == null || gameManager == null || shakeTarget == null ||
                flashImage == null || particleContainer == null)
            {
                Debug.LogError("[WinEffectUI] A reference is missing in the Inspector.", this);
                return;
            }

            shakeOrigin = shakeTarget.anchoredPosition;

            flashImage.raycastTarget = false; // effects must never block taps
            flashImage.color = Color.clear;

            BuildPool();
            isReady = true;
        }

        private void OnEnable()
        {
            if (isReady) gameManager.OnRoundCompleted += HandleRoundCompleted;
        }

        private void OnDisable()
        {
            if (gameManager != null) gameManager.OnRoundCompleted -= HandleRoundCompleted;
            if (isReady) ResetEffects();
        }

        private void BuildPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = new GameObject("Confetti", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(particleContainer, false);

                Image image = go.GetComponent<Image>();
                image.raycastTarget = false;
                image.sprite = particleSprite;

                go.SetActive(false);
                pool.Add(new Particle { rect = go.GetComponent<RectTransform>(), image = image });
            }
        }

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            if (payout.IsWin)
            {
                StartFlash(winFlashColor);
                SpawnConfetti(Mathf.Min(poolSize, particlesPerMatch * payout.Matches));
            }
            else
            {
                StartFlash(loseFlashColor);
                StartShake();
            }
        }

        // ---------- Flash ----------

        private void StartFlash(Color color)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / flashDuration);
                Color c = color;
                c.a = color.a * (1f - t);
                flashImage.color = c;
                yield return null;
            }

            flashImage.color = Color.clear;
            flashRoutine = null;
        }

        // ---------- Shake ----------

        private void StartShake()
        {
            if (shakeRoutine != null) StopCoroutine(shakeRoutine);
            shakeTarget.anchoredPosition = shakeOrigin;
            shakeRoutine = StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            float elapsed = 0f;
            while (elapsed < shakeDuration)
            {
                elapsed += Time.deltaTime;
                float strength = shakeMagnitude * (1f - Mathf.Clamp01(elapsed / shakeDuration));
                shakeTarget.anchoredPosition = shakeOrigin + Random.insideUnitCircle * strength;
                yield return null;
            }

            shakeTarget.anchoredPosition = shakeOrigin;
            shakeRoutine = null;
        }

        // ---------- Confetti ----------

        private void SpawnConfetti(int count)
        {
            int spawned = 0;
            foreach (Particle particle in pool)
            {
                if (spawned >= count) break;
                if (particle.active) continue;

                Launch(particle);
                spawned++;
            }

            if (spawned > 0 && confettiRoutine == null)
            {
                confettiRoutine = StartCoroutine(ConfettiLoop());
            }
        }

        private void Launch(Particle p)
        {
            p.active = true;
            p.rect.gameObject.SetActive(true);

            float size = Random.Range(minSize, maxSize);
            p.rect.sizeDelta = new Vector2(size, size);
            p.rect.position = shakeTarget.position; // start at the middle of the result tray
            p.rect.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            float angle = Random.Range(30f, 150f) * Mathf.Deg2Rad; // fan upward
            float speed = Random.Range(minSpeed, maxSpeed);
            p.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
            p.spin = Random.Range(-spinSpeed, spinSpeed);
            p.maxLife = Random.Range(minLife, maxLife);
            p.life = p.maxLife;

            Color color = Color.white;
            if (config.Colors.Count > 0 && config.Colors[Random.Range(0, config.Colors.Count)] != null)
            {
                color = config.Colors[Random.Range(0, config.Colors.Count)].uiColor;
            }
            p.baseColor = color;
            p.image.color = color;
        }

        private IEnumerator ConfettiLoop()
        {
            bool anyActive = true;

            while (anyActive)
            {
                anyActive = false;
                float dt = Time.deltaTime;

                foreach (Particle p in pool)
                {
                    if (!p.active) continue;

                    p.life -= dt;
                    if (p.life <= 0f)
                    {
                        Deactivate(p);
                        continue;
                    }

                    p.velocity.y -= gravity * dt;
                    p.rect.anchoredPosition += p.velocity * dt;
                    p.rect.Rotate(0f, 0f, p.spin * dt);

                    Color c = p.baseColor;
                    c.a = Mathf.Clamp01(p.life / (p.maxLife * 0.35f)); // fade out in the last 35%
                    p.image.color = c;

                    anyActive = true;
                }

                yield return null;
            }

            confettiRoutine = null;
        }

        private static void Deactivate(Particle p)
        {
            p.active = false;
            p.rect.gameObject.SetActive(false);
        }

        private void ResetEffects()
        {
            if (flashRoutine != null) { StopCoroutine(flashRoutine); flashRoutine = null; }
            if (shakeRoutine != null) { StopCoroutine(shakeRoutine); shakeRoutine = null; }
            if (confettiRoutine != null) { StopCoroutine(confettiRoutine); confettiRoutine = null; }

            flashImage.color = Color.clear;
            shakeTarget.anchoredPosition = shakeOrigin;
            foreach (Particle p in pool) Deactivate(p);
        }
    }
}