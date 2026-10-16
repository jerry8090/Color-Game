using System;
using TMPro;
using UnityEngine;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// The betting-window timer and the automatic round cycle. ENABLED BY DEFAULT (the ROLL button was removed).
    ///
    /// ON:  every round has a betting window.
    ///      1. When the machine is ready (Idle, cubes on the shelf), the window opens: the manager clears the old
    ///         color and bet, and a countdown starts.
    ///      2. At the deadline the manager starts the machine round. With a valid color AND bet it is a wagered round,
    ///         otherwise a round without a wager. The machine runs fully either way.
    ///      3. When the round and the cube return are finished, the next window opens. Repeats forever.
    /// OFF: nothing happens here. The manual ROLL button works as before.
    ///
    /// This script only keeps time. It never touches the balance, the cubes or the result.
    /// It polls once per frame instead of subscribing to events, so there is nothing to subscribe twice.
    /// Put this on its own empty object (NOT on the ROLL button, which it may hide).
    /// </summary>
    public class AutoRollController : MonoBehaviour
    {
        private enum Phase
        {
            Off,
            WaitingForRound,
            BettingOpen
        }

        [Header("References")]
        [SerializeField] private ColorGameManager gameManager;

        [Header("Betting cycle")]
        [Tooltip("ON = a betting window opens before every round (the game has no ROLL button any more). Keep it ticked.")]
        [SerializeField] private bool autoRollEnabled = true;
        [Tooltip("Seconds the player has to pick a color AND a bet.")]
        [SerializeField] private float bettingWindowSeconds = 15f;
        [Tooltip("Countdown ticks play once per second for this many final seconds. The last second plays the final warning.")]
        [SerializeField] private int countdownSoundSeconds = 5;

        [Header("Optional")]
        [Tooltip("Hidden while the cycle is ON, shown again when it is OFF. Drag the RollButton object here.")]
        [SerializeField] private GameObject manualRollButton;
        [Tooltip("Shows 'PLACE YOUR BETS  12'. Leave empty for no countdown text.")]
        [SerializeField] private TMP_Text countdownText;

        /// <summary>Raised once per second in the final seconds. Carries the whole seconds left (1 = last second).</summary>
        public event Action<int> OnCountdownTick;

        private Phase phase = Phase.Off;
        private float remaining;
        private int lastSecond = int.MaxValue;
        private bool buttonHiddenByUs;
        private bool isReady;

        public bool AutoRollEnabled => autoRollEnabled;
        public bool IsBettingOpen => phase == Phase.BettingOpen;
        public float RemainingSeconds => remaining;

        private void OnValidate()
        {
            bettingWindowSeconds = Mathf.Max(3f, bettingWindowSeconds);
            countdownSoundSeconds = Mathf.Clamp(countdownSoundSeconds, 1, 30);
        }

        private void Awake()
        {
            if (gameManager == null)
            {
                Debug.LogError("[AutoRollController] Game Manager is not assigned in the Inspector.", this);
                return;
            }

            isReady = true;
        }

        private void OnDisable()
        {
            phase = Phase.Off;
            remaining = 0f;
            SetText(string.Empty);

            if (buttonHiddenByUs && manualRollButton != null) manualRollButton.SetActive(true);
            buttonHiddenByUs = false;
        }

        /// <summary>Turns the betting cycle on or off at run time.</summary>
        public void SetAutoRollEnabled(bool value)
        {
            autoRollEnabled = value;
        }

        private void Update()
        {
            if (!isReady) return;

            ApplyManualButton();

            if (!autoRollEnabled)
            {
                if (phase != Phase.Off)
                {
                    phase = Phase.Off;
                    SetText(string.Empty);
                }
                return;
            }

            if (phase == Phase.Off) phase = Phase.WaitingForRound;

            if (phase == Phase.WaitingForRound)
            {
                TryOpenWindow();
            }
            else if (phase == Phase.BettingOpen)
            {
                RunCountdown();
            }
        }

        // The window opens only when the machine is completely ready: no round, cubes back on the shelf.
        private void TryOpenWindow()
        {
            SetText(string.Empty);

            if (gameManager.IsRoundRunning || gameManager.IsMachineBusy || gameManager.State != ColorGame.Core.GameState.Idle) return;
            if (!gameManager.BeginBettingWindow()) return;

            remaining = bettingWindowSeconds;
            lastSecond = int.MaxValue;
            phase = Phase.BettingOpen;
        }

        private void RunCountdown()
        {
            remaining -= Time.deltaTime;

            if (remaining <= 0f)
            {
                CloseWindow();
                return;
            }

            int secondsLeft = Mathf.CeilToInt(remaining);

            if (secondsLeft != lastSecond)
            {
                lastSecond = secondsLeft;
                if (secondsLeft >= 1 && secondsLeft <= countdownSoundSeconds) OnCountdownTick?.Invoke(secondsLeft);
            }

            SetText("PLACE YOUR BETS  " + secondsLeft);
        }

        // The deadline. The manager decides wager or no bet and starts the machine either way.
        private void CloseWindow()
        {
            remaining = 0f;
            phase = Phase.WaitingForRound;
            SetText(string.Empty);

            gameManager.CloseBettingAndStartRound();

            if (!gameManager.IsRoundRunning)
            {
                Debug.LogWarning("[AutoRollController] The round did not start at the deadline. A new window will open when the machine is ready.", this);
            }
        }

        private void ApplyManualButton()
        {
            if (manualRollButton == null) return;

            if (autoRollEnabled && !buttonHiddenByUs)
            {
                manualRollButton.SetActive(false);
                buttonHiddenByUs = true;
            }
            else if (!autoRollEnabled && buttonHiddenByUs)
            {
                manualRollButton.SetActive(true);
                buttonHiddenByUs = false;
            }
        }

        private void SetText(string value)
        {
            if (countdownText != null && countdownText.text != value) countdownText.text = value;
        }
    }
}
