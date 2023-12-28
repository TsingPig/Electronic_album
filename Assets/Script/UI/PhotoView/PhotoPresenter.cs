using MVPFrameWork;
using UnityEngine;

public class PhotoPresenter : PresenterBase<IPhotoView>, IPhotoPresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PhotoView);
    }
}