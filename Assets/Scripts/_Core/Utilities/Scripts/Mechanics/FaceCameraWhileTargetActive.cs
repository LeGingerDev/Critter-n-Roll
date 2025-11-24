using UnityEngine;
namespace Core.Utilities.Mechanics
{
    public class FaceCameraWhileTargetActive : MonoBehaviour
    {
        [System.Flags]
        private enum Axis
        {
            Nothing = 0,
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2,
            All = X | Y | Z
        }

        [SerializeField, Tooltip("The object whose active state determines if we face the camera.")]
        private GameObject _target;

        [SerializeField, Tooltip("Which axes to follow when facing the camera.")]
        private Axis _followAxis = Axis.All;

        private Transform _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main?.transform;
            if (_mainCamera == null)
            {
                Debug.LogWarning("Main Camera not found in scene.");
            }
        }

        private void Update()
        {
            if (_target != null && _target.activeSelf && _mainCamera != null)
            {
                FaceCamera();
            }
        }

        private void FaceCamera()
        {
            Vector3 direction = _mainCamera.position - transform.position;

            // Zero out any axis that is NOT selected in the flags
            direction = new Vector3(
                _followAxis.HasFlag(Axis.X) ? direction.x : 0f,
                _followAxis.HasFlag(Axis.Y) ? direction.y : 0f,
                _followAxis.HasFlag(Axis.Z) ? direction.z : 0f
            );

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _target.transform.rotation = targetRotation;
            }
        }
    }

}