using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = System.Random;

namespace Utilities.General
{
    public static class OTBGRandom
    {
        private static int _seed;

        private static readonly Dictionary<BaseBehaviour, Random> _randomDict =
            new Dictionary<BaseBehaviour, Random>();

        public static void SetSeed(int seed)
        {
            _seed = seed;
        }

        public static void CleanUp()
        {
            _randomDict.Clear();
            SetSeed(new Random().Next());
        }

        public static int Range(BaseBehaviour otbg, int min, int max)
        {
            return GetRandom(otbg).Next(min, max);
        }

        /// <summary>
        /// Min defaults to 0, only needs max value
        /// </summary>
        public static int Range(BaseBehaviour otbg, int max)
        {
            return GetRandom(otbg).Next(0, max);
        }

        public static float Range(BaseBehaviour otbg, float min, float max)
        {
            return (float)(GetRandom(otbg).NextDouble() * (max - min) + min);
        }

        public static Vector2 Vector2Range(BaseBehaviour otbg, float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(Range(otbg, minX, maxX), Range(otbg, minY, maxY));
        }

        public static bool RandomBool(BaseBehaviour otbg)
        {
            return GetRandom(otbg).Next(2) == 0;
        }

        public static Vector2 RandomDirection2D(BaseBehaviour otbg)
        {
            float angle = Range(otbg, 0f, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        }

        public static Random GetRandom(BaseBehaviour otbg)
        {
            if (!_randomDict.ContainsKey(otbg))
                _randomDict.Add(otbg, new Random(_seed));
            return _randomDict[otbg];
        }
    }
}