using System;

namespace ScoredProductions.StreamLinked.API.Scopes {

	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class TwitchAPIScopeAttribute : TwitchSoftwareCategory
    {
        public readonly TwitchAPIClassEnum[] LinkedClasses;

		public TwitchAPIScopeAttribute(params TwitchAPIClassEnum[] values) {
            this.LinkedClasses = values;
		}
    }
}
