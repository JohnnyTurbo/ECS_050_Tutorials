using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TMG.AnimationCurves
{
    public class AccelerationCurveAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private int _numberOfSamples;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var sampledCurve = ref blobBuilder.ConstructRoot<SampledCurve>();
            var sampledCurveArray = blobBuilder.Allocate(ref sampledCurve.SampledPoints, _numberOfSamples);
            sampledCurve.NumberOfSamples = _numberOfSamples;
            
            for (var i = 0; i < _numberOfSamples; i++)
            {
                var samplePoint = (float)i / (_numberOfSamples - 1);
                var sampleValue = _accelerationCurve.Evaluate(samplePoint);
                sampledCurveArray[i] = sampleValue;
            }

            var blobAssetReference = blobBuilder.CreateBlobAssetReference<SampledCurve>(Allocator.Persistent);

            var accelerationCurveReference = new AccelerationCurveReference { Value = blobAssetReference };
            dstManager.AddComponentData(entity, accelerationCurveReference);
        }
    }
}