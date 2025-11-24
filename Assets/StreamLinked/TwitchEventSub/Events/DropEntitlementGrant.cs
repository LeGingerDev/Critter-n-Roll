using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.EventSub.Events.Objects;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.Events {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#dropentitlementgrant">Twitch Eventsub Reference</see>
	/// </summary>
	[Serializable]
	public struct DropEntitlementGrant : IEvent {

		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public Data[] data { get; set; }

		public readonly TwitchEventSubSubscriptionsEnum Enum => TwitchEventSubSubscriptionsEnum.Drop_Entitlement_Grant;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public DropEntitlementGrant(JsonValue body) {
			this.id = body[TwitchWords.ID].AsString;
			this.data = body[TwitchWords.DATA].AsJsonArray?.ToModelArray<Data>();
		}
	}
}