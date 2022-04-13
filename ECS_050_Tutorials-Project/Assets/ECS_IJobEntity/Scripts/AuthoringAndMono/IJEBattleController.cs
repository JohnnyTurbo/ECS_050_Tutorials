using TMPro;
using Unity.Entities;
using UnityEngine;

namespace TMG.IJE
{
    public class IJEBattleController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _effectTypeDropdown;
        [SerializeField] private TMP_Dropdown _teamSelectDropdown;
        [SerializeField] private TMP_InputField _hitPointsInputField;

        private Entity _spellController;
        private EntityManager _entityManager;
        
        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _spellController = _entityManager.CreateEntityQuery(typeof(SpellController)).GetSingletonEntity();
        }

        public void OnButtonExecute()
        {
            var effectModifier = _effectTypeDropdown.value == 0 ? 1 : -1;
            var effectAmount = int.Parse(_hitPointsInputField.text) * effectModifier;

            new PerformSpellJob
            {
                EffectAmount = 10,
                ECB = World.DefaultGameObjectInjectionWorld.GetExistingSystem<EndSimulationEntityCommandBufferSystem>()
                    .CreateCommandBuffer().AsParallelWriter()
            }.ScheduleParallel();
            
            var newSpellExecutionData = new SpellExecutionData
            {
                EffectAmount = effectAmount,
                TeamID = _teamSelectDropdown.value
            };
            _entityManager.AddComponentData(_spellController, newSpellExecutionData);
        }
    }
}