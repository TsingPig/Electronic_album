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
    /// �û���Ϣ�Ƿ񻺴�
    /// </summary>
    public bool UserInformationCached = false;

    /// <summary>
    /// ������ڣ������ж��Ƿ���ڻ�����˺���Ϣ������ֱ���Զ���¼��
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
        string filePath = Path.Combine(CACHA_PATH, USER_DATA_FILE);
        File.WriteAllText(filePath, json);
        Debug.Log($"������Ϣ���˺ţ�{account}   �ǳƣ�{nickName}   ͼ��{icon.name}");
    }

    /// <summary>
    ///  ����ͷ�񵽱��أ������ر����·��
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