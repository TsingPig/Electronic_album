using Michsky.MUIP;
using MVPFrameWork;
using System.Diagnostics;
using TMPro;
using TsingPigSDK;
using UnityEngine.UI;
using UIManager = MVPFrameWork.UIManager;

public class BBSTypePresenter : PresenterBase<IBBSTypeView>, IBBSTypePresenter
{
    ButtonManager BtnCreateBBSType { get; set; }

    public void EnterBBSTypeCreate()
    {
        Log.Info("¼¾ÉñÎÒ°®Äã");
        UIManager.Instance.Enter(ViewId.BBSView);
    }
}