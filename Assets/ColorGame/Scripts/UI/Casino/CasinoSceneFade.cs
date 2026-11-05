using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColorGame.UI.Casino
{
    /// <summary>
    /// A persistent black overlay that fades OUT every time a scene finishes loading
    /// (MainMenu to ColorGame and back). It installs itself, needs no scene setup and never blocks taps.
    /// To remove the effect, delete this file.
    /// </summary>
    public class CasinoSceneFade : MonoBehaviour
    {
        private const float Duration = 0.35f;

        private static CasinoSceneFade instance;

        private Image image;
        private Coroutine routine;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (instance != null) return;
            GameObject go = new GameObject("CasinoSceneFade", typeof(Canvas), typeof(CasinoSceneFade));
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (instance != null && instance != this) { Destroy(gameObject); return; }
            instance = this;

            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = short.MaxValue;

            GameObject black = new GameObject("Black", typeof(RectTransform), typeof(Image));
            black.transform.SetParent(transform, false);
            RectTransform rect = (RectTransform)black.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            image = black.GetComponent<Image>();
            image.color = Color.black;
            image.raycastTarget = false;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void Start()
        {
            FadeIn();
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FadeIn();
        }

        private void FadeIn()
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            image.color = Color.black;
            yield return null;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Duration;
                image.color = new Color(0f, 0f, 0f, 1f - Mathf.Clamp01(t));
                yield return null;
            }

            image.color = new Color(0f, 0f, 0f, 0f);
            routine = null;
        }
    }
}