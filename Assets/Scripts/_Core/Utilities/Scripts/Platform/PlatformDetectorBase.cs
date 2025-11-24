using Core;
using UnityEngine;

namespace Utilities.Platform
{
    public class PlatformDetectorBase : BaseBehaviour
    {
        protected virtual void OnUnityEditor()
        {
            Debug.Log("Unity Editor");
            // Override this to handle Unity Editor-specific behavior
        }

        protected virtual void OnMobile()
        {
            Debug.Log("Mobile");
            // Override this to handle mobile-specific behavior (Android and iOS)
        }

        protected virtual void OnWebGL()
        {
            Debug.Log("WebGL");
            // Override this to handle WebGL-specific behavior
        }

        protected virtual void OnOtherPlatforms()
        {
            Debug.Log("Other Platforms");
            // Override this to handle other platforms
        }

        private void Awake()
        {
            DetectPlatform();
        }

        private void DetectPlatform()
        {
#if UNITY_EDITOR
            OnUnityEditor();
#elif UNITY_ANDROID || UNITY_IOS
                OnMobile();
#elif UNITY_WEBGL
                OnWebGL();
#else
                OnOtherPlatforms();
#endif
        }
    }
}