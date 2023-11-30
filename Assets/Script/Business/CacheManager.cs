using System.IO;
using TsingPigSDK;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;


public class CacheManager : Singleton<CacheManager>
{

    //public static string CACHA_PATH => Application.persistentDataPath;
    public const string CACHA_PATH = "Assets/Resources/UserInformation";
    public const string USER_DATA_FILE = "userData.json";
    public const string ICON_FOLDER = "icons";

    /// <summary>
    /// 用户信息是否缓存
    /// </summary>
    public bool UserInformationCached = false;

    /// <summary>
    /// 程序入口，首先判断是否存在缓存的账号信息。是则直接自动登录。
    /// </summary>
    public void ApplicationEntry()
    {
        string filePath = Path.Combine(CACHA_PATH, USER_DATA_FILE);
        if(File.Exists(filePath))
        {
            UIManager.Instance.Enter(ViewId.MainView);

            UserInformationCached = true;
        }
        else
        {
            UIManager.Instance.Enter(ViewId.LoginView);
        }
    }

    /// <summary>
    /// 登录时调用，保存用户信息和头像到本地
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="nickName">昵称</param>
    /// <param name="icon">头像贴图</param>
    public void SaveUserInformation(string account, string nickName, Texture2D icon)
    {
        UserInformationCached = true;

        UserInformation userData = new UserInformation
        {
            userName = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon)
        };

        string json = JsonUtility.ToJson(userData);

        // 保存到本地文件
        string filePath = Path.Combine(CACHA_PATH, USER_DATA_FILE);
        File.WriteAllText(filePath, json);
        Debug.Log($"缓存信息：账号：{account}   昵称：{nickName}   图像：{icon.name}");
    }

    /// <summary>
    ///  保存头像到本地，并返回保存的路径
    /// </summary>
    /// <param name="account"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    private string SaveIcon(string account, Texture2D icon)
    {
        string iconPath = Path.Combine(CACHA_PATH, ICON_FOLDER);
        if(!Directory.Exists(iconPath))
        {
            Directory.CreateDirectory(iconPath);
        }

        iconPath = Path.Combine(iconPath, account + ".png");
        byte[] bytes = icon.EncodeToPNG();
        File.WriteAllBytes(iconPath, bytes);

        return iconPath;
    }

    private new void Awake()
    {
        base.Awake();
        ApplicationEntry();

    }
}