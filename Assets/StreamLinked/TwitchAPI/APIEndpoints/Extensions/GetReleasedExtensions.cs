using System;

using ScoredProductions.StreamLinked.API.Scopes;
using ScoredProductions.StreamLinked.API.SharedContainers;
using ScoredProductions.StreamLinked.LightJson;

using UnityEngine;

namespace ScoredProductions.StreamLinked.API.Extensions {
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-released-extensions">Twitch API Class</see>
	/// </summary>
	[Serializable]
	public struct GetReleasedExtensions : IExtensions {

		[field: SerializeField] public string author_name { get; set; }
		[field: SerializeField] public bool bits_enabled { get; set; }
		[field: SerializeField] public bool can_install { get; set; }
		[field: SerializeField] public string configuration_location { get; set; }
		[field: SerializeField] public string description { get; set; }
		[field: SerializeField] public string eula_tos_url { get; set; }
		[field: SerializeField] public bool has_chat_support { get; set; }
		[field: SerializeField] public string icon_url { get; set; }
		[field: SerializeField] public IconUrls icon_urls { get; set; }
		[field: SerializeField] public string id { get; set; }
		[field: SerializeField] public string name { get; set; }
		[field: SerializeField] public string privacy_policy_url { get; set; }
		[field: SerializeField] public bool request_identity_link { get; set; }
		[field: SerializeField] public string[] screenshot_urls { get; set; }
		[field: SerializeField] public string state { get; set; }
		[field: SerializeField] public string subscriptions_support_level { get; set; }
		[field: SerializeField] public string summary { get; set; }
		[field: SerializeField] public string support_email { get; set; }
		[field: SerializeField] public string version { get; set; }
		[field: SerializeField] public string viewer_summary { get; set; }
		[field: SerializeField] public Views views { get; set; }
		[field: SerializeField] public string[] allowlisted_config_urls { get; set; }
		[field: SerializeField] public string[] allowlisted_panel_urls { get; set; }

		public void Initialise(JsonValue body) {
			this.author_name = body[TwitchWords.AUTHOR_NAME].AsString;
			this.bits_enabled = body[TwitchWords.BITS_ENABLED].AsBoolean;
			this.can_install = body[TwitchWords.CAN_INSTALL].AsBoolean;
			this.configuration_location = body[TwitchWords.CONFIGURATION_LOCATION].AsString;
			this.description = body[TwitchWords.DESCRIPTION].AsString;
			this.eula_tos_url = body[TwitchWords.EULA_TOS_URL].AsString;
			this.has_chat_support = body[TwitchWords.HAS_CHAT_SUPPORT].AsBoolean;
			this.icon_url = body[TwitchWords.ICON_URL].AsString;
			this.icon_urls = new IconUrls(body[TwitchWords.ICON_URLS]);
			this.id = body[TwitchWords.ID].AsString;
			this.name = body[TwitchWords.NAME].AsString;
			this.privacy_policy_url = body[TwitchWords.PRIVACY_POLICY_URL].AsString;
			this.request_identity_link = body[TwitchWords.REQUEST_IDENTITY_LINK].AsBoolean;
			this.screenshot_urls = body[TwitchWords.SCREENSHOT_URLS].AsJsonArray?.CastToStringArray;
			this.state = body[TwitchWords.STATE].AsString;
			this.subscriptions_support_level = body[TwitchWords.SUBSCRIPTIONS_SUPPORT_LEVEL].AsString;
			this.summary = body[TwitchWords.SUMMARY].AsString;
			this.support_email = body[TwitchWords.SUPPORT_EMAIL].AsString;
			this.version = body[TwitchWords.VERSION].AsString;
			this.viewer_summary = body[TwitchWords.VIEWER_SUMMARY].AsString;
			this.views = new Views(body[TwitchWords.VIEWS]);
			this.allowlisted_config_urls = body[TwitchWords.ALLOWLISTED_CONFIG_URLS].AsJsonArray?.CastToStringArray;
			this.allowlisted_panel_urls = body[TwitchWords.ALLOWLISTED_PANEL_URLS].AsJsonArray?.CastToStringArray;
		}

		public readonly TwitchAPIRequestMethod HTTPMethod => TwitchAPIRequestMethod.GET;
		public readonly string Endpoint => TwitchAPILinks.GetReleasedExtensions;
		public readonly TwitchAPIClassEnum TypeEnum => TwitchAPIClassEnum.GetReleasedExtensions;
		public readonly TwitchScopesEnum[] Scopes => Array.Empty<TwitchScopesEnum>();

		public static string EXTENSION_ID => TwitchWords.EXTENSION_ID;
		public static string EXTENSION_VERSION => TwitchWords.EXTENSION_VERSION;

	}
}