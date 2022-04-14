using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.IJE
{
    public partial class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var specialMoveQuery = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<MoveSpeed>(), ComponentType.ReadOnly<SpecialMoveTag>());
            var regularMoveDesc = new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<Translation>(), ComponentType.ReadOnly<MoveSpeed>() },
                None = new[] { ComponentType.ReadOnly<SpecialMoveTag>() }
            };

            var regularMoveQuery = EntityManager.CreateEntityQuery(regularMoveDesc);
            
            var moveJob = new MoveJob { DeltaTime = Time.DeltaTime, MoveModifier = 1f};
            moveJob.ScheduleParallel(regularMoveQuery);
            moveJob.MoveModifier = 0.5f;
            moveJob.ScheduleParallel(specialMoveQuery);
        }
    }
    
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        public float MoveModifier;
        
        public void Execute(ref Translation translation, in MoveSpeed moveSpeed)
        {
            translation.Value += math.forward() * moveSpeed.Value * MoveModifier * DeltaTime;
        }
    }
}