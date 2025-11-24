using System;

namespace ScoredProductions.StreamLinked.API {

	[Serializable]
	public enum TwitchResourceEnum {
		Ads = 0,
		Analytics = 1,
		/// <summary>
		/// Not a category/resource on Twitch website but handy for Auth specific
		/// </summary>
		Auth = 2,
		Bits = 3,
		CCLs = 4,
		Channel_Points = 5,
		Channels = 6,
		Charity = 7,
		Chat = 8,
		Clips = 9,
		Conduits = 10,
		Entitlements = 11,
		EventSub = 12,
		Extensions = 13,
		Games = 14,
		Goals = 15,
		Guest_Star = 16,
		Hype_Train = 17,
		Moderation = 18,
		Polls = 19,
		Predictions = 20,
		Raids = 21,
		Schedule = 22,
		Search = 23,
		Streams = 24,
		Subscriptions = 25,
		Teams = 26,
		Users = 27,
		Videos = 28,
		Whispers = 29,
	}
}