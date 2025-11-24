using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDetection
{
    [SerializeField, FoldoutGroup("Explosion Settings")]
    private ExplosionSettings _explosionSettings;

    [SerializeField, FoldoutGroup("Components")]
    private MeshRenderer _meshRenderer;

    [SerializeField, FoldoutGroup("Components")]
    private GameObject _explosionEffect;

    private Material _materialInstance;
    private ProximityTrigger _proximityTrigger;
    private bool _isExploding;

    private void Awake()
    {
        _proximityTrigger = GetComponent<ProximityTrigger>();
        InitializeMaterial();
    }

    private void InitializeMaterial()
    {
        if (_meshRenderer != null)
        {
            _materialInstance = _meshRenderer.material; // This creates an instance
            ResetShaderProperty();
        }
    }

    public void OnDetectionTriggered()
    {
        if (_isExploding) return;

        StartCoroutine(ExplosionSequence());
    }

    private IEnumerator ExplosionSequence()
    {
        _isExploding = true;
        _proximityTrigger.StopDetection();

        // Tween shader property with increasing speed
        var tween = CreateTickingTween();
        yield return tween.WaitForCompletion();

        // Explode
        TriggerExplosion();

        // Clean up
        yield return new WaitForSeconds(0.1f); // Small delay to ensure explosion effect is visible
        Destroy(gameObject);
    }

    private Tween CreateTickingTween()
    {
        var propertyName = _explosionSettings.GetShaderPropertyName();
        var totalDuration = _explosionSettings.GetTickDuration();

        // Start with slow ticking, speed up exponentially
        var sequence = DOTween.Sequence();

        float currentTickDuration = 0.8f; // Start slow
        float totalTime = 0f;

        while (totalTime < totalDuration)
        {
            // Add tick down (1 to 0)
            sequence.Append(_materialInstance.DOFloat(0f, propertyName, currentTickDuration * 0.5f));
            // Add tick up (0 to 1) 
            sequence.Append(_materialInstance.DOFloat(1f, propertyName, currentTickDuration * 0.5f));

            Debug.Log("Calling tick down/up with duration: " + currentTickDuration);

            _meshRenderer.transform.DOPunchScale(0.1f * Vector3.one, currentTickDuration * 0.5f, 10, 0.5f);

            totalTime += currentTickDuration;
            currentTickDuration *= 0.85f; // Get 15% faster each tick

            // Minimum tick speed
            if (currentTickDuration < 0.05f) currentTickDuration = 0.05f;
        }

        return sequence;
    }

    private void TriggerExplosion()
    {
        ActivateExplosionEffect();
        ApplyExplosionForce();
    }

    private void ActivateExplosionEffect()
    {
        if (_explosionEffect != null)
        {
            _explosionEffect.SetActive(true);
            _explosionEffect.transform.SetParent(null);

            // Destroy explosion effect after 4 seconds
            Destroy(_explosionEffect, 4f);
        }
    }

    private void ApplyExplosionForce()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            _explosionSettings.GetExplosionRadius(),
            _explosionSettings.GetAffectedLayers()
        );

        foreach (var col in colliders)
        {
            var rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(
                    _explosionSettings.GetExplosionForce(),
                    transform.position,
                    _explosionSettings.GetExplosionRadius(),
                    1f // Upward modifier
                );
            }
        }
    }

    private void ResetShaderProperty()
    {
        if (_materialInstance != null)
        {
            _materialInstance.SetFloat(_explosionSettings.GetShaderPropertyName(), 1f);
        }
    }

    private void OnDestroy()
    {
        // Clean up material instance
        if (_materialInstance != null)
        {
            DestroyImmediate(_materialInstance);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionSettings.GetExplosionRadius());
    }
}