using ScoredProductions.StreamLinked.IRC.Tags;
using ScoredProductions.StreamLinked.IRC;
using System;
using UnityEngine;

public static class TwitchMessageExtensions
{
    /// <summary>Gets the Twitch user ID if available, or 0.</summary>
    public static long GetUserId(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv) ? priv.user_id : 0L;

    /// <summary>Gets the display name (case-corrected) or raw username.</summary>
    public static string GetDisplayName(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv && !string.IsNullOrEmpty(priv.display_name))
            ? priv.display_name : msg.Username;

    /// <summary>Gets the user’s chat colour or clear if none.</summary>
    public static Color GetNameColor(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv && ColorUtility.TryParseHtmlString(priv.color, out var c))
            ? c : Color.white;

    /// <summary>True if the user is a moderator.</summary>
    public static bool IsModerator(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv) && priv.mod;

    /// <summary>True if the user is a subscriber.</summary>
    public static bool IsSubscriber(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv) && priv.subscriber;

    /// <summary>True if the user is a VIP.</summary>
    public static bool IsVip(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv) && priv.vip;

    /// <summary>True if the user has turbo.</summary>
    public static bool IsTurbo(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv) && priv.turbo;

    /// <summary>Gets the raw chat text.</summary>
    public static string GetMessageText(this TwitchMessage msg)
        => msg.ChatMessage;

    /// <summary>Gets the array of emote positions, or empty.</summary>
    public static EmotePosition[] GetEmotes(this TwitchMessage msg)
        => (msg.ProcessedTags is PRIVMSG priv && priv.emotes != null)
            ? priv.emotes : Array.Empty<EmotePosition>();

    /// <summary>Gets badge names the user has in this message.</summary>
    public static string[] GetBadgeNames(this TwitchMessage msg)
    {
        msg.GetBadgeNames(out _);
        return msg.GetBadgeNames(out _);
    }
}