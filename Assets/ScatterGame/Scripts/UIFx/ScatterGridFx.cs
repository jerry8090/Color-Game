using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScatterGame.Core;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Animation layer on top of the EXISTING 5x4 grid. It never changes symbols, spin timing or results.
    ///  1) First-open card entrance (staggered fade + scale + slight flip + gold shimmer). Only the cell ROOT is touched
    ///     (SlotGridView only touches each cell's Content), and every cell is restored exactly afterwards.
    ///  2) Winning cells: gold glow pulse. Non-winning cells dim briefly (optional).
    ///  3) Pooled gold sparkles on medium/big wins (no Instantiate/Destroy during play).
    /// Cell order is the hierarchy order built by ScatterSceneBuilder: index = reel * Rows + row.
    /// </summary>
    public class ScatterGridFx : MonoBehaviour
    {
        private const int Cells = SlotConfig.Reels * SlotConfig.Rows;
        private static bool entrancePlayedThisSession;

        [Header("References")]
        [Tooltip("The GridFrame object (has SlotGridView). Auto-found if empty.")]
        [SerializeField] private SlotGridView grid;

        [Header("First-open entrance")]
        [SerializeField] private bool playEntrance = true;
        [Tooltip("OFF: plays every time the Scatter screen opens (it is reloaded from the menu). ON: only the first time this session.")]
        [SerializeField] private bool oncePerSession = false;
        [SerializeField] private float startDelay = 0.25f;
        [SerializeField] private float stagger = 0.05f;
        [SerializeField] private float duration = 0.45f;
        [SerializeField, Range(0.5f, 1f)] private float startScale = 0.82f;
        [Tooltip("Slight Y rotation at the start (degrees). 0 = no flip.")]
        [SerializeField] private float startYaw = -35f;
        [SerializeField] private float startOffsetY = -24f;
        [SerializeField, Range(0f, 1f)] private float shimmerAlpha = 0.45f;

        [Header("Win highlight")]
        [SerializeField] private Color glowColor = new Color(1f, 0.82f, 0.3f, 1f);
        [SerializeField] private float glowExpand = 8f;
        [SerializeField] private float glowSeconds = 0.9f;
        [Tooltip("Cascade win >= this many times the bet counts as MEDIUM.")]
        [SerializeField] private float mediumWinMultiple = 3f;
        [Tooltip("Keep equal to ScatterGameController > Big Win Multiple.")]
        [SerializeField] private float bigWinMultiple = 10f;
        [SerializeField] private bool dimNonWinning = true;
        [SerializeField, Range(0.3f, 1f)] private float dimAlpha = 0.6f;

        [Header("Sparkles")]
        [SerializeField] private int sparklePool = 36;
        [SerializeField] private int mediumBurst = 9;
        [SerializeField] private int bigBurst = 21;

        private SlotSymbolView[] views;
        private RectTransform[] rects;
        private CanvasGroup[] groups;
        private Image[] glows;
        private Vector3[] baseScale;
        private Quaternion[] baseRot;
        private Vector2[] basePos;
        private float[] glowT, glowDur, glowPeak, dimT;
        private bool[] winScratch;

        private Image[] sparks;
        private Vector2[] sPos, sVel;
        private float[] sLife, sMax, sSize;
        private int nextSpark;
        private RectTransform layer;

        private bool initialized, entranceRunning;
        private Coroutine loop;

        private void Awake()
        {
            if (grid == null) grid = FindAnyObjectByType<SlotGridView>();
            Canvas canvas = grid != null ? grid.GetComponentInParent<Canvas>() : null;
            if (grid == null || canvas == null)
            {
                Debug.LogError("[ScatterGridFx] SlotGridView or its Canvas was not found.", this);
                enabled = false;
                return;
            }

            views = grid.GetComponentsInChildren<SlotSymbolView>(true);
            if (views.Length != Cells)
            {
                Debug.LogWarning("[ScatterGridFx] Expected " + Cells + " cells but found " + views.Length + ". Effects disabled.", this);
                enabled = false;
                return;
            }

            rects = new RectTransform[Cells];
            groups = new CanvasGroup[Cells];
            glows = new Image[Cells];
            baseScale = new Vector3[Cells];
            baseRot = new Quaternion[Cells];
            basePos = new Vector2[Cells];
            glowT = new float[Cells];
            glowDur = new float[Cells];
            glowPeak = new float[Cells];
            dimT = new float[Cells];
            winScratch = new bool[Cells];

            for (int i = 0; i < Cells; i++)
            {
                RectTransform rt = (RectTransform)views[i].transform;
                rects[i] = rt;
                baseScale[i] = rt.localScale;
                baseRot[i] = rt.localRotation;
                basePos[i] = rt.anchoredPosition;

                CanvasGroup group = rt.GetComponent<CanvasGroup>();
                groups[i] = group != null ? group : rt.gameObject.AddComponent<CanvasGroup>();

                Image glow = ScatterFx.MakeImage(rt, "FxGlow", new Color(glowColor.r, glowColor.g, glowColor.b, 0f));
                ScatterFx.Stretch(glow.rectTransform, glowExpand);
                glow.rectTransform.SetSiblingIndex(0); // behind the symbol
                glow.enabled = false;
                glows[i] = glow;
            }

            layer = ScatterFx.GetLayer(canvas.rootCanvas.transform);
            int n = Mathf.Max(8, sparklePool);
            sparks = new Image[n];
            sPos = new Vector2[n];
            sVel = new Vector2[n];
            sLife = new float[n];
            sMax = new float[n];
            sSize = new float[n];
            for (int i = 0; i < n; i++)
            {
                Image s = ScatterFx.MakeImage(layer, "Spark", Color.white);
                s.rectTransform.anchorMin = s.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                s.enabled = false;
                sparks[i] = s;
                sLife[i] = sMax[i] = 1f;
            }

            // Hide the cards BEFORE the first frame so they never flash at full size.
            entranceRunning = playEntrance && !(oncePerSession && entrancePlayedThisSession);
            if (entranceRunning)
            {
                for (int i = 0; i < Cells; i++) groups[i].alpha = 0f;
            }

            initialized = true;
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

            StopAllCoroutines();
            loop = null;
            entranceRunning = false;
            for (int i = 0; i < Cells; i++)
            {
                if (rects[i] == null) continue;
                RestoreCell(i);
                groups[i].alpha = 1f;
                glows[i].enabled = false;
                glowT[i] = dimT[i] = 0f;
            }
            for (int i = 0; i < sparks.Length; i++) if (sparks[i] != null) sparks[i].enabled = false;
        }

        private void Start()
        {
            if (entranceRunning) StartCoroutine(Entrance());
        }

        // ---------- Entrance ----------

        private IEnumerator Entrance()
        {
            yield return new WaitForSecondsRealtime(startDelay);

            float total = ((SlotConfig.Reels - 1) + (SlotConfig.Rows - 1)) * stagger + duration;
            float t = 0f;
            while (t < total)
            {
                t += Time.unscaledDeltaTime;
                for (int i = 0; i < Cells; i++)
                {
                    int reel = i / SlotConfig.Rows;
                    int row = i % SlotConfig.Rows;
                    ApplyEntrance(i, Mathf.Clamp01((t - (reel + row) * stagger) / Mathf.Max(0.05f, duration)));
                }
                yield return null;
            }

            for (int i = 0; i < Cells; i++)
            {
                RestoreCell(i);
                groups[i].alpha = 1f;
                glows[i].enabled = false;
            }
            entranceRunning = false;
            entrancePlayedThisSession = true;
        }

        private void ApplyEntrance(int i, float k)
        {
            float s = Mathf.LerpUnclamped(startScale, 1f, ScatterFx.EaseOutBack(k));
            float ease = ScatterFx.EaseOutCubic(k);

            rects[i].localScale = new Vector3(baseScale[i].x * s, baseScale[i].y * s, baseScale[i].z);
            rects[i].localRotation = baseRot[i] * Quaternion.Euler(0f, Mathf.Lerp(startYaw, 0f, ease), 0f);
            rects[i].anchoredPosition = basePos[i] + new Vector2(0f, Mathf.Lerp(startOffsetY, 0f, ease));
            groups[i].alpha = Mathf.Clamp01(k * 2.2f);

            float shimmer = shimmerAlpha * Mathf.Sin(k * Mathf.PI);
            glows[i].color = new Color(glowColor.r, glowColor.g, glowColor.b, shimmer);
            glows[i].enabled = shimmer > 0.01f;
        }

        private void RestoreCell(int i)
        {
            rects[i].localScale = baseScale[i];
            rects[i].localRotation = baseRot[i];
            rects[i].anchoredPosition = basePos[i];
        }

        // ---------- Wins ----------

        private void HandleCascadeWin(IReadOnlyCollection<GridPosition> positions, int multiplier, long winMilli, long bet)
        {
            if (bet <= 0 || positions == null || positions.Count == 0) return;

            float ratio = winMilli / (1000f * bet);
            int tier = ratio >= bigWinMultiple ? 2 : (ratio >= mediumWinMultiple ? 1 : 0);
            float speed = Mathf.Max(0.25f, grid.Speed); // follows the controller's fast-forward
            float dur = glowSeconds / speed;
            float peak = tier == 0 ? 0.55f : (tier == 1 ? 0.8f : 1f);

            for (int i = 0; i < Cells; i++) winScratch[i] = false;
            foreach (GridPosition p in positions)
            {
                int i = p.Reel * SlotConfig.Rows + p.Row;
                winScratch[i] = true;
                glowT[i] = 1f;
                glowDur[i] = dur;
                glowPeak[i] = peak;
            }

            if (dimNonWinning && !entranceRunning)
            {
                for (int i = 0; i < Cells; i++) if (!winScratch[i]) dimT[i] = dur * 0.6f;
            }

            if (tier == 1) SpawnBursts(positions, mediumBurst);
            else if (tier == 2) SpawnBursts(positions, bigBurst);

            Kick();
        }

        private void HandleRoundSettled(long creditsPaid, long bet)
        {
            if (creditsPaid <= 0 || bet <= 0) return;
            if (creditsPaid >= bet * bigWinMultiple)
            {
                Burst(rects[(SlotConfig.Reels / 2) * SlotConfig.Rows + 1], bigBurst / 2);
                Burst(rects[(SlotConfig.Reels / 2) * SlotConfig.Rows + 2], bigBurst / 2);
                Kick();
            }
        }

        // ---------- Sparkles ----------

        private void SpawnBursts(IReadOnlyCollection<GridPosition> positions, int total)
        {
            if (total <= 0) return;
            int picks = Mathf.Min(3, positions.Count);
            int step = Mathf.Max(1, positions.Count / picks);
            int per = Mathf.Max(1, total / picks);

            int index = 0, made = 0;
            foreach (GridPosition p in positions)
            {
                if (index++ % step != 0) continue;
                if (made++ >= picks) break;
                Burst(rects[p.Reel * SlotConfig.Rows + p.Row], per);
            }
        }

        private void Burst(RectTransform cell, int count)
        {
            layer.SetAsLastSibling();
            Vector2 origin = layer.InverseTransformPoint(cell.TransformPoint(cell.rect.center));

            for (int n = 0; n < count; n++)
            {
                int s = nextSpark;
                nextSpark = (nextSpark + 1) % sparks.Length;

                float angle = Random.value * Mathf.PI * 2f;
                float speed = Random.Range(180f, 420f);
                sPos[s] = origin;
                sVel[s] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                sLife[s] = 0f;
                sMax[s] = Random.Range(0.45f, 0.8f);
                sSize[s] = Random.Range(14f, 28f);

                sparks[s].rectTransform.sizeDelta = new Vector2(sSize[s], sSize[s]);
                sparks[s].rectTransform.anchoredPosition = origin;
                sparks[s].enabled = true;
            }
        }

        // ---------- Loop (runs only while something is animating) ----------

        private void Kick()
        {
            if (loop == null && isActiveAndEnabled) loop = StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (true)
            {
                float dt = Time.unscaledDeltaTime;
                bool any = false;

                for (int i = 0; i < Cells; i++)
                {
                    if (glowT[i] > 0f)
                    {
                        glowT[i] -= dt / Mathf.Max(0.05f, glowDur[i]);
                        float u = 1f - Mathf.Clamp01(glowT[i]);
                        float env = Mathf.Clamp01(u / 0.12f) * Mathf.Clamp01((1f - u) / 0.3f);
                        float pulse = 0.75f + 0.25f * Mathf.Sin(u * Mathf.PI * 4f);
                        float a = glowPeak[i] * env * pulse;

                        glows[i].color = new Color(glowColor.r, glowColor.g, glowColor.b, a);
                        glows[i].rectTransform.localScale = Vector3.one * (1f + 0.12f * env * pulse);
                        glows[i].enabled = glowT[i] > 0f && a > 0.01f;
                        any = true;
                    }

                    if (!entranceRunning)
                    {
                        float target = dimT[i] > 0f ? dimAlpha : 1f;
                        if (dimT[i] > 0f) { dimT[i] -= dt; any = true; }
                        if (!Mathf.Approximately(groups[i].alpha, target))
                        {
                            groups[i].alpha = Mathf.MoveTowards(groups[i].alpha, target, dt / 0.15f);
                            any = true;
                        }
                    }
                }

                for (int s = 0; s < sparks.Length; s++)
                {
                    if (!sparks[s].enabled) continue;

                    sLife[s] += dt;
                    float k = sLife[s] / sMax[s];
                    if (k >= 1f) { sparks[s].enabled = false; continue; }

                    sVel[s].y -= 520f * dt;
                    sPos[s] += sVel[s] * dt;
                    sparks[s].rectTransform.anchoredPosition = sPos[s];
                    sparks[s].rectTransform.localScale = Vector3.one * (1f - 0.5f * k);
                    sparks[s].color = new Color(1f, Mathf.Lerp(0.95f, 0.75f, k), Mathf.Lerp(0.6f, 0.25f, k), 1f - k);
                    any = true;
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