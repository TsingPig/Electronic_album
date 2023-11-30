using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager>
{
    public string filename;

    [Button("上传服务器测试")]
    public void UploadTest()
    {
        StartCoroutine(UploadFile());
    }

    [Button("下载测试")]

    public void DownLoadTest()
    {
        StartCoroutine(DownloadFile());
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



    IEnumerator DownloadFile()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://1.12.46.157:80/download/" + filename);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.Success)
        {
            byte[] fileData = www.downloadHandler.data;
            Debug.Log("File downloaded successfully");

            string savePath = "Assets/Resources/file.jpg";
            File.WriteAllBytes(savePath, fileData);
        }
        else
        {
            Debug.LogError("Error downloading file: " + www.error);
        }
    }


    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("生成：" + CacheManager.Instance);
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}