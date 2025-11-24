using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private Image _experienceSlider;

    [Header("Animation Settings")]
    [Tooltip("Total time for the entire level up sequence, regardless of how many levels gained")]
    [SerializeField] private float _totalAnimationDuration = 3.0f;
    [Tooltip("Percentage of transition time used for pauses between level completions (0-1)")]
    [SerializeField] private float _pauseTimeRatio = 0.2f;
    [SerializeField] private Ease _fillEaseType = Ease.OutCubic;

    private int _startExperience;
    private int _addedExperience;
    private List<LevelTransition> _levelTransitions;
    private Coroutine _currentAnimationCoroutine;

    [Button]
    public void Initialise(int startExperience, int addedExperience)
    {
        _startExperience = startExperience;
        _addedExperience = addedExperience;

        // Calculate all level transitions needed
        _levelTransitions = CalculateLevelTransitions();

        // Calculate timing for each transition
        CalculateTransitionTiming();

        // Set initial UI state
        SetupInitialState();

        // Start the animation sequence
        StartLevelUpAnimation();
    }

  

    private List<LevelTransition> CalculateLevelTransitions()
    {
        var transitions = new List<LevelTransition>();

        var startResult = LevelUpManager.Instance.CalculateLevelFromXp(_startExperience);
        var endResult = LevelUpManager.Instance.CalculateLevelFromXp(_startExperience + _addedExperience);

        int currentLevel = startResult.level;
        int remainingXp = startResult.remainingXp;
        int xpToProcess = _addedExperience;

        // Process each level transition
        while (xpToProcess > 0 && currentLevel <= endResult.level)
        {
            int xpRequiredForCurrentLevel = LevelUpManager.Instance.GetXpRequiredForLevel(currentLevel);
            int xpNeededToCompleteLevel = xpRequiredForCurrentLevel - remainingXp;

            if (xpToProcess >= xpNeededToCompleteLevel)
            {
                // We complete this level
                transitions.Add(new LevelTransition
                {
                    level = currentLevel,
                    startProgress = (float)remainingXp / xpRequiredForCurrentLevel,
                    endProgress = 1.0f,
                    xpUsed = xpNeededToCompleteLevel,
                    completesLevel = true
                });

                xpToProcess -= xpNeededToCompleteLevel;
                currentLevel++;
                remainingXp = 0;
            }
            else
            {
                // We don't complete this level
                int finalXp = remainingXp + xpToProcess;
                transitions.Add(new LevelTransition
                {
                    level = currentLevel,
                    startProgress = (float)remainingXp / xpRequiredForCurrentLevel,
                    endProgress = (float)finalXp / xpRequiredForCurrentLevel,
                    xpUsed = xpToProcess,
                    completesLevel = false
                });

                xpToProcess = 0;
            }
        }

        return transitions;
    }

    private void CalculateTransitionTiming()
    {
        if (_levelTransitions.Count == 0) return;

        // Count how many levels we complete (for pauses)
        int levelCompletions = 0;
        foreach (var transition in _levelTransitions)
        {
            if (transition.completesLevel)
                levelCompletions++;
        }

        // Calculate time allocation
        float totalPauseTime = levelCompletions * _totalAnimationDuration * _pauseTimeRatio;
        float totalFillTime = _totalAnimationDuration - totalPauseTime;

        // Distribute fill time evenly across all transitions
        float timePerTransition = totalFillTime / _levelTransitions.Count;
        float pauseDuration = levelCompletions > 0 ? (totalPauseTime / levelCompletions) : 0f;

        // Apply timing to transitions
        for (int i = 0; i < _levelTransitions.Count; i++)
        {
            var transition = _levelTransitions[i];
            transition.fillDuration = timePerTransition;
            transition.pauseDuration = transition.completesLevel ? pauseDuration : 0f;
            _levelTransitions[i] = transition;
        }

        // Debug info
        Debug.Log($"Level Up Timing: {_levelTransitions.Count} transitions, {levelCompletions} completions. " +
                  $"Fill: {timePerTransition:F2}s per transition, Pause: {pauseDuration:F2}s per completion. " +
                  $"Total: {_totalAnimationDuration}s");
    }

    private void SetupInitialState()
    {
        if (_levelTransitions.Count == 0) return;

        var firstTransition = _levelTransitions[0];
        _currentLevelText.text = firstTransition.level.ToString();
        _nextLevelText.text = (firstTransition.level + 1).ToString();
        _experienceSlider.fillAmount = firstTransition.startProgress;
    }

    private void StartLevelUpAnimation()
    {
        if (_currentAnimationCoroutine != null)
        {
            StopCoroutine(_currentAnimationCoroutine);
        }

        _currentAnimationCoroutine = StartCoroutine(AnimateLevelUpSequence());
    }

    private IEnumerator AnimateLevelUpSequence()
    {
        foreach (var transition in _levelTransitions)
        {
            // Update level text
            _currentLevelText.text = transition.level.ToString();
            _nextLevelText.text = (transition.level + 1).ToString();

            // Animate the bar fill with calculated duration
            yield return AnimateBarFill(transition);

            // If we completed a level, pause for calculated duration
            if (transition.completesLevel)
            {
                yield return new WaitForSeconds(transition.pauseDuration);
                _experienceSlider.fillAmount = 0f; // Reset for next level
            }
        }

        // Animation complete
        OnAnimationComplete();
    }

    private IEnumerator AnimateBarFill(LevelTransition transition)
    {
        float startValue = transition.startProgress;
        float endValue = transition.endProgress;

        // Use DOTween to animate the bar with calculated duration
        var tween = _experienceSlider.DOFillAmount(endValue, transition.fillDuration)
            .SetEase(_fillEaseType)
            .From(startValue);

        yield return tween.WaitForCompletion();
    }

    private void OnAnimationComplete()
    {
        // Check if the final transition completed a level
        if (_levelTransitions.Count > 0)
        {
            var finalTransition = _levelTransitions[_levelTransitions.Count - 1];
            if (finalTransition.completesLevel)
            {
                // Update text to show the new level we've reached
                int newLevel = finalTransition.level + 1;
                _currentLevelText.text = newLevel.ToString();
                _nextLevelText.text = (newLevel + 1).ToString();
            }
        }

        // Optional: Trigger completion events here
        Debug.Log("Level up animation complete!");
        // TODO: Replace with your Topic System
        // LevelUpAnimationCompleted?.Invoke();
    }

    // Public method to skip animation if needed
    public void SkipToFinalState()
    {
        if (_currentAnimationCoroutine != null)
        {
            StopCoroutine(_currentAnimationCoroutine);
        }

        if (_levelTransitions.Count == 0) return;

        var finalTransition = _levelTransitions[_levelTransitions.Count - 1];
        _currentLevelText.text = finalTransition.level.ToString();
        _nextLevelText.text = (finalTransition.level + 1).ToString();
        _experienceSlider.fillAmount = finalTransition.endProgress;

        OnAnimationComplete();
    }
}