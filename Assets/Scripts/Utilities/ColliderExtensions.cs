using UnityEngine;
namespace LGD.Utilities
{
    public static class ColliderExtensions
    {
        /// <summary>
        /// Returns a random point inside the bounds of the collider.
        /// Retries until a point lands within the actual collider volume (using ClosestPoint),
        /// up to <paramref name="maxIterations"/> times. Falls back to bounds.center if none found.
        /// </summary>
        /// <param name="collider">The collider to sample.</param>
        /// <param name="maxIterations">How many times to attempt finding a valid point.</param>
        /// <returns>A point guaranteed to be inside the collider’s AABB, and—if found within
        /// the iteration limit—also inside the collider’s shape.</returns>
        public static Vector3 GetRandomPointInside(this Collider collider, int maxIterations = 30)
        {
            var bounds = collider.bounds;
            Vector3 randomPoint = bounds.center;

            for (int i = 0; i < maxIterations; i++)
            {
                randomPoint = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                // If the point is inside the collider, ClosestPoint returns the same point.
                if (collider.ClosestPoint(randomPoint) == randomPoint)
                    return randomPoint;
            }

            // Fallback if we couldn't find an interior point in time.
            return bounds.center;
        }
    }
}