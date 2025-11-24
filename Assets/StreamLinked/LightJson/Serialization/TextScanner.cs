using System;
using System.IO;

namespace ScoredProductions.StreamLinked.LightJson.Serialization {
	using ErrorType = JsonParseException.ErrorType;

	/// <summary>
	/// Represents a text scanner that reads one character at a time.
	/// </summary>
	public sealed class TextScanner
	{
		private TextReader reader;
		private TextPosition position;

		/// <summary>
		/// Gets the position of the scanner within the text.
		/// </summary>
		public TextPosition Position => this.position;

		/// <summary>
		/// Gets a value indicating whether there are still characters to be read.
		/// </summary>
		public bool CanRead => this.reader.Peek() != -1;

		/// <summary>
		/// Initializes a new instance of TextScanner.
		/// </summary>
		/// <param name="reader">The TextReader to read the text.</param>
		public TextScanner(TextReader reader)
		{
			this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
		}

		/// <summary>
		/// Reads the next character in the stream without changing the current position.
		/// </summary>
		public char Peek()
		{
			int next = this.reader.Peek();

			if (next == -1)
			{
				throw new JsonParseException(
					ErrorType.IncompleteMessage,
					this.position
				);
			}

			return (char)next;
		}

		/// <summary>
		/// Reads the next character in the stream, advancing the text position.
		/// </summary>
		public char Read()
		{
			int next = this.reader.Read();

			if (next == -1)
			{
				throw new JsonParseException(
					ErrorType.IncompleteMessage,
					this.position
				);
			}

			switch (next)
			{
				case '\r':
					// Normalize '\r\n' line encoding to '\n'.
					if (this.reader.Peek() == '\n')
					{
						this.reader.Read();
					}
					goto case '\n';
				case '\n':
					this.position.line += 1;
					this.position.column = 0;
					return '\n';
				default:
					this.position.column += 1;
					return (char)next;
			}
		}

		/// <summary>
		/// Advances the scanner to next non-whitespace character.
		/// </summary>
		public void SkipWhitespace()
		{
			while (char.IsWhiteSpace(this.Peek()))
			{
				this.Read();
			}
		}

		/// <summary>
		/// Verifies that the given character matches the next character in the stream.
		/// If the characters do not match, an exception will be thrown.
		/// </summary>
		/// <param name="next">The expected character.</param>
		public void Assert(char next)
		{
			if (this.Peek() == next)
			{
				this.Read();
			}
			else
			{
				throw new JsonParseException(
					string.Format("Parser expected '{0}'", next),
					ErrorType.InvalidOrUnexpectedCharacter,
					this.position
				);
			}
		}

		/// <summary>
		/// Verifies that the given string matches the next characters in the stream.
		/// If the strings do not match, an exception will be thrown.
		/// </summary>
		/// <param name="next">The expected string.</param>
		public void Assert(string next)
		{
			try
			{
				for (int i = 0; i < next.Length; i += 1)
				{
					this.Assert(next[i]);
				}
			}
			catch (JsonParseException e) when (e.Type == ErrorType.InvalidOrUnexpectedCharacter)
			{
				throw new JsonParseException(
					string.Format("Parser expected '{0}'", next),
					ErrorType.InvalidOrUnexpectedCharacter,
					this.position
				);
			}
		}
	}
}
