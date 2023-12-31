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
                    photoTex = CacheManager.LoadTexture(path).Scale(200, 200);
                    //photoTex = ScaleTexture(photoTex, 200, 200);
                    if(photoTex != null)
                    {
                        ServerManager.Instance.UploadPhoto(CacheManager.Instance.UserName, _model.AlbumName, photoTex.EncodeToPNG(), () =>
                        {
                            ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, RefreshUploadedPhotoItemAsync);
                        });
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
    /// 上传多张图片
    /// </summary>
    public void UploadMultiPhotos()
    {
        Texture2D[] photoTextures = null;
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((path) =>
        {
            foreach(var p in path)
            {
                Debug.Log("图片路径: " + p);
            }
            if(path != null)
            {
                try
                {
                    photoTextures = CacheManager.LoadTexture(path).Scale(200, 200);
                    //photoTextures = ScaleTexture(photoTextures, 200, 200);
                    if(photoTextures != null)
                    {
                        ServerManager.Instance.UploadPhotos(CacheManager.Instance.UserName, _model.AlbumName, photoTextures.EncodeToPNG(), () =>
                        {
                            ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, RefreshUploadedPhotoItemAsync);
                        });
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
        MVPFrameWork.UIManager.Instance.Quit(ViewId.PhotoView);
    }

    /// <summary>
    /// 初始化异步加载照片
    /// </summary>
    /// <param name="albumSize"></param>
    private async void InitialPhotoItemsAsync(int albumSize)
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_ITEM_DATA_PATH);

        for(int i = 0; i < albumSize; i++)
        {
            PhotoItem photoItem = (await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>();
            photoItem.photoId = i;
            photoItem.AlbumName = _model.AlbumName;
            Image photoImage = photoItem.Cover;
            ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, _model.AlbumName, i, photoImage);
        }
    }

    /// <summary>
    /// 异步刷新新上传的照片项
    /// </summary>
    private async void RefreshUploadedPhotoItemAsync(int albumSize)
    {
        PhotoItem photoItem = (await Instantiater.InstantiateAsync(StrDef.PHOTO_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>();
        Image photoImage = photoItem.Cover;
        photoItem.AlbumName = _model.AlbumName;
        photoItem.photoId = albumSize - 1;
        ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, _model.AlbumName, albumSize - 1, photoImage);
    }

    [Obsolete("方法已经废弃，请改用Texture2D扩展方法")]
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