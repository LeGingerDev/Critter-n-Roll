using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Bumper : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Bumper Settings")]
    private BumperSettings _bumperSettings;

    [SerializeField, FoldoutGroup("Components")]
    private Transform _modelContainer;

    private CapsuleCollider _capsuleCollider;
    private Vector3 _originalScale;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.isTrigger = false;

        if (_modelContainer != null)
        {
            _originalScale = _modelContainer.localScale;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object is on the affected layer
        if (!IsLayerInMask(collision.gameObject.layer, _bumperSettings.GetAffectedLayers()))
            return;

        var rb = collision.transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            ApplyBounce(rb, collision.contacts[0].point);
            TriggerVisualEffect();
        }
    }

    private void ApplyBounce(Rigidbody rigidbody, Vector3 contactPoint)
    {
        // Calculate bounce direction from bumper center to contact point
        var bounceDirection = (contactPoint - transform.position).normalized;

        // Apply the bounce force
        rigidbody.angularVelocity = Vector3.zero;
        bounceDirection.y = 0;
        rigidbody.AddForce(bounceDirection * _bumperSettings.GetBounceForce(), ForceMode.Impulse);
    }

    private void TriggerVisualEffect()
    {
        if (_modelContainer != null)
        {
            // Kill any existing scale tweens to prevent conflicts
            _modelContainer.DOKill();

            // Reset to original scale first, then punch
            _modelContainer.localScale = _originalScale;
            _modelContainer.DOPunchScale(
                Vector3.one * _bumperSettings.GetPunchScale(),
                _bumperSettings.GetPunchDuration(),
                vibrato: 1,
                elasticity: 0.5f
            ).SetEase(_bumperSettings.GetPunchEase());
        }
    }

    private bool IsLayerInMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the bumper detection area
        Gizmos.color = Color.magenta;

        var sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        }

        // Draw bounce force visualization
        Gizmos.color = Color.yellow;
        var forceVisualizationRadius = sphereCollider ? sphereCollider.radius + 1f : 2f;

        // Draw arrows showing bounce directions
        for (int i = 0; i < 8; i++)
        {
            var angle = (i * 45f) * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            var startPos = transform.position + direction * (sphereCollider ? sphereCollider.radius : 1f);
            var endPos = startPos + direction * 1.5f;

            Gizmos.DrawRay(startPos, direction * 1.5f);
        }
    }
}