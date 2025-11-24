using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpScreen : MonoBehaviour
{
    [SerializeField] private PlayerLevelUpDisplayBox _playerLevelUpPrefab;
    [SerializeField] private Transform _levelUpContainer;
    [SerializeField] private List<PlayerLevelUpDisplayBox> _playerDisplayBoxes = new List<PlayerLevelUpDisplayBox>();
    
    public void OnEnable()
    {
        CreateLevelDisplays();
    }

    public void OnDisable()
    {
        Cleanup();
    }

    public void CreateLevelDisplays()
    {
        List<PlayerUserData> activePlayers = ActivePlayerManager.Instance.GetActivePlayers().ToList();

        foreach (var player in activePlayers)
        {
            PlayerLevelUpDisplayBox display = Instantiate(_playerLevelUpPrefab, _levelUpContainer);
            display.Initialise(player);
            _playerDisplayBoxes.Add(display);
        }
    }

    public void Cleanup()
    {
        _playerDisplayBoxes.ForEach(box => Destroy(box.gameObject));
        _playerDisplayBoxes.Clear();
    }
}
