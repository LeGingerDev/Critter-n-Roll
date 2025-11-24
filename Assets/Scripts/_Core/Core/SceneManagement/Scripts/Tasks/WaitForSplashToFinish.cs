using SceneManagement;
using System.Collections;
using Tasks;
using UnityEngine;
namespace SceneManagerment.Tasks
{
    public class WaitForSplashToFinish : TaskBase
    {
        private bool _hasSplashScreenFinished;

        public override IEnumerator ExecuteInternal()
        {
            _hasSplashScreenFinished = false;
            SceneManager.OnSceneLoadingFinished += SceneManager_OnSceneLoadingFinished;
            yield return new WaitUntil(() => _hasSplashScreenFinished);
            SceneManager.OnSceneLoadingFinished -= SceneManager_OnSceneLoadingFinished;
        }

        private void SceneManager_OnSceneLoadingFinished()
        {
            _hasSplashScreenFinished = true;
        }
    }
}
