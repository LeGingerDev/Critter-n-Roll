using System.Collections.Generic;
using System.Linq;
using SceneManagerment.Splashscreen;
using UnityEngine;

namespace Tasks
{
    [RequireComponent(typeof(TaskManager))]
    public class TaskAnnouncer : MonoBehaviour
    {
        private List<TaskBase> tasks = new List<TaskBase>();

        private void Awake()
        {
            tasks = GetComponentsInChildren<TaskBase>().ToList();

            tasks.ForEach(task => { task.OnTaskStarted += OnTaskStarted; });
        }

        private void OnTaskStarted(TaskBase task)
        {
            LoadingBar loadingBar = FindFirstObjectByType<LoadingBar>(FindObjectsInactive.Include);
            if (!loadingBar)
                return;

            loadingBar.Initialise(tasks.Count);
            loadingBar.SetStatus(task.TaskMessage);
        }

        private void OnDestroy()
        {
            tasks.ForEach(task => { task.OnTaskStarted -= OnTaskStarted; });
        }
    }
}