using Sirenix.OdinInspector;

using UnityEngine;

/// <summary>
/// ScriptableObject containing all XP calculation parameters.
/// </summary>
[CreateAssetMenu(fileName = "New Level Calculation", menuName = "Game/Level Calculation Reference")]
public class CalculationReference : ScriptableObject
{
    [FoldoutGroup("Base Settings")]
    [SerializeField] private int _baseXpRequired = 100;

    [FoldoutGroup("Base Settings")]
    [SerializeField] private int _xpGrowthAmount = 20;

    [FoldoutGroup("Base Settings")]
    [Tooltip("How XP requirements grow: 1.0 = linear, >1.0 = exponential")]
    [SerializeField] private float _growthExponent = 1.0f;

    [FoldoutGroup("Limits")]
    [SerializeField] private int _maxLevel = 100;

    [FoldoutGroup("Debug")]
    [SerializeField] private int _debugCurrentLevel = 1;

    [FoldoutGroup("Debug")]
    [SerializeField] private int _debugTotalXp = 0;

    [FoldoutGroup("Debug")]
    [Button("Calculate Debug Level")]
    private void CalculateDebugLevel()
    {
        var result = LevelCalculationExtensions.CalculateLevelFromXp(_debugTotalXp, this);
        Debug.Log($"Total XP: {_debugTotalXp} = Level {result.level} with {result.remainingXp}/{result.xpForCurrentLevel} XP ({result.progressPercentage:P1})");
    }

    [FoldoutGroup("Debug")]
    [Button("Calculate XP Required for Debug Level")]
    private void CalculateDebugXpRequired()
    {
        int xpRequired = LevelCalculationExtensions.GetXpRequiredForLevel(_debugCurrentLevel, this);
        int totalXpRequired = LevelCalculationExtensions.GetTotalXpForLevel(_debugCurrentLevel, this);
        Debug.Log($"Level {_debugCurrentLevel} requires {xpRequired} XP (Total: {totalXpRequired} XP)");
    }

    // Getters for the calculation system
    public int GetBaseXpRequired() => _baseXpRequired;
    public int GetXpGrowthAmount() => _xpGrowthAmount;
    public float GetGrowthExponent() => _growthExponent;
    public int GetMaxLevel() => _maxLevel;
}