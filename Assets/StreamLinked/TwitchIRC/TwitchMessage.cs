using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ScoredProductions.StreamLinked.IRC.Tags;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.ManagersAndBuilders;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;

namespace ScoredProductions.StreamLinked.IRC {

	/// <summary>
	/// Container of an IRC message received from Twitch.
	/// </summary>
	[Serializable]
	public struct TwitchMessage {
		[field: SerializeField] public string RawMessage { get; private set; }
		[field: SerializeField] public string Command { get; private set; }
		public readonly TwitchIRCCommand CommandEnum => Enum.TryParse(this.Command, out TwitchIRCCommand @out) ? @out : TwitchIRCCommand.NONE;
		[field: SerializeField] public string FullSender { get; private set; }
		[field: SerializeField] public string Username { get; private set; }
		[field: SerializeField] public string ChatMessage { get; private set; }
		[field: SerializeField] public string[] Parameters { get; private set; }
		[field: SerializeField] public ITagContainer ProcessedTags { get; private set; }

		public TwitchMessage(string message) {

			this.FullSender = string.Empty;
			this.Username = string.Empty;
			this.ChatMessage = string.Empty;
			this.Command = string.Empty;
			this.ProcessedTags = null;

			this.RawMessage = new string(message);

			string tagsString = "";

			int msgLen = message.Length;
			int msgLowerBound = 0;
			int msgUpperBound = msgLen;
			Span<char> messageData = stackalloc char[msgLen];
			for (int x = 0; x < msgLen; x++) {
				messageData[x] = message[x];
			}

			if (messageData[0] == '@') { // Message with tags
				int indexOfSenderParams = messageData.IndexOf(' ');
				tagsString = new string(messageData[..indexOfSenderParams]);
				msgLowerBound = tagsString.Length + 1;
			}

			if (messageData[msgLowerBound] == ':') {
				msgLowerBound++;
			}

			int indexOfMessageStart = -1; // Extract Chat Message
			for (int x = msgLowerBound; x < msgUpperBound; x++) {
				if (messageData[x] == ':') {
					indexOfMessageStart = x;
					break;
				}
			}

			if (indexOfMessageStart != -1) {
				this.ChatMessage = new string(messageData[(indexOfMessageStart + 1)..]);
				msgUpperBound = indexOfMessageStart;
			}

			if ((msgUpperBound - msgLowerBound) == 0) {
				this.Parameters = Array.Empty<string>();
				return;
			}

			int foundParams = 1; // data in between counts as a param even if no spaces are found
			for (int x = msgLowerBound; x < msgUpperBound; x++) {
				if ((x == msgUpperBound - 1) || (messageData[x] == ' ' && (x + 1) != msgUpperBound && (x == 0 || messageData[x - 1] != ' '))) {
					foundParams++;
				}
			}

			this.Parameters = new string[foundParams];
			int paramIndex = 0;
			int paramLower = msgLowerBound;
			for (int x = msgLowerBound; x < msgUpperBound; x++) {
				if ((x == msgUpperBound - 1) || (messageData[x] == ' ' && (x + 1) != msgUpperBound && (x == 0 || messageData[x - 1] != ' '))) {
					this.Parameters[paramIndex++] = new string(messageData[paramLower..x]);
					paramLower = x + 1;
				}
			}
			this.Parameters[paramIndex] = new string(messageData[paramLower..msgUpperBound]);


			this.FullSender = new string(this.Parameters[0]);

			int senderDivider = this.FullSender.IndexOf('!');
			if (senderDivider > 0) {
				this.Username = this.FullSender[..senderDivider];
			}
			else {
				this.Username = new string(this.FullSender);
			}

			if (foundParams < 2) {
				return;
			}

			this.Command = new string(this.Parameters[1]);

			bool tagsExist = !string.IsNullOrEmpty(tagsString);

			// With command type, parse tags
			if (tagsExist && Enum.TryParse(this.Command, out TwitchIRCCommand command)) {
				JsonValue tags = ITagContainer.ExtractTags(tagsString.AsSpan(), command);

				this.ProcessedTags = command switch {
					TwitchIRCCommand.CLEARCHAT => new CLEARCHAT(tags),
					TwitchIRCCommand.CLEARMSG => new CLEARMSG(tags),
					TwitchIRCCommand.GLOBALUSERSTATE => new GLOBALUSERSTATE(tags),
					TwitchIRCCommand.NOTICE => new NOTICE(tags),
					TwitchIRCCommand.PRIVMSG => new PRIVMSG(tags, this.ChatMessage),
					TwitchIRCCommand.ROOMSTATE => new ROOMSTATE(tags),
					TwitchIRCCommand.USERNOTICE => new USERNOTICE(tags, this.ChatMessage),
					TwitchIRCCommand.USERSTATE => new USERSTATE(tags),
					_ => new OTHER(tagsString)
				};
			}
			else if (tagsExist) {
				this.ProcessedTags = new OTHER(tagsString);
			}
		}

		public readonly string[] GetBadgeNames(out long room_id) {
			room_id = 0;
			BadgeData[] badges = null;
			switch (this.ProcessedTags) {
				case GLOBALUSERSTATE gus:
					badges = gus.badges;
					break;
				case PRIVMSG pri:
					if (string.IsNullOrWhiteSpace(pri.source_id)) {
						badges = pri.badges;
						room_id = pri.room_id;
					}
					else {
						badges = pri.source_badges;
						room_id = pri.source_room_id;
					}
					break;
				case USERNOTICE notice:
					if (string.IsNullOrWhiteSpace(notice.source_id)) {
						badges = notice.badges;
						room_id = notice.room_id;
					}
					else {
						badges = notice.source_badges;
						room_id = notice.source_room_id;
					}
					break;
				case USERSTATE state:
					badges = state.badges;
					break;
			}
			if (badges == null) {
				return Array.Empty<string>();
			}
			int len = badges.Length;
			string[] badgeNames = new string[len];
			for (int x = 0; x < len; x++) {
				badgeNames[x] = badges[x].Name;
			}
			return badgeNames;
		}

		public readonly string[] GetEmoteNames() {
			EmotePosition[] receivedEmotes = null;
			switch (this.ProcessedTags) {
				case PRIVMSG pri:
					receivedEmotes = pri.emotes;
					break;
				case USERNOTICE notice:
					receivedEmotes = notice.emotes;
					break;
			}
			if (receivedEmotes == null) {
				return Array.Empty<string>();
			}
			int len = receivedEmotes.Length;
			string[] emoteNames = new string[len];
			for (int x = 0; x < receivedEmotes.Length; x++) {
				emoteNames[x] = receivedEmotes[x].EmoteId;
			}
			return emoteNames;
		}

		public readonly bool CheckHasBadgesOrEmotes() {
			bool check = false;
			switch (this.ProcessedTags) {
				case GLOBALUSERSTATE gus:
					check = gus.badges.Length > 0;
					break;
				case PRIVMSG pri:
					if (string.IsNullOrWhiteSpace(pri.source_id)) {
						check = pri.badges?.Length > 0;
					}
					else {
						check = pri.source_badges?.Length > 0;
					}
					break;
				case USERNOTICE notice:
					if (string.IsNullOrWhiteSpace(notice.source_id)) {
						check = notice.badges?.Length > 0;
					}
					else {
						check = notice.source_badges?.Length > 0;
					}
					break;
				case USERSTATE state:
					check = state.badges.Length > 0;
					break;
			}


			return check;
		}

		public readonly IEnumerator BuildMessageForTextmeshWithWait(Action<(string User, string Message)> Callback) {
			bool emotesExist = TwitchEmoteManager.GetInstance(out TwitchEmoteManager emotesInstance);
			bool badgesExist = TwitchBadgeManager.GetInstance(out TwitchBadgeManager badgeInstance);

			(string user, string message) returningMessage = ("", "");
			StringBuilder builder = new StringBuilder();

			// User / badges
			BadgeData[] badges = null;
			string colour = null;
			EmotePosition[] receivedEmotes = null;
			long room_id = -1;
			switch (this.ProcessedTags) {
				case GLOBALUSERSTATE gus:
					badges = gus.badges;
					colour = gus.color;
					break;
				case PRIVMSG pri:
					if (string.IsNullOrWhiteSpace(pri.source_id)) {
						badges = pri.badges;
						room_id = pri.room_id;
					}
					else {
						badges = pri.source_badges;
						room_id = pri.source_room_id;
					}
					colour = pri.color;
					receivedEmotes = pri.emotes;
					break;
				case USERNOTICE notice:
					if (string.IsNullOrWhiteSpace(notice.source_id)) {
						badges = notice.badges;
						room_id = notice.room_id;
					}
					else {
						badges = notice.source_badges;
						room_id = notice.source_room_id;
					}
					colour = notice.color;
					receivedEmotes = notice.emotes;
					break;
				case USERSTATE state:
					badges = state.badges;
					colour = state.colour;
					break;
			}

			while ((badgesExist && badgeInstance.Busy) | (emotesExist && emotesInstance.Busy)) {
				yield return TwitchStatic.EndOfFrameWait;
			}

			if (badgesExist && badges != null && badges.Length > 0) {
				builder.Append(badgeInstance.GetBadgeTMPText(badges, room_id));
			}

			if (string.IsNullOrEmpty(colour)) {
				builder.Append(this.Username);
			}
			else {
				builder.Append(this.Username.RichTextColour(colour));
			}

			builder.Append(':');
			returningMessage.user = builder.ToString();

			// Message

			builder.Clear();
			int previousEnd = 0;

			if (emotesExist && receivedEmotes != null && receivedEmotes.Length > 0) {
				List<(string emoteName, string textmeshpro)> emoteText = emotesInstance.GetEmotesTMPText(receivedEmotes);
				string text;
				foreach (EmotePosition emotes in receivedEmotes) {
					text = "";
					for (int x = 0; x < emoteText.Count; x++) {
						(string emoteName, string textmeshpro) = emoteText[x];
						if (!string.IsNullOrEmpty(emoteName) && !string.IsNullOrEmpty(textmeshpro) && emoteName.Equals(emotes.EmoteId)) {
							text = textmeshpro;
							break;
						}
					}

					if (!string.IsNullOrEmpty(text)) {
						if (emotes.Start > 0) {
							builder.Append(this.ChatMessage[previousEnd..emotes.Start] + text);
						}
						else {
							builder.Append(text);
						}

						previousEnd = emotes.End;
					}
				}
				if (previousEnd < this.ChatMessage.Length) {
					builder.Append(this.ChatMessage[previousEnd..]);
				}
			}

			if (builder.Length > 0) {
				returningMessage.message = builder.ToString();
			}
			else {
				returningMessage.message = this.ChatMessage;
			}

			Callback.Invoke(returningMessage);
		}

		public readonly (string User, string Message) BuildMessageForTextmesh() {
			(string user, string message) returningMessage = ("", "");
			StringBuilder builder = new StringBuilder();

			// User / badges
			BadgeData[] badges = null;
			string colour = null;
			EmotePosition[] receivedEmotes = null;
			long room_id = -1;
			switch (this.ProcessedTags) {
				case GLOBALUSERSTATE gus:
					badges = gus.badges;
					colour = gus.color;
					break;
				case PRIVMSG pri:
					if (string.IsNullOrWhiteSpace(pri.source_id)) {
						badges = pri.badges;
						room_id = pri.room_id;
					}
					else {
						badges = pri.source_badges;
						room_id = pri.source_room_id;
					}
					colour = pri.color;
					receivedEmotes = pri.emotes;
					break;
				case USERNOTICE notice:
					if (string.IsNullOrWhiteSpace(notice.source_id)) {
						badges = notice.badges;
						room_id = notice.room_id;
					}
					else {
						badges = notice.source_badges;
						room_id = notice.source_room_id;
					}
					colour = notice.color;
					receivedEmotes = notice.emotes;
					break;
				case USERSTATE state:
					badges = state.badges;
					colour = state.colour;
					break;
			}

			if (badges != null && badges.Length > 0 && TwitchBadgeManager.GetInstance(out TwitchBadgeManager badgeInstance)) {
				builder.Append(badgeInstance.GetBadgeTMPText(badges, room_id));
			}

			if (string.IsNullOrEmpty(colour)) {
				builder.Append(this.Username);
			}
			else {
				builder.Append(this.Username.RichTextColour(colour));
			}

			builder.Append(':');
			returningMessage.user = builder.ToString();

			// Message

			builder.Clear();
			int previousEnd = 0;

			if (receivedEmotes != null && receivedEmotes.Length > 0 && TwitchEmoteManager.GetInstance(out TwitchEmoteManager emotesInstance)) {
				List<(string emoteName, string textmeshpro)> emoteText = emotesInstance.GetEmotesTMPText(receivedEmotes);
				string text;
				foreach (EmotePosition emotes in receivedEmotes) {
					text = "";
					for (int x = 0; x < emoteText.Count; x++) {
						(string emoteName, string textmeshpro) = emoteText[x];
						if (!string.IsNullOrEmpty(emoteName) && !string.IsNullOrEmpty(textmeshpro) && emoteName.Equals(emotes.EmoteId)) {
							text = textmeshpro;
							break;
						}
					}

					if (!string.IsNullOrEmpty(text)) {
						if (emotes.Start > 0) {
							builder.Append(this.ChatMessage[previousEnd..emotes.Start] + text);
						}
						else {
							builder.Append(text);
						}

						previousEnd = emotes.End;
					}
				}
				if (previousEnd < this.ChatMessage.Length) {
					builder.Append(this.ChatMessage[previousEnd..]);
				}
			}

			if (builder.Length > 0) {
				returningMessage.message = builder.ToString();
			}
			else {
				returningMessage.message = this.ChatMessage;
			}

			return returningMessage;
		}
	}
}