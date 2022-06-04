using Unity.Entities;

namespace TMG.AnimationCurves
{
    [GenerateAuthoringComponent]
    public struct BaseMoveSpeed : IComponentData
    {
        public float Value;
    }
}