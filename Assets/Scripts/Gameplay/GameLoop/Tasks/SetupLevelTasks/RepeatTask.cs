using System.Collections;
using Tasks;

public class RepeatTask : TaskBase
{


    public override IEnumerator ExecuteInternal()
    {
        TaskManager tm = GetComponentInParent<TaskManager>();
        tm.Restart();
        yield break;
    }
}
