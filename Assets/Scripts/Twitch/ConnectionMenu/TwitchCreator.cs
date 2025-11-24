using ScoredProductions.StreamLinked.API;
using UnityEngine;

public class TwitchCreator : MonoBehaviour
{
    public GameObject _globalObjects;

    public Transform _twitchPanel;

    private void Start()
    {
        _twitchPanel.gameObject.SetActive(false);
    }

    public void ActivateTwitch()
    {
        if(FindFirstObjectByType<TwitchAPIClient>() == null)
            Instantiate(_globalObjects, null);

        TogglePanel();
    }

    public void TogglePanel() => _twitchPanel.gameObject.SetActive(!_twitchPanel.gameObject.activeSelf);

}
