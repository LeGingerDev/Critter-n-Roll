using Core;
using Core.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDisplayController : BaseBehaviour
{
    [SerializeField]
    private PlayerDisplay _playerDisplayPrefab;
    private List<PlayerDisplay> _playerDisplays = new List<PlayerDisplay>();


    [Topic(PlayerManagementEventIds.ON_PLAYER_JOINED)]
    public void OnPlayerJoined(object sender, PlayerUserData player)
    {
        StartCoroutine(WaitToUpdateJoin());
    }
    public IEnumerator WaitToUpdateJoin()
    {
        yield return new WaitForEndOfFrame();
        List<PlayerUserData> players = ActivePlayerManager.Instance.GetActivePlayers().ToList();
        players.ForEach(p => 
        CreatePlayer(p));
    }

    public IEnumerator WaitToUpdateLeave(PlayerUserData player)
    {
        yield return new WaitForEndOfFrame();

        if (!_playerDisplays.Any(p => p.PlayerUserData.ChatData.GetUserId() == player.ChatData.GetUserId()))
            yield break;

        PlayerDisplay displayToRemove = _playerDisplays.FirstOrDefault(p => p.PlayerUserData.ChatData.GetUserId() == player.ChatData.GetUserId());
        if (displayToRemove != null)
        {
            _playerDisplays.Remove(displayToRemove);
            Destroy(displayToRemove.gameObject);
        }
    }

    [Topic(PlayerManagementEventIds.ON_PLAYERS_CLEARED)]
    public void OnPlayersCleared(object sender)
    {
        Clear();
        StartCoroutine(WaitToUpdateJoin());
    }


    [Topic(PlayerManagementEventIds.ON_PLAYER_LEFT)]
    public void OnPlayerLeft(object sender, PlayerUserData player)
    {
        StartCoroutine(WaitToUpdateLeave(player));
    }

    public void CreatePlayer(PlayerUserData playerUserData)
    {
        if(_playerDisplays.Any(p => p.PlayerUserData.ChatData.GetUserId() == playerUserData.ChatData.GetUserId()))
        {
            Debug.LogWarning($"Player {playerUserData.ChatData.GetDisplayName()} already exists in the display list.");
            return;
        }

        PlayerDisplay newDisplay = Instantiate(_playerDisplayPrefab, transform);
        newDisplay.Initialise(playerUserData);
        _playerDisplays.Add(newDisplay);
    }

    public void Clear()
    {
        foreach (var display in _playerDisplays)
        {
            Destroy(display.gameObject);
        }
        _playerDisplays.Clear();
    }
}
