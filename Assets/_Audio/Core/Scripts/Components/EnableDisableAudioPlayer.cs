using Audio.Managers;
using Audio.Models;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnableDisableAudioPlayer : MonoBehaviour
{
    public bool playOnEnable = true;
    public bool playOnDisable = true;
    [ShowIf("@playOnEnable")]
    public AudioPlayerData enableAudio;
    [ShowIf("@playOnDisable")]
    public AudioPlayerData disableAudio;

    private void OnEnable()
    {
        if (!playOnEnable)
            return;
        AudioManager.Instance.PlaySFX(enableAudio.clipId, enableAudio.isOneShot, enableAudio.positionBased ? transform.position : null);
    }
    private void OnDisable()
    {
        if (!playOnDisable)
            return;
        AudioManager.Instance.PlaySFX(disableAudio.clipId, disableAudio.isOneShot, disableAudio.positionBased ? transform.position : null);
    }
}