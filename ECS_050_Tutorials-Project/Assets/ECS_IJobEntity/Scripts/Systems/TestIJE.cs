using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.IJE
{
    public partial class TestIJESystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var testQuery = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<CoolTag>(), ComponentType.ReadOnly<MoveSpeed>());
            var testQDesc = new EntityQueryDesc
            {
                All = new []{ComponentType.ReadWrite<Translation>() },
                None = new[] { ComponentType.ReadOnly<CoolTag>() }
            };
            var testQuery2 = EntityManager.CreateEntityQuery(testQDesc);
            /*new TestIJE
            {
                DeltaTime = Time.DeltaTime,
            }.ScheduleParallel(testQuery);*/

            //new TestIJE().ScheduleParallel(testQuery2);

            var testIJE = new TestIJE { DeltaTime = Time.DeltaTime };
            testIJE.ScheduleParallel(testQuery);
            testIJE.DeltaTime = 0.25f;
            testIJE.ScheduleParallel();
        }
    }
    
    public partial struct TestIJE : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(Entity e, [EntityInQueryIndex]int sortKey, ref Translation translation, in MoveSpeed moveSpeed)
        {
            Debug.Log(e.Index + " " + sortKey);
            translation.Value += math.forward() * moveSpeed.Value * DeltaTime;
        }
    }
}