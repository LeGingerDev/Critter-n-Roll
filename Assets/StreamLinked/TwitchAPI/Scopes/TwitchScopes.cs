using System;

namespace ScoredProductions.StreamLinked.API.Scopes {

	/// <summary>
	/// All scopes used by Twitch API Auth tokens.
	/// All exact string formats of the scopes.
	/// </summary>
	public static class TwitchScopes {

		/// <summary>
		/// Returns exact API name from enum, listed in <see langword="TwitchAPIScopes"/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetLinkedEnumToString(this TwitchScopesEnum value)
			=> (string)typeof(TwitchScopes).GetField(value.ToString()).GetValue(null);

		/// <summary>
		/// Convert returned API Scopes into the enum
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TwitchScopesEnum GetLinkedStringToEnum(string value)
			=> (TwitchScopesEnum)Enum.Parse(typeof(TwitchScopesEnum), value.Replace(':', '_'));

		/// <summary>
		/// View analytics data for the Twitch Extensions owned by the authenticated account.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-extension-analytics">Get Extension Analytics</see>
		/// </summary>
		public const string analytics_read_extensions = "analytics:read:extensions";
		/// <summary>
		/// View analytics data for the games owned by the authenticated account.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-game-analytics">Get Game Analytics</see>
		/// </summary>
		public const string analytics_read_games = "analytics:read:games";
		/// <summary>
		/// View Bits information for a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-bits-leaderboard">Get Bits Leaderboard</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">Channel Bits Use</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">Channel Cheer</see>
		/// </summary>
		public const string bits_read = "bits:read";
		/// <summary>
		/// Joins your channel’s chatroom as a bot user, and perform chat-related actions as that user.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-message">Send Chat Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear">Channel Chat Clear</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear_user_messages">Channel Chat Clear User Messages</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Channel Chat Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Channel Chat Message Delete</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Channel Chat Notification</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Channel Chat Settings Update</see>
		/// </summary>
		public const string channel_bot = "channel:bot";
		/// <summary>
		/// Manage ads schedule on a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#snooze-next-ad">Snooze Next Ad</see>
		/// </summary>
		public const string channel_manage_ads = "channel:manage:ads";
		/// <summary>
		/// Read the ads schedule and details on your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-ad-schedule">Get Ad Schedule</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Channel Ad Break Begin</see>
		/// </summary>
		public const string channel_read_ads = "channel:read:ads";
		/// <summary>
		/// Manage a channel’s broadcast configuration, including updating channel configuration and managing stream markers and stream tags.
		/// <see href="https://dev.twitch.tv/docs/api/reference#modify-channel-information">Modify Channel Information</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-stream-marker">Create Stream Marker</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#replace-stream-tags">Replace Stream Tags</see>
		/// </summary>
		public const string channel_manage_broadcast = "channel:manage:broadcast";
		/// <summary>
		/// Read charity campaign details and user donations on your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-charity-campaign">Get Charity Campaign</see> | 
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-charity-campaign-donations">Get Charity Campaign Donations</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaigndonate">Charity Donation</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstart">Charity Campaign Start</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignprogress">Charity Campaign Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstop">Charity Campaign Stop</see>
		/// </summary>
		public const string channel_read_charity = "channel:read:charity";
		/// <summary>
		/// Run commercials on a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#start-commercial">Start Commercial</see>
		/// </summary>
		public const string channel_edit_commercial = "channel:edit:commercial";
		/// <summary>
		/// View a list of users with the editor role for a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-channel-editors">Get Channel Editors</see>
		/// </summary>
		public const string channel_read_editors = "channel:read:editors";
		/// <summary>
		/// Manage a channel’s Extension configuration, including activating Extensions.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-user-active-extensions">Add Channel Moderator</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#remove-channel-moderator">Remove Channel Moderator</see>
		/// </summary>
		public const string channel_manage_extensions = "channel:manage:extensions";
		/// <summary>
		/// View Creator Goals for a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-creator-goals">Get Creator Goals</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalbegin">Goal Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalprogress">Goal Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalend">Goal End</see> 
		/// </summary>
		public const string channel_read_goals = "channel:read:goals";
		/// <summary>
		/// Read Guest Star details for your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-channel-guest-star-settings">Get Channel Guest Star Settings</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-guest-star-session">Get Guest Star Session</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-guest-star-invites">Get Guest Star Invites</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Channel Guest Star Session Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Channel Guest Star Session End</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Channel Guest Star Guest Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Channel Guest Star Settings Update</see>
		/// </summary>
		public const string channel_read_guest_star = "channel:read:guest_star";
		/// <summary>
		/// Manage Guest Star for your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-channel-guest-star-settings">Update Channel Guest Star Settings</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-guest-star-session">Create Guest Star Session</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#end-guest-star-session">End Guest Star Session</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#send-guest-star-invite">Send Guest Star Invite</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-guest-star-invite">Delete Guest Star Invite</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#assign-guest-star-slot">Assign Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-guest-star-slot">Update Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-guest-star-slot">Delete Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-guest-star-slot-settings">Update Guest Star Slot Settings</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Channel Guest Star Session Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Channel Guest Star Session End</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Channel Guest Star Guest Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Channel Guest Star Settings Update</see>
		/// </summary>
		public const string channel_manage_guest_star = "channel:manage:guest_star";
		/// <summary>
		/// View Hype Train information for a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-hype-train-events">Get Hype Train Events</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin">Hype Train Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainprogress">Hype Train Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainend">Hype Train End</see>
		/// </summary>
		public const string channel_read_hype_train = "channel:read:hype_train";
		/// <summary>
		/// Add or remove the moderator role from users in your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#add-channel-moderator">Add Channel Moderator</see> | 
		/// <see href="https://dev.twitch.tv/docs/api/reference#remove-channel-moderator">Remove Channel Moderator</see> | 
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-moderators">Get Moderators</see>
		/// </summary>
		public const string channel_manage_moderators = "channel:manage:moderators";
		/// <summary>
		/// View a channel’s polls.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-polls">Get Polls</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Channel Poll Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Channel Poll Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Channel Poll End</see>
		/// </summary>
		public const string channel_read_polls = "channel:read:polls";
		/// <summary>
		/// Manage a channel’s polls.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-polls">Get Polls</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-poll">Create Poll</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#end-poll">End Poll</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Channel Poll Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Channel Poll Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Channel Poll End</see>
		/// </summary>
		public const string channel_manage_polls = "channel:manage:polls";
		/// <summary>
		/// View a channel’s Channel Points Predictions.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-predictions">Get Channel Points Predictions</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionbegin">Channel Prediction Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionprogress">Channel Prediction Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionlock">Channel Prediction Lock</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionend">Channel Prediction End</see>
		/// </summary>
		public const string channel_read_predictions = "channel:read:predictions";
		/// <summary>
		/// Manage of channel’s Channel Points Predictions
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-predictions">Get Channel Points Predictions</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-prediction">Create Channel Points Prediction</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#end-prediction">End Channel Points Prediction</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionbegin">Channel Prediction Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionprogress">Channel Prediction Progress</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionlock">Channel Prediction Lock</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionend">Channel Prediction End</see>
		/// </summary>
		public const string channel_manage_predictions = "channel:manage:predictions";
		/// <summary>
		/// Manage a channel raiding another channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#start-a-raid">Start a raid</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#cancel-a-raid">Cancel a raid</see>
		/// </summary>
		public const string channel_manage_raids = "channel:manage:raids";
		/// <summary>
		/// View Channel Points custom rewards and their redemptions on a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-custom-reward">Get Custom Reward</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-custom-reward-redemption">Get Custom Reward Redemption</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd">Channel Points Automatic Reward Redemption</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd-v2">Channel Points Automatic Reward Redemption v2</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">Channel Points Custom Reward Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">Channel Points Custom Reward Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">Channel Points Custom Reward Remove</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">Channel Points Custom Reward Redemption Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">Channel Points Custom Reward Redemption Update</see>
		/// </summary>
		public const string channel_read_redemptions = "channel:read:redemptions";
		/// <summary>
		/// Manage Channel Points custom rewards and their redemptions on a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-custom-reward">Get Custom Reward</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-custom-reward-redemption">Get Custom Reward Redemption</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-custom-rewards">Create Custom Rewards</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-custom-reward">Delete Custom Reward</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-custom-reward">Update Custom Reward</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-redemption-status">Update Redemption Status</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd">Channel Points Automatic Reward Redemption</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">Channel Points Custom Reward Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">Channel Points Custom Reward Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">Channel Points Custom Reward Remove</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">Channel Points Custom Reward Redemption Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">Channel Points Custom Reward Redemption Update</see> | 
		/// </summary>
		public const string channel_manage_redemptions = "channel:manage:redemptions";
		/// <summary>
		/// Manage a channel’s stream schedule.
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-channel-stream-schedule">Update Channel Stream Schedule</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-channel-stream-schedule-segment">Create Channel Stream Schedule Segment</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-channel-stream-schedule-segment">Update Channel Stream Schedule Segment</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-channel-stream-schedule-segment">Delete Channel Stream Schedule Segment</see> 
		/// </summary>
		public const string channel_manage_schedule = "channel:manage:schedule";
		/// <summary>
		/// View an authorized user’s stream key.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-stream-key">Get Stream Key</see>
		/// </summary>
		public const string channel_read_stream_key = "channel:read:stream_key";
		/// <summary>
		/// View a list of all subscribers to a channel and check if a user is subscribed to a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-broadcaster-subscriptions">Get Broadcaster Subscriptions</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscribe">Channel Subscribe</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionend">Channel Subscription End</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptiongift">Channel Subscription Gift</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionmessage">Channel Subscription Message</see>
		/// </summary>
		public const string channel_read_subscriptions = "channel:read:subscriptions";
		/// <summary>
		/// Manage a channel’s videos, including deleting videos.
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-videos">Delete Videos</see>
		/// </summary>
		public const string channel_manage_videos = "channel:manage:videos";
		/// <summary>
		/// Read the list of VIPs in your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-vips">Get VIPs</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipadd">Channel VIP Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipremove">Channel VIP Remove</see>
		/// </summary>
		public const string channel_read_vips = "channel:read:vips";
		/// <summary>
		/// Add or remove the VIP role from users in your channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-vips">Get VIPs</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#add-channel-vip">Add Channel VIP</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#remove-channel-vip">Remove Channel VIP</see>
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipadd">Channel VIP Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipremove">Channel VIP Remove</see>
		/// </summary>
		public const string channel_manage_vips = "channel:manage:vips";
		/// <summary>
		/// Perform moderation actions in a channel.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelban">Channel Ban</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban">Channel Unban</see>
		/// </summary>
		public const string channel_moderate = "channel:moderate";
		/// <summary>
		/// Manage Clips for a channel.
		/// <see href="https://dev.twitch.tv/docs/api/reference#create-clip">Create Clip</see> 
		/// </summary>
		public const string clips_edit = "clips:edit";
		/// <summary>
		/// View a channel’s moderation data including Moderators, Bans, Timeouts, and Automod settings.
		/// <see href="https://dev.twitch.tv/docs/api/reference#check-automod-status">Check AutoMod Status</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-banned-users">Get Banned Users</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-moderators">Get Moderators</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderatoradd">Channel Moderator Add</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderatorremove">Channel Moderator Remove</see>
		/// </summary>
		public const string moderation_read = "moderation:read";
		/// <summary>
		/// Send announcements in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/api/reference#send-chat-announcement">Send Chat Announcement</see>
		/// </summary>
		public const string moderator_manage_announcements = "moderator:manage:announcements";
		/// <summary>
		/// Manage messages held for review by AutoMod in channels where you are a moderator.
		/// <see href="https://dev.twitch.tv/docs/api/reference#manage-held-automod-messages">Manage Held AutoMod Messages</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessagehold">AutoMod Message Hold</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessagehold-v2">AutoMod Message Hold v2</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessageupdate">AutoMod Message Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#autoMessageupdate-v2">AutoMod Message Update v2</see> | 
		/// <see href="https://dev.twitch.tv/docs/authentication/eventsub/eventsub-subscription-types/#automodtermsupdate">AutoMod Terms Update</see>
		/// </summary>
		public const string moderator_manage_automod = "moderator:manage:automod";
		/// <summary>
		/// View a broadcaster’s AutoMod settings.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-automod-settings">Get AutoMod Settings</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodsettingsupdate">AutoMod Settings Update</see>
		/// </summary>
		public const string moderator_read_automod_settings = "moderator:read:automod_settings";
		/// <summary>
		/// Manage a broadcaster’s AutoMod settings.
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-automod-settings">Update AutoMod Settings</see>
		/// </summary>
		public const string moderator_manage_automod_settings = "moderator:manage:automod_settings";
		/// <summary>
		/// Read the list of bans or unbans in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see>
		/// </summary>
		public const string moderator_read_banned_users = "moderator:read:banned_users";
		/// <summary>
		/// Ban and unban users.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-banned-users">Get Banned Users</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#ban-user">Ban users</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#unban-user">Unban user</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see>
		/// </summary>
		public const string moderator_manage_banned_users = "moderator:manage:banned_users";
		/// <summary>
		/// View a broadcaster’s list of blocked terms.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-blocked-terms">Get Blocked Terms</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_read_blocked_terms = "moderator:read:blocked_terms";
		/// <summary>
		/// Read deleted chat messages in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_read_chat_messages = "moderator:read:chat_messages";
		/// <summary>
		/// Manage a broadcaster’s list of blocked terms.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-blocked-terms">Get Blocked Terms</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#add-blocked-term">Add Blocked Term</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#remove-blocked-term">Remove Blocked Term</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_manage_blocked_terms = "moderator:manage:blocked_terms";
		/// <summary>
		/// Delete chat messages in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-chat-messages">Delete Chat Messages</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_manage_chat_messages = "moderator:manage:chat_messages";
		/// <summary>
		/// View a broadcaster’s chat room settings.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-chat-settings">Get Chat Settings</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_read_chat_settings = "moderator:read:chat_settings";
		/// <summary>
		/// Manage a broadcaster’s chat room settings.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-chat-settings">Update Chat Settings</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_manage_chat_settings = "moderator:manage:chat_settings";
		/// <summary>
		/// View the chatters in a broadcaster’s chat room.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-chatters">Get Chatters</see>
		/// </summary>
		public const string moderator_read_chatters = "moderator:read:chatters";
		/// <summary>
		/// Read the followers of a broadcaster.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-channel-followers">Get Channel Followers</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Channel Follow</see>
		/// </summary>
		public const string moderator_read_followers = "moderator:read:followers";
		/// <summary>
		/// Read Guest Star details for channels where you are a Guest Star moderator.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-channel-guest-star-settings">Get Channel Guest Star Settings</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-guest-star-session">Get Guest Star Session</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-guest-star-invites">Get Guest Star Invites</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Channel Guest Star Session Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Channel Guest Star Session End</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Channel Guest Star Guest Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Channel Guest Star Settings Update</see>
		/// </summary>
		public const string moderator_read_guest_star = "moderator:read:guest_star";
		/// <summary>
		/// Manage Guest Star for channels where you are a Guest Star moderator.
		/// <see href="https://dev.twitch.tv/docs/api/reference#send-guest-star-invite">Send Guest Star Invite</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-guest-star-invite">Delete Guest Star Invite</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#assign-guest-star-slot">Assign Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-guest-star-slot">Update Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#delete-guest-star-slot">Delete Guest Star Slot</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-guest-star-slot-settings">Update Guest Star Slot Settings</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Channel Guest Star Session Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Channel Guest Star Session End</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Channel Guest Star Guest Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Channel Guest Star Settings Update</see>
		/// </summary>
		public const string moderator_manage_guest_star = "moderator:manage:guest_star";
		/// <summary>
		/// Read the list of moderators in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see>
		/// </summary>
		public const string moderator_read_moderators = "moderator:read:moderators";
		/// <summary>
		/// View a broadcaster’s Shield Mode status.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-shield-mode-status">Get Shield Mode Status</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modebegin">Shield Mode Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modeend">Shield Mode End</see>
		/// </summary>
		public const string moderator_read_shield_mode = "moderator:read:shield_mode";
		/// <summary>
		/// Manage a broadcaster’s Shield Mode status.
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-shield-mode-status">Update Shield Mode Status</see>
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modebegin">Shield Mode Begin</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modeend">Shield Mode End</see>
		/// </summary>
		public const string moderator_manage_shield_mode = "moderator:manage:shield_mode";
		/// <summary>
		/// View a broadcaster’s shoutouts.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutcreate">Shoutout Create</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutreceive">Shoutout Received</see>
		/// </summary>
		public const string moderator_read_shoutouts = "moderator:read:shoutouts";
		/// <summary>
		/// Manage a broadcaster’s shoutouts.
		/// <see href="https://dev.twitch.tv/docs/api/reference#send-a-shoutout">Send a Shoutout</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutcreate">Shoutout Create</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutreceive">Shoutout Received</see>
		/// </summary>
		public const string moderator_manage_shoutouts = "moderator:manage:shoutouts";
		/// <summary>
		/// Read chat messages from suspicious users and see users flagged as suspicious in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_usermessage">Channel Suspicious User Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_userupdate">Channel Suspicious User Update</see>
		/// </summary>
		public const string moderator_read_suspicious_users = "moderator:read:suspicious_users";
		/// <summary>
		/// View a broadcaster’s unban requests.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-unban-requests">Get Unban Requests</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestcreate">Channel Unban Request Create</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestresolve">Channel Unban Request Resolve</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_read_unban_requests = "moderator:read:unban_requests";
		/// <summary>
		/// Manage a broadcaster’s unban requests.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#resolve-unban-requests">Resolve Unban Requests</see>
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestcreate">Channel Unban Request Create</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestresolve">Channel Unban Request Resolve</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// </summary>
		public const string moderator_manage_unban_requests = "moderator:manage:unban_requests";
		/// <summary>
		/// Read the list of VIPs in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Channel Moderate</see>
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see>
		/// </summary>
		public const string moderator_read_vips = "moderator:read:vips";
		/// <summary>
		/// Warn users in channels where you have the moderator role.
		/// <see href="hhttps://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningacknowledge">Channel Warning Acknowledge</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningsend">Channel Warning Send</see>
		/// </summary>
		public const string moderator_read_warnings = "moderator:read:warnings";
		/// <summary>
		/// Read warnings in channels where you have the moderator role.
		/// <see href="https://dev.twitch.tv/docs/api/reference#warn-chat-user">Warn Chat User</see> | 
		/// <see href="hhttps://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Channel Moderate v2</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningacknowledge">Channel Warning Acknowledge</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningsend">Channel Warning Send</see>
		/// </summary>
		public const string moderator_manage_warnings = "moderator:manage:warnings";
		/// <summary>
		/// Join a specified chat channel as your user and appear as a bot, and perform chat-related actions as your user.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-message">Send Chat Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear">Channel Chat Clear</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear_user_messages">Channel Chat Clear User Messages</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Channel Chat Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Channel Chat Message Delete</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Channel Chat Notification</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Channel Chat Settings Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_user_message_hold">Channel Chat User Message Hold</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_update">Channel Chat User Message Update</see>
		/// </summary>
		public const string user_bot = "user:bot";
		/// <summary>
		/// Manage a user object.
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-user">Update User</see>
		/// </summary>
		public const string user_edit = "user:edit";
		/// <summary>
		/// View and edit a user’s broadcasting configuration, including Extension configurations.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-extensions">Get User Extensions</see> | 
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-active-extensions">Get User Active Extensions</see> | 
		/// <see href="https://dev.twitch.tv/docs/api/reference/#update-user-extensions">Update User Extensions</see>
		/// </summary>
		public const string user_edit_broadcast = "user:edit:broadcast";
		/// <summary>
		/// View the block list of a user.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-user-block-list">Get User Block List</see>
		/// </summary>
		public const string user_read_blocked_users = "user:read:blocked_users";
		/// <summary>
		/// Manage the block list of a user.
		/// <see href="https://dev.twitch.tv/docs/api/reference#block-user">Block User</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#unblock-user">Unblock User</see>
		/// </summary>
		public const string user_manage_blocked_users = "user:manage:blocked_users";
		/// <summary>
		/// View a user’s broadcasting configuration, including Extension configurations.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-stream-markers">Get Stream Markers</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-user-extensions">Get User Extensions</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-user-active-extensions">Get User Active Extensions</see>
		/// </summary>
		public const string user_read_broadcast = "user:read:broadcast";
		/// <summary>
		/// View live stream chat and room messages.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear">Channel Chat Clear</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear_user_messages">Channel Chat Clear User Messages</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Channel Chat Message</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Channel Chat Message Delete</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Channel Chat Notification</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Channel Chat Settings Update</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_user_message_hold">Channel Chat User Message Hold</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_update">Channel Chat User Message Update</see>
		/// </summary>
		public const string user_read_chat = "user:read:chat";
		/// <summary>
		/// Update the color used for the user’s name in chat.Update User Chat Color
		/// <see href="https://dev.twitch.tv/docs/api/reference#update-user-chat-color">Update User Chat Color</see>
		/// </summary>
		public const string user_manage_chat_color = "user:manage:chat_color";
		/// <summary>
		/// View a user’s email address.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-users">Get Users</see> (optional) | 
		/// <see href="https://dev.twitch.tv/docs/api/reference/#update-user">Update User</see> (optional) | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userupdate">User Update</see> (optional)
		/// </summary>
		public const string user_read_email = "user:read:email";
		/// <summary>
		/// View emotes available to a user
		/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-emotes">Get User Emotes</see>
		/// </summary>
		public const string user_read_emotes = "user:read:emotes";
		/// <summary>
		/// View the list of channels a user follows.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-followed-channels">Get Followed Channels</see> |
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-followed-streams">Get Followed Streams</see>
		/// </summary>
		public const string user_read_follows = "user:read:follows";
		/// <summary>
		/// Read the list of channels you have moderator privileges in.
		/// <see href="https://dev.twitch.tv/docs/api/reference#get-moderated-channels">Get Moderated Channels</see>
		/// </summary>
		public const string user_read_moderated_channels = "user:read:moderated_channels";
		/// <summary>
		/// View if an authorized user is subscribed to specific channels.
		/// <see href="https://dev.twitch.tv/docs/api/reference#check-user-subscription">Check User Subscription</see>
		/// </summary>
		public const string user_read_subscriptions = "user:read:subscriptions";
		/// <summary>
		/// Receive whispers sent to your user.
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userwhispermessage">Whisper Received</see>
		/// </summary>
		public const string user_read_whispers = "user:read:whispers";
		/// <summary>
		/// Read whispers that you send and receive, and send whispers on your behalf.
		/// <see href="https://dev.twitch.tv/docs/api/reference#send-whisper">Send Whisper</see> | 
		/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userwhispermessage">Whisper Received</see>
		/// </summary>
		public const string user_manage_whispers = "user:manage:whispers";
		/// <summary>
		/// Send chat messages to a chatroom.
		/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-message">Send Chat Message</see>
		/// </summary>
		public const string user_write_chat = "user:write:chat";
		/// <summary>
		/// Send chat messages to a chatroom using an IRC connection.
		/// </summary>
		public const string chat_edit = "chat:edit";
		/// <summary>
		/// View chat messages sent in a chatroom using an IRC connection.
		/// </summary>
		public const string chat_read = "chat:read";
		/// <summary>
		/// Receive whisper messages for your user using PubSub.
		/// </summary>
		[Obsolete("Used by pubsub only, pubsub is not implemented in StreamLinked")]
		public const string whispers_read = "whispers:read";
	}
}