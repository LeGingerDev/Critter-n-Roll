using Audio.Core;
using Audio.Managers;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelSelector : NumberSelector<Level>
{
    [SerializeField]
    private DifficultyVisualiser _difficultyVisualiser;
    [SerializeField]
    private Transform _movingElement;

    public void ResetPosition() => _movingElement.position = new Vector3(_movingElement.position.x, -1000, _movingElement.position.z);

    public IEnumerator ShowAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        _movingElement.DOLocalMoveY(0, 0.25f).SetEase(Ease.OutBack);
        AudioManager.Instance.PlaySFX(AudioConstIds.CARD_SHOW);
    }

    protected override void BindData(Level data)
    {
        _difficultyVisualiser.Initialise(data.levelDifficulty);

        _itemName.text = data.levelName;
        //_itemDescription.text = data.levelDescription;
        if (data.levelVisual != null)
        {
            _itemImage.sprite = data.levelVisual;
            _itemImage.enabled = true;
        }
        else
        {
            _itemImage.enabled = false;
        }
    } 
}
