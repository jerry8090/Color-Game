using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScatterGame.Core;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Notification hub + tiny helpers for the UI animations. Gameplay only RAISES events (display-only data);
    /// nothing here can change a result. Subscribers are guarded so an effect bug never breaks a round.
    /// </summary>
    public static class ScatterFx
    {
        public static event Action<IReadOnlyCollection<GridPosition>, int, long, long> CascadeWin; // cells, multiplier, winMilli, bet
        public static event Action<long, long> RoundSettled;                                       // creditsPaid, bet
        public static event Action<bool> AutoChanged;

        public static void RaiseCascadeWin(IReadOnlyCollection<GridPosition> cells, int multiplier, long winMilli, long bet)
        {
            try { CascadeWin?.Invoke(cells, multiplier, winMilli, bet); } catch (Exception e) { Debug.LogException(e); }
        }

        public static void RaiseRoundSettled(long creditsPaid, long bet)
        {
            try { RoundSettled?.Invoke(creditsPaid, bet); } catch (Exception e) { Debug.LogException(e); }
        }

        public static void RaiseAutoChanged(bool on)
        {
            try { AutoChanged?.Invoke(on); } catch (Exception e) { Debug.LogException(e); }
        }

        // ---------- Helpers ----------

        private static Sprite glow;

        /// <summary>Soft white radial dot, generated once in code (no asset needed). Tint it with Image.color.</summary>
        public static Sprite GlowSprite
        {
            get
            {
                if (glow == null) glow = BuildGlow();
                return glow;
            }
        }

        private static Sprite BuildGlow()
        {
            const int n = 64;
            Texture2D tex = new Texture2D(n, n, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color32[] px = new Color32[n * n];
            float c = (n - 1) * 0.5f;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    float d = Mathf.Sqrt((x - c) * (x - c) + (y - c) * (y - c)) / c;
                    float a = Mathf.Clamp01(1f - d);
                    a = a * a * (3f - 2f * a); // smoothstep falloff
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
            }

            tex.SetPixels32(px);
            tex.Apply(false, true);

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 100f);
            sprite.hideFlags = HideFlags.HideAndDontSave;
            return sprite;
        }

        public static float EaseOutCubic(float t)
        {
            t = 1f - Mathf.Clamp01(t);
            return 1f - t * t * t;
        }

        /// <summary>Subtle overshoot (same curve family as SlotGridView). 0 -> 0, 1 -> exactly 1.</summary>
        public static float EaseOutBack(float t)
        {
            t = Mathf.Clamp01(t);
            const float c1 = 1.2f;
            float u = t - 1f;
            return 1f + (c1 + 1f) * u * u * u + c1 * u * u;
        }

        public static Image MakeImage(Transform parent, string name, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Image image = go.GetComponent<Image>();
            image.sprite = GlowSprite;
            image.color = color;
            image.raycastTarget = false; // effects must never block taps
            return image;
        }

        public static void Stretch(RectTransform rt, float expand)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(-expand, -expand);
            rt.offsetMax = new Vector2(expand, expand);
        }

        /// <summary>One full-screen, non-blocking layer on the root canvas for sparkles, flash and curtain. Kept last = on top.</summary>
        public static RectTransform GetLayer(Transform rootCanvas)
        {
            Transform existing = rootCanvas.Find("FxLayer");
            RectTransform layer;
            if (existing != null)
            {
                layer = (RectTransform)existing;
            }
            else
            {
                GameObject go = new GameObject("FxLayer", typeof(RectTransform));
                go.transform.SetParent(rootCanvas, false);
                layer = (RectTransform)go.transform;
                layer.anchorMin = Vector2.zero;
                layer.anchorMax = Vector2.one;
                layer.offsetMin = Vector2.zero;
                layer.offsetMax = Vector2.zero;
            }
            layer.SetAsLastSibling();
            return layer;
        }
    }
}