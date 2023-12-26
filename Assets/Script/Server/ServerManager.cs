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
    /// ����ip
    /// </summary>
    public string host = "http://1.12.46.157";
    public int post = 80;

    /// <summary>
    /// ����url
    /// </summary>
    public string url => $"{host}:{post}";

    private string _account = CacheManager.Instance.UserName;

    /// <summary>
    /// ͷ��������ɺ󣬸����û���ʾ
    /// </summary>
    public Func<UserInformation> DownLoadUserIcon_Event;

    /// <summary>
    /// ��������ϴ�ͷ��
    /// </summary>
    /// <param name="account"></param>
    /// <param name="usericon"></param>
    public void UploadUserIcon(string account, byte[] usericon)
    {
        StartCoroutine(UploadFile(account, "usericon.jpg", usericon));
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
    /// �������ļ���
    /// </summary>
    /// <param name="account">�û��˺�</param>
    /// <param name="folderName">�����</param>
    /// <param name="callback">�ص�</param>
    /// <returns></returns>
    IEnumerator CreateEmptyFolder(string folderPath, Action<string> callback)
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
    IEnumerator UploadFile(string account, string fileName, byte[] bytes)
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
    /// �ӷ����������ļ�
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


}