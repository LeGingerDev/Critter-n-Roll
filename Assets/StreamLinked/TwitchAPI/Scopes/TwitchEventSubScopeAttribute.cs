using System;

using ScoredProductions.StreamLinked.EventSub;

namespace ScoredProductions.StreamLinked.API.Scopes {
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class TwitchEventSubScopeAttribute : TwitchSoftwareCategory
    {
        public readonly TwitchEventSubSubscriptionsEnum[] LinkedSubscriptions;

		public TwitchEventSubScopeAttribute(params TwitchEventSubSubscriptionsEnum[] values) {
            this.LinkedSubscriptions = values;
        }
    }
}
