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
    /// ��鱾���Ƿ����û���Ϣ����������Զ���¼
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
            // ����ͷ��
            PresentUserInformation(userInformation);
        }
        return userInformation;
    }




    /// <summary>
    /// ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="nickName">�ǳ�</param>
    /// <param name="icon">ͷ����ͼ</param>
    public void SaveUserInformation(string account, string nickName, Texture2D icon)
    {
        UserInformation userData = new UserInformation
        {
            userName = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon)
        };

        string json = JsonUtility.ToJson(userData);

        // ���浽�����ļ�
        string filePath = Path.Combine(FilePath, userDataFileName);
        File.WriteAllText(filePath, json);
        Debug.Log($"������Ϣ���˺ţ�{account}   �ǳƣ�{nickName}   ͼ��{icon.name}");
    }

    /// <summary>
    /// ������ͼ���е��û���Ϣ
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
    /// �û��˳���¼ʱ���ã�����û���Ϣ��ͷ���ļ�
    /// </summary>
    public void ClearUserInformationCache()
    {
        string filePath = Path.Combine(FilePath, userDataFileName);
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // ���ͷ���ļ���
        string iconFolderPath = Path.Combine(FilePath, iconFolder);
        if(Directory.Exists(iconFolderPath))
        {
            Directory.Delete(iconFolderPath, true);
        }
        Debug.Log("����û���Ϣ����");
        GameManager.Instance.UserInformationCached = false;
        UIManager.Instance.Quit(ViewId.MainView);
        UIManager.Instance.Enter(ViewId.LoginView);

    }

    /// <summary>
    ///  ����ͷ�񵽱��أ������ر����·��
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
    /// ����ͷ������
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

