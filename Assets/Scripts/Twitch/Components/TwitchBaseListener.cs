using Core;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class TwitchBaseListener : BaseBehaviour, ITwitchCommandListener
{
    [FoldoutGroup("Base"), SerializeField] private PlayerUserData _playerData;
    [FoldoutGroup("Base"), SerializeField] private bool _isGlobalListener;

    public PlayerUserData PlayerData => _playerData;
    public string Username => _playerData?.ChatData.GetLoginName() ?? string.Empty;

    protected virtual void OnEnable()
    {
        base.OnEnable();

        if (_isGlobalListener)
            InitializeForUser();
    }

    /// <summary>
    /// Call this once immediately after instantiating the prefab.
    /// Grabs (or creates) the PlayerUserData, wires up views, and registers for chat.
    /// </summary>
    public virtual void InitializeForUser(PlayerUserData data)
    {
        TwitchCommandManager.Instance?.UnregisterListener(this);
        _playerData = data ?? throw new ArgumentNullException(nameof(data));
        gameObject.name = $"Player_{data.ChatData.GetLoginName()}";
        foreach (var v in GetComponentsInChildren<IPlayerView>()) v.Initialize(data);
        TwitchCommandManager.Instance?.RegisterListener(this);
    }

    public virtual void InitializeForUser()
    {
        TwitchCommandManager.Instance?.UnregisterListener(this);
        _playerData = null;
        gameObject.name = name; // or “GlobalListener”…
        foreach (var v in GetComponentsInChildren<IPlayerView>()) v.Initialize(null);
        TwitchCommandManager.Instance?.RegisterListener(this);
    }

    protected virtual void OnDisable()
    {
        base.OnDisable();
        TwitchCommandManager.Instance?.UnregisterListener(this);
    }

    // Only handles messages from its own user unless global
    public bool CanHandleUser(TwitchMessage msg)
    {
        if (_isGlobalListener)
            return true;

        return PlayerData.ChatData.GetUserId() == msg.GetUserId();
    }

    public abstract void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg);
}