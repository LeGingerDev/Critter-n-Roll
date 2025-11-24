using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.API.Users;
using ScoredProductions.StreamLinked.ManagersAndBuilders;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Events;

namespace ScoredProductions.StreamLinked.IRC {

	/// <summary>
	/// Twitch IRC Singleton.
	/// Handles connection and processing of messages.
	/// </summary>
	[DefaultExecutionOrder(-0x4)]
	public class TwitchIRCClient : SingletonDispatcher<TwitchIRCClient> {

		public static int MessageCapacity = 10000;

		/// <summary>
		/// 10 KB
		/// </summary>
		public const int BUFFER_SIZE = 10240;

		public Queue<TwitchMessage> AllMessages { get; private set; } = new Queue<TwitchMessage>(MessageCapacity);

		[SerializeField]
		private bool ircEnabled = false;
		public bool IRCEnabled
		{
			get => this.ircEnabled;
			set
			{
				if (this.ircEnabled != value) {
					this.ircEnabled = value;
					if (InstanceIsAlive) {
						if (this.ircEnabled) {
							this.ReconnectToTwitch();
						}
						else {
							this.CloseIRC();
						}
					}
				}
			}
		}

		[SerializeField, HideInInspector, Tooltip("Token to use for the IRC, if left blank it will default to the one provided to the API Client")]
		private TokenInstance ircToken;
		public TokenInstance IRCToken
		{
			get => this.ircToken;
			set
			{
				if (this.ircToken != value) {
					this.EndFunctionality();
					this.BuildCancelTokens();
					this.ircToken = value;
					this.ircTokenUserInfo = null;
				}
			}
		}

		private GetUsers? ircTokenUserInfo;

		public bool IsConnected => this.tcpClient != null && this.tcpClient.Connected;
		public bool IsConnecting => this.ConnectingToTwitchRoutine != null;
		public bool IsConnectingOrConnected => this.IsConnected | this.IsConnecting;

		public bool SaveTargetToPlayerPrefs = true;

		public bool CommandsEnabled = true;
		public bool MembershipEnabled = true;
		public bool TagsEnabled = true;
		public bool SSLConnection = false;
		[Tooltip("Moves the stream reading from the connection to an external thread instead of a coroutine in this object. [Original Method]")]
		public bool UseAsyncToRead = false;

		public bool OverwriteFromInternalSettings;

		[HideInInspector]
		public UnityEvent<TwitchMessage> OnJOIN;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnNICK;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnNOTICE;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnPART;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnPASS;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnPING;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnPONG;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnPRIVMSG;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnCLEARCHAT;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnCLEARMSG;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnGLOBALUSERSTATE;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnHOSTTARGET;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnRECONNECT;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnROOMSTATE;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnUSERNOTICE;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnUSERSTATE;
		[HideInInspector]
		public UnityEvent<TwitchMessage> OnCAP;
		[HideInInspector, Tooltip("For messages with either no command specified or command not listed in available events")]
		public UnityEvent<TwitchMessage> OnOTHER;

		[HideInInspector]
		public UnityEvent OnIRCStarted;
		[HideInInspector]
		public UnityEvent OnIRCStopped;


        [SerializeField]
        private bool _enableConnectionMonitoring = true;
        public bool EnableConnectionMonitoring => this._enableConnectionMonitoring;

        [SerializeField]
        private float _connectionCheckInterval = 15f;
        public float GetConnectionCheckInterval() => this._connectionCheckInterval;

        [SerializeField]
        private int _maxReconnectionAttempts = 5;
        public int GetMaxReconnectionAttempts() => this._maxReconnectionAttempts;

        private Coroutine _connectionMonitoringRoutine;
        private int _currentReconnectionAttempts = 0;
        private bool _isReconnecting = false;

        [SerializeField, HideInInspector]
		private string twitchTarget;
		public string TwitchTarget
		{
			get => this.twitchTarget;
			set
			{
				if (value != this.TwitchTarget) {
					this.twitchTarget = value?.Trim() ?? value;
					if (this.SaveTargetToPlayerPrefs) {
						InternalSettingsStore.EditSetting(SavedSettings.TwitchTarget, value, this.LogDebugLevel == DebugManager.DebugLevel.Full);
					}
					if (this.IsConnected && !string.IsNullOrWhiteSpace(value)) {
						this.JoinChannel(value);
					}
				}
			}
		}

		public string ConnectedChannelOnSocket { get; private set; }

		public bool ClientStopped => this.tcpClient == null;

		private GetUsers? joinedRoomUserData = null;
		public GetUsers? JoinedRoomUserData
		{
			get => this.joinedRoomUserData;
			private set
			{
				if (value == null && !this.joinedRoomUserData.HasValue) {
					return;
				}
				GetUsers? oldValue = this.joinedRoomUserData;
				if (value == null) {
					this.joinedRoomUserData = null;
					OnJoinedRoomUserUpdated?.Invoke(oldValue, null);
				}
				else if (this.joinedRoomUserData == null) {
					this.joinedRoomUserData = value;
					OnJoinedRoomUserUpdated?.Invoke(null, value);
				}
				else if (this.joinedRoomUserData.Value.login != value.Value.login) {
					this.joinedRoomUserData = value;
					OnJoinedRoomUserUpdated?.Invoke(this.joinedRoomUserData, value);
				}
			}
		}

		[SerializeField, HideInInspector]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		public delegate Task ReturnAPIData(GetUsers? OldUser, GetUsers? NewUser);
		public static ReturnAPIData OnJoinedRoomUserUpdated;

		private TcpClient tcpClient;

		private NetworkStream internalTCPStream;

		private Stream ProducedStream;

		private StreamWriter streamWriter;

		private readonly byte[] buffer = new byte[BUFFER_SIZE];

		private Coroutine ConnectingToTwitchRoutine;

		private Coroutine NetworkReader;

		private TwitchAPIClient apiReference;

		private AsyncCallback AsyncDataReader;

		private WaitUntil _waitForAPIAuth;
		private WaitUntil WaitForAPIAuth => _waitForAPIAuth ??= new WaitUntil(this.CheckIRCTokenIsInQueue);

		private CancellationTokenSource RequestAPICancellationToken;

		private static readonly Encoding Encoder = Encoding.UTF8;

		private static readonly ConcurrentQueue<string> MessageQueue = new ConcurrentQueue<string>(); 

		private TwitchIRCClient() { }

		protected override void Awake() {
			if (this.EstablishSingleton(true)) {
				this.AsyncDataReader = new AsyncCallback(this.ReadAsyncCallback);
			}
		}

        private void OnEnable()
        {
            this.BuildCancelTokens();
            if (this.ircEnabled)
            {
                this.ReconnectToTwitch();
            }

            // Start connection monitoring
            if (this._enableConnectionMonitoring && this._connectionMonitoringRoutine == null)
            {
                this._connectionMonitoringRoutine = this.StartCoroutine(this.ConnectionMonitoringRoutine());
            }
        }

        private void OnDestroy() {
			this.EndFunctionality();

			this.OnJOIN?.RemoveAllListeners();
			this.OnNICK?.RemoveAllListeners();
			this.OnNOTICE?.RemoveAllListeners();
			this.OnPART?.RemoveAllListeners();
			this.OnPASS?.RemoveAllListeners();
			this.OnPING?.RemoveAllListeners();
			this.OnPONG?.RemoveAllListeners();
			this.OnPRIVMSG?.RemoveAllListeners();
			this.OnCLEARCHAT?.RemoveAllListeners();
			this.OnCLEARMSG?.RemoveAllListeners();
			this.OnGLOBALUSERSTATE?.RemoveAllListeners();
			this.OnHOSTTARGET?.RemoveAllListeners();
			this.OnRECONNECT?.RemoveAllListeners();
			this.OnROOMSTATE?.RemoveAllListeners();
			this.OnUSERNOTICE?.RemoveAllListeners();
			this.OnUSERSTATE?.RemoveAllListeners();
			this.OnCAP?.RemoveAllListeners();
			this.OnOTHER?.RemoveAllListeners();
		}

		private void OnDisable() {
			this.EndFunctionality();
		}

		protected override void OnApplicationQuit() {
			this.EndFunctionality();
			base.OnApplicationQuit();
		}

        private void EndFunctionality()
        {
            this.CloseIRC();

            this.JoinedRoomUserData = null;

            if (this.ConnectingToTwitchRoutine != null)
            {
                this.StopCoroutine(this.ConnectingToTwitchRoutine);
            }

            if (this.NetworkReader != null)
            {
                this.StopCoroutine(this.NetworkReader);
            }

            // Stop connection monitoring
            if (this._connectionMonitoringRoutine != null)
            {
                this.StopCoroutine(this._connectionMonitoringRoutine);
                this._connectionMonitoringRoutine = null;
            }

            this._isReconnecting = false;
            this._currentReconnectionAttempts = 0;

            this.EndCancelTokens();
        }

        protected override void LateUpdate() {
			base.LateUpdate();

			while (MessageQueue.TryDequeue(out string message)) {
				this.ProcessMessage(message);
			}
		}

		public void SaveTwitchTargetToPlayerPrefs() {
			if (string.IsNullOrWhiteSpace(this.twitchTarget)) {
				InternalSettingsStore.EditSetting(SavedSettings.TwitchTarget, "", this.LogDebugLevel == DebugManager.DebugLevel.Full);
			}
			else {
				if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchTarget, out string target, this.LogDebugLevel == DebugManager.DebugLevel.Full)) {
					if (target != this.twitchTarget) {
						InternalSettingsStore.EditSetting(SavedSettings.TwitchTarget, this.twitchTarget, this.LogDebugLevel == DebugManager.DebugLevel.Full);
					}
				}
				else {
					InternalSettingsStore.EditSetting(SavedSettings.TwitchTarget, this.twitchTarget, this.LogDebugLevel == DebugManager.DebugLevel.Full);
				}
			}
		}

		private bool CheckIRCTokenIsInQueue() {
			return this.apiReference == null || this.apiReference.CheckTokenIsInQueue(this.ircToken);
		}

		private void EndCancelTokens() {
			if (this.RequestAPICancellationToken != null) {
				try {
					this.RequestAPICancellationToken.Cancel();
					this.RequestAPICancellationToken.Dispose();
					this.RequestAPICancellationToken = null;
				} catch {
					if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage("RequestAPICancellationToken was Canceled".RichTextColour("orange"));
					}
				}
			}
		}

		private void BuildCancelTokens() {
			this.RequestAPICancellationToken ??= new CancellationTokenSource();
		}

		/// <summary>
		/// Get Twitch information of Target room
		/// </summary>
		public async Task GetJoinedRoomUserData() {
			this.JoinedRoomUserData = null;
			if (!string.IsNullOrWhiteSpace(this.ConnectedChannelOnSocket)) {

			TwitchAPIDataContainer<GetUsers> returnedData
					= await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetUsers>(
						QueryParameters: new (string, string)[] {
							(GetUsers.LOGIN, this.ConnectedChannelOnSocket)
						},
						cancelToken: this.RequestAPICancellationToken.Token);
				if (!returnedData.HasErrored) {
					this.JoinedRoomUserData = returnedData.data[0];
					if (TwitchBadgeManager.GetInstance(out TwitchBadgeManager manager)) {
						manager.GetChannelBadges(this.JoinedRoomUserData.Value, true, this.ircToken);
					}
				}
				else {
					if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage($"GetUsers API call failed to get Users. TwitchTarget: {{{this.ConnectedChannelOnSocket}}}, Error: {returnedData.ErrorText}", DebugManager.ErrorLevel.Assertion);
					}
				}
			} else {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"GetUsers API call failed to get Users. ConnectedChannelOnSocket is null or empty when it shouldnt be.", DebugManager.ErrorLevel.Assertion);
				}
			}
		}

		/// <summary>
		/// Attempts to aquire information required to start Twitch IRC
		/// </summary>
		private bool GetSettings() {
			if (this.OverwriteFromInternalSettings) {
				if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchTarget, out string target)) {
					this.twitchTarget = target;
				}
			}
			else if (string.IsNullOrEmpty(this.twitchTarget)) {
				if (InternalSettingsStore.TryGetSetting(SavedSettings.TwitchTarget, out string target)) {
					this.twitchTarget = target;
				}
			}
			if (string.IsNullOrEmpty(this.twitchTarget)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"Failed to get a Target for {TwitchAPIClient.Name}, There is no channel to point to.", DebugManager.ErrorLevel.Error);
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Sends a message to Twitch Chat
		/// </summary>
		public void SendToTwitch(params string[] messages) {
			if (this.streamWriter == null) {
				return;
			}
			try {
				bool needsFlush = false;
				for (int i = 0; i < messages.Length; i++) {
					if (string.IsNullOrWhiteSpace(messages[i])) {
						continue;
					}
					else {
						if (!messages[i].EndsWith(TwitchWords.END_MESSAGE_TAG, StringComparison.InvariantCultureIgnoreCase)) {
							messages[i] += TwitchWords.END_MESSAGE_TAG;
						}
						this.streamWriter?.WriteLine(messages[i]);
						if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
							DebugManager.LogMessage(messages[i]);
						}
						needsFlush = true;
					}
				}
				if (needsFlush) {
					this.streamWriter?.Flush();
				}
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex, DebugManager.ErrorLevel.Exception);
				}
			}
		}

		/// <summary>
		/// Leaves and joins a new room, provide blank room to not join new room
		/// </summary>
		public void JoinChannel(string room) {
			this.LeaveChannel();
			this.BuildCancelTokens();

			if (!string.IsNullOrWhiteSpace(room)) {

				this.ConnectedChannelOnSocket = room.ToLower();

				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage($"IRC Attempting to join room: {this.ConnectedChannelOnSocket}");
				}

				this.SendToTwitch($"{TwitchWords.JOIN} #{this.ConnectedChannelOnSocket}");

				if (this.CommandsEnabled) {
					this.SendToTwitch($"CAP REQ :twitch.tv/commands{TwitchWords.END_MESSAGE_TAG}");
				}

				if (this.MembershipEnabled) {
					this.SendToTwitch($"CAP REQ :twitch.tv/membership{TwitchWords.END_MESSAGE_TAG}");
				}

				if (this.TagsEnabled) {
					this.SendToTwitch($"CAP REQ :twitch.tv/tags{TwitchWords.END_MESSAGE_TAG}");
				}

				Task.Run(this.GetJoinedRoomUserData);
			}
		}

        /// <summary>
        /// Process response received in the IRC
        /// </summary>
        private void ReadAsyncCallback(IAsyncResult result)
        {
            if (this.ProducedStream == null || this.ClientStopped || !this.IsConnected)
            {
                return;
            }

            try
            {
                int bytesRead = this.ProducedStream.EndRead(result);

                if (bytesRead == 0)
                {
                    if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                    {
                        DebugManager.LogMessage("Connection closed by remote host - triggering reconnection".RichTextColour("orange"));
                    }

                    // Trigger reconnection on main thread
                    MainThreadDispatchQueue.Enqueue(this.HandleConnectionLoss);
                    return;
                }

                this.ProcessReceivedData(bytesRead);
            }
            catch (ObjectDisposedException)
            {
                // Normal shutdown, don't reconnect
                return;
            }
            catch (Exception ex)
            {
                if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                {
                    DebugManager.LogMessage($"ReadAsyncCallback error: {ex.Message}".RichTextColour("red"));
                }

                MainThreadDispatchQueue.Enqueue(this.HandleConnectionLoss);
                return;
            }

            this.ContinueAsyncRead();
        }

        private IEnumerator ReadCoroutineCallback() {
			int index = 0;
			while (this.ProducedStream?.CanRead ?? false) {
				if (this.internalTCPStream.DataAvailable) {

					int bytesRead = this.ProducedStream.Read(this.buffer, 0, BUFFER_SIZE);
					Span<char> receivedMessage = stackalloc char[bytesRead];

					try {
						Encoder.GetChars(this.buffer.AsSpan(0, bytesRead), receivedMessage);

						while ((index = receivedMessage.IndexOf(TwitchWords.END_MESSAGE_TAG_PART_2)) > 0 && receivedMessage[index - 1] == TwitchWords.END_MESSAGE_TAG_PART_1) {
							MessageQueue.Enqueue(new string(receivedMessage[..index]));
							receivedMessage = receivedMessage[(index + 1)..];
						}
						receivedMessage = Span<char>.Empty;

						if (this.ClientStopped || !this.IsConnected || this.ProducedStream == null || !this.ProducedStream.CanRead) {
							yield break;
						}
					} catch (Exception ex) {
						if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
							DebugManager.LogMessage($"Problem occured in Callback, Error: {{{ex.Message}}} : StackTrace: {{{ex.StackTrace.RichTextColour("yellow")}}} : MessageState: {{{receivedMessage.ToString()}}}");
						}
					}
				}

				yield return TwitchStatic.EndOfFrameWait;
			}
		}

		/// <summary>
		/// Stops and starts Twitch IRC, Runs a coroutine
		/// </summary>
		public void ReconnectToTwitch() {
			if (this.ircToken != null) {
				MainThreadDispatchQueue.Enqueue(this.ThreadReconnectToTwitch);
			}
		}

		private void ThreadReconnectToTwitch() {
			if (this.ConnectingToTwitchRoutine != null) {
				this.StopCoroutine(this.ConnectingToTwitchRoutine);
			}
			this.ConnectingToTwitchRoutine = this.StartCoroutine(this.StartTwitchReconnectionRoutine());
			this.OnIRCStarted?.Invoke();
		}

		private IEnumerator StartTwitchReconnectionRoutine() {
			if (!this.ircEnabled) {
				DebugManager.LogMessage($"IRC Disabled, Connect process Canceled.".RichTextColour("yellow"), DebugManager.ErrorLevel.Assertion);
				goto End;
			}
			if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
				DebugManager.LogMessage($"Twitch IRC {(this.IsConnected ? "Reconnecting" : "Connecting")}.");
			}
			if (this.IsConnected) {
				this.CloseIRC();
			}

			if (this.apiReference != null || TwitchAPIClient.GetInstance(out this.apiReference)) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage($"Twitch IRC waiting for API credentials.");
				}

				if (this.ircToken.CheckRefreshNeeded(this.LogDebugLevel > DebugManager.DebugLevel.Necessary)) {
					this.apiReference.GetNewAuthToken(this.ircToken);
				}

				if (this.CheckIRCTokenIsInQueue()) {
					yield return this.WaitForAPIAuth;
				}

				if (!this.GetSettings()) {
					goto End;
				}

				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage($"Twitch IRC credentials aquired.");
				}

				if (!this.ircTokenUserInfo.HasValue) {
					yield return this.apiReference.MakeTwitchAPIRequest<GetUsers>(this.GetUserInfo, this.ircToken);
				}

				if (this.SSLConnection) {
					this.tcpClient = new TcpClient(TwitchWords.CHAT_HOST_ADDRESS, TwitchStatic.SSL_IRC_PORT);
					this.internalTCPStream = this.tcpClient.GetStream();
					SslStream ssl = new SslStream(this.internalTCPStream, true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
					try {
						ssl.AuthenticateAsClient(TwitchWords.CHAT_HOST_ADDRESS);
					} catch { // abort						
						this.CloseIRC();
						yield break;
					}
					this.ProducedStream = ssl;
				}
				else {
					this.tcpClient = new TcpClient(TwitchWords.CHAT_HOST_ADDRESS, TwitchStatic.NON_SSL_IRC_PORT);
					 this.ProducedStream = this.internalTCPStream = this.tcpClient.GetStream();
				}

				this.streamWriter = new StreamWriter(this.ProducedStream);

				if (this.UseAsyncToRead) {
					this.ProducedStream.BeginRead(this.buffer, 0, BUFFER_SIZE, this.AsyncDataReader, null);
				} else {
					this.NetworkReader = this.StartCoroutine(this.ReadCoroutineCallback());
				}

				this.SendToTwitch($"{TwitchWords.PASS} {TwitchStatic.AppendOAuthToAuth(this.apiReference.DefaultAPIToken.OAuthToken.Access_Token)}{TwitchWords.END_MESSAGE_TAG}",
									$"{TwitchWords.NICK} {this.ircTokenUserInfo.Value.login}{TwitchWords.END_MESSAGE_TAG}");

				this.JoinChannel(this.TwitchTarget);
			}
			else {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("TwitchAPIClient not available, cant authenticate with Twitch.", DebugManager.ErrorLevel.Assertion);
				}
			}

			End:
			this.ConnectingToTwitchRoutine = null;
		}

		private void GetUserInfo(TwitchAPIDataContainer<GetUsers> data) {
			if (data.HasErrored) {
				this.EndFunctionality();
			}
			else {
				this.ircTokenUserInfo = data.data[0];
			}
		}

		/// <summary>
		/// Leaves twitch IRC room
		/// </summary>
		/// <param name="newRoom"></param>
		private void LeaveChannel() {
			if (!string.IsNullOrEmpty(this.ConnectedChannelOnSocket)) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"Leaving current room: {this.ConnectedChannelOnSocket}");
				}
				this.SendToTwitch($"{TwitchWords.PART} #{this.ConnectedChannelOnSocket}");
				this.ConnectedChannelOnSocket = null;
			}
		}

		private void CloseIRC() {
			this.LeaveChannel();

			if (this.streamWriter != null) {
				lock (this.streamWriter) {
					this.streamWriter.Flush();
					this.streamWriter.Close();
					this.streamWriter.Dispose();
				}
				this.streamWriter = null;
			}
			if (this.ProducedStream != null) {
				lock (this.ProducedStream) {
					this.ProducedStream.Flush();
					this.ProducedStream.Close();
					this.ProducedStream.Dispose();
				}
				this.ProducedStream = null;
			}
			if (this.tcpClient != null) {
				lock (this.tcpClient) {
					this.tcpClient.Close();
					this.tcpClient.Dispose();
				}
				this.tcpClient = null;
			}

			Array.Clear(this.buffer, 0, BUFFER_SIZE); // Clear buffer ready for new instance

			this.OnIRCStopped?.Invoke();
		}

		public void SendMessageToChat(string message) {
			this.SendToTwitch($"{TwitchWords.PRIVMSG} #{this.twitchTarget} : {message}");
		}

		/// <summary>
		/// Builds message into a TwitchMessage object
		/// </summary>
		/// <param name="rawMessage"></param>
		private void ProcessMessage(string rawMessage) {
			if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
				DebugManager.LogMessage($"Message Received: {rawMessage}");
			}

			try {
				TwitchMessage msg = new TwitchMessage(rawMessage);

				switch (msg.CommandEnum) {
					case TwitchIRCCommand.JOIN:
						if (this.OnJOIN != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnJOIN?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.NICK:
						if (this.OnNICK != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnNICK?.Invoke(msg));
						}
						break;
					// Auth
					case TwitchIRCCommand.NOTICE:
						if (msg.FullSender.Equals(TwitchWords.SENDER_ADDRESS)) {
							if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
								DebugManager.LogMessage(msg.RawMessage.RichTextColour("yellow"));
							}
						}
						if (this.OnNOTICE != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnNOTICE?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.PART:
						if (this.OnPART != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnPART?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.PASS:
						if (this.OnPASS != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnPASS?.Invoke(msg));
						}
						break;
					// Responder
					case TwitchIRCCommand.PING:
						this.SendToTwitch($"PONG {rawMessage[5..]}{TwitchWords.END_MESSAGE_TAG}");
						if (this.OnPING != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnPING?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.PONG:
						if (this.OnPONG != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnPONG?.Invoke(msg));
						}
						break;
					// Chat message
					case TwitchIRCCommand.PRIVMSG:
						if (this.OnPRIVMSG != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnPRIVMSG?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.CLEARCHAT:
						if (this.OnCLEARCHAT != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnCLEARCHAT?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.CLEARMSG:
						if (this.OnCLEARMSG != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnCLEARMSG?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.GLOBALUSERSTATE:
						if (this.OnGLOBALUSERSTATE != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnGLOBALUSERSTATE?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.HOSTTARGET:
						if (this.OnHOSTTARGET != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnHOSTTARGET?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.RECONNECT:
						if (this.OnRECONNECT != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnRECONNECT?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.ROOMSTATE:
						if (this.OnROOMSTATE != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnROOMSTATE?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.USERNOTICE:
						if (this.OnUSERNOTICE != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnUSERNOTICE?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.USERSTATE:
						if (this.OnUSERSTATE != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnUSERSTATE?.Invoke(msg));
						}
						break;
					case TwitchIRCCommand.CAP:
						if (this.OnCAP != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnCAP?.Invoke(msg));
						}
						break;
					default:
						if (this.OnOTHER != null) {
							MainThreadDispatchQueue.Enqueue(() => this.OnOTHER?.Invoke(msg));
						}
						break;
				}

				while (this.AllMessages.Count > MessageCapacity) {
					this.AllMessages.Dequeue();
				}

				this.AllMessages.Enqueue(msg);
			} catch (Exception exception) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"Exception occurred when processing rawMessage: {rawMessage} : {exception.ToString().RichTextColour("yellow")}", DebugManager.ErrorLevel.Error);
				}
			}
		}

		// The following method is invoked by the RemoteCertificateValidationDelegate.
		public static bool ValidateServerCertificate(
			  object sender,
			  X509Certificate certificate,
			  X509Chain chain,
			  SslPolicyErrors sslPolicyErrors) {
			if (sslPolicyErrors == SslPolicyErrors.None) {
				return true;
			}
			DebugManager.LogMessage($"IRC SSL Certificate error: {sslPolicyErrors}");
			// Do not allow this client to communicate with unauthenticated servers.
			return false;
		}

        private void ProcessReceivedData(int bytesRead)
        {
            Span<char> receivedMessage = stackalloc char[bytesRead];
            Encoder.GetChars(this.buffer.AsSpan(0, bytesRead), receivedMessage);

            while (true)
            {
                int index = receivedMessage.IndexOf(TwitchWords.END_MESSAGE_TAG_PART_2);
                if (index <= 0 || receivedMessage[index - 1] != TwitchWords.END_MESSAGE_TAG_PART_1)
                {
                    break;
                }

                MessageQueue.Enqueue(new string(receivedMessage[..index]));
                receivedMessage = receivedMessage[(index + 1)..];
            }
        }

        // Add this method to continue async reading safely:
        private void ContinueAsyncRead()
        {
            if (!this.ClientStopped && this.IsConnected &&
                this.ProducedStream != null && this.ProducedStream.CanRead)
            {
                try
                {
                    this.ProducedStream.BeginRead(this.buffer, 0, BUFFER_SIZE, this.AsyncDataReader, null);
                }
                catch (Exception ex)
                {
                    if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                    {
                        DebugManager.LogMessage($"Failed to continue async read: {ex.Message}".RichTextColour("red"));
                    }

                    MainThreadDispatchQueue.Enqueue(this.HandleConnectionLoss);
                }
            }
        }

        // Add this new method to handle connection loss:
        private void HandleConnectionLoss()
        {
            if (!this.ircEnabled || this._isReconnecting)
            {
                return;
            }

            if (this.LogDebugLevel != DebugManager.DebugLevel.None)
            {
                DebugManager.LogMessage("Connection lost - attempting reconnection".RichTextColour("yellow"));
            }

            this.ReconnectWithRetry();
        }

        // Add this method for retry logic:
        private void ReconnectWithRetry()
        {
            if (this._isReconnecting)
            {
                return;
            }

            this._isReconnecting = true;
            this._currentReconnectionAttempts = 0;

            if (this.ConnectingToTwitchRoutine != null)
            {
                this.StopCoroutine(this.ConnectingToTwitchRoutine);
            }

            this.ConnectingToTwitchRoutine = this.StartCoroutine(this.ReconnectionWithRetryRoutine());
        }

        // Add this coroutine for handling reconnection with retry:
        private IEnumerator ReconnectionWithRetryRoutine()
        {
            while (this._currentReconnectionAttempts < this._maxReconnectionAttempts && this.ircEnabled)
            {
                this._currentReconnectionAttempts++;

                if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                {
                    DebugManager.LogMessage($"Reconnection attempt {this._currentReconnectionAttempts}/{this._maxReconnectionAttempts}");
                }

                // Use your existing reconnection logic
                yield return this.StartCoroutine(this.StartTwitchReconnectionRoutine());

                // Check if reconnection succeeded
                if (this.IsConnected)
                {
                    this._isReconnecting = false;
                    this._currentReconnectionAttempts = 0;

                    if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                    {
                        DebugManager.LogMessage("Reconnection successful".RichTextColour("green"));
                    }
                    yield break;
                }

                // Wait before next attempt (exponential backoff)
                float waitTime = Mathf.Pow(2, this._currentReconnectionAttempts) * 2f; // 2, 4, 8, 16, 32 seconds
                yield return new WaitForSeconds(waitTime);
            }

            // All attempts failed
            this._isReconnecting = false;
            if (this.LogDebugLevel != DebugManager.DebugLevel.None)
            {
                DebugManager.LogMessage($"Failed to reconnect after {this._maxReconnectionAttempts} attempts".RichTextColour("red"));
            }
        }

        // Add this connection monitoring coroutine:
        private IEnumerator ConnectionMonitoringRoutine()
        {
            WaitForSeconds waitInterval = new WaitForSeconds(this._connectionCheckInterval);

            while (this.ircEnabled)
            {
                yield return waitInterval;

                if (this.ircEnabled && !this.IsConnected && !this._isReconnecting)
                {
                    if (this.LogDebugLevel != DebugManager.DebugLevel.None)
                    {
                        DebugManager.LogMessage("Connection monitoring detected disconnection".RichTextColour("yellow"));
                    }

                    this.HandleConnectionLoss();
                }
            }
        }

    }
}