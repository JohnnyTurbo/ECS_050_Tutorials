using Unity.Entities;

namespace TMG.IJE
{
    [UpdateAfter(typeof(PerformSpellSystem))]
    public partial class SetBattleUISystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            Entities.ForEach((BattleEntityManaged battleEntityManaged, ref HitPoints hitPoints) =>
            {
                hitPoints.Current = hitPoints.Max;
                battleEntityManaged.HealthText.text = $"{hitPoints.Current}/{hitPoints.Max}";
                battleEntityManaged.HealthBarUI.value = 1f;
            }).WithoutBurst().Run();
        }

        protected override void OnUpdate()
        {
            Entities
                .WithChangeFilter<HitPoints>()
                .ForEach((BattleEntityManaged battleEntityManaged, ref HitPoints hitPoints) =>
            {
                battleEntityManaged.HealthText.text = $"{hitPoints.Current}/{hitPoints.Max}";
                battleEntityManaged.HealthBarUI.value = (float) hitPoints.Current / hitPoints.Max;
            }).WithoutBurst().Run();
        }
    }
}