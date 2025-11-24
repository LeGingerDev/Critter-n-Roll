using Core.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tasks;
using UnityEngine;

public class StartGameplayTask : TaskBase
{
    [SerializeField]
    private TimedSelection _gameplayTimer;

    public override IEnumerator ExecuteInternal()
    {
        Level currentLevel = LevelManager.Instance.GetSelectedLevel();
        _gameplayTimer.gameObject.SetActive(true);

        List<PlayerUserData> players = ActivePlayerManager.Instance.GetActivePlayers().ToList();
        players.ForEach(u => u.AddGamePlayed());
        yield return _gameplayTimer.WaitTime(currentLevel.GetLevelDuration(), true);
        _gameplayTimer.gameObject.SetActive(false);
    }

    [Topic(GameLoopEventIds.ON_ALL_PLAYERS_FINISHED)]
    public void OnAllPlayersFinished(object sender)
    {
        _gameplayTimer.ForceComplete();
    }
}
