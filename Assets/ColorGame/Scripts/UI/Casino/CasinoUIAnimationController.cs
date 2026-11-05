using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// ONE object per scene holding every animation setting. Other Casino scripts read from it.
    /// Its only Update drives the shared gold "breathing" glow, and only while glow components exist.
    /// </summary>
    public class CasinoUIAnimationController : MonoBehaviour
    {
        [Header("Master switches")]
        [SerializeField] private bool idleAnimationsEnabled = true;
        [SerializeField] private bool goldParticlesEnabled = true;
        [Tooltip("Halves particles, caps the pool and updates glow at 30 Hz.")]
        [SerializeField] private bool mobilePerformanceMode = true;

        [Header("Palette")]
        [SerializeField] private Color gold = new Color(1f, 0.84f, 0.29f, 1f);
        [SerializeField] private Color crimson = new Color(1f, 0.25f, 0.25f, 1f);

        [Header("Buttons")]
        [SerializeField, Range(0.8f, 1f)] private float buttonPressScale = 0.94f;
        [SerializeField] private float buttonAnimDuration = 0.12f;

        [Header("Gold glow (idle breathing)")]
        [SerializeField] private float glowSpeed = 2.2f;
        [SerializeField, Range(0f, 1f)] private float glowMinAlpha = 0.35f;
        [SerializeField, Range(0f, 1f)] private float glowMaxAlpha = 0.9f;

        [Header("Gold shimmer")]
        [SerializeField] private float shimmerDuration = 0.9f;
        [Tooltip("Seconds between sweeps.")]
        [SerializeField] private float shimmerInterval = 5f;
        [SerializeField, Range(0f, 1f)] private float shimmerIntensity = 0.22f;
        [SerializeField, Range(0.1f, 0.6f)] private float shimmerWidth = 0.28f;

        [Header("Panels")]
        [SerializeField] private float panelFadeDuration = 0.25f;
        [SerializeField, Range(0.5f, 1f)] private float panelStartScale = 0.92f;

        [Header("Result")]
        [SerializeField] private float resultDuration = 0.45f;
        [SerializeField, Range(0.5f, 1f)] private float resultStartScale = 0.92f;
        [Tooltip("Result section alpha while the cubes are rolling. 1 = no dimming.")]
        [SerializeField, Range(0.3f, 1f)] private float dimWhileRolling = 0.7f;
        [SerializeField, Range(0f, 1f)] private float resultFlashAlpha = 0.3f;
        [SerializeField] private float winCountUpDuration = 0.7f;
        [SerializeField, Range(1f, 1.2f)] private float winBounceScale = 1.06f;

        [Header("Gold particles")]
        [SerializeField] private int particlesPerSlot = 10;
        [SerializeField] private float particleSpeed = 420f;
        [SerializeField] private float particleLife = 0.9f;
        [SerializeField] private float particleGravity = 900f;
        [SerializeField] private Vector2 particleSize = new Vector2(14f, 26f);

        [Header("Text pop (round number)")]
        [SerializeField, Range(1f, 1.5f)] private float textPopScale = 1.18f;
        [SerializeField] private float textPopDuration = 0.35f;

        public bool IdleAnimationsEnabled => idleAnimationsEnabled;
        public bool GoldParticlesEnabled => goldParticlesEnabled;
        public Color Gold => gold;
        public Color Crimson => crimson;
        public float ButtonPressScale => buttonPressScale;
        public float ButtonAnimDuration => buttonAnimDuration;
        public float ShimmerDuration => shimmerDuration;
        public float ShimmerInterval => Mathf.Max(1f, shimmerInterval);
        public float ShimmerIntensity => shimmerIntensity;
        public float ShimmerWidth => shimmerWidth;
        public float PanelFadeDuration => panelFadeDuration;
        public float PanelStartScale => panelStartScale;
        public float ResultDuration => resultDuration;
        public float ResultStartScale => resultStartScale;
        public float DimWhileRolling => dimWhileRolling;
        public float ResultFlashAlpha => resultFlashAlpha;
        public float WinCountUpDuration => winCountUpDuration;
        public float WinBounceScale => winBounceScale;
        public int ParticlesPerSlot => mobilePerformanceMode ? Mathf.Max(3, particlesPerSlot / 2) : particlesPerSlot;
        public int ParticlePoolSize => mobilePerformanceMode ? 24 : 48;
        public float ParticleSpeed => particleSpeed;
        public float ParticleLife => particleLife;
        public float ParticleGravity => particleGravity;
        public Vector2 ParticleSize => particleSize;
        public float TextPopScale => textPopScale;
        public float TextPopDuration => textPopDuration;

        private readonly List<CasinoGlowPulse> pulses = new List<CasinoGlowPulse>();
        private float nextTick;

        private void OnValidate()
        {
            glowMaxAlpha = Mathf.Max(glowMinAlpha, glowMaxAlpha);
            buttonAnimDuration = Mathf.Max(0.03f, buttonAnimDuration);
            particlesPerSlot = Mathf.Clamp(particlesPerSlot, 0, 30);
        }

        private void Awake()
        {
            enabled = pulses.Count > 0;
        }

        public void Register(CasinoGlowPulse pulse)
        {
            if (!pulses.Contains(pulse)) pulses.Add(pulse);
            enabled = true;
        }

        public void Unregister(CasinoGlowPulse pulse)
        {
            pulses.Remove(pulse);
            if (pulses.Count == 0) enabled = false;
        }

        private void Update()
        {
            if (mobilePerformanceMode)
            {
                if (Time.unscaledTime < nextTick) return;
                nextTick = Time.unscaledTime + 1f / 30f;
            }

            float k = idleAnimationsEnabled ? 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * glowSpeed) : 1f;
            float alpha = Mathf.Lerp(glowMinAlpha, glowMaxAlpha, k);
            for (int i = 0; i < pulses.Count; i++) pulses[i].Apply(alpha);
        }

        // ---------- Shared helpers ----------

        public static float EaseOutCubic(float t)
        {
            t = Mathf.Clamp01(t);
            float u = 1f - t;
            return 1f - u * u * u;
        }

        public static float EaseOutBack(float t)
        {
            t = Mathf.Clamp01(t);
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            float u = t - 1f;
            return 1f + c3 * u * u * u + c1 * u * u;
        }

        /// <summary>A stretched, non-raycast Image used for flashes. Created once, never per frame.</summary>
        public static Image CreateOverlay(RectTransform parent, string name, Sprite sprite, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.type = sprite != null ? Image.Type.Sliced : Image.Type.Simple;
            image.color = color;
            image.raycastTarget = false;
            return image;
        }
    }

    /// <summary>Tiny procedural sprites (created once, shared). No art files needed.</summary>
    public static class CasinoSprites
    {
        private static Sprite softBar;
        private static Sprite softDot;

        public static Sprite SoftBar
        {
            get { if (softBar == null) softBar = BuildBar(); return softBar; }
        }

        public static Sprite SoftDot
        {
            get { if (softDot == null) softDot = BuildDot(); return softDot; }
        }

        private static Sprite BuildBar()
        {
            const int w = 64, h = 4;
            Texture2D tex = NewTexture(w, h);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float a = Mathf.Sin(x / (w - 1f) * Mathf.PI);
                    a *= a;
                    px[y * w + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
            }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return MakeSprite(tex);
        }

        private static Sprite BuildDot()
        {
            const int s = 32;
            Texture2D tex = NewTexture(s, s);
            Color32[] px = new Color32[s * s];
            float c = (s - 1) * 0.5f;
            for (int y = 0; y < s; y++)
            {
                for (int x = 0; x < s; x++)
                {
                    float d = Mathf.Sqrt((x - c) * (x - c) + (y - c) * (y - c)) / c;
                    float a = Mathf.Clamp01(1f - d);
                    a *= a;
                    px[y * s + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
            }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return MakeSprite(tex);
        }

        private static Texture2D NewTexture(int w, int h)
        {
            return new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };
        }

        private static Sprite MakeSprite(Texture2D tex)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
            sprite.hideFlags = HideFlags.HideAndDontSave;
            return sprite;
        }
    }
}