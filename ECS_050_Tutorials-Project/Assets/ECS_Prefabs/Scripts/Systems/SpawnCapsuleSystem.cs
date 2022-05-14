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
        private BeginSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnStartRunning()
        {
            Application.targetFrameRate = 30;
            _capsulePrefab = GetSingleton<CapsulePrefab>().Value;
            _random.InitState(454);
            _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            
            _capsuleSpawner = GetSingletonEntity<LastSpawnedCapsule>();
            EntityManager.AddBuffer<CapsuleReferenceBufferElement>(_capsuleSpawner);
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();

            if (Input.GetKeyDown(KeyCode.A))
            {
                var newCap = EntityManager.Instantiate(_capsulePrefab);
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                //var newPos2 = new LocalToWorld { Value = new float4x4(quaternion.identity, randPos) };
                EntityManager.SetComponentData(newCap, newPos);
                //EntityManager.SetComponentData(newCap, newPos2);
                
                SetSingleton(new LastSpawnedCapsule{Value = newCap});
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var newCap = ecb.Instantiate(_capsulePrefab);
                var randPos = _random.NextFloat3(_minPos, _maxPos);
                var newPos = new Translation { Value = randPos };
                ecb.SetComponent(newCap, newPos);
                ecb.AppendToBuffer(_capsuleSpawner, new CapsuleReferenceBufferElement{Value = newCap});
                //SetSingleton(new LastSpawnedCapsule{Value = newCap});
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
    
    public partial class BufferCleanup : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, DynamicBuffer<CapsuleReferenceBufferElement> capBuf, ref LastSpawnedCapsule lastCap) =>
            {
                if(capBuf.IsEmpty){ return; }
                lastCap.Value = capBuf[capBuf.Length - 1].Value;
                capBuf.Clear();
            }).Run();
        }
    }
}