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
    private string FilePath => "Assets/Resources/UserInformation";
    private const string userDataFileName = "userData.json";
    private const string iconFolder = "icons";

    // ��¼ʱ���ã������û���Ϣ��ͷ�񵽱���
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

    // ��鱾���Ƿ����û���Ϣ����������Զ���¼
    public bool TryAutoLogin(out UserInformation userData)
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

    // ����ͷ��
    private string Loadicon(string iconPath)
    {
        if(File.Exists(iconPath))
        {
            return iconPath;
        }

        return null;
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
