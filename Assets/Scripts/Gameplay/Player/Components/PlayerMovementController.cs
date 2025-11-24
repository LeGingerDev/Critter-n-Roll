using Audio.Core;
using Audio.Managers;
using DG.Tweening;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : PlayerListenerBase
{
    public enum MovementState
    {
        Idle,
        Moving
    }

    // These events fire once per state transition
    public event Action OnMovementStarted;
    public event Action OnMovementStopped;

    private Rigidbody _rb;
    private MovementState _currentState = MovementState.Idle;

    // Threshold under which we consider the player to have 'stopped'
    [SerializeField, Tooltip("Below this speed, the player is considered stopped.")]
    private float _stopThreshold = 0.1f;

    private Coroutine _waitForStopCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // At start, assume we're idle (so fire stopped event once if anything is listening)
        OnMovementStopped?.Invoke();
    }

    private void FixedUpdate()
    {
        // Rotate the transform to face direction of movement (if moving)
        OrientTowardsMovement();
    }

    private void OrientTowardsMovement()
    {
        // Only orient if current horizontal speed is above the threshold
        Vector3 horizontalVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (horizontalVel.magnitude > _stopThreshold)
        {
            // Compute desired rotation based on velocity direction
            Quaternion targetRot = Quaternion.LookRotation(-horizontalVel);
            transform.rotation = targetRot;
        }
    }

    [Button]
    public void Move(Vector2 direction, float force)
    {
        if (force <= 0f) return;

        RestartWait();

        Vector3 dir3D = MapXZ(direction.normalized);
        _rb.AddForce(dir3D * force, ForceMode.Impulse);
    }

    public void RestartWait()
    {
        if (_waitForStopCoroutine != null)
        {
            StopCoroutine(_waitForStopCoroutine);
            _waitForStopCoroutine = null;
        }
        _waitForStopCoroutine = StartCoroutine(CheckForStop());
    }

    public IEnumerator CheckForStop()
    {
        // Transition to Moving state and fire started event
        _currentState = MovementState.Moving;
        OnMovementStarted?.Invoke();

        // Give physics a moment to respond before checking velocity
        yield return new WaitForSeconds(0.2f);

        while (_currentState == MovementState.Moving)
        {
            Vector3 horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if (horizontalVelocity.magnitude <= _stopThreshold)
            {
                // Transition to Idle and fire stopped event
                _currentState = MovementState.Idle;
                OnMovementStopped?.Invoke();
            }
            yield return new WaitForFixedUpdate();
        }

        _waitForStopCoroutine = null;
    }

    [Button("Move By Angle")]
    public void MoveByAngle(
        [Tooltip("0° = up (Z+), 90° = right (X+), 180° = down (Z–), 270° = left (X–)")]
        float angleDegrees,
        float force)
    {
        if (force <= 0f) return;

        RestartWait();

        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector3 dir3D = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
        _rb.AddForce(dir3D * force, ForceMode.Impulse);

        AudioManager.Instance.PlaySFX(AudioConstIds.PLAYER_WHOOSH, true, transform.position);
    }

    private Vector3 MapXZ(Vector3 input)
    {
        return new Vector3(input.x, 0f, input.y);
    }

    public override void HandleCommand(string commandKey, object args, string sender, TwitchMessage msg)
    {
        if (args is not DirectionArguments directionArgs)
            return;
        
        HandleMovementCommand(directionArgs);
    }

    public void HandleMovementCommand(DirectionArguments args)
    {
        MoveByAngle(args.angle, ConvertForce(args.force));
    }

    public float ConvertForce(float percentage)
    {
        return percentage * 3f;
    }

    public Coroutine StartLoopedPathMovement(LoopPathController pathController, float speed)
    {
        return StartCoroutine(LoopMovement(pathController, speed));
    }

    private IEnumerator LoopMovement(LoopPathController pathController, float speed)
    {
        Transform lastPoint = null;

        while (true)
        {
            OnMovementStarted?.Invoke();
            // get next corner
            Transform nextPoint = pathController.QueryNextTarget(transform.position, lastPoint);
            lastPoint = nextPoint;

            // compute duration so DOTween can do speed-based move
            float dist = Vector3.Distance(transform.position, nextPoint.position);
            float duration = dist / speed;

            // tween and wait
            Tween t = _rb.DOMove(nextPoint.position, duration)
                         .SetEase(Ease.Linear);
            yield return t.WaitForCompletion();
        }
    }
}
