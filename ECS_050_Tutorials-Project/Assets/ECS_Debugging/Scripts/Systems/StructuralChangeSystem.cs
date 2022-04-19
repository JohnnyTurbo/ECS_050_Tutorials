using Unity.Collections;
using Unity.Entities;
using UnityEngine;

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

            Entities.WithAll<TestTag,TestData>().ForEach((Entity e, int entityInQueryIndex) =>
            {
                ecb.RemoveComponent<TestData>(entityInQueryIndex, e);
            }).ScheduleParallel();

            Entities.WithAll<TestTag>().WithNone<TestData>().ForEach((Entity e, int entityInQueryIndex) =>
            {
                ecb.AddComponent<TestData>(entityInQueryIndex, e);
            }).ScheduleParallel();
            
            _ecbSystem.AddJobHandleForProducer(Dependency);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Breakpoint!");
            }
        }
    }

    public struct TestTag : IComponentData {}

    public struct TestData : IComponentData
    {
        public int Value;
    }
}