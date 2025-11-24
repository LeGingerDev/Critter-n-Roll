using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.IRC.Tags;
using Sirenix.OdinInspector;
using System;

using UnityEngine;

/// <summary>
/// Updated PlayerUserData to only store XP and derive level.
/// </summary>
[Serializable]
public class PlayerUserData
{
    [FoldoutGroup("Chat Info")]
    [SerializeField] private ChatUserData _chatData;

    [FoldoutGroup("Game Stats")]
    [SerializeField] private int _xp;

    [FoldoutGroup("Game Stats")]
    [SerializeField] private int _numberOfLevelsPlayed;

    [FoldoutGroup("Game Stats")]
    [SerializeField] private int _numberOfLevelsFinished;

    [FoldoutGroup("Aesthetics")]
    [SerializeField] private string _customisationId;

    public PlayerUserData(ChatUserData chatData)
    {
        _chatData = chatData;
        _xp = 0;
    }

    // Expose chat data if needed
    public ChatUserData ChatData => _chatData;

    // Game-specific logic
    public void AddXp(int amount)
    {
        _xp += amount;
    }

    public void AddGamePlayed() => _numberOfLevelsPlayed++;
    public void AddGameFinished() => _numberOfLevelsFinished++;

    public void SetCustomisationOption(string id)
    {
        _customisationId = id;
    }

    // Derived getters - no stored level anymore
    public int GetLevel()
    {
        return LevelUpManager.Instance.GetLevelFromExperience(_xp);
    }

    public float GetWinRatePercentage() => (_numberOfLevelsPlayed == 0) ? 0f : ((float)_numberOfLevelsFinished / _numberOfLevelsPlayed) * 100f;

    public int GetXp() => _xp;
    public int GetNumberOfLevelsPlayed() => _numberOfLevelsPlayed;
    public int GetNumberOfLevelsFinished() => _numberOfLevelsFinished;
    public string GetCustomisationId() => _customisationId;
}


/// <summary>
/// Core chat user data (transport?agnostic).
/// </summary>
[Serializable]
public class ChatUserData
{
    [FoldoutGroup("Identity")]
    [SerializeField] private long _userId;
    [FoldoutGroup("Identity")]
    [SerializeField] private string _loginName;
    [FoldoutGroup("Identity")]
    [SerializeField] private string _displayName;

    [FoldoutGroup("Chat Settings")]
    [SerializeField] private Color _nameColor;

    [FoldoutGroup("Chat Status")]
    [SerializeField] private bool _isModerator;
    [FoldoutGroup("Chat Status")]
    [SerializeField] private bool _isSubscriber;
    [FoldoutGroup("Chat Status")]
    [SerializeField] private bool _isVip;
    [FoldoutGroup("Chat Status")]
    [SerializeField] private bool _isTurbo;

    [FoldoutGroup("Activity")]
    [SerializeField] private int _messageCount;
    [FoldoutGroup("Activity")]
    [SerializeField] private DateTime _firstSeen;
    [FoldoutGroup("Activity")]
    [SerializeField] private DateTime _lastSeen;

    public ChatUserData(TwitchMessage msg)
    {
        // Only PRIVMSG contains actual user data
        if (msg.ProcessedTags is PRIVMSG priv)
        {
            _userId = priv.user_id;
            _loginName = msg.Username.ToLowerInvariant();
            _displayName = !string.IsNullOrEmpty(priv.display_name)
                            ? priv.display_name
                            : msg.Username;

            // Color
            _nameColor = Color.clear;
            ColorUtility.TryParseHtmlString(priv.color, out _nameColor);

            // Flags
            _isModerator = priv.mod;
            _isSubscriber = priv.subscriber;
            _isVip = priv.vip;
            _isTurbo = priv.turbo;
        }
        else
        {
            // Defaults if not a PRIVMSG
            _userId = 0L;
            _loginName = msg.Username.ToLowerInvariant();
            _displayName = msg.Username;
            _nameColor = Color.clear;
            _isModerator = false;
            _isSubscriber = false;
            _isVip = false;
            _isTurbo = false;
        }

        // Initialize activity
        _messageCount = 1;
        _firstSeen = _lastSeen = DateTime.UtcNow;
    }

    /// <summary>Call on each new chat message.</summary>
    public void RegisterMessage()
    {
        _messageCount++;
        _lastSeen = DateTime.UtcNow;
    }

    // Exposed getters
    public long GetUserId() => _userId;
    public string GetLoginName() => _loginName;
    public string GetDisplayName() => _displayName;
    public Color GetNameColor() => _nameColor;
    public bool IsModerator() => _isModerator;
    public bool IsSubscriber() => _isSubscriber;
    public bool IsVip() => _isVip;
    public bool IsTurbo() => _isTurbo;
    public int GetMessageCount() => _messageCount;
    public DateTime GetFirstSeen() => _firstSeen;
    public DateTime GetLastSeen() => _lastSeen;
}

