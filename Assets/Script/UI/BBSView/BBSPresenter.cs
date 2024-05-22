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

    private void RefreshBBSView()
    {
        Debug.Log("RefreshBBSView");

        RefreshPostModel(() =>
        {
            RefreshBBSPostItem(() =>
            {
                _view.BBSPostItemRoot.RebuildLayout();
            });
        });
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


}