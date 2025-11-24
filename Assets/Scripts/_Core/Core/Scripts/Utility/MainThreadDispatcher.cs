using System;
using System.Collections.Concurrent;
using Core.Singleton;
using UnityEngine;

namespace Core.Utility
{
    [ExecuteAlways]
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
    {
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        public static void Enqueue(Action action)
        {
            Instance._actions.Enqueue(action);
        }

        private void Update()
        {
            while (_actions.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }
    }
}