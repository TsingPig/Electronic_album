using Michsky.MUIP;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    private ButtonManager _btnItem;

    protected ButtonManager BtnItem
    {
        get => _btnItem;
        set
        {
            _btnItem = value;
            _btnItem.onClick.AddListener(OnClick);
        }
    }

    protected virtual void OnClick()
    { }
}