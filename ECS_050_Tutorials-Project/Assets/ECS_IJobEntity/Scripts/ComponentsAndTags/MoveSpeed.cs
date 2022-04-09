using Unity.Entities;

namespace TMG.IJE
{
    [GenerateAuthoringComponent]
    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }
}