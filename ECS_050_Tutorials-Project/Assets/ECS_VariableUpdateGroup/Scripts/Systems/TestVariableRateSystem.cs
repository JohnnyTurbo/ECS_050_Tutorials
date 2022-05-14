using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.VariableUpdateGroup
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(VariableRateSimulationSystemGroup))]
    public partial class TestVariableRateSystem : SystemBase
    {
        private int _tick = 0;

        protected override void OnStartRunning()
        {
            
            
            IRateManager rateManager = new RateUtils.VariableRateManager(500);
            
            
            var variableRateSystem = World.GetExistingSystem<VariableRateSimulationSystemGroup>();
            variableRateSystem.RateManager = rateManager;
        }

        protected override void OnUpdate()
        {
            Debug.Log($"Tick: {_tick} - Time: {Time.ElapsedTime} - Frame: {UnityEngine.Time.frameCount}");
            Debug.Log($"DeltaTime: {Time.DeltaTime}");
            _tick++;
        }
    }

    public class TurboUpdateGroup : ComponentSystemGroup
    {
        public TurboUpdateGroup()
        {
            RateManager = new RateUtils.VariableRateManager(5000, true);
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(TurboUpdateGroup))]
    public partial class ExpensiveJobScheduler : SystemBase
    {
        private int _pointCount = 2500;
        private Random _random;
        private float3 _minPos = new float3(-100, -100, -100);
        private float3 _maxPos = new float3(100, 100, 100);

        protected override void OnStartRunning()
        {
            _random.InitState((uint) System.DateTime.Now.Millisecond); 
        }

        protected override void OnUpdate()
        {
            var pointArray = new NativeArray<float3>(_pointCount, Allocator.TempJob);
            
            for (var i = 0; i < _pointCount; i++)
            {
                pointArray[i] = _random.NextFloat3(_minPos, _maxPos);
            }

            var expensiveJob = new ExpensiveJob
            {
                PointArray = pointArray
            };

            var expensiveDependency = expensiveJob.Schedule();

            pointArray.Dispose(expensiveDependency);

            new PostExpensiveJobJob().Schedule(expensiveDependency);
        }
    }

    [BurstCompile]
    public struct ExpensiveJob : IJob
    {
        public NativeArray<float3> PointArray;
        
        public void Execute()
        {
            Debug.Log($"Begin calculating distances");
            var pointCount = PointArray.Length;
            for (var i = 0; i < pointCount; i++)
            {
                for (var j = 0; j < pointCount; j++)
                {
                    var dist = math.distance(PointArray[i], PointArray[j]);
                }
            }
        }
    }

    public struct PostExpensiveJobJob : IJob
    {
        public void Execute()
        {
            Debug.Log("End calculating distance");
        }
    }

    public struct CurPoint : IComponentData
    {
        public float3 Value;
    }

    [DisableAutoCreation]
    //[AlwaysUpdateSystem]
    [UpdateInGroup(typeof(TurboUpdateGroup))]
    public partial class ExpensiveForEach : SystemBase
    {
        private int _pointCount = 1000;
        private Random _random;
        private float3 _minPos = new float3(-100, -100, -100);
        private float3 _maxPos = new float3(100, 100, 100);
        private EntityQuery _pointEntities;

        protected override void OnCreate()
        {
            JobsUtility.JobWorkerCount = 2;
        }

        protected override void OnStartRunning()
        {
            _random.InitState((uint) System.DateTime.Now.Millisecond); 
            var pointArch = EntityManager.CreateArchetype(typeof(CurPoint));
            using var ents = EntityManager.CreateEntity(pointArch, _pointCount, Allocator.TempJob);
            _pointEntities = GetEntityQuery(typeof(CurPoint));

            for (var i = 0; i < _pointCount; i++)
            {
                var newRand = new CurPoint { Value = _random.NextFloat3(_minPos, _maxPos) };
                EntityManager.SetComponentData(ents[i], newRand);
            }
        }

        protected override void OnUpdate()
        {
            Debug.Log("Scheduling EFE");
            var points = _pointEntities.ToEntityArray(Allocator.TempJob);
            var pointLookup = GetComponentDataFromEntity<CurPoint>();
            
            Entities
                .WithName("ExpensiveForEachJob")
                .WithNativeDisableContainerSafetyRestriction(pointLookup)
                .WithDisposeOnCompletion(points)
                .ForEach((in CurPoint myPoint) =>
            {
                foreach (var pointEntity in points)
                {
                    math.distance(myPoint.Value, pointLookup[pointEntity].Value);
                }
            }).Schedule();
        }
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    //[UpdateInGroup(typeof(TurboUpdateGroup))]
    //[UpdateAfter(typeof(ExpensiveForEach))]
    public partial class ExpensiveDependency : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref CurPoint point) =>
            {
                point.Value += 1;
            }).Schedule();
        }
    }
}












