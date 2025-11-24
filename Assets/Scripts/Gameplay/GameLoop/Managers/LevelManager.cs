using Core.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private Level _previousLevel;
    [SerializeField]
    private Level _currentLevel;

    [SerializeField]
    private LevelsContainer _levelsContainer;

    private LevelController _currentlySpawnedController;
    private EnvironmentController _currentlySpawnedEnvironmentController;

    public List<Level> Levels => _levelsContainer.Levels;

    public LevelController CurrentlySpawnedController => _currentlySpawnedController;
    public EnvironmentController CurrentlySpawnedEnvironmentController => _currentlySpawnedEnvironmentController;
    public Level GetSelectedLevel() => _currentLevel;
    public Level GetPreviousLevel() => _previousLevel;
    public Level GetLevelFromCurrentlySpawned() => _currentlySpawnedController != null ? _currentlySpawnedController.Level : null;
    public LevelDifficulty GetCurrentLevelDifficulty() => _currentLevel.levelDifficulty;
    public void SelectLevel(Level level)
    {
        if(_currentLevel != null)
            _previousLevel = _currentLevel;
        _currentLevel = level;

        Publish(GameLoopEventIds.ON_LEVEL_SELECTED, _currentLevel);
    }

    public void SetLevelController(LevelController levelController) => _currentlySpawnedController = levelController;
    public void SetEnvironmentController(EnvironmentController environmentController) => _currentlySpawnedEnvironmentController = environmentController;
    public void ClearLevelController() => _currentlySpawnedController = null;
    public void ClearEnvironmentController() => _currentlySpawnedEnvironmentController = null;

    public bool AreEnvironmentsTheSame()
    {
        if (_previousLevel == null || _currentLevel == null)
            return false;
        return _previousLevel.environment.environmentName == _currentLevel.environment.environmentName;
    }
}
