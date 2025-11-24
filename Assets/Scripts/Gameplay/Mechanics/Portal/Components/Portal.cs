using Audio.Core;
using Audio.Managers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [FoldoutGroup("Portal Settings"), SerializeField]
    private Portal _linkedPortal;
    [FoldoutGroup("Portal Settings"), SerializeField]
    private float _detectionRadius = 2f;
    [FoldoutGroup("Portal Settings"), SerializeField]
    private LayerMask _playerLayer;

    private HashSet<PlayerController> _recentPlayers = new HashSet<PlayerController>();

    public void Start()
    {
        StartCoroutine(PortalLoop());
    }

    public IEnumerator PortalLoop()
    {
        while(true)
        {
            PlayerDetectionCheck();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PlayerDetectionCheck()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _detectionRadius, _playerLayer);
        if (cols.Length <= 0) return;

        List<PlayerController> players = cols.ToList().Select(c => c.GetComponent<PlayerController>())
            .Where(p => !_linkedPortal.HasPlayerEnteredRecently(p)).ToList();

        if (players.Count <= 0) return;

        players.ForEach(p => StartCoroutine(TeleportPlayer(p)));
    }

    public bool HasPlayerEnteredRecently(PlayerController playerController)
    {
        if(_recentPlayers.Contains(playerController)) return true;
        return false;
    }

    public IEnumerator PlayerPortalTracking(PlayerController playerController)
    {
        _linkedPortal.AddPlayerToHash(playerController);
        AddPlayerToHash(playerController);
        while (Vector3.Distance(_linkedPortal.transform.position, playerController.transform.position) < _detectionRadius + 1f)
        {
            yield return null;
        }
        RemovePlayerFromHash(playerController);
        _linkedPortal.RemovePlayerFromHash(playerController);
    }

    public IEnumerator TeleportPlayer(PlayerController player)
    {

        Rigidbody rb = player.GetComponent<Rigidbody>();
        Vector3 currentVel = rb.linearVelocity;
        Vector3 teleportToPos = _linkedPortal.transform.position;
        //AudioManager.Instance.PlaySFX(AudioConstIds.PORTAL, true, transform.position);

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();

        player.transform.position = teleportToPos;
        rb.isKinematic = false;
        rb.linearVelocity = currentVel;
        StartCoroutine(PlayerPortalTracking(player));
    }

    public void AddPlayerToHash(PlayerController player) => _recentPlayers.Add(player);
    public void RemovePlayerFromHash(PlayerController player) => _recentPlayers.Remove(player);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
