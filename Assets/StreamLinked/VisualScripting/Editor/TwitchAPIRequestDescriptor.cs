#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using ScoredProductions.StreamLinked.VisualScripting;

	using Unity.VisualScripting;

	[Descriptor(typeof(TwitchAPIRequest))]
	public class TwitchAPIRequestDescriptor : UnitDescriptor<TwitchAPIRequest> {
		public TwitchAPIRequestDescriptor(TwitchAPIRequest unit) : base(unit) { }

		protected override void DefinedPort(IUnitPort port, UnitPortDescription description) {
			base.DefinedPort(port, description);
			switch (port.key) {
				case nameof(TwitchAPIRequest.HeaderValues):
					description.summary = "The header values [-h] that will be sent up with the request";
					break;
				case nameof(TwitchAPIRequest.QueryParameters):
					description.summary = "The query values [-q] appended to the end of the Endpoint";
					break;
				case nameof(TwitchAPIRequest.RawData):
					description.summary = "The JSON data [-d] send up with the request if the Endpoint supports it";
					break;
				//case nameof(TwitchAPIRequest.OnComplete):
				//	description.summary = "The request is completed.";
				//	break;
				case nameof(TwitchAPIRequest.Result):
					switch (this.target.ReturnType) {
						case TwitchAPIRequest.ReturnValueType.String:
							description.summary = $"If successful this is the returned data as a string";
							break;
						case TwitchAPIRequest.ReturnValueType.JsonValue:
							description.summary = $"If successful this is the returned data stored in a JsonValue object";
							break;
						case TwitchAPIRequest.ReturnValueType.DataContainer:
							description.summary = $"If successful this is the returned data stored in a TwitchAPIDataContainer container of type {this.target.RequestType}";
							break;
					}
					break;
			}
		}
	}

}
#endif