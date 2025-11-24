using System;
using System.Collections;
using Core.Singleton;
using UnityEngine;

namespace SceneManagement
{
    public class SceneManager : MonoSingleton<SceneManager>
    {
        public static event Action OnSceneLoadingStarted;
        public static event Action OnSceneLoadingFinished;

        private Coroutine loadSceneInternal;

        public void GoToLevel(string sceneName, float forceLoadTime = 2f, bool unloadPrevious = false)
        {
            if (loadSceneInternal != null)
                return;
            loadSceneInternal = StartCoroutine(SceneLoading(sceneName, forceLoadTime, unloadPrevious));
        }

        public IEnumerator SceneLoading(string sceneName, float forceLoadTime = 2f, bool unloadPrevious = false)
        {
            OnSceneLoadingStarted?.Invoke();

            yield return new WaitForSeconds(forceLoadTime / 2);
            Debug.Log($"Loading {sceneName}");
            AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            yield return load;

            if (unloadPrevious)
            {
                AsyncOperation unload =
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnityEngine.SceneManagement.SceneManager
                        .GetActiveScene().name);
                yield return unload;
            }

            yield return new WaitForSeconds(forceLoadTime / 2);
            OnSceneLoadingFinished?.Invoke();
            loadSceneInternal = null;
        }

        public void ReloadScene()
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GoToLevel(sceneName, unloadPrevious: true);
        }
    }
}