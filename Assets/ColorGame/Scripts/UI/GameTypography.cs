using TMPro;
using UnityEngine;

namespace Shared.UI
{
    public enum TextRole { Title, Number, Button, Label, Body, Bill }

    /// <summary>One font asset plus one shared material preset per text role (outline and shadow differ per role).</summary>
    [CreateAssetMenu(fileName = "GameTypography", menuName = "Color Game/Game Typography")]
    public class GameTypography : ScriptableObject
    {
        [SerializeField] private TMP_FontAsset font;
        [SerializeField] private Material titleMaterial;
        [SerializeField] private Material numberMaterial;
        [SerializeField] private Material buttonMaterial;
        [SerializeField] private Material labelMaterial;
        [SerializeField] private Material bodyMaterial;
        [SerializeField] private Material billMaterial;

        public TMP_FontAsset Font => font;

        public Material GetMaterial(TextRole role)
        {
            switch (role)
            {
                case TextRole.Title: return titleMaterial;
                case TextRole.Number: return numberMaterial;
                case TextRole.Button: return buttonMaterial;
                case TextRole.Label: return labelMaterial;
                case TextRole.Bill: return billMaterial;
                default: return bodyMaterial;
            }
        }

        public void Apply(TMP_Text text, TextRole role)
        {
            if (text == null || font == null) return;
            text.font = font;
            Material material = GetMaterial(role);
            if (material != null) text.fontSharedMaterial = material;
        }
    }
}