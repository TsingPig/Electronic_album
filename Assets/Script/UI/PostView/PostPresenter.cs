using MVPFrameWork;

public class PostPresenter : PresenterBase<IPostView, IPostModel>, IPostPresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PostView);
    }

    /// <summary>
    /// 【管理员操作】删除帖子 
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
                _view.Notification.title = "当前用户不是管理员！";
                _view.Notification.OpenNotification();
            }
        );
    }
}