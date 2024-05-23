using MVPFrameWork;
using Sirenix.Utilities;
using System;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;

public class PostPresenter : PresenterBase<IPostView, IPostModel>, IPostPresenter
{
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        InitializePostItem(RefreshPostView);
    }

    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PostView);
        _view.CreateCommentPanel.gameObject.SetActive(false);
    }

    [Obsolete("请使用TryDeletePost，其Notification效果更加通用。")]
    /// <summary>
    /// 【管理员操作】删除帖子 
    /// </summary>
    public void DeletePost()
    {
        CacheManager.Instance.CheckSuper(() =>
            {
                ServerManager.Instance.DeletePostItem(_model.Post.PostId, () =>
                {
                    UIManager.Instance.Quit(ViewId.PostView);
                }
                );
            }, () =>
            {
                //_view.Notification.title = "当前用户不是管理员！";
                //_view.Notification.OpenNotification();
            }
        );
    }

    /// <summary>
    /// 【管理员操作】尝试删除帖子 
    /// </summary>
    public void TryDeletePost()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.DeletePostItem(_model.Post.PostId,
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
                    Title = "删帖操作需要管理员权限！"
                });
            }
        );
    }

    /// <summary>
    /// 确认创建评论
    /// </summary>
    public void SureCreateComment()
    {
        ServerManager.Instance.CreateCommentItem(CacheManager.Instance.UserName, _model.Post.PostId, _view.InptComment.text,
            () =>
            {
                _view.CreateCommentPanel.gameObject.SetActive(false);
                RefreshPostView();
            });
    }

    /// <summary>
    /// 初始化帖子内容主体
    /// </summary>
    private async void InitializePostItem(Action callback = null)
    {
        BBSPostItem bBSPostItem = (await Instantiater.InstantiateAsync(StrDef.POST_ITEM_DATA_PATH, _view.PostItemRoot.transform)).GetComponent<BBSPostItem>();
        bBSPostItem.BtnEnterPost.interactable = false;
        bBSPostItem.Title.text = _model.Post.Title;
        bBSPostItem.UserName.text = _model.Post.UserName;
        bBSPostItem.Content.text = _model.Post.Content;
        bBSPostItem.PhotoUrls = _model.Post.PhotoUrls;
        bBSPostItem.Post = _model.Post;

        await bBSPostItem.LoadPostPhotoItem(StrDef.POST_PHOTO_ITEM_DATA_PATH);

        _view.PostItemRoot.RebuildLayout();

        await Task.Delay(200);

        RectTransform scrollbarRectTransform = _view.ScrollbarView;
        RectTransform postRootRectTransform = _view.PostRoot;
        RectTransform postItemRootRectTransform = _view.PostItemRoot.GetComponent<RectTransform>();
        float postRootHeight = postRootRectTransform.rect.height;
        float postItemRootHeight = postItemRootRectTransform.rect.height;
        float newHeight = postRootHeight - postItemRootHeight - 50f;
        scrollbarRectTransform.sizeDelta = new Vector2(scrollbarRectTransform.sizeDelta.x, newHeight);

        callback?.Invoke();

    }

    /// <summary>
    /// 刷新视图（数据、布局）
    /// </summary>
    private void RefreshPostView()
    {
        ClearPostItem();
        InitializePostItem(() => { RefreshPostModel(() => { RefreshCommentItem(() => { _view.CommentItemRoot.RebuildLayout(); }); }); });
    }

    /// <summary>
    /// 刷新帖子评论数据
    /// </summary>
    /// <param name="callback"></param>
    private async void RefreshPostModel(Action callback = null)
    {
        _model.Comments = await ServerManager.Instance.GetComments(_model.Post.PostId);
        callback?.Invoke();
    }

    /// <summary>
    /// 刷新评论区项
    /// </summary>
    /// <param name="callback"></param>
    private async void RefreshCommentItem(Action callback = null)
    {
        foreach(IPostModel.Comment comment in _model.Comments)
        {
            CommentItem commentItem = (await Instantiater.InstantiateAsync(StrDef.COMMENT_ITEM_DATA_PATH, _view.CommentItemRoot.transform)).GetComponent<CommentItem>();
            commentItem.UserName.text = comment.UserName;
            commentItem.Content.text = comment.Content;
            commentItem.CreateTime.text = comment.CreateTime;
        }
        callback?.Invoke();
    }

    private void ClearPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.POST_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.POST_PHOTO_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.COMMENT_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.POST_PHOTO_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.COMMENT_ITEM_DATA_PATH);
    }
}