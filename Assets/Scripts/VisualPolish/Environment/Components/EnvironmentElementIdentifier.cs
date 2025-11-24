using UnityEngine;
using Sirenix.OdinInspector;

namespace YourNamespace
{
    /// <summary>
    /// Attach this to any environment object to tag it with a type and record its starting position.
    /// </summary>
    public class EnvironmentElementIdentifier : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Identifier")]
        private EnvironmentElementType _elementType;

        private Vector3 _initialPosition;

        /// <summary>Gets the assigned element type.</summary>
        public EnvironmentElementType GetElementType() => _elementType;
        /// <summary>Gets the world position when spawned.</summary>
        public Vector3 GetInitialPosition() => _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.position;
        }
    }
}