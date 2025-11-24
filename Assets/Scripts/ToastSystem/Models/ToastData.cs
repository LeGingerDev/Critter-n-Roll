using System;
using UnityEngine;

[Serializable]
public class ToastData
{
    public string message;
    public ToastType type;
    public Sprite icon;

    public ToastData(string message, ToastType type, Sprite icon = null)
    {
        this.message = message;
        this.type = type;
        this.icon = icon;
    }

    public ToastData() { }
}