using System.Collections;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.Users;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;
using UnityEngine.UI;

namespace ScoredProductions.StreamLinked.TwitchSceneFiles {
	public class GetUserTestScript : MonoBehaviour
    {
        public InputField UserInputField;
		public Text OutputText;

		private Coroutine requestRoutine;

		private void Awake() {
			if (this.UserInputField == null || this.OutputText == null) {
                this.enabled = false;
            }
		}

		public void RequestUserData() {
			if (this.UserInputField == null) {
				DebugManager.LogMessage("Reference empty, canceling call.", DebugManager.ErrorLevel.Error);
				return;
			}
			if (this.requestRoutine != null) {
				DebugManager.LogMessage("Request in progress, please wait.", DebugManager.ErrorLevel.Error);
				return;
			}

			string userName = this.UserInputField.text;

			// (static) Return an Enumerator built for a Coroutine, can get data from either the Enumerator or the Callback
			IEnumerator enumerator = TwitchAPIClient.MakeTwitchAPIRequest<GetUsers>(QueryParameters: new (string, string)[] {
					(GetUsers.LOGIN, userName)
				},
				ScopeSettings: APIScopeWarning.None,
				SuccessCallback: this.SuccessCallback);
			this.requestRoutine = this.StartCoroutine(enumerator);

			//// (instance) Return the coroutine started in the client to get the data, can get data from the Callback
			//if (TwitchAPIClient.GetInstance(out TwitchAPIClient client)) {
			//	this.requestRoutine = client.MakeTwitchAPIRequest<GetUsers>(this.SuccessCallback,
			//		QueryParameters: new (string, string)[] {
			//			(GetUsers.LOGIN, userName)
			//		},
			//		ScopeSettings: APIScopeWarning.None);
			//}

			//// (static) Return a Task to aquire the data, no coroutine, await here and use the data from where you left off
			//TwitchAPIDataContainer<GetUsers> userData1 = await TwitchAPIClient.MakeTwitchAPIRequestAsync<GetUsers>(
			//		QueryParameters: new (string, string)[] {
			//			(GetUsers.LOGIN, userName)
			//		},
			//		ScopeSettings: APIScopeWarning.None);
			//this.SuccessCallback(userData1);

			//// (static) Returns the requested data, no task, no coroutine however hangs the frame until its complete or times out
			//TwitchAPIDataContainer<GetUsers> userData2 = TwitchAPIClient.MakeTwitchAPIRequest<GetUsers>(10000,
			//	QueryParameters: new (string, string)[] {
			//		(GetUsers.LOGIN, userName)
			//	},
			//	ScopeSettings: APIScopeWarning.None);
			//this.SuccessCallback(userData2);

		}

		private void SuccessCallback(TwitchAPIDataContainer<GetUsers> returnedValue) {
			if (!returnedValue.HasErrored) {
				this.OutputText.text = JsonWriter.Serialize(returnedValue, true);
			}
			this.requestRoutine = null;
		}
	}
}
