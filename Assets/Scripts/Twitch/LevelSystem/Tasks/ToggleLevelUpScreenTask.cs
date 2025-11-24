using System.Collections;
using Tasks;
using UnityEngine;

public class ToggleLevelUpScreenTask : TaskBase
{
    [SerializeField] private GameObject _levelUpScreen;
    [SerializeField] private bool _activeState;
    public override IEnumerator ExecuteInternal()
    {
        _levelUpScreen.gameObject.SetActive(_activeState);
        yield return null;
    }
}
