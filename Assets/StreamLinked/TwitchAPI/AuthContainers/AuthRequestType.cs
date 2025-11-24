using System;

namespace ScoredProductions.StreamLinked.API.AuthContainers {
	/// <summary>
	/// Client Auth Token types.
	/// </summary>
	[Serializable]
	public enum AuthRequestType {
		ImplicitGrantFlow = 0,
		AuthorizationCodeGrantFlow = 1,
		DeviceCodeGrantFlow = 2,
		ClientCredentialsGrantFlow = 3,
	}
}