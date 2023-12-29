using MVPFrameWork;
using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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
        //Debug.Log(_model.AlbumName);
        //Debug.Log((Model as IPhotoModel).Name); µÈ¼ÛÐ´·¨
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    private async void LoadPhotoItemAsync()
    {
        var task = Res<GameObject>.LoadAsync(StrDef.PHOTO_ITEM_DATA_PATH);
        await task;
        GameObject.Instantiate(task.Result, _view.GridPhotoContent.transform);
        //var handler = Res<GameObject>.LoadAsync(StrDef.PHOTO_ITEM_DATA_PATH);
        //await handler;
        ////GameObject obj = handler.Result;
        //AsyncOperationHandle<IList<IResourceLocation>> handlers = Addressables.LoadResourceLocationsAsync(StrDef.PHOTO_ITEM_DATA_PATH);
        //IList<IResourceLocation> results = handlers.Result;
        Instantiater.Release();
        for(int i = 0; i < 20; i++)
        {
            await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform);
        }
    }
}