using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class CreatePostItemPresenter : PresenterBase<ICreatePostItemView, ICreatePostItemModel>, ICreatePostItemPresenter {

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
    /// �ϴ���ͼ
    /// </summary>
    public void UploadPhotos()
    {
#if UNITY_EDITOR
        UploadPhoto();
#else
        Texture2D[] photoTextures = null;
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((path) =>
        {
            foreach(var p in path)
            {
                Debug.Log("ͼƬ·��: " + p);
            }
            if(path != null)
            {
                try
                {
                    photoTextures = CacheManager.LoadTexture(path).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize);
                    _model.Photos = photoTextures;
                    VisualizeUploadedPhotos(path.Length);
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
    /// �ϴ���ͼ
    /// </summary>
    public void UploadPhoto()
    {
        Texture2D[] photoTextures = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if(path != null)
            {
                try
                {
                    photoTextures = new Texture2D[1] { CacheManager.LoadTexture(path).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize) };
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
    /// ����һ���µ�����
    /// </summary>
    public void CreatePhotoWallItem()
    {

        ServerManager.Instance.CreateAlbumFolder(CacheManager.Instance.UserName, DefaultTargetAlbumName, () =>
        {
            if(_model.Photos != null || _model.)
            {
                ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);

                ServerManager.Instance.UploadPhotos(CacheManager.Instance.UserName, DefaultTargetAlbumName, _model.Photos.EncodeToPNG(), () =>
                {
                    ServerManager.Instance.UploadMomentItem(CacheManager.Instance.UserName, _view.InptContent.text, _model.Photos.Length, () =>
                    {
                        Debug.Log($"�ϴ���̬�ɹ�");
                        MVPFrameWork.UIManager.Instance.Quit(ViewId.CreatePhotoWallItemView);
                    });
                });
            }
            else
            {
                Debug.LogWarning("�޷��ϴ��ն�̬");
            }
        });
    }

    public void Quit()
    {
        MVPFrameWork.UIManager.Instance.Quit(ViewId.CreatePostItemView);
    }

    /// <summary>
    /// ˢ���ϴ��б��ͼƬ
    /// </summary>
    /// <param name="uploadedPhotoCount"></param>
    private async void VisualizeUploadedPhotos(int uploadedPhotoCount)
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_UPLOAD_ITEM_DATA_PATH);
        for(int i = 0; i < uploadedPhotoCount; i++)
        {
            PhotoItem photoItem = (await Instantiater.InstantiateAsync(StrDef.PHOTO_UPLOAD_ITEM_DATA_PATH, _view.GridPhotoContent.transform)).GetComponent<PhotoItem>();
            Image photoImage = photoItem.Cover;
            photoImage.sprite = Sprite.Create(_model.Photos[i], new Rect(0, 0, ConstDef.ScaleSize, ConstDef.ScaleSize), new Vector2(0.5f, 0.5f));
        }
    }

    private void Initialize()
    {
        _view.InptContent.text = string.Empty;
        _view.InptTitle.text = string.Empty;

        Instantiater.DeactivateObjectPool(StrDef.PHOTO_UPLOAD_ITEM_DATA_PATH);
        _model.Photos = null;
        _model.Content = null;
    }
}