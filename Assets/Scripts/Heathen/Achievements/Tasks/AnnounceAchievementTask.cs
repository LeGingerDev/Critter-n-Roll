using System.Collections;
using Tasks;
using UnityEngine;
using Utilities.Attributes;

public class AnnounceAchievementTask : TaskBase
{
    [ConstDropdown(typeof(AchievementEventIds))]
    public string announcementEvent;
    public int amount;
    public override IEnumerator ExecuteInternal()
    {
        Publish(announcementEvent, amount);
        yield return null;
    }
}
