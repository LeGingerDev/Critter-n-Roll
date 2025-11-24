using UnityEngine;

/// <summary>
/// Display monitor partial class - handles all status display logic
/// </summary>
public partial class TwitchConnectionMenu
{
    /// <summary>
    /// Updates all display elements based on current state
    /// </summary>
    private void UpdateDisplay()
    {
        UpdateDebugInfo();
        UpdateStatusText();
        UpdateStatusIndicator();
        UpdatePanelVisibility();
        UpdateChannelDisplay();
        UpdateButtonStates();
        UpdateLoadingIndicators();
    }

    /// <summary>
    /// Updates debug information shown in inspector
    /// </summary>
    private void UpdateDebugInfo()
    {
        _isAuthenticated = GetIsAuthenticated();
        _isConnectedToIRC = GetIsConnectedToIRC();
        _isConnectingToIRC = GetIsConnectingToIRC();
        _currentStatus = GetConnectionStatus();
        _currentChannel = GetCurrentChannel();
    }

    /// <summary>
    /// Updates main status text display
    /// </summary>
    private void UpdateStatusText()
    {
        if (_statusText == null) return;

        if (!_isAuthenticated)
        {
            _statusText.text = "Not Authenticated";
            _statusText.color = _disconnectedColor;
        }
        else if (_isConnectingToIRC)
        {
            _statusText.text = "Connecting to Chat...";
            _statusText.color = _authenticatedColor;
        }
        else if (_isConnectedToIRC)
        {
            _statusText.text = "Connected to Chat!";
            _statusText.color = _connectedColor;
        }
        else
        {
            _statusText.text = "Authenticated";
            _statusText.color = _authenticatedColor;
        }
    }

    /// <summary>
    /// Updates status indicator color
    /// </summary>
    private void UpdateStatusIndicator()
    {
        if (_statusIndicator == null) return;

        if (!_isAuthenticated)
        {
            _statusIndicator.color = _disconnectedColor;
        }
        else if (_isConnectingToIRC)
        {
            _statusIndicator.color = _authenticatedColor;
        }
        else if (_isConnectedToIRC)
        {
            _statusIndicator.color = _connectedColor;
        }
        else
        {
            _statusIndicator.color = _authenticatedColor;
        }
    }

    /// <summary>
    /// Shows/hides panels based on authentication state
    /// </summary>
    private void UpdatePanelVisibility()
    {
        if (_authenticatedPanel != null)
            _authenticatedPanel.SetActive(_isAuthenticated);

        if (_notAuthenticatedPanel != null)
            _notAuthenticatedPanel.SetActive(!_isAuthenticated);
    }

    /// <summary>
    /// Updates channel name display
    /// </summary>
    private void UpdateChannelDisplay()
    {
        if (_channelText == null) return;

        string channelName = GetCurrentChannel();
        if (string.IsNullOrEmpty(channelName) || channelName == "None")
        {
            _channelText.text = "Channel: Not Set";
        }
        else
        {
            _channelText.text = $"Channel: {channelName}";
        }
    }

    /// <summary>
    /// Updates button interactable states
    /// </summary>
    private void UpdateButtonStates()
    {
        // Auth button - always interactable unless currently authenticating
        if (_authButton != null)
        {
            bool isAuthenticating = IsCurrentlyAuthenticating();
            _authButton.interactable = !isAuthenticating;
        }

        // Connect button - only interactable when authenticated and not connecting
        if (_connectButton != null)
        {
            _connectButton.interactable = _isAuthenticated;
        }
    }

    /// <summary>
    /// Updates loading/connecting indicators
    /// </summary>
    private void UpdateLoadingIndicators()
    {
        if (_authenticatingIndicator != null)
            _authenticatingIndicator.SetActive(IsCurrentlyAuthenticating());

        if (_connectingIndicator != null)
            _connectingIndicator.SetActive(_isConnectingToIRC);
    }

    /// <summary>
    /// Checks if authentication is currently in progress
    /// </summary>
    private bool IsCurrentlyAuthenticating()
    {
        bool isAuthenticating = _apiClient != null && _apiClient.GettingNewToken;
        Debug.Log($"IsCurrentlyAuthenticating: {isAuthenticating}");
        return _apiClient != null && _apiClient.GettingNewToken;
    }

    /// <summary>
    /// Gets user-friendly status message with color
    /// </summary>
    public string GetColoredStatus()
    {
        string status = GetConnectionStatus();
        Color statusColor = _disconnectedColor;

        if (_isConnectedToIRC)
            statusColor = _connectedColor;
        else if (_isAuthenticated)
            statusColor = _authenticatedColor;

        return $"<color=#{ColorUtility.ToHtmlStringRGB(statusColor)}>{status}</color>";
    }
}