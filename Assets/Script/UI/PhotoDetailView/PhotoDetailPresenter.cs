using MVPFrameWork;
using TsingPigSDK;

public class PhotoDetailPresenter : PresenterBase<IPhotoDetailView, IPhotoDetailModel>, IPhotoDetailPresenter
{
    public void DeletePhoto()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoDetailView);
        ServerManager.Instance.DeletePhoto(CacheManager.Instance.UserName, _model.AlbumName, 
            Instantiater.DeactivateObjectById(StrDef.PHOTO_ITEM_DATA_PATH, _model.PhotoId));
    }

    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }

    public override void OnShowCompleted()
    {
        base.OnCreateCompleted();
        _view.ImgDetailPhoto.sprite = _model.ImgPhotoDetail.sprite;
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoDetailView);
    }
}