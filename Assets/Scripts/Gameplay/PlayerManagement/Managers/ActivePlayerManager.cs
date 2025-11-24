using Core.Singleton;
using General.Utility;
using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.IRC.Tags;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Tracks only the players currently in the live game session.
/// Uses user ID to map data and instances.
/// </summary>
public class ActivePlayerManager : MonoSingleton<ActivePlayerManager>
{
    [FoldoutGroup("Active Players"), SerializeField]
    private int _maxPlayers = 20;

    [FoldoutGroup("Active Players"), SerializeField]
    private GameObject _playerPrefab;

    // maps userId -> (PlayerUserData, instantiated GameObject)
    private Dictionary<long, (PlayerUserData data, GameObject instance)> _activePlayers
        = new Dictionary<long, (PlayerUserData, GameObject)>();

    public List<PlayerUserData> ActivePlayersDebug = new List<PlayerUserData>();

    private ISpawnPositioner _spawnPositioner;

    /// <summary>Allow setting the prefab from another script.</summary>
    public void SetPlayerPrefab(GameObject prefab)
    {
        _playerPrefab = prefab;
    }

    /// <summary>
    /// Spawns a new player into the game session if not already present.
    /// </summary>
    public void JoinPlayer(TwitchMessage msg)
    {
        if (_activePlayers.Count >= _maxPlayers)
            return;

        var userData = PlayerDataManager.Instance.RegisterPlayer(msg);
        long userId = userData.ChatData.GetUserId();

        if (_activePlayers.ContainsKey(userId))
        {
            Debug.LogWarning("User " + userId + " is already in-game!");
            return;
        }

        _spawnPositioner = UtilityFuncs.FindInterfaceInScene<ISpawnPositioner>();

        // instantiate prefab and initialize views
        var go = Instantiate(_playerPrefab, _spawnPositioner.GetSpawnPosition(), Quaternion.identity);
        foreach (var view in go.GetComponentsInChildren<IPlayerView>())
        {
            view.Initialize(userData);
        }

        // track active
        _activePlayers.Add(userId, (userData, go));
        ActivePlayersDebug.Add(userData);

        Publish(PlayerManagementEventIds.ON_PLAYER_JOINED, userData);
    }

    /// <summary>
    /// Removes a player from the current session and destroys their GameObject.
    /// </summary>
    public bool LeavePlayer(long userId)
    {
        if (!_activePlayers.TryGetValue(userId, out var tuple))
            return false;

        //TODO: In future add some polish to this instead of just destroying
        _activePlayers.Remove(userId);
        ActivePlayersDebug.Remove(tuple.data);
        Publish(PlayerManagementEventIds.ON_PLAYER_LEFT, tuple.data);
        Destroy(tuple.instance);
        return true;
    }

    /// <summary>Gets all active players' data.</summary>
    public IReadOnlyList<PlayerUserData> GetActivePlayers()
        => _activePlayers.Values.Select(t => t.data).ToList();

    /// <summary>Clears all active players and destroys their GameObjects.</summary>
    [Button]
    public void ClearAllActive()
    {
        foreach (var tuple in _activePlayers.Values)
            Destroy(tuple.instance);
        _activePlayers.Clear();
        Publish(PlayerManagementEventIds.ON_PLAYERS_CLEARED);
    }

    public int GetTotalPlayers()
    {
        return _activePlayers.Count;
    }
    public bool IsPlayerActive(long userId)
    {
        return _activePlayers.ContainsKey(userId);
    }
    public List<GameObject> GetAllPlayerObjects() => _activePlayers.Values.Select(t => t.instance).ToList();
    public GameObject GetPlayerInstance(long userId)
    {
        return _activePlayers.TryGetValue(userId, out var tuple) ? tuple.instance : null;
    }

    public GameObject GetPlayerInstance(string displayName)
    {
        PlayerUserData player = GetActivePlayers().FirstOrDefault(p => p.ChatData.GetDisplayName() == displayName);
        if (player == null)
        {
            Debug.LogWarning($"No active player found with username: {displayName}");
            return null;
        }
        return GetPlayerInstance(player.ChatData.GetUserId());
    }
    public int GetMaxPlayerCount()
    {
        return _maxPlayers;
    }

    public HashSet<PlayerUserData> GetAllActivePlayersExcept(HashSet<PlayerController> exclude)
    {
        var excludeIds = exclude.Select(p => p.PlayerData.ChatData.GetUserId()).ToHashSet();
        return _activePlayers.Values
            .Where(t => !excludeIds.Contains(t.data.ChatData.GetUserId()))
            .Select(t => t.data)
            .ToHashSet();
    }


    public bool IsAPlayer(TwitchMessage twitchMessage)
    {
        if (twitchMessage.ProcessedTags is PRIVMSG priv)
        {
            return _activePlayers.ContainsKey(priv.user_id);
        }
        return false;
    }
}

