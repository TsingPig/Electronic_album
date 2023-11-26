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
    private static string FilePath => "Assets/Resources/UserInformation";
    private static string userDataFileName = "userData.json";
    private static string iconFolder = "icons";


    /// <summary>
    /// 检查本地是否有用户信息，如果有则自动登录
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = null;
        string filePath = Path.Combine("Assets/Resources/UserInformation", "userData.json");
        if(GameManager.Instance.UserInformationCached)
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
    /// 登录时调用，保存用户信息和头像到本地
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="nickName">昵称</param>
    /// <param name="icon">头像贴图</param>
    public void SaveUserInformation(string account, string nickName, Texture2D icon)
    {
        UserInformation userData = new UserInformation
        {
            userName = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon)
        };

        string json = JsonUtility.ToJson(userData);

        // 保存到本地文件
        string filePath = Path.Combine(FilePath, userDataFileName);
        File.WriteAllText(filePath, json);
        Debug.Log($"缓存信息：账号：{account}   昵称：{nickName}   图像：{icon.name}");
    }

    /// <summary>
    /// 呈现视图层中的用户信息
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture = LoadIconTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }


    /// <summary>
    /// 用户退出登录时调用，清除用户信息和头像文件
    /// </summary>
    public void ClearUserInformationCache()
    {
        string filePath = Path.Combine(FilePath, userDataFileName);
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // 清除头像文件夹
        string iconFolderPath = Path.Combine(FilePath, iconFolder);
        if(Directory.Exists(iconFolderPath))
        {
            Directory.Delete(iconFolderPath, true);
        }
        Debug.Log("清除用户信息缓存");
        GameManager.Instance.UserInformationCached = false;
        UIManager.Instance.Quit(ViewId.MainView);
        UIManager.Instance.Enter(ViewId.LoginView);

    }

    /// <summary>
    ///  保存头像到本地，并返回保存的路径
    /// </summary>
    /// <param name="account"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    private string SaveIcon(string account, Texture2D icon)
    {
        string iconPath = Path.Combine(FilePath, iconFolder);
        if(!Directory.Exists(iconPath))
        {
            Directory.CreateDirectory(iconPath);
        }

        iconPath = Path.Combine(iconPath, account + ".png");
        byte[] bytes = icon.EncodeToPNG();
        File.WriteAllBytes(iconPath, bytes);

        return iconPath;
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

