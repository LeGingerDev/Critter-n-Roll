using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ColliderVisualizer : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        var mc = GetComponent<MeshCollider>();
        if (mc?.sharedMesh == null) return;

        // Use a custom color
        Gizmos.color = Color.cyan;

        // Draw the mesh in world space
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireMesh(mc.sharedMesh);
    }
}
