using Unity.Entities;
using Unity.Mathematics;

namespace TMG.AnimationCurves
{
    [GenerateAuthoringComponent]
    public struct AccelerationTimer : IComponentData
    {
        public float Value;
        public float Max;

        public float Normalized => Value / Max;

        public static AccelerationTimer operator +(AccelerationTimer accelerationTimer, float deltaTime) =>
            accelerationTimer.Increment(deltaTime);

        private AccelerationTimer Increment(float deltaTime)
        {
            Value = math.clamp(Value += deltaTime, 0f, Max);
            return this;
        }

        public void Reset() { Value = 0f; }
    }
}