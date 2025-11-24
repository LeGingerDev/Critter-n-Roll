namespace ScoredProductions.StreamLinked.EventSub {

	public static class TwitchEventSubSubscriptions {

		/// <summary>
		/// A user is notified if a message is caught by automod for review.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessagehold">Link</see>
		/// </summary>
		public const string Automod_Message_Hold = "automod.message.hold";
		/// <summary>
		/// A message in the automod queue had its status changed.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessageupdate">Link</see>
		/// </summary>
		public const string Automod_Message_Update = "automod.message.update";
		/// <summary>
		/// A notification is sent when a broadcaster’s automod settings are updated.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodsettingsupdate">Link</see>
		/// </summary>
		public const string Automod_Settings_Update = "automod.settings.update";
		/// <summary>
		/// A notification is sent when a broadcaster’s automod terms are updated. Changes to private terms are not sent.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodtermsupdate">Link</see>
		/// </summary>
		public const string Automod_Terms_Update = "automod.terms.update";
		/// <summary>
		/// A notification is sent whenever Bits are used on a channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">Link</see>
		/// </summary>
		public const string Channel_Bits_Use = "channel.bits.use";
		/// <summary>
		/// A broadcaster updates their channel properties e.g., category, title, content classification labels, broadcast, or language.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelupdate">Link</see>
		/// </summary>
		public const string Channel_Update = "channel.update";
		/// <summary>
		/// A specified channel receives a follow.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Link</see>
		/// </summary>
		public const string Channel_Follow = "channel.follow";
		/// <summary>
		/// A midroll commercial break has started running.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Link</see>
		/// </summary>
		public const string Channel_Ad_Break_Begin = "channel.ad_break.begin";
		/// <summary>
		/// A midroll commercial break has started running.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear">Link</see>
		/// </summary>
		public const string Channel_Chat_Clear = "channel.chat.clear";
		/// <summary>
		/// A moderator or bot has cleared all messages from a specific user.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear_user_messages">Link</see>
		/// </summary>
		public const string Channel_Chat_Clear_User_Messages = "channel.chat.clear_user_messages";
		/// <summary>
		/// Any user sends a message to a specific chat room.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Link</see>
		/// </summary>
		public const string Channel_Chat_Message = "channel.chat.message";
		/// <summary>
		/// A moderator has removed a specific message.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Link</see>
		/// </summary>
		public const string Channel_Chat_Message_Delete = "channel.chat.message_delete";
		/// <summary>
		/// A notification for when an event that appears in chat has occurred.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Link</see>
		/// </summary>
		public const string Channel_Chat_Notification = "channel.chat.notification";
		/// <summary>
		/// This event sends a notification when a broadcaster’s chat settings are updated.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Link</see>
		/// </summary>
		public const string Channel_Chat_Settings_Update = "channel.chat_settings.update";
		/// <summary>
		/// A user is notified if their message is caught by automod.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_hold">Link</see>
		/// </summary>
		public const string Channel_Chat_User_Message_Hold = "channel.chat.user_message_hold";
		/// <summary>
		/// A user is notified if their message’s automod status is updated.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_update">Link</see>
		/// </summary>
		public const string Channel_Chat_User_Message_Update = "channel.chat.user_message_update";
		/// <summary>
		/// A notification when a channel becomes active in an active shared chat session.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshared_chatbegin">Link</see>
		/// </summary>
		public const string Channel_Shared_Chat_Session_Begin = "channel.shared_chat.begin";
		/// <summary>
		/// A notification when the active shared chat session the channel is in changes.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshared_chatupdate">Link</see>
		/// </summary>
		public const string Channel_Shared_Chat_Session_Update = "channel.shared_chat.update";
		/// <summary>
		/// A notification when a channel leaves a shared chat session or the session ends.
		/// <see href = "https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshared_chatend">Link</see>
		/// </summary>
		public const string Channel_Shared_Chat_Session_End = "channel.shared_chat.end";
		/// <summary>
		/// A notification when a specified channel receives a subscriber. This does not include resubscribes.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscribe">Link</see>
		/// </summary>
		public const string Channel_Subscribe = "channel.subscribe";
		/// <summary>
		/// A notification when a subscription to the specified channel ends.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionend">Link</see>
		/// </summary>
		public const string Channel_Subscription_End = "channel.subscription.end";
		/// <summary>
		/// A notification when a viewer gives a gift subscription to one or more users in the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptiongift">Link</see>
		/// </summary>
		public const string Channel_Subscription_Gift = "channel.subscription.gift";
		/// <summary>
		/// A notification when a user sends a resubscription chat message in a specific channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionmessage">Link</see>
		/// </summary>
		public const string Channel_Subscription_Message = "channel.subscription.message";
		/// <summary>
		/// A user cheers on the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Link</see>
		/// </summary>
		public const string Channel_Cheer = "channel.cheer";
		/// <summary>
		/// A broadcaster raids another broadcaster’s channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelraid">Link</see>
		/// </summary>
		public const string Channel_Raid = "channel.raid";
		/// <summary>
		/// A viewer is banned from the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelban">Link</see>
		/// </summary>
		public const string Channel_Ban = "channel.ban";
		/// <summary>
		/// A viewer is unbanned from the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban">Link</see>
		/// </summary>
		public const string Channel_Unban = "channel.unban";
		/// <summary>
		/// A user creates an unban request.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestcreate">Link</see>
		/// </summary>
		public const string Channel_Unban_Request_Create = "channel.unban_request.create";
		/// <summary>
		/// An unban request has been resolved.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestupdate">Link</see>
		/// </summary>
		public const string Channel_Unban_Request_Resolve = "channel.unban_request.resolve";
		/// <summary>
		/// A moderator performs a moderation action in a channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Link</see>
		/// </summary>
		public const string Channel_Moderate = "channel.moderate";
		/// <summary>
		/// Moderator privileges were added to a user on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderatoradd">Link</see>
		/// </summary>
		public const string Channel_Moderator_Add = "channel.moderator.add";
		/// <summary>
		/// Moderator privileges were removed from a user on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderatorremove">Link</see>
		/// </summary>
		public const string Channel_Moderator_Remove = "channel.moderator.remove";
		/// <summary>
		/// The host began a new Guest Star session.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Link</see>
		/// </summary>
		public const string Channel_Guest_Star_Session_Begin = "channel.guest_star_session.begin";
		/// <summary>
		/// A running Guest Star session has ended.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Link</see>
		/// </summary>
		public const string Channel_Guest_Star_Session_End = "channel.guest_star_session.end";
		/// <summary>
		/// A guest or a slot is updated in an active Guest Star session.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Link</see>
		/// </summary>
		public const string Channel_Guest_Star_Guest_Update = "channel.guest_star_guest.update";
		/// <summary>
		/// The host preferences for Guest Star have been updated.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Link</see>
		/// </summary>
		public const string Channel_Guest_Star_Settings_Update = "channel.guest_star_settings.update";
		/// <summary>
		/// A viewer has redeemed an automatic channel points reward on the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_rewardadd">Link</see>
		/// </summary>
		public const string Channel_Channel_Points_Automatic_Reward_Redemption = "channel.channel_points_automatic_reward.add";
		/// <summary>
		/// A custom channel points reward has been created for the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">Link</see>
		/// </summary>
		public const string Channel_Points_Custom_Reward_Add = "channel.channel_points_custom_reward.add";
		/// <summary>
		/// A custom channel points reward has been updated for the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">Link</see>
		/// </summary>
		public const string Channel_Points_Custom_Reward_Update = "channel.channel_points_custom_reward.update";
		/// <summary>
		/// A custom channel points reward has been removed from the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">Link</see>
		/// </summary>
		public const string Channel_Points_Custom_Reward_Remove = "channel.channel_points_custom_reward.remove";
		/// <summary>
		/// A viewer has redeemed a custom channel points reward on the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">Link</see>
		/// </summary>
		public const string Channel_Points_Custom_Reward_Redemption_Add = "channel.channel_points_custom_reward_redemption.add";
		/// <summary>
		/// A redemption of a channel points custom reward has been updated for the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">Link</see>
		/// </summary>
		public const string Channel_Points_Custom_Reward_Redemption_Update = "channel.channel_points_custom_reward_redemption.update";
		/// <summary>
		/// A poll started on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Link</see>
		/// </summary>
		public const string Channel_Poll_Begin = "channel.poll.begin";
		/// <summary>
		/// Users respond to a poll on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Link</see>
		/// </summary>
		public const string Channel_Poll_Progress = "channel.poll.progress";
		/// <summary>
		/// A poll ended on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Link</see>
		/// </summary>
		public const string Channel_Poll_End = "channel.poll.end";
		/// <summary>
		/// A Prediction started on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionbegin">Link</see>
		/// </summary>
		public const string Channel_Prediction_Begin = "channel.prediction.begin";
		/// <summary>
		/// Users participated in a Prediction on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionprogress">Link</see>
		/// </summary>
		public const string Channel_Prediction_Progress = "channel.prediction.progress";
		/// <summary>
		/// A Prediction was locked on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionlock">Link</see>
		/// </summary>
		public const string Channel_Prediction_Lock = "channel.prediction.lock";
		/// <summary>
		/// A Prediction ended on a specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionend">Link</see>
		/// </summary>
		public const string Channel_Prediction_End = "channel.prediction.end";
		/// <summary>
		/// A chat message has been sent by a suspicious user.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_usermessage">Link</see>
		/// </summary>
		public const string Channel_Suspicious_User_Message = "channel.suspicious_user.message";
		/// <summary>
		/// A suspicious user has been updated.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_userupdate">Link</see>
		/// </summary>
		public const string Channel_Suspicious_User_Update = "channel.suspicious_user.update";
		/// <summary>
		/// A VIP is added to the channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipadd">Link</see>
		/// </summary>
		public const string Channel_Vip_Add = "channel.vip.add";
		/// <summary>
		/// A VIP is removed from the channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipremove">Link</see>
		/// </summary>
		public const string Channel_Vip_Remove = "channel.vip.remove";
		/// <summary>
		/// A user awknowledges a warning. Broadcasters and moderators can see the warning’s details.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningacknowledge">Link</see>
		/// </summary>
		public const string Channel_Warning_Acknowledge = "channel.warning.acknowledge";
		/// <summary>
		/// A user is sent a warning. Broadcasters and moderators can see the warning’s details.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningsend">Link</see>
		/// </summary>
		public const string Channel_Warning_Send = "channel.warning.send";
		/// <summary>
		/// Sends an event notification when a user donates to the broadcaster’s charity campaign.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaigndonate">Link</see>
		/// </summary>
		public const string Channel_Charity_Donation = "channel.charity_campaign.donate";
		/// <summary>
		/// Sends an event notification when the broadcaster starts a charity campaign.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstart">Link</see>
		/// </summary>
		public const string Channel_Charity_Campaign_Start = "channel.charity_campaign.start";
		/// <summary>
		/// Sends an event notification when progress is made towards the campaign’s goal or when the broadcaster changes the fundraising goal.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignprogress">Link</see>
		/// </summary>
		public const string Channel_Charity_Campaign_Progress = "channel.charity_campaign.progress";
		/// <summary>
		/// Sends an event notification when the broadcaster stops a charity campaign.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstop">Link</see>
		/// </summary>
		public const string Channel_Charity_Campaign_Stop = "channel.charity_campaign.stop";
		/// <summary>
		/// Sends a notification when EventSub disables a shard due to the status of the underlying transport changing.
		/// <see href="A user creates an unban request.">Link</see>
		/// </summary>
		public const string Conduit_Shard_Disabled = "conduit.shard.disabled";
		/// <summary>
		/// An entitlement for a Drop is granted to a user.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#dropentitlementgrant">Link</see>
		/// </summary>
		public const string Drop_Entitlement_Grant = "drop.entitlement.grant";
		/// <summary>
		/// A Bits transaction occurred for a specified Twitch Extension.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#extensionbits_transactioncreate">Link</see>
		/// </summary>
		public const string Extension_Bits_Transaction_Create = "extension.bits_transaction.create";
		/// <summary>
		/// Get notified when a broadcaster begins a goal.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalbegin">Link</see>
		/// </summary>
		public const string Channel_Goal_Begin = "channel.goal.begin";
		/// <summary>
		/// Get notified when progress (either positive or negative) is made towards a broadcaster’s goal.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalprogress">Link</see>
		/// </summary>
		public const string Channel_Goal_Progress = "channel.goal.progress";
		/// <summary>
		/// Get notified when a broadcaster ends a goal.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalend">Link</see>
		/// </summary>
		public const string Channel_Goal_End = "channel.goal.end";
		/// <summary>
		/// Get notified when a broadcaster ends a goal.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin">Link</see>
		/// </summary>
		public const string Channel_Hype_Train_Begin = "channel.hype_train.begin";
		/// <summary>
		/// A Hype Train makes progress on the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainprogress">Link</see>
		/// </summary>
		public const string Channel_Hype_Train_Progress = "channel.hype_train.progress";
		/// <summary>
		/// A Hype Train ends on the specified channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainend">Link</see>
		/// </summary>
		public const string Channel_Hype_Train_End = "channel.hype_train.end";
		/// <summary>
		/// Sends a notification when the broadcaster activates Shield Mode.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modebegin">Link</see>
		/// </summary>
		public const string Channel_Shield_Mode_Begin = "channel.shield_mode.begin";
		/// <summary>
		/// Sends a notification when the broadcaster deactivates Shield Mode.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modeend">Link</see>
		/// </summary>
		public const string Channel_Shield_Mode_End = "channel.shield_mode.end";
		/// <summary>
		/// Sends a notification when the specified broadcaster sends a Shoutout.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutcreate">Link</see>
		/// </summary>
		public const string Channel_Shoutout_Create = "channel.shoutout.create";
		/// <summary>
		/// Sends a notification when the specified broadcaster receives a Shoutout.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutreceive">Link</see>
		/// </summary>
		public const string Channel_Shoutout_Received = "channel.shoutout.receive";
		/// <summary>
		/// The specified broadcaster starts a stream.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamonline">Link</see>
		/// </summary>
		public const string Stream_Online = "stream.online";
		/// <summary>
		/// The specified broadcaster stops a stream.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">Link</see>
		/// </summary>
		public const string Stream_Offline = "stream.offline";
		/// <summary>
		/// A user’s authorization has been granted to your client id.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userauthorizationgrant">Link</see>
		/// </summary>
		public const string User_Authorization_Grant = "user.authorization.grant";
		/// <summary>
		/// A user’s authorization has been revoked for your client id.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userauthorizationrevoke">Link</see>
		/// </summary>
		public const string User_Authorization_Revoke = "user.authorization.revoke";
		/// <summary>
		/// A user has updated their account.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userupdate">Link</see>
		/// </summary>
		public const string User_Update = "user.update";
		/// <summary>
		/// A user receives a whisper.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userwhispermessage">Link</see>
		/// </summary>
		public const string User_Whisper_Message = "user.whisper.message";
	}
}
