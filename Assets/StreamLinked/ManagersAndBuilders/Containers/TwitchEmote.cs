using System;
using System.Collections.Generic;

using UnityEngine;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders.Containers {
	/// <summary>
	/// Data container pertaining to a twitch emote
	/// </summary>
	public class TwitchEmote {

		[Serializable]
		public enum EmoteSize : byte {
			url_1x,
			url_2x,
			url_4x
		}

		[Serializable]
		public enum ThemeMode : byte {
			dark,
			light
		}

		public bool IsGlobal { get; set; }

		public string ID { get; set; }
		public string Name { get; set; }
		public DateTime LastAccessTime { get; set; }
		public int TimesCalled { get; set; }
		public List<Texture2D> DownloadedTexture { get; set; }
		public EmoteSize DownloadedSize { get; set; }
		public ThemeMode DownloadedTheme { get; set; }
		public bool IgnoreEmote { get; set; }
		/// <summary>
		/// If Animated, what is the frame rate
		/// </summary>
		public float FrameRate { get; set; }

		public int TotalFrames => this.DownloadedTexture == null ? 0 : this.DownloadedTexture.Count;
		public bool IsAnimated => this.TotalFrames > 1;
		public bool IsDownloaded(EmoteSize size, ThemeMode theme) => this.TotalFrames > 0 && size == this.DownloadedSize && theme == this.DownloadedTheme;

		public int AnimationStartIndex { get; set; }
		public int AnimationEndIndex { get; set; }

		public string TextmeshFormating => this.IsAnimated
			? $@"<sprite name=""{this.ID}"" anim=""{this.AnimationStartIndex}, {this.AnimationEndIndex}, {this.FrameRate}"">"
			: $@"<sprite name=""{this.ID}"">";

		public void MarkUnused() {
			this.DownloadedTexture.Clear();
			this.IgnoreEmote = true;
		}

	}
}