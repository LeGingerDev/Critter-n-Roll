#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System.Collections.Generic;
	using System.Text;

	using ScoredProductions.StreamLinked.ManagersAndBuilders;
	using ScoredProductions.StreamLinked.ManagersAndBuilders.Containers;

	using UnityEditor;

	using UnityEngine;

	[CustomEditor(typeof(TwitchEmoteManager))]
	public class TwitchEmoteManagerEditor : Editor {

		private TwitchEmoteManager source;

		private List<TwitchEmote> emotesContainer = new List<TwitchEmote>();

		private string search;

		public override bool RequiresConstantRepaint() {
			if (this.source == null) {
				return false;
			}
			return this.source.Busy;
		}

		public override void OnInspectorGUI() {
			this.DrawDefaultInspector();

			if (this.target is TwitchEmoteManager s) {
				this.source = s;
				GUILayout.Space(5);

				GUILayout.Label($"Downloader Working: {this.source.DownloaderWorking}");
				GUILayout.Space(5);

				GUILayout.Label($"Emotes In Download Queue: {this.source.CurrentWaitingItems}");
				GUILayout.Space(5);

				this.emotesContainer.Clear();
				this.emotesContainer.AddRange(this.source.DownloadedEmotes);

				GUILayout.Label($"Emotes Downloaded: {this.emotesContainer.Count}");

				if (this.emotesContainer.Count > 0) {
					StringBuilder sb = new StringBuilder();
					this.search = EditorGUILayout.TextField(this.search);
					for (int x = 0; x < this.emotesContainer.Count; x++) {
						TwitchEmote emote = this.emotesContainer[x];
						if (this.IsMatch(emote, this.search)) {
							sb.Append("- ");
							sb.Append(emote.ID);
							sb.Append("\n\t");
							sb.Append("Name: ");
							sb.Append(emote.Name);
							sb.Append("\n\t");
							sb.Append("Is Global: ");
							sb.Append(emote.IsGlobal.ToString());
							sb.Append("\n\t");
							sb.Append("Animated: ");
							sb.Append(emote.IsAnimated);
							if (emote.IsAnimated) {
								sb.Append("\n\t");
								sb.Append("FrameRate: ");
								sb.Append(emote.FrameRate);
								sb.Append("\n\t");
								sb.Append("Total Frames: ");
								sb.Append(emote.TotalFrames);
								sb.Append("\n\t");
								sb.Append("Current Atlas Positions: ");
								sb.Append($"({emote.AnimationStartIndex}, {emote.AnimationEndIndex})");
							}
							sb.Append("\n\t");
							sb.Append("Times Called: ");
							sb.Append(emote.TimesCalled);
							sb.Append("\n\t");
							sb.Append("Last time used: ");
							sb.Append(emote.LastAccessTime);
							sb.Append("\n\t");
							sb.Append("Currently Downloaded: ");
							sb.Append(emote.IsDownloaded(emote.DownloadedSize, emote.DownloadedTheme));

							EditorGUILayout.HelpBox(sb.ToString(), MessageType.None);
							sb.Clear();
						}
					}
				}
			}
		}

		public bool IsMatch(TwitchEmote emote, string search) {
			return string.IsNullOrWhiteSpace(search) || emote.ID.Contains(this.search) || emote.Name.Contains(this.search);
		}
	}
}
#endif