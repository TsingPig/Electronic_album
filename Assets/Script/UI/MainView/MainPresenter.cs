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
    /// ��鱾���Ƿ����û���Ϣ����������Զ���¼
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

            // ����ͷ��
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
    /// ����ͷ��
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
    /// ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
    /// </summary>
    /// <param name="account">�˺�</param>
    /// <param name="nickName">�ǳ�</param>
    /// <param name="icon">ͷ����ͼ</param>
    public void SaveUserInformation(string account, string nickName, Texture2D icon)
    {
        UserInformation userData = new UserInformation
        {
            account = account,
            nickName = nickName,
            iconPath = SaveIcon(account, icon)
        };

        string json = JsonUtility.ToJson(userData);

        // ���浽�����ļ�
        string filePath = Path.Combine(FilePath, userDataFileName);
        File.WriteAllText(filePath, json);
        Debug.Log($"������Ϣ���˺ţ�{account}   �ǳƣ�{nickName}   ͼ��{icon.name}");
    }

    
    // ����ͷ�񵽱��أ������ر����·��
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



    // �û��˳���¼ʱ���ã�����û���Ϣ��ͷ���ļ�
    public void ClearUserData()
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
    }

    // �ڼ����û���Ϣʱ���ã���ͷ��ֵ��Button��Image���
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

    // ����ͷ������
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
