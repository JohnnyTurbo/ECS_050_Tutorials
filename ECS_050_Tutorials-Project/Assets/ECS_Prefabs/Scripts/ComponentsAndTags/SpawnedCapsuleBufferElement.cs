using Unity.Entities;

namespace TMG.ECSPrefabs
{
    [InternalBufferCapacity(8)]
    [GenerateAuthoringComponent]
    public struct SpawnedCapsuleBufferElement : IBufferElementData
    {
        public Entity Value;
    }
}