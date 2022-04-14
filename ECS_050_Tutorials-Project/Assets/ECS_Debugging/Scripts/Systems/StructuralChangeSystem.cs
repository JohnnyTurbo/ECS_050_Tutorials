using Unity.Collections;
using Unity.Entities;

namespace TMG.ECSDebug
{
    [DisableAutoCreation]
    public partial class StructuralChangeSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnCreate()
        {
            var debugArchetype = EntityManager.CreateArchetype(typeof(TestTag));
            EntityManager.CreateEntity(debugArchetype, 10000);
        }

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities.WithAll<TestTag,BestTag>().ForEach((Entity e, int entityInQueryIndex) =>
            {
                ecb.RemoveComponent<BestTag>(entityInQueryIndex, e);
            }).ScheduleParallel();
            
            Entities.WithAll<TestTag>().WithNone<BestTag>().ForEach((Entity e, int entityInQueryIndex) =>
            {
                ecb.AddComponent<BestTag>(entityInQueryIndex, e);
            }).ScheduleParallel();
            
            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }
    
    public struct TestTag : IComponentData{}
    public struct BestTag : IComponentData{}
}