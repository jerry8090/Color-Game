using UnityEngine;
using UnityEngine.UI;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// Makes an existing Outline "breathe" gold. It never enables or disables the Outline:
    /// whoever owns the selection (for example BetButtonView) still decides when it is visible.
    /// </summary>
    [DisallowMultipleComponent]
    public class CasinoGlowPulse : MonoBehaviour
    {
        [SerializeField] private CasinoUIAnimationController controller;
        [Tooltip("The Outline to pulse. Leave empty to use the Outline on this object.")]
        [SerializeField] private Outline outline;
        [SerializeField] private Color glowColor = new Color(1f, 0.84f, 0.29f, 1f);

        private bool ready;

        private void Awake()
        {
            if (controller == null) controller = FindAnyObjectByType<CasinoUIAnimationController>();
            if (outline == null) outline = GetComponent<Outline>();

            if (controller == null || outline == null)
            {
                Debug.LogError("[CasinoGlowPulse] Needs a CasinoUIAnimationController in the scene and an Outline.", this);
                enabled = false;
                return;
            }
            ready = true;
        }

        private void OnEnable()
        {
            if (ready) controller.Register(this);
        }

        private void OnDisable()
        {
            if (controller != null) controller.Unregister(this);
        }

        public void Apply(float alpha)
        {
            if (outline == null || !outline.enabled) return;
            Color c = glowColor;
            c.a = alpha;
            outline.effectColor = c;
        }
    }
}