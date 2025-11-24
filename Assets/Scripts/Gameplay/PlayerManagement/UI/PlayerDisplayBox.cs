using Core;
using Core.Events;
using TMPro;
using UnityEngine;

public class PlayerDisplayBox : BaseBehaviour
{
    private TextMeshProUGUI _playerCountText;

    private void Awake()
    {
        _playerCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _playerCountText.text = $"Players 0/{ActivePlayerManager.Instance.GetMaxPlayerCount()}";
    }

    [Topic(PlayerManagementEventIds.ON_PLAYER_JOINED)]
    [Topic(PlayerManagementEventIds.ON_PLAYER_LEFT)]
    public void UpdatePlayerCount(object sender, PlayerUserData player)
    {
        int currentCount = ActivePlayerManager.Instance.GetTotalPlayers();
        int maxCount = ActivePlayerManager.Instance.GetMaxPlayerCount();
        _playerCountText.text = $"Players {currentCount}/{maxCount}";
    }
}
