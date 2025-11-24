using Core;
using General.Utility;
using System.Collections.Generic;
using UnityEngine;

public class EndGate : BaseBehaviour
{
    [SerializeField]
    private GameObject _fence;
    [SerializeField]
    private GameObject _gate;

    [SerializeField]
    private SubscribablePhysics _exitTrigger;

    private List<PlayerController> _playersCaught = new List<PlayerController>();

    private void Awake()
    {
        _exitTrigger.OnTriggerEnterEvent += OnExitEntered;
    }

    private void Start()
    {
        TriggerExit(false);
    }

    private void OnDestroy()
    {
        _exitTrigger.OnTriggerEnterEvent -= OnExitEntered;
    }

    private void OnExitEntered(Collider obj)
    {
        if (!obj.TryGetComponent(out PlayerController player))
            return;

        if (HasPlayerEntered(player))
            return;
        Debug.Log("Player entered");
        _playersCaught.Add(player);
        player.PlayerData.AddGameFinished();
        player.gameObject.layer = LayerMask.NameToLayer("FinishedPlayer");
        Publish(GameLoopEventIds.ON_PLAYER_FINISHED, player);
    }

    public void TriggerExit(bool isTriggered)
    {
        _fence.SetActive(!isTriggered);
        _gate.SetActive(isTriggered);
    }

    public void TriggerBlock()
    {
        _fence.SetActive(true);
        _gate.SetActive(false);
        _playersCaught.Clear();
    }

    public bool HasPlayerEntered(PlayerController player) => _playersCaught.Contains(player);
}
