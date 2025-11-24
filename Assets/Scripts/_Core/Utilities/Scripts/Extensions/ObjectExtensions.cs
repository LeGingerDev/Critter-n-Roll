using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static T[] FindObjectsOfInterface<T>(this Object[] objects) where T : class
       => objects.OfType<T>().ToArray();

        public static T FindInterfaceInScene<T>(this Object[] objects) where T : class
            => objects.OfType<T>().FirstOrDefault();

        // THIS is what changes:
        public static T[] FindObjectsOfInterface<T>(bool includeInactive = false) where T : class
        {
            var searchType = includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            return Object
                .FindObjectsByType<MonoBehaviour>(searchType, FindObjectsSortMode.None)
                .FindObjectsOfInterface<T>();
        }

        public static T FindInterfaceInScene<T>(bool includeInactive = false) where T : class
        {
            return FindObjectsOfInterface<T>(includeInactive).FirstOrDefault();
        }
    }
}