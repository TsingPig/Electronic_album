using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TsingPigSDK;
using UnityEngine.Networking;
using System.IO;
using System;
using static System.Net.WebRequestMethods;

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

    private string _account = CacheManager.Instance.UserName;

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
    /// 创建空文件夹
    /// </summary>
    /// <param name="account">用户账号</param>
    /// <param name="folderName">相册名</param>
    /// <param name="callback">回调</param>
    /// <returns></returns>
    IEnumerator CreateEmptyFolder(string folderPath, Action<string> callback)
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
    IEnumerator UploadFile(string account, string fileName, byte[] bytes)
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
    /// 从服务器下载文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator DownloadFile(string filePath, Action<byte[]> callback)
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