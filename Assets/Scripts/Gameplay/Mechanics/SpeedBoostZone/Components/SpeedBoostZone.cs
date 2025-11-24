using Sirenix.OdinInspector;
using UnityEngine;

public class SpeedBoostZone : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Boost Settings")]
    private SpeedBoostSettings _boostSettings;

    private void Start()
    {
        StartCoroutine(BoostLoop());
    }

    private System.Collections.IEnumerator BoostLoop()
    {
        var waitInterval = new WaitForSeconds(_boostSettings.GetUpdateInterval());

        while (true)
        {
            ApplyBoostToRigidbodiesInZone();
            yield return waitInterval;
        }
    }

    private void ApplyBoostToRigidbodiesInZone()
    {
        // Convert full size to half-extents for OverlapBox
        var halfExtents = _boostSettings.GetBoostZoneSize() * 0.5f;

        var colliders = Physics.OverlapBox(
            transform.position,
            halfExtents, // Now correctly using half of the desired full size
            transform.rotation,
            _boostSettings.GetAffectedLayers()
        );

        foreach (var col in colliders)
        {
            var rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                ApplyBoostForce(rb);
            }
        }
    }

    private void ApplyBoostForce(Rigidbody rigidbody)
    {
        var boostDirection = transform.forward;
        var force = boostDirection * _boostSettings.GetBoostForce();

        rigidbody.AddForce(force, _boostSettings.GetForceMode());
    }

    private void OnDrawGizmosSelected()
    {
        // Draw in world space to avoid scale issues
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        // Draw the exact size you're setting
        Gizmos.DrawWireCube(Vector3.zero, _boostSettings.GetBoostZoneSize());

        // Reset matrix for arrow
        Gizmos.matrix = Matrix4x4.identity;

        // Draw boost direction arrow
        Gizmos.color = Color.yellow;
        var startPos = transform.position;
        var endPos = startPos + (transform.forward * 2f);

        Gizmos.DrawRay(startPos, transform.forward * 2f);
        // Simple arrowhead
        var right = Vector3.Cross(transform.forward, Vector3.up) * 0.3f;
        var up = Vector3.Cross(right, transform.forward) * 0.3f;

        Gizmos.DrawRay(endPos, (-transform.forward + right) * 0.5f);
        Gizmos.DrawRay(endPos, (-transform.forward - right) * 0.5f);
        Gizmos.DrawRay(endPos, (-transform.forward + up) * 0.5f);
        Gizmos.DrawRay(endPos, (-transform.forward - up) * 0.5f);
    }
}