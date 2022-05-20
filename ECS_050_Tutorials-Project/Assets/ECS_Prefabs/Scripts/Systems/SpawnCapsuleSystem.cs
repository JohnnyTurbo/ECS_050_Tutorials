using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECSPrefabs
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class SpawnCapsuleSystem : SystemBase
    {
        private Entity _capsulePrefab;
        private Entity _capsuleSpawner;
        private Random _random;
        private float3 _minPos = float3.zero;
        private float3 _maxPos = new float3(50, 0, 50);
        private BeginSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            Application.targetFrameRate = 30;
            _capsulePrefab = GetSingleton<CapsulePrefab>().Value;
            
            _random.InitState(4554);
            _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            
            _capsuleSpawner = GetSingletonEntity<LastSpawnedCapsule>();
            EntityManager.AddBuffer<SpawnedCapsuleBufferElement>(_capsuleSpawner);
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var newCapsule = EntityManager.Instantiate(_capsulePrefab);
                
                SetSingleton(new LastSpawnedCapsule{Value = newCapsule});
                
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                EntityManager.SetComponentData(newCapsule, newPos);
                Debug.Break();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var ecb = _ecbSystem.CreateCommandBuffer();
                
                var newCapsule = ecb.Instantiate(_capsulePrefab);
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                ecb.SetComponent(newCapsule, newPos);
                
                ecb.AppendToBuffer(_capsuleSpawner, new SpawnedCapsuleBufferElement{Value = newCapsule});
                //Debug.Break();
            }
            
            Entities.ForEach((DynamicBuffer<SpawnedCapsuleBufferElement> capBuf, ref LastSpawnedCapsule lastCap) =>
            {
                if(capBuf.IsEmpty){ return; }
                lastCap.Value = capBuf[capBuf.Length - 1].Value;
                capBuf.Clear();
            }).Run();
            
            var lastSpawned = GetSingleton<LastSpawnedCapsule>().Value;
            if (lastSpawned != Entity.Null)
            {
                var lastPos = GetComponent<Translation>(lastSpawned);
                var upPos = new float3(25, 25, 25);
                Debug.DrawLine(lastPos.Value, upPos, Color.red);
            }
        }
    }
}























