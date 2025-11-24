using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement.Loading
{
    public class ZoomLoading : LoadingScreen
    {
        [SerializeField]
        private Image _zoomImage;
        [SerializeField]
        private float _zoomDuration = 0.5f;

        [SerializeField]
        private Ease _inEase;
        [SerializeField]
        private Ease _outEase;

        public override void StartLoading()
        {
            base.StartLoading();
            _zoomImage.transform.DOScale(Vector3.one * 6f, _zoomDuration).From(Vector3.zero).SetEase(_inEase);
        }

        public override void FinishLoading()
        {
            base.FinishLoading();
            _zoomImage.transform.DOScale(Vector3.zero, _zoomDuration).SetEase(_outEase);
        }
    }

}

