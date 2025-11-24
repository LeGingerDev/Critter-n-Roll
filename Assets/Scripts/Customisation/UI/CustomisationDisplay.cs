using Audio.Core;
using Audio.Managers;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CustomisationDisplay : NumberSelector<Customisation>
{
    [SerializeField] private Transform _movingElement;

    public void ResetPosition() => _movingElement.position = new Vector3(_movingElement.position.x, -1000, _movingElement.position.z);

    public IEnumerator ShowAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        _movingElement.DOLocalMoveY(0, 0.25f).SetEase(Ease.OutBack);
        AudioManager.Instance.PlaySFX(AudioConstIds.CARD_SHOW); // Change this to whatever SFX you want
    }

    protected override void BindData(Customisation data)
    {
        _itemName.text = data.displayName;
        _itemImage.sprite = data.icon;
    }
}