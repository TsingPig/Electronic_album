using MVPFrameWork;

public class PostPresenter : PresenterBase<IPostView, IPostModel>, IPostPresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PostView);
    }
}