using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct ChannelChatNotification : IChannel {

		[field: SerializeField] public string broadcaster_user_id { get; set; }
		[field: SerializeField] public string broadcaster_user_name { get; set; }
		[field: SerializeField] public string broadcaster_user_login { get; set; }
		[field: SerializeField] public string chatter_user_id { get; set; }
		[field: SerializeField] public string chatter_user_name { get; set; }
		[field: SerializeField] public string chatter_user_login { get; set; }
		[field: SerializeField] public bool chatter_is_anonymous { get; set; }
		[field: SerializeField] public string color { get; set; }
		[field: SerializeField] public Badge[] badges { get; set; }
		[field: SerializeField] public string system_message { get; set; }
		[field: SerializeField] public string message_id { get; set; }
		[field: SerializeField] public Message message { get; set; }
		/// <summary>
		/// Note: The list below does contain the current set of supported usernotice types that are shared during shared chat. However, these are subject to change.
		/// </summary>
		[field: SerializeField] public string notice_type { get; set; }
		[field: SerializeField] public Sub sub { get; set; }
		[field: SerializeField] public Resub resub { get; set; }
		[field: SerializeField] public SubGift sub_gift { get; set; }
		[field: SerializeField] public CommunitySubGift community_sub_gift { get; set; }
		[field: SerializeField] public GiftPaidUpgrade gift_paid_upgrade { get; set; }
		[field: SerializeField] public PrimePaidUpgrade prime_paid_upgrade { get; set; }
		[field: SerializeField] public Raid raid { get; set; }
		/// <summary>
		/// "Returns an empty payload if notice_type is unraid, otherwise returns null." I've made this a bool to simplify this...
		/// </summary>
		[field: SerializeField] public bool unraid { get; set; }
		[field: SerializeField] public PayItForward pay_it_forward { get; set; }
		[field: SerializeField] public Announcement announcement { get; set; }
		[field: SerializeField] public BitsBadgeTier bits_badge_tier { get; set; }
		[field: SerializeField] public CharityDonation charity_donation { get; set; }
		[field: SerializeField] public string source_broadcaster_user_id { get; set; }
		[field: SerializeField] public string source_broadcaster_user_name { get; set; }
		[field: SerializeField] public string source_broadcaster_user_login { get; set; }
		[field: SerializeField] public string source_message_id { get; set; }
		[field: SerializeField] public Badge[] source_badges { get; set; }
		[field: SerializeField] public Sub shared_chat_sub { get; set; }
		[field: SerializeField] public Resub shared_chat_resub { get; set; }
		[field: SerializeField] public SubGift shared_chat_sub_gift { get; set; }
		[field: SerializeField] public CommunitySubGift shared_chat_community_sub_gift { get; set; }
		[field: SerializeField] public GiftPaidUpgrade shared_chat_gift_paid_upgrade { get; set; }
		[field: SerializeField] public PrimePaidUpgrade shared_chat_prime_paid_upgrade { get; set; }
		[field: SerializeField] public PayItForward shared_chat_pay_it_forward { get; set; }
		[field: SerializeField] public Raid shared_chat_raid { get; set; }
		[field: SerializeField] public Announcement shared_chat_announcement { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Channel_Chat_Notification;
		public readonly TwitchScopesEnum[] Scopes => new TwitchScopesEnum[] {
			TwitchScopesEnum.channel_bot,
			TwitchScopesEnum.user_bot,
			TwitchScopesEnum.user_read_chat,
		};

		public ChannelChatNotification(JsonValue body) {
			this.broadcaster_user_id = body[TwitchWords.BROADCASTER_USER_ID].AsString;
			this.broadcaster_user_name = body[TwitchWords.BROADCASTER_USER_NAME].AsString;
			this.broadcaster_user_login = body[TwitchWords.BROADCASTER_USER_LOGIN].AsString;
			this.chatter_user_id = body[TwitchWords.CHATTER_USER_ID].AsString;
			this.chatter_user_name = body[TwitchWords.CHATTER_USER_NAME].AsString;
			this.chatter_user_login = body[TwitchWords.CHATTER_USER_LOGIN].AsString;
			this.chatter_is_anonymous = body[TwitchWords.CHATTER_IS_ANONYMOUS].AsBoolean;
			this.color = body[TwitchWords.COLOR].AsString;
			this.badges = body[TwitchWords.BADGES].AsJsonArray?.ToModelArray<Badge>();
			this.system_message = body[TwitchWords.SYSTEM_MESSAGE].AsString;
			this.message_id = body[TwitchWords.MESSAGE_ID].AsString;
			this.message = new Message(body[TwitchWords.MESSAGE]);
			this.notice_type = body[TwitchWords.NOTICE_TYPE].AsString;
			this.sub = new Sub(body[TwitchWords.SUB]);
			this.resub = new Resub(body[TwitchWords.RESUB]);
			this.sub_gift = new SubGift(body[TwitchWords.SUB_GIFT]);
			this.community_sub_gift = new CommunitySubGift(body[TwitchWords.COMMUNITY_SUB_GIFT]);
			this.gift_paid_upgrade = new GiftPaidUpgrade(body[TwitchWords.GIFT_PAID_UPGRADE]);
			this.prime_paid_upgrade = new PrimePaidUpgrade(body[TwitchWords.PRIME_PAID_UPGRADE]);
			this.raid = new Raid(body[TwitchWords.RAID]);
			this.unraid = this.notice_type == "unraid";
			this.pay_it_forward = new PayItForward(body[TwitchWords.PAY_IT_FORWARD]);
			this.announcement = new Announcement(body[TwitchWords.ANNOUNCEMENT]);
			this.bits_badge_tier = new BitsBadgeTier(body[TwitchWords.BITS_BADGE_TIER]);
			this.charity_donation = new CharityDonation(body[TwitchWords.CHARITY_DONATION]);
			this.source_broadcaster_user_id = body[TwitchWords.SOURCE_BROADCASTER_USER_ID].AsString;
			this.source_broadcaster_user_name = body[TwitchWords.SOURCE_BROADCASTER_USER_NAME].AsString;
			this.source_broadcaster_user_login = body[TwitchWords.SOURCE_BROADCASTER_USER_LOGIN].AsString;
			this.source_message_id = body[TwitchWords.SOURCE_MESSAGE_ID].AsString;
			this.source_badges = body[TwitchWords.SOURCE_BADGES].AsJsonArray.ToModelArray<Badge>();
			this.shared_chat_sub = new Sub(body[TwitchWords.SHARED_CHAT_SUB]);
			this.shared_chat_resub = new Resub(body[TwitchWords.SHARED_CHAT_RESUB]);
			this.shared_chat_sub_gift = new SubGift(body[TwitchWords.SHARED_CHAT_SUB_GIFT]);
			this.shared_chat_community_sub_gift = new CommunitySubGift(body[TwitchWords.SHARED_CHAT_COMMUNITY_SUB_GIFT]);
			this.shared_chat_gift_paid_upgrade = new GiftPaidUpgrade(body[TwitchWords.SHARED_CHAT_GIFT_PAID_UPGRADE]);
			this.shared_chat_prime_paid_upgrade = new PrimePaidUpgrade(body[TwitchWords.SHARED_CHAT_PRIME_PAID_UPGRADE]);
			this.shared_chat_pay_it_forward = new PayItForward(body[TwitchWords.SHARED_CHAT_PAY_IT_FORWARD]);
			this.shared_chat_raid = new Raid(body[TwitchWords.SHARED_CHAT_RAID]);
			this.shared_chat_announcement = new Announcement(body[TwitchWords.SHARED_CHAT_ANNOUNCEMENT]);
		}
	}
}