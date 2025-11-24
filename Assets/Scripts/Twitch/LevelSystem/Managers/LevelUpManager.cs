using Core.Singleton;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for all level-up calculations and queries.
/// </summary>
public class LevelUpManager : MonoSingleton<LevelUpManager>
{
    [FoldoutGroup("Configuration")]
    [SerializeField] private CalculationReference _calculationReference;
    [SerializeField] private float _didntFinishMultiplier = 0.5f;
    [SerializeField] private List<LevelExperienceAmount> _levelExperienceAmounts = new List<LevelExperienceAmount>();

    // TODO: Replace with your Topic System when needed
    // public static event System.Action<int, int> OnLevelCalculated;

    protected override void Awake()
    {
        base.Awake();
        if (_calculationReference == null)
        {
            Debug.LogError("LevelUpManager: No CalculationReference assigned!");
        }
    }

    /// <summary>
    /// Given current level and XP earned, what level does that get you to?
    /// </summary>
    public LevelCalculationResult CalculateLevelFromXp(int totalXp)
    {
        return LevelCalculationExtensions.CalculateLevelFromXp(totalXp, _calculationReference);
    }

    /// <summary>
    /// How much XP is required for a specific level?
    /// </summary>
    public int GetXpRequiredForLevel(int level)
    {
        return LevelCalculationExtensions.GetXpRequiredForLevel(level, _calculationReference);
    }

    /// <summary>
    /// How much total XP is needed to reach a specific level?
    /// </summary>
    public int GetTotalXpForLevel(int level)
    {
        return LevelCalculationExtensions.GetTotalXpForLevel(level, _calculationReference);
    }

    /// <summary>
    /// Get progress info for a specific level and current XP.
    /// </summary>
    public LevelProgressInfo GetLevelProgress(int currentLevel, int currentXp)
    {
        int xpForCurrentLevel = GetXpRequiredForLevel(currentLevel);
        int xpForNextLevel = GetXpRequiredForLevel(currentLevel + 1);

        return new LevelProgressInfo
        {
            currentLevel = currentLevel,
            nextLevel = currentLevel + 1,
            currentLevelXp = currentXp,
            xpRequiredForCurrentLevel = xpForCurrentLevel,
            xpRequiredForNextLevel = xpForNextLevel,
            progressToNextLevel = (float)currentXp / xpForNextLevel
        };
    }

    public int GetExperienceAmountForLevelDifficulty(LevelDifficulty levelDifficulty, PlayerUserData userData)
    {
        LevelExperienceAmount entry = _levelExperienceAmounts.Find(e => e.levelDifficulty == levelDifficulty);
        bool didPlayerFinish = GameManager.Instance.DidPlayerFinish(userData);
        float finalMultiplier = didPlayerFinish ? 1.0f : _didntFinishMultiplier;
        return Mathf.RoundToInt(entry.experienceAmount * finalMultiplier);
    }

    public int GetLevelFromExperience(int experience)
    {
        return LevelCalculationExtensions.CalculateLevelFromXp(experience, _calculationReference).level;
    }
}

[Serializable]
public class LevelExperienceAmount
{
    public LevelDifficulty levelDifficulty;
    public int experienceAmount;
}