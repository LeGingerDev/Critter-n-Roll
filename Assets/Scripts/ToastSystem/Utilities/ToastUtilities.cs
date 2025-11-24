public static class ToastUtilities
{
    public static void ShowInfoToast(string message, ToastLocation location = ToastLocation.Top)
    {
        ToastData data = new ToastData(message, ToastType.Info);
        ToastManager.Instance.SpawnToast(data, location);
    }

    public static void ShowSuccessToast(string message, ToastLocation location = ToastLocation.Top)
    {
        ToastData data = new ToastData(message, ToastType.Success);
        ToastManager.Instance.SpawnToast(data, location);
    }

    public static void ShowWarningToast(string message, ToastLocation location = ToastLocation.Top)
    {
        ToastData data = new ToastData(message, ToastType.Warning);
        ToastManager.Instance.SpawnToast(data, location);
    }

    public static void ShowErrorToast(string message, ToastLocation location = ToastLocation.Top)
    {
        ToastData data = new ToastData(message, ToastType.Error);
        ToastManager.Instance.SpawnToast(data, location);
    }
}