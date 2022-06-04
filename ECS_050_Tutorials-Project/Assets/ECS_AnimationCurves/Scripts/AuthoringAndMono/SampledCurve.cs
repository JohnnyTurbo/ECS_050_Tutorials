using Unity.Entities;
using Unity.Mathematics;

namespace TMG.AnimationCurves
{
    public struct SampledCurve
    {
        public BlobArray<float> SampledPoints;
        public int NumberOfSamples;

        public float GetValueAtTime(float time)
        {
            var approxIndex = (NumberOfSamples - 1) * time;
            var indexBelow = (int)math.floor(approxIndex);
            if (indexBelow >= NumberOfSamples - 1)
            {
                return SampledPoints[NumberOfSamples - 1];
            }
            var indexRemainder = approxIndex - indexBelow;
            return math.lerp(SampledPoints[indexBelow], SampledPoints[indexBelow + 1], indexRemainder);
        }
    }
}