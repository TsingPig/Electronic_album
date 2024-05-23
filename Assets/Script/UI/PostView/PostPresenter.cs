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
        InitializePostItem();
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        RefreshPostView();
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
                ServerManager.Instance.DeletePostItem(_model.Post.PostId, () =>
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
                    Title = "ɾ��������Ҫ����ԱȨ�ޣ�"
                });
            }
        );
    }


    private void RefreshPostView()
    {
        ClearPostItem();
        InitializePostItem();

    }

    private async void RefreshPostModel(Action callback = null)
    {
        _model.Comments = await ServerManager.Instance.GetComments(_model.Post.PostId);
        // _model.Posts = await ServerManager.Instance.GetBBSPosts(_model.Section.sectionname);
        callback?.Invoke();
    }

    /// <summary>
    /// ��ʾ������������
    /// </summary>
    private async void InitializePostItem()
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

        _view.CommentItemRoot.RebuildLayout();



    }

    /// <summary>
    /// ˢ����������
    /// </summary>
    /// <param name="callback"></param>
    private async void RefreshCommentItem(Action callback = null)
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


    private void ClearPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.POST_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.POST_PHOTO_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.POST_PHOTO_ITEM_DATA_PATH);
    }
}