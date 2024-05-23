using MVPFrameWork;
using TMPro;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;

public class BBSTypeCreatePresenter : PresenterBase<IBBSTypeCreateView>, IBBSTypeCreatePresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.BBSTypeCreateView);
        Debug.Log("Quit BBSTypeCreateView");
    }

    public void Create()
    {
        Debug.Log($"{_view.InptBBSTypeName.text}");
        ServerManager.Instance.CreateBBSType(_view.InptBBSTypeName.text);
        UIManager.Instance.Quit(ViewId.BBSTypeCreateView);
        // ServerManager.Instance.GetBBS(CacheManager.Instance.UserName);
    }

    public void TryCreate()
    {
        Debug.Log($"{_view.InptBBSTypeName.text}");
        CacheManager.Instance.CheckSuper(
            () =>
            {
                ServerManager.Instance.CreateBBSType(_view.InptBBSTypeName.text, () =>
                {
                    UIManager.Instance.Quit(ViewId.BBSTypeCreateView);
                });
            },
            () =>
            {
                UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
                {
                    Title = "创建板块操作需要管理员权限！"
                });
            }
        );
    }

    public void ClearInformation(TMP_InputField info)
    {
        if(info == null)
        {
            return;
        }
        info.text = "";
    }
}