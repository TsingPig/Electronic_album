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
    /// 公网ip
    /// </summary>
    public string host = "http://1.12.46.157";

    public int post = 80;

    /// <summary>
    /// 公网url
    /// </summary>
    public string url => $"{host}:{post}";

    /// <summary>
    /// 头像下载完成后，更新用户显示
    /// </summary>
    public Func<UserInformation> DownLoadUserIcon_Event;

    /// <summary>
    /// 更新相册列表后回调事件
    /// </summary>
    public Action<FolderList> UpdateAlbum_Event;

    /// <summary>
    /// 向服务器上传头像
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
    /// 从服务器下载头像
    /// </summary>
    /// <param name="account"></param>
    public void DownLoadUserIcon(string account)
    {
        string filePath = $"{RestrictedStringToLettersOrNumbers(account)}/usericon.jpg";
        StartCoroutine(DownloadFile(filePath, CacheManager.Instance.SaveIcon));
    }

    /// <summary>
    /// 为用户创建空相册文件夹
    /// </summary>
    /// <param name="account"></param>
    /// <param name="folderName"></param>
    /// <param name="callback"></param>
    public void CreateAlbumFolder(string account, string folderName, Action<string> callback = null)
    {
        StartCoroutine(CreateEmptyFolder($"{account}/{folderName}", callback));
    }

    /// <summary>
    /// 获取用户的相册列表
    /// </summary>
    /// <param name="account">用户名</param>
    public void GetAlbumFolder(string account)
    {
        StartCoroutine(GetFolders(account, UpdateAlbum_Event));
    }

    /// <summary>
    /// 删除用户的某个相册
    /// </summary>
    /// <param name="account">用户名</param>
    /// <param name="albumName">相册名</param>
    public void DeletaAlbumFolder(string account, string albumName)
    {
        StartCoroutine(DeleteFolder(account, albumName, UpdateAlbum_Event));
    }

    /// <summary>
    /// 获取用户相册的所有图片
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    public void GetAlbumSize(string account, string albumName, Action<int> callback = null)
    {
        StartCoroutine(GetConnectSize($"{account}/{albumName}", callback));
    }

    /// <summary>
    /// 异步加载相册中的图片
    /// </summary>
    /// <param name="account">账户</param>
    /// <param name="albumName">相册名</param>
    /// <param name="photoId">照片id</param>
    /// <param name="image">图片组件</param>
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

                Debug.Log($"{photoId}请求完毕");
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
    /// 创建空文件夹
    /// </summary>
    /// <param name="account">用户账号</param>
    /// <param name="folderName">相册名</param>
    /// <param name="callback">回调</param>
    /// <returns></returns>
    private IEnumerator CreateEmptyFolder(string folderPath, Action<string> callback)
    {
        // 创建一个表单数据对象
        using(UnityWebRequest www = UnityWebRequest.Post($"{url}/createEmptyFolder/{folderPath}", ""))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"文件夹创建成功：{folderPath}");
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
    /// 向服务器上传文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private IEnumerator UploadFile(string account, string fileName, byte[] bytes)
    {
        // 创建一个表单数据对象
        WWWForm form = new WWWForm();
        form.AddField("account", account); // 添加账户信息到表单

        // 添加文件数据到表单
        form.AddBinaryData("file", bytes, fileName, "image/jpg");

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // 禁用压缩
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
        // 创建一个表单数据对象
        WWWForm form = new WWWForm();
        form.AddField("account", account); // 添加账户信息到表单
        form.AddField("album_name", albumName); // 添加账户信息到表单

        // 添加文件数据到表单
        form.AddBinaryData("file", bytes);

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload_photo", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // 禁用压缩
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
    /// 从服务器下载文件
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

            Debug.Log($"文件下载成功：{filePath}");
            callback?.Invoke(fileData);
            DownLoadUserIcon_Event?.Invoke();
        }
        else
        {
            Debug.LogError($"网络请求错误: {url}/download/{filePath} {www.error}");
        }
    }

    /// <summary>
    ///  获得某个文件夹路径下的所有文件夹
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
    ///  获得某个文件夹路径下的文件数量
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
                Debug.Log($"{folderPath}：文件数量为{connectSize}");
            }
            else
            {
                Debug.LogError($"无法获取{folderPath}下的文件数量: " + www.error);
            }
        }
    }

    /// <summary>
    /// 删除某个父目录下的子文件夹和其内容
    /// </summary>
    /// <param name="folderPath">父目录</param>
    /// <param name="folderName">子文件夹名</param>
    /// <param name="callback">回调</param>
    /// <returns></returns>
    private IEnumerator DeleteFolder(string folderPath, string folderName, Action<FolderList> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/delete_folder/{folderPath}/{folderName}"))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"子文件夹删除成功：{folderName}");
                yield return StartCoroutine(GetFolders($"{folderPath}", callback));
                Debug.Log($"刷新文件夹列表");
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
                Debug.Log($"照片删除成功：{photoPath}");
            }
            else
            {
                Debug.LogError($"Error delete photo: {www.error}");
            }
        }
    }

    /// <summary>
    /// 返回只包含合法字符（字母/数字）的字符串
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