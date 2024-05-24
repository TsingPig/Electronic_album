using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using UIManager = MVPFrameWork.UIManager;

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
        ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);
    }

    /// <summary>
    /// 上传单张照片
    /// </summary>
    public void UploadPhoto()
    {
        string albumName = _model.AlbumName;
        if(albumName == "Moment" || albumName == "Post")
        {
            Debug.LogError("不能上传到系统相册！");
            UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
            {
                Title = "不能上传到系统相册！"
            });
            return;
        }
        Texture2D photoTex = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("图片路径: " + path);
            if(path != null)
            {
                try
                {
                    photoTex = CacheManager.LoadTexture(path).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize);
                    //photoTex = ScaleTexture(photoTex, ConstDef.ScaleSize, ConstDef.ScaleSize);
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
        string albumName = _model.AlbumName;
        if(albumName == "Moment" || albumName == "Post")
        {
            Debug.LogError("不能上传到系统相册！");
            UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
            {
                Title = "不能上传到系统相册！"
            });
            return;
        }
#if UNITY_EDITOR
        UploadPhoto();
#else
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
                    photoTextures = CacheManager.LoadTexture(path).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize);
                    if(photoTextures != null)
                    {
                        ServerManager.Instance.UploadPhotos(CacheManager.Instance.UserName, _model.AlbumName, photoTextures.EncodeToPNG(), () =>
                        {
                            ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, InitialPhotoItemsAsync);
                        });
                    }
                    else
                    {
                        Debug.Log($"Cannot load image from {path}");
                    }

                    //ServerManager.Instance.UploadPhotos(CacheManager.Instance.UserName, _model.AlbumName, path, () =>
                    //{
                    //    ServerManager.Instance.GetAlbumSize(CacheManager.Instance.UserName, _model.AlbumName, RefreshUploadedPhotoItemAsync);
                    //});
                }
                catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });
#endif
    }

    /// <summary>
    /// 删除相册
    /// </summary>
    public void DeleteAlbum()
    {
        string albumName = _model.AlbumName;
        if(albumName == "Moment" || albumName == "Post")
        {
            Debug.LogError("不能删除系统相册！");
            UIManager.Instance.Enter(ViewId.NotificationView, new NotificationModel()
            {
                Title = "不能删除系统相册！"
            });
            return;
        }
        ServerManager.Instance.DeletaAlbumFolder(CacheManager.Instance.UserName, albumName);
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