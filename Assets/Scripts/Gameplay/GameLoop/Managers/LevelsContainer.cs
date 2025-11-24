using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsContainer", menuName = "LGD/Levels/Create Levels Container")]

public class LevelsContainer : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    [SerializeField]
    private Level _debugLevel;

    public List<Level> Levels => GetLevels();

    public List<Level> GetLevels()
    {
        if (_debugLevel == null) return levels;
        return new List<Level> { _debugLevel , _debugLevel, _debugLevel};
    }
}