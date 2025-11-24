using System.Collections.Generic;
using System.Linq;
using General.Utility;
using UnityEngine;

namespace Utilities.Extensions
{
    public static class TransformExtensions
    {
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>()
                         .Where(t => t.GetComponent<DontDestroyOnClear>() == null))
            {
                if (child == transform)
                    continue;
                Object.Destroy(child.gameObject);
            }
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
            return children;
        }

        public static void Clear<T>(this Transform transform) where T : MonoBehaviour
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>()
                         .Where(t => t.GetComponent<T>() != null &&
                                     t.GetComponent<DontDestroyOnClear>() == null))
            {
                if (child == transform)
                    continue;
                Object.Destroy(child.gameObject);
            }
        }

        // Get Distance along the X axis
        public static float DistanceX(this Transform transform, Transform otherTransform)
        {
            return otherTransform.position.x - transform.position.x;
        }

        // Get Distance along the Y axis
        public static float DistanceY(this Transform transform, Transform otherTransform)
        {
            return otherTransform.position.y - transform.position.y;
        }

        // Get Distance along the Z axis
        public static float DistanceZ(this Transform transform, Transform otherTransform)
        {
            return otherTransform.position.z - transform.position.z;
        }


        public static float Distance(this Transform transform, Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }

        public static void LookAt2D(this Transform transform, Transform target, Vector3 forwardAxis)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, forwardAxis);
        }
    }
}