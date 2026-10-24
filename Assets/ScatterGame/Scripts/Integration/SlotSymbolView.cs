using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// One cell of the 5x4 grid. Display only: it shows the symbol it is told to show.
    /// With a sprite: the Icon fills the whole Content area, there is no background behind the (transparent) artwork,
    /// and the golden / highlight outlines follow the artwork's silhouette.
    /// Without a sprite: the old coloured tile with a label is shown.
    /// </summary>
    public class SlotSymbolView : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Outline goldBorder;
        [SerializeField] private Outline highlight;

        private Outline iconGold;
        private Outline iconHighlight;
        private Color tint = Color.gray;
        private bool configured;
        private bool usingSprite;
        private bool golden;
        private bool highlighted;

        public int SymbolId { get; private set; } = -1;
        public RectTransform Cell => (RectTransform)transform;

        /// <summary>
        /// Makes the Icon fill its parent completely (no padding). Safe to call many times.
        /// SlotGridView calls it for every cell at start, so it also fixes scenes built earlier.
        /// </summary>
        public void ConfigureIcon(bool preserveAspect)
        {
            RectTransform rect = icon.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);

            icon.type = Image.Type.Simple;
            icon.preserveAspect = preserveAspect;
            icon.raycastTarget = false;   // never blocks taps
            icon.color = Color.white;     // do not tint the artwork

            if (iconGold == null) iconGold = AddIconOutline(new Color(1f, 0.84f, 0.25f, 1f), new Vector2(3f, 3f));
            if (iconHighlight == null) iconHighlight = AddIconOutline(new Color(1f, 0.97f, 0.6f, 1f), new Vector2(6f, 6f));

            configured = true;
            Apply();
        }

        public void Show(ScatterGameSettings settings, int symbolId)
        {
            if (!configured) ConfigureIcon(true);

            SymbolId = symbolId;
            SymbolEntry entry = settings != null ? settings.GetEntry(symbolId) : null;
            Sprite sprite = entry != null ? entry.sprite : null;

            usingSprite = sprite != null;
            golden = ScatterSymbolLook.IsGolden(settings, symbolId);

            if (usingSprite)
            {
                icon.sprite = sprite;
                icon.enabled = true;
                label.enabled = false;
            }
            else
            {
                icon.enabled = false;
                label.enabled = true;
                label.text = ScatterSymbolLook.Label(settings, symbolId);
                label.color = ScatterSymbolLook.TextColor(settings, symbolId);
                tint = ScatterSymbolLook.Tint(settings, symbolId);
            }

            Apply();
        }

        public void SetHighlight(bool on)
        {
            highlighted = on;
            Apply();
        }

        public void SetScale(float scale)
        {
            content.localScale = new Vector3(scale, scale, 1f);
        }

        public void SetOffsetY(float y)
        {
            content.anchoredPosition = new Vector2(0f, y);
        }

        // Sprite cells: transparent background, outlines on the Icon. Tile cells: coloured background, outlines on it.
        private void Apply()
        {
            background.color = usingSprite ? new Color(1f, 1f, 1f, 0f) : tint;

            goldBorder.enabled = golden && !usingSprite;
            highlight.enabled = highlighted && !usingSprite;

            if (iconGold != null) iconGold.enabled = golden && usingSprite;
            if (iconHighlight != null) iconHighlight.enabled = highlighted && usingSprite;
        }

        private Outline AddIconOutline(Color color, Vector2 distance)
        {
            Outline outline = icon.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = distance;
            outline.enabled = false;
            return outline;
        }
    }
}