using System.Collections;
using Tasks;
using UnityEngine;

public class WaitForTimeTask : TaskBase
{
    [SerializeField]
    private float _timeToWait = 1f;
    public override IEnumerator ExecuteInternal()
    {
        yield return new WaitForSeconds(_timeToWait);
    }
}
