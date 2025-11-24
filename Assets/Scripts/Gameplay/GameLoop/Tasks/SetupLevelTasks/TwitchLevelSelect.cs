using ES3Types;
using System.Collections;
using Tasks;
using UnityEngine;

public class TwitchLevelSelect : TaskBase
{
    [SerializeField]
    private LevelSelectionWindow _levelSelectionWindow;
    [SerializeField]
    private float _decisionTime = 30f;
    public override IEnumerator ExecuteInternal()
    {
        _levelSelectionWindow.gameObject.SetActive(true);
        yield return _levelSelectionWindow.OnOpen(_decisionTime);
        Level levelSelected = _levelSelectionWindow.GetHighestSelectionData();
        LevelManager.Instance.SelectLevel(levelSelected);
        _levelSelectionWindow.OnClose();
    }
}

