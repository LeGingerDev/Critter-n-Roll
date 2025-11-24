using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.API.EventSub;
using ScoredProductions.StreamLinked.EventSub.Events;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.ExtensionAttributes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.EventSub.WebSocketMessages;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.Events;

namespace ScoredProductions.StreamLinked.EventSub {

	/// <summary>
	/// Twitch EventSub Singleton. Connects using WebSocket, Webhook not supported.
	/// </summary>
	[DefaultExecutionOrder(-0x2)]
	public class TwitchEventSubClient : SingletonDispatcher<TwitchEventSubClient> {

		public const string WebSocketAddress = "wss://eventsub.wss.twitch.tv/ws";

		[Range(0, 600), Tooltip("Adds keepalive_timeout_seconds to the websocket address, values less than 10 will disable this functionality.")]
		public int KeepaliveTimeoutSeconds = 0;

		/// <summary>
		/// Used to get the websocket address currently used by the client
		/// </summary>
		public string GetSocketAddress => WebSocketAddress
			+ (this.KeepaliveTimeoutSeconds >= 10 ? "?keepalive_timeout_seconds=" + this.KeepaliveTimeoutSeconds : "");

		/// <summary>
		/// 10 KB
		/// </summary>
		public const int BUFFER_SIZE = 10240;

		[SerializeField, HideInInspector, Tooltip("Token to use for the EventSub, if left blank it will default to the one provided to the API Client")]
		private TokenInstance eventSubToken;
		public TokenInstance EventSubToken
		{
			get => this.eventSubToken;
			set
			{
				if (this.eventSubToken != value) {
					this.eventSubToken = value;
					Task.Run(this.CloseWebsocket);
				}
			}
		}

		[HideInInspector]
		public UnityEvent<AutoMessageHold> OnAutoMessageHold;
		[HideInInspector]
		public UnityEvent<AutoMessageHoldV2> OnAutoMessageHoldV2;
		[HideInInspector]
		public UnityEvent<AutoMessageUpdate> OnAutoMessageUpdate;
		[HideInInspector]
		public UnityEvent<AutoMessageUpdateV2> OnAutoMessageUpdateV2;
		[HideInInspector]
		public UnityEvent<AutomodSettingsUpdate> OnAutomodSettingsUpdate;
		[HideInInspector]
		public UnityEvent<AutomodTermsUpdate> OnAutomodTermsUpdate;
		[HideInInspector]
		public UnityEvent<ChannelBitsUse> OnChannelBitsUse;
		[HideInInspector]
		public UnityEvent<ChannelUpdate> OnChannelUpdate;
		[HideInInspector]
		public UnityEvent<ChannelFollow> OnChannelFollow;
		[HideInInspector]
		public UnityEvent<ChannelAdBreakBegin> OnChannelAdBreakBegin;
		[HideInInspector]
		public UnityEvent<ChannelChatClear> OnChannelChatClear;
		[HideInInspector]
		public UnityEvent<ChannelChatClearUserMessages> OnChannelChatClearUserMessages;
		[HideInInspector]
		public UnityEvent<ChannelChatMessage> OnChannelChatMessage;
		[HideInInspector]
		public UnityEvent<ChannelChatMessageDelete> OnChannelChatMessageDelete;
		[HideInInspector]
		public UnityEvent<ChannelChatNotification> OnChannelChatNotification;
		[HideInInspector]
		public UnityEvent<ChannelChatSettingsUpdate> OnChannelChatSettingsUpdate;
		[HideInInspector]
		public UnityEvent<ChannelChatUserMessageHold> OnChannelChatUserMessageHold;
		[HideInInspector]
		public UnityEvent<ChannelChatUserMessageUpdate> OnChannelChatUserMessageUpdate;
		[HideInInspector]
		public UnityEvent<ChannelSharedChatSessionBegin> OnChannelSharedChatSessionBegin;
		[HideInInspector]
		public UnityEvent<ChannelSharedChatSessionUpdate> OnChannelSharedChatSessionUpdate;
		[HideInInspector]
		public UnityEvent<ChannelSharedChatSessionEnd> OnChannelSharedChatSessionEnd;
		[HideInInspector]
		public UnityEvent<ChannelSubscribe> OnChannelSubscribe;
		[HideInInspector]
		public UnityEvent<ChannelSubscriptionEnd> OnChannelSubscriptionEnd;
		[HideInInspector]
		public UnityEvent<ChannelSubscriptionGift> OnChannelSubscriptionGift;
		[HideInInspector]
		public UnityEvent<ChannelSubscriptionMessage> OnChannelSubscriptionMessage;
		[HideInInspector]
		public UnityEvent<ChannelCheer> OnChannelCheer;
		[HideInInspector]
		public UnityEvent<ChannelRaid> OnChannelRaid;
		[HideInInspector]
		public UnityEvent<ChannelBan> OnChannelBan;
		[HideInInspector]
		public UnityEvent<ChannelUnban> OnChannelUnban;
		[HideInInspector]
		public UnityEvent<ChannelUnbanRequestCreate> OnChannelUnbanRequestCreate;
		[HideInInspector]
		public UnityEvent<ChannelUnbanRequestResolve> OnChannelUnbanRequestResolve;
		[HideInInspector]
		public UnityEvent<ChannelModerate> OnChannelModerate;
		[HideInInspector]
		public UnityEvent<ChannelModerateV2> OnChannelModerateV2;
		[HideInInspector]
		public UnityEvent<ChannelModeratorAdd> OnChannelModeratorAdd;
		[HideInInspector]
		public UnityEvent<ChannelModeratorRemove> OnChannelModeratorRemove;
		[HideInInspector]
		public UnityEvent<ChannelGuestStarSessionBegin> OnChannelGuestStarSessionBegin;
		[HideInInspector]
		public UnityEvent<ChannelGuestStarSessionEnd> OnChannelGuestStarSessionEnd;
		[HideInInspector]
		public UnityEvent<ChannelGuestStarGuestUpdate> OnChannelGuestStarGuestUpdate;
		[HideInInspector]
		public UnityEvent<ChannelGuestStarSettingsUpdate> OnChannelGuestStarSettingsUpdate;
		[HideInInspector]
		public UnityEvent<ChannelPointsAutomaticRewardRedemption> OnChannelPointsAutomaticRewardRedemption;
		[HideInInspector]
		public UnityEvent<ChannelPointsAutomaticRewardRedemptionV2> OnChannelPointsAutomaticRewardRedemptionV2;
		[HideInInspector]
		public UnityEvent<ChannelPointsCustomRewardAdd> OnChannelPointsCustomRewardAdd;
		[HideInInspector]
		public UnityEvent<ChannelPointsCustomRewardUpdate> OnChannelPointsCustomRewardUpdate;
		[HideInInspector]
		public UnityEvent<ChannelPointsCustomRewardRemove> OnChannelPointsCustomRewardRemove;
		[HideInInspector]
		public UnityEvent<ChannelPointsCustomRewardRedemptionAdd> OnChannelPointsCustomRewardRedemptionAdd;
		[HideInInspector]
		public UnityEvent<ChannelPointsCustomRewardRedemptionUpdate> OnChannelPointsCustomRewardRedemptionUpdate;
		[HideInInspector]
		public UnityEvent<ChannelPollBegin> OnChannelPollBegin;
		[HideInInspector]
		public UnityEvent<ChannelPollProgress> OnChannelPollProgress;
		[HideInInspector]
		public UnityEvent<ChannelPollEnd> OnChannelPollEnd;
		[HideInInspector]
		public UnityEvent<ChannelPredictionBegin> OnChannelPredictionBegin;
		[HideInInspector]
		public UnityEvent<ChannelPredictionProgress> OnChannelPredictionProgress;
		[HideInInspector]
		public UnityEvent<ChannelPredictionLock> OnChannelPredictionLock;
		[HideInInspector]
		public UnityEvent<ChannelPredictionEnd> OnChannelPredictionEnd;
		[HideInInspector]
		public UnityEvent<ChannelSuspiciousUserMessage> OnChannelSuspiciousUserMessage;
		[HideInInspector]
		public UnityEvent<ChannelSuspiciousUserUpdate> OnChannelSuspiciousUserUpdate;
		[HideInInspector]
		public UnityEvent<ChannelVIPAdd> OnChannelVIPAdd;
		[HideInInspector]
		public UnityEvent<ChannelVIPRemove> OnChannelVIPRemove;
		[HideInInspector]
		public UnityEvent<ChannelWarningAcknowledge> OnChannelWarningAcknowledge;
		[HideInInspector]
		public UnityEvent<ChannelWarningSend> OnChannelWarningSend;
		[HideInInspector]
		public UnityEvent<ChannelCharityDonation> OnCharityDonation;
		[HideInInspector]
		public UnityEvent<ChannelCharityCampaignStart> OnCharityCampaignStart;
		[HideInInspector]
		public UnityEvent<ChannelCharityCampaignProgress> OnCharityCampaignProgress;
		[HideInInspector]
		public UnityEvent<ChannelCharityCampaignStop> OnCharityCampaignStop;
		[HideInInspector]
		public UnityEvent<ConduitShardDisabled> OnConduitShardDisabled;
		[HideInInspector]
		public UnityEvent<DropEntitlementGrant> OnDropEntitlementGrant;
		[HideInInspector]
		public UnityEvent<ExtensionBitsTransactionCreate> OnExtensionBitsTransactionCreate;
		[HideInInspector]
		public UnityEvent<ChannelGoalsBegin> OnGoalsBegin;
		[HideInInspector]
		public UnityEvent<ChannelGoalsProgress> OnGoalsProgress;
		[HideInInspector]
		public UnityEvent<ChannelGoalsEnd> OnGoalsEnd;
		[HideInInspector]
		public UnityEvent<ChannelHypeTrainBegin> OnHypeTrainBegin;
		[HideInInspector]
		public UnityEvent<ChannelHypeTrainProgress> OnHypeTrainProgress;
		[HideInInspector]
		public UnityEvent<ChannelHypeTrainEnd> OnHypeTrainEnd;
		[HideInInspector]
		public UnityEvent<ChannelShoutoutCreate> OnShoutoutCreate;
		[HideInInspector]
		public UnityEvent<ChannelShoutoutReceived> OnShoutoutReceived;
		[HideInInspector]
		public UnityEvent<StreamOnline> OnStreamOnline;
		[HideInInspector]
		public UnityEvent<StreamOffline> OnStreamOffline;
		[HideInInspector]
		public UnityEvent<UserAuthorizationGrant> OnUserAuthorizationGrant;
		[HideInInspector]
		public UnityEvent<UserAuthorizationRevoke> OnUserAuthorizationRevoke;
		[HideInInspector]
		public UnityEvent<UserUpdate> OnUserUpdate;
		[HideInInspector]
		public UnityEvent<WhisperReceived> OnWhisperReceived;

		[HideInInspector]
		public UnityEvent<Subscription> OnUserRemovedSubscriptionRevoked;
		[HideInInspector]
		public UnityEvent<Subscription> OnAuthorizationRemoved;
		[HideInInspector]
		public UnityEvent<Subscription> OnVersionRemovedSubscriptionRevoked;

		public static bool EventSubConnectionActive => GetInstance(out TwitchEventSubClient client) && client.webSocket?.State == WebSocketState.Open && client.CurrentSessionState.HasValue;

		public static bool EventSubStartingUp => GetInstance(out TwitchEventSubClient client) && client.StartUpTask != null && !client.StartUpTask.IsCompleted;

		public List<Subscription> GetSubscriptions => new List<Subscription>(this.SessionSubscriptions.Values);

		public WebSocketState SocketState => this.webSocket?.State ?? WebSocketState.None;

		[SerializeField]
		private bool persistBetweenScenes = true;
		public override bool PersistBetweenScenes => this.persistBetweenScenes;

		public int UsedCost { get; private set; } = 0;
		public int KnownMaxTotalCost { get; private set; } = 0;
		public Session? CurrentSessionState { get; private set; }

		public Transport SessionTransport { get; private set; }

		// Webhooks are only in ASP.NET so not compatible without external library
		private ClientWebSocket webSocket;
		private Task recieverThread;
		private ClientWebSocket webSocketReconnector;
		private Task recieverThreadReconnector;

		private readonly byte[] buffer = new byte[BUFFER_SIZE];

		private readonly Dictionary<string, Subscription> SessionSubscriptions = new Dictionary<string, Subscription>();

		private Task StartUpTask;

		private CancellationTokenSource cts = new CancellationTokenSource();

		private bool reconnecting = false;

		private TwitchAPIClient apiClient;

		private readonly HashSet<string> receivedMessageIds = new HashSet<string>();

		protected override void Awake() {
			if (this.EstablishSingleton(true)) { }
		}

		private void OnDisable() {
			this.CloseWebsocket().ConfigureAwait(false);
		}

		private void OnDestroy() {
			FieldInfo[] classFields = typeof(TwitchEventSubClient).GetFields();
			for (int x = 0; x < classFields.Length; x++) {
				FieldInfo fieldInfo = classFields[x];
				if (typeof(UnityEventBase).IsAssignableFrom(fieldInfo.FieldType)) {
					object field = fieldInfo.GetValue(this);
					if (field != null) {
						UnityEventBase baseEvent = (UnityEventBase)field;
						baseEvent.RemoveAllListeners();
					}
				}
			}
		}

		private void EndCancelTokens() {
			if (this.cts != null) {
				try {
					this.cts.Cancel();
					this.cts.Dispose();
					this.cts = null;
				} catch {
					if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
						DebugManager.LogMessage("EventSub cancel token was ended.".RichTextColour("orange"));
					}
				}
			}
		}

		/// <summary>
		/// Ends current websocket session
		/// </summary>
		/// <returns></returns>
		public async Task CloseWebsocket() {
			if (this.webSocket != null && this.webSocket.State == WebSocketState.Open) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("Closing down EventSub websocket connection.");
				}

				await this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Functionality Ending", this.cts.Token);
			}
			this.webSocket?.Dispose();

			this.receivedMessageIds.Clear();
			this.CurrentSessionState = null;

			this.StartUpTask?.Dispose();

			this.EndCancelTokens();
		}

		/// <summary>
		/// Returns a copy of the current sessions subscriptions
		/// </summary>
		public Subscription[] GetCurrentSubscriptions() {
			return this.SessionSubscriptions.Values.ToArray();
		}

		/// <summary>
		/// Get subscription data by ID
		/// </summary>
		public bool GetSubscription(string subscriptionId, out Subscription sub) {
			return this.SessionSubscriptions.TryGetValue(subscriptionId, out sub);
		}

		/// <summary>
		/// Starts up the Event sub
		/// </summary>
		public Task BeginConnectionSession() {
			return this.BeginConnectionSession(false, false);
		}

		/// <summary>
		/// Starts up the Event sub
		/// </summary>
		/// <param name="restart">If you want to force restart the server</param>
		/// <param name="resubscribe">If you want to resubscribe to existing subscriptions</param>
		public async Task BeginConnectionSession(bool restart = false, bool resubscribe = false, params (TwitchEventSubSubscriptionsEnum, Condition)[] immedieteSubs) {
			this.cts ??= new CancellationTokenSource();

			if (!TwitchAPIClient.GetInstance(out TwitchAPIClient client)) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"TwitchEventSubClient; TwitchAPIClient not found, Startup cancelled");
					return;
				}
			}

			if (!client.CheckOAuthExistsAndInDate(this.EventSubToken)) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"TwitchEventSubClient; Token not ready, waiting.");
				}
				do {
					await Task.Delay(1000);
					if (this.cts.IsCancellationRequested) {
						return;
					}
				} while (!client.CheckOAuthExistsAndInDate(this.EventSubToken));
			}

			if (restart || !EventSubConnectionActive) {
				this.CurrentSessionState = null;

				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"TwitchEventSubClient; {(restart ? "restarting" : "starting")} connection");
				}
				if (this.StartUpTask == null || this.StartUpTask.IsCompleted) {
					this.StartUpTask = this.BuildSocketAndThread();
				}
			}

			await this.StartUpTask;

			// Hold until connection has been made and ready to receive
			while (!this.CurrentSessionState.HasValue) {
				await Task.Delay(1000);
				if (this.cts.IsCancellationRequested) {
					return;
				}
			}

			if (this.cts.IsCancellationRequested) {
				return;
			}

			if (resubscribe) {
				await this.ResubscribeToSessionEvents();
			}

			if (this.cts.IsCancellationRequested) {
				return;
			}

			// Subs not already subscribed to, to then sub too
			foreach ((TwitchEventSubSubscriptionsEnum, Condition) sub in immedieteSubs) {
				if (this.cts.IsCancellationRequested) {
					return;
				}

				await this.SubscribeToEvent(sub.Item1, sub.Item2);
			}
		}


		/// <summary>
		/// Builds the WebSocket and the thread that will receive the data
		/// </summary>
		private async Task BuildSocketAndThread() {
			try {
				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage("TwitchEventSubClient; build socket and thread");
				}
				if (this.webSocket != null && this.webSocket.State == WebSocketState.Open) {
					await this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket rebuild requested", this.cts.Token);
					this.webSocket.Dispose();
				}

				this.webSocket = await this.BuildNewConnection(this.GetSocketAddress);

				if (this.recieverThread != null && !this.recieverThread.IsCompleted) {
					this.EndCancelTokens();
					this.cts = new CancellationTokenSource();
				}
				this.recieverThread = this.ManageResponseAsync(this.webSocket);
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
			}
		}

		/// <summary>
		/// Builds a new websocket object
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		private async Task<ClientWebSocket> BuildNewConnection(string uri) {
			try {
				if (this.LogDebugLevel > DebugManager.DebugLevel.Necessary) {
					DebugManager.LogMessage("TwitchEventSubClient; building connection");
				}
				ClientWebSocket ws = new ClientWebSocket();
				ws.Options.SetBuffer(BUFFER_SIZE, 100);
				await ws.ConnectAsync(new Uri(uri), this.cts.Token);

				return ws;
			} catch (Exception ex) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				throw;
			}
		}

		/// <summary>
		/// Websocket response reading Task (Warning: long term task)
		/// </summary>
		/// <param name="socketReference"></param>
		/// <returns></returns>
		private async Task ManageResponseAsync(ClientWebSocket socketReference) {
			if (this.LogDebugLevel == DebugManager.DebugLevel.Full) {
				DebugManager.LogMessage("TwitchEventSubClient; building response thread");
			}
			while (socketReference.State == WebSocketState.Open && !this.cts.IsCancellationRequested) {
				using (MemoryStream ms = new MemoryStream()) {
					WebSocketReceiveResult result;
					do {
						result = await socketReference.ReceiveAsync(this.buffer, this.cts.Token);
						if (this.cts.IsCancellationRequested) {
							goto Close;
						}
						ms.Write(this.buffer, 0, result.Count);
					}
					while (!result.EndOfMessage);

					ms.Seek(0, SeekOrigin.Begin);

					if (result.MessageType == WebSocketMessageType.Text) {
						using (StreamReader reader = new StreamReader(ms, Encoding.UTF8)) {
							await this.ParseSocketMessage(reader.ReadToEnd());
						}
					}
					else if (result.MessageType == WebSocketMessageType.Close) {
						await socketReference.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
					}
				}
			}
			Close:
			await socketReference.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
		}

		/// <summary>
		/// Parse json data received into an Object and process required action from data
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private async Task ParseSocketMessage(string message) {
			JsonValue parsedMessage = JsonReader.Parse(message);
			SocketResponse response;
			try {
				response = new SocketResponse(parsedMessage);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
				return;
			}
			switch (this.LogDebugLevel) {
				case DebugManager.DebugLevel.Necessary:
					JsonValue json = new JsonObject() {
						{ TwitchWords.SUBSCRIPTION_TYPE, response.metadata.subscription_type },
						{ TwitchWords.MESSAGE_TIMESTAMP, response.metadata.message_timestamp }
					};
					DebugManager.LogMessage("TwitchEventSubClient; Message Received: " + json.AsString.RichTextItalic());
					break;
				case DebugManager.DebugLevel.Normal:
					DebugManager.LogMessage("TwitchEventSubClient; Message Received: " + response.metadata.ToJSON().ToString().RichTextItalic());
					break;
				case DebugManager.DebugLevel.Full:
					DebugManager.LogMessage("TwitchEventSubClient; Message Received: " + message.RichTextItalic());
					break;
			}

			if (!this.receivedMessageIds.Contains(response.metadata.message_id)) {
				this.receivedMessageIds.Add(response.metadata.message_id);

				switch (response.metadata.message_type) {
					case TwitchWords.SESSION_WELCOME: // First Connection
						this.CurrentSessionState = response.payload.session;
						if (this.reconnecting) {
							await this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnection", this.cts.Token);
							this.recieverThread?.Dispose();
							this.webSocket = this.webSocketReconnector;
							this.recieverThread = this.recieverThreadReconnector;
						}
						this.SessionTransport = new Transport() {
							method = "websocket",
							session_id = this.CurrentSessionState?.id
						};

						this.reconnecting = false;
						break;
					case TwitchWords.SESSION_KEEPALIVE: // keepalive_timeout_seconds expired (Session class) 

						break;
					case TwitchWords.NOTIFICATION: // Events
						MainThreadDispatchQueue.Enqueue(() => this.FireEventDelegate(response.payload.eventData));
						break;
					case TwitchWords.SESSION_RECONNECT: // Notified to reconnect
						this.CurrentSessionState = response.payload.session;
						this.webSocketReconnector = await this.BuildNewConnection(response.payload.session.reconnect_url);
						this.recieverThreadReconnector = this.ManageResponseAsync(this.webSocket);
						this.reconnecting = true;
						break;
					case TwitchWords.REVOCATION: // Subscription removed by Twitch
						switch (response.payload.subscription.status) {
							case TwitchWords.USER_REMOVED: // User deleted
								MainThreadDispatchQueue.Enqueue(() => this.OnUserRemovedSubscriptionRevoked?.Invoke(response.payload.subscription));
								break;
							case TwitchWords.AUTHORIZATION_REVOKED: // Auth to connect was invalidated
								this.SessionSubscriptions.Remove(response.payload.subscription.id);
								MainThreadDispatchQueue.Enqueue(() => this.OnAuthorizationRemoved?.Invoke(response.payload.subscription));
								break;
							case TwitchWords.VERSION_REMOVED: // Subscription type or version is no longer supported
								MainThreadDispatchQueue.Enqueue(() => this.OnVersionRemovedSubscriptionRevoked?.Invoke(response.payload.subscription));
								break;
						}
						break;
				}
			}
		}

		private void FireEventDelegate(IEvent builtEvent) {
			Type thisType = this.GetType();
			Type eventBody = builtEvent.GetType();

			FieldInfo foundEventField = thisType.GetField($"On{eventBody.Name}");

			if (foundEventField == null) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage("EventSub Event not found for body of type: " + eventBody.Name, DebugManager.ErrorLevel.Error);
				}
				return;
			}

			object unityEvent = foundEventField.GetValue(this);
			MethodInfo invokeMethod = unityEvent.GetType().GetMethod("Invoke");
			invokeMethod.Invoke(unityEvent, new object[] { Convert.ChangeType(builtEvent, eventBody) });
		}

		public async Task SubscribeToEvent<T>(Condition condition, APIScopeWarning scopeCheck = APIScopeWarning.WarnOnMissing) where T : IEvent, new() {
			if (apiClient == null && !TwitchAPIClient.GetInstance(out apiClient)) {
				DebugManager.LogMessage($"TwitchEventSubClient; Twitch API Client was not found, request aborted.", DebugManager.ErrorLevel.Error);
				return;
			}

			TokenInstance credentials = this.EventSubToken;
			if (credentials == null) {
				if (apiClient.DefaultAPIToken == null) {
					DebugManager.LogMessage($"TwitchEventSubClient; No credentials were found to make the subscription request, request aborted.", DebugManager.ErrorLevel.Error);
					return;
				}
				credentials = apiClient.DefaultAPIToken;
			}

			T eventBody = new T();
			credentials.PerformScopeCheck(scopeCheck, eventBody);
			string eventType = eventBody.Enum.ToTwitchNameString();
			
			string JSON = CreateEventSubSubscription.BuildDataJson(
					eventType,
					eventBody.Enum.ToVersionString(),
					condition,
					this.SessionTransport
				);

			TwitchAPIDataContainer<CreateEventSubSubscription> returnedData = await TwitchAPIClient.MakeTwitchAPIRequestAsync<CreateEventSubSubscription>(
				credentials,
				HeaderValues: new (string, string)[] {
					CreateEventSubSubscription.CONTENT_TYPE,
				},
				RawData: JSON,
				cancelToken: this.cts.Token);
			if (returnedData.HasErrored) {
				if (this.LogDebugLevel != DebugManager.DebugLevel.None) {
					DebugManager.LogMessage($"TwitchEventSubClient; Failed to subscribe event; {{{eventType}}} with conditions; {JsonWriter.Serialize(condition)}. Error message: {{{returnedData.ErrorText}}}", DebugManager.ErrorLevel.Error);
				}
			}
			else {
				Subscription createdSub = new Subscription(returnedData.data[0]);
				this.SessionSubscriptions[createdSub.id] = createdSub;

				if (returnedData.EventSubData.max_total_cost > int.MinValue) {
					this.KnownMaxTotalCost = returnedData.EventSubData.max_total_cost;
				}
				if (returnedData.EventSubData.total_cost > int.MinValue) {
					this.UsedCost = returnedData.EventSubData.total_cost;
				}
			}
		}

		public Task SubscribeToEvent(string type, Condition condition) {
			TwitchEventSubSubscriptionsEnum enumValue = type.GetEnumFromTwitchName();
			return this.SubscribeToEvent(enumValue, condition);
		}

		public Task SubscribeToEvent(TwitchEventSubSubscriptionsEnum type, Condition condition) {
			try {
				Type t = typeof(TwitchEventSubClient);
				string methodName = nameof(SubscribeToEvent);
				foreach (MethodInfo a in t.GetMethods()) {
					if (a.ContainsGenericParameters && a.Name == methodName) {
						return (Task)a.MakeGenericMethod(type.ToLinkedType()).Invoke(this, new object[] { condition, APIScopeWarning.None });
					}
				}
				throw new Exception("No method found to execute subscription.");
			} catch (Exception ex) {
				if (this.LogDebugLevel > DebugManager.DebugLevel.None) {
					DebugManager.LogMessage(ex);
				}
				return Task.CompletedTask;
			}
		}

		/// <summary>
		/// Closes and resubscribes current subscriptions / Uses stored subscriptions to resubscribe
		/// </summary>
		/// <returns></returns>
		public async Task ResubscribeToSessionEvents() {
			foreach (Subscription sub in this.SessionSubscriptions.Values.ToArray()) {
				if (this.cts.IsCancellationRequested) {
					return;
				}
				this.SessionSubscriptions.Remove(sub.id);
				await this.SubscribeToEvent(sub.type, sub.condition);
			}
		}

		/// <summary>
		/// Makes the EventSub unsubscribe from the registered subscriptions
		/// </summary>
		/// <param name="Ids">Subscriptions</param>
		public void UnsubscribeFromEvents(params string[] Ids) {
			if (Ids.Length == 0) {
				return;
			}
			if (apiClient == null && TwitchAPIClient.GetInstance(out apiClient)) {
				DebugManager.LogMessage($"TwitchEventSubClient; Twitch API Client was not found, request aborted.", DebugManager.ErrorLevel.Error);
				return;
			}

			TokenInstance credentials = this.EventSubToken;
			if (credentials == null) {
				if (apiClient.DefaultAPIToken == null) {
					DebugManager.LogMessage($"TwitchEventSubClient; No credentials were found to make the subscription request, request aborted.", DebugManager.ErrorLevel.Error);
					return;
				}
				credentials = apiClient.DefaultAPIToken;
			}

			foreach (string id in Ids) {
				if (this.SessionSubscriptions.ContainsKey(id)) {
					this.StartCoroutine(TwitchAPIClient.MakeTwitchAPIRequest<DeleteEventSubSubscription>(
						credentials,
						QueryParameters: new (string, string)[] {
							(DeleteEventSubSubscription.ID, id)
						},
						SuccessCallback: r => {
							this.SessionSubscriptions.Remove(id);
						}
					));
				}
			}
		}
	}
}
