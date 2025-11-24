using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections; // Remove if you’re not using Odin Inspector

public class FloatingEffect : MonoBehaviour
{
    [FoldoutGroup("Floating Settings")]
    [SerializeField, Tooltip("How far (in units) this object moves up and down.")]
    private float _floatAmplitude = 0.5f;

    [FoldoutGroup("Floating Settings")]
    [SerializeField, Tooltip("How fast (cycles per second) the up/down motion is.")]
    private float _floatSpeed = 1f;

    [FoldoutGroup("Floating Settings")]
    [SerializeField, Tooltip("Maximum tilt angle (in degrees) on the Z-axis.")]
    private float _maxTiltAngle = 5f;

    [FoldoutGroup("Floating Settings")]
    [SerializeField, Tooltip("How fast (cycles per second) the tilting side-to-side is.")]
    private float _tiltSpeed = 0.5f;
    [FoldoutGroup("Floating Settings")]
    [SerializeField, Tooltip("How fast (cycles per second) the tilting side-to-side is.")]
    private bool _randomiseStartTime;

    // Cache the original local position & rotation so we always float relative to start
    private Vector3 _originalLocalPos;
    private Quaternion _originalLocalRot;
    private Coroutine _floatCoroutine;



    private void OnEnable()
    {
        // Record starting values (use localPosition so it works on UI/RectTransform)
        _originalLocalPos = transform.localPosition;
        _originalLocalRot = transform.localRotation;

        // Start the floating coroutine
        _floatCoroutine = StartCoroutine(FloatingRoutine());
    }

    private void OnDisable()
    {
        // Stop the coroutine if the component is disabled
        if (_floatCoroutine != null)
        {
            StopCoroutine(_floatCoroutine);
            _floatCoroutine = null;
        }

        // Optional: revert to original position/rotation if you want
        transform.localPosition = _originalLocalPos;
        transform.localRotation = _originalLocalRot;
    }

    private IEnumerator FloatingRoutine()
    {

        float timeElapsed = 0f;
        if (_randomiseStartTime)
            timeElapsed = Random.Range(0f, 5f);

        while (true)
        {
            // Advance time
            timeElapsed += Time.deltaTime;

            // ===== Vertical bobbing (sinusoidal) =====
            // Calculate a value between -1 and +1, based on sine wave
            float verticalOffset = Mathf.Sin(timeElapsed * Mathf.PI * 2f * _floatSpeed) * _floatAmplitude;
            Vector3 newLocalPos = _originalLocalPos + Vector3.up * verticalOffset;
            transform.localPosition = newLocalPos;

            // ===== Side-to-side tilt (also sinusoidal) =====
            // Again use sine: range is -_maxTiltAngle .. +_maxTiltAngle
            float tiltAngle = Mathf.Sin(timeElapsed * Mathf.PI * 2f * _tiltSpeed) * _maxTiltAngle;
            Quaternion tiltRot = Quaternion.Euler(0f, 0f, tiltAngle);
            transform.localRotation = _originalLocalRot * tiltRot;

            yield return null; // wait until next frame
        }
    }

    #region Public API (if you ever need to adjust parameters at runtime)
    /// <summary>
    /// Change float amplitude at runtime.
    /// </summary>
    public void SetFloatAmplitude(float newAmp)
    {
        _floatAmplitude = newAmp;
    }

    /// <summary>
    /// Change float speed at runtime.
    /// </summary>
    public void SetFloatSpeed(float newSpeed)
    {
        _floatSpeed = newSpeed;
    }

    /// <summary>
    /// Change maximum tilt angle at runtime.
    /// </summary>
    public void SetMaxTiltAngle(float newAngle)
    {
        _maxTiltAngle = newAngle;
    }

    /// <summary>
    /// Change tilt speed at runtime.
    /// </summary>
    public void SetTiltSpeed(float newSpeed)
    {
        _tiltSpeed = newSpeed;
    }
    #endregion
}
