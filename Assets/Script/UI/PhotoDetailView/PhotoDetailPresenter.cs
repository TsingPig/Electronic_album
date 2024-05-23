using MVPFrameWork;
using TsingPigSDK;

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
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoDetailView);
        ServerManager.Instance.DeletePhoto(CacheManager.Instance.UserName, _model.AlbumName,
            Instantiater.DeactivateObjectById(StrDef.PHOTO_ITEM_DATA_PATH, _model.PhotoId));
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoDetailView);
    }
}