using MVPFrameWork;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
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
        ServerManager.Instance.DownLoadUserIcon_Event += LoadUserInformation;
    }

    /// <summary>
    /// ���ر��ػ�����û���Ϣ
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = null;
       // string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);
        if(CacheManager.Instance.UserInformationCached)
        {
            string json = File.ReadAllText(CacheManager.USER_DATA_FILE);
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

        CacheManager.Instance.ClearUserInformationCache();
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);

    }


    public void UpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(true);
        _view.TxtNickName.gameObject.SetActive(false);
        _view.BtnUpdateNickName.gameObject.SetActive(false);
    }

    public void SureUpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(false);
        _view.TxtNickName.gameObject.SetActive(true);
        _view.BtnUpdateNickName.gameObject.SetActive(true);

        string updatedNickName = _view.InptNickName.text;
        if(updatedNickName != string.Empty)
        {

            _view.TxtNickName.text = updatedNickName;

            CacheManager.Instance.UpdateNickName(updatedNickName);


            Debug.Log($"�ǳƸ���Ϊ��{updatedNickName}");

        }
        else
        {
            Debug.LogWarning("�ǳƲ���Ϊ��");
        }
    }


    public void UpdateUserIcon()
    {
        Texture2D icon = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("ͼƬ·��: " + path);
            if(path != null)
            {
                try
                {
                    icon = CacheManager.LoadIconTexture(path);
                    if(icon != null)
                    {
                        _view.BtnUserIcon.image.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));

                        Debug.Log("ͷ����³ɹ�");

                        CacheManager.Instance.UpdateIcon(icon);
                    }
                    else
                    {
                        Debug.Log($"Cannot load image from {path}");
                    }
                } catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });

        Debug.Log("Ȩ�޽����" + permission);

    }



    /// <summary>
    /// ������ͼ���е��û���Ϣ
    /// </summary>
    /// <param name="userInformation"></param>
    private void PresentUserInformation(UserInformation userInformation)
    {
        _view.TxtUserName.text = userInformation.userName;
        _view.TxtNickName.text = userInformation.nickName;
        Texture2D texture = CacheManager.LoadIconTexture(userInformation.iconPath);
        _view.BtnUserIcon.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }





    #endregion
}

