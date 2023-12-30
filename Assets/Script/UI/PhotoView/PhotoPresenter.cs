using MVPFrameWork;
using System;
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
        //Debug.Log((Model as IPhotoModel).Name); 等价写法
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    public void UploadPhoto()
    {
        Texture2D photoTex = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("图片路径: " + path);
            if(path != null)
            {
                try
                {
                    photoTex = CacheManager.LoadTexture(path);
                    photoTex = ScaleTexture(photoTex, 200, 200);
                    if(photoTex != null)
                    {
                        ServerManager.Instance.UploadPhoto(CacheManager.Instance.UserName, _model.AlbumName, photoTex.EncodeToPNG(), OnShowCompleted);
                    }
                    else
                    {
                        Debug.Log($"Cannot load image from {path}");
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });
    }

    public void DeleteAlbum()
    {
        ServerManager.Instance.DeletaAlbumFolder(CacheManager.Instance.UserName, _model.AlbumName);

        //MVPFrameWork.UIManager.Instance.Quit(View)
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

    private Texture2D ScaleTexture(Texture2D source, float targetWidth, float targetHeight)
    {
        Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

        float incX = (1.0f / targetWidth);
        float incY = (1.0f / targetHeight);

        for(int i = 0; i < result.height; ++i)
        {
            for(int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        return result;
    }
}