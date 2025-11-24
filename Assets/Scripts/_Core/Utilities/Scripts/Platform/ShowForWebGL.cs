namespace Utilities.Platform
{
    public class ShowForWebGL : PlatformDetectorBase
    {
        protected override void OnMobile()
        {
            gameObject.SetActive(false);
            base.OnMobile();
        }

        protected override void OnUnityEditor()
        {
            gameObject.SetActive(false);
            base.OnUnityEditor();
        }

        protected override void OnWebGL()
        {
            base.OnWebGL();
        }

        protected override void OnOtherPlatforms()
        {
            gameObject.SetActive(false);
            base.OnOtherPlatforms();
        }
    }
}