using Unity.Collections;
using Unity.Entities;

namespace TMG.FunctionPointers
{
    public partial class InitializePlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Enabled = false;
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            Entities
                .WithAll<PlayerTag>()
                .ForEach((Entity playerEntity, in CharacterStats baseStats) =>
                {
                    ecb.AddComponent(playerEntity, new TotalMoveSpeed { Value = baseStats.MoveSpeed });
                    ecb.AddComponent(playerEntity, new TotalAttackPoints { Value = baseStats.AttackPoints });
                }).Run();
            
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}