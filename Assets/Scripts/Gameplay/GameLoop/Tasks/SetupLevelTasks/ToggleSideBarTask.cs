using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using Tasks;
using UnityEngine;

public class ToggleSideBarTask : TaskBase
{
    [SerializeField]
    private RectTransform _sideBar;
    [SerializeField]
    private bool _desiredState;
    [SerializeField]
    private float _duration = 0.5f;
    public override IEnumerator ExecuteInternal()
    {
        ToggleSideBar(_desiredState);
        yield return null;
    }
    [Button]
    public void ToggleSideBar(bool isActive)
    {
        float targetX = isActive ? 40f : -700f;
        _sideBar.DOAnchorPosX(targetX, _duration).SetEase(Ease.OutBack);
    }
}