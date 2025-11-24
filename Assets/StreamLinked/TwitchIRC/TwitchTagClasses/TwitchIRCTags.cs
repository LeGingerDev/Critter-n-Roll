using System;
using System.Reflection;

namespace ScoredProductions.StreamLinked.IRC.Tags {

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/chat/irc">Twitch Documentation Page</see>
	/// </summary>
	public static class TwitchIRCTags {

		private static string[] Get(Type type) {
			FieldInfo[] fi = type.GetFields();
			int len = fi.Length;
			string[] g = new string[len];
			for (int x = 0; x < len; x++) {
				g[x] = (string)fi[x].GetValue(null);
			}
			return g;
		}

		public static class CLEARCHAT {

			/// <summary>
			/// Optional. The message includes this tag if the user was put in a timeout. The tag contains the duration of the timeout, in seconds.
			/// </summary>
			public const string BAN_DURATION = "ban-duration";
			/// <summary>
			/// The ID of the channel where the messages were removed from.
			/// </summary>
			public const string ROOM_ID = "room-id";
			/// <summary>
			/// Optional. The ID of the user that was banned or put in a timeout. The user was banned if the message doesn’t include the ban-duration tag.
			/// </summary>
			public const string TARGET_USER_ID = "target-user-id";
			/// <summary>
			/// The UNIX timestamp.
			/// </summary>
			public const string TMI_SENT_TS = "tmi-sent-ts";

			private static readonly string[] _getall = Get(typeof(CLEARCHAT));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class CLEARMSG {

			/// <summary>
			/// The name of the user who sent the message.
			/// </summary>
			public const string LOGIN = "login";
			/// <summary>
			/// Optional. The ID of the channel (chat room) where the message was removed from.
			/// </summary>
			public const string ROOM_ID = "room-id";
			/// <summary>
			/// A UUID that identifies the message that was removed.
			/// </summary>
			public const string TARGET_MSG_ID = "target-msg-id";
			/// <summary>
			/// The UNIX timestamp.
			/// </summary>
			public const string TMI_SENT_TS = "tmi-sent-ts";


			private static readonly string[] _getall = Get(typeof(CLEARMSG));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class GLOBALUSERSTATE {

			/// <summary>
			/// Contains metadata related to the chat badges in the badges tag.
			/// Currently, this tag contains metadata only for subscriber badges, to indicate the number of months the user has been a subscriber.
			/// </summary>
			public const string BADGE_INFO = "badge-info";
			/// <summary>
			/// Comma-separated list of chat badges in the form, [badge]/[version]. For example, admin/1. There are many possible badge values, but here are few:
			/// admin, bits, broadcaster, moderator, subscriber, staff, turbo
			/// Most badges have only 1 version, but some badges like subscriber badges offer different versions of the badge depending on how long the user has subscribed.
			/// To get the badge, use the Get Global Chat Badges and Get Channel Chat Badges APIs.Match the badge to the set-id field’s value in the response.Then, match the version to the id field in the list of versions.
			/// </summary>
			public const string BADGES = "badges";
			/// <summary>
			/// The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #[RGB]. This tag may be empty if it is never set.
			/// </summary>
			public const string COLOR = "color";
			/// <summary>
			/// The user’s display name, escaped as described in the IRCv3 spec. This tag may be empty if it is never set.
			/// </summary>
			public const string DISPLAY_NAME = "display-name";
			/// <summary>
			/// A comma-delimited list of IDs that identify the emote sets that the user has access to. Is always set to at least zero (0). To access the emotes in the set, use the Get Emote Sets API.
			/// </summary>
			public const string EMOTE_SETS = "emote-sets";
			/// <summary>
			/// A Boolean value that indicates whether the user has site-wide commercial free mode enabled. Is true (1) if enabled; otherwise, false (0).
			/// </summary>
			public const string TURBO = "turbo";
			/// <summary>
			/// The user’s ID.
			/// </summary>
			public const string USER_ID = "user-id";
			/// <summary>
			/// The type of user. Possible values are:
			/// "" — A normal user,
			/// admin — A Twitch administrator,
			/// global_mod — A global moderator,
			/// staff — A Twitch employee
			/// </summary>
			public const string USER_TYPE = "user-type";


			private static readonly string[] _getall = Get(typeof(GLOBALUSERSTATE));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class NOTICE {

			/// <summary>
			/// An ID that you can use to programmatically determine the action’s outcome. For a list of possible IDs, see NOTICE Message IDs.
			/// </summary>
			public const string MSG_ID = "msg-id";
			/// <summary>
			/// The ID of the user that the action targeted.
			/// </summary>
			public const string TARGET_USER_ID = "target-user-id";


			private static readonly string[] _getall = Get(typeof(NOTICE));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class PRIVMSG {

			/// <summary>
			/// Contains metadata related to the chat badges in the badges tag.
			/// Currently, this tag contains metadata only for subscriber badges, to indicate the number of months the user has been a subscriber.
			/// </summary>
			public const string BADGE_INFO = "badge-info";
			/// <summary>
			/// Comma-separated list of chat badges in the form, [badge]/[version]'. For example, admin/1. There are many possible badge values, but here are few:
			/// admin, bits, broadcaster, moderator, subscriber, staff, turbo.
			/// Most badges have only 1 version, but some badges like subscriber badges offer different versions of the badge depending on how long the user has subscribed.
			/// To get the badge, use the Get Global Chat Badges and Get Channel Chat Badges APIs.Match the badge to the set-id field’s value in the response.Then, match the version to the id field in the list of versions.
			/// </summary>
			public const string BADGES = "badges";
			/// <summary>
			/// The amount of Bits the user cheered. Only a Bits cheer message includes this tag. To learn more about Bits, see the Extensions Monetization Guide. To get the cheermote, use the Get Cheermotes API. Match the cheer amount to the id field’s value in the response. Then, get the cheermote’s URL based on the cheermote theme, type, and size you want to use.
			/// </summary>
			public const string BITS = "bits";
			/// <summary>
			/// The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #[RGB]. This tag may be empty if it is never set.
			/// </summary>
			public const string COLOR = "color";
			/// <summary>
			/// The user’s display name, escaped as described in the IRCv3 spec. This tag may be empty if it is never set.
			/// </summary>
			public const string DISPLAY_NAME = "display-name";
			/// <summary>
			/// A comma-delimited list of emotes and their positions in the message. Each emote is in the form, [emote ID]:[start position]-[end position]. The position indices are zero-based.
			/// To get the actual emote, see the Get Channel Emotes and Get Global Emotes APIs.For information about how to use the information that the APIs return, see Twitch emotes.
			/// NOTE It’s possible for the emotes flag’s value to be set to an action instead of identifying an emote.For example, \001ACTION barfs on the floor.\001.
			/// </summary>
			public const string EMOTES = "emotes";
			/// <summary>
			/// An ID that uniquely identifies the message.
			/// </summary>
			public const string ID = "id";
			/// <summary>
			/// A Boolean value that determines whether the user is a moderator. Is true (1) if the user is a moderator; otherwise, false (0).
			/// </summary>
			public const string MOD = "mod";
			/// <summary>
			/// An ID that uniquely identifies the parent message that this message is replying to. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_PARENT_MSG_ID = "reply-parent-msg-id";
			/// <summary>
			/// An ID that identifies the sender of the parent message. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_PARENT_USER_ID = "reply-parent-user-id";
			/// <summary>
			/// The login name of the sender of the parent message. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_PARENT_USER_LOGIN = "reply-parent-user-login";
			/// <summary>
			/// The display name of the sender of the parent message. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_PARENT_DISPLAY_NAME = "reply-parent-display-name";
			/// <summary>
			/// The text of the parent message. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_PARENT_MSG_BODY = "reply-parent-msg-body";
			/// <summary>
			/// An ID that uniquely identifies the top-level parent message of the reply thread that this message is replying to. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_THREAD_PARENT_MSG_ID = "reply-thread-parent-msg-id";
			/// <summary>
			/// he login name of the sender of the top-level parent message. The message does not include this tag if this message is not a reply.
			/// </summary>
			public const string REPLY_THREAD_PARENT_USER_LOGIN = "reply-thread-parent-user-login";
			/// <summary>
			/// An ID that identifies the chat room (channel).
			/// </summary>
			public const string ROOM_ID = "room-id";
			/// <summary>
			/// Comma-seperated list of chat badges for the chatter in the room the message was sent from. This uses the same format as the badges tag.
			/// </summary>
			public const string SOURCE_BADGES = "source-badges";
			/// <summary>
			/// Contains metadata related to the chat badges in the source-badges tag.
			/// </summary>
			public const string SOURCE_BADGE_INFO = "source-badge-info";
			/// <summary>
			/// A UUID that identifies the source message from the channel the message was sent from.
			/// </summary>
			public const string SOURCE_ID = "source-id";
			/// <summary>
			/// A UUID that identifies the source message from the channel the message was sent from.
			/// </summary>
			public const string SOURCE_ONLY = "source-only";
			/// <summary>
			/// An ID that identifies the chat room (channel) the message was sent from.
			/// </summary>
			public const string SOURCE_ROOM_ID = "source-room-id";
			/// <summary>
			/// A Boolean value that determines whether the user is a subscriber. Is true (1) if the user is a subscriber; otherwise, false (0).
			/// </summary>
			public const string SUBSCRIBER = "subscriber";
			/// <summary>
			/// The UNIX timestamp.
			/// </summary>
			public const string TMI_SENT_TS = "tmi-sent-ts";
			/// <summary>
			/// A Boolean value that indicates whether the user has site-wide commercial free mode enabled. Is true (1) if enabled; otherwise, false (0).
			/// </summary>
			public const string TURBO = "turbo";
			/// <summary>
			/// The user’s ID.
			/// </summary>
			public const string USER_ID = "user-id";
			/// <summary>
			/// The type of user. Possible values are:
			/// "" — A normal user,
			/// admin — A Twitch administrator,
			/// global_mod — A global moderator,
			/// staff — A Twitch employe
			/// </summary>
			public const string USER_TYPE = "user-type";
			/// <summary>
			/// A Boolean value that determines whether the user that sent the chat is a VIP. The message includes this tag if the user is a VIP; otherwise, the message doesn’t include this tag (check for the presence of the tag instead of whether the tag is set to true or false).
			/// </summary>
			public const string VIP = "vip";


			private static readonly string[] _getall = Get(typeof(PRIVMSG));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class ROOMSTATE {

			/// <summary>
			/// A Boolean value that determines whether the chat room allows only messages with emotes. Is true (1) if only emotes are allowed; otherwise, false (0).
			/// </summary>
			public const string EMOTE_ONLY = "emote-only";
			/// <summary>
			/// An integer value that determines whether only followers can post messages in the chat room. The value indicates how long, in minutes, the user must have followed the broadcaster before posting chat messages. If the value is -1, the chat room is not restricted to followers only.
			/// </summary>
			public const string FOLLOWERS_ONLY = "followers-only";
			/// <summary>
			/// A Boolean value that determines whether a user’s messages must be unique. Applies only to messages with more than 9 characters. Is true (1) if users must post unique messages; otherwise, false (0).
			/// </summary>
			public const string R9K = "r9k";
			/// <summary>
			/// An ID that identifies the chat room (channel).
			/// </summary>
			public const string ROOM_ID = "room-id";
			/// <summary>
			/// An integer value that determines how long, in seconds, users must wait between sending messages.
			/// </summary>
			public const string SLOW = "slow";
			/// <summary>
			/// A Boolean value that determines whether only subscribers and moderators can chat in the chat room. Is true (1) if only subscribers and moderators can chat; otherwise, false (0).
			/// </summary>
			public const string SUBS_ONLY = "subs-only";


			private static readonly string[] _getall = Get(typeof(ROOMSTATE));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class USERNOTICE {

			/// <summary>
			/// Contains metadata related to the chat badges in the badges tag.
			/// Currently, this tag contains metadata only for subscriber badges, to indicate the number of months the user has been a subscriber.
			/// </summary>
			public const string BADGE_INFO = "badge-info";
			/// <summary>
			/// Comma-separated list of chat badges in the form, [badge]/[version]. For example, admin/1. There are many possible badge values, but here are few:
			/// admin, bits, broadcaster, moderator, subscriber, staff, turbo
			/// Most badges have only 1 version, but some badges like subscriber badges offer different versions of the badge depending on how long the user has subscribed.
			/// To get the badge, use the Get Global Chat Badges and Get Channel Chat Badges APIs.Match the badge to the set-id field’s value in the response.Then, match the version to the id field in the list of versions.
			/// </summary>
			public const string BADGES = "badges";
			/// <summary>
			/// The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #[RGB]. This tag may be empty if it is never set.
			/// </summary>
			public const string COLOR = "color";
			/// <summary>
			/// The user’s display name, escaped as described in the IRCv3 spec. This tag may be empty if it is never set.
			/// </summary>
			public const string DISPLAY_NAME = "display-name";
			/// <summary>
			/// A comma-delimited list of emotes and their positions in the message. Each emote is in the form, [emote ID]:[start position]-[end position]. The position indices are zero-based.
			/// To get the actual emote, see the Get Channel Emotes and Get Global Emotes APIs.For information about how to use the information that the APIs return, see Twitch emotes.
			/// NOTE It’s possible for the emotes flag’s value to be set to an action instead of identifying an emote.For example, \001ACTION barfs on the floor.\001.
			/// </summary>
			public const string EMOTES = "emotes";
			/// <summary>
			/// An ID that uniquely identifies the message.
			/// </summary>
			public const string ID = "id";
			/// <summary>
			/// The login name of the user whose action generated the message.
			/// </summary>
			public const string LOGIN = "login";
			/// <summary>
			/// A Boolean value that determines whether the user is a moderator. Is true (1) if the user is a moderator; otherwise, false (0).
			/// </summary>
			public const string MOD = "mod";
			/// <summary>
			/// The type of notice (not the ID). Possible values are:
			/// sub, resub, subgift, submysterygift, giftpaidupgrade, rewardgift, anongiftpaidupgrade, raid, unraid, ritual, bitsbadgetier
			/// </summary>
			public const string MSG_ID = "msg-id";
			/// <summary>
			/// An ID that identifies the chat room (channel).
			/// </summary>
			public const string ROOM_ID = "room-id";
			/// <summary>
			/// Comma-seperated list of chat badges for the chatter in the room the message was sent from. This uses the same format as the badges tag.
			/// </summary>
			public const string SOURCE_BADGES = "source-badges";
			/// <summary>
			/// Contains metadata related to the chat badges in the source-badges tag.
			/// </summary>
			public const string SOURCE_BADGE_INFO = "source-badge-info";
			/// <summary>
			/// A UUID that identifies the source message from the channel the message was sent from. This will be the same as id if this is the message sent to the source channel.
			/// </summary>
			public const string SOURCE_ID = "source-id";
			/// <summary>
			/// An ID that identifies the chat room (channel) the USERNOTICE was sent from.
			/// </summary>
			public const string SOURCE_ROOM_ID = "source-room-id";
			/// <summary>
			/// The value of msg-id from the USERNOTICE sent to the source channel.
			/// </summary>
			public const string SOURCE_MSG_ID = "source-msg-id";
			/// <summary>
			/// A Boolean value that determines whether the user is a subscriber. Is true (1) if the user is a subscriber; otherwise, false (0).
			/// </summary>
			public const string SUBSCRIBER = "subscriber";
			/// <summary>
			/// The message Twitch shows in the chat room for this notice.
			/// </summary>
			public const string SYSTEM_MSG = "system-msg";
			/// <summary>
			/// The UNIX timestamp.
			/// </summary>
			public const string TMI_SENT_TS = "tmi-sent-ts";
			/// <summary>
			/// A Boolean value that indicates whether the user has site-wide commercial free mode enabled. Is true (1) if enabled; otherwise, false (0).
			/// </summary>
			public const string TURBO = "turbo";
			/// <summary>
			/// The user’s ID.
			/// </summary>
			public const string USER_ID = "user-id";
			/// <summary>
			/// The type of user. Possible values are:
			/// "" — A normal user,
			/// admin — A Twitch administrator,
			/// global_mod — A global moderator,
			/// staff — A Twitch employe
			/// </summary>
			public const string USER_TYPE = "user-type";
			/// <summary>
			/// Included only with sub and resub notices.
			/// The total number of months the user has subscribed.This is the same as msg-param-months but sent for different types of user notices.
			/// </summary>
			public const string MSG_PARAM_CUMULATIVE_MONTHS = "msg-param-cumulative-months";
			/// <summary>
			/// Included only with raid notices.
			/// The display name of the broadcaster raiding this channel.
			/// </summary>
			public const string MSG_PARAM_DISPLAYNAME = "msg-param-displayName";
			/// <summary>
			/// Included only with raid notices.
			/// The login name of the broadcaster raiding this channel.
			/// </summary>
			public const string MSG_PARAM_LOGIN = "msg-param-login";
			/// <summary>
			/// Included only with subgift notices.
			/// The total number of months the user has subscribed. This is the same as msg-param-cumulative-months but sent for different types of user notices.
			/// </summary>
			public const string MSG_PARAM_MONTHS = "msg-param-months";
			/// <summary>
			/// Included only with anongiftpaidupgrade and giftpaidupgrade notices.
			/// The number of gifts the gifter has given during the promo indicated by msg-param-promo-name.
			/// </summary>
			public const string MSG_PARAM_PROMO_GIFT_TOTAL = "msg-param-promo-gift-total";
			/// <summary>
			/// Included only with anongiftpaidupgrade and giftpaidupgrade notices.
			/// The subscriptions promo, if any, that is ongoing (for example, Subtember 2018).
			/// </summary>
			public const string MSG_PARAM_PROMO_NAME = "msg-param-promo-name";
			/// <summary>
			/// Included only with subgift notices.
			/// The display name of the subscription gift recipient.
			/// </summary>
			public const string MSG_PARAM_RECIPIENT_DISPLAY_NAME = "msg-param-recipient-display-name";
			/// <summary>
			/// Included only with subgift notices.
			/// The user ID of the subscription gift recipient.
			/// </summary>
			public const string MSG_PARAM_RECIPIENT_ID = "msg-param-recipient-id";
			/// <summary>
			/// Included only with subgift notices.
			/// The user name of the subscription gift recipient.
			/// </summary>
			public const string MSG_PARAM_RECIPIENT_USER_NAME = "msg-param-recipient-user-name";
			/// <summary>
			/// Included only with giftpaidupgrade notices.
			/// The login name of the user who gifted the subscription.
			/// </summary>
			public const string MSG_PARAM_SENDER_LOGIN = "msg-param-sender-login";
			/// <summary>
			/// Included only with giftpaidupgrade notices.
			/// The display name of the user who gifted the subscription.
			/// </summary>
			public const string MSG_PARAM_SENDER_NAME = "msg-param-sender-name";
			/// <summary>
			/// Included only with sub and resub notices.
			/// A Boolean value that indicates whether the user wants their streaks shared.
			/// </summary>
			public const string MSG_PARAM_SHOULD_SHARE_STREAK = "msg-param-should-share-streak";
			/// <summary>
			/// Included only with sub and resub notices.
			/// The number of consecutive months the user has subscribed. This is zero (0) if msg-param-should-share-streak is 0.
			/// </summary>
			public const string MSG_PARAM_STREAK_MONTHS = "msg-param-streak-months";
			/// <summary>
			/// Included only with sub, resub and subgift notices.
			/// The type of subscription plan being used. Possible values are:
			/// Prime — Amazon Prime subscription,
			/// 1000 — First level of paid subscription,
			/// 2000 — Second level of paid subscription,
			/// 3000 — Third level of paid subscription
			/// </summary>
			public const string MSG_PARAM_SUB_PLAN = "msg-param-sub-plan";
			/// <summary>
			/// Included only with sub, resub, and subgift notices.
			/// The display name of the subscription plan. This may be a default name or one created by the channel owner.
			/// </summary>
			public const string MSG_PARAM_SUB_PLAN_NAME = "msg-param-sub-plan-name";
			/// <summary>
			/// Included only with raid notices.
			/// The number of viewers raiding this channel from the broadcaster’s channel.
			/// </summary>
			public const string MSG_PARAM_VIEWERCOUNT = "msg-param-viewerCount";
			/// <summary>
			/// Included only with ritual notices.
			/// The name of the ritual being celebrated. Possible values are: new_chatter.
			/// </summary>
			public const string MSG_PARAM_RITUAL_NAME = "msg-param-ritual-name";
			/// <summary>
			/// Included only with bitsbadgetier notices.
			/// The tier of the Bits badge the user just earned. For example, 100, 1000, or 10000.
			/// </summary>
			public const string MSG_PARAM_THRESHOLD = "msg-param-threshold";
			/// <summary>
			/// Included only with subgift notices.
			/// The number of months gifted as part of a single, multi-month gift.
			/// </summary>
			public const string MSG_PARAM_GIFT_MONTHS = "msg-param-gift-months";


			private static readonly string[] _getall = Get(typeof(USERNOTICE));
			public static string[] GetAll => (string[])_getall.Clone();

		}

		public static class USERSTATE {

			/// <summary>
			/// Contains metadata related to the chat badges in the badges tag.
			/// Currently, this tag contains metadata only for subscriber badges, to indicate the number of months the user has been a subscriber.
			/// </summary>
			public const string BADGE_INFO = "badge-info";
			/// <summary>
			/// Comma-separated list of chat badges in the form, [badge]/[version]. For example, admin/1. There are many possible badge values, but here are few:
			/// admin, bits, broadcaster, moderator, subscriber, staff, turbo
			/// Most badges have only 1 version, but some badges like subscriber badges offer different versions of the badge depending on how long the user has subscribed.
			/// To get the badge, use the Get Global Chat Badges and Get Channel Chat Badges APIs.Match the badge to the set-id field’s value in the response.Then, match the version to the id field in the list of versions.
			/// </summary>
			public const string BADGES = "badges";
			/// <summary>
			/// The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #[RGB]. This tag may be empty if it is never set.
			/// </summary>
			public const string COLOR = "color";
			/// <summary>
			/// The user’s display name, escaped as described in the IRCv3 spec. This tag may be empty if it is never set.
			/// </summary>
			public const string DISPLAY_NAME = "display-name";
			/// <summary>
			/// A comma-delimited list of IDs that identify the emote sets that the user has access to. Is always set to at least zero (0). To access the emotes in the set, use the Get Emote Sets API.
			/// </summary>
			public const string EMOTE_SETS = "emote-sets";
			/// <summary>
			/// An ID that uniquely identifies the message.
			/// </summary>
			public const string ID = "id";
			/// <summary>
			/// A Boolean value that determines whether the user is a moderator. Is true (1) if the user is a moderator; otherwise, false (0).
			/// </summary>
			public const string MOD = "mod";
			/// <summary>
			/// A Boolean value that determines whether the user is a subscriber. Is true (1) if the user is a subscriber; otherwise, false (0).
			/// </summary>
			public const string SUBSCRIBER = "subscriber";
			/// <summary>
			/// A Boolean value that indicates whether the user has site-wide commercial free mode enabled. Is true (1) if enabled; otherwise, false (0).
			/// </summary>
			public const string TURBO = "turbo";
			/// <summary>
			/// The type of user. Possible values are:
			/// "" — A normal user,
			/// admin — A Twitch administrator,
			/// global_mod — A global moderator,
			/// staff — A Twitch employe
			/// </summary>
			public const string USER_TYPE = "user-type";


			private static readonly string[] _getall = Get(typeof(USERSTATE));
			public static string[] GetAll => (string[])_getall.Clone();

		}
	}
}
