using Core;
using UnityEngine;

public class SettingsButton : BaseBehaviour
{
    public bool isCheckingKeyPress;

    private void Update()
    {
        if(!isCheckingKeyPress) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettings(true);
        }
    }

    public void OpenSettings(bool isByKey = false)
    {
        Publish(SettingsEventIds.ON_SETTINGS_SELECTED, isByKey);
    }
}
