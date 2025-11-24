using System;

namespace ScoredProductions.StreamLinked.IRC {

	[Flags]
	public enum TwitchUserPerm : byte {
		Viewer = 0x1,
		Subscriber = 0x2,
		VIP = 0x4,
		Moderator = 0x8,
		Admin = 0x10,
		Global_Moderator = 0x20,
		Broadcaster = 0x40,
		Staff = 0x80
	}
}