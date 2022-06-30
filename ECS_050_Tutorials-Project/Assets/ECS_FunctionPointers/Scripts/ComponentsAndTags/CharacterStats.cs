using Unity.Entities;

namespace TMG.FunctionPointers
{
    [GenerateAuthoringComponent]
    public struct CharacterStats : IComponentData
    {
        public int AttackPoints;
        public float MoveSpeed;
    }
    
    public struct TotalAttackPoints : IComponentData
    {
        public int Value;
    }

    public struct TotalMoveSpeed : IComponentData
    {
        public float Value;
    }
}