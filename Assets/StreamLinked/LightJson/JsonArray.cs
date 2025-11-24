using System;
using System.Collections.Generic;
using System.Diagnostics;

using ScoredProductions.StreamLinked.LightJson.Serialization;
using ScoredProductions.StreamLinked.Utility;

using UnityEngine;

namespace ScoredProductions.StreamLinked.LightJson {
	/// <summary>
	/// Represents an ordered collection of JsonValues.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonArrayDebugView))]
	[Serializable]
	public sealed class JsonArray : IEnumerable<JsonValue>, ISerializationCallbackReceiver {

		private List<JsonValue> items;

		[SerializeField, HideInInspector] private string[] items_store;

		/// <summary>
		/// Gets the number of values in this collection.
		/// </summary>
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>
		/// Gets or sets the value at the given index.
		/// </summary>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <remarks>
		/// The getter will return JsonValue.Null if the given index is out of range.
		/// </remarks>
		public JsonValue this[int index]
		{
			get
			{
				if (index >= 0 && index < this.items.Count) {
					return this.items[index];
				}
				else {
					return JsonValue.Null;
				}
			}
			set
			{
				this.items[index] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of JsonArray.
		/// </summary>
		public JsonArray() {
			this.items = new List<JsonValue>();
		}

		/// <summary>
		/// Initializes a new instance of JsonArray, adding the given values to the collection.
		/// </summary>
		/// <param name="values">The values to be added to this collection.</param>
		public JsonArray(params JsonValue[] values) : this() {
			if (values == null) {
				throw new ArgumentNullException(nameof(values));
			}

			foreach (JsonValue value in values) {
				this.items.Add(value);
			}
		}

		/// <summary>
		/// Initializes a new instance of JsonArray, adding the given values to the collection.
		/// </summary>
		/// <param name="values">The values to be added to this collection.</param>
		public JsonArray(params string[] values) : this() {
			if (values == null) {
				throw new ArgumentNullException(nameof(values));
			}

			foreach (string value in values) {
				this.items.Add(value);
			}
		}

		/// <summary>
		/// Adds the given value to this collection.
		/// </summary>
		/// <param name="value">The value to be added.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray Add(JsonValue value) {
			this.items.Add(value);
			return this;
		}

		/// <summary>
		/// Adds the given value to this collection only if the value is not null.
		/// </summary>
		/// <param name="value">The value to be added.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray AddIfNotNull(JsonValue value) {
			if (!value.IsNull) {
				this.Add(value);
			}

			return this;
		}

		/// <summary>
		/// Inserts the given value at the given index in this collection.
		/// </summary>
		/// <param name="index">The index where the given value will be inserted.</param>
		/// <param name="value">The value to be inserted into this collection.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray Insert(int index, JsonValue value) {
			this.items.Insert(index, value);
			return this;
		}

		/// <summary>
		/// Inserts the given value at the given index in this collection.
		/// </summary>
		/// <param name="index">The index where the given value will be inserted.</param>
		/// <param name="value">The value to be inserted into this collection.</param>
		/// <returns>Returns this collection.</returns>
		public JsonArray InsertIfNotNull(int index, JsonValue value) {
			if (!value.IsNull) {
				this.Insert(index, value);
			}

			return this;
		}

		/// <summary>
		/// Removes the value at the given index.
		/// </summary>
		/// <param name="index">The index of the value to be removed.</param>
		/// <returns>Return this collection.</returns>
		public JsonArray Remove(int index) {
			this.items.RemoveAt(index);
			return this;
		}

		/// <summary>
		/// Clears the contents of this collection.
		/// </summary>
		/// <returns>Returns this collection.</returns>
		public JsonArray Clear() {
			this.items.Clear();
			return this;
		}

		/// <summary>
		/// Determines whether the given item is in the JsonArray.
		/// </summary>
		/// <param name="item">The item to locate in the JsonArray.</param>
		/// <returns>Returns true if the item is found; otherwise, false.</returns>
		public bool Contains(JsonValue item) {
			return this.items.Contains(item);
		}

		/// <summary>
		/// Determines the index of the given item in this JsonArray.
		/// </summary>
		/// <param name="item">The item to locate in this JsonArray.</param>
		/// <returns>The index of the item, if found. Otherwise, returns -1.</returns>
		public int IndexOf(JsonValue item) {
			return this.items.IndexOf(item);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<JsonValue> GetEnumerator() {
			return this.items.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		/// <summary>
		/// Return values as a string array
		/// </summary>
		/// <returns></returns>
		public string[] CastToStringArray
		{
			get
			{
				string[] a = new string[this.items.Count];
				for (int x = 0; x < this.items.Count; x++) {
					a[x] = this.items[x].AsString;
				}

				return a;
			}
		}

		/// <summary>
		/// Returns a JSON string representing the state of the array.
		/// </summary>
		/// <remarks>
		/// The resulting string is safe to be inserted as is into dynamically
		/// generated JavaScript or JSON code.
		/// </remarks>
		public override string ToString() {
			return this.ToString(false);
		}

		/// <summary>
		/// Returns a JSON string representing the state of the array.
		/// </summary>
		/// <remarks>
		/// The resulting string is safe to be inserted as is into dynamically
		/// generated JavaScript or JSON code.
		/// </remarks>
		/// <param name="pretty">
		/// Indicates whether the resulting string should be formatted for human-readability.
		/// </param>
		public string ToString(bool pretty) {
			return JsonWriter.Serialize(this, pretty);
		}

		public T[] ToModelArray<T>() where T : struct {
			T[] modelArray = new T[this.items.Count];

			for (int x = 0; x < this.items.Count; x++) {
				modelArray[x] = (T)Activator.CreateInstance(typeof(T), (object)this.items[x]);
			}

			return modelArray;
		}

		public void OnBeforeSerialize() {
			this.items_store = new string[this.items.Count];
			for (int x = 0; x < this.items.Count; x++) {
				this.items_store[x] = JsonWriter.Serialize(this.items[x]);
			}
		}

		public void OnAfterDeserialize() {
			if (!this.items_store.IsNullOrEmpty()) {
				this.items ??= new List<JsonValue>(this.items_store.Length);
				this.items.Clear();

				for (int x = 0; x < this.items.Count; x++) {
					this.items.Add(JsonReader.Parse(this.items_store[x]));
				}
			}
			this.items_store = null;
		}

		private class JsonArrayDebugView {
			private JsonArray jsonArray;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonValue[] Items
			{
				get
				{
					JsonValue[] items = new JsonValue[this.jsonArray.Count];

					for (int i = 0; i < this.jsonArray.Count; i += 1) {
						items[i] = this.jsonArray[i];
					}

					return items;
				}
			}

			public JsonArrayDebugView(JsonArray jsonArray) {
				this.jsonArray = jsonArray;
			}
		}
	}
}
