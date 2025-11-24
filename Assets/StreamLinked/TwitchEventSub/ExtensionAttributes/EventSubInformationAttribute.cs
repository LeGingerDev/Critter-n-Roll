using System;
using System.Reflection;

namespace ScoredProductions.StreamLinked.EventSub.ExtensionAttributes {

	/// <summary>
	/// Attribute to link the enums correct string name and version value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public class EventSubInformationAttribute : Attribute {
		
		public string TwitchName { get; }
		public string Version { get; }
		public Type Class { get; }

		public EventSubInformationAttribute(string TwitchName, string Version, Type Class) {
			this.TwitchName = TwitchName;
			this.Version = Version;
			this.Class = Class;
		}
	}

	public static class EventSubInformationExtensions {
		public static string ToTwitchNameString(this TwitchEventSubSubscriptionsEnum val) {
			EventSubInformationAttribute[] attributes = (EventSubInformationAttribute[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(EventSubInformationAttribute), false);
			return attributes.Length > 0 ? attributes[0].TwitchName : string.Empty;
		}

		public static string ToVersionString(this TwitchEventSubSubscriptionsEnum val) {
			EventSubInformationAttribute[] attributes = (EventSubInformationAttribute[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(EventSubInformationAttribute), false);
			return attributes.Length > 0 ? attributes[0].Version : string.Empty;
		}
		
		public static Type ToLinkedType(this TwitchEventSubSubscriptionsEnum val) {
			EventSubInformationAttribute[] attributes = (EventSubInformationAttribute[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(EventSubInformationAttribute), false);
			return attributes.Length > 0 ? attributes[0].Class : null;
		}

		public static TwitchEventSubSubscriptionsEnum GetEnumFromTwitchName(this string TwitchName) {
			foreach (FieldInfo field in typeof(TwitchEventSubSubscriptionsEnum).GetFields()) {
				if (Attribute.GetCustomAttribute(field, typeof(EventSubInformationAttribute)) is EventSubInformationAttribute attribute
					&& attribute.TwitchName == TwitchName) {
					return (TwitchEventSubSubscriptionsEnum)field.GetValue(null);
				}
			}

			throw new ArgumentException("Not found.", nameof(TwitchName));
		}
	}
}
