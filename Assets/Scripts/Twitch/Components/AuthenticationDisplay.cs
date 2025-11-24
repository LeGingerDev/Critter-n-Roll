using UnityEngine;
using TMPro;
using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.IRC;
using ScoredProductions.StreamLinked.API.Users;

namespace YourGameNamespace
{
    /// <summary>
    /// Displays current Twitch authentication and connection status
    /// </summary>
    public class AuthenticationDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _authStatusText;
        [SerializeField] private TextMeshProUGUI _channelNameText;
        [SerializeField] private GameObject _authenticatedPanel;
        [SerializeField] private GameObject _notAuthenticatedPanel;

        private TwitchAPIClient _apiClient;
        private TwitchIRCClient _ircClient;

        private void Start()
        {
            SetupReferences();
            UpdateDisplay();

            // Subscribe to auth events
            // TODO: Replace with your Topic System
            if (_apiClient != null)
            {
                _apiClient.OnAuthenticationSuccess.AddListener(OnAuthStateChanged);
            }

            // Update display periodically
            InvokeRepeating(nameof(UpdateDisplay), 1f, 1f);
        }

        private void OnDestroy()
        {
            if (_apiClient != null)
            {
                _apiClient.OnAuthenticationSuccess.RemoveListener(OnAuthStateChanged);
            }
        }

        private void SetupReferences()
        {
            TwitchAPIClient.GetInstance(out _apiClient);
            TwitchIRCClient.GetInstance(out _ircClient);
        }

        private void OnAuthStateChanged()
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            bool isAuthenticated = IsAuthenticated();

            // Show/hide panels
            if (_authenticatedPanel != null)
                _authenticatedPanel.SetActive(isAuthenticated);

            if (_notAuthenticatedPanel != null)
                _notAuthenticatedPanel.SetActive(!isAuthenticated);

            if (isAuthenticated)
            {
                UpdateAuthenticatedDisplay();
            }
            else
            {
                UpdateNotAuthenticatedDisplay();
            }
        }

        private void UpdateAuthenticatedDisplay()
        {
            // Update auth status
            if (_authStatusText != null)
            {
                if (IsConnectedToChat())
                {
                    _authStatusText.text = "Twitch Connected!";
                    _authStatusText.color = Color.green;
                }
                else
                {
                    _authStatusText.text = "Twitch Authenticated!";
                    _authStatusText.color = Color.yellow;
                }
            }

            // Update channel name
            if (_channelNameText != null)
            {
                string channelName = GetChannelName();
                _channelNameText.text = $"Twitch Channel: {channelName}";
            }
        }

        private void UpdateNotAuthenticatedDisplay()
        {
            if (_authStatusText != null)
            {
                _authStatusText.text = "Not Authenticated";
                _authStatusText.color = Color.red;
            }

            if (_channelNameText != null)
            {
                _channelNameText.text = "Twitch Channel: None";
            }
        }

        private bool IsAuthenticated()
        {
            return TwitchAPIClient.APIOAuthAvailable;
        }

        private bool IsConnectedToChat()
        {
            return _ircClient != null && _ircClient.IsConnected;
        }

        private string GetChannelName()
        {
            if (_ircClient != null && !string.IsNullOrEmpty(_ircClient.TwitchTarget))
            {
                return _ircClient.TwitchTarget;
            }

            return GetAuthenticatedUsername();
        }

        private string GetAuthenticatedUsername()
        {
            if (IsAuthenticated())
            {
                try
                {
                    var response = TwitchAPIClient.MakeTwitchAPIRequest<GetUsers>(1000);
                    if (!response.HasErrored && response.data.Length > 0)
                    {
                        return response.data[0].login;
                    }
                }
                catch
                {
                    // Ignore errors, just return unknown
                }
            }

            return "Unknown";
        }

        // Public methods for external control
        public string GetCurrentStatus()
        {
            if (IsConnectedToChat())
                return "Connected";
            else if (IsAuthenticated())
                return "Authenticated";
            else
                return "Not Authenticated";
        }
    }
}

