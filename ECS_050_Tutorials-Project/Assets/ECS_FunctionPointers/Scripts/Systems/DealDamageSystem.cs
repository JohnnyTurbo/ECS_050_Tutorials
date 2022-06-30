using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.FunctionPointers
{
    public partial class DealDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            Entities.ForEach((Entity damagedEntity, in Damage damage, in Translation translation) =>
            {
                var uiPosition = translation.Value + math.up();
                DamageUIController.Instance.DisplayDamageUI(damage.Value, uiPosition);
                ecb.RemoveComponent<Damage>(damagedEntity);
            }).WithoutBurst().Run();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}