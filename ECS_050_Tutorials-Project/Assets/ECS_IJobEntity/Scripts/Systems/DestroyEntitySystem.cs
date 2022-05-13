using Unity.Entities;
using UnityEngine;

namespace TMG.IJE
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class DestroyEntitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<HitPoints, DestroyEntityTag>()
                .ForEach((Entity e, BattleEntityManaged battleEntityManaged) =>
                {
                    battleEntityManaged.CharacterImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                    EntityManager.DestroyEntity(e);
                }).WithStructuralChanges().WithoutBurst().Run();
        }
    }
}