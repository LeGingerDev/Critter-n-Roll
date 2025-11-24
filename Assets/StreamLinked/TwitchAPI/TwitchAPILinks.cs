namespace ScoredProductions.StreamLinked.API {

	/// <summary>
	/// All addresses used by Twitch API
	/// </summary>
	public static class TwitchAPILinks {
		public const string TwitchAPIWebsite = "https://api.twitch.tv/";

		public const string StartCommercial = "https://api.twitch.tv/helix/channels/commercial";
		public const string GetAdSchedule = "https://api.twitch.tv/helix/channels/ads";
		public const string SnoozeNextAd = "https://api.twitch.tv/helix/channels/ads/schedule/snooze";
		public const string GetExtensionAnalytics = "https://api.twitch.tv/helix/analytics/extensions";
		public const string GetGameAnalytics = "https://api.twitch.tv/helix/analytics/games";
		public const string GetBitsLeaderboard = "https://api.twitch.tv/helix/bits/leaderboard";
		public const string GetCheermotes = "https://api.twitch.tv/helix/bits/cheermotes";
		public const string GetExtensionTransactions = "https://api.twitch.tv/helix/extensions/transactions";
		public const string ChannelInformation = "https://api.twitch.tv/helix/channels";
		public const string GetChannelEditors = "https://api.twitch.tv/helix/channels/editors";
		public const string GetFollowedChannels = "https://api.twitch.tv/helix/channels/followed";
		public const string GetChannelFollowers = "https://api.twitch.tv/helix/channels/followers";
		public const string CustomRewards = "https://api.twitch.tv/helix/channel_points/custom_rewards";
		public const string CustomRewardRedemption = "https://api.twitch.tv/helix/channel_points/custom_rewards/redemptions";
		public const string GetCharityCampaign = "https://api.twitch.tv/helix/charity/campaigns";
		public const string GetCharityCampaignDonations = "https://api.twitch.tv/helix/charity/donations";
		public const string GetChatters = "https://api.twitch.tv/helix/chat/chatters";
		public const string GetUserEmotes = "https://api.twitch.tv/helix/chat/emotes/user";
		public const string GetChannelEmotes = "https://api.twitch.tv/helix/chat/emotes";
		public const string GetGlobalEmotes = "https://api.twitch.tv/helix/chat/emotes/global";
		public const string GetEmoteSets = "https://api.twitch.tv/helix/chat/emotes/set";
		public const string GetChannelChatBadges = "https://api.twitch.tv/helix/chat/badges";
		public const string GetGlobalChatBadges = "https://api.twitch.tv/helix/chat/badges/global";
		public const string ChatSettings = "https://api.twitch.tv/helix/chat/settings";
		public const string SendChatAnnouncement = "https://api.twitch.tv/helix/chat/announcements";
		public const string SendAShoutout = "https://api.twitch.tv/helix/chat/shoutouts";
		public const string UserChatColor = "https://api.twitch.tv/helix/chat/color";
		public const string Clips = "https://api.twitch.tv/helix/clips";
		public const string GetContentClassificationLabels = "https://api.twitch.tv/helix/content_classification_labels";
		public const string DropsEntitlements = "https://api.twitch.tv/helix/entitlements/drops";
		public const string ExtensionConfigurationSegment = "https://api.twitch.tv/helix/extensions/configurations";
		public const string SetExtensionRequiredConfiguration = "https://api.twitch.tv/helix/extensions/required_configuration";
		public const string SendExtensionPubSubMessage = "https://api.twitch.tv/helix/extensions/pubsub";
		public const string GetExtensionLiveChannels = "https://api.twitch.tv/helix/extensions/live";
		public const string ExtensionSecrets = "https://api.twitch.tv/helix/extensions/jwt/secrets";
		public const string SendExtensionChatMessage = "https://api.twitch.tv/helix/extensions/chat";
		public const string GetExtensions = "https://api.twitch.tv/helix/extensions";
		public const string GetReleasedExtensions = "https://api.twitch.tv/helix/extensions/released";
		public const string ExtensionBitsProducts = "https://api.twitch.tv/helix/bits/extensions";
		public const string EventSubSubscription = "https://api.twitch.tv/helix/eventsub/subscriptions";
		public const string GetTopGames = "https://api.twitch.tv/helix/games/top";
		public const string GetGames = "https://api.twitch.tv/helix/games";
		public const string GetCreatorGoals = "https://api.twitch.tv/helix/goals";
		public const string ChannelGuestStarSettings = "https://api.twitch.tv/helix/guest_star/channel_settings";
		public const string GuestStarSession = "https://api.twitch.tv/helix/guest_star/session";
		public const string GuestStarInvites = "https://api.twitch.tv/helix/guest_star/invites";
		public const string GuestStarSlot = "https://api.twitch.tv/helix/guest_star/slot";
		public const string UpdateGuestStarSlotSettings = "https://api.twitch.tv/helix/guest_star/slot_settings";
		public const string GetHypeTrainEvents = "https://api.twitch.tv/helix/hypetrain/events";
		public const string CheckAutoModStatus = "https://api.twitch.tv/helix/moderation/enforcements/status";
		public const string ManageHeldAutoMessages = "https://api.twitch.tv/helix/moderation/automod/message";
		public const string AutoModSettings = "https://api.twitch.tv/helix/moderation/automod/settings";
		public const string GetBannedUsers = "https://api.twitch.tv/helix/moderation/banned";
		public const string BanUser = "https://api.twitch.tv/helix/moderation/bans";
		public const string BlockedTerms = "https://api.twitch.tv/helix/moderation/blocked_terms";
		public const string DeleteChatMessages = "https://api.twitch.tv/helix/moderation/chat";
		public const string Moderators = "https://api.twitch.tv/helix/moderation/moderators";
		public const string WarnUser = "https://api.twitch.tv/helix/moderation/warnings";
		public const string VIPs = "https://api.twitch.tv/helix/channels/vips";
		public const string ShieldModeStatus = "https://api.twitch.tv/helix/moderation/shield_mode";
		public const string Polls = "https://api.twitch.tv/helix/polls";
		public const string Predictions = "https://api.twitch.tv/helix/predictions";
		public const string Raids = "https://api.twitch.tv/helix/raids";
		public const string GetChannelStreamSchedule = "https://api.twitch.tv/helix/schedule";
		public const string GetChanneliCalendar = "https://api.twitch.tv/helix/schedule/icalendar";
		public const string UpdateChannelStreamSchedule = "https://api.twitch.tv/helix/schedule/settings";
		public const string ChannelStreamScheduleSegment = "https://api.twitch.tv/helix/schedule/segment";
		public const string SearchCategories = "https://api.twitch.tv/helix/search/categories";
		public const string SearchChannels = "https://api.twitch.tv/helix/search/channels";
		public const string GetStreamKey = "https://api.twitch.tv/helix/streams/key";
		public const string GetStreams = "https://api.twitch.tv/helix/streams";
		public const string GetFollowedStreams = "https://api.twitch.tv/helix/streams/followed";
		public const string CreateStreamMarker = "https://api.twitch.tv/helix/streams/markers";
		public const string GetStreamMarkers = "https://api.twitch.tv/helix/streams/markers";
		public const string GetBroadcasterSubscriptions = "https://api.twitch.tv/helix/subscriptions";
		public const string CheckUserSubscription = "https://api.twitch.tv/helix/subscriptions/user";
		public const string GetChannelTeams = "https://api.twitch.tv/helix/teams/channel";
		public const string GetTeams = "https://api.twitch.tv/helix/teams";
		public const string Users = "https://api.twitch.tv/helix/users";
		public const string UnbanRequests = "https://api.twitch.tv/helix/moderation/unban_requests";
		public const string UserBlockList = "https://api.twitch.tv/helix/users/blocks";
		public const string GetUserExtensions = "https://api.twitch.tv/helix/users/extensions/list";
		public const string UserActiveExtensions = "https://api.twitch.tv/helix/users/extensions";
		public const string Videos = "https://api.twitch.tv/helix/videos";
		public const string SendWhisper = "https://api.twitch.tv/helix/whispers";
		public const string GetModeratedChannels = "https://api.twitch.tv/helix/moderation/channels";
		public const string Conduits = "https://api.twitch.tv/helix/eventsub/conduits";
		public const string ConduitShards = "https://api.twitch.tv/helix/eventsub/conduits/shards";
		public const string SendChatMessage = "https://api.twitch.tv/helix/chat/messages";
		public const string Session = "https://api.twitch.tv/helix/shared_chat/session";

		// Non Reference page links
		public const string GetAuthToken = "https://id.twitch.tv/oauth2/token";
		public const string GetAuthData = "https://id.twitch.tv/oauth2/authorize";
		public const string GetTokenValidation = "https://id.twitch.tv/oauth2/validate";
		public const string GetDeviceToken = "https://id.twitch.tv/oauth2/device";
	}
}