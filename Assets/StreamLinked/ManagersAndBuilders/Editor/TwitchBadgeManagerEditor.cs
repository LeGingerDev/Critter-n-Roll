#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System.Collections.Generic;
	using System.Text;

	using ScoredProductions.StreamLinked.ManagersAndBuilders;
	using ScoredProductions.StreamLinked.ManagersAndBuilders.Containers;

	using UnityEditor;

	using UnityEngine;

	[CustomEditor(typeof(TwitchBadgeManager))]
	public class TwitchBadgeManagerEditor : Editor {

		private TwitchBadgeManager source;

		private string search;

		public override bool RequiresConstantRepaint() {
			if (this.source == null) {
				return false;
			}
			return this.source.Busy;
		}

		public override void OnInspectorGUI() {
			this.DrawDefaultInspector();

			if (this.target is TwitchBadgeManager s) {
				this.source = s;
				GUILayout.Space(10);

				GUILayout.Label("Downloader Working: " + this.source.DownloaderWorking.ToString());
				GUILayout.Space(5);

				GUILayout.Label("Badges In Download Queue: " + this.source.CurrentWaitingItems.ToString());
				GUILayout.Space(5);

				GUILayout.Label("Badges Downloaded: " + this.source.GetBadgeCount());
				GUILayout.Space(5);

				this.search = EditorGUILayout.TextField(this.search);
				StringBuilder sb = new StringBuilder();
				List<TwitchBadge> badgeCache = new List<TwitchBadge>();
				foreach (KeyValuePair<long, List<TwitchBadge>> pair in this.source.RoomBadges) {
					badgeCache.Clear();
					if (pair.Value == null || pair.Value.Count == 0) {
						continue;
					}
					for (int x = 0; x < pair.Value.Count; x++) {
						TwitchBadge b = pair.Value[x];
						if (this.IsMatch(b, this.search)) {
							badgeCache.Add(b);
						}
					}
					if (badgeCache.Count == 0) {
						continue;
					}

					GUILayout.Space(3);
					if (pair.Key == TwitchBadge.GlobalRoomValue) {
						EditorGUILayout.LabelField("Global Badges");
					}
					else {
						EditorGUILayout.LabelField(badgeCache[0].AssociatedChannel);
					}
					GUILayout.Space(3);

					foreach (TwitchBadge badge in badgeCache) {
						sb.Clear();
						sb.Append(badge.AssociatedChannel);
						sb.Append("\n");
						sb.Append(badge.Set_ID);
						sb.Append("\n");
						sb.Append("Currently Downloaded: ");
						sb.Append(badge.IsDownloaded(badge.Size));
						sb.Append("\n");
						foreach (TwitchBadge.Version version in badge.Versions) {
							sb.Append("\t { Title: ");
							sb.Append(version.title);
							sb.Append(" } - { ID: ");
							sb.Append(version.id);
							sb.Append(" } - { Description: ");
							sb.Append(version.description);
							sb.Append(" } - { Full Name: ");
							sb.Append(badge.FullVersionName(version));
							sb.Append(" }");
							sb.Append("\n");
						}

						EditorGUILayout.HelpBox(sb.ToString(), MessageType.None);
					}
				}
			}
		}

		public bool IsMatch(TwitchBadge badge, string search) {
			if (string.IsNullOrWhiteSpace(search) || badge.Set_ID.Contains(this.search)) {
				return true;
			}

			if (badge.Versions != null) {
				for (int x = 0; x < badge.Versions.Count; x++) {
					TwitchBadge.Version v = badge.Versions[x];
					if (v.id.Contains(this.search) ||
						v.title.Contains(search) ||
						v.description.Contains(search) ||
						v.FullVersionName(badge).Contains(search)) {
						return true;
					}
				}
			}

			return false;
		}
	}
}
#endif