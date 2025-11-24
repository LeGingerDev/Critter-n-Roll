using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace ScoredProductions.StreamLinked.LightJson.Serialization
{
	using ErrorType = JsonParseException.ErrorType;

	/// <summary>
	/// Represents a reader that can read JsonValues.
	/// </summary>
	public struct JsonReader
	{
		private readonly TextScanner scanner;

		private JsonReader(TextReader reader)
		{
			this.scanner = new TextScanner(reader);
		}

		private readonly string ReadJsonKey()
		{
			return this.ReadString();
		}

		private JsonValue ReadJsonValue()
		{
			this.scanner.SkipWhitespace();

			char next = this.scanner.Peek();

			if (char.IsNumber(next))
			{
				return this.ReadNumber();
			}

			switch (next)
			{
				case '{':
					return this.ReadObject();
				case '[':
					return this.ReadArray();
				case '"':
					return this.ReadString();
				case '-':
					return this.ReadNumber();
				case 't':
				case 'f':
					return this.ReadBoolean();
				case 'n':
					return this.ReadNull();
				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this.scanner.Position
					);
			}
		}

		private readonly JsonValue ReadNull()
		{
			this.scanner.Assert("null");
			return JsonValue.Null;
		}

		private readonly JsonValue ReadBoolean()
		{
			switch (this.scanner.Peek())
			{
				case 't':
					this.scanner.Assert("true");
					return true;
				case 'f':
					this.scanner.Assert("false");
					return false;
				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this.scanner.Position
					);
			}
		}

		private readonly void ReadDigits(StringBuilder builder)
		{
			while (this.scanner.CanRead && char.IsDigit(this.scanner.Peek()))
			{
				builder.Append(this.scanner.Read());
			}
		}

		private readonly JsonValue ReadNumber()
		{
			StringBuilder builder = new StringBuilder();

			if (this.scanner.Peek() == '-')
			{
				builder.Append(this.scanner.Read());
			}

			if (this.scanner.Peek() == '0')
			{
				builder.Append(this.scanner.Read());
			}
			else
			{
				this.ReadDigits(builder);
			}

			if (this.scanner.CanRead && this.scanner.Peek() == '.')
			{
				builder.Append(this.scanner.Read());
				this.ReadDigits(builder);
			}

			if (this.scanner.CanRead && char.ToLowerInvariant(this.scanner.Peek()) == 'e')
			{
				builder.Append(this.scanner.Read());

				char next = this.scanner.Peek();

				switch (next)
				{
					case '+':
					case '-':
						builder.Append(this.scanner.Read());
						break;
				}

				this.ReadDigits(builder);
			}

			return double.Parse(
				builder.ToString(),
				CultureInfo.InvariantCulture
			);
		}

		private readonly string ReadString()
		{
			StringBuilder builder = new StringBuilder();

			this.scanner.Assert('"');

			while (true)
			{
				char c = this.scanner.Read();

				if (c == '\\')
				{
					c = this.scanner.Read();

					switch (char.ToLower(c))
					{
						case '"':  // "
						case '\\': // \
						case '/':  // /
							builder.Append(c);
							break;
						case 'b':
							builder.Append('\b');
							break;
						case 'f':
							builder.Append('\f');
							break;
						case 'n':
							builder.Append('\n');
							break;
						case 'r':
							builder.Append('\r');
							break;
						case 't':
							builder.Append('\t');
							break;
						case 'u':
							builder.Append(this.ReadUnicodeLiteral());
							break;
						default:
							throw new JsonParseException(
								ErrorType.InvalidOrUnexpectedCharacter,
								this.scanner.Position
							);
					}
				}
				else if (c == '"')
				{
					break;
				}
				else
				{
                    /*
                     * According to the spec:
                     * 
                     * unescaped = %x20-21 / %x23-5B / %x5D-10FFFF
                     * 
                     * i.e. c cannot be < 0x20, be 0x22 (a double quote) or a 
                     * backslash (0x5C).
                     * 
                     * c cannot be a back slash or double quote as the above 
                     * would have hit. So just check for < 0x20.
                     * 
                     * > 0x10FFFF is unnecessary *I think* because it's obviously
                     * out of the range of a character but we might need to look ahead
                     * to get the whole utf-16 codepoint
                     */
                    if (c < '\u0020')
                    {
                        throw new JsonParseException(
                            ErrorType.InvalidOrUnexpectedCharacter,
                            this.scanner.Position
                        );
                    }
                    else
					{
						builder.Append(c);
					}
				}
			}

			return builder.ToString();
		}

		private readonly int ReadHexDigit()
		{
			switch (char.ToUpper(this.scanner.Read()))
			{
				case '0':
					return 0;
				case '1':
					return 1;
				case '2':
					return 2;
				case '3':
					return 3;
				case '4':
					return 4;
				case '5':
					return 5;
				case '6':
					return 6;
				case '7':
					return 7;
				case '8':
					return 8;
				case '9':
					return 9;
				case 'A':
					return 10;
				case 'B':
					return 11;
				case 'C':
					return 12;
				case 'D':
					return 13;
				case 'E':
					return 14;
				case 'F':
					return 15;
				default:
					throw new JsonParseException(
						ErrorType.InvalidOrUnexpectedCharacter,
						this.scanner.Position
					);
			}
		}

		private readonly char ReadUnicodeLiteral()
		{
			int value = 0;

			value += this.ReadHexDigit() * 4096; // 16^3
			value += this.ReadHexDigit() * 256;  // 16^2
			value += this.ReadHexDigit() * 16;   // 16^1
			value += this.ReadHexDigit();        // 16^0

			return (char)value;
		}

		private JsonObject ReadObject() => this.ReadObject(new JsonObject());

		private JsonObject ReadObject(JsonObject jsonObject)
		{
			this.scanner.Assert('{');

			this.scanner.SkipWhitespace();

			if (this.scanner.Peek() == '}')
			{
				this.scanner.Read();
			}
			else
			{
				while (true)
				{
					this.scanner.SkipWhitespace();

					string key = this.ReadJsonKey();

					if (jsonObject.ContainsKey(key))
					{
						throw new JsonParseException(
							ErrorType.DuplicateObjectKeys,
							this.scanner.Position
						);
					}

					this.scanner.SkipWhitespace();

					this.scanner.Assert(':');

					this.scanner.SkipWhitespace();

					JsonValue value = this.ReadJsonValue();

					jsonObject.Add(key, value);

					this.scanner.SkipWhitespace();

					char next = this.scanner.Read();

					if (next == '}')
					{
						break;
					}
					else if (next == ',')
					{
						continue;
					}
					else
					{
						throw new JsonParseException(
							ErrorType.InvalidOrUnexpectedCharacter,
							this.scanner.Position
						);
					}
				}
			}

			return jsonObject;
		}

		private JsonArray ReadArray() => this.ReadArray(new JsonArray());

		private JsonArray ReadArray(JsonArray jsonArray)
		{
			this.scanner.Assert('[');

			this.scanner.SkipWhitespace();

			if (this.scanner.Peek() == ']')
			{
				this.scanner.Read();
			}
			else
			{
				while (true)
				{
					this.scanner.SkipWhitespace();

					JsonValue value = this.ReadJsonValue();

					jsonArray.Add(value);

					this.scanner.SkipWhitespace();

					char next = this.scanner.Read();

					if (next == ']')
					{
						break;
					}
					else if (next == ',')
					{
						continue;
					}
					else
					{
						throw new JsonParseException(
							ErrorType.InvalidOrUnexpectedCharacter,
							this.scanner.Position
						);
					}
				}
			}

			return jsonArray;
		}

		private JsonValue Parse()
		{
			this.scanner.SkipWhitespace();
			return this.ReadJsonValue();
		}

		/// <summary>
		/// Creates a JsonValue by using the given TextReader.
		/// </summary>
		/// <param name="reader">The TextReader used to read a JSON message.</param>
		public static JsonValue Parse(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}

			return new JsonReader(reader).Parse();
		}

		/// <summary>
		/// Creates a JsonValue by reader the JSON message in the given string.
		/// </summary>
		/// <param name="source">The string containing the JSON message.</param>
		public static JsonValue Parse(string source)
		{
			if (string.IsNullOrWhiteSpace(source))
			{
				return new JsonValue();
			}

			using (StringReader reader = new StringReader(source))
			{
				JsonReader jReader = new JsonReader(reader);
				return jReader.Parse();
			}
		}

		/// <summary>
		/// Creates a JsonValue by reading the given file.
		/// </summary>
		/// <param name="path">The file path to be read.</param>
		public static JsonValue ParseFile(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			// NOTE: FileAccess.Read is needed to be able to open read-only files
			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			using (StreamReader reader = new StreamReader(stream))
			{
				return new JsonReader(reader).Parse();
			}
		}
	}
}
