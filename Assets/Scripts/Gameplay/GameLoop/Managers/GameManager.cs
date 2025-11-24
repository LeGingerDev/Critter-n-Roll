using Core.Events;
using Core.Singleton;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Tasks;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public TaskManager _setupTaskManager;

    private HashSet<PlayerController> _playersFinished = new HashSet<PlayerController>();

    [Button]
    public void OnSetup()
    {
        StartCoroutine(_setupTaskManager.Execute());
    }

    [Topic(GameLoopEventIds.ON_LEVEL_STARTED)]
    public void OnLevelStarted(object sender, Level level)
    {
        Debug.Log($"Level {level.levelName} has started.");
        _playersFinished.Clear(); // Reset the finished players when a new level starts
    }

    [Topic(GameLoopEventIds.ON_PLAYER_FINISHED)]
    public void OnPlayerFinished(object sender, PlayerController player)
    {
        Debug.Log($"Player {player.name} has finished.");
        _playersFinished.Add(player);
        HandleIsLevelFinished();

        if(_playersFinished.Count == 1)
        {
            FirstPlayerFinished(player);
        }
    }

    public void FirstPlayerFinished(PlayerController playerController)
    {
        ToastData playerNotification = new ToastData()
        {
            message = $"{playerController.PlayerData.ChatData.GetDisplayName()} is the first to finish the level!",
            icon = null,
            type = ToastType.Success
        };

        ToastManager.Instance.SpawnToast(playerNotification);
    }

    public void HandleIsLevelFinished()
    {
        int totalPlayers = ActivePlayerManager.Instance.GetTotalPlayers();

        if (_playersFinished.Count < totalPlayers)
            return;

        Publish(GameLoopEventIds.ON_ALL_PLAYERS_FINISHED);
    }

    public HashSet<PlayerUserData> GetFinishedPlayers() => _playersFinished.Select(u => u.PlayerData).ToHashSet();
    public HashSet<PlayerUserData> GetUnfinishedPlayers() => ActivePlayerManager.Instance.GetAllActivePlayersExcept(_playersFinished);
    public bool DidPlayerFinish(PlayerUserData player) => _playersFinished.Any(p => p.PlayerData == player);
}
