using Unity.Entities;

namespace TMG.IJE
{
    [GenerateAuthoringComponent]
    public struct HitPoints : IComponentData
    {
        public int Current;
        public int Max;
    }
}