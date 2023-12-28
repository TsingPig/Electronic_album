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
        PhotoModel model = new PhotoModel();
        model.SetName(AlbumTitle.text);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoView, model);
        Debug.Log("OnClick");
    }

    private void Start()
    {
        AlbumTitle.text = gameObject.name;

        BtnItem = BtnEnterManager;
    }
}