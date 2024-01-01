using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class CreatePhotoWallItemPresenter : PresenterBase<ICreatePhotoWallItemView, ICreatePhotoWallItemModel>, ICreatePhotoWallItemPresenter
{
    public static string DefaultTargetAlbumName = "Moment";

    public override void OnCreateCompleted()
    {
        base.OnCreateCompleted();
        OnShowCompleted();
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        Initialize();
    }

    /// <summary>
    /// 上传多图
    /// </summary>
    public void UploadPhotos()
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
                    _model.Photos = photoTextures;
                    VisualizeUploadedPhotos(path.Length);
                }
                catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });
    }

    [Obsolete("上传单图已经过时，请改用上传多图")]
    public void UploadPhoto()
    {
        Texture2D[] photoTextures = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if(path != null)
            {
                try
                {
                    photoTextures = new Texture2D[1] { CacheManager.LoadTexture(path).Scale(200, 200) };
                    _model.Photos = photoTextures;
                    VisualizeUploadedPhotos(1);
                }
                catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });
    }

    /// <summary>
    /// 创建一条新的动态项
    /// </summary>
    public void CreatePhotoWallItem()
    {
        ServerManager.Instance.CreateAlbumFolder(CacheManager.Instance.UserName, DefaultTargetAlbumName, () =>
        {
            if(_model.Photos != null)
            {
                ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);

                ServerManager.Instance.UploadPhotos(CacheManager.Instance.UserName, DefaultTargetAlbumName, _model.Photos.EncodeToPNG(), () =>
                {
                    ServerManager.Instance.UploadMomentItem(CacheManager.Instance.UserName, _view.InptContent.text, _model.Photos.Length, () =>
                    {
                        Debug.Log($"上传动态成功");
                        MVPFrameWork.UIManager.Instance.Quit(ViewId.CreatePhotoWallItemView);
                    });
                });
            }
            else
            {
                Debug.LogWarning("无法上传空动态");
            }
        });
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.CreatePhotoWallItemView);
    }

    /// <summary>
    /// 刷新上传列表的图片
    /// </summary>
    /// <param name="uploadedPhotoCount"></param>
    private async void VisualizeUploadedPhotos(int uploadedPhotoCount)
    {
        for(int i = 0; i < uploadedPhotoCount; i++)
        {
            PhotoItem photoItem = (await Instantiater.InstantiateAsync(StrDef.PHOTO_UPLOAD_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>();
            Image photoImage = photoItem.Cover;
            photoImage.sprite = Sprite.Create(_model.Photos[i], new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f));
        }
    }

    private void Initialize()
    {
        _view.InptContent.text = string.Empty;
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_UPLOAD_ITEM_DATA_PATH);
        _model.Photos = null;
        _model.Content = null;
    }
}