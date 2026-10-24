
using UnityEngine;

namespace Shared.UI
{
    public enum OrientationMode
    {
        Portrait,
        Landscape
    }

    public class SceneOrientation : MonoBehaviour
    {
        [SerializeField]
        private OrientationMode mode = OrientationMode.Portrait;

        private void Awake()
        {
            Apply(mode);
        }

        private void OnApplicationPause(bool paused)
        {
            if (!paused)
            {
                Apply(mode);
            }
        }

        public static void Apply(OrientationMode target)
        {
            if (target == OrientationMode.Portrait)
            {
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;

                Screen.orientation = ScreenOrientation.Portrait;
            }
            else
            {
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;

                // Force landscape regardless of how the phone is held.
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
        }
    }
}
