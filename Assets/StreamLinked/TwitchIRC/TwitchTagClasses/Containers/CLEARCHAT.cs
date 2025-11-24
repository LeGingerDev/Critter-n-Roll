using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.CLEARCHAT)]
	public struct CLEARCHAT : ITagContainer {

		[field: SerializeField] public long ban_duration { get; set; }
		[field: SerializeField] public string room_id { get; set; }
		[field: SerializeField] public string target_user_id { get; set; }
		[field: SerializeField] public long tmi_sent_ts { get; set; }
		public readonly DateTime TimestampDate => TwitchStatic.TwitchUTCStart.AddMilliseconds(this.tmi_sent_ts);

		public CLEARCHAT(JsonValue tags) {
			this.ban_duration = tags[TwitchIRCTags.CLEARCHAT.BAN_DURATION].AsLong;
			this.room_id = tags[TwitchIRCTags.CLEARCHAT.ROOM_ID].AsString;
			this.target_user_id = tags[TwitchIRCTags.CLEARCHAT.TARGET_USER_ID].AsString;
			this.tmi_sent_ts = tags[TwitchIRCTags.CLEARCHAT.TMI_SENT_TS].AsLong;
		}
	}
}