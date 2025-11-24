using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Platform
{
    [RequireComponent(typeof(RectTransform))]
    public class PlatformUIMover : PlatformDetectorBase
    {
        [FoldoutGroup("Mobile Settings")] public Vector2 _mobilePosition;
        [FoldoutGroup("Mobile Settings")] public Vector2 _mobileSizeDelta;

        [FoldoutGroup("WebGL Settings")] public Vector2 _webGLPosition;
        [FoldoutGroup("WebGL Settings")] public Vector2 _webGLSizeDelta;

        [FoldoutGroup("Editor Settings")] public Vector2 _editorPosition;
        [FoldoutGroup("Editor Settings")] public Vector2 _editorSizeDelta;

        [FoldoutGroup("Other Settings")] public Vector2 _otherPosition;
        [FoldoutGroup("Other Settings")] public Vector2 _otherSizeDelta;

        private RectTransform _rectTransform;

        [Button("Set Mobile")]
        private void SetMobile()
        {
            _rectTransform.anchoredPosition = _mobilePosition;
            _rectTransform.sizeDelta = _mobileSizeDelta;
        }

        [Button("Show Mobile")]
        private void ShowMobile()
        {
            _mobilePosition = _rectTransform.anchoredPosition;
            _mobileSizeDelta = _rectTransform.sizeDelta;
        }

        [Button("Set WebGL")]
        private void SetWebGL()
        {
            _rectTransform.anchoredPosition = _webGLPosition;
            _rectTransform.sizeDelta = _webGLSizeDelta;
        }

        [Button("Show WebGL")]
        private void ShowWebGL()
        {
            _webGLPosition = _rectTransform.anchoredPosition;
            _webGLSizeDelta = _rectTransform.sizeDelta;
        }

        [Button("Set Editor")]
        private void SetEditor()
        {
            _rectTransform.anchoredPosition = _editorPosition;
            _rectTransform.sizeDelta = _editorSizeDelta;
        }

        [Button("Show Editor")]
        private void ShowEditor()
        {
            _editorPosition = _rectTransform.anchoredPosition;
            _editorSizeDelta = _rectTransform.sizeDelta;
        }

        [Button("Set Other")]
        private void SetOther()
        {
            _rectTransform.anchoredPosition = _otherPosition;
            _rectTransform.sizeDelta = _otherSizeDelta;
        }

        [Button("Show Other")]
        private void ShowOther()
        {
            _otherPosition = _rectTransform.anchoredPosition;
            _otherSizeDelta = _rectTransform.sizeDelta;
        }

        private void OnValidate()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
        }

        protected override void OnMobile()
        {
            SetMobile();
        }

        protected override void OnWebGL()
        {
            SetWebGL();
        }

        protected override void OnUnityEditor()
        {
            SetEditor();
        }

        protected override void OnOtherPlatforms()
        {
            SetOther();
        }
    }
}