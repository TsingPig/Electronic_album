using MVPFrameWork;
using UnityEngine;

public class PhotoPresenter : PresenterBase<IPhotoView, IPhotoModel>, IPhotoPresenter
{
    public override void OnShowCompleted()
    {
        base.OnCreateCompleted();
        _view.TxtAlbumName.text = _model.AlbumName;
        //Debug.Log(_model.AlbumName);
        //Debug.Log((Model as IPhotoModel).Name); µÈ¼ÛÐ´·¨
    }

    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.PhotoView);
    }
}