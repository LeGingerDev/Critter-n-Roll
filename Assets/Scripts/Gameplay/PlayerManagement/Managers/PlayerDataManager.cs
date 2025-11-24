using Core.Singleton;
using NUnit.Framework;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Global repository of every player that has ever registered.
/// Automatically loads on awake and saves when new players are added.
/// </summary>
public class PlayerDataManager : MonoSingleton<PlayerDataManager>
{
    [FoldoutGroup("Players")]
    [SerializeField]
    private List<PlayerUserData> _playerRecords = new List<PlayerUserData>();

    private const string SaveKey = "playerRecords";

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    /// <summary>
    /// Register a player via incoming IRC message. Returns existing or new data.
    /// If new, automatically saves full list to disk.
    /// </summary>
    public PlayerUserData RegisterPlayer(TwitchMessage msg)
    {
        var chatData = new ChatUserData(msg);
        long userId = chatData.GetUserId();

        // find existing by ID
        var existing = _playerRecords.FirstOrDefault(p => p.ChatData.GetUserId() == userId);
        if (existing != null)
        {
            existing.ChatData.RegisterMessage();
            return existing;
        }

        // create new
        var newPlayer = new PlayerUserData(chatData);
        _playerRecords.Add(newPlayer);
        SaveData(); // auto-save on new registration
        return newPlayer;
    }

    /// <summary>Get all registered players.</summary>
    public IReadOnlyList<PlayerUserData> GetAllPlayers() => _playerRecords;

    /// <summary>Find by Twitch user ID.</summary>
    public PlayerUserData GetByUserId(long userId)
        => _playerRecords.FirstOrDefault(p => p.ChatData.GetUserId() == userId);

    /// <summary>Try get player by ID.</summary>
    public bool TryGetByUserId(long userId, out PlayerUserData player)
    {
        player = GetByUserId(userId);
        return player != null;
    }

    /// <summary>Remove a player by ID.</summary>
    public bool RemoveByUserId(long userId)
    {
        bool removed = _playerRecords.RemoveAll(p => p.ChatData.GetUserId() == userId) > 0;
        if (removed) SaveData();
        return removed;
    }

    /// <summary>Clear all player data.</summary>
    [Button]
    public void ClearAll()
    {
        _playerRecords.Clear();
        SaveData();
    }

    /// <summary>Save the full playerRecords list with Easy Save.</summary>
    [Button]
    public void SaveData()
    {
        ES3.Save(SaveKey, _playerRecords);
        Debug.Log("PlayerDataManager: saved " + _playerRecords.Count + " records.");
    }

    /// <summary>Load the playerRecords list from Easy Save if present.</summary>
    [Button]
    public void LoadData()
    {
        if (ES3.KeyExists(SaveKey))
        {
            _playerRecords = ES3.Load<List<PlayerUserData>>(SaveKey);
            Debug.Log("PlayerDataManager: loaded " + _playerRecords.Count + " records.");
        }
    }
}
