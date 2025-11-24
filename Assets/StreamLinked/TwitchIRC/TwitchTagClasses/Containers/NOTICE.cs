using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.NOTICE)]
	public struct NOTICE : ITagContainer {

		[field: SerializeField] public string msg_id { get; set; }
		public readonly MessageTag NoticeOutcome => Enum.TryParse(this.msg_id, out MessageTag result) ? result : MessageTag.Other;
		[field: SerializeField] public string target_user_id { get; set; }


		public NOTICE(JsonValue tags) {
			this.msg_id = tags[TwitchIRCTags.NOTICE.MSG_ID].AsString;
			this.target_user_id = tags[TwitchIRCTags.NOTICE.TARGET_USER_ID].AsString;
		}


		[Serializable]
		public enum MessageTag : byte {
			Other = 0,
			/// <summary>
			/// This room is no longer in emote-only mode.
			/// </summary>
			emote_only_off = 1,
			/// <summary>
			/// This room is now in emote-only mode.
			/// </summary>
			emote_only_on = 2,
			/// <summary>
			/// This room is no longer in followers-only mode.
			/// </summary>
			followers_off = 3,
			/// <summary>
			/// This room is now in [duration] followers-only mode.
			/// </summary>
			followers_on = 4,
			/// <summary>
			/// This room is now in followers-only mode.
			/// </summary>
			followers_on_zero = 5,
			/// <summary>
			/// You are permanently banned from talking in [channel].
			/// </summary>
			msg_banned = 6,
			/// <summary>
			/// Your message was not sent because it contained too many unprocessable characters. If you believe this is an error, please rephrase and try again.
			/// </summary>
			msg_bad_characters = 7,
			/// <summary>
			/// Your message was not sent because your account is not in good standing in this channel.
			/// </summary>
			msg_channel_blocked = 8,
			/// <summary>
			/// This channel does not exist or has been suspended.
			/// </summary>
			msg_channel_suspended = 9,
			/// <summary>
			/// Your message was not sent because it is identical to the previous one you sent, less than 30 seconds ago.
			/// </summary>
			msg_duplicate = 10,
			/// <summary>
			/// This room is in emote-only mode. You can find your currently available emoticons using the smiley in the chat text area.
			/// </summary>
			msg_emoteonly = 11,
			/// <summary>
			/// This room is in [duration] followers-only mode. Follow [channel] to join the community! Note: These <c>msg_followers</c> tags are kickbacks to a user who does not meet the criteria; that is, does not follow or has not followed long enough.
			/// </summary>
			msg_followersonly = 12,
			/// <summary>
			/// This room is in [duration1] followers-only mode. You have been following for [duration2]. Continue following to chat!
			/// </summary>
			msg_followersonly_followed = 13,
			/// <summary>
			/// This room is in followers-only mode. Follow [channel] to join the community!
			/// </summary>
			msg_followersonly_zero = 14,
			/// <summary>
			/// This room is in unique-chat mode and the message you attempted to send is not unique.
			/// </summary>
			msg_r9k = 15,
			/// <summary>
			/// Your message was not sent because you are sending messages too quickly.
			/// </summary>
			msg_ratelimit = 16,
			/// <summary>
			/// Hey! Your message is being checked by mods and has not been sent.
			/// </summary>
			msg_rejected = 17,
			/// <summary>
			/// Your message wasn’t posted due to conflicts with the channel’s moderation settings.
			/// </summary>
			msg_rejected_mandatory = 18,
			/// <summary>
			/// A verified phone number is required to chat in this channel. Please visit <see href="https://www.twitch.tv/settings/security"/> to verify your phone number.
			/// </summary>
			msg_requires_verified_phone_number = 19,
			/// <summary>
			/// This room is in slow mode and you are sending messages too quickly. You will be able to talk again in [number] seconds.
			/// </summary>
			msg_slowmode = 20,
			/// <summary>
			/// This room is in subscribers only mode. To talk, purchase a channel subscription at https://www.twitch.tv/products/[broadcaster login name]/ticket?ref=subscriber_only_mode_chat.
			/// </summary>
			msg_subsonly = 21,
			/// <summary>
			/// You don’t have permission to perform that action.
			/// </summary>
			msg_suspended = 22,
			/// <summary>
			/// You are timed out for [number] more seconds.
			/// </summary>
			msg_timedout = 23,
			/// <summary>
			/// This room requires a verified account to chat. Please verify your account at <see href="https://www.twitch.tv/settings/security"/>.
			/// </summary>
			msg_verified_email = 24,
			/// <summary>
			/// This room is no longer in slow mode.
			/// </summary>
			slow_off = 25,
			/// <summary>
			/// This room is now in slow mode. You may send messages every [number] seconds.
			/// </summary>
			slow_on = 26,
			/// <summary>
			/// This room is no longer in subscribers-only mode.
			/// </summary>
			subs_off = 27,
			/// <summary>
			/// This room is now in subscribers-only mode.
			/// </summary>
			subs_on = 28,
			/// <summary>
			/// The community has closed channel [channel] due to Terms of Service violations.
			/// </summary>
			tos_ban = 29,
			/// <summary>
			/// Unrecognized command: [command]
			/// </summary>
			unrecognized_cmd = 30,
		}
	}
}