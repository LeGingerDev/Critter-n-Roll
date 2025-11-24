using System;

using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.ManagersAndBuilders;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.PRIVMSG)]
	public struct PRIVMSG : ITagContainer {

		[field: SerializeField] public string badge_info { get; set; }
		[field: SerializeField] public BadgeData[] badges { get; set; }
		[field: SerializeField] public int bits { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public EmotePosition[] emotes { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public bool mod { get; set; }
		[field: SerializeField] public string reply_parent_msg_id { get; set; }
		[field: SerializeField] public string reply_parent_user_id { get; set; }
		[field: SerializeField] public string reply_parent_user_login { get; set; }
		[field: SerializeField] public string reply_parent_display_name { get; set; }
		[field: SerializeField] public string reply_parent_msg_body { get; set; }
		[field: SerializeField] public string reply_thread_parent_msg_id { get; set; }
		[field: SerializeField] public string reply_thread_parent_user_login { get; set; }
		[field: SerializeField] public long room_id { get; set; }
		[field: SerializeField] public string source_badge_info { get; set; }
		[field: SerializeField] public BadgeData[] source_badges { get; set; }
		[field: SerializeField] public string source_id { get; set; }
		[field: SerializeField] public bool source_only { get; set; }
		[field: SerializeField] public long source_room_id { get; set; }
		[field: SerializeField] public bool subscriber { get; set; }
		[field: SerializeField] public long tmi_sent_ts { get; set; }
		public readonly DateTime TimestampDate => TwitchStatic.TwitchUTCStart.AddMilliseconds(this.tmi_sent_ts);
		[field: SerializeField] public bool turbo { get; set; }
		[field: SerializeField] public long user_id { get; set; }
		[field: SerializeField] public string user_type { get; set; }
		[field: SerializeField] public bool vip { get; set; }

		public PRIVMSG(JsonValue tags, string chatMessage) {
			bool managerExists = TwitchBadgeManager.GetInstance(out TwitchBadgeManager manager);
			TokenInstance ti = (managerExists && TwitchIRCClient.GetInstance(out TwitchIRCClient client)) ? client.IRCToken : null;

			this.badge_info = tags[TwitchIRCTags.PRIVMSG.BADGE_INFO].AsString;

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.PRIVMSG.BADGES].AsString, out BadgeData[] badgeArray)) {
				this.badges = badgeArray;
			} else {
				this.badges = null;
			}

			this.bits = tags[TwitchIRCTags.PRIVMSG.BITS].AsInteger;
			this.color = tags[TwitchIRCTags.PRIVMSG.COLOR].AsString;
			this.display_name = tags[TwitchIRCTags.PRIVMSG.DISPLAY_NAME].AsString;

			if (ITagContainer.ExtractEmotes(tags[TwitchIRCTags.PRIVMSG.EMOTES].AsString, chatMessage, out EmotePosition[] emoteArray)) {
				this.emotes = emoteArray;
			}
			else {
				this.emotes = null;
			}

			this.id = tags[TwitchIRCTags.PRIVMSG.ID].AsString;
			this.mod = tags[TwitchIRCTags.PRIVMSG.MOD].AsBoolean;
			this.reply_parent_msg_id = tags[TwitchIRCTags.PRIVMSG.REPLY_PARENT_MSG_ID].AsString;
			this.reply_parent_user_id = tags[TwitchIRCTags.PRIVMSG.REPLY_PARENT_USER_ID].AsString;
			this.reply_parent_user_login = tags[TwitchIRCTags.PRIVMSG.REPLY_PARENT_USER_LOGIN].AsString;
			this.reply_parent_display_name = tags[TwitchIRCTags.PRIVMSG.REPLY_PARENT_DISPLAY_NAME].AsString;
			this.reply_parent_msg_body = tags[TwitchIRCTags.PRIVMSG.REPLY_PARENT_MSG_BODY].AsString;
			this.reply_thread_parent_msg_id = tags[TwitchIRCTags.PRIVMSG.REPLY_THREAD_PARENT_MSG_ID].AsString;
			this.reply_thread_parent_user_login = tags[TwitchIRCTags.PRIVMSG.REPLY_THREAD_PARENT_USER_LOGIN].AsString;

			this.room_id = tags[TwitchIRCTags.PRIVMSG.ROOM_ID].AsLong;
			if (this.room_id != long.MinValue && managerExists) {
				manager.GetChannelBadges(this.room_id, false, ti);
			}

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.PRIVMSG.SOURCE_BADGES].AsString, out BadgeData[] sourceBadgeArray)) {
				this.source_badges = sourceBadgeArray;
			}
			else {
				this.source_badges = null;
			}

			this.source_badge_info = tags[TwitchIRCTags.PRIVMSG.SOURCE_BADGE_INFO].AsString;
			this.source_id = tags[TwitchIRCTags.PRIVMSG.SOURCE_ID].AsString;
			this.source_only = tags[TwitchIRCTags.PRIVMSG.SOURCE_ONLY].AsBoolean;

			this.source_room_id = tags[TwitchIRCTags.PRIVMSG.SOURCE_ROOM_ID].AsLong;
			if (!string.IsNullOrWhiteSpace(this.source_id) && this.source_room_id != long.MinValue && managerExists) {
				manager.GetChannelBadges(this.source_room_id, false, ti);
			}

			this.subscriber = tags[TwitchIRCTags.PRIVMSG.SUBSCRIBER].AsBoolean;
			this.tmi_sent_ts = tags[TwitchIRCTags.PRIVMSG.TMI_SENT_TS].AsLong;
			this.turbo = tags[TwitchIRCTags.PRIVMSG.SUBSCRIBER].AsBoolean;
			this.user_id = tags[TwitchIRCTags.PRIVMSG.USER_ID].AsLong;
			this.user_type = tags[TwitchIRCTags.PRIVMSG.USER_TYPE].AsString;
			this.vip = tags[TwitchIRCTags.PRIVMSG.VIP].AsBoolean;
		}
	}
}