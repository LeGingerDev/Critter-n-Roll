using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.CCLs {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-content-classification-labels">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetContentClassificationLabels : ICCLs {

		[field: SerializeField] public Label[] content_classification_labels { get; set; }

		public void Initialise(JsonValue body) {
			this.content_classification_labels = body[TwitchWords.CONTENT_CLASSIFICATION_LABELS].AsJsonArray?.ToModelArray<Label>();
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetContentClassificationLabels;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetContentClassificationLabels;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string LOCALE => TwitchWords.LOCALE;
	}
}