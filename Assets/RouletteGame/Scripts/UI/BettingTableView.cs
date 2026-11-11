using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RouletteGame.Core;

namespace RouletteGame.UI
{
    /// <summary>
    /// The betting table, drawn for a portrait screen: a zero on top, 12 rows of 3 numbers, dozens on the left,
    /// columns and even-money bets below. It builds every betting area and every small tap spot (split, street,
    /// corner, six line) from the Roulette rules when the scene starts, so positions can never disagree with the bets.
    /// DISPLAY AND INPUT ONLY: a tap raises OnAreaTapped(bet). The controller decides what happens.
    /// Chips lie on top as small markers that show the total on each area.
    /// </summary>
    public class BettingTableView : MonoBehaviour
    {
        private const int Rows = 15;               // zero, 12 number rows, columns row, even-money row
        private const float StripWidth = 0.16f;    // the dozens
        private const float ColumnWidth = 0.28f;   // each of the 3 number columns

        [Header("References")]
        [Tooltip("Cells and tap spots are created inside this rectangle.")]
        [SerializeField] private RectTransform tableArea;
        [Tooltip("Same size as the table area, drawn above it. The chip markers live here.")]
        [SerializeField] private RectTransform chipLayer;
        [SerializeField] private Sprite cellSprite;
        [SerializeField] private TMP_FontAsset font;

        [Header("Look")]
        [SerializeField] private Color outsideColor = new Color32(58, 36, 92, 255);
        [SerializeField] private Color highlightColor = new Color32(255, 213, 74, 255);
        [SerializeField] private Color markerColor = new Color32(255, 213, 74, 255);
        [SerializeField] private Color markerWonColor = new Color32(80, 200, 110, 255);
        [SerializeField] private Color markerLostColor = new Color32(130, 110, 140, 255);
        [Tooltip("Shows the small tap spots faintly, to help while designing.")]
        [SerializeField] private bool showTapSpots = false;

        /// <summary>A betting area was tapped. The bet carries amount 1 and only says WHERE.</summary>
        public event Action<RouletteBet> OnAreaTapped;

        private sealed class Area
        {
            public Vector2 center;
            public Image image;
            public TMP_Text label;
            public Color baseColor;
            public Color baseTextColor;
        }

        private sealed class Marker
        {
            public RectTransform root;
            public Image image;
            public TMP_Text text;
        }

        private readonly Dictionary<string, Area> areas = new Dictionary<string, Area>();
        private readonly Dictionary<string, Marker> markers = new Dictionary<string, Marker>();
        private CanvasGroup group;
        private Area highlighted;
        private bool isReady;

        private void Awake()
        {
            if (tableArea == null || chipLayer == null)
            {
                Debug.LogError("[BettingTableView] Table Area or Chip Layer is not assigned in the Inspector.", this);
                return;
            }

            group = tableArea.GetComponent<CanvasGroup>();
            if (group == null) group = tableArea.gameObject.AddComponent<CanvasGroup>();

            BuildNumbers();
            BuildOutside();
            BuildTapSpots();
            isReady = true;
        }

        public void SetInteractable(bool interactable)
        {
            if (group != null) group.blocksRaycasts = interactable;
        }

        /// <summary>Shows the chips of the current bets.</summary>
        public void Refresh(IReadOnlyList<RouletteBet> bets)
        {
            if (!isReady) return;

            foreach (Marker marker in markers.Values) marker.root.gameObject.SetActive(false);

            foreach (RouletteBet bet in bets)
            {
                if (!areas.TryGetValue(bet.Key, out Area area)) continue;
                Marker marker = GetMarker(bet.Key, area);
                marker.root.gameObject.SetActive(true);
                marker.text.text = Abbreviate(bet.Amount);
                marker.image.color = markerColor;
            }
        }

        /// <summary>After the spin: winning chips turn green, losing chips grey.</summary>
        public void ShowOutcome(RoundPayout payout)
        {
            if (!isReady || payout == null) return;

            foreach (BetOutcome outcome in payout.Outcomes)
            {
                if (markers.TryGetValue(outcome.Bet.Key, out Marker marker) && marker.root.gameObject.activeSelf)
                {
                    marker.image.color = outcome.Won ? markerWonColor : markerLostColor;
                }
            }
        }

        public void HighlightNumber(int number)
        {
            ClearHighlight();
            if (!isReady || !areas.TryGetValue(RouletteBet.Straight(number, 1).Key, out Area area)) return;

            highlighted = area;
            area.image.color = highlightColor;
            area.label.color = RouletteUi.Dark;
        }

        public void ClearHighlight()
        {
            if (highlighted == null) return;
            highlighted.image.color = highlighted.baseColor;
            highlighted.label.color = highlighted.baseTextColor;
            highlighted = null;
        }

        // ---------- Building ----------

        private static float RowTop(int row) { return 1f - row / (float)Rows; }
        private static float RowBottom(int row) { return 1f - (row + 1) / (float)Rows; }
        private static float RowMiddle(int row) { return 1f - (row + 0.5f) / (float)Rows; }
        private static float ColumnLeft(int column) { return StripWidth + column * ColumnWidth; }
        private static float ColumnMiddle(int column) { return StripWidth + (column + 0.5f) * ColumnWidth; }

        private void BuildNumbers()
        {
            // The zero covers the three number columns.
            AddCell("Zero", StripWidth, RowBottom(0), 1f, RowTop(0), RouletteUi.ColorFor(PocketColor.Green), "0", 34f, RouletteBet.Straight(0, 1));

            for (int row = 1; row <= 12; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    int number = (row - 1) * 3 + column + 1;
                    Color color = RouletteUi.ColorFor(RouletteLayout.ColorOf(number));
                    AddCell("Number_" + number, ColumnLeft(column), RowBottom(row), ColumnLeft(column + 1), RowTop(row),
                            color, number.ToString(), 32f, RouletteBet.Straight(number, 1));
                }
            }
        }

        private void BuildOutside()
        {
            // Dozens: three tall cells on the left, four rows each.
            string[] dozenNames = { "1ST\n12", "2ND\n12", "3RD\n12" };
            for (int d = 0; d < 3; d++)
            {
                int firstRow = 1 + d * 4;
                AddCell("Dozen_" + (d + 1), 0f, RowBottom(firstRow + 3), StripWidth, RowTop(firstRow), outsideColor, dozenNames[d], 30f, RouletteBet.Dozen(d + 1, 1));
            }

            // Columns: "2 to 1" under each number column.
            for (int column = 0; column < 3; column++)
            {
                AddCell("Column_" + (column + 1), ColumnLeft(column), RowBottom(13), ColumnLeft(column + 1), RowTop(13),
                        outsideColor, "2:1", 28f, RouletteBet.Column(column + 1, 1));
            }

            // Even-money bets across the whole width.
            float w = 1f / 6f;
            AddCell("Low", 0f * w, RowBottom(14), 1f * w, RowTop(14), outsideColor, "1-18", 26f, RouletteBet.Low(1));
            AddCell("Even", 1f * w, RowBottom(14), 2f * w, RowTop(14), outsideColor, "EVEN", 26f, RouletteBet.Even(1));
            AddCell("Red", 2f * w, RowBottom(14), 3f * w, RowTop(14), RouletteUi.ColorFor(PocketColor.Red), "RED", 26f, RouletteBet.Red(1));
            AddCell("Black", 3f * w, RowBottom(14), 4f * w, RowTop(14), RouletteUi.ColorFor(PocketColor.Black), "BLACK", 26f, RouletteBet.Black(1));
            AddCell("Odd", 4f * w, RowBottom(14), 5f * w, RowTop(14), outsideColor, "ODD", 26f, RouletteBet.Odd(1));
            AddCell("High", 5f * w, RowBottom(14), 6f * w, RowTop(14), outsideColor, "19-36", 26f, RouletteBet.High(1));
        }

        // Small tap spots on the lines between numbers. Every bet is created through RouletteBet, so a shape the
        // rules do not allow can never get a tap spot. Corners are added last so they sit on top.
        private void BuildTapSpots()
        {
            // Splits with the zero.
            for (int column = 0; column < 3; column++)
            {
                AddSpot("Split_0_" + (column + 1), new Vector2(ColumnMiddle(column), RowBottom(0)), new Vector2(90f, 26f), RouletteBet.Split(0, column + 1, 1));
            }

            for (int row = 1; row <= 12; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    int n = (row - 1) * 3 + column + 1;

                    if (column < 2) // side by side
                    {
                        AddSpot("Split_" + n + "_" + (n + 1), new Vector2(ColumnLeft(column + 1), RowMiddle(row)), new Vector2(38f, 34f), RouletteBet.Split(n, n + 1, 1));
                    }
                    if (row < 12) // one above the other
                    {
                        AddSpot("Split_" + n + "_" + (n + 3), new Vector2(ColumnMiddle(column), RowBottom(row)), new Vector2(90f, 26f), RouletteBet.Split(n, n + 3, 1));
                    }
                }
            }

            // Streets (a row of three) and six lines (two rows) on the left edge of the numbers.
            for (int row = 1; row <= 12; row++)
            {
                int first = (row - 1) * 3 + 1;
                AddSpot("Street_" + first, new Vector2(StripWidth, RowMiddle(row)), new Vector2(34f, 34f), RouletteBet.Street(first, 1));
                if (row < 12)
                {
                    AddSpot("SixLine_" + first, new Vector2(StripWidth, RowBottom(row)), new Vector2(34f, 28f), RouletteBet.SixLine(first, 1));
                }
            }

            for (int row = 1; row <= 11; row++)
            {
                for (int column = 0; column < 2; column++)
                {
                    int lowest = (row - 1) * 3 + column + 1;
                    AddSpot("Corner_" + lowest, new Vector2(ColumnLeft(column + 1), RowBottom(row)), new Vector2(40f, 34f), RouletteBet.Corner(lowest, 1));
                }
            }
        }

        private void AddCell(string name, float x0, float y0, float x1, float y1, Color color, string text, float size, RouletteBet bet)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(tableArea, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = new Vector2(x0, y0);
            rect.anchorMax = new Vector2(x1, y1);
            rect.offsetMin = new Vector2(2f, 2f);
            rect.offsetMax = new Vector2(-2f, -2f);

            Image image = go.GetComponent<Image>();
            image.sprite = cellSprite;
            image.type = cellSprite != null ? Image.Type.Sliced : Image.Type.Simple;
            image.pixelsPerUnitMultiplier = 3f; // small corners on small cells
            image.color = color;

            ConfigureButton(go.GetComponent<Button>(), image, bet);

            TextMeshProUGUI label = RouletteUi.MakeText(go.transform, "Label", text, size, Color.white, font);
            Area area = new Area
            {
                center = new Vector2((x0 + x1) * 0.5f, (y0 + y1) * 0.5f),
                image = image,
                label = label,
                baseColor = color,
                baseTextColor = Color.white
            };
            areas[bet.Key] = area;
        }

        private void AddSpot(string name, Vector2 center, Vector2 sizeInPixels, RouletteBet bet)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(tableArea, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = center;
            rect.anchorMax = center;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = sizeInPixels;
            rect.anchoredPosition = Vector2.zero;

            // A fully transparent image still receives taps.
            Image image = go.GetComponent<Image>();
            image.color = showTapSpots ? new Color(1f, 1f, 1f, 0.18f) : new Color(1f, 1f, 1f, 0f);

            ConfigureButton(go.GetComponent<Button>(), image, bet);
            areas[bet.Key] = new Area { center = center, image = image, label = null, baseColor = image.color, baseTextColor = Color.white };
        }

        private void ConfigureButton(Button button, Image target, RouletteBet bet)
        {
            button.targetGraphic = target;
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => OnAreaTapped?.Invoke(bet));
        }

        private Marker GetMarker(string key, Area area)
        {
            if (markers.TryGetValue(key, out Marker existing)) return existing;

            GameObject go = new GameObject("Chip_" + key, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(chipLayer, false);

            RectTransform rect = (RectTransform)go.transform;
            rect.anchorMin = area.center;
            rect.anchorMax = area.center;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(58f, 58f);
            rect.anchoredPosition = Vector2.zero;

            Image image = go.GetComponent<Image>();
            image.sprite = cellSprite;
            image.type = cellSprite != null ? Image.Type.Sliced : Image.Type.Simple;
            image.color = markerColor;
            image.raycastTarget = false; // chips never block taps on the table

            Outline outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.7f);
            outline.effectDistance = new Vector2(2f, -2f);

            TextMeshProUGUI text = RouletteUi.MakeText(go.transform, "Amount", "0", 26f, RouletteUi.Dark, font);
            Marker marker = new Marker { root = rect, image = image, text = text };
            markers[key] = marker;
            return marker;
        }

        private static string Abbreviate(long value)
        {
            if (value < 1000) return value.ToString(CultureInfo.InvariantCulture);
            if (value < 1000000) return Trim(value / 1000.0) + "K";
            return Trim(value / 1000000.0) + "M";
        }

        private static string Trim(double value)
        {
            return value % 1.0 == 0.0 ? ((long)value).ToString(CultureInfo.InvariantCulture) : value.ToString("0.#", CultureInfo.InvariantCulture);
        }
    }
}
