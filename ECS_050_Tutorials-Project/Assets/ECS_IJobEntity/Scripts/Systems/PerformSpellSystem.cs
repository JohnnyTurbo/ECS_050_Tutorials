using Unity.Entities;

namespace TMG.IJE
{
    public partial class PerformSpellSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<SpellExecutionData>();
        }

        protected override void OnStartRunning()
        {
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().AsParallelWriter();
            var spellExecution = GetSingleton<SpellExecutionData>();

            var battleEntityQuery = EntityManager.CreateEntityQuery(typeof(HitPoints), typeof(TeamID));
            
            switch (spellExecution.TeamID)
            {
                case 0:
                    var team1 = new TeamID { Value = 0 };
                    battleEntityQuery.SetSharedComponentFilter(team1);
                    break;
                
                case 1:
                    var team2 = new TeamID { Value = 1 };
                    battleEntityQuery.SetSharedComponentFilter(team2);
                    break;
            }
            
            var newSpell = new PerformSpellJob
            {
                EffectAmount = spellExecution.EffectAmount,
                ECB = ecb
            };
            newSpell.ScheduleParallel(battleEntityQuery);

            EntityManager.RemoveComponent<SpellExecutionData>(GetSingletonEntity<SpellExecutionData>());
        }
    }
}












