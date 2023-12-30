using MVPFrameWork;
using TsingPigSDK;

public class PhotoDetailPresenter : PresenterBase<IPhotoDetailView, IPhotoDetailModel>, IPhotoDetailPresenter
{
    public void DeletePhoto()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoDetailView);
        Instantiater.DeactivateObjectByIndex(StrDef.PHOTO_ITEM_DATA_PATH, _model.PhotoId);
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