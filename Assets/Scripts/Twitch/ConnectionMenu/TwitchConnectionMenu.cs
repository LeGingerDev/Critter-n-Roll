using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Complete Twitch connection menu with authentication, IRC connection, and status display
/// </summary>
public partial class TwitchConnectionMenu : MonoBehaviour
{
    // Display Components
    [FoldoutGroup("Display Settings")]
    [SerializeField] private TextMeshProUGUI _statusText;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private TextMeshProUGUI _channelText;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private Image _statusIndicator;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private GameObject _authenticatedPanel;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private GameObject _notAuthenticatedPanel;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private Color _disconnectedColor = Color.red;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private Color _authenticatedColor = Color.yellow;

    [FoldoutGroup("Display Settings")]
    [SerializeField] private Color _connectedColor = Color.green;

    // Authentication Components
    [FoldoutGroup("Authentication")]
    [SerializeField] private Button _authButton;

    [FoldoutGroup("Authentication")]
    [SerializeField] private bool _clearTokensOnAuth = true;

    [FoldoutGroup("Authentication")]
    [SerializeField] private GameObject _authenticatingIndicator;

    // IRC Components
    [FoldoutGroup("IRC Connection")]
    [SerializeField] private Button _connectButton;

    [FoldoutGroup("IRC Connection")]
    [SerializeField] private bool _autoSetChannelToUser = true;

    [FoldoutGroup("IRC Connection")]
    [SerializeField] private GameObject _connectingIndicator;

    // Debug Info
    [FoldoutGroup("Debug Info")]
    [ShowInInspector, ReadOnly] private string _currentStatus;

    [FoldoutGroup("Debug Info")]
    [ShowInInspector, ReadOnly] private string _currentChannel;

    [FoldoutGroup("Debug Info")]
    [ShowInInspector, ReadOnly] private bool _isAuthenticated;

    [FoldoutGroup("Debug Info")]
    [ShowInInspector, ReadOnly] private bool _isConnectedToIRC;

    [FoldoutGroup("Debug Info")]
    [ShowInInspector, ReadOnly] private bool _isConnectingToIRC;

    // Internal references
    private TwitchAPIClient _apiClient;
    private TwitchIRCClient _ircClient;

    private void Start()
    {
        SetupReferences();
        SetupButtons();
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
        InvokeRepeating(nameof(UpdateDisplay), 0.5f, 0.5f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateDisplay));
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

    private void SetupButtons()
    {
        if (_authButton != null)
        {
            _authButton.onClick.AddListener(OnAuthButtonClicked);
        }

        if (_connectButton != null)
        {
            _connectButton.onClick.AddListener(OnConnectButtonClicked);
        }
    }

    private void OnAuthButtonClicked()
    {
        HandleAuthentication();
    }

    private void OnConnectButtonClicked()
    {
        HandleIRCConnection();
    }

    // Public methods for external access
    public bool GetIsAuthenticated() => TwitchAPIClient.APIOAuthAvailable;
    public bool GetIsConnectedToIRC() => _ircClient != null && _ircClient.IsConnected;
    public bool GetIsConnectingToIRC() => _ircClient != null && _ircClient.IsConnecting;
    public string GetCurrentChannel() => _ircClient?.TwitchTarget ?? "None";

    public string GetConnectionStatus()
    {
        if (!GetIsAuthenticated())
            return "Not Authenticated";
        else if (GetIsConnectingToIRC())
            return "Connecting to IRC";
        else if (GetIsConnectedToIRC())
            return "Connected to IRC";
        else
            return "Authenticated";
    }
}

