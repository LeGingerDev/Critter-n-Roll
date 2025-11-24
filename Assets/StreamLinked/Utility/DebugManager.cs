using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.Events;

namespace ScoredProductions.StreamLinked.Utility {

	/// <summary>
	/// Singleton object and static methods pertaining to storing and managing logging and debug messages.
	/// </summary>
	[DefaultExecutionOrder(-0x10)]
	public class DebugManager : SingletonInstance<DebugManager> {

		public override bool PersistBetweenScenes => true;

		// Small functionality to allow buttons to print a log
		// Assigned as a singleton because you dont need any more than this item...
		#region Instance_Functions

		public UnityEvent<string> OnDebugMessageReceived;

		// Main thread queue
		private readonly Queue<string> MessageQueue = new Queue<string>();

		protected override void Awake() {
			if (this.EstablishSingleton(true)) {
				OnDebugMessagePosted += this.GetMessage;
			}
		}

		private void FixedUpdate() {
			while (this.MessageQueue.TryDequeue(out string message)) {
				this.OnDebugMessageReceived?.Invoke(message);
			}
		}

		private void GetMessage(string s) {
			this.MessageQueue.Enqueue(s);
		}

#if UNITY_EDITOR
		public void PrintLogToFile() {
			Directory.CreateDirectory(GeneratedFilePath);
			File.WriteAllLines(GeneratedFilePath + @"\DebugLog.txt", PostedMessages);
		}
#endif

		#endregion


		[Serializable]
		public enum ErrorLevel : byte {
			Default = 0,
			Assertion = 1,
			Error = 2,
			Exception = 3,
			Warning = 4,
		}


		[Serializable]
		public enum DebugLevel : byte {
			None = 0,
			Necessary = 1,
			Normal = 2,
			Full = 3
		}

		public readonly static string GeneratedFilePath = Directory.GetCurrentDirectory() + @"\GeneratedFiles";

		public static Action<string> OnDebugMessagePosted;

		private readonly static LinkedList<string> _postedMessages = new LinkedList<string>();
		public static List<string> PostedMessages => new List<string>(_postedMessages);

		public static int MessageLimit { get; private set; } = 1000;

		/// <summary>
		/// <see langword="false"/> to keep a log of all messages to log, <see langword="true"/> to discard messages
		/// </summary>
		public static bool PostToDebugCallsDisabled = false;

		/// <summary>
		/// Logs message to DebugManager container, posts to Debug.Log% if in editor
		/// </summary>
		/// <param name="message">String message of log, some Unity Logs support RichText</param>
		/// <param name="state">Message type logged to the Unity Editor console, disabled outside of editor</param>
		public static void LogMessage(string message, ErrorLevel state = ErrorLevel.Default) {
			if (PostToDebugCallsDisabled) {
				return;
			}

			string debugmessage = $"{DateTime.Now:yyyy-MM-dd / HH-mm-ss-FFFFFFF} :: {message}";
#if UNITY_EDITOR
			switch (state) {
				case ErrorLevel.Assertion:
					Debug.LogAssertion(debugmessage);
					break;
				case ErrorLevel.Error:
					Debug.LogError(debugmessage);
					break;
				case ErrorLevel.Exception:
					Debug.LogException(new Exception(debugmessage));
					break;
				case ErrorLevel.Warning:
					Debug.LogWarning(debugmessage);
					break;
				default:
					Debug.Log(debugmessage);
					break;
			}
#endif
			_postedMessages.AddLast(debugmessage);
			while (_postedMessages.Count > MessageLimit) {
				_postedMessages.RemoveFirst();
			}
			OnDebugMessagePosted?.Invoke(debugmessage);
		}

		/// <summary>
		/// Logs exception to DebugManager container, posts to Debug.Log% if in editor
		/// </summary>
		/// <param name="exception">Thrown exception to log</param>
		/// <param name="state">Message type logged to the Unity Editor console, disabled outside of editor</param>
		public static void LogMessage(Exception exception, ErrorLevel state = ErrorLevel.Exception) {
			if (PostToDebugCallsDisabled) {
				return;
			}
			
			string debugmessage = $"{DateTime.Now:yyyy-MM-dd / HH-mm-ss} :: {exception.Message}";
#if UNITY_EDITOR
			switch (state) {
				case ErrorLevel.Assertion:
					Debug.LogAssertion(debugmessage);
					break;
				case ErrorLevel.Error:
					Debug.LogError(debugmessage);
					break;
				case ErrorLevel.Exception:
					Debug.LogException(exception);
					break;
				case ErrorLevel.Warning:
					Debug.LogWarning(debugmessage);
					break;
				default:
					Debug.Log(debugmessage);
					break;
			}
#endif
			_postedMessages.AddLast(debugmessage);
			while (_postedMessages.Count > MessageLimit) {
				_postedMessages.RemoveFirst();
			}
			OnDebugMessagePosted?.Invoke(debugmessage);
		}

		public static void ClearPostedMessages() {
			_postedMessages.Clear();
		}

		public static void ClearPostedMessages(out List<string> oldMessages) {
			oldMessages = new List<string>(PostedMessages);
			ClearPostedMessages();
		}

		public static void UpdateMessageLimit(int limit) {
			MessageLimit = limit;
			while (_postedMessages.Count > MessageLimit) {
				_postedMessages.RemoveFirst();
			}
		}

		public static void UpdateMessageLimit(int limit, out List<string> oldMessages) {
			oldMessages = new List<string>(PostedMessages);
			UpdateMessageLimit(limit);
		}
	}
}
