using System;
using System.Collections;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tasks
{
    [RequireComponent(typeof(Task))]
    public abstract class TaskBase : BaseBehaviour, ITask
    {
        public event Action<TaskBase> OnTaskStarted;

        [SerializeField] private bool _waitForCompletion = true;

        [FoldoutGroup("Announcement"), SerializeField]
        private bool _announceTask;

        [FoldoutGroup("Announcement"), SerializeField, ShowIf("@_announceTask")]
        private string _taskMessage;

        [SerializeField, ReadOnly] private bool _willExecute;

        private Task _parent;
        public string TaskMessage => _taskMessage;
        private bool _restartRequested = false;
        public IEnumerator Execute()
        {
            Started(this);  // Announce task start (called once per overall execution)
            do
            {
                _restartRequested = false;  // reset the flag at start of each attempt
                Coroutine internalCoroutine = StartCoroutine(ExecuteInternal());

                if (!_waitForCompletion)
                {
                    // If not waiting for completion, we fire-and-forget the task.
                    // In non-wait mode, restart logic is not handled by TaskManager.
                    yield break;
                }

                // Wait for the internal coroutine to finish
                yield return internalCoroutine;
                // Loop again if a restart was requested during ExecuteInternal
            }
            while (_restartRequested);
        }

        public abstract IEnumerator ExecuteInternal();
        /// <summary>Call this within ExecuteInternal to restart the task from the beginning.</summary>
        protected void RestartTask()
        {
            _restartRequested = true;
        }
        public virtual bool IsFinished() => true;

        public virtual bool CanExecute()
        {
            return true;
        }

        public void Started(TaskBase task)
        {
            OnTaskStarted?.Invoke(task);
        }

        private void OnValidate()
        {
            _willExecute = CanExecute();
            _parent ??= GetComponent<Task>();
        }

        protected void InterruptWithError(string error)
        {
            if (_parent == null)
                _parent = GetComponent<Task>();
            _parent.InterruptWithError(error);
        }
    }
}