using System;

using ScoredProductions.StreamLinked.API.Ads;
using ScoredProductions.StreamLinked.API.Analytics;
using ScoredProductions.StreamLinked.API.Auth;
using ScoredProductions.StreamLinked.API.Bits;
using ScoredProductions.StreamLinked.API.CCLs;
using ScoredProductions.StreamLinked.API.Channel_Points;
using ScoredProductions.StreamLinked.API.Channels;
using ScoredProductions.StreamLinked.API.Charity;
using ScoredProductions.StreamLinked.API.Chat;
using ScoredProductions.StreamLinked.API.Clips;
using ScoredProductions.StreamLinked.API.Conduits;
using ScoredProductions.StreamLinked.API.Entitlements;
using ScoredProductions.StreamLinked.API.EventSub;
using ScoredProductions.StreamLinked.API.Extensions;
using ScoredProductions.StreamLinked.API.Games;
using ScoredProductions.StreamLinked.API.Guest_Star;
using ScoredProductions.StreamLinked.API.Hype_Train;
using ScoredProductions.StreamLinked.API.Moderation;
using ScoredProductions.StreamLinked.API.Polls;
using ScoredProductions.StreamLinked.API.Predictions;
using ScoredProductions.StreamLinked.API.Raids;
using ScoredProductions.StreamLinked.API.Schedule;
using ScoredProductions.StreamLinked.API.Search;
using ScoredProductions.StreamLinked.API.Streams;
using ScoredProductions.StreamLinked.API.Subscriptions;
using ScoredProductions.StreamLinked.API.Teams;
using ScoredProductions.StreamLinked.API.Users;
using ScoredProductions.StreamLinked.API.Videos;
using ScoredProductions.StreamLinked.API.Whispers;

namespace ScoredProductions.StreamLinked.API {

	/* 
		C# Interactive Get next value to add enum

		You can do a right click on the project in the solution explorer and than click `Initialize interactive with project".
		var NextValue = Enum.GetValues(typeof(ScoredProductions.StreamLinked.API.TwitchAPIClassEnum)).Cast<ScoredProductions.StreamLinked.API.TwitchAPIClassEnum>().Max();
		Console.Write(((int)NextValue) + 1);
	 */

	[Serializable]
	public enum TwitchAPIClassEnum {
		// Ads
		[TwitchAPIClassInformation(typeof(GetAdSchedule), TwitchResourceEnum.Ads)]
		GetAdSchedule = 0,
		[TwitchAPIClassInformation(typeof(SnoozeNextAd), TwitchResourceEnum.Ads)]
		SnoozeNextAd = 1,
		[TwitchAPIClassInformation(typeof(StartCommercial), TwitchResourceEnum.Ads)]
		StartCommercial = 2,
		// Analytics
		[TwitchAPIClassInformation(typeof(GetExtensionAnalytics), TwitchResourceEnum.Analytics)]
		GetExtensionAnalytics = 3,
		[TwitchAPIClassInformation(typeof(GetGameAnalytics), TwitchResourceEnum.Analytics)]
		GetGameAnalytics = 4,
		// Auth
		[TwitchAPIClassInformation(typeof(GetAuthorizationCodeToken), TwitchResourceEnum.Auth)]
		GetAuthorizationCodeToken = 5,
		[TwitchAPIClassInformation(typeof(GetClientCredentialsGrantFlow), TwitchResourceEnum.Auth)]
		GetClientCredentialsGrantFlow = 6,
		[TwitchAPIClassInformation(typeof(GetDeviceCodeGrantAuthorisation), TwitchResourceEnum.Auth)]
		GetDeviceCodeGrantAuthorisation = 7,
		[TwitchAPIClassInformation(typeof(GetDeviceCodeGrantFlow), TwitchResourceEnum.Auth)]
		GetDeviceCodeGrantFlow = 8,
		[TwitchAPIClassInformation(typeof(GetTokenRefresh), TwitchResourceEnum.Auth)]
		GetTokenRefresh = 9,
		[TwitchAPIClassInformation(typeof(GetValidatedTokenInfo), TwitchResourceEnum.Auth)]
		GetValidatedTokenInfo = 10,
		// Bits
		[TwitchAPIClassInformation(typeof(GetBitsLeaderboard), TwitchResourceEnum.Bits)]
		GetBitsLeaderboard = 11,
		[TwitchAPIClassInformation(typeof(GetCheermotes), TwitchResourceEnum.Bits)]
		GetCheermotes = 12,
		[TwitchAPIClassInformation(typeof(GetExtensionTransactions), TwitchResourceEnum.Bits)]
		GetExtensionTransactions = 13,
		// CCLs
		[TwitchAPIClassInformation(typeof(GetContentClassificationLabels), TwitchResourceEnum.CCLs)]
		GetContentClassificationLabels = 14,
		// Channel_Points
		[TwitchAPIClassInformation(typeof(CreateCustomRewards), TwitchResourceEnum.Channel_Points)]
		CreateCustomRewards = 15,
		[TwitchAPIClassInformation(typeof(DeleteCustomReward), TwitchResourceEnum.Channel_Points)]
		DeleteCustomReward = 16,
		[TwitchAPIClassInformation(typeof(GetCustomReward), TwitchResourceEnum.Channel_Points)]
		GetCustomReward = 17,
		[TwitchAPIClassInformation(typeof(GetCustomRewardRedemption), TwitchResourceEnum.Channel_Points)]
		GetCustomRewardRedemption = 18,
		[TwitchAPIClassInformation(typeof(UpdateCustomReward), TwitchResourceEnum.Channel_Points)]
		UpdateCustomReward = 19,
		[TwitchAPIClassInformation(typeof(UpdateRedemptionStatus), TwitchResourceEnum.Channel_Points)]
		UpdateRedemptionStatus = 20,
		// Channels
		[TwitchAPIClassInformation(typeof(GetChannelEditors), TwitchResourceEnum.Channels)]
		GetChannelEditors = 21,
		[TwitchAPIClassInformation(typeof(GetChannelFollowers), TwitchResourceEnum.Channels)]
		GetChannelFollowers = 22,
		[TwitchAPIClassInformation(typeof(GetChannelInformation), TwitchResourceEnum.Channels)]
		GetChannelInformation = 23,
		[TwitchAPIClassInformation(typeof(GetFollowedChannels), TwitchResourceEnum.Channels)]
		GetFollowedChannels = 24,
		[TwitchAPIClassInformation(typeof(ModifyChannelInformation), TwitchResourceEnum.Channels)]
		ModifyChannelInformation = 25,
		// Charity
		[TwitchAPIClassInformation(typeof(GetCharityCampaign), TwitchResourceEnum.Charity)]
		GetCharityCampaign = 26,
		[TwitchAPIClassInformation(typeof(GetCharityCampaignDonations), TwitchResourceEnum.Charity)]
		GetCharityCampaignDonations = 27,
		// Chat
		[TwitchAPIClassInformation(typeof(GetChannelChatBadges), TwitchResourceEnum.Chat)]
		GetChannelChatBadges = 28,
		[TwitchAPIClassInformation(typeof(GetChannelEmotes), TwitchResourceEnum.Chat)]
		GetChannelEmotes = 29,
		[TwitchAPIClassInformation(typeof(GetChatSettings), TwitchResourceEnum.Chat)]
		GetChatSettings = 30,
		[TwitchAPIClassInformation(typeof(GetChatters), TwitchResourceEnum.Chat)]
		GetChatters = 31,
		[TwitchAPIClassInformation(typeof(GetEmoteSets), TwitchResourceEnum.Chat)]
		GetEmoteSets = 32,
		[TwitchAPIClassInformation(typeof(GetGlobalChatBadges), TwitchResourceEnum.Chat)]
		GetGlobalChatBadges = 33,
		[TwitchAPIClassInformation(typeof(GetGlobalEmotes), TwitchResourceEnum.Chat)]
		GetGlobalEmotes = 34,
		[TwitchAPIClassInformation(typeof(GetSharedChatSession), TwitchResourceEnum.Chat)]
		GetSharedChatSession = 35,
		[TwitchAPIClassInformation(typeof(GetUserChatColor), TwitchResourceEnum.Chat)]
		GetUserChatColor = 36,
		[TwitchAPIClassInformation(typeof(GetUserEmotes), TwitchResourceEnum.Chat)]
		GetUserEmotes = 37,
		[TwitchAPIClassInformation(typeof(SendAShoutout), TwitchResourceEnum.Chat)]
		SendAShoutout = 38,
		[TwitchAPIClassInformation(typeof(SendChatAnnouncement), TwitchResourceEnum.Chat)]
		SendChatAnnouncement = 39,
		[TwitchAPIClassInformation(typeof(SendChatMessage), TwitchResourceEnum.Chat)]
		SendChatMessage = 40,
		[TwitchAPIClassInformation(typeof(UpdateChatSettings), TwitchResourceEnum.Chat)]
		UpdateChatSettings = 41,
		[TwitchAPIClassInformation(typeof(UpdateUserChatColor), TwitchResourceEnum.Chat)]
		UpdateUserChatColor = 42,
		// Clips
		[TwitchAPIClassInformation(typeof(CreateClip), TwitchResourceEnum.Clips)]
		CreateClip = 43,
		[TwitchAPIClassInformation(typeof(GetClips), TwitchResourceEnum.Clips)]
		GetClips = 44,
		// Conduits
		[TwitchAPIClassInformation(typeof(CreateConduits), TwitchResourceEnum.Conduits)]
		CreateConduits = 45,
		[TwitchAPIClassInformation(typeof(DeleteConduits), TwitchResourceEnum.Conduits)]
		DeleteConduits = 46,
		[TwitchAPIClassInformation(typeof(GetConduits), TwitchResourceEnum.Conduits)]
		GetConduits = 47,
		[TwitchAPIClassInformation(typeof(GetConduitShards), TwitchResourceEnum.Conduits)]
		GetConduitShards = 48,
		[TwitchAPIClassInformation(typeof(UpdateConduits), TwitchResourceEnum.Conduits)]
		UpdateConduits = 49,
		[TwitchAPIClassInformation(typeof(UpdateConduitShards), TwitchResourceEnum.Conduits)]
		UpdateConduitShards = 50,
		// Entitlements
		[TwitchAPIClassInformation(typeof(GetDropsEntitlements), TwitchResourceEnum.Entitlements)]
		GetDropsEntitlements = 51,
		[TwitchAPIClassInformation(typeof(UpdateDropsEntitlements), TwitchResourceEnum.Entitlements)]
		UpdateDropsEntitlements = 52,
		// EventSub
		[TwitchAPIClassInformation(typeof(CreateEventSubSubscription), TwitchResourceEnum.EventSub)]
		CreateEventSubSubscription = 53,
		[TwitchAPIClassInformation(typeof(DeleteEventSubSubscription), TwitchResourceEnum.EventSub)]
		DeleteEventSubSubscription = 54,
		[TwitchAPIClassInformation(typeof(GetEventSubSubscriptions), TwitchResourceEnum.EventSub)]
		GetEventSubSubscriptions = 55,
		// Extensions
		[TwitchAPIClassInformation(typeof(CreateExtensionSecret), TwitchResourceEnum.Extensions)]
		CreateExtensionSecret = 56,
		[TwitchAPIClassInformation(typeof(GetExtensionBitsProducts), TwitchResourceEnum.Extensions)]
		GetExtensionBitsProducts = 57,
		[TwitchAPIClassInformation(typeof(GetExtensionConfigurationSegment), TwitchResourceEnum.Extensions)]
		GetExtensionConfigurationSegment = 58,
		[TwitchAPIClassInformation(typeof(GetExtensionLiveChannels), TwitchResourceEnum.Extensions)]
		GetExtensionLiveChannels = 59,
		[TwitchAPIClassInformation(typeof(GetExtensions), TwitchResourceEnum.Extensions)]
		GetExtensions = 60,
		[TwitchAPIClassInformation(typeof(GetExtensionSecrets), TwitchResourceEnum.Extensions)]
		GetExtensionSecrets = 61,
		[TwitchAPIClassInformation(typeof(GetReleasedExtensions), TwitchResourceEnum.Extensions)]
		GetReleasedExtensions = 62,
		[TwitchAPIClassInformation(typeof(SendExtensionChatMessage), TwitchResourceEnum.Extensions)]
		SendExtensionChatMessage = 63,
		[TwitchAPIClassInformation(typeof(SendExtensionPubSubMessage), TwitchResourceEnum.Extensions)]
		SendExtensionPubSubMessage = 64,
		[TwitchAPIClassInformation(typeof(SetExtensionConfigurationSegment), TwitchResourceEnum.Extensions)]
		SetExtensionConfigurationSegment = 65,
		[TwitchAPIClassInformation(typeof(SetExtensionRequiredConfiguration), TwitchResourceEnum.Extensions)]
		SetExtensionRequiredConfiguration = 66,
		[TwitchAPIClassInformation(typeof(UpdateExtensionBitsProduct), TwitchResourceEnum.Extensions)]
		UpdateExtensionBitsProduct = 67,
		// Games
		[TwitchAPIClassInformation(typeof(GetGames), TwitchResourceEnum.Games)]
		GetGames = 68,
		[TwitchAPIClassInformation(typeof(GetTopGames), TwitchResourceEnum.Games)]
		GetTopGames = 69,
		// Goals
		[TwitchAPIClassInformation(typeof(GetTopGames), TwitchResourceEnum.Goals)]
		GetCreatorGoals = 70,
		// Guest_Star
		[TwitchAPIClassInformation(typeof(AssignGuestStarSlot), TwitchResourceEnum.Guest_Star)]
		AssignGuestStarSlot = 71,
		[TwitchAPIClassInformation(typeof(CreateGuestStarSession), TwitchResourceEnum.Guest_Star)]
		CreateGuestStarSession = 72,
		[TwitchAPIClassInformation(typeof(DeleteGuestStarInvite), TwitchResourceEnum.Guest_Star)]
		DeleteGuestStarInvite = 73,
		[TwitchAPIClassInformation(typeof(DeleteGuestStarSlot), TwitchResourceEnum.Guest_Star)]
		DeleteGuestStarSlot = 74,
		[TwitchAPIClassInformation(typeof(EndGuestStarSession), TwitchResourceEnum.Guest_Star)]
		EndGuestStarSession = 75,
		[TwitchAPIClassInformation(typeof(GetChannelGuestStarSettings), TwitchResourceEnum.Guest_Star)]
		GetChannelGuestStarSettings = 76,
		[TwitchAPIClassInformation(typeof(GetGuestStarInvites), TwitchResourceEnum.Guest_Star)]
		GetGuestStarInvites = 77,
		[TwitchAPIClassInformation(typeof(GetGuestStarSession), TwitchResourceEnum.Guest_Star)]
		GetGuestStarSession = 78,
		[TwitchAPIClassInformation(typeof(SendGuestStarInvite), TwitchResourceEnum.Guest_Star)]
		SendGuestStarInvite = 79,
		[TwitchAPIClassInformation(typeof(UpdateChannelGuestStarSettings), TwitchResourceEnum.Guest_Star)]
		UpdateChannelGuestStarSettings = 80,
		[TwitchAPIClassInformation(typeof(UpdateGuestStarSlot), TwitchResourceEnum.Guest_Star)]
		UpdateGuestStarSlot = 81,
		[TwitchAPIClassInformation(typeof(UpdateGuestStarSlotSettings), TwitchResourceEnum.Guest_Star)]
		UpdateGuestStarSlotSettings = 82,
		// Hype_Train
		[TwitchAPIClassInformation(typeof(GetHypeTrainEvents), TwitchResourceEnum.Hype_Train)]
		GetHypeTrainEvents = 83,
		// Moderation
		[TwitchAPIClassInformation(typeof(AddBlockedTerm), TwitchResourceEnum.Moderation)]
		AddBlockedTerm = 84,
		[TwitchAPIClassInformation(typeof(AddChannelModerator), TwitchResourceEnum.Moderation)]
		AddChannelModerator = 85,
		[TwitchAPIClassInformation(typeof(AddChannelVIP), TwitchResourceEnum.Moderation)]
		AddChannelVIP = 86,
		[TwitchAPIClassInformation(typeof(BanUser), TwitchResourceEnum.Moderation)]
		BanUser = 87,
		[TwitchAPIClassInformation(typeof(CheckAutoModStatus), TwitchResourceEnum.Moderation)]
		CheckAutoModStatus = 88,
		[TwitchAPIClassInformation(typeof(DeleteChatMessages), TwitchResourceEnum.Moderation)]
		DeleteChatMessages = 89,
		[TwitchAPIClassInformation(typeof(GetAutoModSettings), TwitchResourceEnum.Moderation)]
		GetAutoModSettings = 90,
		[TwitchAPIClassInformation(typeof(GetBannedUsers), TwitchResourceEnum.Moderation)]
		GetBannedUsers = 91,
		[TwitchAPIClassInformation(typeof(GetBlockedTerms), TwitchResourceEnum.Moderation)]
		GetBlockedTerms = 92,
		[TwitchAPIClassInformation(typeof(GetModeratedChannels), TwitchResourceEnum.Moderation)]
		GetModeratedChannels = 93,
		[TwitchAPIClassInformation(typeof(GetModerators), TwitchResourceEnum.Moderation)]
		GetModerators = 94,
		[TwitchAPIClassInformation(typeof(GetShieldModeStatus), TwitchResourceEnum.Moderation)]
		GetShieldModeStatus = 95,
		[TwitchAPIClassInformation(typeof(GetUnbanRequests), TwitchResourceEnum.Moderation)]
		GetUnbanRequests = 96,
		[TwitchAPIClassInformation(typeof(GetVIPs), TwitchResourceEnum.Moderation)]
		GetVIPs = 97,
		[TwitchAPIClassInformation(typeof(ManageHeldAutoMessages), TwitchResourceEnum.Moderation)]
		ManageHeldAutoMessages = 98,
		[TwitchAPIClassInformation(typeof(RemoveBlockedTerm), TwitchResourceEnum.Moderation)]
		RemoveBlockedTerm = 99,
		[TwitchAPIClassInformation(typeof(RemoveChannelModerator), TwitchResourceEnum.Moderation)]
		RemoveChannelModerator = 100,
		[TwitchAPIClassInformation(typeof(RemoveChannelVIP), TwitchResourceEnum.Moderation)]
		RemoveChannelVIP = 101,
		[TwitchAPIClassInformation(typeof(ResolveUnbanRequests), TwitchResourceEnum.Moderation)]
		ResolveUnbanRequests = 102,
		[TwitchAPIClassInformation(typeof(UnbanUser), TwitchResourceEnum.Moderation)]
		UnbanUser = 103,
		[TwitchAPIClassInformation(typeof(UpdateAutoModSettings), TwitchResourceEnum.Moderation)]
		UpdateAutoModSettings = 104,
		[TwitchAPIClassInformation(typeof(UpdateShieldModeStatus), TwitchResourceEnum.Moderation)]
		UpdateShieldModeStatus = 105,
		[TwitchAPIClassInformation(typeof(WarnChatUser), TwitchResourceEnum.Moderation)]
		WarnChatUser = 106,
		// Polls
		[TwitchAPIClassInformation(typeof(CreatePoll), TwitchResourceEnum.Polls)]
		CreatePoll = 107,
		[TwitchAPIClassInformation(typeof(EndPoll), TwitchResourceEnum.Polls)]
		EndPoll = 108,
		[TwitchAPIClassInformation(typeof(GetPolls), TwitchResourceEnum.Polls)]
		GetPolls = 109,
		// Predictions
		[TwitchAPIClassInformation(typeof(CreatePrediction), TwitchResourceEnum.Predictions)]
		CreatePrediction = 110,
		[TwitchAPIClassInformation(typeof(EndPrediction), TwitchResourceEnum.Predictions)]
		EndPrediction = 111,
		[TwitchAPIClassInformation(typeof(GetPredictions), TwitchResourceEnum.Predictions)]
		GetPredictions = 112,
		// Raids
		[TwitchAPIClassInformation(typeof(CancelARaid), TwitchResourceEnum.Raids)]
		CancelARaid = 113,
		[TwitchAPIClassInformation(typeof(StartARaid), TwitchResourceEnum.Raids)]
		StartARaid = 114,
		// Schedule
		[TwitchAPIClassInformation(typeof(CreateChannelStreamScheduleSegment), TwitchResourceEnum.Schedule)]
		CreateChannelStreamScheduleSegment = 115,
		[TwitchAPIClassInformation(typeof(DeleteChannelStreamScheduleSegment), TwitchResourceEnum.Schedule)]
		DeleteChannelStreamScheduleSegment = 116,
		[TwitchAPIClassInformation(typeof(GetChanneliCalendar), TwitchResourceEnum.Schedule)]
		GetChanneliCalendar = 117,
		[TwitchAPIClassInformation(typeof(GetChannelStreamSchedule), TwitchResourceEnum.Schedule)]
		GetChannelStreamSchedule = 118,
		[TwitchAPIClassInformation(typeof(UpdateChannelStreamSchedule), TwitchResourceEnum.Schedule)]
		UpdateChannelStreamSchedule = 119,
		[TwitchAPIClassInformation(typeof(UpdateChannelStreamScheduleSegment), TwitchResourceEnum.Schedule)]
		UpdateChannelStreamScheduleSegment = 120,
		// Search
		[TwitchAPIClassInformation(typeof(SearchCategories), TwitchResourceEnum.Search)]
		SearchCategories = 121,
		[TwitchAPIClassInformation(typeof(SearchChannels), TwitchResourceEnum.Search)]
		SearchChannels = 122,
		// Streams
		[TwitchAPIClassInformation(typeof(CreateStreamMarker), TwitchResourceEnum.Streams)]
		CreateStreamMarker = 123,
		[TwitchAPIClassInformation(typeof(GetFollowedStreams), TwitchResourceEnum.Streams)]
		GetFollowedStreams = 124,
		[TwitchAPIClassInformation(typeof(GetStreamKey), TwitchResourceEnum.Streams)]
		GetStreamKey = 125,
		[TwitchAPIClassInformation(typeof(GetStreamMarkers), TwitchResourceEnum.Streams)]
		GetStreamMarkers = 126,
		[TwitchAPIClassInformation(typeof(GetStreams), TwitchResourceEnum.Streams)]
		GetStreams = 127,
		// Subscriptions
		[TwitchAPIClassInformation(typeof(CheckUserSubscription), TwitchResourceEnum.Subscriptions)]
		CheckUserSubscription = 128,
		[TwitchAPIClassInformation(typeof(GetBroadcasterSubscriptions), TwitchResourceEnum.Subscriptions)]
		GetBroadcasterSubscriptions = 129,
		// Teams
		[TwitchAPIClassInformation(typeof(GetChannelTeams), TwitchResourceEnum.Teams)]
		GetChannelTeams = 130,
		[TwitchAPIClassInformation(typeof(GetTeams), TwitchResourceEnum.Teams)]
		GetTeams = 131,
		// Users
		[TwitchAPIClassInformation(typeof(BlockUser), TwitchResourceEnum.Users)]
		BlockUser = 132,
		[TwitchAPIClassInformation(typeof(GetUserActiveExtensions), TwitchResourceEnum.Users)]
		GetUserActiveExtensions = 133,
		[TwitchAPIClassInformation(typeof(GetUserBlockList), TwitchResourceEnum.Users)]
		GetUserBlockList = 134,
		[TwitchAPIClassInformation(typeof(GetUserExtensions), TwitchResourceEnum.Users)]
		GetUserExtensions = 135,
		[TwitchAPIClassInformation(typeof(GetUsers), TwitchResourceEnum.Users)]
		GetUsers = 136,
		[TwitchAPIClassInformation(typeof(UnblockUser), TwitchResourceEnum.Users)]
		UnblockUser = 137,
		[TwitchAPIClassInformation(typeof(UpdateUser), TwitchResourceEnum.Users)]
		UpdateUser = 138,
		[TwitchAPIClassInformation(typeof(UpdateUserExtensions), TwitchResourceEnum.Users)]
		UpdateUserExtensions = 139,
		// Videos
		[TwitchAPIClassInformation(typeof(DeleteVideos), TwitchResourceEnum.Videos)]
		DeleteVideos = 140,
		[TwitchAPIClassInformation(typeof(GetVideos), TwitchResourceEnum.Videos)]
		GetVideos = 141,
		// Whispers
		[TwitchAPIClassInformation(typeof(SendWhisper), TwitchResourceEnum.Whispers)]
		SendWhisper = 142,
	}
}