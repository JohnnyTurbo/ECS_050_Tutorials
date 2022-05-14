using Unity.Entities;

namespace TMG.ECSPrefabs
{
    [GenerateAuthoringComponent]
    public struct LastSpawnedCapsule : IComponentData
    {
        public Entity Value;
    }
}