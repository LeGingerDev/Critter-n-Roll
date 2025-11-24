using System.Collections;
using Tasks;

public class ClearPlayersTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        ActivePlayerManager.Instance.ClearAllActive();
        yield break;
    }
}
