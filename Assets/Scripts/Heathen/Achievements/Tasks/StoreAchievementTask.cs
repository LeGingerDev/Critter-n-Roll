using System.Collections;
using Tasks;

public class StoreAchievementTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        AchievementTracker.Instance.Store();
        yield return null;
    }
}
