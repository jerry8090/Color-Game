using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScatterGame.Core;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// The 5x4 grid on screen. It only animates what it is told: spin to a grid, highlight, remove, drop.
    /// Cell order in the array is reel by reel, top to bottom (index = reel * Rows + row).
    /// </summary>
    public class SlotGridView : MonoBehaviour
    {
        [SerializeField] private SlotSymbolView[] cells = new SlotSymbolView[SlotConfig.Reels * SlotConfig.Rows];
        [Tooltip("ON: the card keeps its proportions (no distortion). OFF: the card is stretched to fill the whole cell.")]
        [SerializeField] private bool keepSpriteProportions = true;
        [Header("Timing (seconds at normal speed)")]
        [SerializeField] private float spinTime = 0.7f;
        [SerializeField] private float reelStagger = 0.16f;
        [SerializeField] private float swapInterval = 0.06f;
        [SerializeField] private float dropTime = 0.42f;
        [SerializeField] private float highlightTime = 0.6f;
        [SerializeField] private float removeTime = 0.22f;

        /// <summary>Raised when a reel stops (the controller plays the sound).</summary>
        public event Action<int> OnReelStopped;

        /// <summary>1 = normal. The controller raises it to fast-forward.</summary>
        public float Speed { get; set; } = 1f;

        private ScatterGameSettings settings;
        private readonly List<int> visualIds = new List<int>();
        private int activeTweens;
        private bool isReady;

        private float Dt => Time.deltaTime * Mathf.Max(0.1f, Speed);

        public bool Init(ScatterGameSettings gameSettings)
        {
            settings = gameSettings;
            isReady = false;

            if (settings == null || cells == null || cells.Length != SlotConfig.Reels * SlotConfig.Rows)
            {
                Debug.LogError("[SlotGridView] Settings missing or the grid does not have exactly 20 cells.", this);
                return false;
            }
            foreach (SlotSymbolView cell in cells) cell.ConfigureIcon(keepSpriteProportions);
            foreach (SlotSymbolView cell in cells)
            {
                if (cell == null)
                {
                    Debug.LogError("[SlotGridView] A cell reference is missing.", this);
                    return false;
                }
            }

            visualIds.Clear();
            foreach (SymbolEntry entry in settings.Symbols)
            {
                if (entry == null) continue;
                int total = 0;
                foreach (int weight in entry.reelWeights) total += Mathf.Max(0, weight);
                if (total > 0) visualIds.Add(entry.id);
            }

            if (visualIds.Count == 0)
            {
                Debug.LogError("[SlotGridView] No symbol can appear on the reels.", this);
                return false;
            }

            isReady = true;
            return true;
        }

        private SlotSymbolView Cell(int reel, int row)
        {
            return cells[reel * SlotConfig.Rows + row];
        }

        private float CellHeight => Mathf.Max(1f, cells[0].Cell.rect.height);

        // ---------- Immediate ----------

        public void ShowGrid(SlotGrid grid)
        {
            if (!isReady) return;
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    SlotSymbolView cell = Cell(reel, row);
                    cell.Show(settings, grid.Get(reel, row));
                    cell.SetScale(1f);
                    cell.SetOffsetY(0f);
                    cell.SetHighlight(false);
                }
            }
        }

        public void ClearHighlights()
        {
            foreach (SlotSymbolView cell in cells)
            {
                cell.SetHighlight(false);
                cell.SetScale(1f);
            }
        }

        // ---------- Animations ----------

        /// <summary>All reels spin, then stop one by one on the given grid.</summary>
        public IEnumerator SpinTo(SlotGrid finalGrid)
        {
            if (!isReady) yield break;

            foreach (SlotSymbolView cell in cells)
            {
                cell.SetHighlight(false);
                cell.SetScale(1f);
                cell.SetOffsetY(0f);
            }

            bool[] stopped = new bool[SlotConfig.Reels];
            int stoppedCount = 0;
            float elapsed = 0f;
            float swapTimer = 0f;

            while (stoppedCount < SlotConfig.Reels)
            {
                float dt = Dt;
                elapsed += dt;
                swapTimer += dt;

                if (swapTimer >= swapInterval)
                {
                    swapTimer = 0f;
                    for (int reel = 0; reel < SlotConfig.Reels; reel++)
                    {
                        if (stopped[reel]) continue;
                        for (int row = 0; row < SlotConfig.Rows; row++)
                        {
                            Cell(reel, row).Show(settings, visualIds[UnityEngine.Random.Range(0, visualIds.Count)]);
                        }
                    }
                }

                for (int reel = 0; reel < SlotConfig.Reels; reel++)
                {
                    if (stopped[reel] || elapsed < spinTime + reel * reelStagger) continue;

                    stopped[reel] = true;
                    stoppedCount++;
                    for (int row = 0; row < SlotConfig.Rows; row++) Cell(reel, row).Show(settings, finalGrid.Get(reel, row));
                    StartCoroutine(Counted(DropReel(reel)));
                    OnReelStopped?.Invoke(reel);
                }

                yield return null;
            }

            yield return WaitAll();
        }

        /// <summary>Pulses the winning cells. They stay highlighted until the cascade removes them.</summary>
        public IEnumerator Highlight(IEnumerable<GridPosition> positions)
        {
            if (!isReady) yield break;

            List<SlotSymbolView> list = new List<SlotSymbolView>();
            foreach (GridPosition p in positions)
            {
                SlotSymbolView cell = Cell(p.Reel, p.Row);
                cell.SetHighlight(true);
                list.Add(cell);
            }

            yield return Tween(highlightTime, k =>
            {
                float scale = 1f + 0.12f * Mathf.Sin(k * Mathf.PI);
                foreach (SlotSymbolView cell in list) cell.SetScale(scale);
            });

            foreach (SlotSymbolView cell in list) cell.SetScale(1f);
        }

        /// <summary>Pulses every visible symbol of one category (used for the Scatters).</summary>
        public IEnumerator PulseCategory(SymbolCategory category)
        {
            if (!isReady) yield break;

            List<SlotSymbolView> list = new List<SlotSymbolView>();
            foreach (SlotSymbolView cell in cells)
            {
                SymbolEntry entry = settings.GetEntry(cell.SymbolId);
                if (entry != null && entry.category == category)
                {
                    cell.SetHighlight(true);
                    list.Add(cell);
                }
            }

            yield return Tween(highlightTime * 1.4f, k =>
            {
                float scale = 1f + 0.2f * Mathf.Sin(k * Mathf.PI * 2f);
                foreach (SlotSymbolView cell in list) cell.SetScale(scale);
            });

            foreach (SlotSymbolView cell in list)
            {
                cell.SetScale(1f);
                cell.SetHighlight(false);
            }
        }

        /// <summary>One cascade: winning cells disappear (golden ones turn Wild), then everything falls and refills.</summary>
        public IEnumerator Cascade(CascadeStep step, int wildId)
        {
            if (!isReady) yield break;

            foreach (GridPosition p in step.TransformedToWild) Cell(p.Reel, p.Row).Show(settings, wildId);

            yield return Tween(removeTime, k =>
            {
                foreach (GridPosition p in step.Removed) Cell(p.Reel, p.Row).SetScale(1f - k);
                foreach (GridPosition p in step.TransformedToWild) Cell(p.Reel, p.Row).SetScale(1f + 0.35f * Mathf.Sin(k * Mathf.PI));
            });

            // Work out how far each cell has to fall (in rows). Survivors keep their order and drop to the bottom.
            bool[,] removed = new bool[SlotConfig.Reels, SlotConfig.Rows];
            foreach (GridPosition p in step.Removed) removed[p.Reel, p.Row] = true;

            float[,] offsets = new float[SlotConfig.Reels, SlotConfig.Rows];
            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                List<int> originalRows = new List<int>();
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    if (!removed[reel, row]) originalRows.Add(row);
                }

                int empty = SlotConfig.Rows - originalRows.Count;
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    offsets[reel, row] = row < empty ? empty : row - originalRows[row - empty];
                }
            }

            for (int reel = 0; reel < SlotConfig.Reels; reel++)
            {
                for (int row = 0; row < SlotConfig.Rows; row++)
                {
                    SlotSymbolView cell = Cell(reel, row);
                    cell.Show(settings, step.GridAfter.Get(reel, row));
                    cell.SetScale(1f);
                    cell.SetHighlight(false);
                }
            }

            float height = CellHeight;
            yield return Tween(dropTime, k =>
            {
                float eased = EaseOutBack(k);
                for (int reel = 0; reel < SlotConfig.Reels; reel++)
                {
                    for (int row = 0; row < SlotConfig.Rows; row++)
                    {
                        Cell(reel, row).SetOffsetY(offsets[reel, row] * height * (1f - eased));
                    }
                }
            });

            foreach (SlotSymbolView cell in cells) cell.SetOffsetY(0f);
        }

        // ---------- Helpers ----------

        private IEnumerator DropReel(int reel)
        {
            float start = CellHeight;
            yield return Tween(dropTime * 0.7f, k =>
            {
                float y = start * (1f - EaseOutBack(k));
                for (int row = 0; row < SlotConfig.Rows; row++) Cell(reel, row).SetOffsetY(y);
            });
            for (int row = 0; row < SlotConfig.Rows; row++) Cell(reel, row).SetOffsetY(0f);
        }

        private IEnumerator Tween(float duration, Action<float> apply)
        {
            if (duration <= 0f)
            {
                apply(1f);
                yield break;
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Dt / duration;
                apply(Mathf.Clamp01(t));
                yield return null;
            }
        }

        private IEnumerator Counted(IEnumerator inner)
        {
            activeTweens++;
            yield return inner;
            activeTweens--;
        }

        private IEnumerator WaitAll()
        {
            while (activeTweens > 0) yield return null;
        }

        private static float EaseOutBack(float t)
        {
            const float c1 = 1.2f;
            const float c3 = c1 + 1f;
            float u = t - 1f;
            return 1f + c3 * u * u * u + c1 * u * u;
        }
    }
}