using Unity.Entities;

namespace TMG.ManagedComponents
{
    [GenerateAuthoringComponent]
    public class DemoManaged : IComponentData
    {
        public delegate void PrintMessage();

        public PrintMessage Message;
        public DemoGameController Controller;
    }
}