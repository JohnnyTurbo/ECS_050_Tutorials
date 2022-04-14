using Unity.Entities;

namespace TMG.ECSDebug
{
    [DisableAutoCreation]
    public partial class LargeMemorySystem : SystemBase
    {
        protected override void OnCreate()
        {
            var debugArchetype = EntityManager.CreateArchetype(typeof(LargeComponent));
            EntityManager.CreateEntity(debugArchetype, 1000);
        }
        
        protected override void OnUpdate()
        {
            
        }
    }
    
    public struct LargeComponent : IComponentData
    {
        public long Value1;
        public long Value2;
        public long Value3;
        public long Value4;
        public long Value5;
        public long Value6;
        public long Value7;
        public long Value8;
        public long Value9;
        public long Value10;
        public long Value11;
        public long Value12;
        public long Value13;
        public long Value14;
        public long Value15;
        public long Value16;
        public long Value17;
        public long Value18;
        public long Value19;
        public long Value20;
    }
}