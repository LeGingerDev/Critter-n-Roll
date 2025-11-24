using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image toggleImage;

    private Action OnToggleTrue;
    private Action OnToggleFalse;

    private bool _currentState;

    /// <summary>
    /// Initialises the toggle with the current state and the actions to perform on toggle.
    /// </summary>
    /// <param name="currentState">Useful for loading the state in before using it. Useful for setting display</param>
    /// <param name="onToggleTrue">What happens when the toggle is true</param>
    /// <param name="onToggleFalse">What happens when the toggle is false</param>
    /// <param name="isDirty">Setting this makes the toggle after initialising run the action commands. Not usually necessary</param>
    public void Initialise(bool currentState, Action onToggleTrue, Action onToggleFalse, bool isDirty = false)
    {
        OnToggleTrue = onToggleTrue;
        OnToggleFalse = onToggleFalse;
        _currentState = currentState;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ToggleState();
    }

    public void ToggleState()
    {
        _currentState = !_currentState;
        if (_currentState)
            OnToggleTrue?.Invoke();
        else
            OnToggleFalse?.Invoke();

        toggleImage.enabled = _currentState;
    }

    public void SetState(bool newState, bool isDirty)
    {
        _currentState = newState;
        if (isDirty)
        {
            if (_currentState)
                OnToggleTrue?.Invoke();
            else
                OnToggleFalse?.Invoke();
        }
    }
}

