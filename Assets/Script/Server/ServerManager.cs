using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TsingPigSDK;
using UnityEngine.Networking;
using System.IO;
using System;

public class ServerManager : Singleton<ServerManager>
{
    /// <summary>
    /// 头像下载完成后，更新用户显示
    /// </summary>
    public Func<UserInformation> DownLoadUserIcon_Event;

    /// <summary>
    /// 向服务器上传头像
    /// </summary>
    /// <param name="account"></param>
    /// <param name="usericon"></param>
    public void UploadUserIcon(string account, byte[] usericon)
    {
        StartCoroutine(UploadFile(account, "usericon.jpg", usericon));
    }

    /// <summary>
    /// 从服务器下载头像
    /// </summary>
    /// <param name="account"></param>
    public void DownLoadUserIcon(string account)
    {
        StartCoroutine(DownloadFile(account, "usericon.jpg", CacheManager.Instance.SaveIcon));
    }


    public void CreateAlbum(string account, string albumName, Action<string> callback = null)
    {
        StartCoroutine(CreateAlbumCoroutine(account, albumName, callback));
    }

    IEnumerator CreateAlbumCoroutine(string account, string albumName, Action<string> callback)
    {
        // 创建一个表单数据对象
        WWWForm form = new WWWForm();

        // 将数据作为 JSON 字符串添加到表单
        string jsonData = JsonUtility.ToJson(new { album_name = albumName });
        form.AddField("album_name", albumName);
        form.AddField("json", jsonData, System.Text.Encoding.UTF8);

        UnityWebRequest www = UnityWebRequest.Post($"http://1.12.46.157:80/createAlbum/{account}", form);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Album created successfully");
            callback?.Invoke("Album created successfully");
        }
        else
        {
            Debug.LogError($"Error creating album: {www.error}");
            callback?.Invoke($"Error creating album: {www.error}");
        }
    }

    /// <summary>
    /// 向服务器上传文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    IEnumerator UploadFile(string account, string fileName, byte[] bytes)
    {
        // 创建一个表单数据对象
        WWWForm form = new WWWForm();
        form.AddField("account", account); // 添加账户信息到表单

        // 添加文件数据到表单
        form.AddBinaryData("file", bytes, fileName, "image/jpg");

        using(UnityWebRequest www = UnityWebRequest.Post("http://1.12.46.157/upload", form))
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
    /// 从服务器下载文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator DownloadFile(string account, string fileName, Action<string, byte[]> callback)
    {

        string filePath = RestrictedStringToLettersOrNumbers(account) + "/" + fileName;

        UnityWebRequest www = UnityWebRequest.Get($"http://1.12.46.157:80/download/" + filePath);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = www.downloadHandler.data;

            Debug.Log("文件下载成功");

            callback?.Invoke(account, fileData);
            DownLoadUserIcon_Event?.Invoke();
        }
        else
        {
            Debug.LogError($"网络请求错误: {fileName} " + www.error);
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


}