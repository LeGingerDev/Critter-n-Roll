using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Proximity Settings")]
    private ProximitySettings _settings;

    private IDetection _detectionTarget;
    private Coroutine _detectionCoroutine;
    private bool _hasTriggered;

    private void Awake()
    {
        _detectionTarget = GetComponent<IDetection>();
        if (_detectionTarget == null)
        {
            Debug.LogError($"ProximityTrigger on {gameObject.name} requires a component implementing IDetection", this);
        }
    }

    private void Start()
    {
        StartDetection();
    }

    public void StartDetection()
    {
        if (_detectionCoroutine != null)
            StopCoroutine(_detectionCoroutine);

        _hasTriggered = false;
        _detectionCoroutine = StartCoroutine(DetectionLoop());
    }

    public void StopDetection()
    {
        if (_detectionCoroutine != null)
        {
            StopCoroutine(_detectionCoroutine);
            _detectionCoroutine = null;
        }
    }

    private IEnumerator DetectionLoop()
    {
        var waitInterval = new WaitForSeconds(_settings.GetCheckInterval());

        while (!_hasTriggered)
        {
            if (CheckForTargetsInRange())
            {
                _hasTriggered = true;
                _detectionTarget?.OnDetectionTriggered();
                yield break;
            }

            yield return waitInterval;
        }
    }

    private bool CheckForTargetsInRange()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            _settings.GetDetectionRadius(),
            _settings.GetTargetLayers()
        );

        return colliders.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _hasTriggered ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _settings.GetDetectionRadius());
    }
}