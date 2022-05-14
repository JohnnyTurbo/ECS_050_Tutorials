using Unity.Entities;

namespace TMG.ECSPrefabs
{
    [InternalBufferCapacity(8)]
    [GenerateAuthoringComponent]
    public struct CapsuleReferenceBufferElement : IBufferElementData
    {
        public Entity Value;

        public static implicit operator CapsuleReferenceBufferElement(Entity value)
        {
            return new CapsuleReferenceBufferElement { Value = value };
        }

        public static implicit operator Entity(CapsuleReferenceBufferElement element)
        {
            return element.Value;
        }
    }
}