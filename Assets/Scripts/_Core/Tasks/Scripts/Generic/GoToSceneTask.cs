using System.Collections;
using SceneManagement;
using Sirenix.OdinInspector;
using Tasks;
using UnityEngine;
using Utilities.PropertyAttributes;

public class GoToSceneTask : TaskBase, ITask
{
    [SerializeField, SceneDropdown] private string _sceneName;

    [SerializeField] private bool _forceWait;

    [SerializeField, ShowIf("@_forceWait")]
    private float _forceDuration;


    public override IEnumerator ExecuteInternal()
    {
        yield return null;
        if (_forceWait)
            yield return new WaitForSeconds(_forceDuration);
        SceneManager.Instance.GoToLevel(_sceneName);
    }
}