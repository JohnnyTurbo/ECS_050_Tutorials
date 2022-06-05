using Unity.Entities;

namespace TMG.AnimationCurves
{
    public struct AccelerationCurveReference : IComponentData
    {
        public BlobAssetReference<SampledCurve> Value;
        public readonly float GetValueAtTime(float time) => Value.Value.GetValueAtTime(time);
    }
}