using System;

namespace ScoredProductions.StreamLinked.API.Scopes {
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class TwitchIRCScopeAttribute : TwitchSoftwareCategory { }
}
