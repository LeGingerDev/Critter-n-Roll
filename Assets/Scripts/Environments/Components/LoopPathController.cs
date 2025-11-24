// LoopPathController.cs
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoopPathController : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Path Points")]
    private List<Transform> _points;

    /// <summary>
    /// Return the next corner in the loop, based on your current position
    /// and the last visited point.  If lastVisited is null or not in the list,
    /// we pick the “higher‐index” of your two nearest corners.
    /// </summary>
    public Transform QueryNextTarget(Vector3 currentPos, Transform lastVisited = null)
    {
        if (_points == null || _points.Count == 0)
            throw new InvalidOperationException("No path points assigned!");

        int count = _points.Count;

        // first call (or if lastVisited wasn't one of our points)
        if (lastVisited == null || !_points.Contains(lastVisited))
        {
            var dists = new List<(int idx, float dist)>();
            for (int i = 0; i < count; i++)
                dists.Add((i, (currentPos - _points[i].position).sqrMagnitude));
            dists.Sort((a, b) => a.dist.CompareTo(b.dist));

            int i0 = dists[0].idx;
            int i1 = dists[1].idx;
            int startIdx = Mathf.Max(i0, i1);
            return _points[startIdx];
        }

        // subsequent calls: just go to next index (wrap around)
        int lastIdx = _points.IndexOf(lastVisited);
        int nextIdx = (lastIdx + 1) % count;
        return _points[nextIdx];
    }
}
