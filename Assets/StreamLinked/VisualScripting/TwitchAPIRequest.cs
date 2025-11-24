using System;
using System.Collections;
using System.Collections.Generic;

using ScoredProductions.StreamLinked.API;
using ScoredProductions.StreamLinked.API.AuthContainers;
using ScoredProductions.StreamLinked.LightJson;
using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using Unity.VisualScripting;

using UnityEngine;

namespace ScoredProductions.StreamLinked.VisualScripting {

	[UnitCategory("StreamLinked")]
	[TypeIcon(typeof(FixedJoint))] // Closest I could find to a http link icon?
	public class TwitchAPIRequest : Unit {
		public enum ReturnValueType {
			String,
			JsonValue,
			DataContainer
		}

		[Inspectable]
		public bool RawRequest;

		[Inspectable]
		public bool Coroutine;

		public bool IsNotRaw => !this.RawRequest;
		[InspectableIf(nameof(IsNotRaw))]
		public TwitchAPIClassEnum RequestType = TwitchAPIClassEnum.GetUsers; // Set type in inspector, translates set type on In and Out

		public bool HasReturn => !typeof(INoResponse).IsAssignableFrom(this.RequestType.GetAPIClass());
		public bool NeedsJson => typeof(IJsonRequest).IsAssignableFrom(this.RequestType.GetAPIClass());

		[InspectableIf(nameof(HasReturn))]
		public ReturnValueType ReturnType = ReturnValueType.DataContainer;

		public bool Check => !this.Coroutine;
		[InspectableIf(nameof(Check))]
		public int Timeout = 10000;

		[DoNotSerialize]
		[PortLabelHidden]
		public ControlInput In;

		[DoNotSerialize]
		[AllowsNull]
		public ValueInput Endpoint;
		[DoNotSerialize]
		[AllowsNull]
		public ValueInput HTTPMethod;

		[DoNotSerialize]
		[AllowsNull]
		public ValueInput Credentials;
		[DoNotSerialize]
		[AllowsNull]
		public ValueInput HeaderValues;
		[DoNotSerialize]
		[AllowsNull]
		public ValueInput QueryParameters;
		[DoNotSerialize]
		[AllowsNull]
		public ValueInput RawData;

		[DoNotSerialize]
		[PortLabelHidden]
		public ControlOutput Out;
		[DoNotSerialize]
		public ValueOutput Result;

		private string _result = string.Empty;

		protected override void Definition() {
			this.DefineInputControl();

			this.DefineParameters();

			this.DefineOutputControl();
		}

		protected void DefineInputControl() {
			if (this.Coroutine) {
				this.In = this.ControlInputCoroutine(nameof(this.In), this.MakeRequestCoroutine);
			}
			else {
				this.In = this.ControlInput(nameof(this.In), this.MakeRequestSynchronous);
			}
		}

		protected void DefineParameters() {
			if (this.RawRequest) {
				this.Endpoint = this.ValueInput(nameof(this.Endpoint), TwitchAPIClassEnum.GetUsers.GetEndpoint());
				this.Requirement(this.Endpoint, this.In);
				this.HTTPMethod = this.ValueInput(nameof(this.HTTPMethod), TwitchAPIRequestMethod.GET);
				this.Requirement(this.HTTPMethod, this.In);
			}

			this.Credentials = this.ValueInput<TokenInstance>(nameof(this.Credentials), null).AllowsNull();
			this.HeaderValues = this.ValueInput<List<StringPair>>(nameof(this.HeaderValues), null).AllowsNull();
			this.QueryParameters = this.ValueInput<List<StringPair>>(nameof(this.QueryParameters), null).AllowsNull();

			if (this.NeedsJson) {
				this.RawData = this.ValueInput<string>(nameof(this.RawData), null).AllowsNull();
			}

			// defaults to allow values to not be filled, other ways dont seem to work
			this.HeaderValues.unit.defaultValues.Add(nameof(this.HeaderValues), null);
			this.QueryParameters.unit.defaultValues.Add(nameof(this.QueryParameters), null);
		}

		protected void DefineOutputControl() {
			this.Out = this.ControlOutput(nameof(this.Out));
			if (this.HasReturn) {
				if (this.RawRequest) {
					this.Result = this.ValueOutput(typeof(string), nameof(this.Result), this.ReturnResult);
				}
				else {
					this.Result = this.ValueOutput(
								this.ReturnType switch {
									ReturnValueType.DataContainer => typeof(TwitchAPIDataContainer<>).MakeGenericType(RequestType.GetAPIClass()),
									ReturnValueType.JsonValue => typeof(JsonValue),
									ReturnValueType.String => typeof(string),
									_ => throw new NotImplementedException(),
								},
								nameof(this.Result),
								this.BuildReflectionResult);
				}
			}

			this.Succession(this.In, this.Out);
		}

		public object BuildReflectionResult(Flow flow) {
			switch (this.ReturnType) {
				case ReturnValueType.String:
					return this._result;
				case ReturnValueType.JsonValue:
					return JsonReader.Parse(this._result);
				case ReturnValueType.DataContainer:
					JsonValue parsedResult = JsonReader.Parse(this._result);
					return Activator.CreateInstance(
						typeof(TwitchAPIDataContainer<>).MakeGenericType(RequestType.GetAPIClass()),
						parsedResult);
			}
			throw new NotImplementedException("Return type not supported.");
		}

		public object ReturnResult(Flow flow) {
			return this._result;
		}

		protected ControlOutput MakeRequestSynchronous(Flow flow) {
			string Endpoint;
			TwitchAPIRequestMethod Method;
			if (this.RawRequest) {
				Endpoint = flow.GetValue<string>(this.Endpoint);
				Method = flow.GetValue<TwitchAPIRequestMethod>(this.HTTPMethod);
			}
			else {
				ITwitchAPIDataObject o = (ITwitchAPIDataObject)Activator.CreateInstance(this.RequestType.GetAPIClass());
				Endpoint = o.Endpoint;
				Method = o.HTTPMethod;
			}

			TokenInstance credentials = flow.GetValue<TokenInstance>(this.Credentials);
			(string, string)[] headers = flow.GetValue<List<StringPair>>(this.HeaderValues).ToTuple();
			(string, string)[] querys = flow.GetValue<List<StringPair>>(this.QueryParameters).ToTuple();
			string rawData = flow.GetValue<string>(this.RawData);

			try {
				this._result = TwitchAPIClient.MakeTwitchAPIRequestRaw(
					Endpoint,
					Method,
					this.Timeout,
					credentials,
					headers.IsNullOrEmpty() ? null : headers,
					querys.IsNullOrEmpty() ? null : querys,
					rawData);
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);
			}

			return this.Out;
		}

		protected IEnumerator MakeRequestCoroutine(Flow flow) {
			string Endpoint;
			TwitchAPIRequestMethod Method;
			if (this.RawRequest) {
				Endpoint = flow.GetValue<string>(this.Endpoint);
				Method = flow.GetValue<TwitchAPIRequestMethod>(this.HTTPMethod);
			}
			else {
				ITwitchAPIDataObject o = (ITwitchAPIDataObject)Activator.CreateInstance(this.RequestType.GetAPIClass());
				Endpoint = o.Endpoint;
				Method = o.HTTPMethod;
			}

			TokenInstance credentials = flow.GetValue<TokenInstance>(this.Credentials);
			(string, string)[] headers = flow.GetValue<List<StringPair>>(this.HeaderValues).ToTuple();
			(string, string)[] querys = flow.GetValue<List<StringPair>>(this.QueryParameters).ToTuple();
			string rawData = flow.GetValue<string>(this.RawData);

			yield return TwitchAPIClient.MakeTwitchAPIRequestRaw(
				Endpoint,
				Method,
				credentials,
				headers.IsNullOrEmpty() ? null : headers,
				querys.IsNullOrEmpty() ? null : querys,
				rawData,
				this.GetSuccessValue);

			flow.Run(this.Out);
		}

		private void GetSuccessValue(string jv) {
			this._result = jv;
		}

	}
}
