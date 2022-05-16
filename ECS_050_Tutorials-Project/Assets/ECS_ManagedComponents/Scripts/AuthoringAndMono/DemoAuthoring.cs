using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ManagedComponents
{
    public class DemoAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Transform _transform;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<CopyTransformFromGameObject>(entity);
            dstManager.AddComponentObject(entity, _transform);
        }
    }
    
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class MoveTransformSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Transform managedTransform) =>
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    managedTransform.position = Vector3.up;
                }
            }).WithoutBurst().Run();
        }
    }

    public partial class DemoManagedSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            // Can't run GetSingleton on managed type
            var entityWithManagedData = GetSingletonEntity<DemoManagedTag>();
            var managedComponent = EntityManager.GetComponentData<DemoManaged>(entityWithManagedData);

            managedComponent.Message = ImportantMessage;
            managedComponent.Controller = DemoGameController.Instance;
        }

        private void ImportantMessage()
        {
            Debug.Log("Subscribe to Turbo Makes Games (and like this video).");
        }
        
        protected override void OnUpdate()
        {
            Entities.ForEach((DemoManaged managed) =>
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    managed.Message?.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    managed.Controller.IncrementScore(1,50);
                }
                
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    managed.Controller.IncrementScore(2,50);
                }
            }).WithoutBurst().Run();
        }
    }
}