using System.Collections;
using Tasks;

public class SavePlayersTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        PlayerDataManager.Instance.SaveData();
        yield return null;
    }
}
