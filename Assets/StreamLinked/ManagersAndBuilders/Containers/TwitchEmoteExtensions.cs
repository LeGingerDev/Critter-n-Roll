using System;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders.Containers {
	public static class EmoteExtensions {
		//https://discuss.dev.twitch.tv/t/irc-badge-info-how-to-get-emotes-data/42099/4
		public const string EmoteFormat = "default";

		/// <summary>
		/// https://static-cdn.jtvnw.net/emoticons/v2/{{id}}/{{format}}/{{theme_mode}}/{{scale}}
		/// </summary>
		public const string URLBase = "https://static-cdn.jtvnw.net/emoticons/v2/{0}/{1}/{2}/{3}";

		//EmoteFormat
		private const string gif = "gif";
		private const string png = "png";

		//EmoteSize
		private const string url_1x = "1.0";
		private const string url_2x = "2.0";
		private const string url_4x = "3.0";

		public static string GetFileType(bool isAnimated) {
			return isAnimated ? gif : png;
		}

		public static string GetSizeString(this TwitchEmote.EmoteSize value) {
			return value switch {
				TwitchEmote.EmoteSize.url_1x => url_1x,
				TwitchEmote.EmoteSize.url_2x => url_2x,
				TwitchEmote.EmoteSize.url_4x => url_4x,
				_ => throw new NotImplementedException("EmoteSize not implemented to create Image URL")
			};
		}

		public static string CreateImageURL(this TwitchEmote emote, TwitchEmote.ThemeMode theme, TwitchEmote.EmoteSize size) {
			return string.Format(URLBase,
				emote.ID,
				EmoteFormat,
				theme.ToString(),
				size.GetSizeString());
		}
	}
}