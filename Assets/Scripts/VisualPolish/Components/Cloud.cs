using Sirenix.OdinInspector;
using UnityEngine;
namespace LGD.VisualPolish.Components
{
    public class Cloud : MonoBehaviour
    {
        [MinMaxSlider(0f, 100f), SerializeField]
        private Vector2 _speedRange;

        private float _movementSpeed;
        private bool _inReverse;
        public void Initialise(bool inReverse = false)
        {
            SetSpeedToMove();
            _inReverse = inReverse;
            transform.localScale = Vector3.one * Random.Range(0.4f, 0.7f);
            Destroy(gameObject, 25);
        }

        private void Update()
        {
            Vector3 direction = _inReverse ? Vector3.left : Vector3.right;
            transform.position += direction * _movementSpeed * Time.deltaTime;
        }

        public void SetSpeedToMove()
        {
            _movementSpeed = Random.Range(_speedRange.x, _speedRange.y);
        }
    }

}