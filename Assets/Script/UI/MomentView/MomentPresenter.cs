using MVPFrameWork;
using UnityEngine;

public class MomentPresenter : PresenterBase<IMomentView, IMomentModel>, IMomentPresenter
{
    public void Quit()
    {
        Debug.Log("MomentPresenter");
        UIManager.Instance.Quit(ViewId.MomentView);
    }
    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }
    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        if (_model.PhotoWallItemObj)
        {

            GameObject.Instantiate(_model.PhotoWallItemObj, _view.PhotoWallItemRoot);
        }
        else
        {
            Debug.LogError("PhotoWallItemObj为空，无法加载");
        }
    }

    public override void OnHideStart()
    {
        base.OnHideStart();
        GameObject.Destroy(_view.PhotoWallItemRoot.GetChild(0).gameObject);
    }
}