using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;

namespace Tasks
{
    public class Task : MonoBehaviour
    {
        private const string TASK_NAME_FORMAT = "Task - {0}";

        [SerializeField] private string _taskName = "DUMMY";
        private TaskBase[] _tasks;

        private TaskManager _context;
        private bool _hasError;

        private List<Coroutine> _coroutineTasks = new List<Coroutine>();


        private void Awake()
        {
            Reset();
        }

        private void OnValidate()
        {
            Reset();
        }

        private void Reset()
        {
            gameObject.name = string.Format(TASK_NAME_FORMAT, _taskName);
            _tasks = GetComponents<TaskBase>();
            _hasError = false;
        }

        /// <summary>
        /// Execute all the instances of ITask attached to this component as coroutines
        /// </summary>
        /// <returns></returns>
        public IEnumerator Execute(TaskManager context)
        {
            Reset();
            _context = context;
            gameObject.name = string.Format(TASK_NAME_FORMAT, _taskName) + " Running";

            _coroutineTasks = new List<Coroutine>();
            foreach (TaskBase task in _tasks)
            {
                if (!task.CanExecute())
                    continue;
                _coroutineTasks.Add(StartCoroutine(task.Execute()));
            }

            foreach (Coroutine coroutine in _coroutineTasks)
                yield return coroutine;

            gameObject.name = string.Format(TASK_NAME_FORMAT, _taskName);

            Debug.Log($"Finished {gameObject.name}" + (!_hasError ? string.Empty : " - ERROR"));
        }

        public void InterruptWithError(string error)
        {
            if (_context == null)
                return;
            _context.Interrupt(error);
        }
    }
}


public class LerpMoveTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        throw new System.NotImplementedException();
    }
}
