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
    /// ������ڣ������ж��Ƿ���ڻ�����˺���Ϣ������ֱ���Զ���¼��
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
    /// ��·���м�������
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
    /// ��·�������м����������顣
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
    /// �û���Ϣ�Ƿ񻺴�
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
    /// ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="nickName">�ǳ�</param>
    /// <param name="icon">ͷ����ͼ</param>
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

        // ���浽�����ļ�
        File.WriteAllText(USER_DATA_FILE, json);
        Debug.Log($"������Ϣ���˺ţ�{account}   �ǳƣ�{nickName}   ͼ��{icon.name}   �Ƿ�Ϊ����Ա��{isSuper}");
    }

    /// <summary>
    /// �޸Ļ����е��ǳ�
    /// </summary>
    /// <param name="updateNickName">�µ��ǳ�</param>
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

                Debug.Log($"�ǳ����޸�Ϊ��{updateNickName}");

                //ֱд�����ݿ���
                MySQLManager.Instance.UpdateNickName(UserName, updateNickName);
            }
            else
            {
                Debug.LogError("�û������ļ������ڣ�");
            }
        }
        else
        {
            Debug.LogError("�û���Ϣδ���棬�޷��޸��ǳƣ�");
        }
    }

    /// <summary>
    /// �û��˳���¼ʱ���ã�����û���Ϣ��ͷ���ļ�
    /// </summary>
    public void ClearUserInformationCache()
    {
        if(File.Exists(USER_DATA_FILE))
        {
            File.Delete(USER_DATA_FILE);
        }

        // ���ͷ���ļ���
        if(Directory.Exists(ICON_PATH))
        {
            Directory.Delete(ICON_PATH, true);
        }
        Debug.Log("����û���Ϣ����");
        UserInformationCached = false;
        UserInform = null;
    }

    /// <summary>
    /// ���ֽ�����ʽ����ͷ�񵽱��ء�
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
    /// ����ͷ�񣺻���+������
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
    /// ����Աѡ������
    /// </summary>
    /// <param name="succeedCallback">�ǹ���Ա��ִ�еĺ���</param>
    /// <param name="failedCallback">���ǹ���Ա��ִ�еĻص�</param>
    /// <returns></returns>
    public bool CheckSuper(Action succeedCallback = null, Action failedCallback = null)
    {
        if(UserInform.isSuper)
        {
            succeedCallback?.Invoke();
        }
        else
        {
            Debug.Log($"{UserName} ���ǹ���Ա���޷�ִ�иò���");
            failedCallback?.Invoke();
        }
        return UserInform.isSuper;
    }

    /// <summary>
    ///  ����ͷ�񵽱��أ������ر����·��
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