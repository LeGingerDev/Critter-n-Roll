using System;
using System.Collections.Generic;

using UnityEngine;

namespace ScoredProductions.StreamLinked.ManagersAndBuilders.Containers {
	/// <summary>
	/// Data container pertaining to a twitch badge
	/// </summary>
	[Serializable]
	public class TwitchBadge {

		public const long GlobalRoomValue = -1;

		[Serializable]
		public enum BadgeSize {
			One,
			Two,
			Three
		}

		[Serializable]
		public class Version {
			[field: SerializeField] public string id { get; set; }
			[field: SerializeField] public string image_url_1x { get; set; }
			[field: SerializeField] public string image_url_2x { get; set; }
			[field: SerializeField] public string image_url_4x { get; set; }
			[field: SerializeField] public string title { get; set; }
			[field: SerializeField] public string description { get; set; }
			[field: SerializeField] public string click_action { get; set; }
			[field: SerializeField] public string click_url { get; set; }
			[field: NonSerialized] public Texture2D ImageData { get; set; }

			public string DownloadURL(BadgeSize size) => size switch {
				BadgeSize.One => this.image_url_1x,
				BadgeSize.Two => this.image_url_2x,
				BadgeSize.Three => this.image_url_4x,
				_ => throw new NotImplementedException()
			};

			public string FullVersionName(TwitchBadge badge) => badge.AssociatedChannel + badge.Set_ID + this.id;
		}

		[field: SerializeField] public bool IsGlobal { get; set; }
		/// <summary>
		/// Room/User that the badge originates from, if the emote is '<c>IsGlobal</c>' then the default value is -1
		/// </summary>
		[field: SerializeField] public long Room_Id { get; set; } = GlobalRoomValue;
		[field: SerializeField] public string Set_ID { get; set; }
		[field: SerializeField] public DateTime LastAccessTime { get; set; }
		[field: SerializeField] public List<Version> Versions { get; set; }
		[field: SerializeField] public bool IgnoreBadge { get; set; }
		[field: SerializeField] public BadgeSize Size { get; set; }
		[field: SerializeField] public string AssociatedChannel { get; set; }

		public bool IsDownloaded(BadgeSize size) {
			if (this.Size != size) {
				return false;
			}
			foreach (Version version in this.Versions) {
				if (version.ImageData == null) {
					return false;
				}
			}
			return true;
		}

		//public string FullVersionName(Version version) => this.AssociatedChannel + this.Set_ID + version.id;
		public string FullVersionName(Version version) {
			Span<char> data = stackalloc char[this.AssociatedChannel.Length + this.Set_ID.Length + version.id.Length];

			int x = 0;
			int y = 0;
			int z = 0;

			while (z < 3) {
				switch (z) {
					case 0:
						for (y = 0; y < this.AssociatedChannel.Length; y++) {
							data[x++] = this.AssociatedChannel[y];
						}
						break;
					case 1:
						for (y = 0; y < this.Set_ID.Length; y++) {
							data[x++] = this.Set_ID[y];
						}
						break;
					case 2:
						for (y = 0; y < version.id.Length; y++) {
							data[x++] = version.id[y];
						}
						break;
				}
				z++;
			}

			return new string(data);
		}

		public string TextmeshFormating(Version version) => $@"<sprite name=""{this.FullVersionName(version)}"">";

		public void MarkUnused() {
			for (int x = 0; x < this.Versions.Count; x++) {
				this.Versions[x].ImageData = null;
			}
			this.IgnoreBadge = true;
		}

	}
}