using System;

using ScoredProductions.StreamLinked.EventSub.Events;
using ScoredProductions.StreamLinked.EventSub.ExtensionAttributes;

namespace ScoredProductions.StreamLinked.EventSub {

	/* 
		C# Interactive Get next value to add enum

		You can do a right click on the project in the solution explorer and than click `Initialize interactive with project".
		var NextValue = Enum.GetValues(typeof(ScoredProductions.StreamLinked.EventSub.TwitchEventSubSubscriptionsEnum)).Cast<ScoredProductions.StreamLinked.EventSub.TwitchEventSubSubscriptionsEnum>().Max();
		Console.Write(((int)NextValue) + 1);
	 */

	/// <summary>
	/// All EventSub Subscription Types, each attributed with its true string value and version, both accessible with extension.
	/// </summary>

	[Serializable]
	public enum TwitchEventSubSubscriptionsEnum {
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Message_Hold, "1", typeof(AutoMessageHold))]
		Automod_Message_Hold = 0,
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Message_Hold, "2", typeof(AutoMessageHoldV2))]
		Automod_Message_Hold_V2 = 78,
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Message_Update, "1", typeof(AutoMessageUpdate))]
		Automod_Message_Update = 1,
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Message_Update, "2", typeof(AutoMessageUpdateV2))]
		Automod_Message_Update_V2 = 79,
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Settings_Update, "1", typeof(AutomodSettingsUpdate))]
		Automod_Settings_Update = 2,
		[EventSubInformation(TwitchEventSubSubscriptions.Automod_Terms_Update, "1", typeof(AutomodTermsUpdate))]
		Automod_Terms_Update = 3,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Bits_Use, "1", typeof(ChannelBitsUse))]
		Channel_Bits_Use = 80,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Update, "2", typeof(ChannelUpdate))]
		Channel_Update = 4,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Follow, "2", typeof(ChannelFollow))]
		Channel_Follow = 5,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Ad_Break_Begin, "1", typeof(ChannelAdBreakBegin))]
		Channel_Ad_Break_Begin = 6,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Clear, "1", typeof(ChannelChatClear))]
		Channel_Chat_Clear = 7,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Clear_User_Messages, "1", typeof(ChannelChatClearUserMessages))]
		Channel_Chat_Clear_User_Messages = 8,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Message,"1", typeof(ChannelChatMessage))]
		Channel_Chat_Message = 9,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Message_Delete, "1", typeof(ChannelChatMessageDelete))]
		Channel_Chat_Message_Delete = 10,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Notification, "1", typeof(ChannelChatNotification))]
		Channel_Chat_Notification = 11,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_Settings_Update, "1", typeof(ChannelChatSettingsUpdate))]
		Channel_Chat_Settings_Update = 12,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_User_Message_Hold, "1", typeof(ChannelChatUserMessageHold))]
		Channel_Chat_User_Message_Hold = 13,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Chat_User_Message_Update, "1", typeof(ChannelChatUserMessageUpdate))]
		Channel_Chat_User_Message_Update = 14,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shared_Chat_Session_Begin, "1", typeof(ChannelSharedChatSessionBegin))]
		Channel_Shared_Chat_Session_Begin = 75,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shared_Chat_Session_Update, "1", typeof(ChannelSharedChatSessionUpdate))]
		Channel_Shared_Chat_Session_Update = 76,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shared_Chat_Session_End, "1", typeof(ChannelSharedChatSessionEnd))]
		Channel_Shared_Chat_Session_End = 77,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Subscribe, "1", typeof(ChannelSubscribe))]
		Channel_Subscribe = 15,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Subscription_End, "1", typeof(ChannelSubscriptionEnd))]
		Channel_Subscription_End = 16,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Subscription_Gift, "1", typeof(ChannelSubscriptionGift))]
		Channel_Subscription_Gift = 17,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Subscription_Message, "1", typeof(ChannelSubscriptionMessage))]
		Channel_Subscription_Message = 18,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Cheer, "1", typeof(ChannelCheer))]
		Channel_Cheer = 19,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Raid, "1", typeof(ChannelRaid))]
		Channel_Raid = 20,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Ban, "1", typeof(ChannelBan))]
		Channel_Ban = 21,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Unban, "1", typeof(ChannelUnban))]
		Channel_Unban = 22,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Unban_Request_Create, "1", typeof(ChannelUnbanRequestCreate))]
		Channel_Unban_Request_Create = 23,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Unban_Request_Resolve, "1", typeof(ChannelUnbanRequestResolve))]
		Channel_Unban_Request_Resolve = 24,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Moderate, "1", typeof(ChannelModerate))]
		Channel_Moderate = 25,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Moderate, "2", typeof(ChannelModerateV2))]
		Channel_Moderate_V2 = 26,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Moderator_Add, "1", typeof(ChannelModeratorAdd))]
		Channel_Moderator_Add = 27,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Moderator_Remove, "1", typeof(ChannelModeratorRemove))]
		Channel_Moderator_Remove = 28,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Guest_Star_Session_Begin, "beta", typeof(ChannelGuestStarSessionBegin))]
		Channel_Guest_Star_Session_Begin = 29,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Guest_Star_Session_End, "beta", typeof(ChannelGuestStarSessionEnd))]
		Channel_Guest_Star_Session_End = 30,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Guest_Star_Guest_Update, "beta", typeof(ChannelGuestStarGuestUpdate))]
		Channel_Guest_Star_Guest_Update = 31,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Guest_Star_Settings_Update, "beta", typeof(ChannelGuestStarSettingsUpdate))]
		Channel_Guest_Star_Settings_Update = 32,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Channel_Points_Automatic_Reward_Redemption, "1", typeof(ChannelPointsAutomaticRewardRedemption))]
		Channel_Points_Automatic_Reward_Redemption = 33,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Channel_Points_Automatic_Reward_Redemption, "2", typeof(ChannelPointsAutomaticRewardRedemptionV2))]
		Channel_Points_Automatic_Reward_Redemption_V2 = 81,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Points_Custom_Reward_Add, "1", typeof(ChannelPointsCustomRewardAdd))]
		Channel_Points_Custom_Reward_Add = 34,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Points_Custom_Reward_Update, "1", typeof(ChannelPointsCustomRewardUpdate))]
		Channel_Points_Custom_Reward_Update = 35,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Points_Custom_Reward_Remove, "1", typeof(ChannelPointsCustomRewardRemove))]
		Channel_Points_Custom_Reward_Remove = 36,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Points_Custom_Reward_Redemption_Add, "1", typeof(ChannelPointsCustomRewardRedemptionAdd))]
		Channel_Points_Custom_Reward_Redemption_Add = 37,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Points_Custom_Reward_Redemption_Update, "1", typeof(ChannelPointsCustomRewardRedemptionUpdate))]
		Channel_Points_Custom_Reward_Redemption_Update = 38,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Poll_Begin, "1", typeof(ChannelPollBegin))]
		Channel_Poll_Begin = 39,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Poll_Progress, "1", typeof(ChannelPollProgress))]
		Channel_Poll_Progress = 40,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Poll_End, "1", typeof(ChannelPollEnd))]
		Channel_Poll_End = 41,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Prediction_Begin, "1", typeof(ChannelPredictionBegin))]
		Channel_Prediction_Begin = 42,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Prediction_Progress, "1", typeof(ChannelPredictionProgress))]
		Channel_Prediction_Progress = 43,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Prediction_Lock, "1", typeof(ChannelPredictionLock))]
		Channel_Prediction_Lock = 44,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Prediction_End, "1", typeof(ChannelPredictionEnd))]
		Channel_Prediction_End = 45,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Suspicious_User_Message, "1", typeof(ChannelSuspiciousUserMessage))]
		Channel_Suspicious_User_Message = 46,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Suspicious_User_Update, "1", typeof(ChannelSuspiciousUserUpdate))]
		Channel_Suspicious_User_Update = 47,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Vip_Add, "1", typeof(ChannelVIPAdd))]
		Channel_VIP_Add = 48,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Vip_Remove, "1", typeof(ChannelVIPRemove))]
		Channel_VIP_Remove = 49,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Warning_Acknowledge, "1", typeof(ChannelWarningAcknowledge))]
		Channel_Warning_Acknowledge = 50,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Warning_Send, "1", typeof(ChannelWarningSend))]
		Channel_Warning_Send = 51,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Charity_Donation, "1", typeof(ChannelCharityDonation))]
		Channel_Charity_Donation = 52,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Charity_Campaign_Start, "1", typeof(ChannelCharityCampaignStart))]
		Channel_Charity_Campaign_Start = 53,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Charity_Campaign_Progress, "1", typeof(ChannelCharityCampaignProgress))]
		Channel_Charity_Campaign_Progress = 54,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Charity_Campaign_Stop, "1", typeof(ChannelCharityCampaignStop))]
		Channel_Charity_Campaign_Stop = 55,
		[EventSubInformation(TwitchEventSubSubscriptions.Conduit_Shard_Disabled, "1", typeof(ConduitShardDisabled))]
		Conduit_Shard_Disabled = 56,
		[EventSubInformation(TwitchEventSubSubscriptions.Drop_Entitlement_Grant, "1", typeof(DropEntitlementGrant))]
		Drop_Entitlement_Grant = 57,
		[EventSubInformation(TwitchEventSubSubscriptions.Extension_Bits_Transaction_Create, "1", typeof(ExtensionBitsTransactionCreate))]
		Extension_Bits_Transaction_Create = 58,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Goal_Begin, "1", typeof(ChannelGoalsBegin))]
		Channel_Goal_Begin = 59,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Goal_Progress, "1", typeof(ChannelGoalsProgress))]
		Channel_Goal_Progress = 60,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Goal_End, "1", typeof(ChannelGoalsEnd))]
		Channel_Goal_End = 61,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Hype_Train_Begin, "1", typeof(ChannelHypeTrainBegin))]
		Channel_Hype_Train_Begin = 62,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Hype_Train_Progress, "1", typeof(ChannelHypeTrainProgress))]
		Channel_Hype_Train_Progress = 63,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Hype_Train_End, "1", typeof(ChannelHypeTrainEnd))]
		Channel_Hype_Train_End = 64,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shield_Mode_Begin, "1", typeof(ChannelShieldModeBegin))]
		Channel_Shield_Mode_Begin = 65,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shield_Mode_End, "1", typeof(ChannelShieldModeEnd))]
		Channel_Shield_Mode_End = 66,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shoutout_Create, "1", typeof(ChannelShoutoutCreate))]
		Channel_Shoutout_Create = 67,
		[EventSubInformation(TwitchEventSubSubscriptions.Channel_Shoutout_Received, "1", typeof(ChannelShoutoutReceived))]
		Channel_Shoutout_Received = 68,
		[EventSubInformation(TwitchEventSubSubscriptions.Stream_Online, "1", typeof(StreamOnline))]
		Stream_Online = 69,
		[EventSubInformation(TwitchEventSubSubscriptions.Stream_Offline, "1", typeof(StreamOffline))]
		Stream_Offline = 70,
		[EventSubInformation(TwitchEventSubSubscriptions.User_Authorization_Grant, "1", typeof(UserAuthorizationGrant))]
		User_Authorization_Grant = 71,
		[EventSubInformation(TwitchEventSubSubscriptions.User_Authorization_Revoke, "1", typeof(UserAuthorizationRevoke))]
		User_Authorization_Revoke = 72,
		[EventSubInformation(TwitchEventSubSubscriptions.User_Update, "1", typeof(UserUpdate))]
		User_Update = 73,
		[EventSubInformation(TwitchEventSubSubscriptions.User_Whisper_Message, "1", typeof(WhisperReceived))]
		Whisper_Received = 74
	}
}
