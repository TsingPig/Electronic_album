using System;
using System.Collections;
using System.Threading.Tasks;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerManager : Singleton<ServerManager>
{
    /// <summary>
    /// ����ip
    /// </summary>
    public string host = "http://1.12.46.157";

    public int post = 80;

    /// <summary>
    /// ����url
    /// </summary>
    public string url => $"{host}:{post}";

    /// <summary>
    /// ͷ��������ɺ󣬸����û���ʾ
    /// </summary>
    public Func<UserInformation> DownLoadUserIcon_Event;

    /// <summary>
    /// ��������б��ص��¼�
    /// </summary>
    public Action<FolderList> UpdateAlbum_Event;

    /// <summary>
    /// ��������ϴ�ͷ��
    /// </summary>
    /// <param name="account"></param>
    /// <param name="usericon"></param>
    public void UploadUserIcon(string account, byte[] usericon)
    {
        StartCoroutine(UploadFile(account, "usericon.jpg", usericon));
    }

    public void UploadPhoto(string account, string albumName, byte[] photo, Action callback = null)
    {
        StartCoroutine(UploadPhotoFile(account, albumName, photo, callback));
    }

    /// <summary>
    /// �ӷ���������ͷ��
    /// </summary>
    /// <param name="account"></param>
    public void DownLoadUserIcon(string account)
    {
        string filePath = $"{RestrictedStringToLettersOrNumbers(account)}/usericon.jpg";
        StartCoroutine(DownloadFile(filePath, CacheManager.Instance.SaveIcon));
    }

    /// <summary>
    /// Ϊ�û�����������ļ���
    /// </summary>
    /// <param name="account"></param>
    /// <param name="folderName"></param>
    /// <param name="callback"></param>
    public void CreateAlbumFolder(string account, string folderName, Action<string> callback = null)
    {
        StartCoroutine(CreateEmptyFolder($"{account}/{folderName}", callback));
    }

    /// <summary>
    /// ��ȡ�û�������б�
    /// </summary>
    /// <param name="account">�û���</param>
    public void GetAlbumFolder(string account)
    {
        StartCoroutine(GetFolders(account, UpdateAlbum_Event));
    }

    /// <summary>
    /// ɾ���û���ĳ�����
    /// </summary>
    /// <param name="account">�û���</param>
    /// <param name="albumName">�����</param>
    public void DeletaAlbumFolder(string account, string albumName)
    {
        StartCoroutine(DeleteFolder(account, albumName, UpdateAlbum_Event));
    }

    /// <summary>
    /// ��ȡ�û���������ͼƬ
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    public void GetAlbumSize(string account, string albumName, Action<int> callback = null)
    {
        StartCoroutine(GetConnectSize($"{account}/{albumName}", callback));
    }

    /// <summary>
    /// �첽��������е�ͼƬ
    /// </summary>
    /// <param name="account">�˻�</param>
    /// <param name="albumName">�����</param>
    /// <param name="photoId">��Ƭid</param>
    /// <param name="image">ͼƬ���</param>
    public async void GetPhotoAsync(string account, string albumName, int photoId, Image image)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_photos/{account}/{albumName}/{photoId}.jpg"))
        {
            using(DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true))
            {
                www.downloadHandler = texD1;

                www.SendWebRequest();

                while(!www.isDone)
                {
                    await Task.Yield();
                }

                Debug.Log($"{photoId}�������");
                Texture2D photoTex = texD1.texture;
                image.sprite = Sprite.Create(photoTex, new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public void DeletePhoto(string account, string albumName, int photoIndex, Action<int> callback = null)
    {
        StartCoroutine(DeletePhoto($"{account}/{albumName}/{photoIndex}.jpg", callback));
    }

    /// <summary>
    /// �������ļ���
    /// </summary>
    /// <param name="account">�û��˺�</param>
    /// <param name="folderName">�����</param>
    /// <param name="callback">�ص�</param>
    /// <returns></returns>
    private IEnumerator CreateEmptyFolder(string folderPath, Action<string> callback)
    {
        // ����һ�������ݶ���
        using(UnityWebRequest www = UnityWebRequest.Post($"{url}/createEmptyFolder/{folderPath}", ""))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"�ļ��д����ɹ���{folderPath}");
                callback?.Invoke(www.result.ToString());
            }
            else
            {
                Debug.LogError($"Error creating album: {www.error}");
                callback?.Invoke(www.error);
            }
        }
    }

    /// <summary>
    /// ��������ϴ��ļ�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private IEnumerator UploadFile(string account, string fileName, byte[] bytes)
    {
        // ����һ�������ݶ���
        WWWForm form = new WWWForm();
        form.AddField("account", account); // ����˻���Ϣ����

        // ����ļ����ݵ���
        form.AddBinaryData("file", bytes, fileName, "image/jpg");

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // ����ѹ��
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully");
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="bytes"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadPhotoFile(string account, string albumName, byte[] bytes, Action callback)
    {
        // ����һ�������ݶ���
        WWWForm form = new WWWForm();
        form.AddField("account", account); // ����˻���Ϣ����
        form.AddField("album_name", albumName); // ����˻���Ϣ����

        // ����ļ����ݵ���
        form.AddBinaryData("file", bytes);

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload_photo", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // ����ѹ��
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully");
                callback?.Invoke();
            }
            else
            {
                Debug.LogError("Error uploading file: " + www.error);
            }
        }
    }

    /// <summary>
    /// �ӷ����������ļ�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator DownloadFile(string filePath, Action<byte[]> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{url}/download/{filePath}");

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = www.downloadHandler.data;

            Debug.Log($"�ļ����سɹ���{filePath}");
            callback?.Invoke(fileData);
            DownLoadUserIcon_Event?.Invoke();
        }
        else
        {
            Debug.LogError($"�����������: {url}/download/{filePath} {www.error}");
        }
    }

    /// <summary>
    ///  ���ĳ���ļ���·���µ������ļ���
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    private IEnumerator GetFolders(string folderPath, Action<FolderList> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_folders/{folderPath}"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                FolderList folderList = JsonUtility.FromJson<FolderList>(jsonResponse);

                if(folderList != null && folderList.folders != null)
                {
                    foreach(string folder in folderList.folders)
                    {
                        Debug.Log("Folder Name: " + folder);
                    }
                }

                callback?.Invoke(folderList);

                Debug.Log("folderPath get successfully");
            }
            else
            {
                Debug.LogError("Error get folderPath: " + www.error);
            }
        }
    }

    /// <summary>
    ///  ���ĳ���ļ���·���µ��ļ�����
    /// </summary>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    private IEnumerator GetConnectSize(string folderPath, Action<int> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/connect_size/{folderPath}"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                int connectSize = int.Parse(jsonResponse);
                callback?.Invoke(connectSize);
                Debug.Log($"{folderPath}���ļ�����Ϊ{connectSize}");
            }
            else
            {
                Debug.LogError($"�޷���ȡ{folderPath}�µ��ļ�����: " + www.error);
            }
        }
    }

    /// <summary>
    /// ɾ��ĳ����Ŀ¼�µ����ļ��к�������
    /// </summary>
    /// <param name="folderPath">��Ŀ¼</param>
    /// <param name="folderName">���ļ�����</param>
    /// <param name="callback">�ص�</param>
    /// <returns></returns>
    private IEnumerator DeleteFolder(string folderPath, string folderName, Action<FolderList> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/delete_folder/{folderPath}/{folderName}"))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"���ļ���ɾ���ɹ���{folderName}");
                yield return StartCoroutine(GetFolders($"{folderPath}", callback));
                Debug.Log($"ˢ���ļ����б�");
            }
            else
            {
                Debug.LogError($"Error delete album: {www.error}");
            }
        }
    }

    private IEnumerator DeletePhoto(string photoPath, Action<int> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/delete_photo/{photoPath}"))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"��Ƭɾ���ɹ���{photoPath}");
            }
            else
            {
                Debug.LogError($"Error delete photo: {www.error}");
            }
        }
    }

    /// <summary>
    /// ����ֻ�����Ϸ��ַ�����ĸ/���֣����ַ���
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string RestrictedStringToLettersOrNumbers(string str)
    {
        string restrictedString = string.Empty;
        foreach(char ch in str)
        {
            if(char.IsLetterOrDigit(ch))
            {
                restrictedString += ch;
            }
        }
        return restrictedString;
    }

    private void Init()
    {
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }

    [System.Serializable]
    public class FolderList
    {
        public string[] folders;
    }
}