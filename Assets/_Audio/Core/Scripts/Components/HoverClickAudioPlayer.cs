using Audio.Managers;
using Audio.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverClickAudioPlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool playOnHover = true;
    public bool playOnClick = true;
    [ShowIf("@playOnHover")]
    public AudioPlayerData hoverAudio;
    [ShowIf("@playOnClick")]
    public AudioPlayerData clickAudio;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playOnHover)
            return;
        AudioManager.Instance.PlaySFX(hoverAudio.clipId, hoverAudio.isOneShot, hoverAudio.positionBased ? transform.position : null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!playOnClick)
            return;
        AudioManager.Instance.PlaySFX(clickAudio.clipId, clickAudio.isOneShot, clickAudio.positionBased ? transform.position : null);
    }
}
