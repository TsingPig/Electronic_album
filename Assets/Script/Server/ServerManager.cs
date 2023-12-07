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


    public void UploadTest()
    {
        StartCoroutine(UploadFile());
    }

    public void DownLoadUserIcon(string account)
    {
        StartCoroutine(DownloadFile(account, CacheManager.Instance.SaveIcon));
    }


    IEnumerator UploadFile()
    {
        string filePath = "C:\\Users\\TsingPig\\Desktop\\IDLE\\1.jpg";
        byte[] fileData = File.ReadAllBytes(filePath);

        UnityWebRequest www = UnityWebRequest.Post("http://1.12.46.157/upload", new List<IMultipartFormSection>
        {
            new MultipartFormFileSection("file", fileData, "file.jpg", "image/jpg")
        });
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


    IEnumerator DownloadFile(string account, Action<string, byte[]> callback)
    {

        string fileName = RestrictedStringToLettersOrNumbers(account) + "/usericon.jpg";

        UnityWebRequest www = UnityWebRequest.Get($"http://1.12.46.157:80/download/" + fileName);

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