using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.ROOMSTATE)]
	public struct ROOMSTATE : ITagContainer {

		[field: SerializeField] public bool emote_only { get; set; }
		[field: SerializeField] public int followers_only { get; set; }
		[field: SerializeField] public bool r9k { get; set; }
		[field: SerializeField] public string room_id { get; set; }
		[field: SerializeField] public int slow { get; set; }
		[field: SerializeField] public bool subs_only { get; set; }

		public ROOMSTATE(JsonValue tags) {
			this.emote_only = tags[TwitchIRCTags.ROOMSTATE.EMOTE_ONLY].AsBoolean;
			this.followers_only = tags[TwitchIRCTags.ROOMSTATE.FOLLOWERS_ONLY].AsInteger;
			this.r9k = tags[TwitchIRCTags.ROOMSTATE.R9K].AsBoolean;
			this.room_id = tags[TwitchIRCTags.ROOMSTATE.ROOM_ID].AsString;
			this.slow = tags[TwitchIRCTags.ROOMSTATE.SLOW].AsInteger;
			this.subs_only = tags[TwitchIRCTags.ROOMSTATE.SUBS_ONLY].AsBoolean;
		}
	}
}