using Core;

namespace SceneManagement.Loading
{
    public class LoadingScreen : BaseBehaviour
    {
        public virtual void StartLoading()
        {
            print("Start Loading");
        }

        public virtual void FinishLoading()
        {
            print("Finish Loading");
        }
    }
}