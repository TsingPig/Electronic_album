using MVPFrameWork;
using UnityEngine;

public class PhotoPresenter : PresenterBase<IPhotoView>, IPhotoPresenter
{

    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        Debug.Log((Model as IPhotoModel).Name);
    }

    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PhotoView);
    }
}