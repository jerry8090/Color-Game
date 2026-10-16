using System;
using UnityEngine;
using ColorGame.Core;

namespace ColorGame.Machine
{
    /// <summary>
    /// One physical die. It carries its logical color (AssignedColor), so the result
    /// is never guessed from a sprite color. Visual and physics helpers only.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class ColorDie : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private SpriteRenderer bodyRenderer;
        [SerializeField] private SpriteRenderer glowRenderer;

        [Header("Look")]
        [SerializeField] private float highlightedScale = 1.12f;
        [SerializeField, Range(0f, 1f)] private float dimmedAlpha = 0.5f;

        /// <summary>Raised on every collision with the impact speed (for optional impact sounds).</summary>
        public event Action<ColorDie, float> OnImpact;

        private Color tint = Color.white;
        private Vector3 baseScale = Vector3.one;
        private bool isSetup;

        public GameColor AssignedColor { get; private set; }
        public int SlotIndex { get; private set; }
        public Rigidbody2D Body => body;

        public void Setup(ColorDefinition definition, int slotIndex)
        {
            if (definition == null || body == null || bodyRenderer == null)
            {
                Debug.LogError("[ColorDie] Missing definition or references on the ColorDie prefab.", this);
                return;
            }

            AssignedColor = definition.color;
            SlotIndex = slotIndex;

            tint = definition.uiColor;
            tint.a = 1f;
            bodyRenderer.color = tint;

            baseScale = transform.localScale;
            if (glowRenderer != null) glowRenderer.enabled = false;

            isSetup = true;
        }

        /// <summary>Turns physics on and gives the die its first push.</summary>
        public void BeginSimulation(Vector2 velocity, float spinDegreesPerSecond)
        {
            body.simulated = true;
            body.linearVelocity = velocity;
            body.angularVelocity = spinDegreesPerSecond;
        }

        /// <summary>Stops the die completely (it no longer takes part in physics).</summary>
        public void Freeze()
        {
            if (body == null) return;
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
            body.simulated = false;
        }

        public void SetHighlighted(bool highlighted)
        {
            if (!isSetup) return;
            transform.localScale = baseScale * (highlighted ? highlightedScale : 1f);
            if (glowRenderer != null) glowRenderer.enabled = highlighted;
        }

        public void SetDimmed(bool dimmed)
        {
            if (!isSetup) return;
            Color c = tint;
            c.a = dimmed ? dimmedAlpha : 1f;
            bodyRenderer.color = c;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnImpact?.Invoke(this, collision.relativeVelocity.magnitude);
        }
    }
}