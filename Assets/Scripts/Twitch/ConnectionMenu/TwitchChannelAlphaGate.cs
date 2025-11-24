using System.Collections;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// Simple gate that controls CanvasGroup alpha based on Twitch IRC channel connection
/// </summary>
public class TwitchChannelAlphaGate : MonoBehaviour
{
    [FoldoutGroup("Alpha Settings")]
    [SerializeField] private CanvasGroup[] _gatedCanvasGroups;

    [FoldoutGroup("Alpha Settings")]
    [SerializeField] private float _connectedAlpha = 1f;

    [FoldoutGroup("Alpha Settings")]
    [SerializeField] private float _disconnectedAlpha = 0.5f;

    private TwitchIRCClient _ircClient;

    private void Start()
    {
        StartCoroutine(UpdateAlphaRoutine());
    }

    private IEnumerator UpdateAlphaRoutine()
    {
        while (this != null)
        {
            // Keep trying to get IRC client if we don't have it yet
            if (_ircClient == null)
            {
                TwitchIRCClient.GetInstance(out _ircClient);
            }

            bool isConnected = _ircClient != null && _ircClient.IsConnected;

            SetAlpha(isConnected);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetAlpha(bool isConnected)
    {
        if (_gatedCanvasGroups != null)
        {
            float targetAlpha = isConnected ? _connectedAlpha : _disconnectedAlpha;

            for (int i = 0; i < _gatedCanvasGroups.Length; i++)
            {
                if (_gatedCanvasGroups[i] != null)
                {
                    _gatedCanvasGroups[i].alpha = targetAlpha;
                }
            }
        }
    }
}