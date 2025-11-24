using JetBrains.Annotations;
using System.Collections;
using Tasks;
using UnityEngine;

public class SkinsSelectionTask : TaskBase
{
    [SerializeField]
    private int _numberOfChoices = 3;
    [SerializeField]
    private float _decisionTime = 30f;

    private CustomisationWindow _customisationWindow;

    private void Awake()
    {
        _customisationWindow = FindFirstObjectByType<CustomisationWindow>(FindObjectsInactive.Include);
    }

    public override IEnumerator ExecuteInternal()
    {
        if (!CustomisationManager.Instance.GetCustomisationFlag())
            yield break;

        CustomisationManager.Instance.SetCustomisationFlag(false);

        ToggleSideBarTask sideBarTask = FindFirstObjectByType<ToggleSideBarTask>();
        sideBarTask.ToggleSideBar(false);

        _customisationWindow.gameObject.SetActive(true);
        _customisationWindow.SetSelectionChoices(_numberOfChoices); // Set number of choices to 3
        yield return _customisationWindow.OnOpen(_decisionTime, true);
        _customisationWindow.OnClose();
        yield return null;
    }
}
