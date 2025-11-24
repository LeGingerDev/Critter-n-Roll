using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class NumberSelector<T> : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _itemName;
    [SerializeField] protected TextMeshProUGUI _itemDescription;
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _selectionNumberText;
    [SerializeField] protected TextMeshProUGUI _playerCountText;

    private List<string> _usernames = new List<string>();
    private T _data;
    private int _selectionNumber;
    private System.Action<T, string> _onSelected;

    public T GetData()
    {
        return _data;
    }

    public void Initialise(T data, int selectionNumber, System.Action<T, string> onSelected)
    {
        _data = data;
        _selectionNumber = selectionNumber;
        _onSelected = onSelected;
        _selectionNumberText.text = $"!v {_selectionNumber}";
        BindData(data);
        _usernames.Clear();
        _playerCountText.text = "0";
    }

    public bool IsSelectionNumber(int num) => _selectionNumber == num;
    public bool ContainsPlayer(string username) => _usernames.Contains(username);

    public void AddPlayer(string username)
    {
        if (_usernames.Contains(username)) return;
        _usernames.Add(username);
        _playerCountText.text = _usernames.Count.ToString();
        _onSelected?.Invoke(_data, username);
    }

    public void RemovePlayer(string username)
    {
        if (!_usernames.Remove(username)) return;
        _playerCountText.text = _usernames.Count.ToString();
    }

    public int GetNumberOfSelected() => _usernames.Count;

    protected abstract void BindData(T data);
}