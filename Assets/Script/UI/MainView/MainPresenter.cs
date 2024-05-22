using MVPFrameWork;
using System;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using Michsky.MUIP;
using UIManager = MVPFrameWork.UIManager;


public class MainPresenter : PresenterBase<IMainView, IMainModel>, IMainPresenter
{
    public override void OnCreateCompleted()
    {
        ServerManager.Instance.UpdateAlbumEvent += PresenterAlbumList;
        ServerManager.Instance.UpdateMomentEvent += RefreshPhotoWallView;
        ServerManager.Instance.UpdateBBSTypeEvent += RefreshBBSTypeView;
        ServerManager.Instance.DownLoadUserIconEvent += LoadUserInformation;
        CacheManager.Instance.UserInformUpdateEvent += LoadUserInformation;
        LoadUserInformation();
        RefreshMomentsModel(() => { RefreshPhotoWallItem(RefreshPhotoWallViewLayout); });
        RefreshSectionsModel(() => { RefreshBBSTypeItem(RefreshBBSTypeViewLayout); });
    }

    public override void OnShowCompleted()
    {
        base.OnShowCompleted();
        RefreshPhotoWallView();
        RefreshBBSTypeView();

    }

    #region TopPanel

    public void EnterCreatePhotoWallItemView()
    {
        CreatePhotoWallItemModel createPhotoWallItemModel = new CreatePhotoWallItemModel();
        MVPFrameWork.UIManager.Instance.Enter(ViewId.CreatePhotoWallItemView, createPhotoWallItemModel);
    }

    /// <summary>
    /// 用户退出登录时调用，清除用户信息和头像文件
    /// </summary>
    public void ClearUserInformationCache()
    {
        ClearAlbumList();
        CacheManager.Instance.ClearUserInformationCache();
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);
    }

    #endregion TopPanel

    #region BBSTypeView

    #region Public
    public void EnterBBSTypeCreateView()
    {
        UIManager.Instance.Enter(ViewId.BBSTypeCreateView);
    }


    #endregion

    #region Private


    private async void RefreshSectionsModel(Action callback = null)
    {
        _model.Sections = await ServerManager.Instance.GetSections();
        callback?.Invoke();

    }

    private void RefreshBBSTypeView()
    {
        Debug.Log("RefreshBBSTypeView");
        ClearBBSTypeItem();
        RefreshSectionsModel(() => { RefreshBBSTypeItem(RefreshBBSTypeViewLayout); });

    }

    private async void RefreshBBSTypeItem(Action callback = null)
    {
        foreach(IMainModel.Section section in _model.Sections)
        {
            BBSTypeItem bBSTypeItem = (await Instantiater.InstantiateAsync(StrDef.B_B_S_TYPE_ITEM_DATA_PATH, _view.BBSTypeItemRoot.transform)).GetComponent<BBSTypeItem>();
            bBSTypeItem.TxtBBSTypeName.text = section.sectionname;

            // Debug.Log(section.sectionname);
        }
        callback?.Invoke();
    }


    private void ClearBBSTypeItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.B_B_S_TYPE_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.B_B_S_TYPE_ITEM_DATA_PATH);
    }

    private void RefreshBBSTypeViewLayout()
    {
        _view.BBSTypeItemRoot.RebuildLayout();
    }

    #endregion

    #endregion

    #region PhotoWallView

    private async void RefreshMomentsModel(Action callback = null)
    {
        _model.Moments = await ServerManager.Instance.GetAllPhotoWallItems();
        callback?.Invoke();
    }

    private void RefreshPhotoWallView()
    {
        ClearPhotoWallItem();
        RefreshMomentsModel(() => RefreshPhotoWallItem(RefreshPhotoWallViewLayout));
    }

    private async void RefreshPhotoWallItem(Action callback = null)
    {
        foreach(IMainModel.Moment moment in _model.Moments)
        {
            PhotoWallItem photoWallItem = (await Instantiater.InstantiateAsync(StrDef.PHOTO_WALL_ITEM_DATA_PATH, _view.PhotoWallItemRoot.transform)).GetComponent<PhotoWallItem>();
            photoWallItem.TxtContent.text = moment.Content;
            photoWallItem.TxtUserName.text = MySQLManager.Instance.GetNickName(moment.UserName);
            photoWallItem.TxtHeartCount.text = UnityEngine.Random.Range(0, 100).ToString();
            photoWallItem.PhotoUrls = moment.PhotoUrls;
            await photoWallItem.LoadMomentPhotoItems();
        }
        callback?.Invoke();
    }

    private void ClearPhotoWallItem()
    {
        Instantiater.DeactivateObjectPool(StrDef.PHOTO_WALL_ITEM_DATA_PATH);
        Instantiater.DeactivateObjectPool(StrDef.MOMENT_PHOTO_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.PHOTO_WALL_ITEM_DATA_PATH);
        Instantiater.Release(StrDef.MOMENT_PHOTO_ITEM_DATA_PATH);
    }

    private void RefreshPhotoWallViewLayout()
    {
        _view.PhotoWallItemRoot.RebuildLayout();
    }

    #endregion PhotoWallView

    #region UserInformation

    #region Public

    /// <summary>
    /// 加载本地缓存的用户信息
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = CacheManager.Instance.UserInform;

        if(CacheManager.Instance.UserInformationCached)
        {
            PresentUserInformation(userInformation);
            ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);
        }
        return userInformation;
    }

    /// <summary>
    /// 更新昵称
    /// </summary>
    public void UpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(true);
        _view.TxtNickName.gameObject.SetActive(false);
        _view.BtnUpdateNickName.gameObject.SetActive(false);
    }

    /// <summary>
    /// 确认更新
    /// </summary>
    public void SureUpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(false);
        _view.TxtNickName.gameObject.SetActive(true);
        _view.BtnUpdateNickName.gameObject.SetActive(true);

        string updatedNickName = _view.InptNickName.text;
        if(updatedNickName != string.Empty)
        {
            _view.TxtNickName.text = updatedNickName;

            CacheManager.Instance.UpdateNickName(updatedNickName);

            Debug.Log($"昵称更新为：{updatedNickName}");
        }
        else
        {
            Debug.LogWarning("昵称不可为空");
        }
    }

    /// <summary>
    /// 更新用户头像
    /// </summary>
    /// <param name="icon"></param>
    public void UpdateUserIcon()
    {
        Texture2D icon = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("图片路径: " + path);
            if(path != null)
            {
                try
                {
                    icon = CacheManager.LoadTexture(path).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize);
                    if(icon != null)
                    {
                        _view.BtnUserIcon.image.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));

                        Debug.Log("头像更新成功");

                        CacheManager.Instance.UpdateIcon(icon);
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

        Debug.Log("权限结果：" + permission);
    }

    #endregion Public

    #region Private

    /// <summary>
    /// 呈现视图层中的用户信息
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture = CacheManager.LoadTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    #endregion Private

    #endregion UserInformation

    #region AlbumView

    #region Public

    public void EnterAlbumCreateView()
    {
        MVPFrameWork.UIManager.Instance.Enter(ViewId.AlbumCreateView);
    }

    #endregion Public

    #region Private

    /// <summary>
    /// 渲染相册列表项
    /// </summary>
    /// <param name="albumList"></param>
    private async void PresenterAlbumList(ServerManager.FolderList albumList)
    {
        ClearAlbumList();
        GameObject obj = await Res<GameObject>.LoadAsync(StrDef.ALBUM_ITEM_DATA_PATH);
        foreach(var item in albumList.folders)
        {
            GameObject instantiatedObject = GameObject.Instantiate(obj, _view.GridAlbumContent.transform);
            instantiatedObject.name = item;
            Transform secondChildTransform = instantiatedObject.transform.GetChild(1);
            Image image = secondChildTransform.GetComponent<Image>();
            ServerManager.Instance.GetPhotoAsync(CacheManager.Instance.UserName, item, -1, image);
        }
    }

    /// <summary>
    ///  清空相册列表UI
    /// </summary>
    private void ClearAlbumList()
    {
        for(int i = _view.GridAlbumContent.transform.childCount - 1; i > 0; i--)
        {
            Transform childTransform = _view.GridAlbumContent.transform.GetChild(i);
            GameObject.Destroy(childTransform.gameObject);
        }
    }

    
    #endregion Private

    #endregion AlbumView
}

[Serializable]
public class UserInformation
{
    public string userName;
    public string nickName;
    public string iconPath;
    public bool isSuper;
}