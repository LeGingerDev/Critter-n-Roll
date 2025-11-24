using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Mechanics
{
    public class DistanceChecker : MonoBehaviour
    {
        public UnityEvent OnBeforeThresholdReached;
        public UnityEvent OnAfterThresholdReached;

        [SerializeField] private float _distanceThreshold = 15f;

        private Vector3 _lastPosition;
        private float _velocity;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void Update()
        {
            _velocity = Vector3.Distance(_lastPosition, transform.position) / Time.fixedDeltaTime;

            if (_velocity < _distanceThreshold)
            {
                OnBeforeThresholdReached?.Invoke();
            }
            else
            {
                OnAfterThresholdReached?.Invoke();
            }

            _lastPosition = transform.position;
        }
    }
}