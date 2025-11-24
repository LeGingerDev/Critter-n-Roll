using System;

using ScoredProductions.StreamLinked.EventSub.ExtensionAttributes;
using ScoredProductions.StreamLinked.EventSub.Interfaces;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.EventSub.WebSocketMessages {
	[Serializable]
	public struct Payload {
		[field: SerializeField] public Session session { get; set; }
		[field: SerializeField] public Subscription subscription { get; set; }
		[field: SerializeField] public IEvent eventData { get; set; }

		public Payload(JsonValue body) {
			this.session = new Session(body[TwitchWords.SESSION]);
			this.subscription = new Subscription(body[TwitchWords.SUBSCRIPTION]);
			
			if (!string.IsNullOrWhiteSpace(this.subscription.type)) {
				TwitchEventSubSubscriptionsEnum typeValue = this.subscription.type.GetEnumFromTwitchName();
				this.eventData = (IEvent)Activator.CreateInstance(typeValue.ToLinkedType(), (object)body[TwitchWords.EVENT]);
			} else {
				this.eventData = null;
			}
		}

	}
}