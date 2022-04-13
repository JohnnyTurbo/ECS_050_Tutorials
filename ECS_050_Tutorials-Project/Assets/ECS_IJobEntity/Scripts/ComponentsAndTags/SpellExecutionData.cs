using Unity.Entities;

namespace TMG.IJE
{
    public struct SpellExecutionData : IComponentData
    {
        public int EffectAmount;
        public int TeamID;
    }
}