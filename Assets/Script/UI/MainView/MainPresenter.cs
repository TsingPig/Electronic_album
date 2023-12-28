using MVPFrameWork;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

[Serializable]
public class UserInformation
{
    public string userName;
    public string nickName;
    public string iconPath;
}

public class MainPresenter : PresenterBase<IMainView>, IMainPresenter
{
    public override void OnCreateCompleted()
    {
        ServerManager.Instance.DownLoadUserIcon_Event += LoadUserInformation;
        CacheManager.Instance.UserInformUpdate_Event += LoadUserInformation;
        ServerManager.Instance.UpdateAlbum_Event += PresenterAlbumList;
        LoadUserInformation();

    }

    #region UserInformation

    #region Public

    /// <summary>
    /// ���ر��ػ�����û���Ϣ
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public UserInformation LoadUserInformation()
    {
        UserInformation userInformation = CacheManager.Instance.UserInform;

        // string filePath = Path.Combine(CacheManager.CACHA_PATH, CacheManager.USER_DATA_FILE);

        if(CacheManager.Instance.UserInformationCached)
        {
            //string json = File.ReadAllText(CacheManager.USER_DATA_FILE);
            //userInformation = JsonUtility.FromJson<UserInformation>(json);
            //Debug.Log(json);
            // ����ͷ��

            // �����û���Ϣ
            PresentUserInformation(userInformation);

            // �첽��������б�
            ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);
        }
        return userInformation;
    }


    /// <summary>
    /// �û��˳���¼ʱ���ã�����û���Ϣ��ͷ���ļ�
    /// </summary>
    public void ClearUserInformationCache()
    {

        ClearAlbumList();
        CacheManager.Instance.ClearUserInformationCache();
        MVPFrameWork.UIManager.Instance.Quit(ViewId.MainView);
        MVPFrameWork.UIManager.Instance.Enter(ViewId.LoginView);

    }

    /// <summary>
    /// �����ǳ�
    /// </summary>
    public void UpdateNickName()
    {
        _view.InptNickName.gameObject.SetActive(true);
        _view.TxtNickName.gameObject.SetActive(false);
        _view.BtnUpdateNickName.gameObject.SetActive(false);
    }

    /// <summary>
    /// ȷ�ϸ���
    /// </summary>
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

    /// <summary>
    /// �����û�ͷ��
    /// </summary>
    /// <param name="icon"></param>
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
                }
                catch(Exception e)
                {
                    Debug.LogError($"Error loading image: {e.Message}");
                }
            }
        });

        Debug.Log("Ȩ�޽����" + permission);

    }

    #endregion

    #region Private
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

    #endregion

    #region AlbumView

    #region Public
    public void EnterAlbumCreateView()
    {
        MVPFrameWork.UIManager.Instance.Enter(ViewId.AlbumCreateView);
    }



    #endregion

    #region Private

    /// <summary>
    /// ��Ⱦ����б���
    /// </summary>
    /// <param name="albumList"></param>
    private async void PresenterAlbumList(ServerManager.FolderList albumList)
    {
        ClearAlbumList();
        GameObject obj = await Res<GameObject>.LoadAsync(StrDef.ALBUM_ITEM_DATA_PATH);
        foreach(var item in albumList.folders)
        {
            GameObject instantiatedObject = GameObject.Instantiate(obj, _view.GridAlbumContent.transform);
            instantiatedObject.name = item;

        }
    }


    /// <summary>
    ///  �������б�UI
    /// </summary>
    private void ClearAlbumList()
    {
        for(int i = _view.GridAlbumContent.transform.childCount - 1; i > 0; i--)
        {
            Transform childTransform = _view.GridAlbumContent.transform.GetChild(i);
            GameObject.Destroy(childTransform.gameObject);
        }
    }

    #endregion


    #endregion
}

