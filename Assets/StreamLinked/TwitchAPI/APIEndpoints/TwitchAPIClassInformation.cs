using System;

namespace ScoredProductions.StreamLinked.API {

	/// <summary>
	/// Attribute to link the enums correct string name and version value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public class TwitchAPIClassInformation : Attribute {

		public Type LinkedClass { get; }
		public TwitchResourceEnum ResourceType { get; }

		public TwitchAPIClassInformation(Type linkedClass, TwitchResourceEnum ResourceType) {
			if (typeof(ITwitchAPIDataObject).IsAssignableFrom(linkedClass) && (linkedClass.IsClass || linkedClass.IsValueType)) {
				this.LinkedClass = linkedClass;
			} else {
				throw new ArgumentException("Provided type is not a class derived from ITwitchAPIDataObject");
			}

			this.ResourceType = ResourceType;
		}
	}

	public static class TwitchAPIClassExtensions {
		public static Type GetAPIClass(this TwitchAPIClassEnum val) {
			TwitchAPIClassInformation[] attributes = (TwitchAPIClassInformation[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchAPIClassInformation), false);
			return attributes[0].LinkedClass;
		}

		public static string GetEndpoint(this TwitchAPIClassEnum val) {
			TwitchAPIClassInformation[] attributes = (TwitchAPIClassInformation[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchAPIClassInformation), false);
			Type linkedClass = attributes[0].LinkedClass;
			return (string)linkedClass.GetProperty(nameof(ITwitchAPIDataObject.Endpoint)).GetValue(Activator.CreateInstance(linkedClass));
		}

		public static TwitchAPIRequestMethod GetHTTPMethod(this TwitchAPIClassEnum val) {
			TwitchAPIClassInformation[] attributes = (TwitchAPIClassInformation[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchAPIClassInformation), false);
			Type linkedClass = attributes[0].LinkedClass;
			return (TwitchAPIRequestMethod)linkedClass.GetProperty(nameof(ITwitchAPIDataObject.HTTPMethod)).GetValue(Activator.CreateInstance(linkedClass));
		}

		public static TwitchResourceEnum GetResourceType(this TwitchAPIClassEnum val) {
			TwitchAPIClassInformation[] attributes = (TwitchAPIClassInformation[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchAPIClassInformation), false);
			return attributes[0].ResourceType;
		}
	}
}