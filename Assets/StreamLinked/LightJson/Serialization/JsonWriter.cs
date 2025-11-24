using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ScoredProductions.StreamLinked.LightJson.Serialization {
	using ErrorType = JsonSerializationException.ErrorType;

	/// <summary>
	/// Represents a TextWriter adapter that can write string representations of JsonValues.
	/// </summary>
	public sealed class JsonWriter
	{
		private int indent;
		private bool isNewLine;

		/// <summary>
		/// A set of containing all the collection objects (JsonObject/JsonArray) being rendered.
		/// It is used to prevent circular references; since collections that contain themselves
		/// will never finish rendering.
		/// </summary>
		private HashSet<IEnumerable<JsonValue>> renderingCollections;

		/// <summary>
		/// Gets or sets the string representing a indent in the output.
		/// </summary>
		public string IndentString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a space in the output.
		/// </summary>
		public string SpacingString { get; set; }

		/// <summary>
		/// Gets or sets the string representing a new line on the output.
		/// </summary>
		public string NewLineString { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether JsonObject properties should be written in a deterministic order.
		/// </summary>
		public bool SortObjects { get; set; }

		/// <summary>
		/// Gets or sets the TextWriter to which this JsonWriter writes.
		/// </summary>
		public TextWriter InnerWriter { get; set; }

		/// <summary>
		/// Initializes a new instance of JsonWriter.
		/// </summary>
		/// <param name="innerWriter">The TextWriter used to write JsonValues.</param>
		public JsonWriter(TextWriter innerWriter) : this(innerWriter, false) { }

		/// <summary>
		/// Initializes a new instance of JsonWriter.
		/// </summary>
		/// <param name="innerWriter">The TextWriter used to write JsonValues.</param>
		/// <param name="pretty">
		/// A value indicating whether the output of the writer should be human-readable.
		/// </param>
		public JsonWriter(TextWriter innerWriter, bool pretty)
		{
			if (pretty)
			{
				this.IndentString = "\t";
				this.SpacingString = " ";
				this.NewLineString = "\n";
			}

			this.renderingCollections = new HashSet<IEnumerable<JsonValue>>();

			this.InnerWriter = innerWriter;
		}

		private void Write(string text)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				this.WriteIndentation();
			}

			this.InnerWriter.Write(text);
		}
		
		private void Write(char character)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				this.WriteIndentation();
			}

			this.InnerWriter.Write(character);
		}

		private void WriteEncodedJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
					this.Write("null");
					break;
				case JsonValueType.Boolean:
					this.Write(value.AsString);
					break;
				case JsonValueType.Number:
					if (!IsValidNumber(value))
					{
						throw new JsonSerializationException(ErrorType.InvalidNumber);
					}

					this.Write(((double)value).ToString(CultureInfo.InvariantCulture));
					break;
				case JsonValueType.String:
					this.WriteEncodedString((string)value);
					break;
				case JsonValueType.Object:
					this.Write(string.Format("JsonObject[{0}]", value.AsJsonObject.Count));
					break;
				case JsonValueType.Array:
					this.Write(string.Format("JsonArray[{0}]", value.AsJsonArray.Count));
					break;
				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		private void WriteEncodedString(string text)
		{
			this.Write('\"');

			for (int i = 0; i < text.Length; i += 1)
			{
				char currentChar = text[i];

				// Encoding special characters.
				switch (currentChar)
				{
					case '\\':
						this.InnerWriter.Write("\\\\");
						break;
					case '\"':
						this.InnerWriter.Write("\\\"");
						break;
					case '/':
						this.InnerWriter.Write("\\/");
						break;
					case '\b':
						this.InnerWriter.Write("\\b");
						break;
					case '\f':
						this.InnerWriter.Write("\\f");
						break;
					case '\n':
						this.InnerWriter.Write("\\n");
						break;
					case '\r':
						this.InnerWriter.Write("\\r");
						break;
					case '\t':
						this.InnerWriter.Write("\\t");
						break;
					default:
						this.InnerWriter.Write(currentChar);
						break;
				}
			}

			this.InnerWriter.Write('\"');
		}

		private void WriteIndentation()
		{
			for (int i = 0; i < this.indent; i++)
			{
				this.Write(this.IndentString);
			}
		}

		private void WriteSpacing()
		{
			this.Write(this.SpacingString);
		}

		private void WriteLine()
		{
			this.Write(this.NewLineString);
			this.isNewLine = true;
		}

		private void WriteLine(string line)
		{
			this.Write(line);
			this.WriteLine();
		}
		
		private void WriteLine(char line)
		{
			this.Write(line);
			this.WriteLine();
		}

		private void AddRenderingCollection(IEnumerable<JsonValue> value)
		{
			if (!this.renderingCollections.Add(value))
			{
				throw new JsonSerializationException(ErrorType.CircularReference);
			}
		}

		private void RemoveRenderingCollection(IEnumerable<JsonValue> value)
		{
			this.renderingCollections.Remove(value);
		}

		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
				case JsonValueType.Boolean:
				case JsonValueType.Number:
				case JsonValueType.String:
					this.WriteEncodedJsonValue(value);
					break;
				case JsonValueType.Object:
					this.Render((JsonObject)value);
					break;
				case JsonValueType.Array:
					this.Render((JsonArray)value);
					break;
				default:
					throw new JsonSerializationException(ErrorType.InvalidValueType);
			}
		}

		private void Render(JsonArray value)
		{
			this.AddRenderingCollection(value);

			this.WriteLine('[');

			this.indent += 1;

			using (IEnumerator<JsonValue> enumerator = value.GetEnumerator())
			{
				bool hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					this.Render(enumerator.Current);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						this.WriteLine(',');
					}
					else
					{
						this.WriteLine();
					}
				}
			}

			this.indent -= 1;

			this.Write(']');

			this.RemoveRenderingCollection(value);
		}

		private void Render(JsonObject value)
		{
			this.AddRenderingCollection(value);

			this.WriteLine('{');

			this.indent += 1;

			using(IEnumerator<KeyValuePair<string, JsonValue>> enumerator = this.GetJsonObjectEnumerator(value))
			{
				bool hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					this.WriteEncodedString(enumerator.Current.Key);
					this.Write(':');
					this.WriteSpacing();
					this.Render(enumerator.Current.Value);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						this.WriteLine(',');
					}
					else
					{
						this.WriteLine();
					}
				}
			}

			this.indent -= 1;

			this.Write('}');

			this.RemoveRenderingCollection(value);
		}

		/// <summary>
		/// Gets an JsonObject enumarator based on the configuration of this JsonWriter.
		/// If JsonWriter.SortObjects is set to true, then a ordered enumerator is returned.
		/// Otherwise, a faster non-deterministic enumerator is returned.
		/// </summary>
		/// <param name="jsonObject">The JsonObject for which to get an enumerator.</param>
		private IEnumerator<KeyValuePair<string, JsonValue>> GetJsonObjectEnumerator(JsonObject jsonObject)
		{
			if (this.SortObjects)
			{
				SortedDictionary<string, JsonValue> sortedDictionary = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);

				foreach (KeyValuePair<string, JsonValue> item in jsonObject)
				{
					sortedDictionary.Add(item.Key, item.Value);
				}

				return sortedDictionary.GetEnumerator();
			}
			else
			{
				return jsonObject.GetEnumerator();
			}
		}

		/// <summary>
		/// Writes the given value to the InnerWriter.
		/// </summary>
		/// <param name="jsonValue">The JsonValue to write.</param>
		public void Write(JsonValue jsonValue)
		{
			this.indent = 0;
			this.isNewLine = true;

			this.Render(jsonValue);

			this.renderingCollections.Clear();
		}

		private static bool IsValidNumber(double number)
		{
			return !(double.IsNaN(number) || double.IsInfinity(number));
		}


		/// <summary>
		/// Attempts to automatically serialize a struct into a JSON, always check output. Will only serialize ValueType (and ValueType array) <c>Properties</c>.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		public static string Serialize<T>(T value, bool pretty = false) where T : struct {
			return Serialize(StructToJsonValue(value), pretty);
		}

		/// <summary>
		/// Attempts to automatically serialize a struct into a JsonValue, always check output. Will only store ValueType (and ValueType array) <c>Properties</c>.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		public static JsonValue StructToJsonValue<T>(T value) where T : struct {
			Type baseType = typeof(T);
			JsonObject container = new JsonObject();
			PropertyInfo[] properties = baseType.GetProperties();
			foreach (PropertyInfo pi in properties) {
				object getValue = pi.GetValue(value);
				if (getValue != null) {
					Type ValueType = getValue.GetType();
					if (ValueType.IsArray) {
						Type elementType = ValueType.GetElementType();

						JsonArray arrayContainer = new JsonArray();

						switch (Type.GetTypeCode(elementType)) {
							case TypeCode.Byte:
							case TypeCode.SByte:
							case TypeCode.UInt16:
							case TypeCode.UInt32:
							case TypeCode.UInt64:
							case TypeCode.Int16:
							case TypeCode.Int32:
							case TypeCode.Int64:
							case TypeCode.Decimal:
							case TypeCode.Double:
							case TypeCode.Single:
							case TypeCode.String:
							case TypeCode.Char:
							case TypeCode.DateTime:
							case TypeCode.Boolean:
							case TypeCode.Empty:
								foreach (object index in (Array)getValue) {
									arrayContainer.Add(index.ToString());
								}
								break;
							default:
								if (!elementType.IsValueType) {
									break;
								}
								foreach (object index in (Array)getValue) {
									if (index != null) {
										arrayContainer.Add((JsonValue)typeof(JsonWriter)
											.GetMethod(nameof(StructToJsonValue))
											.MakeGenericMethod(elementType)
											.Invoke(null, new object[] { index }));
									}
								}
								break;
						}
						container.Add(pi.Name, arrayContainer);
					}
					else if (ValueType == typeof(JsonValue)) {
						container.Add(pi.Name, (JsonValue)getValue);
					} 
					else if (ValueType == typeof(JsonObject)) {
						container.Add(pi.Name, (JsonObject)getValue);
					} 
					else if (ValueType == typeof(JsonObject)) {
						container.Add(pi.Name, (JsonArray)getValue);
					}
					else {
						switch (Type.GetTypeCode(ValueType)) {
							case TypeCode.Byte:
							case TypeCode.SByte:
							case TypeCode.UInt16:
							case TypeCode.UInt32:
							case TypeCode.UInt64:
							case TypeCode.Int16:
							case TypeCode.Int32:
							case TypeCode.Int64:
							case TypeCode.Decimal:
							case TypeCode.Double:
							case TypeCode.Single:
							case TypeCode.String:
							case TypeCode.Char:
							case TypeCode.DateTime:
							case TypeCode.Boolean:
							case TypeCode.Empty:
								container.Add(pi.Name, getValue.ToString());
								break;
							default:
								if (!ValueType.IsValueType && pi.GetSetMethod() == null) {
									break;
								}

								container.Add(pi.Name, (JsonValue)typeof(JsonWriter)
									.GetMethod(nameof(StructToJsonValue))
									.MakeGenericMethod(ValueType)
									.Invoke(null, new object[] { getValue }));
								break;
						}
					}
				}
			}
			return container;
		}


		/// <summary>
		/// Generates a string representation of the given value.
		/// </summary>
		/// <param name="value">The value to serialize.</param>
		/// <param name="pretty">Indicates whether the resulting string should be formatted for human-readability.</param>
		public static string Serialize(JsonValue value, bool pretty = false)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				JsonWriter jsonWriter = new JsonWriter(stringWriter, pretty);

				jsonWriter.Write(value);

				return stringWriter.ToString();
			}
		}
	}
}
