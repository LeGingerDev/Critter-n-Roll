using Core;
using Core.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Composite3DCollider : BaseBehaviour
{
    [Button]
    [Topic(EnvironmentEventIds.ON_UPDATE_COLLIDER_COMPOSITE)]
    public void UpdateCollider(object sender)
    {
        // 1. Collect meshes from children with the ColliderIdentifier component
        List<CombineInstance> combineList = new List<CombineInstance>();
        foreach (Transform child in transform)
        {
            // Only consider children with the special marker component
            if (child.GetComponent<ColliderIdentifier>() != null)
            {
                if(child.gameObject.activeInHierarchy == false) continue;  // skip inactive children
                MeshFilter mf = child.GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null) continue;  // skip if no mesh
                CombineInstance ci = new CombineInstance();
                ci.mesh = mf.sharedMesh;
                // Transform child mesh into parent's local space
                ci.transform = transform.worldToLocalMatrix * child.transform.localToWorldMatrix;
                combineList.Add(ci);
            }
        }

        // Nothing to combine? Then clear the collider and return.
        MeshCollider mc = GetComponent<MeshCollider>();
        if (combineList.Count == 0)
        {
            mc.sharedMesh = null;
            return;
        }

        // 2. Create a new mesh and combine all collected meshes
        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "CombinedColliderMesh";
        combinedMesh.CombineMeshes(combineList.ToArray(), true, true);
        // Optionally, you could optimize the mesh here (e.g., combinedMesh.Optimize()) if needed

        // 3. Assign the combined mesh to the MeshCollider
        mc.sharedMesh = combinedMesh;
        mc.convex = false;  // ensure the collider stays concave (allow holes/complex shape)

        // The MeshCollider will now use the merged mesh for collision.
    }
}
