using Michsky.MUIP;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����б���ĵ���¼�
/// </summary>
public class PhotoItem : ItemBase
{
    public ButtonManager BtnEnterManager;
    public Image Cover;

    protected override void OnClick()
    {
        base.OnClick();
        Debug.Log($"PhotoItem OnClick");

        //PhotoModel model = new PhotoModel();
        //model.SetModel(AlbumTitle.text);

        // Debug.Log(AlbumTitle.text);

        //MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoView, model);
    }

    private void Start()
    {
        BtnItem = BtnEnterManager;
    }
}