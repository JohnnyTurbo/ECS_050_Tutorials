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
            var approxSampleIndex = (NumberOfSamples - 1) * time;
            var sampleIndexBelow = (int)math.floor(approxSampleIndex);
            if (sampleIndexBelow >= NumberOfSamples - 1)
            {
                return SampledPoints[NumberOfSamples - 1];
            }
            var indexRemainder = approxSampleIndex - sampleIndexBelow;
            return math.lerp(SampledPoints[sampleIndexBelow], SampledPoints[sampleIndexBelow + 1], indexRemainder);
        }
    }
}