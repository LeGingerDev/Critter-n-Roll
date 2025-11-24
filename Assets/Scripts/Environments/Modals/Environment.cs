
using UnityEngine;

[CreateAssetMenu(fileName ="Environment", menuName ="LGD/Environments/Create Environment")]
public class Environment : ScriptableObject
{
    public string environmentName;
    public string environmentDescription;
    public Sprite environmentIcon;
    public EnvironmentController environmentPrefab;
}