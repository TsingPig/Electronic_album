using MVPFrameWork;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UserInformation
{
    public string account;
    public string nickName;
    public string iconPath;
}

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    private static string FilePath => "Assets/Resources/UserInformation";
    private static string userDataFileName = "userData.json";
    private static string iconFolder = "icons";

    /// <summary>
    /// 检查本地是否有用户信息，如果有则自动登录
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public static bool TryAutoLogin(out UserInformation userData)
    {
        string filePath = Path.Combine(FilePath, userDataFileName);

        if(File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userData = JsonUtility.FromJson<UserInformation>(json);

            // 加载头像
            if(!string.IsNullOrEmpty(userData.iconPath))
            {
                userData.iconPath = Loadicon(userData.iconPath);
            }

            return true;
        }

        userData = null;
        return false;
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <param name="iconPath"></param>
    /// <returns></returns>
    private static string Loadicon(string iconPath)
    {
        if(File.Exists(iconPath))
        {
            return iconPath;
        }

        return null;
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
            account = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon)
        };

        string json = JsonUtility.ToJson(userData);

        // 保存到本地文件
        string filePath = Path.Combine(FilePath, userDataFileName);
        File.WriteAllText(filePath, json);
        Debug.Log($"缓存信息：账号：{account}   昵称：{nickName}   图像：{icon.name}");
    }

    
    // 保存头像到本地，并返回保存的路径
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



    // 用户退出登录时调用，清除用户信息和头像文件
    public void ClearUserData()
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
    }

    // 在加载用户信息时调用，将头像赋值给Button的Image组件
    public void SeticonImage(string iconPath)
    {
        if(_view.BtnUserIcon != null && !string.IsNullOrEmpty(iconPath))
        {
            Texture2D texture = LoadiconTexture(iconPath);
            if(texture != null)
            {
                Image image = _view.BtnUserIcon.GetComponent<Image>();
                if(image != null)
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }

    // 加载头像纹理
    private Texture2D LoadiconTexture(string iconPath)
    {
        if(File.Exists(iconPath))
        {
            byte[] fileData = File.ReadAllBytes(iconPath);
            Texture2D texture = new Texture2D(2, 2); // You may need to adjust the size as per your requirement
            texture.LoadImage(fileData); // LoadImage automatically resizes the texture
            return texture;
        }

        return null;
    }
}
