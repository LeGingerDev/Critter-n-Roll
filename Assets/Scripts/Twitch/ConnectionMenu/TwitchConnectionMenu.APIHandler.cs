using UnityEngine;

/// <summary>
/// Authentication handler partial class - handles all Twitch authentication logic
/// </summary>
public partial class TwitchConnectionMenu
{
    /// <summary>
    /// Handles authentication button click and flow
    /// </summary>
    private void HandleAuthentication()
    {
        if (IsCurrentlyAuthenticating())
        {
            Debug.LogWarning("Authentication already in progress, please wait...");
            return;
        }

        if (GetIsAuthenticated())
        {
            HandleReAuthentication();
        }
        else
        {
            StartAuthentication();
        }
    }

    /// <summary>
    /// Starts fresh authentication process
    /// </summary>
    private void StartAuthentication()
    {
        Debug.Log("Starting Twitch authentication...");

        if (_clearTokensOnAuth)
        {
            ClearStoredTokens();
        }

        // Ensure IRC is disconnected during auth
        DisconnectIRC();

        // Start authentication process
        RequestNewAuthToken();
    }

    /// <summary>
    /// Handles re-authentication when already authenticated
    /// </summary>
    private void HandleReAuthentication()
    {
        Debug.Log("Re-authenticating with Twitch...");

        // Always clear tokens for re-auth
        ClearStoredTokens();

        // Disconnect IRC first
        DisconnectIRC();

        // Start fresh auth
        RequestNewAuthToken();
    }

    /// <summary>
    /// Clears stored authentication tokens
    /// </summary>
    private void ClearStoredTokens()
    {
        if (_apiClient == null) return;

        _apiClient.CleanPlayerPrefTokens();
        Debug.Log("Cleared stored authentication tokens");
    }

    /// <summary>
    /// Requests new authentication token from Twitch
    /// </summary>
    private void RequestNewAuthToken()
    {
        if (_apiClient == null)
        {
            Debug.LogError("TwitchAPIClient not available for authentication!");
            return;
        }

        // Cancel any existing requests
        _apiClient.CancelAPIRequestsAndReset();

        // Request new token
        _apiClient.GetNewAuthToken(_apiClient.DefaultAPIToken);

        Debug.Log("Authentication request sent - check browser for Twitch login");
    }

    /// <summary>
    /// Disconnects IRC during authentication
    /// </summary>
    private void DisconnectIRC()
    {
        if (_ircClient != null && _ircClient.IRCEnabled)
        {
            _ircClient.IRCEnabled = false;
            Debug.Log("Disconnected from IRC for authentication");
        }
    }

    /// <summary>
    /// Forces authentication process - public method for external use
    /// </summary>
    public void ForceAuthentication()
    {
        HandleAuthentication();
    }

    /// <summary>
    /// Clears all authentication data - public method for external use
    /// </summary>
    public void ClearAuthenticationData()
    {
        Debug.Log("Clearing all authentication data...");

        DisconnectIRC();
        ClearStoredTokens();

        if (_apiClient != null)
        {
            _apiClient.CancelAPIRequestsAndReset();
        }

        Debug.Log("Authentication data cleared");
    }

    /// <summary>
    /// Checks if user can authenticate (has required settings)
    /// </summary>
    public bool CanAuthenticate()
    {
        if (_apiClient == null) return false;

        return _apiClient.HasSettingsToGetOAuth();
    }

    /// <summary>
    /// Gets authentication button text based on current state
    /// </summary>
    public string GetAuthButtonText()
    {
        if (IsCurrentlyAuthenticating())
            return "Authenticating...";
        else if (GetIsAuthenticated())
            return "Re-authenticate";
        else
            return "Authenticate with Twitch";
    }

    /// <summary>
    /// Gets detailed authentication status for debugging
    /// </summary>
    public string GetAuthenticationDebugInfo()
    {
        if (_apiClient == null)
            return "API Client not available";

        var status = new System.Text.StringBuilder();
        status.AppendLine($"Can Authenticate: {CanAuthenticate()}");
        status.AppendLine($"Is Authenticated: {GetIsAuthenticated()}");
        status.AppendLine($"Getting New Token: {IsCurrentlyAuthenticating()}");
        status.AppendLine($"Has Client ID: {!string.IsNullOrEmpty(_apiClient.TwitchClientID)}");
        status.AppendLine($"Has Client Secret: {!string.IsNullOrEmpty(_apiClient.TwitchSecret)}");

        return status.ToString();
    }
}