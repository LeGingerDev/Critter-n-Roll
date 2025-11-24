namespace Core.Utilities.Mechanics.Progressions
{
    public class RadialProgressVisualiser : ProgressVisualiserBase
    {
        protected override void UpdateVisual(float fillAmount)
        {
            // For radial progress, set the fill amount directly.
            _progressImage.fillAmount = fillAmount;

            // You can now also access _valueChange here if needed.
            // For example: Debug.Log($"Current value: {_valueChange.value}");
        }
    }
}
