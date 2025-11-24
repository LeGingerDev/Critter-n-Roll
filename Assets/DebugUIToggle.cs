using UnityEngine;

public class DebugUIToggle : MonoBehaviour
{
    private bool _toggleState = true;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            _toggleState = !_toggleState;
            ToggleUI();
        }
    }

    public void ToggleUI()
    {
        foreach (var item in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            item.enabled = _toggleState; 
        }
    }
}
