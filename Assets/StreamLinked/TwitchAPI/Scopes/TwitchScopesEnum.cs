using System;

using ScoredProductions.StreamLinked.EventSub;

namespace ScoredProductions.StreamLinked.API.Scopes {

	/* 
		C# Interactive Get next value to add enum

		You can do a right click on the project in the solution explorer and than click `Initialize interactive with project".
		var NextValue = Enum.GetValues(typeof(ScoredProductions.StreamLinked.TwitchScopesEnum)).Cast<ScoredProductions.StreamLinked.EventSub.TwitchScopesEnum>().Max();
		Console.Write(((int)NextValue) + 1);
	 */

	// Everythings out of numerical order because Twitch recreated the page listing them

	/// <summary>
	/// All scopes used by Twitch API Auth tokens.
	/// In Enum format.
	/// <c>"NOTE An application must request only the scopes required by the APIs that their app calls. If you request more scopes than is required to support your app’s functionality, Twitch may suspend your application’s access to the Twitch API."</c>
	/// - <see href="https://dev.twitch.tv/docs/authentication/scopes/">Twitch Webpage</see>
	/// </summary>
	public enum TwitchScopesEnum {
		[TwitchAPIScope(TwitchAPIClassEnum.GetExtensionAnalytics)]
		analytics_read_extensions = 0,
		[TwitchAPIScope(TwitchAPIClassEnum.GetGameAnalytics)]
		analytics_read_games = 1,
		[TwitchAPIScope(TwitchAPIClassEnum.GetBitsLeaderboard)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Bits_Use,
			TwitchEventSubSubscriptionsEnum.Channel_Cheer)]
		bits_read = 2,
		[TwitchAPIScope(TwitchAPIClassEnum.SendChatMessage)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear_User_Messages,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message_Delete,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Notification,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Settings_Update)]
		channel_bot = 58,
		[TwitchAPIScope(TwitchAPIClassEnum.SnoozeNextAd)]
		channel_manage_ads = 75,
		[TwitchAPIScope(TwitchAPIClassEnum.GetAdSchedule)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Ad_Break_Begin)]
		channel_read_ads = 76,
		[TwitchAPIScope(TwitchAPIClassEnum.ModifyChannelInformation,
			TwitchAPIClassEnum.CreateStreamMarker)]
		channel_manage_broadcast = 3,
		[TwitchAPIScope(TwitchAPIClassEnum.GetCharityCampaign,
			TwitchAPIClassEnum.GetCharityCampaignDonations)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Charity_Donation,
			TwitchEventSubSubscriptionsEnum.Channel_Charity_Campaign_Start,
			TwitchEventSubSubscriptionsEnum.Channel_Charity_Campaign_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Charity_Campaign_Stop)]
		channel_read_charity = 4,
		[TwitchAPIScope(TwitchAPIClassEnum.StartCommercial)]
		channel_edit_commercial = 5,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChannelEditors)]
		channel_read_editors = 13,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUserActiveExtensions,
			TwitchAPIClassEnum.UpdateUserExtensions)]
		channel_manage_extensions = 6,
		[TwitchAPIScope(TwitchAPIClassEnum.GetCreatorGoals)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Goal_Begin, 
			TwitchEventSubSubscriptionsEnum.Channel_Goal_Progress, 
			TwitchEventSubSubscriptionsEnum.Channel_Goal_End)]
		channel_read_goals = 14,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChannelGuestStarSettings, 
			TwitchAPIClassEnum.GetGuestStarSession, 
			TwitchAPIClassEnum.GetGuestStarInvites)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_End,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Guest_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Settings_Update)]
		channel_read_guest_star = 15,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateChannelGuestStarSettings,
			TwitchAPIClassEnum.CreateGuestStarSession,
			TwitchAPIClassEnum.EndGuestStarSession,
			TwitchAPIClassEnum.SendGuestStarInvite,
			TwitchAPIClassEnum.DeleteGuestStarInvite,
			TwitchAPIClassEnum.AssignGuestStarSlot,
			TwitchAPIClassEnum.UpdateGuestStarSlot,
			TwitchAPIClassEnum.DeleteGuestStarSlot,
			TwitchAPIClassEnum.UpdateGuestStarSlotSettings)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_End,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Guest_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Settings_Update)]
		channel_manage_guest_star = 16,
		[TwitchAPIScope(TwitchAPIClassEnum.GetHypeTrainEvents)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Hype_Train_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Hype_Train_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Hype_Train_End)]
		channel_read_hype_train = 17,
		[TwitchAPIScope(TwitchAPIClassEnum.AddChannelModerator,
			TwitchAPIClassEnum.RemoveChannelModerator, 
			TwitchAPIClassEnum.GetModerators)]
		channel_manage_moderators = 65,
		[TwitchAPIScope(TwitchAPIClassEnum.GetPolls)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Poll_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Poll_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Poll_End)]
		channel_read_polls = 18,
		[TwitchAPIScope(TwitchAPIClassEnum.GetPolls,
			TwitchAPIClassEnum.CreatePoll,
			TwitchAPIClassEnum.EndPoll)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Poll_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Poll_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Poll_End)]
		channel_manage_polls = 7,
		[TwitchAPIScope(TwitchAPIClassEnum.GetPredictions)] // Get Channel Points Predictions ??? why such a different name on the website
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Prediction_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_Lock,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_End)]
		channel_read_predictions = 19,
		[TwitchAPIScope(TwitchAPIClassEnum.GetPredictions,
			TwitchAPIClassEnum.CreatePrediction,
			TwitchAPIClassEnum.EndPrediction)]  // such different names why...
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Prediction_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_Progress,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_Lock,
			TwitchEventSubSubscriptionsEnum.Channel_Prediction_End)]
		channel_manage_predictions = 8,
		[TwitchAPIScope(TwitchAPIClassEnum.StartARaid,
			TwitchAPIClassEnum.CancelARaid)]
		channel_manage_raids = 9,
		[TwitchAPIScope(TwitchAPIClassEnum.GetCustomReward,
			TwitchAPIClassEnum.GetCustomRewardRedemption)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Points_Automatic_Reward_Redemption,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Automatic_Reward_Redemption_V2,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Add,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Remove,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Redemption_Add,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Redemption_Update)]
		channel_read_redemptions = 20,
		[TwitchAPIScope(TwitchAPIClassEnum.GetCustomReward,
			TwitchAPIClassEnum.GetCustomRewardRedemption,
			TwitchAPIClassEnum.CreateCustomRewards,
			TwitchAPIClassEnum.DeleteCustomReward,
			TwitchAPIClassEnum.UpdateCustomReward,
			TwitchAPIClassEnum.UpdateRedemptionStatus)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Points_Automatic_Reward_Redemption,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Automatic_Reward_Redemption_V2, // not included but I bet its a typo...
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Add,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Remove,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Redemption_Add,
			TwitchEventSubSubscriptionsEnum.Channel_Points_Custom_Reward_Redemption_Update)]
		channel_manage_redemptions = 10,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateChannelStreamSchedule,
			TwitchAPIClassEnum.CreateChannelStreamScheduleSegment,
			TwitchAPIClassEnum.UpdateChannelStreamScheduleSegment,
			TwitchAPIClassEnum.DeleteChannelStreamScheduleSegment)]
		channel_manage_schedule = 11,
		[TwitchAPIScope(TwitchAPIClassEnum.GetStreamKey)]
		channel_read_stream_key = 21,
		[TwitchAPIScope(TwitchAPIClassEnum.GetBroadcasterSubscriptions)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Subscribe,
			TwitchEventSubSubscriptionsEnum.Channel_Subscription_End,
			TwitchEventSubSubscriptionsEnum.Channel_Subscription_Gift,
			TwitchEventSubSubscriptionsEnum.Channel_Subscription_Message)]
		channel_read_subscriptions = 22,
		[TwitchAPIScope(TwitchAPIClassEnum.DeleteVideos)]
		channel_manage_videos = 12,
		[TwitchAPIScope(TwitchAPIClassEnum.GetVIPs)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_VIP_Add,
			TwitchEventSubSubscriptionsEnum.Channel_VIP_Remove)]
		channel_read_vips = 23,
		[TwitchAPIScope(TwitchAPIClassEnum.GetVIPs,
			TwitchAPIClassEnum.AddChannelVIP,
			TwitchAPIClassEnum.RemoveChannelVIP)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_VIP_Add,
			TwitchEventSubSubscriptionsEnum.Channel_VIP_Remove)]
		channel_manage_vips = 24,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Ban,
			TwitchEventSubSubscriptionsEnum.Channel_Unban)]
		channel_moderate = 59,
		[TwitchAPIScope(TwitchAPIClassEnum.CreateClip)]
		clips_edit = 25,
		[TwitchAPIScope(TwitchAPIClassEnum.CheckAutoModStatus,
			TwitchAPIClassEnum.GetBannedUsers,
			TwitchAPIClassEnum.GetModerators)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderator_Add,
			TwitchEventSubSubscriptionsEnum.Channel_Moderator_Remove)]
		moderation_read = 26,
		[TwitchAPIScope(TwitchAPIClassEnum.SendChatAnnouncement)]
		moderator_manage_announcements = 27,
		[TwitchAPIScope(TwitchAPIClassEnum.ManageHeldAutoMessages)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Automod_Message_Hold,
			TwitchEventSubSubscriptionsEnum.Automod_Message_Hold_V2,
			TwitchEventSubSubscriptionsEnum.Automod_Message_Update,
			TwitchEventSubSubscriptionsEnum.Automod_Message_Update_V2,
			TwitchEventSubSubscriptionsEnum.Automod_Terms_Update)]
		moderator_manage_automod = 28,
		[TwitchAPIScope(TwitchAPIClassEnum.GetAutoModSettings)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Automod_Settings_Update)]
		moderator_read_automod_settings = 29,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateAutoModSettings)]
		moderator_manage_automod_settings = 30,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)]
		moderator_read_banned_users = 77,
		[TwitchAPIScope(TwitchAPIClassEnum.GetBannedUsers,
			TwitchAPIClassEnum.BanUser,
			TwitchAPIClassEnum.UnbanUser)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)]
		moderator_manage_banned_users = 31,
		[TwitchAPIScope(TwitchAPIClassEnum.GetBlockedTerms)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_read_blocked_terms = 32,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_read_chat_messages = 66,
		[TwitchAPIScope(TwitchAPIClassEnum.GetBlockedTerms,
			TwitchAPIClassEnum.AddBlockedTerm,
			TwitchAPIClassEnum.RemoveBlockedTerm)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_manage_blocked_terms = 33,
		[TwitchAPIScope(TwitchAPIClassEnum.DeleteChatMessages)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_manage_chat_messages = 34,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChatSettings)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_read_chat_settings = 35,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateChatSettings)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_manage_chat_settings = 36,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChatters)]
		moderator_read_chatters = 37,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChannelFollowers)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Follow)]
		moderator_read_followers = 38,
		[TwitchAPIScope(TwitchAPIClassEnum.GetChannelGuestStarSettings,
			TwitchAPIClassEnum.GetGuestStarSession,
			TwitchAPIClassEnum.GetGuestStarInvites)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_End,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Guest_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Settings_Update)]
		moderator_read_guest_star = 39,
		[TwitchAPIScope(TwitchAPIClassEnum.SendGuestStarInvite,
			TwitchAPIClassEnum.DeleteGuestStarInvite,
			TwitchAPIClassEnum.AssignGuestStarSlot,
			TwitchAPIClassEnum.UpdateGuestStarSlot,
			TwitchAPIClassEnum.DeleteGuestStarSlot,
			TwitchAPIClassEnum.UpdateGuestStarSlotSettings)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Session_End,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Guest_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Guest_Star_Settings_Update)]
		moderator_manage_guest_star = 40,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)]
		moderator_read_moderators = 68,
		[TwitchAPIScope(TwitchAPIClassEnum.GetShieldModeStatus)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Shield_Mode_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Shield_Mode_End)]
		moderator_read_shield_mode = 41,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateShieldModeStatus)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Shield_Mode_Begin,
			TwitchEventSubSubscriptionsEnum.Channel_Shield_Mode_End)]
		moderator_manage_shield_mode = 42,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Create,
			TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Received)]
		moderator_read_shoutouts = 43,
		[TwitchAPIScope(TwitchAPIClassEnum.SendAShoutout)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Create,
			TwitchEventSubSubscriptionsEnum.Channel_Shoutout_Received)]
		moderator_manage_shoutouts = 44,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Suspicious_User_Message,
			TwitchEventSubSubscriptionsEnum.Channel_Suspicious_User_Update)]
		moderator_read_suspicious_users = 67,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUnbanRequests)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Unban_Request_Create,
			TwitchEventSubSubscriptionsEnum.Channel_Unban_Request_Resolve,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_read_unban_requests = 69,
		[TwitchAPIScope(TwitchAPIClassEnum.ResolveUnbanRequests)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Unban_Request_Create,
			TwitchEventSubSubscriptionsEnum.Channel_Unban_Request_Resolve,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_manage_unban_requests = 70,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate,
			TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2)] // didnt include v2 on site but I bet it will be...
		moderator_read_vips = 71,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2,
			TwitchEventSubSubscriptionsEnum.Channel_Warning_Acknowledge,
			TwitchEventSubSubscriptionsEnum.Channel_Warning_Send)]
		moderator_read_warnings = 46,
		[TwitchAPIScope(TwitchAPIClassEnum.WarnChatUser)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2,
			TwitchEventSubSubscriptionsEnum.Channel_Warning_Acknowledge,
			TwitchEventSubSubscriptionsEnum.Channel_Warning_Send)]
		moderator_manage_warnings = 45,
		[TwitchAPIScope(TwitchAPIClassEnum.SendChatMessage)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear_User_Messages,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message_Delete,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Notification,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Settings_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_User_Message_Hold,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_User_Message_Update)]
		user_bot = 62,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateUser)]
		user_edit = 47,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUserExtensions,
			TwitchAPIClassEnum.GetUserActiveExtensions,
			TwitchAPIClassEnum.UpdateUserExtensions)]
		user_edit_broadcast = 72,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUserBlockList)]
		user_read_blocked_users = 49,
		[TwitchAPIScope(TwitchAPIClassEnum.BlockUser,
			TwitchAPIClassEnum.UnblockUser)]
		user_manage_blocked_users = 48,
		[TwitchAPIScope(TwitchAPIClassEnum.GetStreamMarkers,
			TwitchAPIClassEnum.GetUserExtensions,
			TwitchAPIClassEnum.GetUserActiveExtensions)]
		user_read_broadcast = 50,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Clear_User_Messages,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Message_Delete,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Notification,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_Settings_Update,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_User_Message_Hold,
			TwitchEventSubSubscriptionsEnum.Channel_Chat_User_Message_Update)]
		user_read_chat = 63,
		[TwitchAPIScope(TwitchAPIClassEnum.UpdateUserChatColor)]
		user_manage_chat_color = 51,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUsers, // It says optional as a specific field needs it
			TwitchAPIClassEnum.UpdateUser)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.User_Update)]
		user_read_email = 52,
		[TwitchAPIScope(TwitchAPIClassEnum.GetUserEmotes)]
		user_read_emotes = 73,
		[TwitchAPIScope(TwitchAPIClassEnum.GetFollowedChannels,
			TwitchAPIClassEnum.GetFollowedStreams)]
		user_read_follows = 53,
		[TwitchAPIScope(TwitchAPIClassEnum.GetModeratedChannels)]
		user_read_moderated_channels = 54,
		[TwitchAPIScope(TwitchAPIClassEnum.CheckUserSubscription)]
		user_read_subscriptions = 55,
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Whisper_Received)]
		user_read_whispers = 74,
		[TwitchAPIScope(TwitchAPIClassEnum.SendWhisper)]
		[TwitchEventSubScope(TwitchEventSubSubscriptionsEnum.Whisper_Received)]
		user_manage_whispers = 56,
		[TwitchAPIScope(TwitchAPIClassEnum.SendChatMessage)]
		user_write_chat = 57,
		[TwitchIRCScope]
		chat_edit = 60,
		[TwitchIRCScope]
		chat_read = 61,
		[Obsolete("Used by pubsub only, pubsub is not implemented in StreamLinked")]
		whispers_read = 64,
	}
}