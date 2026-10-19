using UnityEngine;
using Shared.Audio;
using ColorGame.Core;
using ColorGame.DevTools;
using ColorGame.Gameplay;
using ColorGame.Machine;
using ColorGame.UI;

namespace ColorGame.Sound
{
    /// <summary>
    /// The ONE place that connects Color Game events to sounds.
    /// UI scripts and the machine contain no audio code. They raise events, and this script listens.
    /// Every subscription is made in OnEnable and removed in OnDisable, so a new round never adds a second handler.
    /// </summary>
    public class ColorGameAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private BetManager betManager;
        [SerializeField] private ColorResultUI colorResultUI;

        [Header("Optional")]
        [Tooltip("Plays a click when the history panel opens or closes.")]
        [SerializeField] private GameHistoryUI historyUI;
        [Tooltip("Plays a click when the debug panel opens or closes.")]
        [SerializeField] private DebugPanelUI debugPanel;
        [Tooltip("The 3D machine. Cube impacts, settling and the rack sounds come from here.")]
        [SerializeField] private Machine3DPresenter machine;
        [Tooltip("The betting timer. Countdown ticks come from here.")]
        [SerializeField] private AutoRollController autoRoll;

        private bool isReady;

        private void Awake()
        {
            if (audioManager == null || gameManager == null || betManager == null || colorResultUI == null)
            {
                Debug.LogError("[ColorGameAudio] A reference is missing in the Inspector.", this);
                return;
            }
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnSelectedColorChanged += HandleColorSelected;
            gameManager.OnRollStarted += HandleRollStarted;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            gameManager.OnBettingWindowOpened += HandleBettingWindowOpened;
            gameManager.OnBettingWindowClosed += HandleBettingWindowClosed;
            gameManager.OnStateChanged += HandleStateChanged;
            betManager.OnBetChanged += HandleBetChanged;
            colorResultUI.OnSlotStopped += HandleSlotStopped;

            if (historyUI != null) historyUI.OnPanelToggled += HandlePanelToggled;
            if (debugPanel != null) debugPanel.OnPanelToggled += HandlePanelToggled;
            if (autoRoll != null) autoRoll.OnCountdownTick += HandleCountdownTick;
            if (machine != null)
            {
                machine.OnImpact += HandleImpact;
                machine.OnRollSettled += HandleRollSettled;
                machine.OnRackMoved += HandleRackMoved;
            }
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnSelectedColorChanged -= HandleColorSelected;
                gameManager.OnRollStarted -= HandleRollStarted;
                gameManager.OnRoundCompleted -= HandleRoundCompleted;
                gameManager.OnBettingWindowOpened -= HandleBettingWindowOpened;
                gameManager.OnBettingWindowClosed -= HandleBettingWindowClosed;
                gameManager.OnStateChanged -= HandleStateChanged;
            }
            if (betManager != null) betManager.OnBetChanged -= HandleBetChanged;
            if (colorResultUI != null) colorResultUI.OnSlotStopped -= HandleSlotStopped;
            if (historyUI != null) historyUI.OnPanelToggled -= HandlePanelToggled;
            if (debugPanel != null) debugPanel.OnPanelToggled -= HandlePanelToggled;
            if (autoRoll != null) autoRoll.OnCountdownTick -= HandleCountdownTick;
            if (machine != null)
            {
                machine.OnImpact -= HandleImpact;
                machine.OnRollSettled -= HandleRollSettled;
                machine.OnRackMoved -= HandleRackMoved;
            }

            if (audioManager != null) audioManager.StopCountdownSounds();
        }

        // ---------- Selection and buttons ----------

        private void HandleColorSelected(bool hasColor, GameColor color)
        {
            if (hasColor) audioManager.PlaySfx(SfxType.Select);
        }

        private void HandleBetChanged(int bet)
        {
            // bet = 0 means the selection was cleared, which is not a tap.
            if (bet > 0) audioManager.PlaySfx(SfxType.ButtonClick);
        }

        private void HandlePanelToggled(bool isOpen)
        {
            audioManager.PlaySfx(SfxType.ButtonClick);
        }

        // ---------- Betting countdown ----------

        private void HandleBettingWindowOpened()
        {
            audioManager.PlaySfx(SfxType.CountdownOpen);
        }

        private void HandleCountdownTick(int secondsLeft)
        {
            audioManager.PlaySfx(secondsLeft <= 1 ? SfxType.CountdownFinal : SfxType.CountdownTick);
        }

        // The deadline: silence the countdown at once, whether or not a bet was placed.
        private void HandleBettingWindowClosed(bool wagerPlaced)
        {
            audioManager.StopCountdownSounds();
        }

        // Safety: no countdown sound can survive into the falling, settling or reset phases.
        private void HandleStateChanged(GameState state)
        {
            if (state != GameState.Idle) audioManager.StopCountdownSounds();
        }

        // ---------- Round ----------

        private void HandleRollStarted(GameColor[] results)
        {
            audioManager.PlaySfx(SfxType.Roll);
        }

        private void HandleSlotStopped(int slotIndex)
        {
            audioManager.PlaySfx(SfxType.SlotStop);
        }

        // Only raised for rounds with a wager. A round without a bet plays no win or lose sound.
        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            audioManager.PlaySfx(payout.IsWin ? SfxType.Win : SfxType.Lose);
        }

        // ---------- Machine and physics ----------

        private void HandleImpact(ImpactKind kind, float strength)
        {
            SfxType type = kind == ImpactKind.Cube ? SfxType.CubeHitCube
                         : kind == ImpactKind.Floor ? SfxType.CubeHitFloor
                         : SfxType.CubeHitWall;
            audioManager.PlayImpact(type, strength);
        }

        private void HandleRollSettled()
        {
            audioManager.PlaySfx(SfxType.CubeSettle);
        }

        private void HandleRackMoved(bool lifting)
        {
            audioManager.PlaySfx(SfxType.RackMove);
        }
    }
}
