using System;
using System.Collections.Generic;

namespace ScoredProductions.StreamLinked.Utility {

	/// <summary>
	/// Singleton base class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SingletonDispatcher<T> : SingletonInstance<T> where T : SingletonDispatcher<T> {

		/// <summary>
		/// Queue of Actions that will be executed on the main thread (<c>LateUpdate</c>)
		/// </summary>
		protected static readonly Queue<Action> MainThreadDispatchQueue = new Queue<Action>(0);

		public DebugManager.DebugLevel LogDebugLevel = DebugManager.DebugLevel.Normal;

		protected virtual void LateUpdate() {
			while (MainThreadDispatchQueue.TryDequeue(out Action work)) {
				try {
					work?.Invoke();
				} catch (Exception ex) {
					if (LogDebugLevel != DebugManager.DebugLevel.None) {
						DebugManager.LogMessage(ex);
					}
				}
			}
		}
	}
}