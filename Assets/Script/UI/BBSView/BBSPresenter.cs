using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using Michsky.MUIP;
using UIManager = MVPFrameWork.UIManager;

public class BBSPresenter : PresenterBase<IBBSView>, IBBSPresenter
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
    /// ¡¾¹ÜÀíÔ±²Ù×÷¡¿ É¾³ý°å¿é
    /// </summary>
    public void DeleteSection()
    {
        CacheManager.Instance.CheckSuper(
            () =>
            {
                // TODO: ·þÎñÆ÷É¾³ý°å¿éÂß¼­
                ServerManager.Instance.DeleteBBSType("¼¾ÈóÃñ", () =>
                {
                    UIManager.Instance.Quit(ViewId.BBSView);
                    RefreshBBSView();
                });

            });
    }

    private void RefreshBBSView()
    {
        Debug.Log("RefreshBBSView");

        ClearBBSPostItem();

        RefreshPostModel(() => { RefreshBBSPostItem(() => { _view.BBSPostItemRoot.RebuildLayout(); }); });
    }

    private async void RefreshPostModel(Action callback = null)
    {

        callback?.Invoke();
    }

    private async void RefreshBBSPostItem(Action callback = null)
    {
        BBSPostItem bBSPostItem = (await Instantiater.InstantiateAsync(StrDef.B_B_S_POST_ITEM_DATA_PATH, _view.BBSPostItemRoot.transform)).GetComponent<BBSPostItem>();
        callback?.Invoke();
    }

    private void ClearBBSPostItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_POST_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_POST_ITEM_DATA_PATH);
    }


}