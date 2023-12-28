using Michsky.MUIP;
using TMPro;
using UnityEngine;

/// <summary>
/// ����б���ĵ���¼�
/// </summary>
public class AlbumItem : ItemBase
{
    public TMP_Text AlbumTitle;
    public ButtonManager BtnEnterManager;

    protected override void OnClick()
    {
        base.OnClick();
        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoView);
        Debug.Log("OnClick");
    }

    private void Start()
    {
        AlbumTitle.text = gameObject.name;

        BtnItem = BtnEnterManager;
    }
}