using DG.Tweening;
using UnityEngine;

namespace Core.Utilities.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static void FadeTo(this CanvasGroup canvasGroup, bool isActive, float duration = 0.25f, bool forceInstant = false, System.Action onComplete = null)
        {
            float targetAlpha = isActive ? 1f : 0f;
            float fadeDuration = forceInstant ? 0f : duration;

            canvasGroup.DOFade(targetAlpha, fadeDuration).OnComplete(() =>
            {
                canvasGroup.interactable = isActive;
                canvasGroup.blocksRaycasts = isActive;
                onComplete?.Invoke();
            });
        }
    }
}