using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorGame.Gameplay;

namespace ColorGame.DevTools
{
    /// <summary>
    /// DEV TEST ONLY. One button that cycles OFF > WIN x1 > WIN x2 > WIN x3 > LOSE.
    /// Works with the biggest bet color of the next wagered round. Removed itself in release builds.
    /// </summary>
    public class ForceResultUI : MonoBehaviour
    {
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        private void Awake()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (gameManager == null || button == null || label == null)
            {
                Debug.LogError("[ForceResultUI] A reference is missing in the Inspector.", this);
                return;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Cycle);
            Refresh();
#else
            gameObject.SetActive(false);
#endif
        }

        private void OnEnable()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (gameManager != null && label != null) Refresh();
#endif
        }

        private void Cycle()
        {
            int count = System.Enum.GetValues(typeof(ForceResultMode)).Length;
            gameManager.ForcedMode = (ForceResultMode)(((int)gameManager.ForcedMode + 1) % count);
            Refresh();
        }

        private void Refresh()
        {
            switch (gameManager.ForcedMode)
            {
                case ForceResultMode.WinOneMatch: label.text = "FORCE: WIN x1"; break;
                case ForceResultMode.WinTwoMatches: label.text = "FORCE: WIN x2"; break;
                case ForceResultMode.WinThreeMatches: label.text = "FORCE: WIN x3"; break;
                case ForceResultMode.Lose: label.text = "FORCE: LOSE"; break;
                default: label.text = "FORCE: OFF"; break;
            }
        }
    }
}