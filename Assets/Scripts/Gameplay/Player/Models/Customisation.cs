using UnityEngine;

[CreateAssetMenu(fileName = "Customisation_", menuName = "LGD/Customisation/Create Customisation Option")]
public class Customisation : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject modelPrefab;
}
