using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

// Generic base class for leaderboard display
public abstract class LeaderboardDisplay<T> : MonoBehaviour where T : class
{
    public abstract void InitializeEntry(T entry, int rank);


}