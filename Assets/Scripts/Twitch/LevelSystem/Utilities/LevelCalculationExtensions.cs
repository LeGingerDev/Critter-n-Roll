using UnityEngine;

/// <summary>
/// Static extension methods for level calculations.
/// </summary>
public static class LevelCalculationExtensions
{
    /// <summary>
    /// Calculate what level and remaining XP from total XP.
    /// </summary>
    public static LevelCalculationResult CalculateLevelFromXp(int totalXp, CalculationReference calcRef)
    {
        if (calcRef == null || totalXp < 0)
        {
            return new LevelCalculationResult { level = 1, remainingXp = 0, xpForCurrentLevel = calcRef?.GetBaseXpRequired() ?? 100, progressPercentage = 0f };
        }

        int currentLevel = 1;
        int remainingXp = totalXp;
        int maxLevel = calcRef.GetMaxLevel();

        // Keep leveling up until we don't have enough XP
        while (currentLevel < maxLevel)
        {
            int xpRequiredForLevel = GetXpRequiredForLevel(currentLevel, calcRef);

            if (remainingXp >= xpRequiredForLevel)
            {
                remainingXp -= xpRequiredForLevel;
                currentLevel++;
            }
            else
            {
                break;
            }
        }

        // Calculate progress for current level
        int xpForCurrentLevel = GetXpRequiredForLevel(currentLevel, calcRef);
        float progressPercentage = xpForCurrentLevel > 0 ? (float)remainingXp / xpForCurrentLevel : 0f;

        return new LevelCalculationResult
        {
            level = currentLevel,
            remainingXp = remainingXp,
            xpForCurrentLevel = xpForCurrentLevel,
            progressPercentage = progressPercentage
        };
    }

    /// <summary>
    /// Get XP required for a specific level (not cumulative).
    /// </summary>
    public static int GetXpRequiredForLevel(int level, CalculationReference calcRef)
    {
        if (calcRef == null || level <= 1)
            return calcRef?.GetBaseXpRequired() ?? 100;

        // Linear growth: baseXp + (level-1) * growthAmount
        // With exponent support: baseXp + (level-1) * growthAmount * (level-1)^(exponent-1)
        int baseXp = calcRef.GetBaseXpRequired();
        int growthAmount = calcRef.GetXpGrowthAmount();
        float exponent = calcRef.GetGrowthExponent();

        if (Mathf.Approximately(exponent, 1.0f))
        {
            // Pure linear: 100, 120, 140, 160...
            return baseXp + ((level - 1) * growthAmount);
        }
        else
        {
            // With exponent: more complex growth
            float multiplier = Mathf.Pow(level - 1, exponent - 1);
            return Mathf.RoundToInt(baseXp + ((level - 1) * growthAmount * multiplier));
        }
    }

    /// <summary>
    /// Get total cumulative XP needed to reach a specific level.
    /// </summary>
    public static int GetTotalXpForLevel(int targetLevel, CalculationReference calcRef)
    {
        if (targetLevel <= 1) return 0;

        int totalXp = 0;
        for (int level = 1; level < targetLevel; level++)
        {
            totalXp += GetXpRequiredForLevel(level, calcRef);
        }
        return totalXp;
    }
}

/// <summary>
/// Detailed progress info for UI display.
/// </summary>
[System.Serializable]
public struct LevelProgressInfo
{
    public int currentLevel;
    public int nextLevel;
    public int currentLevelXp;
    public int xpRequiredForCurrentLevel;
    public int xpRequiredForNextLevel;
    public float progressToNextLevel; // 0-1
}