using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

public class PlayerCustomisationController : MonoBehaviour, IPlayerView
{
    private PlayerUserData _playerUserData;
    private Customisation _currentCustomisation;
    [SerializeField]
    private Transform _customisationParent;
    private GameObject _currentCustomisationModel;

    [Button]
    public void SetCustomisationOption(Customisation customisation)
    {
        _currentCustomisation = customisation;
        RemoveCurrent();
        CreateNew(customisation);
        GetComponentsInChildren<ICustomisationUpdater>(true).ToList().ForEach(i => i.OnCustomisationUpdated(customisation));
        _playerUserData.SetCustomisationOption(customisation.id);
        PlayerDataManager.Instance.SaveData();
    }

    public void RemoveCurrent()
    {
        Destroy(_currentCustomisationModel);
        _currentCustomisation = null;
    }

    public void CreateNew(Customisation customisation)
    {
        GameObject next = customisation.modelPrefab;
        GameObject spawned = Instantiate(next, transform.position, Quaternion.identity, _customisationParent);

        foreach (Transform playerObject in spawned.GetComponentsInChildren<Transform>())
        {
            playerObject.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        // Set to "Player" layer
        spawned.transform.localRotation = Quaternion.Euler(0, 180, 0); // Adjust rotation if needed
        _currentCustomisationModel = spawned;
    }

    public void Initialize(PlayerUserData username)
    {
        _playerUserData = username;
        if (string.IsNullOrEmpty(username.GetCustomisationId()))
        {
            SetCustomisationOption(CustomisationManager.Instance.GetDefault());
            return;
        }

        Customisation customisation = CustomisationManager.Instance.GetCustomisationById(username.GetCustomisationId());
        SetCustomisationOption(customisation);
    }
}
