using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TMG.FunctionPointers
{
    public partial class AddModificationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var curModification = StatModification.Empty;

            #region CreatStatModRegion

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.MoveSpeed,
                    ModificationType = StatModificationTypes.Numerical,
                    ModificationValue = 5f,
                    Timer = 5f
                };
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.MoveSpeed,
                    ModificationType = StatModificationTypes.Percentage,
                    ModificationValue = 50f,
                    Timer = 3f
                };
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.MoveSpeed,
                    ModificationType = StatModificationTypes.Absolute,
                    ModificationValue = 0f,
                    Timer = 2.5f
                };
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.AttackPoints,
                    ModificationType = StatModificationTypes.Numerical,
                    ModificationValue = -5,
                    Timer = 4f
                };
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.AttackPoints,
                    ModificationType = StatModificationTypes.Percentage,
                    ModificationValue = 150,
                    Timer = 5
                };
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                curModification = new StatModification
                {
                    StatToModify = StatTypes.AttackPoints,
                    ModificationType = StatModificationTypes.Absolute,
                    ModificationValue = 100,
                    Timer = 5
                };
            }

            #endregion

            if (curModification.Equals(StatModification.Empty)){return;}

            HUDUIController.Instance.BeginCooldown(curModification);
            
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            Entities
                .WithNone<StatModification>()
                .WithAll<PlayerTag>()
                .ForEach((Entity playerEntity) =>
                {
                    ecb.AddComponent(playerEntity, curModification);
                    ecb.AddComponent<ModifyStatsTag>(playerEntity);
                }).Run();

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}