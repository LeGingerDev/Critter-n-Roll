using System.Collections;
using SceneManagement;
using UnityEngine;
using Utilities.PropertyAttributes;

namespace Tasks.Generic
{
    public class GoToSceneWithLoadingScreenTask : TaskBase, ITask
    {
        [SerializeField, SceneDropdown] private string _sceneName;
        [SerializeField] private float _splashScreenTime = 2f;

        public override IEnumerator ExecuteInternal()
        {
            yield return null;
            SceneManager.Instance.GoToLevel(_sceneName, _splashScreenTime);
        }
    }
}