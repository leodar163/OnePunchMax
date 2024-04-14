using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {

        public static Vector2 Rotate(this Vector2 v, float delta)
        {
            delta *= Mathf.Deg2Rad;
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
    }
}