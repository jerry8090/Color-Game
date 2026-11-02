using System;
using System.Collections;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;
using Random = UnityEngine.Random;

namespace ColorGame.Machine
{
    /// <summary>
    /// The 3D cabinet. The result comes from REAL physics only.
    ///   1. The rack lifts and the three cubes are released with a little random spin.
    ///   2. Unity physics (stepped by hand, only while rolling) lets them slide, fall, collide and tumble.
    ///   3. When all three cubes have genuinely stopped inside the tray, the top face of each is READ.
    ///   4. Those colors are reported to ColorGameManager. Nothing is predicted, rotated, swapped or corrected.
    ///
    /// If a cube ends up stuck, leaning, outside the tray or never settles, the cubes go back to the shelf and
    /// are released again (no extra cost). After Max Release Attempts the roll fails and the manager refunds the bet.
    /// After the round (OnRoundCompleted or OnRoundVoided) the cubes return to the shelf and the rack lowers.
    /// It never decides a win and never touches the wallet.
    /// </summary>
    public class Machine3DPresenter : MonoBehaviour, IPhysicalRollPresenter ,IForceableRollPresenter
    {
        private static readonly Vector3[] FaceNormals =
        {
            Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
        };

        private static readonly int Count = ColorGameConfig.ResultSlotCount;

        [Header("References")]
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [Tooltip("Exactly 3 cubes, left to right.")]
        [SerializeField] private ColorCube[] cubes;
        [SerializeField] private Rigidbody rackBody;

        [Header("Physics world (3D only)")]
        [SerializeField] private float gravityY = -20f;
        [Tooltip("Seconds per physics step. 1/120 looks smoothest. 1/60 is cheaper.")]
        [SerializeField] private float simulationStep = 1f / 120f;
        [Tooltip("Safety cap when the frame rate drops.")]
        [SerializeField] private int maxStepsPerFrame = 8;

        [Header("Rack (the gate)")]
        [SerializeField] private float rackTravel = 1.7f;
        [Tooltip("Seconds the cubes wait before the rack starts to lift.")]
        [SerializeField] private float rackStartDelay = 0.25f;
        [SerializeField] private float rackDuration = 0.8f;

        [Header("Release variation (truly random every roll)")]
        [Tooltip("Random spin given to each cube when the gate lifts (radians per second).")]
        [SerializeField] private float releaseSpin = 4f;
        [SerializeField] private float releaseSideSpeed = 0.5f;

        [Header("Settle detection")]
        [SerializeField] private float settleSpeed = 0.1f;
        [SerializeField] private float settleAngularSpeed = 0.3f;
        [Tooltip("A cube counts as 'in the tray' when its height (relative to this object) is below this.")]
        [SerializeField] private float settleBelowLocalY = 1.0f;
        [Tooltip("All cubes must stay still for this long.")]
        [SerializeField] private float settleHold = 0.5f;
        [SerializeField] private float minSimTime = 1.5f;
        [Tooltip("Fallback. The attempt fails here if something never settled.")]
        [SerializeField] private float maxSimTime = 8f;

        [Header("Stuck cubes on the shelf (a physical nudge, not a result correction)")]
        [SerializeField] private float stuckGrace = 1.5f;
        [SerializeField] private float stuckSpeed = 0.3f;
        [Tooltip("A cube higher than this (relative to this object) is still on the shelf.")]
        [SerializeField] private float shelfZoneLocalY = 1.8f;
        [SerializeField] private float stuckPushSpeed = 1.5f;
        [SerializeField] private Vector3 releasePushDirection = new Vector3(0f, -0.4f, 0.9f);
        [SerializeField] private int maxPushes = 3;

        [Header("Landing check (is the roll readable?)")]
        [Tooltip("The top face must point up at least this much. 1 = perfectly flat, 0.9 = about 25 degrees of lean.")]
        [Range(0.5f, 0.999f)] [SerializeField] private float minFlatness = 0.9f;
        [Tooltip("If the landing is not clean, the cubes are released again, up to this many times in total.")]
        [SerializeField] private int maxReleaseAttempts = 3;

        [Header("After the round")]
        [SerializeField] private float resultHold = 0.8f;
        [SerializeField] private float returnDuration = 0.8f;
        [SerializeField] private float returnArcHeight = 1.2f;
        [SerializeField] private float rackLowerDuration = 0.6f;

        [Header("Win glow")]
        [SerializeField] private Color glowColor = new Color(1f, 0.85f, 0.3f, 1f);
        [SerializeField] private float glowIntensity = 6f;
        [SerializeField] private float glowRange = 3.5f;
        [SerializeField] private float glowPulseSpeed = 6f;

        [Header("Debug")]
        [SerializeField] private bool verboseLog = false;

        public int RollsPlayed { get; private set; }
        public int InvalidLandings { get; private set; }
        public bool IsBusy => isRolling || isResetting;

        /// <summary>A cube hit something during a real visible roll. Kind and strength (0 to 1). ColorGameAudio plays it.</summary>
        public event Action<ImpactKind, float> OnImpact;

        /// <summary>All cubes came to rest and the result was read.</summary>
        public event Action OnRollSettled;

        /// <summary>The rack started moving. True = lifting (cubes released), false = lowering back.</summary>
        public event Action<bool> OnRackMoved;

        private sealed class SimRun
        {
            public int step;
            public float stable;
            public bool released;
            public bool done;
            public bool timedOut;
            public int[] pushes;
            public float[] nextPushTime;
        }

        private Vector3[] cubeStartPositions;
        private Quaternion[] cubeStartRotations;
        private Vector3 rackStartPosition;
        private Quaternion rackStartRotation;
        private Light[] glows;
        private CubeSoundEmitter[] emitters;
        private bool soundsActive;
        private Coroutine glowRoutine;

        private SimulationMode previousSimulationMode;
        private Vector3 previousGravity;
        private bool changedPhysics;

        private bool isReady;
        private bool isRolling;
        private bool isResetting;
        private bool cubesAtStart = true;
        private bool lastRunTimedOut;

        private void OnValidate()
        {
            simulationStep = Mathf.Clamp(simulationStep, 1f / 240f, 1f / 30f);
            maxStepsPerFrame = Mathf.Max(1, maxStepsPerFrame);
            rackDuration = Mathf.Max(0.1f, rackDuration);
            settleHold = Mathf.Max(0.1f, settleHold);
            minSimTime = Mathf.Max(rackStartDelay + rackDuration + 0.2f, minSimTime);
            maxSimTime = Mathf.Max(minSimTime + 1f, maxSimTime);
            maxPushes = Mathf.Max(0, maxPushes);
            maxReleaseAttempts = Mathf.Clamp(maxReleaseAttempts, 1, 10);
        }

        private void Awake()
        {
            bool missing = config == null || gameManager == null || rackBody == null ||
                           cubes == null || cubes.Length != Count;

            if (!missing)
            {
                foreach (ColorCube cube in cubes)
                {
                    if (cube == null) missing = true;
                }
            }

            if (missing)
            {
                Debug.LogError($"[Machine3DPresenter] A reference is missing in the Inspector (Cubes needs exactly {Count} entries).", this);
                return;
            }

            if (config.Colors.Count < ColorCube.FaceCount)
            {
                Debug.LogError("[Machine3DPresenter] The config needs at least six colors.", this);
                return;
            }

            // Physics is stepped by hand, only during a roll. The 3D world is used by this machine only.
            previousSimulationMode = Physics.simulationMode;
            previousGravity = Physics.gravity;
            Physics.simulationMode = SimulationMode.Script;
            Physics.gravity = new Vector3(0f, gravityY, 0f);
            changedPhysics = true;

            rackBody.isKinematic = true;
            rackBody.useGravity = false;
            rackBody.interpolation = RigidbodyInterpolation.None;

            cubeStartPositions = new Vector3[Count];
            cubeStartRotations = new Quaternion[Count];
            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                rb.sleepThreshold = 0f; // never fall asleep while pressed against the gate
                rb.interpolation = RigidbodyInterpolation.None;
                cubeStartPositions[i] = cubes[i].transform.position;
                cubeStartRotations[i] = cubes[i].transform.rotation;
            }

            rackStartPosition = rackBody.transform.position;
            rackStartRotation = rackBody.transform.rotation;

            CreateGlows();
            CreateEmitters();
            isReady = true;
        }

        private void Start()
        {
            if (!isReady) return;

            // Idle look: each cube shows a different arrangement of the six colors. Every cube has all six colors.
            for (int i = 0; i < Count; i++) cubes[i].ApplyStandardLayout(config, i * 2);
        }

        private void OnEnable()
        {
            if (!isReady) return;
            gameManager.OnResultsShown += HandleResultsShown;
            gameManager.OnRoundCompleted += HandleRoundCompleted;
            gameManager.OnRoundVoided += HandleRoundVoided;
            gameManager.OnNoBetRoundCompleted += HandleNoBetRoundCompleted;

            foreach (CubeSoundEmitter emitter in emitters) emitter.OnImpact += HandleImpact;
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnResultsShown -= HandleResultsShown;
                gameManager.OnRoundCompleted -= HandleRoundCompleted;
                gameManager.OnRoundVoided -= HandleRoundVoided;
                gameManager.OnNoBetRoundCompleted -= HandleNoBetRoundCompleted;
            }

            if (!isReady) return;

            foreach (CubeSoundEmitter emitter in emitters)
            {
                if (emitter != null) emitter.OnImpact -= HandleImpact;
            }
            SetSoundsActive(false);
            forcedColors = null;

            // Coroutines stop on their own. Put the machine back into a clean ready state.
            isRolling = false;
            isResetting = false;
            ClearGlow();
            RestoreStartPose();
        }

        private void OnDestroy()
        {
            if (!changedPhysics) return;
            Physics.simulationMode = previousSimulationMode;
            Physics.gravity = previousGravity;
        }

        // ---------- IPhysicalRollPresenter ----------

        public IEnumerator PlayPhysicalRoll(Action<GameColor[]> onResult)
        {
            if (!isReady)
            {
                Debug.LogError("[Machine3DPresenter] Cannot roll: the machine is not set up correctly.", this);
                yield break;
            }

            if (isRolling)
            {
                Debug.LogWarning("[Machine3DPresenter] A roll is already in progress.", this);
                yield break;
            }

            isRolling = true;

            try
            {
                // A new roll may be requested while the previous round is still returning the cubes.
                while (isResetting) yield return null;

                ClearGlow();
                if (!cubesAtStart) RestoreStartPose();

                for (int attempt = 1; attempt <= maxReleaseAttempts; attempt++)
                {
                    if (attempt > 1)
                    {
                        // Cubes glide back to the shelf and the gate lowers, then everything is released again.
                        yield return ReturnCubes();
                        yield return LowerRack();
                        RestoreStartPose();
                    }

                    yield return PlayVisibleRun();

                    if (TryReadResult(out GameColor[] colors, out string problem))
                    {
                        RollsPlayed++;
                        OnRollSettled?.Invoke();
                        if (verboseLog) Debug.Log("[Machine3DPresenter] Cubes show: " + string.Join(" / ", colors));
                        onResult?.Invoke(colors);
                        yield break;
                    }

                    InvalidLandings++;
                    Debug.LogWarning($"[Machine3DPresenter] Attempt {attempt}/{maxReleaseAttempts}: {problem}", this);
                }

                Debug.LogError("[Machine3DPresenter] The cubes could not settle cleanly. No result is reported, " +
                               "so the round will be cancelled and the bet refunded.", this);
            }
            finally
            {
                isRolling = false;
                SetSoundsActive(false);
            }
        }

        // ---------- Simulation ----------

        private static SimRun NewRun(int count)
        {
            return new SimRun { pushes = new int[count], nextPushTime = new float[count] };
        }

        // Plays one roll on screen: the physics is stepped at a fixed rate while the frames render.
        private IEnumerator PlayVisibleRun()
        {
            cubesAtStart = false;
            SetSoundsActive(true); // collisions are only audible during a real visible roll

            SimRun run = NewRun(Count);
            float accumulator = 0f;

            while (!run.done)
            {
                accumulator += Time.deltaTime;
                int steps = 0;

                while (accumulator >= simulationStep && !run.done)
                {
                    StepSimulation(run);
                    accumulator -= simulationStep;
                    steps++;

                    if (steps >= maxStepsPerFrame)
                    {
                        accumulator = 0f;
                        break;
                    }
                }

                yield return null;
            }

            lastRunTimedOut = run.timedOut;
            if (run.timedOut) Debug.LogWarning("[Machine3DPresenter] The roll hit the time limit.", this);
        }

        private void StepSimulation(SimRun run)
        {
            run.step++;
            float t = run.step * simulationStep;

            float p = Mathf.Clamp01((t - rackStartDelay) / rackDuration);
            rackBody.MovePosition(rackStartPosition + Vector3.up * (rackTravel * Mathf.SmoothStep(0f, 1f, p)));

            if (!run.released && t >= rackStartDelay)
            {
                run.released = true;
                ApplyReleaseVariation();
                if (soundsActive) OnRackMoved?.Invoke(true);
            }

            Physics.Simulate(simulationStep);

            if (run.released) UnstickCubes(run, t);
            CheckSettled(run, t);
        }

        private void ApplyReleaseVariation()
        {
            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                Vector3 spin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * releaseSpin;
                rb.angularVelocity = spin;
                rb.linearVelocity += new Vector3(Random.Range(-1f, 1f) * releaseSideSpeed, 0f, 0f);
            }
        }

        // A cube that is still resting on the shelf long after the gate opened gets a small physical push.
        // This only helps it start moving. It never decides which face ends up on top.
        private void UnstickCubes(SimRun run, float t)
        {
            if (t < rackStartDelay + rackDuration + stuckGrace) return;

            Vector3 direction = transform.TransformDirection(releasePushDirection.normalized);

            for (int i = 0; i < Count; i++)
            {
                if (run.pushes[i] >= maxPushes || t < run.nextPushTime[i]) continue;

                Rigidbody rb = cubes[i].Body;
                if (LocalY(rb.position) <= shelfZoneLocalY) continue;
                if (rb.linearVelocity.magnitude > stuckSpeed) continue;

                rb.AddForce(direction * stuckPushSpeed, ForceMode.VelocityChange);
                run.pushes[i]++;
                run.nextPushTime[i] = t + 1f;
            }
        }

        private void CheckSettled(SimRun run, float t)
        {
            if (t >= maxSimTime)
            {
                run.done = true;
                run.timedOut = true;
                return;
            }

            if (t < minSimTime)
            {
                run.stable = 0f;
                return;
            }

            bool allStill = true;
            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                bool slow = rb.linearVelocity.magnitude < settleSpeed &&
                            rb.angularVelocity.magnitude < settleAngularSpeed;

                if (!slow || !IsInTray(rb.position))
                {
                    allStill = false;
                    break;
                }
            }

            run.stable = allStill ? run.stable + simulationStep : 0f;
            if (run.stable >= settleHold) run.done = true;
        }

        // ---------- Reading the result ----------

        // Reads the top face of every cube as it really lies. Nothing is changed.
        private bool TryReadResult(out GameColor[] colors, out string problem)
        {
            colors = new GameColor[Count];
            problem = null;

            if (lastRunTimedOut)
            {
                problem = "the cubes did not settle before the time limit.";
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                Quaternion rotation = rb.rotation;
                int face = TopFaceIndex(rotation);
                float up = (rotation * FaceNormals[face]).y;

                if (!IsInTray(rb.position))
                {
                    problem = $"cube {i + 1} is not in the tray.";
                    return false;
                }

                if (up < minFlatness)
                {
                    problem = $"cube {i + 1} is leaning (top face {up:F2} up, needs {minFlatness:F2}).";
                    return false;
                }

                colors[i] = cubes[i].GetFaceColor(face);
            }

            return true;
        }


        // ---------- DEV TEST: forced result ----------

        private GameColor[] forcedColors;

        public void SetForcedResult(GameColor[] colors)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            forcedColors = colors != null && colors.Length == Count ? (GameColor[])colors.Clone() : null;
#endif
        }

        // After the cubes have really settled, turns each one so the forced color is on top.
        // Used once per request, then cleared, so nothing can repeat or override it.
        private IEnumerator ApplyForcedResult()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (forcedColors == null || lastRunTimedOut) yield break;

            GameColor[] target = forcedColors;
            forcedColors = null;

            Quaternion[] from = new Quaternion[Count];
            Quaternion[] to = new Quaternion[Count];

            for (int i = 0; i < Count; i++)
            {
                from[i] = cubes[i].Body.rotation;
                to[i] = from[i];

                // Of the faces with the wanted color, take the one nearest to the top (smallest turn).
                int bestFace = -1;
                float bestUp = float.MinValue;
                for (int f = 0; f < FaceNormals.Length; f++)
                {
                    if (cubes[i].GetFaceColor(f) != target[i]) continue;
                    float up = (from[i] * FaceNormals[f]).y;
                    if (up > bestUp) { bestUp = up; bestFace = f; }
                }

                if (bestFace < 0)
                {
                    Debug.LogError($"[Machine3DPresenter] Cube {i + 1} has no {target[i]} face. Forced result skipped for it.", this);
                    continue;
                }

                to[i] = Quaternion.FromToRotation(from[i] * FaceNormals[bestFace], Vector3.up) * from[i];
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / 0.25f;
                float e = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
                for (int i = 0; i < Count; i++) cubes[i].transform.rotation = Quaternion.Slerp(from[i], to[i], e);
                yield return null;
            }

            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                rb.rotation = to[i];
                cubes[i].transform.rotation = to[i];
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            Physics.SyncTransforms();

            Debug.LogWarning("[Machine3DPresenter] DEV TEST: the cubes were turned to show the forced result.", this);
#else
            yield break;
#endif
        }



        // ---------- Highlight (display only) ----------

        // The matches were decided by PayoutCalculator. The machine only shows them.
        private void HandleResultsShown(GameColor[] results, PayoutResult payout)
        {
            if (!isReady) return;

            bool any = false;
            for (int i = 0; i < Count && i < payout.SlotMatches.Count; i++)
            {
                bool matched = payout.SlotMatches[i];
                glows[i].enabled = matched;
                any |= matched;
            }

            if (glowRoutine != null) StopCoroutine(glowRoutine);
            glowRoutine = any ? StartCoroutine(GlowPulse()) : null;
        }

        private IEnumerator GlowPulse()
        {
            while (true)
            {
                float k = 0.75f + 0.25f * Mathf.Sin(Time.time * glowPulseSpeed);
                foreach (Light glow in glows)
                {
                    if (glow != null && glow.enabled) glow.intensity = glowIntensity * k;
                }
                yield return null;
            }
        }

        private void CreateGlows()
        {
            glows = new Light[Count];
            for (int i = 0; i < Count; i++)
            {
                GameObject go = new GameObject("WinGlow");
                go.layer = cubes[i].gameObject.layer;
                go.transform.SetParent(cubes[i].transform, false);
                go.transform.localPosition = new Vector3(0f, 1.2f, 0f);

                Light light = go.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = glowColor;
                light.range = glowRange;
                light.intensity = glowIntensity;
                light.shadows = LightShadows.None;
                light.enabled = false;
                glows[i] = light;
            }
        }

        private void ClearGlow()
        {
            if (glowRoutine != null)
            {
                StopCoroutine(glowRoutine);
                glowRoutine = null;
            }

            if (glows == null) return;
            foreach (Light glow in glows)
            {
                if (glow != null) glow.enabled = false;
            }
        }

        // ---------- Return to ready ----------

        private void HandleRoundCompleted(GameRound round, PayoutResult payout)
        {
            StartReset();
        }

        private void HandleRoundVoided(string roundId)
        {
            StartReset();
        }

        private void HandleNoBetRoundCompleted(GameColor[] results)
        {
            StartReset(); // a round without a wager resets exactly like a wagered one
        }

        // ---------- Collision sounds (events only: no sound and no physics change here) ----------

        private void CreateEmitters()
        {
            emitters = new CubeSoundEmitter[Count];
            for (int i = 0; i < Count; i++)
            {
                CubeSoundEmitter emitter = cubes[i].GetComponent<CubeSoundEmitter>();
                if (emitter == null) emitter = cubes[i].gameObject.AddComponent<CubeSoundEmitter>();
                emitter.Muted = true;
                emitters[i] = emitter;
            }
        }

        private void SetSoundsActive(bool active)
        {
            soundsActive = active;
            if (emitters == null) return;

            foreach (CubeSoundEmitter emitter in emitters)
            {
                if (emitter != null) emitter.Muted = !active;
            }
        }

        private void HandleImpact(ImpactKind kind, float strength)
        {
            OnImpact?.Invoke(kind, strength);
        }

        private void StartReset()
        {
            if (!isReady || isResetting || isRolling) return;
            StartCoroutine(ResetRoutine());
        }

        private IEnumerator ResetRoutine()
        {
            isResetting = true;

            try
            {
                yield return new WaitForSeconds(resultHold);

                ClearGlow();

                if (!cubesAtStart)
                {
                    yield return ReturnCubes();
                    yield return LowerRack();
                }
            }
            finally
            {
                RestoreStartPose(); // always ends in a clean, identical physical state
                isResetting = false;
            }
        }

        // The cubes glide back up to the shelf while the rack is still raised.
        private IEnumerator ReturnCubes()
        {
            Vector3[] fromPos = new Vector3[Count];
            Quaternion[] fromRot = new Quaternion[Count];

            for (int i = 0; i < Count; i++)
            {
                Rigidbody rb = cubes[i].Body;
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
                fromPos[i] = cubes[i].transform.position;
                fromRot[i] = cubes[i].transform.rotation;
            }

            float t = 0f;
            float duration = Mathf.Max(0.05f, returnDuration);

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float e = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));

                for (int i = 0; i < Count; i++)
                {
                    Vector3 pos = Vector3.Lerp(fromPos[i], cubeStartPositions[i], e) +
                                  Vector3.up * (Mathf.Sin(e * Mathf.PI) * returnArcHeight);
                    cubes[i].transform.SetPositionAndRotation(pos, Quaternion.Slerp(fromRot[i], cubeStartRotations[i], e));
                }
                yield return null;
            }
        }

        private IEnumerator LowerRack()
        {
            Vector3 from = rackBody.transform.position;
            float t = 0f;
            float duration = Mathf.Max(0.05f, rackLowerDuration);
            OnRackMoved?.Invoke(false);

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                rackBody.transform.position = Vector3.Lerp(from, rackStartPosition, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t)));
                yield return null;
            }
        }

        // Switching the objects off and on throws away old contact data, so every roll starts from
        // exactly the same physical state.
        private void RestoreStartPose()
        {
            if (cubeStartPositions == null) return;

            if (rackBody != null) rackBody.gameObject.SetActive(false);
            for (int i = 0; i < Count; i++)
            {
                if (cubes[i] != null) cubes[i].gameObject.SetActive(false);
            }

            if (rackBody != null)
            {
                rackBody.transform.SetPositionAndRotation(rackStartPosition, rackStartRotation);
            }

            for (int i = 0; i < Count; i++)
            {
                if (cubes[i] != null) cubes[i].transform.SetPositionAndRotation(cubeStartPositions[i], cubeStartRotations[i]);
            }

            if (rackBody != null) rackBody.gameObject.SetActive(true);
            for (int i = 0; i < Count; i++)
            {
                if (cubes[i] == null) continue;

                cubes[i].gameObject.SetActive(true);
                Rigidbody rb = cubes[i].Body;
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Physics.SyncTransforms();
            cubesAtStart = true;
        }

        // ---------- Helpers ----------

        private float LocalY(Vector3 worldPosition)
        {
            return transform.InverseTransformPoint(worldPosition).y;
        }

        private bool IsInTray(Vector3 worldPosition)
        {
            return LocalY(worldPosition) <= settleBelowLocalY;
        }

        private static int TopFaceIndex(Quaternion rotation)
        {
            int best = 0;
            float bestUp = float.MinValue;

            for (int i = 0; i < FaceNormals.Length; i++)
            {
                float up = (rotation * FaceNormals[i]).y;
                if (up > bestUp)
                {
                    bestUp = up;
                    best = i;
                }
            }
            return best;
        }

        // ---------- Test tools (right-click the component header in Play mode) ----------

        [ContextMenu("TEST: Roll now (no wallet, no payout)")]
        private void TestRoll()
        {
            if (!Application.isPlaying || !isReady || IsBusy)
            {
                Debug.LogWarning("[Machine3DPresenter] Press Play first, and wait until the machine is idle.");
                return;
            }
            StartCoroutine(TestRollRoutine());
        }

        private IEnumerator TestRollRoutine()
        {
            yield return PlayPhysicalRoll(colors =>
                Debug.Log("[Machine3DPresenter] TEST cubes show: " + string.Join(" / ", colors)));
            yield return ApplyForcedResult();    

            yield return new WaitForSeconds(1.5f);
            if (!isResetting && !isRolling) StartCoroutine(ResetRoutine());
        }

        [ContextMenu("TEST: Instant 50 rolls (landing statistics, no visuals)")]
        private void TestInstantRolls()
        {
            if (!Application.isPlaying || !isReady || IsBusy)
            {
                Debug.LogWarning("[Machine3DPresenter] Press Play first, and wait until the machine is idle.");
                return;
            }

            const int runs = 50;
            int clean = 0;
            int[] colorCounts = new int[Enum.GetValues(typeof(GameColor)).Length];
            int guardSteps = Mathf.CeilToInt(maxSimTime / simulationStep) + 10;

            for (int r = 0; r < runs; r++)
            {
                RestoreStartPose();

                SimRun run = NewRun(Count);
                int guard = guardSteps;
                while (!run.done && guard-- > 0) StepSimulation(run);

                lastRunTimedOut = run.timedOut || !run.done;

                if (TryReadResult(out GameColor[] colors, out _))
                {
                    clean++;
                    foreach (GameColor c in colors) colorCounts[(int)c]++;
                }
            }

            RestoreStartPose();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"[Machine3DPresenter] {clean}/{runs} rolls landed cleanly (flat, in the tray, settled).");
            int total = Mathf.Max(1, clean * Count);
            for (int i = 0; i < colorCounts.Length; i++)
            {
                sb.AppendLine($"  {(GameColor)i}: {100f * colorCounts[i] / total:F1}%  (fair would be about 16.7%)");
            }
            Debug.Log(sb.ToString());
        }
    }
}
