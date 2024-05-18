using MVPFrameWork;
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
        ServerManager.Instance.CreateBBSTypeView(_view.InptBBSTypeName.text);
        UIManager.Instance.Quit(ViewId.BBSTypeCreateView);
        // ServerManager.Instance.GetBBSFolder();
    }



   
}