using System.Collections;
using UnityEngine;

namespace ScatterGame.Integration
{
    /// <summary>
    /// Smooth open/close for the EXISTING Scatter History panel. It listens to ScatterHistoryUI.OnPanelToggled,
    /// so History's data, buttons and logic are untouched. Place it on any object in the MainMenu scene.
    /// </summary>
    public class ScatterPanelFx : MonoBehaviour
    {
        [SerializeField] private ScatterHistoryUI historyUI;
        [Tooltip("The CanvasGroup of ScatterHistoryPanel (the same one assigned to ScatterHistoryUI > Panel Group).")]
        [SerializeField] private CanvasGroup panelGroup;
        [SerializeField] private float openSeconds = 0.28f;
        [SerializeField] private float closeSeconds = 0.18f;
        [SerializeField, Range(0.8f, 1f)] private float startScale = 0.95f;
        [SerializeField] private float slideY = -40f;

        private RectTransform rect;
        private Vector3 baseScale;
        private Vector2 basePos;
        private Coroutine routine;

        private void Awake()
        {
            if (panelGroup == null || historyUI == null)
            {
                Debug.LogError("[ScatterPanelFx] Assign History UI and Panel Group.", this);
                enabled = false;
                return;
            }
            rect = (RectTransform)panelGroup.transform;
            baseScale = rect.localScale;
            basePos = rect.anchoredPosition;
        }

        private void OnEnable() { if (historyUI != null) historyUI.OnPanelToggled += Handle; }

        private void OnDisable()
        {
            if (historyUI != null) historyUI.OnPanelToggled -= Handle;
            if (rect != null) { rect.localScale = baseScale; rect.anchoredPosition = basePos; }
        }

        private void Handle(bool open)
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Animate(open));
        }

        private IEnumerator Animate(bool open)
        {
            float seconds = open ? openSeconds : closeSeconds;
            float t = 0f;
            panelGroup.alpha = open ? 0f : 1f; // on close the panel is already hidden by ScatterHistoryUI: show it again while fading

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.05f, seconds);
                float k = ScatterFx.EaseOutCubic(t);
                float shown = open ? k : 1f - k;
                panelGroup.alpha = shown;
                float s = Mathf.Lerp(startScale, 1f, shown);
                rect.localScale = baseScale * s;
                rect.anchoredPosition = basePos + new Vector2(0f, Mathf.Lerp(slideY, 0f, shown));
                yield return null;
            }

            rect.localScale = baseScale;
            rect.anchoredPosition = basePos;
            panelGroup.alpha = open ? 1f : 0f;
            routine = null;
        }
    }
}