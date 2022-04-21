using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace TMG.IJE
{
    [BurstCompile]
    public partial struct PerformSpellJob : IJobEntity
    {
        public int EffectAmount;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        public void Execute(Entity e, [EntityInQueryIndex] int sortKey, ref HitPoints hitPoints)
        {
            var curHitPoints = hitPoints.Current;
            curHitPoints -= EffectAmount;
            curHitPoints = math.clamp(curHitPoints, 0, hitPoints.Max);
            if (curHitPoints <= 0)
            {
                ECB.AddComponent<DestroyEntityTag>(sortKey, e);
            }

            hitPoints.Current = curHitPoints;
        }
    }
}