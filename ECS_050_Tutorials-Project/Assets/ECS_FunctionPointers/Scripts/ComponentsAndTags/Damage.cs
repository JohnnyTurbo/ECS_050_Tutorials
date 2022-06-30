using Unity.Entities;

namespace TMG.FunctionPointers
{
    [GenerateAuthoringComponent]
    public struct Damage : IComponentData
    {
        public int Value;
    }
}