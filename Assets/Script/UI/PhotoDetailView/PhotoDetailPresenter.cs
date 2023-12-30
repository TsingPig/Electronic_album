using MVPFrameWork;
public class PhotoDetailPresenter : PresenterBase<IPhotoDetailView, IPhotoDetailModel>, IPhotoDetailPresenter
{
    public void DeletePhoto()
    {
        UIManager.Instance.Quit(ViewId.PhotoDetailView);
        
    }
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        _view.ImgDetailPhoto = _model.ImgPhotoDetail;
    }
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PhotoDetailView);
    }
}