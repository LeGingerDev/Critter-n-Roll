using UnityEngine;
using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.IRC;
using UnityEngine.UI;


namespace YourGameNamespace
{
    /// <summary>
    /// Simple button to force Twitch re-authentication - useful for testing
    /// </summary>
    public class AuthenticateButton : MonoBehaviour
    {
        [SerializeField] private Button _authenticateButton;
        [SerializeField] private bool _clearTokensFirst = true;
        [SerializeField] private bool _enableIRCAfterAuth = true;

        private TwitchAPIClient _apiClient;
        private TwitchIRCClient _ircClient;

        private void Start()
        {
            SetupReferences();
            SetupButton();
        }

        private void SetupReferences()
        {
            if (!TwitchAPIClient.GetInstance(out _apiClient))
            {
                Debug.LogError("TwitchAPIClient not found!");
                return;
            }

            if (!TwitchIRCClient.GetInstance(out _ircClient))
            {
                Debug.LogError("TwitchIRCClient not found!");
                return;
            }
        }

        private void SetupButton()
        {
            if (_authenticateButton == null)
            {
                Debug.LogError("Authenticate button not assigned!");
                return;
            }

            _authenticateButton.onClick.AddListener(ForceAuthentication);
        }

        private void ForceAuthentication()
        {
            // Clear and start auth
            _apiClient.CleanPlayerPrefTokens();
            _apiClient.CancelAPIRequestsAndReset();

            // Disable IRC
            _ircClient.IRCEnabled = false;

            // Start auth
            _apiClient.GetNewAuthToken(_apiClient.DefaultAPIToken);

            // Enable IRC - it will handle waiting for auth completion internally
            _ircClient.IRCEnabled = true;
        }
        // Public method for external use
        public void ForceReauth()
        {
            ForceAuthentication();
        }

        // Method to just clear tokens without re-authing
        public void ClearTokensOnly()
        {
            _apiClient.CleanPlayerPrefTokens();
            Debug.Log("Tokens cleared - authentication required on next connection");
        }

        // Method to just trigger auth without clearing
        public void AuthWithoutClearing()
        {
            _apiClient.GetNewAuthToken(_apiClient.DefaultAPIToken);
            Debug.Log("Authentication triggered without clearing tokens");
        }
    }
}