using System;
using Unity.Mathematics;

namespace BenScr.UnityStack
{
    [Serializable]
    public struct Range
    {
        public float Min;
        public float Max;

        public float Clamped(float value) => math.clamp(value, Min, Max);
    }
}
