using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace BenScr.UnityStack
{
    /*
     * Used for handeling post processing effects globally, so that they can be easily called from anywhere without needing to reference the volume or profile directly
     */

    public class PostProcessingHandler : MonoBehaviour
    {
        [SerializeField] private VolumeProfile volumeProfile;

        public static PostProcessingHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static void ActivateBlur(bool active)
        {
            if (instance?.volumeProfile.TryGet(out DepthOfField dop) ?? false)
            {
                dop.active = active;
            }
        }
        public static void SetVignette(float intensity, float smoothness)
        {
            if (instance?.volumeProfile.TryGet(out Vignette vignette) ?? false)
            {
                vignette.intensity.Override(intensity);
                vignette.smoothness.Override(smoothness);
            }
        }

        public static void SetBloom(float intensity, float threshold)
        {
            if (instance?.volumeProfile.TryGet(out Bloom bloom) ?? false)
            {
                bloom.intensity.Override(intensity);
                bloom.threshold.Override(threshold);
            }
        }
    }
}