using System;
using UnityEngine;

namespace ColorGame.Machine
{
    /// <summary>What a cube hit. Used to choose the sound.</summary>
    public enum ImpactKind
    {
        Cube,
        Floor,
        Wall
    }

    /// <summary>
    /// Reports REAL collisions of one cube (OnCollisionEnter only, never every frame).
    /// It measures how hard the hit was and raises an event. It plays no sound itself and changes no physics.
    /// Machine3DPresenter adds this component to the cubes at start and forwards the events to ColorGameAudio.
    /// Light touches below Min Impact Speed are ignored.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class CubeSoundEmitter : MonoBehaviour
    {
        [Tooltip("Hits slower than this (relative speed in m/s) make no sound.")]
        [SerializeField] private float minImpactSpeed = 0.8f;
        [Tooltip("Hits at or above this speed play at full strength.")]
        [SerializeField] private float maxImpactSpeed = 9f;
        [Tooltip("Minimum seconds between two sounds from this cube.")]
        [SerializeField] private float minInterval = 0.04f;
        [Tooltip("A surface whose normal points at least this much up or down counts as floor, otherwise wall.")]
        [SerializeField, Range(0.2f, 0.95f)] private float floorNormalY = 0.5f;

        /// <summary>Raised with what was hit and the impact strength from 0 to 1.</summary>
        public event Action<ImpactKind, float> OnImpact;

        /// <summary>Set by Machine3DPresenter. Collisions are silent unless a real visible roll is playing.</summary>
        public bool Muted { get; set; } = true;

        private float lastImpactTime = float.NegativeInfinity;

        private void OnValidate()
        {
            minImpactSpeed = Mathf.Max(0.05f, minImpactSpeed);
            maxImpactSpeed = Mathf.Max(minImpactSpeed + 0.1f, maxImpactSpeed);
            minInterval = Mathf.Max(0f, minInterval);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Muted) return;

            float speed = collision.relativeVelocity.magnitude;
            if (speed < minImpactSpeed) return;

            if (Time.unscaledTime - lastImpactTime < minInterval) return;

            ImpactKind kind;
            ColorCube otherCube = collision.collider.GetComponentInParent<ColorCube>();

            
if (otherCube != null && otherCube.gameObject != gameObject)
{
    // Both cubes receive this callback. Only one reports the collision.
    CubeSoundEmitter otherEmitter = otherCube.GetComponent<CubeSoundEmitter>();

    if (otherEmitter != null &&
        otherEmitter.GetEntityId().CompareTo(GetEntityId()) < 0)
    {
        return;
    }

    kind = ImpactKind.Cube;
}

            else
            {
                Vector3 normal = collision.GetContact(0).normal;
                kind = Mathf.Abs(normal.y) >= floorNormalY ? ImpactKind.Floor : ImpactKind.Wall;
            }

            lastImpactTime = Time.unscaledTime;

            // Harder hits are louder. Bounces lose speed, so they automatically become softer.
            float t = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, speed);
            float strength = Mathf.Lerp(0.08f, 1f, Mathf.Pow(t, 1.3f));
            OnImpact?.Invoke(kind, strength);
        }
    }
}
