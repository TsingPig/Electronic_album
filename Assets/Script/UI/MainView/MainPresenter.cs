using MVPFrameWork;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UserInformation
{
    public string userName;
    public string nickName;
    public string iconPath;
}

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    #region UserInformation

    public override void OnCreateCompleted()
    {
        LoadUserInformation();
        ServerManager.Instance.DownLoadUserIcon_Event += LoadUserInformation;
        CacheManager.Instance.UserInformUpdate_Event += LoadUserInformation;
    }

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
            PresentUserInformation(userInformation);
        }
        return userInformation;
    }


    /// <summary>
    /// 用户退出登录时调用，清除用户信息和头像文件
    /// </summary>
    public void ClearUserInformationCache()
    {

        CacheManager.Instance.ClearUserInformationCache();
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);

    }


    public void UpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(true);
        _view.TxtNickName.gameObject.SetActive(false);
        _view.BtnUpdateNickName.gameObject.SetActive(false);
    }

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
                } catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });

        Debug.Log("权限结果：" + permission);

    }



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

    #region AlbumView

    public void EnterAlbumCreateView()
    {
        UIManager.Instance.Enter(ViewId.AlbumCreateView);
    }

    #endregion
}

