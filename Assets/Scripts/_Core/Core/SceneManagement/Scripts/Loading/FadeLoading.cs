using DG.Tweening;
using UnityEngine.UI;

namespace SceneManagement.Loading
{
    public class FadeLoading : LoadingScreen
    {
        public Image fadeImage;

        public override void StartLoading()
        {
            base.StartLoading();
            fadeImage.DOFade(1, .25f);
        }

        public override void FinishLoading()
        {
            base.FinishLoading();
            fadeImage.DOFade(0, .25f);
        }
    }

}

