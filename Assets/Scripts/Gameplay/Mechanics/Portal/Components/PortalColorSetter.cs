using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalColorSetter : MonoBehaviour
{
    [Button]
    public void SetColors(Color color)
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }
}