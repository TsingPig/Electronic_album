using MVPFrameWork;
using System;
using TsingPigSDK;
using UIManager = MVPFrameWork.UIManager;

public class BBSPresenter : PresenterBase<IBBSView, IBBSModel>, IBBSPresenter
{
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        RefreshBBSView();
        ServerManager.Instance.UpdatePostItemEvent += RefreshBBSView;
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        RefreshBBSView();
    }


    [Obsolete("TryDeleteSection，其Notification效果更加通用。")]
    /// <summary>
    /// 【管理员操作】 删除板块
    /// </summary>
    public void DeleteSection()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.DeleteBBSType(_model.Section.sectionname, () =>
                {
                    UIManager.Instance.Quit(ViewId.BBSView);
                });
            },
            () =>
            {
                //_view.Notification.title = "当前用户不是管理员！";
                //_view.Notification.OpenNotification();
            }
            );
    }

    /// <summary>
    /// 【管理员操作】 尝试删除板块
    /// </summary>
    public void TryDeleteSection()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.DeleteBBSType(_model.Section.sectionname, () =>
                {
                    UIManager.Instance.Quit(ViewId.BBSView);
                });
            },
            () =>
            {
                UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
                {
                    Title = "删除板块操作需要管理员权限！"
                });
            }
            );
    }

    /// <summary>
    /// 进入发布帖子页面
    /// </summary>
    public void EnterCreatePostItemView()
    {
        UIManager.Instance.Enter(ViewId.CreatePostItemView, new CreatePostItemModel()
        {
            SectionName = _model.Section.sectionname
        });
    }

    private void RefreshBBSView()
    {
        ClearBBSPostItem();
        RefreshPostModel(() => { RefreshBBSPostItem(() => { _view.BBSPostItemRoot.RebuildLayout(); }); });
    }

    private async void RefreshPostModel(Action callback = null)
    {
        _model.Posts = await ServerManager.Instance.GetBBSPosts(_model.Section.sectionname);
        callback?.Invoke();
    }

    private async void RefreshBBSPostItem(Action callback = null)
    {
        foreach(IBBSModel.Post post in _model.Posts)
        {
            BBSPostItem bBSPostItem = (await Instantiater.InstantiateAsync(StrDef.B_B_S_POST_ITEM_DATA_PATH, _view.BBSPostItemRoot.transform)).GetComponent<BBSPostItem>();
            bBSPostItem.Title.text = post.Title;
            bBSPostItem.UserName.text = post.UserName;
            bBSPostItem.Content.text = post.Content;
            bBSPostItem.PhotoUrls = post.PhotoUrls;
            bBSPostItem.Post = post;
            await bBSPostItem.LoadPostPhotoItem(StrDef.B_B_S_POST_PHOTO_ITEM_DATA_PATH);
        }
        callback?.Invoke();
    }

    private void ClearBBSPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_POST_PHOTO_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_POST_PHOTO_ITEM_DATA_PATH);
    }
}