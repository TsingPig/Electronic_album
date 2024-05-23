using MVPFrameWork;
using TsingPigSDK;
using UIManager = MVPFrameWork.UIManager;

public class PhotoDetailPresenter : PresenterBase<IPhotoDetailView, IPhotoDetailModel>, IPhotoDetailPresenter
{
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }

    /// <summary>
    /// 只有非动态相册且运行删除时，显示删除按钮
    /// </summary>
    public override void OnShowCompleted()
    {
        base.OnCreateCompleted();
        _view.ImgDetailPhoto.sprite = _model.ImgPhotoDetail.sprite;
        _view.BtnDeletePhoto.gameObject.SetActive(_model.AllowDelete && (_model.AlbumName != CreatePhotoWallItemPresenter.DefaultTargetAlbumName && _model.AlbumName != CreatePostItemPresenter.DefaultTargetAlbumName));
    }

    public void DeletePhoto()
    {
        string albumName = _model.AlbumName;
        if(albumName == "Moment" || albumName == "Post")
        {
            UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
            {
                Title = "无法删除",
            });
            return;
        }
        UIManager.Instance.Quit(ViewId.PhotoDetailView);
        ServerManager.Instance.DeletePhoto(CacheManager.Instance.UserName, albumName,
            Instantiater.DeactivateObjectById(StrDef.PHOTO_ITEM_DATA_PATH, _model.PhotoId));
    }

    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PhotoDetailView);
    }
}