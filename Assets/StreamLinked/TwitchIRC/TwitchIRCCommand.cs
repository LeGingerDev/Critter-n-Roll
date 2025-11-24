using System;

namespace ScoredProductions.StreamLinked.IRC {

	[Serializable]
	public enum TwitchIRCCommand : byte {
		NONE = 0,
		JOIN = 1,
		NICK = 2,
		NOTICE = 3,
		PART = 4,
		PASS = 5,
		PING = 6,
		PONG = 7,
		PRIVMSG = 8,
		CLEARCHAT = 9,
		CLEARMSG = 10,
		GLOBALUSERSTATE = 11,
		HOSTTARGET = 12,
		RECONNECT = 13,
		ROOMSTATE = 14,
		USERNOTICE = 15,
		USERSTATE = 16,
		CAP = 17,
	}
}
