using ScoredProductions.StreamLinked.IRC;
using UnityEngine;

public class TwitchPlayerSpawnManager : TwitchBaseListener
{
    [Tooltip("Prefab must have a PlayerController (subclass of TwitchBaseListener)")]
    [SerializeField]
    private GameObject _playerPrefab;

    public void Awake()
    {
        // Ensure ActivePlayerManager has the correct prefab
        ActivePlayerManager.Instance.SetPlayerPrefab(_playerPrefab);
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        switch (commandKey.ToLowerInvariant())
        {
            case "join":
                // Spawn or retrieve existing
                ActivePlayerManager.Instance.JoinPlayer(msg);
                break;

            case "leave":
                // Remove the player by their Twitch user ID
                var chatData = new ChatUserData(msg);
                long userId = chatData.GetUserId();
                bool removed = ActivePlayerManager.Instance.LeavePlayer(userId);
                if (!removed)
                    Debug.LogWarning($"Could not remove user {userId}; not currently in-game.");
                break;

            default:
                // Other commands not handled here
                break;
        }
    }
}

