using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            T[] array = enumerable.ToArray();
            return array.Random();
        }

        public static T Random<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array must not be empty.");
            }

            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static List<T> RandomMultiple<T>(this IList<T> list, int count)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("List must not be empty.");
            }

            List<T> randomElements = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T randomElement = list.Random();
                randomElements.Add(randomElement);
            }

            return randomElements;
        }

        public static List<T> RandomMultiple<T>(this IList<T> list, int min, int max)
        {
            int count = UnityEngine.Random.Range(min, max + 1); // Ensure max is inclusive
            return list.RandomMultiple(count);
        }

        public static List<T> RandomMultipleUnique<T>(this IList<T> list, int count)
        {
            if (list.Count == 0 || count < 1)
            {
                return new List<T>();
            }

            List<T> temp = new List<T>(list);
            temp.Shuffle();
            count = Mathf.Clamp(count, 0, temp.Count);
            return temp.Take(count).ToList();
        }

        public static List<T> GetImmediateComponentsInChildren<T>(this Transform parent) where T : Component
        {
            List<T> immediateChildrenComponents = new List<T>();
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    immediateChildrenComponents.Add(component);
                }
            }

            return immediateChildrenComponents;
        }

        public static List<T> SelectRange<T>(this List<T> input, int startIndex, int endIndex)
        {
            startIndex = Mathf.Clamp(startIndex, 0, input.Count - 1);
            endIndex = Mathf.Clamp(endIndex, 0, input.Count - 1);

            int count = Mathf.Max(endIndex - startIndex + 1, 0);
            return input.GetRange(startIndex, count);
        }

        public static List<T> SelectRange<T>(this List<T> input, Vector2Int range)
        {
            return SelectRange(input, range.x, range.y);
        }

        public static void DestroyAll<T>(this List<T> list) where T : Component
        {
            // Iterate over all items in the list and destroy their GameObjects
            foreach (var item in list)
            {
                if (item != null)
                    Object.Destroy(item.gameObject);
            }

            // Clear the list after destroying all GameObjects
            list.Clear();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void RemoveNulls<T>(this List<T> list) where T : class
        {
            list.RemoveAll(item => item == null);
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }
    }
}