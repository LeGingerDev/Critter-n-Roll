using Core;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using YourNamespace;

[RequireComponent(typeof(EnvironmentSpawnManager))]
public class LevelController : LevelGrid
{
    [SerializeField]
    private Transform _spawnPosition;

    private Level _levelData;
    private EnvironmentSpawnManager _environmentSpawnManager;
    private List<EndGateIndentifier> _endGateIdentifiers = new List<EndGateIndentifier>();

    public Transform SpawnPosition => _spawnPosition;
    public Level Level => _levelData;
    public void Initialise(Level levelData)
    {
        _levelData = levelData;
        _environmentSpawnManager = GetComponent<EnvironmentSpawnManager>();
        _endGateIdentifiers = GetComponentsInChildren<EndGateIndentifier>().ToList();
        _environmentSpawnManager.MoveAllElementsUp(isInstant: true);
    }

    public IEnumerator ShowLevel()
    {
        bool isFinished = false;
        yield return new WaitForEndOfFrame();

        _environmentSpawnManager.MoveAllElementsDown(isInstant: false, () => isFinished = true);
        yield return new WaitUntil(() => isFinished);
        _endGateIdentifiers.ForEach(eg => eg.TriggerEndGate());
    }

    public IEnumerator HideLevel()
    {
        bool isFinished = false;
        _environmentSpawnManager.MoveAllElementsUp(isInstant: false, () => isFinished = true);
        yield return new WaitUntil(() => isFinished);
    }
}