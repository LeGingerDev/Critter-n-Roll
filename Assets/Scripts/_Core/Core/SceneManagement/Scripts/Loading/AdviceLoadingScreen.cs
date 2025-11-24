using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SceneManagement.Loading
{
    public class AdviceLoadingScreen : LoadingScreen
    {
        [SerializeField]
        private TextMeshProUGUI _messageText;
        [SerializeField]
        private CanvasGroup _fadeCanvas;
        private AdviceLoadingMessagesProvider _messagesProvider;

        private Tween _fadeTween;

        private void Start()
        {
            _fadeCanvas.interactable = false;
            _fadeCanvas.blocksRaycasts = false;
            _fadeCanvas.alpha = 0;
        }

        public override void StartLoading()
        {
            base.StartLoading();
            ToggleCanvas(true);
            _messagesProvider ??= FindFirstObjectByType<AdviceLoadingMessagesProvider>();
            _messageText.text = $"<bounce>{_messagesProvider.GetRandomMessage()}</bounce>";

        }
        public override void FinishLoading()
        {
            base.FinishLoading();
            ToggleCanvas(false);
        }
        public void ToggleCanvas(bool isActive)
        {
            // Kill any existing fade tween to prevent overlap
            _fadeTween?.Kill();

            float targetAlpha = isActive ? 1 : 0;

            // Fade in to alpha 1 or fade out to alpha 0 over 0.3 seconds with an easing function
            if (isActive)
            {
                _fadeTween = _fadeCanvas.DOFade(targetAlpha, 0.3f).SetEase(Ease.OutCubic).From(0).OnComplete(() =>
                {
                    // Disable the content after the fade-out completes
                    _fadeCanvas.blocksRaycasts = true;
                    _fadeCanvas.interactable = true;
                }); 
            }
            else
            {
                _fadeTween = _fadeCanvas.DOFade(targetAlpha, 0.3f)
                    .From(1)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        // Disable the content after the fade-out completes
                        _fadeCanvas.blocksRaycasts = false;
                        _fadeCanvas.interactable = false;
                    });
            }
        }

    }
}