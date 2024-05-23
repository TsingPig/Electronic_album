using MVPFrameWork;
using System;
using TsingPigSDK;
using UIManager = MVPFrameWork.UIManager;

public class PostPresenter : PresenterBase<IPostView, IPostModel>, IPostPresenter
{

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();

    }

    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PostView);
    }

    [Obsolete("��ʹ��TryDeletePost����NotificationЧ������ͨ�á�")]
    /// <summary>
    /// ������Ա������ɾ������ 
    /// </summary>
    public void DeletePost()
    {
        CacheManager.Instance.CheckSuper(() =>
            {
                ServerManager.Instance.DeletePostItem(_model.Post.UserName, _model.Post.CreateTime, () =>
                {
                    UIManager.Instance.Quit(ViewId.PostView);
                }
                );
            }, () =>
            {
                //_view.Notification.title = "��ǰ�û����ǹ���Ա��";
                //_view.Notification.OpenNotification();
            }
        );
    }

    /// <summary>
    /// ������Ա����������ɾ������ 
    /// </summary>
    public void TryDeletePost()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.DeletePostItem(_model.Post.UserName, _model.Post.CreateTime,
                    () =>
                    {
                        UIManager.Instance.Quit(ViewId.PostView);
                    }
                );
            },
            () =>
            {
                UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
                {
                    Title = "ɾ��������Ҫ����ԱȨ�ޣ�"
                });
            }
        );
    }

    private void RefreshBBSView()
    {
        ClearBBSPostItem();
        // RefreshPostModel(() => { RefreshBBSPostItem(() => { _view.BBSPostItemRoot.RebuildLayout(); }); });
    }

    private async void RefreshPostModel(Action callback = null)
    {
        // _model.Posts = await ServerManager.Instance.GetBBSPosts(_model.Section.sectionname);
        callback?.Invoke();
    }

    private async void RefreshBBSPostItem(Action callback = null)
    {
        //foreach(IBBSModel.Post post in _model.Posts)
        //{
        //    BBSPostItem bBSPostItem = (await Instantiater.InstantiateAsync(StrDef.B_B_S_POST_ITEM_DATA_PATH, _view.BBSPostItemRoot.transform)).GetComponent<BBSPostItem>();
        //    bBSPostItem.Title.text = post.Title;
        //    bBSPostItem.UserName.text = post.UserName;
        //    bBSPostItem.Content.text = post.Content;
        //    bBSPostItem.PhotoUrls = post.PhotoUrls;
        //    bBSPostItem.Post = post;
        //    await bBSPostItem.LoadPostItems();
        //}
        //callback?.Invoke();
    }

    private void ClearBBSPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.POST_PHOTO_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.POST_PHOTO_ITEM_DATA_PATH);
    }
}