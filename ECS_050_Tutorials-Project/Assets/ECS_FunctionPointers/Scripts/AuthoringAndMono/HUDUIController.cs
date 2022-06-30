using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.FunctionPointers
{
    public class HUDUIController : MonoBehaviour
    {
        public static HUDUIController Instance;

        [SerializeField] private Image _cooldownMask;
        [SerializeField] private Image _attackIcon;
        [SerializeField] private Image _speedIcon;
        [SerializeField] private TextMeshProUGUI _modifierText;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void BeginCooldown(StatModification statModification)
        {
            switch (statModification.StatToModify)
            {
                case StatTypes.AttackPoints:
                    _attackIcon.enabled = true;
                    break;
                case StatTypes.MoveSpeed:
                    _speedIcon.enabled = true;
                    break;
                default:
                    return;
            }

            var modificationString = "";

            switch (statModification.ModificationType)
            {
                case StatModificationTypes.Percentage:
                    modificationString = $"{statModification.ModificationValue}%";
                    break;
                case StatModificationTypes.Numerical:
                    var sign = statModification.ModificationValue >= 0 ? "+" : "";
                    modificationString = $"{sign}{statModification.ModificationValue}";
                    break;
                case StatModificationTypes.Absolute:
                    modificationString = $"{statModification.ModificationValue}";
                    break;
                default:
                    return;
            }

            _modifierText.enabled = true;
            _modifierText.text = modificationString;
            _cooldownMask.enabled = true;
            
            StartCoroutine(Cooldown(statModification.Timer));
        }

        private IEnumerator Cooldown(float duration)
        {
            var timer = duration;
            while (timer > 0f)
            {
                var fillPercent = timer / duration;
                _cooldownMask.fillAmount = fillPercent;
                timer -= Time.deltaTime;
                yield return null;
            }

            _cooldownMask.enabled = false;
            _attackIcon.enabled = false;
            _speedIcon.enabled = false;
            _modifierText.enabled = false;
        }
    }
}