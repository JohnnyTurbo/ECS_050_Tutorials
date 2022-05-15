using UnityEngine;

namespace TMG.ManagedComponents
{
    public class GameObjectMover : MonoBehaviour
    {
        public float _frequency;
        public float _amplitude;

        private Vector3 _startPos;

        private void Awake()
        {
            transform.position = Vector3.up;
        }

        private void Start()
        {
            transform.position = new Vector3(25, 1, 25);
            _startPos = transform.position;
        }

        private void Update()
        {
            var curMovement = new Vector3(_amplitude * Mathf.Sin(_frequency * Time.time), 0, _amplitude * Mathf.Cos(_frequency * Time.time));
            transform.position = _startPos + curMovement;
            
        }
    }
}