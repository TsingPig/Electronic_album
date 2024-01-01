using Michsky.MUIP;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MomentPhotoItem : ItemBase
{
    public ButtonManager BtnEnterManager;
    public Image ImgPhoto;

    [HideInInspector]
    public string PhotoUrl;

    protected override void OnClick()
    {
        base.OnClick();
        Debug.Log($"MomentPhotoItem OnClick");

        PhotoDetailModel model = new PhotoDetailModel();

        model.SetModel(ImgPhoto, 0, CreatePhotoWallItemPresenter.DefaultTargetAlbumName);

        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoDetailView, model);
    }

    private void Start()
    {
        BtnItem = BtnEnterManager;
    }

    public void GetPhotoAsync()
    {
        ServerManager.Instance.GetPhotoAsync(PhotoUrl, ImgPhoto);
    }
}