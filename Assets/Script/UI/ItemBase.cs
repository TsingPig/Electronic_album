using Michsky.MUIP;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected ButtonManager _btnItem;

    protected virtual void OnClick()
    {

    }
    protected virtual void Start()
    {
        _btnItem.onClick.AddListener(OnClick);
    }

}

