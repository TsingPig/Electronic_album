using MVPFrameWork;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

[Serializable]
public class UserInformation
{
    public string userName;
    public string nickName;
    public string iconPath;
}

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    public override void OnCreateCompleted()
    {
        ServerManager.Instance.DownLoadUserIcon_Event += LoadUserInformation;
        CacheManager.Instance.UserInformUpdate_Event += LoadUserInformation;
        ServerManager.Instance.UpdateAlbum_Event += PresenterAlbumList;
        LoadUserInformation();

    }

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

        // string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);

        if(CacheManager.Instance.UserInformationCached)
        {
            //string json = File.ReadAllText(CacheManager.USER_DATA_FILE);
            //userInformation = JsonUtility.FromJson<UserInformation>(json);
            //Debug.Log(json);
            // 加载头像

            // 加载用户信息
            PresentUserInformation(userInformation);

            // 异步加载相册列表
            ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);
        }
        return userInformation;
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
                    icon = CacheManager.LoadIconTexture(path);
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

    #endregion

    #region Private
    /// <summary>
    /// 呈现视图层中的用户信息
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture = CacheManager.LoadIconTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    #endregion

    #endregion

    #region AlbumView

    #region Public
    public void EnterAlbumCreateView()
    {
        MVPFrameWork.UIManager.Instance.Enter(ViewId.AlbumCreateView);
    }



    #endregion

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

    #endregion


    #endregion
}

