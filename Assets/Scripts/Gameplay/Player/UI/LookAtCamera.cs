using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Transform))]
public class LookAtCamera : MonoBehaviour
{
    [FoldoutGroup("Settings")]
    [SerializeField]
    private Camera _targetCamera;

    /// <summary>
    /// Axes on which the object will rotate to face the target.
    /// </summary>
    [System.Flags]
    public enum Axis
    {
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2
    }

    [FoldoutGroup("Settings")]
    [EnumToggleButtons]
    [SerializeField]
    private Axis _lookAxes = Axis.Y;

    [FoldoutGroup("Settings")]
    [SerializeField]
    private bool _invertDirection = false;

    private Coroutine _lookAtCoroutine;

    /// <summary>
    /// Returns the assigned camera or falls back to Camera.main.
    /// </summary>
    private Camera GetTargetCamera()
    {
        if (_targetCamera != null)
            return _targetCamera;

        if (Camera.main != null)
            return Camera.main;

        Debug.LogError($"[{nameof(LookAtCamera)}] No camera assigned and Camera.main is null.");
        return null;
    }

    private void OnEnable()
    {
        _lookAtCoroutine = StartCoroutine(LookAtCameraRoutine());
    }

    private void OnDisable()
    {
        if (_lookAtCoroutine != null)
        {
            StopCoroutine(_lookAtCoroutine);
            _lookAtCoroutine = null;
        }
    }

    /// <summary>
    /// Continuously rotates the object to face the target camera.
    /// </summary>
    private IEnumerator LookAtCameraRoutine()
    {
        var cam = GetTargetCamera();
        if (cam == null)
            yield break;

        while (true)
        {
            RotateTowardsCamera(cam);
            yield return null;
        }
    }

    /// <summary>
    /// Performs the actual rotation to face the camera, with axis filtering and optional inversion.
    /// </summary>
    private void RotateTowardsCamera(Camera cam)
    {
        Vector3 direction = cam.transform.position - transform.position;
        if (_invertDirection)
            direction = -direction;

        var masked = new Vector3(
            _lookAxes.HasFlag(Axis.X) ? direction.x : 0f,
            _lookAxes.HasFlag(Axis.Y) ? direction.y : 0f,
            _lookAxes.HasFlag(Axis.Z) ? direction.z : 0f
        );

        if (masked == Vector3.zero)
            return;

        transform.rotation = Quaternion.LookRotation(masked);
    }
}
