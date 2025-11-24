// Concrete leaderboard menu implementation for PlayerUserData
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLeaderboardMenu : LeaderboardMenu<PlayerUserData>
{
    public enum PlayerSortType
    {
        XP,
        LevelsPlayed,
        WinRate
    }

    public PlayerSortType CurrentSortType = PlayerSortType.XP;

    public void SetSortType(PlayerSortType sortType)
    {
        CurrentSortType = sortType;
        RefreshLeaderboard();
    }

    // Public method to refresh the leaderboard data
    public void RefreshLeaderboard()
    {
        SetupLeaderboard();
    }

    public override List<PlayerUserData> GetLeaderboardEntries()
    {
        List<PlayerUserData> players = PlayerDataManager.Instance.GetAllPlayers().ToList();

        switch (CurrentSortType)
        {
            case PlayerSortType.LevelsPlayed:
                return players.OrderByDescending(p => p.GetNumberOfLevelsPlayed()).ToList();
            case PlayerSortType.WinRate:
                return players.OrderByDescending(p => p.GetWinRatePercentage()).ToList();
            case PlayerSortType.XP:
                return players.OrderByDescending(p => p.GetXp()).ToList();
        }

        // Default to sorting by XP
        return players.OrderByDescending(p => p.GetXp()).ToList();
    }
}


