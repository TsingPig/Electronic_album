using MVPFrameWork;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

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
        //LoadPhotoItemAsync();

        ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, LoadPhotoItemAsync);

        //Debug.Log(_model.AlbumName);
        //Debug.Log((Model as IPhotoModel).Name); µÈ¼ÛÐ´·¨
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    private async void LoadPhotoItemAsync(int albumSize)
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_ITEM_DATA_PATH);
        //for(int i = 0; i < Random.Range(15, 35); i++)
        //{
        //    await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform);
        //}
        for(int i = 0; i < albumSize; i++)
        {
            Image photoImage = (await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>().Cover;
            ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, _model.AlbumName, i, photoImage);
        }
    }
}