using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.IJE
{
    public class BattleEntityManagedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private GameObject _battleEntityUI;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var image = _battleEntityUI.GetComponent<Image>();
            var healthBarUI = _battleEntityUI.GetComponentInChildren<Slider>();
            var healthText = _battleEntityUI.GetComponentInChildren<TextMeshProUGUI>();

            var newBattleEntityManaged = new BattleEntityManaged
            {
                CharacterImage = image,
                HealthBarUI = healthBarUI,
                HealthText = healthText
            };

            dstManager.AddComponentData(entity, newBattleEntityManaged);
        }
    }
}