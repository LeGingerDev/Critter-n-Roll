namespace Core.Utilities.Mechanics.Progressions
{
    public class SliderProgressVisualiser : ProgressVisualiserBase
    {
        protected override void UpdateVisual(float fillAmount)
        {
            // For slider progress, update the image fill amount.
            _progressImage.fillAmount = fillAmount;

            // Use _valueChange for additional logic if needed.
        }
    }
}
