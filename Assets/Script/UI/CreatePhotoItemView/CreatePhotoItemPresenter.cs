using MVPFrameWork;
public class CreatePhotoItemPresenter : PresenterBase<ICreatePhotoItemView>, ICreatePhotoItemPresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.CreatePhotoWallItemView);
    }

}