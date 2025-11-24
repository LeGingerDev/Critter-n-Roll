using Core.Singleton;
using Sirenix.OdinInspector;

namespace SceneManagement.Loading
{
    public class LoadingScreenController : MonoSingleton<LoadingScreenController>
    {
        LoadingScreen loadingScreen;

        protected override void Awake()
        {
            base.Awake();
            loadingScreen = GetComponentInChildren<LoadingScreen>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneManager.OnSceneLoadingStarted += Show;
            SceneManager.OnSceneLoadingFinished += Hide;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SceneManager.OnSceneLoadingStarted -= Show;
            SceneManager.OnSceneLoadingFinished -= Hide;
        }

        [Button]
        public void Show()
        {
            loadingScreen.StartLoading();
        }

        [Button]
        public void Hide()
        {
            loadingScreen.FinishLoading();
        }
    }
}