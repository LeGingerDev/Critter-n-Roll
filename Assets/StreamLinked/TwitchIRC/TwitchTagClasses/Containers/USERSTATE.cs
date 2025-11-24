using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.USERSTATE)]
	public struct USERSTATE : ITagContainer {

		[field: SerializeField] public string badge_info { get; set; }
		[field: SerializeField] public BadgeData[] badges { get; set; }
		[field: SerializeField] public string colour { get; set; }
		[field: SerializeField] public string displayName { get; set; }
		[field: SerializeField] public int[] emoteSets { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public bool mod { get; set; }
		[field: SerializeField] public bool subscriber { get; set; }
		[field: SerializeField] public bool turbo { get; set; }
		[field: SerializeField] public string user_type { get; set; }


		public USERSTATE(JsonValue tags) {
			this.badge_info = tags[TwitchIRCTags.USERSTATE.BADGE_INFO].AsString;

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.USERSTATE.BADGES].AsString, out BadgeData[] badgeArray)) {
				this.badges = badgeArray;
			}
			else {
				this.badges = null;
			}

			this.colour = tags[TwitchIRCTags.USERSTATE.COLOR].AsString;
			this.displayName = tags[TwitchIRCTags.USERSTATE.DISPLAY_NAME].AsString;

			JsonValue emoteSets = tags[TwitchIRCTags.USERSTATE.EMOTE_SETS];
			if (emoteSets != JsonValue.Null) {
				string[] ids = emoteSets.AsString.Split(',');
				int len = ids.Length;
				this.emoteSets = new int[len];
				for (int x = 0; x < ids.Length; x++) {
					this.emoteSets[x] = int.Parse(ids[x]);
				}
			}
			else {
				this.emoteSets = null;
			}

			this.id = tags[TwitchIRCTags.USERSTATE.ID].AsString;
			this.mod = tags[TwitchIRCTags.USERSTATE.MOD].AsBoolean;
			this.subscriber = tags[TwitchIRCTags.USERSTATE.SUBSCRIBER].AsBoolean;
			this.turbo = tags[TwitchIRCTags.USERSTATE.TURBO].AsBoolean;
			this.user_type = tags[TwitchIRCTags.USERSTATE.USER_TYPE].AsString;
		}
	}
}