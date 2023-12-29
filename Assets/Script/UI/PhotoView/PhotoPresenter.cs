using MVPFrameWork;
using TsingPigSDK;
using UnityEngine;

public class PhotoPresenter : PresenterBase<IPhotoView, IPhotoModel>, IPhotoPresenter
{
    public override void OnCreateCompleted()
    {
        OnShowCompleted();
    }

    public override void OnShowCompleted()
    {
        base.OnCreateCompleted();
        _view.TxtAlbumName.text = _model.AlbumName;
        LoadPhotoItemAsync();

        ServerManager.Instance.GetPhotos(CacheManager.Instance.UserName, _model.AlbumName);

        //Debug.Log(_model.AlbumName);
        //Debug.Log((Model as IPhotoModel).Name); µÈ¼ÛÐ´·¨
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    private async void LoadPhotoItemAsync()
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_ITEM_DATA_PATH);
        for(int i = 0; i < Random.Range(15, 35); i++)
        {
            await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform);
        }
    }
}