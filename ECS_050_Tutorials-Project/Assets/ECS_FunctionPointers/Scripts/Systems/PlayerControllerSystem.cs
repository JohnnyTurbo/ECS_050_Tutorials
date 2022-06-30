using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.FunctionPointers
{
    public partial class PlayerControllerSystem : SystemBase
    {
        private Entity _enemy;

        protected override void OnStartRunning()
        {
            _enemy = GetSingletonEntity<EnemyTag>();
        }

        protected override void OnUpdate()
        {
            var directionalInput = Input.GetAxisRaw("Horizontal");
            var attackInput = Input.GetKeyDown(KeyCode.Space);
            var deltaTime = Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            var enemy = _enemy;
            
            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Translation translation, in TotalMoveSpeed moveSpeed,
                    in TotalAttackPoints attackPoints) =>
                {
                    translation.Value.x += directionalInput * moveSpeed.Value * deltaTime;

                    if (attackInput)
                    {
                        ecb.AddComponent(enemy, new Damage{Value = attackPoints.Value});
                    }
                }).Run();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}