using System;

using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.ManagersAndBuilders;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.USERNOTICE)]
	public struct USERNOTICE : ITagContainer {

		[field: SerializeField] public string badge_info { get; set; }
		[field: SerializeField] public BadgeData[] badges { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public EmotePosition[] emotes { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string login { get; set; }
		[field: SerializeField] public bool mod { get; set; }
		[field: SerializeField] public string msg_id { get; set; }
		public readonly MsgIDEnum MessageID => (MsgIDEnum)Enum.Parse(typeof(MsgIDEnum), this.msg_id);
		[field: SerializeField] public long room_id { get; set; }
		[field: SerializeField] public BadgeData[] source_badges { get; set; }
		[field: SerializeField] public string source_badge_info { get; set; }
		[field: SerializeField] public string source_id { get; set; }
		[field: SerializeField] public long source_room_id { get; set; }
		[field: SerializeField] public bool subscriber { get; set; }
		[field: SerializeField] public string system_msg { get; set; }
		[field: SerializeField] public long tmi_sent_ts { get; set; }
		public readonly DateTime TimestampDate => TwitchStatic.TwitchUTCStart.AddMilliseconds(this.tmi_sent_ts);
		[field: SerializeField] public bool turbo { get; set; }
		[field: SerializeField] public long user_id { get; set; }
		[field: SerializeField] public string user_type { get; set; }

		[field: SerializeField] public int msg_param_cumulative_months { get; set; }
		[field: SerializeField] public string msg_param_displayName { get; set; }
		[field: SerializeField] public string msg_param_login { get; set; }
		[field: SerializeField] public int msg_param_months { get; set; }
		[field: SerializeField] public int msg_param_promo_gift_total { get; set; }
		[field: SerializeField] public string msg_param_promo_name { get; set; }
		[field: SerializeField] public string msg_param_recipient_display_name { get; set; }
		[field: SerializeField] public long msg_param_recipient_id { get; set; }
		[field: SerializeField] public string msg_param_recipient_user_name { get; set; }
		[field: SerializeField] public string msg_param_sender_login { get; set; }
		[field: SerializeField] public string msg_param_sender_name { get; set; }
		[field: SerializeField] public bool msg_param_should_share_streak { get; set; }
		[field: SerializeField] public int msg_param_streak_months { get; set; }
		[field: SerializeField] public string msg_param_sub_plan { get; set; }
		[field: SerializeField] public string msg_param_sub_plan_name { get; set; }
		[field: SerializeField] public int msg_param_viewerCount { get; set; }
		[field: SerializeField] public string msg_param_threshold { get; set; }
		[field: SerializeField] public int msg_param_gift_months { get; set; }

		public enum MsgIDEnum : byte {
			sub,
			resub,
			subgift,
			submysterygift,
			giftpaidupgrade,
			rewardgift,
			anongiftpaidupgrade,
			raid,
			unraid,
			ritual,
			bitsbadgetier,
			sharedchatnotice,
		}

		public USERNOTICE(JsonValue tags, string chatMessage) {
			bool managerExists = TwitchBadgeManager.GetInstance(out TwitchBadgeManager manager);
			TokenInstance ti = (managerExists && TwitchIRCClient.GetInstance(out TwitchIRCClient client)) ? client.IRCToken : null;

			this.badge_info = tags[TwitchIRCTags.USERNOTICE.BADGE_INFO].AsString;

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.USERNOTICE.BADGES].AsString, out BadgeData[] badgeArray)) {
				this.badges = badgeArray;
			}
			else {
				this.badges = null;
			}

			this.color = tags[TwitchIRCTags.USERNOTICE.COLOR].AsString;
			this.display_name = tags[TwitchIRCTags.USERNOTICE.DISPLAY_NAME].AsString;

			if (ITagContainer.ExtractEmotes(tags[TwitchIRCTags.USERNOTICE.EMOTES].AsString, chatMessage, out EmotePosition[] emoteArray)) {
				this.emotes = emoteArray;
			}
			else {
				this.emotes = null;
			}

			this.id = tags[TwitchIRCTags.USERNOTICE.ID].AsString;
			this.login = tags[TwitchIRCTags.USERNOTICE.LOGIN].AsString;
			this.mod = tags[TwitchIRCTags.USERNOTICE.MOD].AsBoolean;
			this.msg_id = tags[TwitchIRCTags.USERNOTICE.MSG_ID].AsString;

			this.room_id = tags[TwitchIRCTags.USERNOTICE.ROOM_ID].AsLong;
			if (this.room_id != long.MinValue && managerExists) {
				manager.GetChannelBadges(this.room_id, false, ti);
			}

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.USERNOTICE.SOURCE_BADGES].AsString, out BadgeData[] sourceBadgeArray)) {
				this.source_badges = sourceBadgeArray;
			}
			else {
				this.source_badges = null;
			}

			this.source_badge_info = tags[TwitchIRCTags.USERNOTICE.SOURCE_BADGE_INFO].AsString;
			this.source_id = tags[TwitchIRCTags.USERNOTICE.SOURCE_ID].AsString;

			this.source_room_id = tags[TwitchIRCTags.USERNOTICE.SOURCE_ROOM_ID].AsLong;
			if (!string.IsNullOrWhiteSpace(this.source_id) && this.source_room_id != long.MinValue && managerExists) {
				manager.GetChannelBadges(this.source_room_id, false, ti);
			}

			this.subscriber = tags[TwitchIRCTags.USERNOTICE.SUBSCRIBER].AsBoolean;
			this.system_msg = tags[TwitchIRCTags.USERNOTICE.SYSTEM_MSG].AsString;
			this.tmi_sent_ts = tags[TwitchIRCTags.USERNOTICE.TMI_SENT_TS].AsLong;
			this.turbo = tags[TwitchIRCTags.USERNOTICE.TURBO].AsBoolean;
			this.user_id = tags[TwitchIRCTags.USERNOTICE.USER_ID].AsLong;
			this.user_type = tags[TwitchIRCTags.USERNOTICE.USER_TYPE].AsString;

			this.msg_param_cumulative_months = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_CUMULATIVE_MONTHS].AsInteger;
			this.msg_param_displayName = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_DISPLAYNAME].AsString;
			this.msg_param_login = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_LOGIN].AsString;
			this.msg_param_months = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_MONTHS].AsInteger;
			this.msg_param_promo_gift_total = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_PROMO_GIFT_TOTAL].AsInteger;
			this.msg_param_promo_name = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_PROMO_NAME].AsString;
			this.msg_param_recipient_display_name = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_RECIPIENT_DISPLAY_NAME].AsString;
			this.msg_param_recipient_id = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_RECIPIENT_ID].AsLong;
			this.msg_param_recipient_user_name = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_RECIPIENT_USER_NAME].AsString;
			this.msg_param_sender_login = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_SENDER_LOGIN].AsString;
			this.msg_param_sender_name = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_SENDER_NAME].AsString;
			this.msg_param_should_share_streak = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_SHOULD_SHARE_STREAK].AsBoolean;
			this.msg_param_streak_months = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_STREAK_MONTHS].AsInteger;
			this.msg_param_sub_plan = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_SUB_PLAN].AsString;
			this.msg_param_sub_plan_name = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_SUB_PLAN_NAME].AsString;
			this.msg_param_viewerCount = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_VIEWERCOUNT].AsInteger;
			this.msg_param_threshold = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_THRESHOLD].AsString;
			this.msg_param_gift_months = tags[TwitchIRCTags.USERNOTICE.MSG_PARAM_GIFT_MONTHS].AsInteger;
		}
	}
}