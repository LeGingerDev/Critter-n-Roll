using System;
using System.Reflection;

using ScoredProductions.StreamLinked.IRC.Tags;

namespace ScoredProductions.StreamLinked.IRC.Extensions {

	[Serializable]
	public class IRCCommandType : Attribute {

		public TwitchIRCCommand Command { get; private set; }

		public IRCCommandType(TwitchIRCCommand command) {
			this.Command = command;
		}
	}

	public static class CommandTypeExtensions {

		public static TwitchIRCCommand GetCommandEnum<T>(this T _) where T : ITagContainer {
			return ((IRCCommandType)typeof(T).GetCustomAttribute(typeof(IRCCommandType), true)).Command;
		}

	}
}