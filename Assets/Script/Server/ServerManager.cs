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
        StartCoroutine(DownloadFile(account, "usericon.jpg", CacheManager.Instance.SaveIcon));
    }


    public void CreateAlbum(string account, string albumName, Action<string> callback = null)
    {
        StartCoroutine(CreateAlbumCoroutine(account, albumName, callback));
    }

    IEnumerator CreateAlbumCoroutine(string account, string albumName, Action<string> callback)
    {
        // ����һ�������ݶ���
        WWWForm form = new WWWForm();

        // ��������Ϊ JSON �ַ�����ӵ���
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

        using(UnityWebRequest www = UnityWebRequest.Post("http://1.12.46.157/upload", form))
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
    IEnumerator DownloadFile(string account, string fileName, Action<string, byte[]> callback)
    {

        string filePath = RestrictedStringToLettersOrNumbers(account) + "/" + fileName;

        UnityWebRequest www = UnityWebRequest.Get($"http://1.12.46.157:80/download/" + filePath);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = www.downloadHandler.data;

            Debug.Log("�ļ����سɹ�");

            callback?.Invoke(account, fileData);
            DownLoadUserIcon_Event?.Invoke();
        }
        else
        {
            Debug.LogError($"�����������: {fileName} " + www.error);
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