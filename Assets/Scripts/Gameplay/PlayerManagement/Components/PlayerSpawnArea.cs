using UnityEngine;

public class PlayerSpawnArea : MonoBehaviour, ISpawnPositioner
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public Vector3 GetSpawnPosition()
    {
        // Use the bounds of the collider to get the spawn position as a random point within the collider's bounds,
        // then use ClosestPoint to ensure it's actually inside the collider volume.
        if (_collider == null)
        {
            Debug.LogError("PlayerSpawnArea: Collider is not set.");
            return Vector3.zero;
        }

        Bounds bounds = _collider.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = 0f; 
        float z = Random.Range(bounds.min.z, bounds.max.z);
        Vector3 randomPoint = new Vector3(x, y, z);

        // ClosestPoint will project that randomPoint onto or inside the collider,
        // guaranteeing a valid spawn position even for meshes or capsules.
        return _collider.ClosestPoint(randomPoint);
    }

}
