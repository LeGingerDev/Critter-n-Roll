using System;

using ScoredProductions.StreamLinked.IRC.Extensions;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC.Tags {
	[Serializable]
	[IRCCommandType(TwitchIRCCommand.GLOBALUSERSTATE)]
	public struct GLOBALUSERSTATE : ITagContainer {

		[field: SerializeField] public int badge_info { get; set; }
		[field: SerializeField] public BadgeData[] badges { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public string display_name { get; set; }
		[field: SerializeField] public int[] emoteSets { get; set; }
		[field: SerializeField] public bool turbo { get; set; }
		[field: SerializeField] public string user_id { get; set; }
		[field: SerializeField] public string user_type { get; set; }

		public GLOBALUSERSTATE(JsonValue tags) {
			this.badge_info = tags[TwitchIRCTags.GLOBALUSERSTATE.BADGE_INFO].AsInteger;

			if (ITagContainer.ExtractBadges(tags[TwitchIRCTags.GLOBALUSERSTATE.BADGES].AsString, out BadgeData[] badgeArray)) {
				this.badges = badgeArray;
			}
			else {
				this.badges = null;
			}

			this.color = tags[TwitchIRCTags.GLOBALUSERSTATE.COLOR].AsString;
			this.display_name = tags[TwitchIRCTags.GLOBALUSERSTATE.DISPLAY_NAME].AsString;

			JsonValue emoteSets = tags[TwitchIRCTags.GLOBALUSERSTATE.EMOTE_SETS];
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

			this.turbo = tags[TwitchIRCTags.GLOBALUSERSTATE.TURBO].AsBoolean;
			this.user_id = tags[TwitchIRCTags.GLOBALUSERSTATE.USER_ID].AsString;
			this.user_type = tags[TwitchIRCTags.GLOBALUSERSTATE.USER_TYPE].AsString;
		}
	}
}