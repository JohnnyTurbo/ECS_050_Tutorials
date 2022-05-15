using Unity.Entities;

namespace TMG.ManagedComponents
{
    [GenerateAuthoringComponent]
    public class TestManaged : IComponentData
    {
        public delegate void TheBest();

        public TheBest Value;
        public TestGameController Controller;
    }
}