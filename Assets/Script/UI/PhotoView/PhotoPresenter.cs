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
        ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, InitialPhotoItemsAsync);
    }

    /// <summary>
    /// 退出当前相册
    /// </summary>
    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    /// <summary>
    /// 上传单张照片
    /// </summary>
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
                        ServerManager.Instance.UploadPhoto(CacheManager.Instance.UserName, _model.AlbumName, photoTex.EncodeToPNG(), RefreshUploadedPhotoItemAsync);
                        //ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, RefreshUploadedPhotoItemAsync);
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

    /// <summary>
    /// 删除相册
    /// </summary>
    public void DeleteAlbum()
    {
        ServerManager.Instance.DeletaAlbumFolder(CacheManager.Instance.UserName, _model.AlbumName);

        //MVPFrameWork.UIManager.Instance.Quit(View)
    }

    /// <summary>
    /// 初始化异步加载照片
    /// </summary>
    /// <param name="albumSize"></param>
    private async void InitialPhotoItemsAsync(int albumSize)
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_ITEM_DATA_PATH);
        //for(int i = 0; i < Random.Range(15, 35); i++)
        //{
        //    await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform);
        //}
        for(int i = albumSize - 1; i >= 0; i--)
        {
            Image photoImage = (await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>().Cover;
            ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, _model.AlbumName, i, photoImage);
        }   
    }

    /// <summary>
    /// 异步刷新新上传的照片项
    /// </summary>
    /// <param name="albumSize"></param>
    private async void RefreshUploadedPhotoItemAsync()
    {
        Image photoImage = (await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>().Cover;
        ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, _model.AlbumName, 0, photoImage);
    }

    /// <summary>
    /// 重设上传图片尺寸，以便于缩小内存占用，加快加载速度
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns>重设后的图片尺寸</returns>

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