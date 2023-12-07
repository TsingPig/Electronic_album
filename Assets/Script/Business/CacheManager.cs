using System.Drawing;
using System.IO;
using System.Security.Principal;
using TsingPigSDK;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;


public class CacheManager : Singleton<CacheManager>
{

    //public static string CACHA_PATH => Application.persistentDataPath;
    public const string CACHA_PATH = "Assets/Resources/UserInformation";
    public static string USER_DATA_FILE => CACHA_PATH + "/userData.json";
    public static string ICON_PATH => CACHA_PATH + "/icons";

    /// <summary>
    /// ����ͷ������
    /// </summary>
    /// <param name="iconPath"></param>
    /// <returns></returns>
    public static Texture2D LoadIconTexture(string iconPath)
    {
        byte[] fileData = File.ReadAllBytes(iconPath);
        Texture2D texture = new Texture2D(200, 200);
        texture.LoadImage(fileData);
        return texture;
    }

    /// <summary>
    /// �û���Ϣ�Ƿ񻺴�
    /// </summary>
    public bool UserInformationCached = false;

    private string UserName
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
            return LoadIconTexture(ICON_PATH);
        }
    }


    /// <summary>
    /// ������ڣ������ж��Ƿ���ڻ�����˺���Ϣ������ֱ���Զ���¼��
    /// </summary>
    public void ApplicationEntry()
    {
        string filePath = USER_DATA_FILE;
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
    /// ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="nickName">�ǳ�</param>
    /// <param name="icon">ͷ����ͼ</param>
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

        // ���浽�����ļ�
        File.WriteAllText(USER_DATA_FILE, json);
        Debug.Log($"������Ϣ���˺ţ�{account}   �ǳƣ�{nickName}   ͼ��{icon.name}");
    }


    /// <summary>
    /// ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="nickName">�ǳ�</param>
    /// <param name="icon">ͷ����ͼ</param>
    public void SaveUserInformation(string account, string nickName)
    {
        UserInformationCached = true;

        UserInformation userData = new UserInformation
        {
            userName = account,
            nickName = nickName,
            iconPath = Path.Combine(ICON_PATH, account + ".jpg")
        };

        string json = JsonUtility.ToJson(userData);
        // ���浽�����ļ�
        File.WriteAllText(USER_DATA_FILE, json);
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


    }


    /// <summary>
    /// ���ֽ�����ʽ����ͷ�񵽱��ء�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="bytes"></param>
    public void SaveIcon(string account, byte[] bytes)
    {
        if(!Directory.Exists(ICON_PATH))
        {
            Directory.CreateDirectory(ICON_PATH);
        }
        string fileName = Path.Combine(ICON_PATH, account + ".jpg");
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