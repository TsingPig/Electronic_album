using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using Michsky.MUIP;
using UIManager = MVPFrameWork.UIManager;

public class BBSPresenter : PresenterBase<IBBSView, IBBSModel>, IBBSPresenter
{
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        RefreshBBSView();
    }


    /// <summary>
    /// 【管理员操作】 删除板块
    /// </summary>
    public void DeleteSection()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                // TODO: 服务器删除板块逻辑
                ServerManager.Instance.DeleteBBSType(_model.Section.sectionname, () =>
                {
                    UIManager.Instance.Quit(ViewId.BBSView);
                });
            },
            () =>
            {
                _view.Notification.title = "当前用户不是管理员！";
                _view.Notification.OpenNotification();
            }

            );
    }

    private void RefreshBBSView()
    {
        Debug.Log("RefreshBBSView");

        ClearBBSPostItem();

        RefreshPostModel(() => { RefreshBBSPostItem(() => { _view.BBSPostItemRoot.RebuildLayout(); }); });
    }

    private async void RefreshPostModel(Action callback = null)
    {
        _model.Posts = await ServerManager.Instance.GetBBSPostItems(_model.Section.sectionname);
        callback?.Invoke();
    }

    private async void RefreshBBSPostItem(Action callback = null)
    {
        foreach (IBBSModel.Post post in _model.Posts)
        {
            BBSPostItem bBSPostItem = (await Instantiater.InstantiateAsync(StrDef.B_B_S_POST_ITEM_DATA_PATH, _view.BBSPostItemRoot.transform)).GetComponent<BBSPostItem>();
        }
        callback?.Invoke();
    }

    private void ClearBBSPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_POST_ITEM_DATA_PATH);
    }


}