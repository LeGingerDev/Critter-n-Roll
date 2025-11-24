using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{
    public class TaskManager : MonoBehaviour
    {
        public event Action<string> OnTaskError;
        public event Action OnTasksExecuted;

        [SerializeField] private UnityEvent OnTasksFailed;

        [SerializeField] private bool _executeOnAwake;
        [SerializeField] private Task[] _tasks;

        /// <summary>
        /// Exposes the list of tasks managed by this TaskManager.
        /// </summary>
        public Task[] Tasks => _tasks;

        // Tracks the currently running execution coroutine.
        private Coroutine _executionCoroutine;

        private void OnValidate()
        {
            _tasks = GetComponentsInChildren<Task>();
        }

        private void Awake()
        {
            if (_executeOnAwake)
                StartCoroutine(Execute());
        }

        /// <summary>
        /// Begins execution of all tasks in order.
        /// </summary>
        public IEnumerator Execute()
        {
            if (_executionCoroutine != null)
            {
                Debug.LogWarning($"Execution of {gameObject.name} has already started.");
                yield break;
            }

            _executionCoroutine = StartCoroutine(ExecuteInternal());
            yield return _executionCoroutine;

            // Clear reference after completion
            _executionCoroutine = null;
        }

        private IEnumerator ExecuteInternal()
        {
            // Give one frame before starting tasks
            yield return null;

            foreach (Task task in _tasks.OrderBy(t => t.transform.GetSiblingIndex()))
            {
                yield return task.Execute(this);
            }

            OnTasksExecuted?.Invoke();

            // Ensure coroutine reference is cleared
            _executionCoroutine = null;
        }

        /// <summary>
        /// Stops any ongoing execution and fires error events.
        /// </summary>
        /// <param name="reason">Reason for interruption.</param>
        public void Interrupt(string reason)
        {
            if (_executionCoroutine == null)
                return;

            Debug.Log($"Aborting tasks from {gameObject.name} due to:\n{reason}");
            StopCoroutine(_executionCoroutine);
            _executionCoroutine = null;

            OnTaskError?.Invoke(reason);
            OnTasksFailed?.Invoke();
        }

        /// <summary>
        /// Restarts the TaskManager from the first task.
        /// If tasks are currently executing, they will be stopped first.
        /// </summary>
        public void Restart()
        {
            if (_executionCoroutine != null)
            {
                StopCoroutine(_executionCoroutine);
                _executionCoroutine = null;
            }

            // Kick off a fresh execution
            StartCoroutine(Execute());
        }

        private void OnDestroy()
        {
            if (_executionCoroutine != null)
            {
                StopCoroutine(_executionCoroutine);
                _executionCoroutine = null;
            }
        }
    }
}
