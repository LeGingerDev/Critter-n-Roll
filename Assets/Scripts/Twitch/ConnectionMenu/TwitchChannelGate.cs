using System.Collections;
using ScoredProductions.StreamLinked.IRC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple gate that disables UI elements until connected to Twitch IRC channel
/// </summary>
public class TwitchChannelGate : MonoBehaviour
{
    [FoldoutGroup("Gated Components")]
    [SerializeField] private Button[] _gatedButtons;

    [FoldoutGroup("Gated Components")]
    [SerializeField] private CanvasGroup[] _gatedCanvasGroups;

    private TwitchIRCClient _ircClient;

    private void Start()
    {
        StartCoroutine(UpdateGateRoutine());
    }

    private IEnumerator UpdateGateRoutine()
    {
        while (this != null)
        {
            // Keep trying to get IRC client if we don't have it yet
            if (_ircClient == null)
            {
                TwitchIRCClient.GetInstance(out _ircClient);
            }

            bool isConnected = _ircClient != null && _ircClient.IsConnected;

            SetInteractables(isConnected);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetInteractables(bool interactable)
    {
        if (_gatedButtons != null)
        {
            for (int i = 0; i < _gatedButtons.Length; i++)
            {
                if (_gatedButtons[i] != null)
                {
                    _gatedButtons[i].interactable = interactable;
                }
            }
        }

        if (_gatedCanvasGroups != null)
        {
            for (int i = 0; i < _gatedCanvasGroups.Length; i++)
            {
                if (_gatedCanvasGroups[i] != null)
                {
                    _gatedCanvasGroups[i].interactable = interactable;
                }
            }
        }
    }
}
