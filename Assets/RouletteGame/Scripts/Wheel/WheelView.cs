using System;
using System.Collections;
using TMPro;
using UnityEngine;
using RouletteGame.Core;

namespace RouletteGame.Wheel
{
    /// <summary>
    /// The wheel and ball on screen. DISPLAY ONLY: it is told the winning number and plays the matching spin.
    /// All the maths is in WheelTrajectory (pure and tested), so the ball always ends in the pocket of that number.
    ///
    /// Setup: Wheel and Ball are siblings under the same parent, both with centered anchors, Wheel at the middle.
    /// The wheel picture must have the zero pocket at the top and the pockets clockwise in European order
    /// (Tools > Roulette Game > Generate Wheel Sprites makes such a picture).
    /// </summary>
    public class WheelView : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The wheel picture. It rotates.")]
        [SerializeField] private RectTransform wheel;
        [Tooltip("The ball. Same parent as the wheel. It is moved on a circle, never parented to the wheel.")]
        [SerializeField] private RectTransform ball;

        [Header("Ball path (fraction of the wheel's half width)")]
        [SerializeField, Range(0.3f, 1f)] private float orbitRadius = 0.93f;
        [SerializeField, Range(0.3f, 1f)] private float pocketRadius = 0.76f;

        [Header("Spin")]
        [SerializeField] private WheelSpinSettings spin = new WheelSpinSettings();
        [Tooltip("Degrees per second the wheel turns while waiting for the next round. 0 = still.")]
        [SerializeField] private float idleSpeed = 8f;

        [Header("Number labels (created when the scene starts)")]
        [SerializeField] private bool createNumberLabels = true;
        [SerializeField] private TMP_FontAsset labelFont;
        [SerializeField, Range(0.3f, 1f)] private float labelRadius = 0.80f;
        [SerializeField] private float labelSize = 34f;

        /// <summary>Uses the animation numbers of the settings asset instead of the ones on this component.</summary>
        public void Configure(WheelSpinSettings settings)
        {
            if (settings != null) spin = settings.Sanitized();
        }

        /// <summary>The spin begins.</summary>
        public event Action OnSpinStarted;

        /// <summary>The ball leaves the rim and drops toward the pockets (play the landing sound here).</summary>
        public event Action OnBallDropped;

        /// <summary>Everything has stopped. Carries the number the ball rests on.</summary>
        public event Action<int> OnSpinFinished;

        public bool IsSpinning { get; private set; }

        /// <summary>The last number the ball landed on, or -1 before the first spin.</summary>
        public int LastNumber { get; private set; } = -1;

        /// <summary>1 = normal. Raise it to fast-forward a running spin.</summary>
        public float Speed { get; set; } = 1f;

        /// <summary>How long a spin takes at normal speed (the controller can wait this long).</summary>
        public float SpinSeconds => spin.Sanitized().TotalDuration;

        private WheelTrajectory current;
        private double wheelAngle;
        private bool isReady;

        private void Awake()
        {
            if (wheel == null || ball == null)
            {
                Debug.LogError("[WheelView] Wheel or Ball is not assigned in the Inspector.", this);
                return;
            }

            if (spin == null) spin = new WheelSpinSettings();
            BuildLabels();
            isReady = true;
            ApplyResting();
        }

        private void OnValidate()
        {
            if (spin == null) return;
            foreach (string problem in spin.GetProblems())
            {
                Debug.LogWarning("[WheelView] " + problem + " A safe value is used while playing.", this);
            }
        }

        private void OnDisable()
        {
            // Leaving mid-spin: jump to the finished pose so the wheel never stays half-way.
            if (!isReady || !IsSpinning) return;
            WheelState end = current.Evaluate(current.Duration);
            Apply(end);
            wheelAngle = WheelTrajectory.Normalize360(end.WheelAngle);
            IsSpinning = false;
        }

        private void Update()
        {
            if (!isReady || IsSpinning || Mathf.Approximately(idleSpeed, 0f)) return;

            wheelAngle = WheelTrajectory.Normalize360(wheelAngle + idleSpeed * Time.deltaTime);
            ApplyResting();
        }

        /// <summary>Plays a whole spin that ends on 'number'. Wait for it with yield return, or listen to OnSpinFinished.</summary>
        public IEnumerator Spin(int number)
        {
            if (!isReady) { Debug.LogError("[WheelView] Cannot spin: the view is not set up correctly.", this); yield break; }
            if (IsSpinning) { Debug.LogWarning("[WheelView] A spin is already running.", this); yield break; }
            if (!RouletteLayout.IsValidNumber(number)) { Debug.LogError("[WheelView] " + number + " is not a roulette number.", this); yield break; }

            IsSpinning = true;
            LastNumber = number;
            current = WheelTrajectory.Create(number, spin, UnityEngine.Random.Range(0f, 360f), wheelAngle);

            ball.gameObject.SetActive(true);
            OnSpinStarted?.Invoke();

            double time = 0.0;
            bool dropped = false;

            while (time < current.Duration)
            {
                time += Time.deltaTime * Mathf.Max(0.1f, Speed);
                WheelState state = current.Evaluate(time);
                Apply(state);
                wheelAngle = state.WheelAngle;

                if (!dropped && time >= current.DropTime)
                {
                    dropped = true;
                    OnBallDropped?.Invoke();
                }
                yield return null;
            }

            WheelState end = current.Evaluate(current.Duration);
            Apply(end);
            wheelAngle = WheelTrajectory.Normalize360(end.WheelAngle);
            IsSpinning = false;

            if (!dropped) OnBallDropped?.Invoke();
            OnSpinFinished?.Invoke(number);
        }

        /// <summary>Puts the ball in the pocket of a number at once (for example the last result when the scene opens).</summary>
        public void SetRestingNumber(int number)
        {
            if (!isReady || IsSpinning) return;
            LastNumber = RouletteLayout.IsValidNumber(number) ? number : -1;
            ApplyResting();
        }

        // ---------- Drawing ----------

        private void ApplyResting()
        {
            wheel.localRotation = Quaternion.Euler(0f, 0f, (float)-wheelAngle);

            if (LastNumber < 0)
            {
                ball.gameObject.SetActive(false);
                return;
            }

            ball.gameObject.SetActive(true);
            PlaceBall(wheelAngle + RouletteLayout.PocketAngleDegrees(LastNumber), 1.0);
        }

        private void Apply(WheelState state)
        {
            wheel.localRotation = Quaternion.Euler(0f, 0f, (float)-state.WheelAngle);
            PlaceBall(state.BallAngle, state.DropProgress);
        }

        // Angle 0 is the top, increasing clockwise.
        private void PlaceBall(double angleDegrees, double dropProgress)
        {
            float half = wheel.rect.width * 0.5f;
            float radius = half * Mathf.Lerp(orbitRadius, pocketRadius, (float)dropProgress);
            double radians = angleDegrees * Math.PI / 180.0;
            Vector2 offset = new Vector2((float)Math.Sin(radians), (float)Math.Cos(radians)) * radius;
            ball.anchoredPosition = wheel.anchoredPosition + offset;
        }

        // The 37 numbers are children of the wheel, so they turn with it. Positions use anchors (fractions of the
        // wheel size), so they are correct at any resolution.
        private void BuildLabels()
        {
            if (!createNumberLabels || wheel.Find("NumberLabels") != null) return;

            GameObject root = new GameObject("NumberLabels", typeof(RectTransform));
            root.transform.SetParent(wheel, false);
            RectTransform rootRect = (RectTransform)root.transform;
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            for (int i = 0; i < RouletteLayout.PocketCount; i++)
            {
                int number = RouletteLayout.NumberAt(i);
                double degrees = RouletteLayout.PocketAngleDegrees(number);
                float radians = (float)(degrees * Math.PI / 180.0);

                GameObject go = new GameObject("Pocket_" + number, typeof(RectTransform), typeof(TextMeshProUGUI));
                go.transform.SetParent(root.transform, false);

                RectTransform rect = (RectTransform)go.transform;
                Vector2 anchor = new Vector2(0.5f + 0.5f * labelRadius * Mathf.Sin(radians), 0.5f + 0.5f * labelRadius * Mathf.Cos(radians));
                rect.anchorMin = anchor;
                rect.anchorMax = anchor;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(labelSize * 2f, labelSize * 1.2f);
                rect.localRotation = Quaternion.Euler(0f, 0f, (float)-degrees); // the top of the number points outward

                TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
                if (labelFont != null) text.font = labelFont;
                text.text = number.ToString();
                text.fontSize = labelSize;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Center;
                text.color = Color.white;
                text.raycastTarget = false;
            }
        }
    }
}
