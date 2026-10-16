using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;
using ColorGame.Machine;

namespace ColorGame.UI
{
    /// <summary>
    /// Owns the three result boxes.
    /// PHYSICAL machine (Machine3DPresenter): the manager announces the roll with NO colors. The boxes stay "?"
    ///   until every cube has settled and the real result is shown, then they reveal left to right.
    /// 2D ColorMachine: boxes stay "?" and reveal each color when its die settles.
    /// No machine: the old flicker animation runs (cosmetic random colors, then the true final colors).
    /// Never decides what is a win.
    /// </summary>
    public class ColorResultUI : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private ResultSlotView slotPrefab;
        [SerializeField] private Transform slotContainer;

        [Header("Optional")]
        [Tooltip("Only for the OLD 2D ColorMachine. Leave empty for the 3D machine.")]
        [SerializeField] private ColorMachine machine;

        [Header("Reveal (physical machine)")]
        [Tooltip("Seconds between the three boxes being revealed. Keep 3 x this below the Result Reveal Delay in the config.")]
        [SerializeField] private float revealStagger = 0.12f;

        /// <summary>Raised with the slot number (0, 1, 2) each time a slot stops (ColorGameAudio listens).</summary>
        public event System.Action<int> OnSlotStopped;

        private readonly List<ResultSlotView> slots = new List<ResultSlotView>();
        private Coroutine spinRoutine;
        private Coroutine revealRoutine;
        private bool awaitingPhysicalResult;
        private bool isReady;

        private void Awake()
        {
            if (config == null || gameManager == null || slotPrefab == null || slotContainer == null)
            {
                Debug.LogError("[ColorResultUI] A reference is missing in the Inspector.", this);
                return;
            }

            for (int i = 0; i < ColorGameConfig.ResultSlotCount; i++)
            {
                ResultSlotView slot = Instantiate(slotPrefab, slotContainer);
                slot.name = "ResultSlot" + (i + 1);
                slot.Init();
                slots.Add(slot);
            }

            isReady = true;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnRollStarted += HandleRollStarted;
            gameManager.OnResultsShown += HandleResultsShown;
            gameManager.OnRoundVoided += HandleRoundVoided;
            gameManager.OnNoBetResultShown += HandleNoBetResultShown;
            gameManager.OnBettingWindowOpened += HandleBettingWindowOpened;
            if (machine != null) machine.OnSlotSettled += HandleMachineSlotSettled;
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnRollStarted -= HandleRollStarted;
                gameManager.OnResultsShown -= HandleResultsShown;
                gameManager.OnRoundVoided -= HandleRoundVoided;
                gameManager.OnNoBetResultShown -= HandleNoBetResultShown;
                gameManager.OnBettingWindowOpened -= HandleBettingWindowOpened;
            }
            if (machine != null) machine.OnSlotSettled -= HandleMachineSlotSettled;
        }

        private void HandleRollStarted(GameColor[] finalResults)
        {
            StopAnimations();

            // finalResults is null for the physical machine: the answer does not exist yet.
            awaitingPhysicalResult = finalResults == null;

            if (machine != null || finalResults == null)
            {
                // The answer must not be visible before the cubes or dice have settled.
                foreach (ResultSlotView slot in slots) slot.ShowUnknown();
                return;
            }

            foreach (ResultSlotView slot in slots) slot.ClearHighlight();
            spinRoutine = StartCoroutine(SpinRoutine(finalResults));
        }

        // A new betting window: the boxes go back to "?" so the new round starts clean.
        private void HandleBettingWindowOpened()
        {
            StopAnimations();
            awaitingPhysicalResult = false;
            foreach (ResultSlotView slot in slots) slot.ShowUnknown();
        }

        // A round WITHOUT a wager: reveal the real cube colors, with no match highlighting and no win or loss.
        private void HandleNoBetResultShown(GameColor[] results)
        {
            StopAnimations();
            awaitingPhysicalResult = false;
            if (results == null) return;
            revealRoutine = StartCoroutine(RevealRoutine(results, null));
        }

        private void HandleRoundVoided(string roundId)
        {
            StopAnimations();
            awaitingPhysicalResult = false;
            foreach (ResultSlotView slot in slots) slot.ShowUnknown();
        }

        // A physical die came to rest (2D machine only): reveal its color in the matching box.
        private void HandleMachineSlotSettled(int slotIndex, GameColor color)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count) return;

            if (config.TryGetColorDefinition(color, out ColorDefinition definition))
            {
                slots[slotIndex].ShowColor(definition, true);
            }

            OnSlotStopped?.Invoke(slotIndex);
        }

        private void HandleResultsShown(GameColor[] finalResults, PayoutResult payout)
        {
            StopAnimations();

            if (awaitingPhysicalResult)
            {
                // The cubes have settled: reveal the real colors left to right, then highlight the matches.
                awaitingPhysicalResult = false;
                revealRoutine = StartCoroutine(RevealRoutine(finalResults, payout));
                return;
            }

            // Make sure the boxes show the true final colors.
            for (int i = 0; i < slots.Count && i < finalResults.Length; i++)
            {
                if (config.TryGetColorDefinition(finalResults[i], out ColorDefinition definition))
                {
                    slots[i].ShowColor(definition, false);
                }
            }

            ApplyHighlights(payout);
        }

        // Highlight the boxes that matched. The matching itself was decided by PayoutCalculator.
        private void ApplyHighlights(PayoutResult payout)
        {
            for (int i = 0; i < slots.Count && i < payout.SlotMatches.Count; i++)
            {
                bool matched = payout.SlotMatches[i];
                slots[i].SetHighlighted(matched);
                slots[i].SetDimmed(payout.IsWin && !matched);
            }
        }

        private IEnumerator RevealRoutine(GameColor[] finalResults, PayoutResult payout)
        {
            for (int i = 0; i < slots.Count && i < finalResults.Length; i++)
            {
                if (config.TryGetColorDefinition(finalResults[i], out ColorDefinition definition))
                {
                    slots[i].ShowColor(definition, true);
                }
                OnSlotStopped?.Invoke(i);
                yield return new WaitForSeconds(Mathf.Max(0f, revealStagger));
            }

            if (payout != null) ApplyHighlights(payout);
            revealRoutine = null;
        }

        // ---------- Old flicker animation (used only when no machine is assigned and no physical result) ----------

        private IEnumerator SpinRoutine(GameColor[] finalResults)
        {
            float[] stopTimes = { config.Slot1StopTime, config.Slot2StopTime, config.Slot3StopTime };
            bool[] stopped = new bool[slots.Count];
            int stoppedCount = 0;
            float elapsed = 0f;
            float swapTimer = 0f;

            while (stoppedCount < slots.Count)
            {
                elapsed += Time.deltaTime;
                swapTimer += Time.deltaTime;

                bool swapNow = swapTimer >= config.SpinSwapInterval;
                if (swapNow) swapTimer = 0f;

                for (int i = 0; i < slots.Count; i++)
                {
                    if (stopped[i]) continue;

                    float stopTime = i < stopTimes.Length ? stopTimes[i] : stopTimes[stopTimes.Length - 1];

                    if (elapsed >= stopTime)
                    {
                        if (config.TryGetColorDefinition(finalResults[i], out ColorDefinition finalDefinition))
                        {
                            slots[i].ShowColor(finalDefinition, true);
                        }
                        stopped[i] = true;
                        stoppedCount++;
                        OnSlotStopped?.Invoke(i);
                    }
                    else if (swapNow)
                    {
                        slots[i].ShowColor(RandomDefinition(), false);
                    }
                }

                yield return null;
            }

            spinRoutine = null;
        }

        private ColorDefinition RandomDefinition()
        {
            int index = Random.Range(0, config.Colors.Count);
            return config.Colors[index];
        }

        private void StopAnimations()
        {
            if (spinRoutine != null)
            {
                StopCoroutine(spinRoutine);
                spinRoutine = null;
            }
            if (revealRoutine != null)
            {
                StopCoroutine(revealRoutine);
                revealRoutine = null;
            }
        }
    }
}
