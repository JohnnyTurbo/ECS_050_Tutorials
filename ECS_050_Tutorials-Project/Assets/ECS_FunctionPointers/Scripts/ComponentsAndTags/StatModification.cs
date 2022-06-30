using Unity.Entities;

namespace TMG.FunctionPointers
{
    [GenerateAuthoringComponent]
    public struct StatModification : IComponentData
    {
        public StatTypes StatToModify;
        public StatModificationTypes ModificationType;
        public float ModificationValue;
        public float Timer;

        public static StatModification Empty => new StatModification
        {
            StatToModify = StatTypes.None,
            ModificationType = StatModificationTypes.None,
            ModificationValue = 0f,
            Timer = 0f
        };
    }
    
    public enum StatTypes
    {
        AttackPoints,
        MoveSpeed,
        None
    }
    
    public enum StatModificationTypes
    {
        Percentage,
        Numerical,
        Absolute,
        None
    }
    
    public struct ModifyStatsTag : IComponentData{}
}