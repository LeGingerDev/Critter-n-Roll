using System;
using System.Collections.Generic;

using ScoredProductions.StreamLinked.EventSub;

namespace ScoredProductions.StreamLinked.API.Scopes {
    public abstract class TwitchSoftwareCategory : Attribute { }

    public static class TwitchSoftwareCategoryExtensions {

		public static List<TwitchAPIClassEnum> GetAPIClasses(this TwitchScopesEnum val) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			List<TwitchAPIClassEnum> classes = new List<TwitchAPIClassEnum>();
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchAPIScopeAttribute attribute) {
					classes.AddRange(attribute.LinkedClasses);
				}
			}
			return classes;
		}

		public static List<TwitchEventSubSubscriptionsEnum> GetEventSubClasses(this TwitchScopesEnum val) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			List<TwitchEventSubSubscriptionsEnum> classes = new List<TwitchEventSubSubscriptionsEnum>();
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchEventSubScopeAttribute attribute) {
					classes.AddRange(attribute.LinkedSubscriptions);
				}
			}
			return classes;
		}

		public static bool IsIRCScope(this TwitchScopesEnum val) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchIRCScopeAttribute attribute) {
					return true;
				}
			}
			return false;
		}

		public static bool IsAPIScope(this TwitchScopesEnum val) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchAPIScopeAttribute attribute) {
					return true;
				}
			}
			return false;
		}
		
		public static bool IsAPIScope(this TwitchScopesEnum val, out List<TwitchAPIClassEnum> apiClasses) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			bool IsAPIScope = false;
			List<TwitchAPIClassEnum> classes = new List<TwitchAPIClassEnum>();
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchAPIScopeAttribute attribute) {
					IsAPIScope = true;
					classes.AddRange(attribute.LinkedClasses);
				}
			}
			apiClasses = IsAPIScope ? classes : null;
			return IsAPIScope;
		}

		public static bool IsEventSubScope(this TwitchScopesEnum val) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchAPIScopeAttribute attribute) {
					return true;
				}
			}
			return false;
		}
		
		public static bool IsEventSubScope(this TwitchScopesEnum val, out List<TwitchEventSubSubscriptionsEnum> eventsubClasses) {
			TwitchSoftwareCategory[] attributes = (TwitchSoftwareCategory[])val
			   .GetType()
			   .GetField(val.ToString())
			   .GetCustomAttributes(typeof(TwitchSoftwareCategory), true);
			bool IsEventSubScope = false;
			List<TwitchEventSubSubscriptionsEnum> classes = new List<TwitchEventSubSubscriptionsEnum>();
			for (int x = 0; x < attributes.Length; x++) {
				if (attributes[x] is TwitchEventSubScopeAttribute attribute) {
					classes.AddRange(attribute.LinkedSubscriptions);
				}
			}
			eventsubClasses = IsEventSubScope ? classes : null;
			return IsEventSubScope;
		}
	}
}
