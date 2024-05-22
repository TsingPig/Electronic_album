using System;
using System.IO;
using TsingPigSDK;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;

public class CacheManager : Singleton<CacheManager>
{
    private UserInformation _userInform;

#if UNITY_EDITOR
    public const string CACHA_PATH = "Assets/Resources/UserInformation";
#else
    public static string CACHA_PATH => Application.persistentDataPath;
#endif

    public static string USER_DATA_FILE => CACHA_PATH + "/userData.json";

    public static string ICON_PATH => CACHA_PATH + "/icons";

    /// <summary>
    /// 程序入口，首先判断是否存在缓存的账号信息。是则直接自动登录。
    /// </summary>
    public void ApplicationEntry()
    {
        string filePath = USER_DATA_FILE;
        if(File.Exists(filePath))
        {
            UIManager.Instance.Enter(ViewId.MainView, new MainModel());

            UserInformationCached = true;
        }
        else
        {
            UIManager.Instance.Enter(ViewId.LoginView);
        }
    }

    /// <summary>
    /// 从路径中加载纹理。
    /// </summary>
    /// <param name="texturePath"></param>
    /// <returns></returns>
    public static Texture2D LoadTexture(string texturePath)
    {
        if(texturePath == null)
        {
            Debug.Log($"{texturePath} is null");
        }
        byte[] fileData = File.ReadAllBytes(texturePath);
        Texture2D texture = new Texture2D(ConstDef.ScaleSize, ConstDef.ScaleSize);
        texture.LoadImage(fileData);
        return texture;
    }

    /// <summary>
    /// 从路径数组中加载纹理数组。
    /// </summary>
    /// <param name="texturePath"></param>
    /// <returns></returns>
    public static Texture2D[] LoadTexture(string[] texturePaths)
    {
        if(texturePaths == null || texturePaths.Length == 0)
        {
            Debug.Log($"{texturePaths} is null or empty");
            return null;
        }
        Texture2D[] texture2Ds = new Texture2D[texturePaths.Length];
        for(int i = 0; i < texturePaths.Length; i++)
        {
            texture2Ds[i] = LoadTexture(texturePaths[i]);
        }
        return texture2Ds;
    }

    /// <summary>
    /// 用户信息是否缓存
    /// </summary>
    public bool UserInformationCached = false;

    public UserInformation UserInform
    {
        get
        {
            if(_userInform == null)
            {
                if(UserInformationCached)
                {
                    string json = File.ReadAllText(USER_DATA_FILE);
                    _userInform = JsonUtility.FromJson<UserInformation>(json);
                }
            }
            return _userInform;
        }
        set
        {
            _userInform = value;
            UserInformUpdateEvent?.Invoke();
        }
    }

    public Func<UserInformation> UserInformUpdateEvent = null;

    public string UserName
    {
        get
        {
            string filePath = USER_DATA_FILE;

            if(File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                UserInformation userData = JsonUtility.FromJson<UserInformation>(json);
                return userData.userName;
            }
            else return "NULL";
        }
    }

    public Texture2D UserIcon
    {
        get
        {
            return LoadTexture(ICON_PATH);
        }
    }

    /// <summary>
    /// 登录时调用，保存用户信息和头像到本地
    /// </summary>
    /// <param name="account">账号</param>
    /// <param name="nickName">昵称</param>
    /// <param name="icon">头像贴图</param>
    public void SaveUserInformation(string account, string nickName, Texture2D icon, bool isSuper)
    {
        UserInformationCached = true;

        UserInformation userData = new UserInformation
        {
            userName = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon),
            isSuper = isSuper
        };

        UserInform = userData;

        string json = JsonUtility.ToJson(userData);

        // 保存到本地文件
        File.WriteAllText(USER_DATA_FILE, json);
        Debug.Log($"缓存信息：账号：{account}   昵称：{nickName}   图像：{icon.name}   是否为管理员：{isSuper}");
    }

    /// <summary>
    /// 修改缓存中的昵称
    /// </summary>
    /// <param name="updateNickName">新的昵称</param>
    public void UpdateNickName(string updateNickName)
    {
        if(UserInformationCached)
        {
            string filePath = USER_DATA_FILE;

            if(File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                UserInformation userData = JsonUtility.FromJson<UserInformation>(json);

                userData.nickName = updateNickName;

                string updateJson = JsonUtility.ToJson(userData);

                UserInform = userData;

                File.WriteAllText(filePath, updateJson);

                Debug.Log($"昵称已修改为：{updateNickName}");

                //直写到数据库中
                MySQLManager.Instance.UpdateNickName(UserName, updateNickName);
            }
            else
            {
                Debug.LogError("用户数据文件不存在！");
            }
        }
        else
        {
            Debug.LogError("用户信息未缓存，无法修改昵称！");
        }
    }

    /// <summary>
    /// 用户退出登录时调用，清除用户信息和头像文件
    /// </summary>
    public void ClearUserInformationCache()
    {
        if(File.Exists(USER_DATA_FILE))
        {
            File.Delete(USER_DATA_FILE);
        }

        // 清除头像文件夹
        if(Directory.Exists(ICON_PATH))
        {
            Directory.Delete(ICON_PATH, true);
        }
        Debug.Log("清除用户信息缓存");
        UserInformationCached = false;
        UserInform = null;
    }

    /// <summary>
    /// 以字节流形式保存头像到本地。
    /// </summary>
    /// <param name="account"></param>
    /// <param name="bytes"></param>
    public void SaveIcon(byte[] bytes)
    {
        if(!Directory.Exists(ICON_PATH))
        {
            Directory.CreateDirectory(ICON_PATH);
        }
        string fileName = Path.Combine(ICON_PATH, UserName + ".jpg");
        File.WriteAllBytes(fileName, bytes);
    }

    /// <summary>
    /// 更新头像：缓存+服务器
    /// </summary>
    /// <param name="updateIcon"></param>
    public void UpdateIcon(Texture2D updateIcon)
    {
        if(!Directory.Exists(ICON_PATH))
        {
            Directory.CreateDirectory(ICON_PATH);
        }

        string fileName = Path.Combine(ICON_PATH, UserName + ".jpg");
        byte[] bytes = updateIcon.EncodeToPNG();
        ServerManager.Instance.UploadUserIcon(UserName, bytes);
        File.WriteAllBytes(fileName, bytes);
    }

    /// <summary>
    /// 管理员选项拦截
    /// </summary>
    /// <param name="succeedCallback">是管理员，执行的函数</param>
    /// <param name="failedCallback">不是管理员，执行的回调</param>
    /// <returns></returns>
    public bool CheckSuper(Action succeedCallback = null, Action failedCallback = null)
    {
        if(UserInform.isSuper)
        {
            succeedCallback?.Invoke();
        }
        else
        {
            Debug.Log($"{UserName} 不是管理员，无法执行该操作");
            failedCallback?.Invoke();
        }
        return UserInform.isSuper;
    }

    /// <summary>
    ///  保存头像到本地，并返回保存的路径
    /// </summary>
    /// <param name="account"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    private string SaveIcon(string account, Texture2D icon)
    {
        if(!Directory.Exists(ICON_PATH))
        {
            Directory.CreateDirectory(ICON_PATH);
        }

        string fileName = Path.Combine(ICON_PATH, account + ".jpg");
        byte[] bytes = icon.EncodeToPNG();

        File.WriteAllBytes(fileName, bytes);

        return fileName;
    }

    private new void Awake()
    {
        base.Awake();
        ApplicationEntry();
    }
}