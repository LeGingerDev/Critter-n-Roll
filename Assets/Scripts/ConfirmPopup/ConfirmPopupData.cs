using System;

public class ConfirmPopupData
{
    public Action onConfirm;
    public Action onCancel;
    public string message;
    public string title;
    public string confirmButtonText;
    public string cancelButtonText;
    public ConfirmPopup.ConfirmType confirmType;
}
