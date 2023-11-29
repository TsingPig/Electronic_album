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

    public override void OnCreateCompleted()
    {
        LoadUserInformation();
    }

    /// <summary>
    /// ��鱾���Ƿ����û���Ϣ����������Զ���¼
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = null;
        string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);
        if(CacheManager.Instance.UserInformationCached)
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
    /// �û��˳���¼ʱ���ã�����û���Ϣ��ͷ���ļ�
    /// </summary>
    public void ClearUserInformationCache()
    {
        string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // ���ͷ���ļ���
        string iconFolderPath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.ICON_FOLDER);
        if(Directory.Exists(iconFolderPath))
        {
            Directory.Delete(iconFolderPath, true);
        }
        Debug.Log("����û���Ϣ����");
        CacheManager.Instance.UserInformationCached = false;
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);

    }


    /// <summary>
    /// ������ͼ���е��û���Ϣ
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture =  LoadIconTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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

