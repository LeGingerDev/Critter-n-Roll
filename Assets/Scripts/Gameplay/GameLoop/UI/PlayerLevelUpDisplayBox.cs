using TMPro;
using UnityEngine;

public class PlayerLevelUpDisplayBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _playerNameText;
    [SerializeField]
    private LevelUpBar _levelUpBar;

    PlayerUserData _playerUserData;
    public void Initialise(PlayerUserData playerUserData)
    {
        _playerUserData = playerUserData;
        _playerNameText.text = playerUserData.ChatData.GetDisplayName();
        int experienceGain = LevelUpManager.Instance.GetExperienceAmountForLevelDifficulty(LevelManager.Instance.GetCurrentLevelDifficulty(), playerUserData);
        _levelUpBar.Initialise(playerUserData.GetXp(), experienceGain);
        playerUserData.AddXp(experienceGain);
    }
}

/// <summary>
/// Data structure representing a single level transition in the animation.
/// Now includes calculated timing information.
/// </summary>
[System.Serializable]
public struct LevelTransition
{
    public int level;
    public float startProgress; // 0-1
    public float endProgress;   // 0-1
    public int xpUsed;
    public bool completesLevel;

    // Calculated timing
    public float fillDuration;
    public float pauseDuration;
}

/// <summary>
/// Static extension methods for level animation calculations.
/// </summary>
public static class LevelAnimationExtensions
{
    /// <summary>
    /// Calculate how many complete levels are gained from adding XP.
    /// </summary>
    public static int CalculateLevelsGained(int startXp, int addedXp)
    {
        var startResult = LevelUpManager.Instance.CalculateLevelFromXp(startXp);
        var endResult = LevelUpManager.Instance.CalculateLevelFromXp(startXp + addedXp);
        return endResult.level - startResult.level;
    }

    /// <summary>
    /// Get a summary of the level up animation for debugging.
    /// </summary>
    public static string GetAnimationSummary(int startXp, int addedXp)
    {
        var startResult = LevelUpManager.Instance.CalculateLevelFromXp(startXp);
        var endResult = LevelUpManager.Instance.CalculateLevelFromXp(startXp + addedXp);

        return $"Animation: Level {startResult.level} ({startResult.progressPercentage:P1}) → Level {endResult.level} ({endResult.progressPercentage:P1})";
    }
}
