using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.CLEARMSG)]
	public struct CLEARMSG : ITagContainer {

		[field: SerializeField] public string login { get; set; }
		[field: SerializeField] public string room_id { get; set; }
		[field: SerializeField] public string target_msg_id { get; set; }
		[field: SerializeField] public long tmi_sent_ts { get; set; }
		public readonly DateTime TimestampDate => TwitchStatic.TwitchUTCStart.AddMilliseconds(this.tmi_sent_ts);

		public CLEARMSG(JsonValue tags) {
			this.login = tags[TwitchIRCTags.CLEARMSG.LOGIN].AsString;
			this.room_id = tags[TwitchIRCTags.CLEARMSG.ROOM_ID].AsString;
			this.target_msg_id = tags[TwitchIRCTags.CLEARMSG.TARGET_MSG_ID].AsString;
			this.tmi_sent_ts = tags[TwitchIRCTags.CLEARMSG.TMI_SENT_TS].AsLong;
		}
	}
}