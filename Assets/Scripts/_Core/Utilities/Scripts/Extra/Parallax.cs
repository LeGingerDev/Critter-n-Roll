using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Extra
{
    public class Parallax : MonoBehaviour
    {
        [FoldoutGroup("References")] [SerializeField]
        private List<ParallaxData> _parallaxLayers = new List<ParallaxData>();

        [FoldoutGroup("Movement Values")] [SerializeField]
        private float _multiplier = 1f;

        [SerializeField] private Transform _targetTransform;
        private Vector3 _previousTargetPosition;

        private void Awake()
        {
            if (_targetTransform == null)
            {
                Debug.LogWarning("Parallax target not set. Please set the target using the SetTarget method.");
            }
        }

        private void LateUpdate()
        {
            if (_targetTransform == null) return;

            float deltaMovement = _targetTransform.position.x - _previousTargetPosition.x;

            foreach (var layer in _parallaxLayers)
            {
                layer.LayerTransform.position += Vector3.right * (deltaMovement * layer.MovementValue * _multiplier);
            }

            _previousTargetPosition = _targetTransform.position;
        }

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            _previousTargetPosition = _targetTransform.position;
        }
    }

    [Serializable]
    public class ParallaxData
    {
        [SerializeField] private Transform layerTransform;
        [SerializeField] private float movementValue;

        public Transform LayerTransform => layerTransform;
        public float MovementValue => movementValue;
    }
}