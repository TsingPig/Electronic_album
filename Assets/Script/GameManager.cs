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
    [Button("�ϴ�����������")]
    public void UploadTest()
    {
        StartCoroutine(UploadFile());
    }

    IEnumerator UploadFile()
    {
        string filePath = "C:\\Users\\TsingPig\\Desktop\\IDLE\\1.jpg";
        byte[] fileData = File.ReadAllBytes(filePath);

        UnityWebRequest www = UnityWebRequest.Post("http://1.12.46.157/upload", new List<IMultipartFormSection>
        {
            new MultipartFormFileSection("file", fileData, "file.jpg", "image/jpg")
        });
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

    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("���ɣ�" + CacheManager.Instance);
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}