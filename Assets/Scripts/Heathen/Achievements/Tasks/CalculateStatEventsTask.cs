using System.Collections;
using Tasks;
using UnityEngine;

public class CalculateStatEventsTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        Publish(AchievementEventIds.GAMES_PLAYED_STAT_ADDED, 1);
        Publish(AchievementEventIds.TOTAL_NUMBER_OF_PLAYERS_UPDATED, PlayerDataManager.Instance.GetAllPlayers().Count);
        Publish(AchievementEventIds.NUMBER_OF_PLAYERS_IN_GAME_CONCURRENT_UPDATED, ActivePlayerManager.Instance.GetActivePlayers().Count);
        AchievementTracker.Instance.Store();
        yield return null;
    }
}
