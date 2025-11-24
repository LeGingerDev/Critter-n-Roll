// TwitchCommandManager.cs
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Core.Singleton;
using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.API;

public class TwitchCommandManager : MonoSingleton<TwitchCommandManager>
{
    private TwitchIRCClient _ircClient;

    private readonly Dictionary<string, (IChatCommand handler, bool isGlobal)> _commands
        = new Dictionary<string, (IChatCommand, bool)>();

    private readonly List<ITwitchCommandListener> _listeners = new();

    protected override void Awake()
    {
        base.Awake();
        RegisterAllCommands();

        if (TwitchIRCClient.CreateOrGetInstance(out this._ircClient))
        {
            _ircClient.OnPRIVMSG.AddListener(OnChatMessageReceived);
        }
        else
        {
            Debug.LogError("Failed to create or get TwitchIRCClient instance.");
            enabled = false; // Disable this manager if IRC client can't be initialized

        }
    }
    private void Start()
    {
        // Subscribe to auth events to re-hook IRC when it reconnects
        if (TwitchAPIClient.GetInstance(out var apiClient))
        {
            // TODO: Replace with your Topic System
            apiClient.OnAuthenticationSuccess.AddListener(ResubscribeToIRC);
        }
    }

    private void ResubscribeToIRC()
    {
        // Re-subscribe to IRC events after authentication/reconnection
        if (TwitchIRCClient.CreateOrGetInstance(out this._ircClient))
        {
            // Remove any existing listener first to avoid duplicates
            _ircClient.OnPRIVMSG.RemoveListener(OnChatMessageReceived);

            // Re-add the listener
            _ircClient.OnPRIVMSG.AddListener(OnChatMessageReceived);

            Debug.Log("[TwitchCmd] Re-subscribed to IRC events after authentication");
        }
    }

    private void RegisterAllCommands()
    {
        var asm = Assembly.GetExecutingAssembly();
        foreach (var type in asm.GetTypes())
        {
            var attr = type.GetCustomAttribute<ChatCommandAttribute>();
            if (attr != null && typeof(IChatCommand).IsAssignableFrom(type))
            {
                var instance = (IChatCommand)Activator.CreateInstance(type);
                _commands[attr.CommandName] = (instance, attr.IsGlobal);
                Debug.Log($"[TwitchCmd] Registered '{attr.CommandName}' (global={attr.IsGlobal})");
            }
        }
    }

    public void RegisterListener(ITwitchCommandListener l)
    {
        if (!_listeners.Contains(l)) _listeners.Add(l);
    }
    public void UnregisterListener(ITwitchCommandListener l)
    {
        _listeners.Remove(l);
    }

    //TODO: Consider making this queue commands or something to ensure buffering and stuff.
    public void OnChatMessageReceived(TwitchMessage msg)
    {
        // Trim only trailing CR/LF so the command key isn’t polluted
        string text = msg.ChatMessage?.TrimEnd('\r', '\n');
        if (string.IsNullOrEmpty(text) || text[0] != '!')
            return;

        // Remove the '!' prefix and trim any leftover whitespace
        string body = text.Substring(1).Trim();
        var parts = body.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Trim the key token before lookup
        string key = parts[0].Trim().ToLower();

        string argsText = parts.Length > 1
            ? string.Join(" ", parts.Skip(1))
            : string.Empty;

        if (!_commands.TryGetValue(key, out var entry))
        {
            Debug.LogWarning($"Unknown Twitch command '{key}'");
            return;
        }

        var (handler, isGlobal) = entry;
        var parsed = handler.ParseArguments(argsText);

        foreach (var listener in _listeners.ToList())
        {
            if (isGlobal || listener.CanHandleUser(msg))
                listener.HandleCommand(key, parsed, msg.Username, msg);
        }
    }

    private void OnDestroy()
    {
        // Clean up auth event subscription too
        if (TwitchAPIClient.GetInstance(out var apiClient))
        {
            apiClient.OnAuthenticationSuccess.RemoveListener(ResubscribeToIRC);
        }

        if (_ircClient != null)
        {
            _ircClient.OnPRIVMSG.RemoveListener(OnChatMessageReceived);
        }
    }

}
