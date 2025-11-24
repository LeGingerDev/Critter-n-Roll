using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndGateIndentifier : MonoBehaviour
{
    [SerializeField]
    private float _detectionRadius;
    [SerializeField]
    private LayerMask _endGateLayerMask;

    public void TriggerEndGate()
    {
        //OverlapSphere, organise by distance and find the closest End Gate
        List<Collider> hitColliders = Physics.OverlapSphere(transform.position, _detectionRadius, _endGateLayerMask).ToList();
        if (hitColliders.Count == 0)
            return;

        // Sort the colliders by distance to the current position
        hitColliders.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
        // Get the closest End Gate
        EndGate closestEndGate = hitColliders[0].GetComponent<EndGate>();
        closestEndGate.TriggerExit(true);
    }

    private void OnDrawGizmos()
    {
        // Draw a sphere in the editor to visualize the detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
