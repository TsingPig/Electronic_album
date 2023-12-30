using Michsky.MUIP;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 相册列表项的点击事件
/// </summary>
public class PhotoItem : ItemBase
{
    public ButtonManager BtnEnterManager;
    public Image Cover;

    [HideInInspector]
    public string AlbumName;

    /// <summary>
    /// 图片索引
    /// </summary>
    //[HideInInspector]
    public int photoId;

    protected override void OnClick()
    {
        base.OnClick();
        Debug.Log($"PhotoItem OnClick");

        PhotoDetailModel model = new PhotoDetailModel();

        model.SetModel(Cover, photoId, AlbumName);

        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoDetailView, model);
    }

    private void Start()
    {
        BtnItem = BtnEnterManager;
    }
}