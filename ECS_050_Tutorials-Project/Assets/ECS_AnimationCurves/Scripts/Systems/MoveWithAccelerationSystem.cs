using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.AnimationCurves
{
    public partial class MoveWithAccelerationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((ref Translation translation, ref AccelerationTimer accelerationTimer, 
                in AccelerationCurveReference accelerationCurve, in BaseMoveSpeed baseMoveSpeed) =>
            {
                if (Input.GetKey(KeyCode.W))
                {
                    accelerationTimer += deltaTime;
                    var accelerationModifier = accelerationCurve.GetValueAtTime(accelerationTimer.Normalized);
                    
                    translation.Value += math.forward() * baseMoveSpeed.Value * accelerationModifier * deltaTime;
                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    accelerationTimer.Reset();
                }
            }).Run();
        }
    }
}