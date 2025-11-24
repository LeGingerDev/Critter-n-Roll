using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelModerateV2 : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string moderator_id { get; set; }
		[field: SerializeField] public string moderator_login { get; set; }
		[field: SerializeField] public string moderator_name { get; set; }
		[field: SerializeField] public string action { get; set; }
		[field: SerializeField] public Followers followers { get; set; }
		[field: SerializeField] public Slow slow { get; set; }
		[field: SerializeField] public Vip vip { get; set; }
		[field: SerializeField] public Unvip unvip { get; set; }
		[field: SerializeField] public Mod mod { get; set; }
		[field: SerializeField] public Unmod unmod { get; set; }
		[field: SerializeField] public Ban ban { get; set; }
		[field: SerializeField] public Unban unban { get; set; }
		[field: SerializeField] public Timeout timeout { get; set; }
		[field: SerializeField] public Untimeout untimeout { get; set; }
		[field: SerializeField] public Raid raid { get; set; }
		[field: SerializeField] public Unraid unraid { get; set; }
		[field: SerializeField] public Delete delete { get; set; }
		[field: SerializeField] public AutomodTerms automod_terms { get; set; }
		[field: SerializeField] public UnbanRequest unban_request { get; set; }
		[field: SerializeField] public Warn warn { get; set; }
		[field: SerializeField] public Ban shared_chat_ban { get; set; }
		[field: SerializeField] public Unban shared_chat_unban { get; set; }
		[field: SerializeField] public Timeout shared_chat_timeout { get; set; }
		[field: SerializeField] public Untimeout shared_chat_untimeout { get; set; }
		[field: SerializeField] public Delete shared_chat_delete { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Moderate_V2;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.moderator_read_banned_users,
			TwitchScopesEnum.moderator_manage_banned_users,
			TwitchScopesEnum.moderator_read_blocked_terms,
			TwitchScopesEnum.moderator_read_chat_messages,
			TwitchScopesEnum.moderator_manage_blocked_terms,
			TwitchScopesEnum.moderator_manage_chat_messages,
			TwitchScopesEnum.moderator_read_chat_settings,
			TwitchScopesEnum.moderator_manage_chat_settings,
			TwitchScopesEnum.moderator_read_moderators,
			TwitchScopesEnum.moderator_read_unban_requests,
			TwitchScopesEnum.moderator_manage_unban_requests,
			TwitchScopesEnum.moderator_read_vips,
			TwitchScopesEnum.moderator_read_warnings,
			TwitchScopesEnum.moderator_manage_warnings,
		};

		public ChannelModerateV2(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.moderator_id = body[TwitchWords.MODERATOR_ID].AsString;
			this.moderator_login = body[TwitchWords.MODERATOR_LOGIN].AsString;
			this.moderator_name = body[TwitchWords.MODERATOR_NAME].AsString;
			this.action = body[TwitchWords.ACTION].AsString;
			this.followers = new Followers(body[TwitchWords.FOLLOWERS]);
			this.slow = new Slow(body[TwitchWords.SLOW]);
			this.vip = new Vip(body[TwitchWords.VIP]);
			this.unvip = new Unvip(body[TwitchWords.UNVIP]);
			this.mod = new Mod(body[TwitchWords.MOD]);
			this.unmod = new Unmod(body[TwitchWords.UNMOD]);
			this.ban = new Ban(body[TwitchWords.BAN]);
			this.unban = new Unban(body[TwitchWords.UNBAN]);
			this.timeout = new Timeout(body[TwitchWords.TIMEOUT]);
			this.untimeout = new Untimeout(body[TwitchWords.UNTIMEOUT]);
			this.raid = new Raid(body[TwitchWords.RAID]);
			this.unraid = new Unraid(body[TwitchWords.UNRAID]);
			this.delete = new Delete(body[TwitchWords.DELETE]);
			this.automod_terms = new AutomodTerms(body[TwitchWords.AUTOMOD_TERMS]);
			this.unban_request = new UnbanRequest(body[TwitchWords.UNBAN_REQUEST]);
			this.warn = new Warn(body[TwitchWords.WARN]);
			this.shared_chat_ban = new Ban(body[TwitchWords.SHARED_CHAT_BAN]);
			this.shared_chat_unban = new Unban(body[TwitchWords.SHARED_CHAT_UNBAN]);
			this.shared_chat_timeout = new Timeout(body[TwitchWords.SHARED_CHAT_TIMEOUT]);
			this.shared_chat_untimeout = new Untimeout(body[TwitchWords.SHARED_CHAT_UNTIMEOUT]);
			this.shared_chat_delete = new Delete(body[TwitchWords.SHARED_CHAT_DELETE]);
		}
	}
}