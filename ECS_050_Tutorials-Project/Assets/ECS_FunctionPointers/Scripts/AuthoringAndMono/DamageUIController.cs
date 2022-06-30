using System.Collections;
using TMPro;
using UnityEngine;

namespace TMG.FunctionPointers
{
    public class DamageUIController : MonoBehaviour
    {
        public static DamageUIController Instance;

        [SerializeField] public GameObject _damageUIPrefab;
        [SerializeField] private Transform _worldUICanvas;
        [SerializeField] private float _lifetime;
        [SerializeField] private float _moveRate;
        [SerializeField] private Transform _cameraTransform;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void DisplayDamageUI(int damageAmount, Vector3 position)
        {
            var damageText = $"{damageAmount}";

            var newDamageUI = Instantiate(_damageUIPrefab, position, Quaternion.identity, _worldUICanvas);

            var damageTMP = newDamageUI.GetComponent<TextMeshProUGUI>();
            damageTMP.text = damageText;

            StartCoroutine(DamageUILifecycle(newDamageUI));
        }

        private IEnumerator DamageUILifecycle(GameObject damageUI)
        {
            var timer = 0f;
            var damageUITransform = damageUI.GetComponent<Transform>();
            while (timer < _lifetime)
            {
                timer += Time.deltaTime;
                damageUITransform.position += new Vector3(0f, _moveRate * Time.deltaTime, 0f);
                damageUITransform.rotation = Quaternion.LookRotation(damageUITransform.position - _cameraTransform.position);
                yield return null;
            }
            Destroy(damageUI);
        }
    }
}