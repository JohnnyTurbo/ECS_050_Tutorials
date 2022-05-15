using Unity.Entities;
using UnityEngine;

namespace TMG.ManagedComponents
{
    public class TestAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, _rigidbody);
        }
    }
    
    public partial class TestPhysicsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, Rigidbody rig) =>
            {
                rig.AddForce(Vector3.up * 20, ForceMode.Impulse);
            }).WithoutBurst().Run();
            Enabled = false;
        }
    }

    public partial class TestManagedSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            var manEnt = GetSingletonEntity<TestManagedTag>();
            //var manComp = EntityManager.GetComponentObject<TestManaged>(manEnt);
            var manComp = EntityManager.GetComponentData<TestManaged>(manEnt);

            manComp.Value = WhoIsBest;
            manComp.Controller = TestGameController.Instance;
            manComp.Value?.Invoke();
        }

        private void WhoIsBest()
        {
            Debug.Log("I AM THE BEST!!");
        }
        
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity e, TestManaged managed) =>
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    managed.Value?.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    //managed.
                }
            }).WithoutBurst().Run();
        }
    }
}