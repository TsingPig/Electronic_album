using MVPFrameWork;
using System;
using System.IO;
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


    /// <summary>
    /// 检查本地是否有用户信息，如果有则自动登录
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = null;
        string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);
        if(CacheManager.Instance.UserInformationCached)
        {
            string json = File.ReadAllText(filePath);
            userInformation = JsonUtility.FromJson<UserInformation>(json);
            Debug.Log(json);
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
        string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // 清除头像文件夹
        string iconFolderPath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.ICON_FOLDER);
        if(Directory.Exists(iconFolderPath))
        {
            Directory.Delete(iconFolderPath, true);
        }
        Debug.Log("清除用户信息缓存");
        CacheManager.Instance.UserInformationCached = false;
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);

    }


    /// <summary>
    /// 呈现视图层中的用户信息
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture =  LoadIconTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }


    /// <summary>
    /// 加载头像纹理
    /// </summary>
    /// <param name="iconPath"></param>
    /// <returns></returns>
    private Texture2D LoadIconTexture(string iconPath)
    {
        byte[] fileData = File.ReadAllBytes(iconPath);
        Texture2D texture = new Texture2D(200, 200);
        texture.LoadImage(fileData);
        return texture;
    }


    #endregion
}

