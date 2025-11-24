using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu.Components.Masterminds
{
    public class HoverClickScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [FoldoutGroup("References"), SerializeField]
        private Transform _hoverTarget;

        [FoldoutGroup("References"), SerializeField]
        private Transform _clickTarget;

        [FoldoutGroup("Effect"), SerializeField]
        private float normalScale = 1.0f;

        [FoldoutGroup("Effect"), SerializeField]
        private float hoverScale = 1.1f;

        [FoldoutGroup("Effect"), SerializeField]
        private float clickScale = 0.9f;

        private Tween _scaleTween;
        private Tween _clickTween;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hoverTarget == null)
                return;

            _scaleTween?.Kill();
            _scaleTween = _hoverTarget.DOScale(hoverScale, 0.25f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hoverTarget == null)
                return;
            _scaleTween?.Kill();
            _scaleTween = _hoverTarget.DOScale(normalScale, 0.25f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_clickTarget == null)
                return;
            _clickTween?.Kill();
            _clickTween = _clickTarget.DOScale(clickScale, 0.1f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_clickTarget == null)
                return;
            _clickTween?.Kill();
            _clickTween = _clickTarget.DOScale(normalScale, 0.1f);
        }
    }
}