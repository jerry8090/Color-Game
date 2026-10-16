using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;
using Random = UnityEngine.Random;

namespace ColorGame.Machine
{
    /// <summary>
    /// The physical color-dice machine. It DOES NOT decide results.
    /// ColorGameManager hands it the official colors. It spawns exactly three dice carrying those
    /// colors, releases them from the top, lets physics bounce and roll them, waits until each one
    /// has really come to rest in its slot, then reads the colors back as the "confirmed" result.
    ///
    /// Safety: dice that leave the machine are put back at the top. Dice that never settle are glided
    /// into their slot after a time limit, so a round can never hang.
    /// </summary>
    public class ColorMachine : MonoBehaviour, IRollPresenter
    {
        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private ColorDie dicePrefab;
        [SerializeField] private Transform diceContainer;
        [Tooltip("Exactly 3. Where each die appears above its chute.")]
        [SerializeField] private Transform[] spawnPoints;
        [Tooltip("Exactly 3. Where each die should rest on the tray floor (the result slots).")]
        [SerializeField] private Transform[] slotPoints;

        [Header("Timing (seconds)")]
        [SerializeField] private float prePause = 0.35f;
        [SerializeField] private float releaseStagger = 0.22f;
        [SerializeField] private float minSettleTime = 1.2f;
        [SerializeField] private float settleHold = 0.4f;
        [SerializeField] private float maxRollTime = 8f;
        [SerializeField] private float guideDuration = 0.6f;
        [SerializeField] private float finalPause = 0.3f;

        [Header("Release")]
        [SerializeField] private float spawnJitterX = 0.6f;
        [SerializeField] private float initialDownSpeed = 2f;
        [SerializeField] private float initialSideSpeed = 1.2f;
        [SerializeField] private float initialSpin = 240f;

        [Header("Settle detection")]
        [SerializeField] private float settleSpeed = 0.08f;
        [SerializeField] private float settleAngularSpeed = 10f;
        [SerializeField] private float slotTolerance = 0.4f;
        [SerializeField] private float restHeightTolerance = 0.35f;

        [Header("Tray assist (gentle sideways pull toward the die's own slot)")]
        [SerializeField] private float trayAssistLocalY = -1.0f;
        [Tooltip("In multiples of the die's weight per unit of distance.")]
        [SerializeField] private float magnetStrength = 1.5f;
        [Tooltip("Maximum pull, as a fraction of the die's weight.")]
        [SerializeField] private float magnetMaxFraction = 1.0f;

        [Header("Safety bounds (relative to this object)")]
        [SerializeField] private float boundsHalfWidth = 4.6f;
        [SerializeField] private float floorKillY = -3.3f;
        [SerializeField] private float ceilingY = 6f;
        [SerializeField] private int maxRecoveries = 2;

        /// <summary>Raised when one die has come to rest in its slot (slot number 0 to 2, and its color).</summary>
        public event Action<int, GameColor> OnSlotSettled;

        /// <summary>Raised on every die collision: slot number and impact speed (optional impact sounds).</summary>
        public event Action<int, float> OnDieImpact;

        private class DieState
        {
            public ColorDie die;
            public int slot;
            public Vector3 restPosition;
            public float releaseDelay;
            public bool released;
            public bool settled;
            public bool guided;
            public float sinceRelease;
            public float settleTimer;
            public int recoveries;
            public float guideT;
            public Vector3 guideFrom;
            public Quaternion guideFromRotation;
        }

        private readonly List<DieState> states = new List<DieState>();
        private bool isReady;
        private bool isPlaying;

        public bool IsPlaying => isPlaying;

        private void OnValidate()
        {
            prePause = Mathf.Max(0f, prePause);
            releaseStagger = Mathf.Max(0f, releaseStagger);
            minSettleTime = Mathf.Max(0.2f, minSettleTime);
            settleHold = Mathf.Max(0.1f, settleHold);
            maxRollTime = Mathf.Max(3f, maxRollTime);
            guideDuration = Mathf.Max(0.1f, guideDuration);
            maxRecoveries = Mathf.Max(0, maxRecoveries);
        }

        private void Awake()
        {
            if (config == null || gameManager == null || dicePrefab == null || diceContainer == null ||
                !HasPoints(spawnPoints) || !HasPoints(slotPoints))
            {
                Debug.LogError("[ColorMachine] A reference is missing in the Inspector " +
                               "(Spawn Points and Slot Points need exactly 3 entries each).", this);
                return;
            }

            isReady = true;
        }

        private void OnEnable()
        {
            if (isReady) gameManager.OnResultsShown += HandleResultsShown;
        }

        private void OnDisable()
        {
            if (gameManager != null) gameManager.OnResultsShown -= HandleResultsShown;
        }

        private static bool HasPoints(Transform[] points)
        {
            if (points == null || points.Length != ColorGameConfig.ResultSlotCount) return false;
            foreach (Transform point in points)
            {
                if (point == null) return false;
            }
            return true;
        }

        // Physics-rate tray assist: a gentle sideways pull toward the die's own slot, only near the floor.
        private void FixedUpdate()
        {
            if (!isPlaying) return;

            float gravity = Mathf.Abs(Physics2D.gravity.y);

            foreach (DieState s in states)
            {
                if (s.die == null || !s.released || s.settled || s.guided) continue;

                Vector3 position = s.die.transform.position;
                if (transform.InverseTransformPoint(position).y > trayAssistLocalY) continue;

                Rigidbody2D rb = s.die.Body;
                float weight = rb.mass * gravity * rb.gravityScale;
                float dx = s.restPosition.x - position.x;
                float force = Mathf.Clamp(dx * magnetStrength, -magnetMaxFraction, magnetMaxFraction) * weight;
                rb.AddForce(new Vector2(force, 0f));
            }
        }

        // ---------- IRollPresenter ----------

        public IEnumerator PlayRoll(GameColor[] officialResult, Action<GameColor[]> onConfirmed)
        {
            if (!isReady)
            {
                Debug.LogError("[ColorMachine] Cannot roll: the machine is not set up correctly.", this);
                yield break;
            }

            if (isPlaying)
            {
                Debug.LogWarning("[ColorMachine] A roll is already in progress.", this);
                yield break;
            }

            int count = ColorGameConfig.ResultSlotCount;

            if (officialResult == null || officialResult.Length != count)
            {
                Debug.LogError($"[ColorMachine] The official result must contain exactly {count} colors.", this);
                yield break;
            }

            // Check every color BEFORE touching the scene.
            ColorDefinition[] definitions = new ColorDefinition[count];
            for (int i = 0; i < count; i++)
            {
                if (!config.TryGetColorDefinition(officialResult[i], out definitions[i])) yield break;
            }

            isPlaying = true;

            try
            {
                ClearDice();
                yield return new WaitForSeconds(prePause);

                for (int i = 0; i < count; i++)
                {
                    states.Add(SpawnDie(i, definitions[i]));
                }

                float elapsed = 0f;
                int releasedCount = 0;
                bool allSettled = false;

                while (!allSettled)
                {
                    float dt = Time.deltaTime;
                    elapsed += dt;

                    // Release the dice one after another from the top.
                    while (releasedCount < states.Count && elapsed >= states[releasedCount].releaseDelay)
                    {
                        Release(states[releasedCount]);
                        releasedCount++;
                    }

                    bool timedOut = elapsed >= maxRollTime;
                    allSettled = true;

                    foreach (DieState s in states)
                    {
                        if (s.settled) continue;

                        if (s.die == null)
                        {
                            s.settled = true; // destroyed from outside: do not wait for it forever
                            continue;
                        }

                        allSettled = false;
                        if (!s.released) continue;

                        s.sinceRelease += dt;

                        if (s.guided)
                        {
                            UpdateGuide(s, dt);
                            continue;
                        }

                        if (timedOut)
                        {
                            Debug.LogWarning($"[ColorMachine] Die {s.slot + 1} did not settle in time. Guiding it into its slot.", this);
                            StartGuide(s);
                            continue;
                        }

                        if (IsOutOfBounds(s.die.transform.position))
                        {
                            Recover(s);
                            if (s.guided) continue;
                        }

                        UpdateSettle(s, dt);
                    }

                    yield return null;
                }

                yield return new WaitForSeconds(finalPause);

                // Identify the final three: read the logical colors back from the physical dice, in slot order.
                GameColor[] confirmed = new GameColor[count];
                for (int i = 0; i < count; i++)
                {
                    if (states[i].die == null)
                    {
                        Debug.LogError("[ColorMachine] A die disappeared. The result cannot be confirmed.", this);
                        yield break;
                    }
                    confirmed[i] = states[i].die.AssignedColor;
                }

                onConfirmed?.Invoke(confirmed);
            }
            finally
            {
                isPlaying = false;
            }
        }

        // ---------- Spawning and releasing ----------

        private DieState SpawnDie(int index, ColorDefinition definition)
        {
            Vector3 position = spawnPoints[index].position + new Vector3(Random.Range(-spawnJitterX, spawnJitterX), 0f, 0f);

            ColorDie die = Instantiate(dicePrefab, position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), diceContainer);
            die.name = $"ResultDie{index + 1}_{definition.color}";
            SetLayerRecursive(die.gameObject, gameObject.layer);
            die.Setup(definition, index);
            die.Body.simulated = false; // waits above the machine until it is released
            die.OnImpact += HandleDieImpact;

            return new DieState
            {
                die = die,
                slot = index,
                restPosition = slotPoints[index].position,
                releaseDelay = index * releaseStagger + Random.Range(0f, releaseStagger * 0.5f)
            };
        }

        private void Release(DieState s)
        {
            if (s.die == null) return;

            s.released = true;
            s.sinceRelease = 0f;

            Vector2 velocity = new Vector2(Random.Range(-initialSideSpeed, initialSideSpeed), -initialDownSpeed);
            s.die.BeginSimulation(velocity, Random.Range(-initialSpin, initialSpin));
        }

        // ---------- Settling ----------

        private void UpdateSettle(DieState s, float dt)
        {
            Rigidbody2D rb = s.die.Body;
            Vector3 position = s.die.transform.position;

            bool slow = rb.linearVelocity.sqrMagnitude <= settleSpeed * settleSpeed &&
                        Mathf.Abs(rb.angularVelocity) <= settleAngularSpeed;

            bool inPlace = Mathf.Abs(position.x - s.restPosition.x) <= slotTolerance &&
                           position.y <= s.restPosition.y + restHeightTolerance;

            // Stable-settle check: slow AND in its slot AND not too early, for a continuous period.
            if (slow && inPlace && s.sinceRelease >= minSettleTime) s.settleTimer += dt;
            else s.settleTimer = 0f;

            if (s.settleTimer >= settleHold) Settle(s);
        }

        private void Settle(DieState s)
        {
            if (s.settled) return;

            s.settled = true;
            s.die.Freeze();
            OnSlotSettled?.Invoke(s.slot, s.die.AssignedColor);
        }

        // ---------- Safety: escaped and stuck dice ----------

        private bool IsOutOfBounds(Vector3 worldPosition)
        {
            Vector3 local = transform.InverseTransformPoint(worldPosition);
            return Mathf.Abs(local.x) > boundsHalfWidth || local.y < floorKillY || local.y > ceilingY;
        }

        private void Recover(DieState s)
        {
            s.recoveries++;

            if (s.recoveries > maxRecoveries)
            {
                Debug.LogWarning($"[ColorMachine] Die {s.slot + 1} escaped too often. Guiding it into its slot.", this);
                StartGuide(s);
                return;
            }

            Debug.LogWarning($"[ColorMachine] Die {s.slot + 1} left the machine. Putting it back at the top.", this);

            Vector3 position = spawnPoints[s.slot].position + new Vector3(Random.Range(-spawnJitterX, spawnJitterX), 0f, 0f);
            Rigidbody2D rb = s.die.Body;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = position;
            rb.rotation = Random.Range(0f, 360f);
            s.die.transform.position = position;

            s.sinceRelease = 0f;
            s.settleTimer = 0f;
        }

        // Smoothly glides a stuck die into its slot (a visible move, not a teleport).
        private void StartGuide(DieState s)
        {
            s.guided = true;
            s.guideT = 0f;
            s.guideFrom = s.die.transform.position;
            s.guideFromRotation = s.die.transform.rotation;
            s.die.Freeze();
        }

        private void UpdateGuide(DieState s, float dt)
        {
            s.guideT += dt / guideDuration;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(s.guideT));

            s.die.transform.position = Vector3.Lerp(s.guideFrom, s.restPosition, t);
            s.die.transform.rotation = Quaternion.Slerp(s.guideFromRotation, Quaternion.identity, t);

            if (s.guideT >= 1f) Settle(s);
        }

        // ---------- Highlighting (display only) ----------

        // The matches were decided by PayoutCalculator. The machine only shows them.
        private void HandleResultsShown(GameColor[] results, PayoutResult payout)
        {
            for (int i = 0; i < states.Count && i < payout.SlotMatches.Count; i++)
            {
                if (states[i].die == null) continue;

                bool matched = payout.SlotMatches[i];
                states[i].die.SetHighlighted(matched);
                states[i].die.SetDimmed(payout.IsWin && !matched);
            }
        }

        private void HandleDieImpact(ColorDie die, float speed)
        {
            OnDieImpact?.Invoke(die.SlotIndex, speed);
        }

        // ---------- Cleanup ----------

        private void ClearDice()
        {
            foreach (DieState s in states)
            {
                if (s.die == null) continue;
                s.die.OnImpact -= HandleDieImpact;
                Destroy(s.die.gameObject);
            }
            states.Clear();

            // Remove anything else left in the container.
            for (int i = diceContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(diceContainer.GetChild(i).gameObject);
            }
        }

        private static void SetLayerRecursive(GameObject target, int layer)
        {
            target.layer = layer;
            foreach (Transform child in target.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }

        // ---------- Test tool ----------

        [ContextMenu("TEST: Roll random dice (no wallet, no payout)")]
        private void TestRoll()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[ColorMachine] Press Play first.");
                return;
            }

            GameColor[] official = new ColorResultGenerator().GenerateResult();
            Debug.Log("[ColorMachine] TEST official result: " + string.Join(" / ", official));

            StartCoroutine(PlayRoll(official, confirmed =>
                Debug.Log("[ColorMachine] TEST confirmed: " + string.Join(" / ", confirmed))));
        }
    }
}