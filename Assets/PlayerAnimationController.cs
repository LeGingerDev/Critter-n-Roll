using Core;
using ScoredProductions.StreamLinked.IRC;
using UnityEngine;

public class PlayerAnimationController : PlayerListenerBase, ICustomisationUpdater
{
    private Animator _animator;
    private PlayerMovementController _movementController;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _movementController = GetComponent<PlayerMovementController>();

        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void Subscribe()
    {
        _movementController.OnMovementStarted += OnMovementStarted;
        _movementController.OnMovementStopped += OnMovementStopped;
    }

    public void Unsubscribe()
    {
        _movementController.OnMovementStarted -= OnMovementStarted;
        _movementController.OnMovementStopped -= OnMovementStopped;
    }

    private void OnMovementStarted()
    {
        SetAnimation("Roll");
    }
    private void OnMovementStopped()
    {
        SetAnimation("Idle_A");
    }
    public void SetAnimation(string animationName)
    {
        if(_animator == null)
            _animator = GetComponentInChildren<Animator>();
        _animator?.Play(animationName);
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        switch(commandKey)
        {
            case "spin":
                SetAnimation("Spin");
                break;
        }
    }

    public void OnCustomisationUpdated(Customisation customisation)
    {
        _animator = GetComponentInChildren<Animator>();
        OnMovementStopped();
    }
}
