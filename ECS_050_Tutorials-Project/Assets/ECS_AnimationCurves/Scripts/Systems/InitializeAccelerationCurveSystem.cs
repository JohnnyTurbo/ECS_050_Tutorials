using Unity.Collections;
using Unity.Entities;

namespace TMG.AnimationCurves
{
    public partial class InitializeAccelerationCurveSystem : SystemBase
    {
        private Entity _gameControllerEntity;

        protected override void OnStartRunning()
        {
            _gameControllerEntity = GetSingletonEntity<GameControllerTag>();
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var gameControllerEntity = _gameControllerEntity;
            Entities
                .WithAll<AccelerationTimer>()
                .WithNone<AccelerationCurveReference>()
                .ForEach((Entity e) =>
                {
                    var accelerationCurveReference = GetComponent<AccelerationCurveReference>(gameControllerEntity);
                    ecb.AddComponent<AccelerationCurveReference>(e);
                    ecb.SetComponent(e, accelerationCurveReference);
                }).Run();
            
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}