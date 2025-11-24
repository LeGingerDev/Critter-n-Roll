using System;

using UnityEngine;

namespace ScoredProductions.StreamLinked.Utility {

	/// <summary>
	/// Singleton base class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DefaultExecutionOrder(-0x1)] //Default execution of all singletons is before gameobject default
	public abstract class SingletonInstance<T> : MonoBehaviour where T : SingletonInstance<T> {
		public static readonly string Name = typeof(T).Name.ToReadable();

		protected static T _instance;

		private static readonly object Locker = new object();

		protected static bool preventNewInstances;

		/// <summary>
		/// If the Singleton is running in the scene. Good to prevent non-playtime functionality running.
		/// </summary>
		public static bool InstanceIsAlive => _instance != null;

		public abstract bool PersistBetweenScenes { get; }

		/// <summary>
		/// Gets the Singleton if it exists in the scene.
		/// </summary>
		public static bool GetInstance(out T instance) {
			instance = _instance;
			return instance != null;
		}

		/// <summary>
		/// Gets the current instance, if a singleton doesnt exist, it will create one.
		/// </summary>
		public static bool CreateOrGetInstance(out T instance) {
			instance = null;
			if (preventNewInstances) {
				return false;
			}
			try {
				if (_instance == null) {
					_instance = (new GameObject(Name)).AddComponent<T>();
				}
				instance = _instance;
				return true;
			} catch (Exception ex) {
				DebugManager.LogMessage(ex);

				return false;
			}
		}

		/// <summary>
		/// <b>Must be called in Awake</b>, Ensures singleton is the only instance.
		/// </summary>
		protected virtual bool EstablishSingleton(bool updateName = false) {
			lock (Locker) {
				if (_instance != null && _instance != (T)this) {
					Destroy(this.gameObject);
					return false;
				}
				else {
					_instance = (T)this;
					if (updateName) {
						_instance.name = Name;
					}
					return true;
				}
			}
		}

		protected virtual void Awake() {
			this.EstablishSingleton(true);
		}

		protected virtual void Start() {
			if (Application.isPlaying && this.PersistBetweenScenes) {
				DontDestroyOnLoad(this.gameObject);
			}
		}

		protected virtual void OnApplicationQuit() {
			preventNewInstances = true;
		}

	}
}