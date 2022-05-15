using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECSPrefabs
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public partial class SpawnCapsuleSystem : SystemBase
    {
        private Entity _capsulePrefab;
        private Entity _capsuleSpawner;
        private Random _random;
        private float3 _minPos = float3.zero;
        private float3 _maxPos = new float3(50, 0, 50);
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            Application.targetFrameRate = 30;
            _capsulePrefab = GetSingleton<CapsulePrefab>().Value;
            
            _random.InitState(454);
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            
            _capsuleSpawner = GetSingletonEntity<LastSpawnedCapsule>();
            EntityManager.AddBuffer<SpawnedCapsuleBufferElement>(_capsuleSpawner);
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();

            if (Input.GetKeyDown(KeyCode.A))
            {
                var newCapsule = EntityManager.Instantiate(_capsulePrefab);
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                EntityManager.SetComponentData(newCapsule, newPos);
                
                var localToWorld = new LocalToWorld { Value = new float4x4(quaternion.identity, randPos) };
                EntityManager.SetComponentData(newCapsule, localToWorld);
                
                SetSingleton(new LastSpawnedCapsule{Value = newCapsule});
                Debug.Break();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var newCapsule = ecb.Instantiate(_capsulePrefab);
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                var localToWorld = new LocalToWorld { Value = new float4x4(quaternion.identity, randPos) };
                ecb.SetComponent(newCapsule, newPos);
                ecb.SetComponent(newCapsule, localToWorld);
                ecb.AppendToBuffer(_capsuleSpawner, new SpawnedCapsuleBufferElement{Value = newCapsule});
                //SetSingleton(new LastSpawnedCapsule{Value = newCap});
                Debug.Break();
            }

            var lastSpawned = GetSingleton<LastSpawnedCapsule>().Value;
            if (lastSpawned != Entity.Null)
            {
                var lastPos = GetComponent<Translation>(lastSpawned);
                var upPos = new float3(25, 25, 25);
                Debug.DrawLine(lastPos.Value, upPos, Color.red);
            }
        }
    }
    
    public partial class SetLastCapsuleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, DynamicBuffer<SpawnedCapsuleBufferElement> capBuf, ref LastSpawnedCapsule lastCap) =>
            {
                if(capBuf.IsEmpty){ return; }
                lastCap.Value = capBuf[capBuf.Length - 1].Value;
                capBuf.Clear();
            }).Run();
        }
    }
}























