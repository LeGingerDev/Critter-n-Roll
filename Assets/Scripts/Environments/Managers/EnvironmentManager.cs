using Core.Singleton;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoSingleton<EnvironmentManager>
{
    [SerializeField]
    private List<Environment> _environments = new List<Environment>();

    
}