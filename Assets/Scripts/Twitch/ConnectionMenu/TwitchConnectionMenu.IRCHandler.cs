using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.Users;
using UnityEngine;

/// <summary>
/// IRC handler partial class - handles all Twitch IRC connection logic
/// </summary>
public partial class TwitchConnectionMenu
{
    /// <summary>
    /// Handles IRC connection button click and flow
    /// </summary>
    private void HandleIRCConnection()
    {
        if (!GetIsAuthenticated())
        {
            Debug.LogWarning("Cannot connect to IRC - not authenticated with Twitch!");
            return;
        }

        if (GetIsConnectingToIRC())
        {
            Debug.LogWarning("IRC connection already in progress, please wait...");
            return;
        }

        if (GetIsConnectedToIRC())
        {
            DisconnectFromIRC();
        }
        else
        {
            ConnectToIRC();
        }
    }

    /// <summary>
    /// Connects to Twitch IRC chat
    /// </summary>
    private void ConnectToIRC()
    {
        Debug.Log("Connecting to Twitch IRC...");

        if (_autoSetChannelToUser)
        {
            SetChannelToAuthenticatedUser();
        }

        // Validate we have a channel target
        if (string.IsNullOrWhiteSpace(_ircClient.TwitchTarget))
        {
            Debug.LogError("Cannot connect to IRC - no channel target set!");
            return;
        }

        // Enable IRC connection
        _ircClient.IRCEnabled = true;

        Debug.Log($"IRC connection initiated for channel: {_ircClient.TwitchTarget}");
    }

    /// <summary>
    /// Disconnects from Twitch IRC chat
    /// </summary>
    private void DisconnectFromIRC()
    {
        Debug.Log("Disconnecting from Twitch IRC...");

        if (_ircClient != null)
        {
            _ircClient.IRCEnabled = false;
        }
    }

    /// <summary>
    /// Sets IRC channel target to the authenticated user's channel
    /// </summary>
    private void SetChannelToAuthenticatedUser()
    {
        try
        {
            var userResponse = TwitchAPIClient.MakeTwitchAPIRequest<GetUsers>(5000);
            if (!userResponse.HasErrored && userResponse.data.Length > 0)
            {
                string username = userResponse.data[0].login;
                _ircClient.TwitchTarget = username;

                Debug.Log($"Set IRC channel to authenticated user: {username}");
            }
            else
            {
                Debug.LogError("Failed to get authenticated user info for IRC channel");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error setting IRC channel to authenticated user: {ex.Message}");
        }
    }

    /// <summary>
    /// Sets IRC channel target manually
    /// </summary>
    public void SetIRCChannel(string channelName)
    {
        if (_ircClient == null)
        {
            Debug.LogError("IRC Client not available!");
            return;
        }

        if (string.IsNullOrWhiteSpace(channelName))
        {
            Debug.LogError("Channel name cannot be empty!");
            return;
        }

        _ircClient.TwitchTarget = channelName.ToLower().Trim();
        Debug.Log($"IRC channel set to: {_ircClient.TwitchTarget}");
    }

    /// <summary>
    /// Forces IRC connection - public method for external use
    /// </summary>
    public void ForceConnectToIRC()
    {
        if (GetIsAuthenticated())
        {
            ConnectToIRC();
        }
        else
        {
            Debug.LogWarning("Cannot force connect to IRC - not authenticated!");
        }
    }

    /// <summary>
    /// Forces IRC disconnection - public method for external use
    /// </summary>
    public void ForceDisconnectFromIRC()
    {
        DisconnectFromIRC();
    }

    /// <summary>
    /// Gets IRC connection button text based on current state
    /// </summary>
    public string GetIRCButtonText()
    {
        if (!GetIsAuthenticated())
            return "Not Authenticated";
        else if (GetIsConnectingToIRC())
            return "Connecting...";
        else if (GetIsConnectedToIRC())
            return "Disconnect from Chat";
        else
            return "Connect to Chat";
    }

    /// <summary>
    /// Sends a message to the connected IRC channel
    /// </summary>
    public void SendChatMessage(string message)
    {
        if (!GetIsConnectedToIRC())
        {
            Debug.LogWarning("Cannot send chat message - not connected to IRC!");
            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Debug.LogWarning("Cannot send empty chat message!");
            return;
        }

        _ircClient.SendMessageToChat(message);
        Debug.Log($"Sent chat message: {message}");
    }

    /// <summary>
    /// Gets detailed IRC status for debugging
    /// </summary>
    public string GetIRCDebugInfo()
    {
        if (_ircClient == null)
            return "IRC Client not available";

        var status = new System.Text.StringBuilder();
        status.AppendLine($"IRC Enabled: {_ircClient.IRCEnabled}");
        status.AppendLine($"Is Connected: {GetIsConnectedToIRC()}");
        status.AppendLine($"Is Connecting: {GetIsConnectingToIRC()}");
        status.AppendLine($"Target Channel: {GetCurrentChannel()}");
        status.AppendLine($"Connected Channel: {_ircClient.ConnectedChannelOnSocket ?? "None"}");
        status.AppendLine($"Auto Set Channel: {_autoSetChannelToUser}");

        return status.ToString();
    }

    /// <summary>
    /// Checks if IRC can connect (authenticated and has target)
    /// </summary>
    public bool CanConnectToIRC()
    {
        return GetIsAuthenticated() &&
               !string.IsNullOrWhiteSpace(_ircClient?.TwitchTarget) &&
               !GetIsConnectingToIRC();
    }
}
