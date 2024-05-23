using MVPFrameWork;

public class PostPresenter : PresenterBase<IPostView, IPostModel>, IPostPresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PostView);
    }

    /// <summary>
    /// ������Ա������ɾ������ 
    /// </summary>
    public void DeletePost()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.DeletePostItem(_model.Post.UserName, _model.Post.CreateTime, () =>
                {
                    UIManager.Instance.Quit(ViewId.PostView);
                }
                );
            },
            () =>
            {
                _view.Notification.title = "��ǰ�û����ǹ���Ա��";
                _view.Notification.OpenNotification();
            }
        );
    }
}