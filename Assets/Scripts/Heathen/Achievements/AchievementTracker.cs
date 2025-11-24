using Core.Events;
using Core.Singleton;
using Heathen.SteamworksIntegration;

public class AchievementTracker : MonoSingleton<AchievementTracker>
{
    public StatObject concurrentPlayerCount;
    public StatObject totalStoredPlayerCount;
    public StatObject NumberOfTimesOpenedCount;
    public StatObject TotalGamesPlayedStat;

    [Topic(AchievementEventIds.TOTAL_NUMBER_OF_PLAYERS_UPDATED)]
    public void TotalNumberOfPlayersInGameUpdated(object sender, int totalNumber)
    {
        totalStoredPlayerCount.SetIntStat(totalNumber);
    }

    [Topic(AchievementEventIds.NUMBER_OF_TIMES_OPENED_GAME_UPDATED)]
    public void NumberOfTimesGameOpenedUpdated(object sender, int ignore)
    {
        NumberOfTimesOpenedCount.AddIntStat(1);
    }

    [Topic(AchievementEventIds.NUMBER_OF_PLAYERS_IN_GAME_CONCURRENT_UPDATED)]
    public void NumberOfPlayersInGameConcurrentUpdated(object sender, int totalConcurrentPlayers)
    {
        concurrentPlayerCount.SetIntStat(totalConcurrentPlayers);
    }

    [Topic(AchievementEventIds.GAMES_PLAYED_STAT_ADDED)]
    public void GamesPlayedStatAdded(object sender, int ignore)
    {
        TotalGamesPlayedStat.AddIntStat(1);
    }

    public void Store()
    {
        concurrentPlayerCount.StoreStats();
    }
}
