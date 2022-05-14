using Unity.Entities;

namespace TMG.ECSPrefabs
{
    [GenerateAuthoringComponent]
    public struct CapsulePrefab : IComponentData
    {
        public Entity Value;
    }
}