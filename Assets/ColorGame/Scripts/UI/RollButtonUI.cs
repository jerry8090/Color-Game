using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shared.Wallet;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// The ROLL button. It is enabled only when ColorGameManager says a round can start,
    /// and a click only asks the manager to start. The manager does all the checking.
    /// While enabled, the button pulses gently to invite a tap.
    /// </summary>
    public class RollButtonUI : MonoBehaviour
    {
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        [Header("Ready Pulse")]
        [SerializeField] private float pulseAmount = 0.04f;
        [SerializeField] private float pulseSpeed = 3.5f;

        private bool isReady;

        private void Awake()
        {
            if (gameManager == null || betManager == null || walletManager == null ||
                button == null || label == null)
            {
                Debug.LogError("[RollButtonUI] A reference is missing in the Inspector.", this);
                return;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClicked);
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnStateChanged += HandleStateChanged;
            gameManager.OnSelectedColorChanged += HandleColorChanged;
            betManager.OnBetChanged += HandleBetChanged;
            walletManager.OnBalanceChanged += HandleBalanceChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnStateChanged -= HandleStateChanged;
                gameManager.OnSelectedColorChanged -= HandleColorChanged;
            }
            if (betManager != null) betManager.OnBetChanged -= HandleBetChanged;
            if (walletManager != null) walletManager.OnBalanceChanged -= HandleBalanceChanged;

            transform.localScale = Vector3.one;
        }

        private void Update()
        {
            if (!isReady) return;

            float scale = button.interactable
                ? 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount
                : 1f;
            transform.localScale = Vector3.one * scale;
        }

        private void HandleClicked()
        {
            gameManager.TryStartRound();
        }

        private void HandleStateChanged(GameState state) { Refresh(); }
        private void HandleColorChanged(bool hasColor, GameColor color) { Refresh(); }
        private void HandleBetChanged(int bet) { Refresh(); }
        private void HandleBalanceChanged(long balance) { Refresh(); }

        private void Refresh()
        {
            button.interactable = gameManager.CanStartRound;
            label.text = gameManager.State == GameState.Idle ? "ROLL" : "ROLLING...";
        }
    }
}