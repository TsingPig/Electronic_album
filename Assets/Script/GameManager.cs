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


    [Button("PickImage")]
    public void PickImage()
    {
        int maxSize = 1000;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if(path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if(texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if(!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                Destroy(texture, 5f);
            }
        });

        Debug.Log("Permission result: " + permission);
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