using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.IJE
{
    public partial class MovementSystem : SystemBase
    {
        private EntityQuery _regularMoveQuery;
        private EntityQuery _specialMoveQuery;
        private EntityQuery _invalidQuery;
        
        protected override void OnStartRunning()
        {
            _regularMoveQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadWrite<Translation>(), 
                ComponentType.ReadOnly<MoveSpeed>(), 
                ComponentType.Exclude<SpecialMoveTag>());
            
            _specialMoveQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadWrite<Translation>(), 
                ComponentType.ReadOnly<MoveSpeed>(), 
                ComponentType.ReadOnly<SpecialMoveTag>());
            
            _invalidQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadOnly<SpecialMoveTag>(), 
                ComponentType.Exclude<MoveSpeed>());
        }
        
        protected override void OnUpdate()
        {
            var newMoveJob = new MoveJob
            {
                DeltaTime = Time.DeltaTime,
                MoveMod = 1f
            };
            
            newMoveJob.ScheduleParallel(_regularMoveQuery);

            newMoveJob.MoveMod = 0.25f;
            
            newMoveJob.ScheduleParallel(_specialMoveQuery);

            newMoveJob.ScheduleParallel(_invalidQuery);
        }
    }
    
    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        public float MoveMod;

        void Execute(ref Translation translation, in MoveSpeed moveSpeed)
        {
            translation.Value += math.forward() * moveSpeed.Value * DeltaTime * MoveMod;
        }
    }
}















