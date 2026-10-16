using System.Collections;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Machine;

namespace ColorGame.DevTools
{
    /// <summary>
    /// TEMPORARY Step 1 tester. It never touches the wallet, bets, or the real round.
    /// In Play mode: click the three dots on this component and pick a command.
    /// </summary>
    public class Cabinet3DPreview : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorCube[] cubes;
        [SerializeField] private Rigidbody rackBody;

        [Header("Rack")]
        [SerializeField] private float rackTravel = 1.7f;
        [SerializeField] private float rackDuration = 0.8f;

        [Header("World physics (3D only)")]
        [SerializeField] private float gravityY = -20f;

        [Header("Settle detection")]
        [SerializeField] private float settleSpeed = 0.1f;
        [SerializeField] private float settleAngularSpeed = 0.3f;
        [SerializeField] private float settleBelowLocalY = 1.0f;
        [SerializeField] private float settleHold = 0.5f;
        [SerializeField] private float maxSimulationTime = 8f;

        [Header("Cycle")]
        [SerializeField] private float resultDisplayTime = 2f;

        private Vector3[] cubeStartPositions;
        private Quaternion[] cubeStartRotations;
        private Vector3 rackStartPosition;
        private bool busy;

        private void Awake()
        {
            Physics.gravity = new Vector3(0f, gravityY, 0f);

            if (cubes == null || cubes.Length == 0 || rackBody == null || config == null)
            {
                Debug.LogError("[Cabinet3DPreview] A reference is missing in the Inspector.", this);
                enabled = false;
                return;
            }

            cubeStartPositions = new Vector3[cubes.Length];
            cubeStartRotations = new Quaternion[cubes.Length];
            for (int i = 0; i < cubes.Length; i++)
            {
                cubeStartPositions[i] = cubes[i].transform.position;
                cubeStartRotations[i] = cubes[i].transform.rotation;
            }
            rackStartPosition = rackBody.position;
        }

        private void Start()
        {
            // Different face layouts so the cubes do not look identical.
            for (int i = 0; i < cubes.Length; i++) cubes[i].ApplyStandardLayout(config, i * 2);
        }

        [ContextMenu("1 - Raise rack (release the cubes)")]
        private void RaiseRack()
        {
            if (!busy) StartCoroutine(MoveRack(true));
        }

        [ContextMenu("2 - Lower rack and reset cubes")]
        private void LowerAndReset()
        {
            if (!busy) StartCoroutine(LowerAndResetRoutine());
        }

        [ContextMenu("3 - FULL CYCLE (release, settle, read faces, reset)")]
        private void FullCycle()
        {
            if (!busy) StartCoroutine(FullCycleRoutine());
        }

        [ContextMenu("4 - Print top face colors now")]
        private void PrintTopColors()
        {
            Debug.Log("[Preview] Top faces: " + TopColorsText());
        }

        private IEnumerator FullCycleRoutine()
        {
            busy = true;
            ResetCubes();
            yield return MoveRack(true);

            float elapsed = 0f;
            float stable = 0f;

            while (elapsed < maxSimulationTime && stable < settleHold)
            {
                yield return new WaitForFixedUpdate();
                elapsed += Time.fixedDeltaTime;
                stable = AllSettled() ? stable + Time.fixedDeltaTime : 0f;
            }

            bool timedOut = stable < settleHold;
            Debug.Log($"[Preview] {(timedOut ? "TIMEOUT" : "Settled")} after {elapsed:F1}s. Top faces: {TopColorsText()}");

            yield return new WaitForSeconds(resultDisplayTime);
            yield return LowerAndResetRoutine();
            busy = false;
        }

        private IEnumerator LowerAndResetRoutine()
        {
            busy = true;
            yield return MoveRack(false);
            ResetCubes();
            busy = false;
        }

        // The rack is a kinematic body moved in physics steps, so cubes react to it correctly.
        private IEnumerator MoveRack(bool up)
        {
            bool wasBusy = busy;
            busy = true;

            Vector3 from = rackBody.position;
            Vector3 to = up ? rackStartPosition + Vector3.up * rackTravel : rackStartPosition;
            float t = 0f;

            while (t < rackDuration)
            {
                yield return new WaitForFixedUpdate();
                t += Time.fixedDeltaTime;
                rackBody.MovePosition(Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / rackDuration)));
            }
            rackBody.MovePosition(to);

            busy = wasBusy;
        }

        private void ResetCubes()
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                Rigidbody rb = cubes[i].Body;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                cubes[i].transform.SetPositionAndRotation(cubeStartPositions[i], cubeStartRotations[i]);
                rb.position = cubeStartPositions[i];
                rb.rotation = cubeStartRotations[i];
                rb.WakeUp();
            }
            Physics.SyncTransforms();
        }

        private bool AllSettled()
        {
            foreach (ColorCube cube in cubes)
            {
                Rigidbody rb = cube.Body;
                bool slow = rb.linearVelocity.magnitude < settleSpeed && rb.angularVelocity.magnitude < settleAngularSpeed;
                bool onFloor = transform.InverseTransformPoint(cube.transform.position).y < settleBelowLocalY;
                if (!slow || !onFloor) return false;
            }
            return true;
        }

        private string TopColorsText()
        {
            string[] names = new string[cubes.Length];
            for (int i = 0; i < cubes.Length; i++) names[i] = cubes[i].GetTopColor().ToString();
            return string.Join(" / ", names);
        }
    }
}