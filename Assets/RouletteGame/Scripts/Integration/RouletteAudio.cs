using UnityEngine;
using Shared.Audio;
using RouletteGame.Core;
using RouletteGame.Wheel;

namespace RouletteGame.Integration
{
    /// <summary>
    /// The ONE place that connects Roulette events to sounds, using the existing AudioManager (no second one).
    /// The controller and the wheel contain no audio code. They raise events, and this script listens.
    /// Every subscription is made in OnEnable and removed in OnDisable, so a new round never adds a second handler.
    /// Master volume and mute come from the shared sound setting through AudioManager.
    /// Put this on any always-active object (the builder puts it next to the controller).
    /// </summary>
    public class RouletteAudio : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private RouletteGameController controller;
        [SerializeField] private WheelView wheel;

        private string lastSoundRoundId;
        private bool isReady;

        private void Awake()
        {
            if (audioManager == null || controller == null || wheel == null)
            {
                Debug.LogError("[RouletteAudio] A reference is missing in the Inspector.", this);
                return;
            }
            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            controller.OnChipPlaced += HandleChipPlaced;
            controller.OnChipRemoved += HandleChipRemoved;
            controller.OnBetsConfirmed += HandleConfirmed;
            controller.OnBetsUnlocked += HandleButton;
            controller.OnButtonPressed += HandleButton;
            controller.OnSpinBegan += HandleSpinBegan;
            controller.OnRoundResult += HandleRoundResult;
            wheel.OnBallDropped += HandleBallDropped;
            wheel.OnSpinFinished += HandleSpinFinished;
        }

        private void OnDisable()
        {
            if (controller != null)
            {
                controller.OnChipPlaced -= HandleChipPlaced;
                controller.OnChipRemoved -= HandleChipRemoved;
                controller.OnBetsConfirmed -= HandleConfirmed;
                controller.OnBetsUnlocked -= HandleButton;
                controller.OnButtonPressed -= HandleButton;
                controller.OnSpinBegan -= HandleSpinBegan;
                controller.OnRoundResult -= HandleRoundResult;
            }
            if (wheel != null)
            {
                wheel.OnBallDropped -= HandleBallDropped;
                wheel.OnSpinFinished -= HandleSpinFinished;
            }

            if (audioManager != null) audioManager.StopMachineSounds();
        }

        private void HandleChipPlaced() { audioManager.PlaySfx(SfxType.ChipPlace); }
        private void HandleChipRemoved() { audioManager.PlaySfx(SfxType.ChipRemove); }
        private void HandleConfirmed() { audioManager.PlaySfx(SfxType.BetConfirm); }
        private void HandleButton() { audioManager.PlaySfx(SfxType.ButtonClick); }

        // The wheel and the rattling ball are one long sound on the machine voice, so it can be stopped.
        private void HandleSpinBegan() { audioManager.PlaySfx(SfxType.WheelSpin); }

        private void HandleBallDropped()
        {
            audioManager.PlaySfx(SfxType.BallDrop);
        }

        private void HandleSpinFinished(int number)
        {
            audioManager.StopMachineSounds();
        }

        // Exactly ONE result sound per round.
        private void HandleRoundResult(RouletteSettlementResult result)
        {
            if (result == null || result.Payout == null) return;
            if (result.RoundId == lastSoundRoundId) return;
            lastSoundRoundId = result.RoundId;

            audioManager.PlaySfx(result.Payout.WinningBetCount > 0 ? SfxType.Win : SfxType.Lose);
        }
    }
}
