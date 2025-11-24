using Core;
using UnityEngine;
using Utilities.General;

namespace Utilities.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 GetRandomPointAlongCircle(BaseBehaviour otbg, float diameter)
        {
            float angle = OTBGRandom.Range(otbg, 0f, Mathf.PI * 2);
            float radius = diameter / 2;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            return new Vector2(x, y);
        }

        public static Vector2 GetRandomPointAlongCircle(BaseBehaviour otbg, Vector2 originPoint, float diameter)
        {
            return originPoint + GetRandomPointAlongCircle(otbg, diameter);
        }

        public static Vector2 GetRandomPointInsideCircle(BaseBehaviour otbg, float diameter)
        {
            float angle = OTBGRandom.Range(otbg, 0f, Mathf.PI * 2);
            float radius = diameter / 2;
            // Scale radius by sqrt of a random number to ensure uniform distribution
            float randomRadius = Mathf.Sqrt(OTBGRandom.Range(otbg, 0f, 1f)) * radius;
            float x = Mathf.Cos(angle) * randomRadius;
            float y = Mathf.Sin(angle) * randomRadius;

            return new Vector2(x, y);
        }

        public static Vector2 GetRandomPointInsideCircle(BaseBehaviour otbg, Vector2 originPoint, float diameter)
        {
            return originPoint + GetRandomPointInsideCircle(otbg, diameter);
        }

        public static float RandomBetweenXY(this Vector2 vector, BaseBehaviour otbg)
        {
            return OTBGRandom.Range(otbg, vector.x, vector.y);
        }
    }
}