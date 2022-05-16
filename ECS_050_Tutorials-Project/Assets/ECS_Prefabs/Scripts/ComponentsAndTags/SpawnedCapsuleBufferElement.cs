using Unity.Entities;

namespace TMG.ECSPrefabs
{
    [InternalBufferCapacity(8)]
    public struct SpawnedCapsuleBufferElement : IBufferElementData
    {
        public Entity Value;
    }
}