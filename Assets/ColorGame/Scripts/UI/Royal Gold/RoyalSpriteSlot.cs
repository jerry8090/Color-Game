using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI.RoyalGold
{
    /// <summary>
    /// Marks an Image as an EMPTY PNG SLOT for the Royal Gold design.
    /// - In the editor the empty slot shows as a faint gold rectangle so you can see where the PNG goes.
    /// - When you drag a PNG into Image > Source Image, the tint is reset to white automatically.
    /// - In Play mode an empty slot hides itself, so no ugly white boxes appear before your art is assigned.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class RoyalSpriteSlot : MonoBehaviour
    {
        /// <summary>The faint gold tint of an empty slot.</summary>
        public static readonly Color PlaceholderColor = new Color(1f, 0.84f, 0.3f, 0.18f);

        [Tooltip("Which PNG you should drag into this Image's Source Image field.")]
        [SerializeField] private string assignThisPng = string.Empty;
        [Tooltip("Hide this Image in Play mode while it has no sprite.")]
        [SerializeField] private bool hideWhenEmpty = true;

        public string AssignThisPng => assignThisPng;

        private void Awake()
        {
            Image image = GetComponent<Image>();
            if (image == null) return;

            if (image.sprite == null)
            {
                if (hideWhenEmpty) image.enabled = false;
                return;
            }

            image.enabled = true;
            if (IsPlaceholder(image.color)) image.color = Color.white;
        }

        private void OnValidate()
        {
            Image image = GetComponent<Image>();
            if (image != null && image.sprite != null && IsPlaceholder(image.color)) image.color = Color.white;
        }

        private static bool IsPlaceholder(Color c)
        {
            return Mathf.Abs(c.r - PlaceholderColor.r) < 0.01f &&
                   Mathf.Abs(c.g - PlaceholderColor.g) < 0.01f &&
                   Mathf.Abs(c.b - PlaceholderColor.b) < 0.01f &&
                   Mathf.Abs(c.a - PlaceholderColor.a) < 0.01f;
        }
    }
}
