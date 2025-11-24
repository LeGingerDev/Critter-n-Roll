using System;

using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;

namespace ScoredProductions.StreamLinked.API.AuthContainers {
	/// <summary>
	/// Base Auth token interface.
	/// </summary>
	public interface IAuthFlow {

		public const string FLOWNAME = "FlowName";
		public const string AUTHREQUESTENUM = "AuthRequestEnum";

		public DateTime OAuthAquireDate { get; }
		public string FlowName { get; }

		public string Access_Token { get; set; }
		public long? Expires_In { get; set; }
		public string Token_Type { get; set; }
		/// <summary>
		/// Gets the Expiry Date of the token from provided information. (<c>OAuthAquireDate</c> + <c>Expires_In</c>)
		/// </summary>
		public DateTime ExpiryDate => this.Expires_In.HasValue
			? this.OAuthAquireDate.AddSeconds(this.Expires_In.Value)
			: DateTime.MinValue;

		public AuthRequestType TypeEnum { get; }
		/// <summary>
		/// Object transformed into a Json Structure
		/// </summary>
		/// <returns></returns>

		public string Out(bool pretty = false) {
			return JsonWriter.Serialize(this.AsJsonValue(), pretty);
		}

		public JsonValue AsJsonValue();
	}
}