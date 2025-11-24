using Core.Events;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Utilities.Extensions;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Settings")]
    private int _fixedRefreshRate = -1; // You can set this to 120 if you want

    private void Awake()
    {
        SetFixedRefreshRate();
    }

    private void SetFixedRefreshRate()
    {
        Application.targetFrameRate = _fixedRefreshRate;
        QualitySettings.vSyncCount = 0;
    }

    [Topic(SettingsEventIds.ON_PAUSED_TOGGLED)]
    public void TogglePause(object sender, bool isPaused)
    {
        ObjectExtensions.FindObjectsOfInterface<IPausable>(true).ToList().ForEach(i =>
        {
            if (isPaused) i.OnPaused();
            else i.OnUnpaused();
        });
    }

}