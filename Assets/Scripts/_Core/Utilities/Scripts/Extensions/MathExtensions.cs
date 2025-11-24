using UnityEngine;

public static class MathExtensions
{
    public static float MapTo(this float value, float min, float max)
    {
        if (max - min == 0) return 1; // If min equals max, the bar should be full.
        if (value >= max) return 1; // Ensures the bar is full if the current value exceeds the max.
        return Mathf.Clamp01((value - min) / (max - min)); // Clamps the result between 0 and 1.
    }

    /// <summary>
    /// Maps a value from one range to another.
    /// </summary>
    /// <param name="value">The float value to map.</param>
    /// <param name="originalMin">Minimum of the original range.</param>
    /// <param name="originalMax">Maximum of the original range.</param>
    /// <param name="newMin">Minimum of the new range.</param>
    /// <param name="newMax">Maximum of the new range.</param>
    /// <returns>The value mapped to the new range.</returns>
    public static float MapTo(float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        // Prevent division by zero if original range is zero.
        if (Mathf.Abs(originalMax - originalMin) < Mathf.Epsilon)
        {
            UnityEngine.Debug.LogWarning("Original range is zero. Returning newMin.");
            return newMin;
        }

        float proportion = (value - originalMin) / (originalMax - originalMin);
        return newMin + proportion * (newMax - newMin);
    }
}