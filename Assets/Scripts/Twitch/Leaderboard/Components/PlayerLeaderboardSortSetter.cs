using UnityEngine;

public class PlayerLeaderboardSortSetter : MonoBehaviour
{
    public PlayerLeaderboardMenu.PlayerSortType sortType;

    public void SetSortType()
    {
        GetComponentInParent<PlayerLeaderboardMenu>().SetSortType(sortType);
    }
}