using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Extensions;

namespace General.Utility
{
    public static class UtilityFuncs
    {
        public static T[] FindObjectsInScene<T>(bool findInactive = false) where T : class
        {
            // simply forward to your ObjectExtensions.FindObjectsOfInterface<T>(bool)
            return ObjectExtensions.FindObjectsOfInterface<T>(findInactive);
        }

        public static T FindInterfaceInScene<T>(bool findInactive = false) where T : class
        {
            // and forward to the single-result overload
            return ObjectExtensions.FindInterfaceInScene<T>(findInactive);
        }

        public static List<T> GetComponentsInParentAndChildren<T>(GameObject gameObject) where T : Component
        {
            var parentComponent = gameObject.GetComponent<T>();

            List<T> childComponents = gameObject.GetComponentsInChildren<T>().ToList();

            if (parentComponent)
            {
                childComponents.Insert(0, parentComponent);
            }

            return childComponents;
        }

        public static bool TryGetComponentInChildren<T>(this MonoBehaviour monoBehaviour, out T component)
            where T : Component
        {
            return TryGetComponentInChildrenRecursive(monoBehaviour.transform, out component);
        }

        private static bool TryGetComponentInChildrenRecursive<T>(Transform parent, out T component) where T : Component
        {
            foreach (Transform child in parent)
            {
                // Check the current child
                if (child.TryGetComponent(out component))
                {
                    return true;
                }

                // Recursively check the children of the current child
                if (TryGetComponentInChildrenRecursive(child, out component))
                {
                    return true;
                }
            }

            component = null;
            return false;
        }
    }
}