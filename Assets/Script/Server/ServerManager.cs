using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
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
    public string host = "http://114.132.233.105";

    public int post = 80;

    /// <summary>
    /// 公网url
    /// </summary>
    public string url => $"{host}:{post}";

    /// <summary>
    /// 头像下载完成后，更新用户显示
    /// </summary>
    public Func<UserInformation> DownLoadUserIconEvent;

    /// <summary>
    /// 更新相册列表后回调事件
    /// </summary>
    public Action<FolderList> UpdateAlbumEvent;

    /// <summary>
    /// 动态更新回调事件
    /// </summary>
    public Action UpdateMomentEvent;

    /// <summary>
    /// BBSType更新回调事件
    /// </summary>
    public Action UpdateBBSTypeEvent;

    /// <summary>
    /// 图片缓存
    /// </summary>
    private Dictionary<string, Sprite> _dicPhotoCache = new Dictionary<string, Sprite>();

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
    /// 向服务器上传图像
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photo"></param>
    /// <param name="callback"></param>
    public void UploadPhoto(string account, string albumName, byte[] photo, Action callback = null)
    {
        StartCoroutine(UploadPhotoFile(account, albumName, photo, callback));
    }

    /// <summary>
    /// 向服务器上传多图文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photos"></param>
    /// <param name="callback"></param>
    public void UploadPhotos(string account, string albumName, byte[][] photos, Action callback = null)
    {
        if(photos == null || photos.Length == 0)
        {
            return;
        }
        StartCoroutine(UploadPhotoFiles(account, albumName, photos, callback));
    }

    /// <summary>
    /// 向服务器上传多图文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photoPath"></param>
    /// <param name="callback"></param>
    public void UploadPhotos(string account, string albumName, string[] photoPath, Action callback = null)
    {
        if(photoPath == null || photoPath.Length == 0)
        {
            return;
        }
        StartCoroutine(UploadPhotoFiles(account, albumName, photoPath, callback));
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
    public void CreateAlbumFolder(string account, string folderName, Action callback = null)
    {
        StartCoroutine(CreateEmptyFolder($"{account}/{folderName}", callback));
    }

    /// <summary>
    /// 创建帖子类型
    /// </summary>
    /// <param name="typeName">帖子类型名</param>
    /// <param name="callback"></param>
    public void CreateBBSType(string typeName, Action callback = null)
    {
        StartCoroutine(CreateSection(typeName, UpdateBBSTypeEvent));
    }

    /// <summary>
    /// 获取用户的相册列表
    /// </summary>
    /// <param name="account">用户名</param>
    public void GetAlbumFolder(string account)
    {
        StartCoroutine(GetFolders(account, UpdateAlbumEvent));
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
    /// 异步加载图片
    /// </summary>
    /// <param name="photoUrl">图片远程Url</param>
    /// <param name="image">图片组件</param>
    public async Task GetPhotoAsync(string photoUrl, Image image)
    {
        if(_dicPhotoCache.ContainsKey(photoUrl))
        {
            image.sprite = _dicPhotoCache[photoUrl];
            return;
        }
        using(UnityWebRequest www = UnityWebRequest.Get(photoUrl))
        {
            using(DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true))
            {
                www.downloadHandler = texD1;

                www.SendWebRequest();

                while(!www.isDone)
                {
                    await Task.Yield();
                }

                Debug.Log($"{photoUrl}请求完毕");
                Texture2D photoTex = texD1.texture;
                image.sprite = Sprite.Create(photoTex, new Rect(0, 0, ConstDef.ScaleSize, ConstDef.ScaleSize), new Vector2(0.5f, 0.5f));
                _dicPhotoCache[photoUrl] = image.sprite;
            }
        }
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

                Debug.Log($"{albumName}/{photoId}请求完毕");
                Texture2D photoTex = texD1.texture;
                image.sprite = Sprite.Create(photoTex, new Rect(0, 0, ConstDef.ScaleSize, ConstDef.ScaleSize), new Vector2(0.5f, 0.5f));
            }
        }
    }

    /// <summary>
    /// 请求所有动态数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<IMainModel.Moment>> GetAllPhotoWallItems()
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_moments"))
        {
            Debug.Log($"开始请求动态数据");
            www.SendWebRequest();
            while(!www.isDone)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = www.downloadHandler.text;
                //string jsonResult = $"{{\"moments\":[{{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/0.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/0.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/1.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/0.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/1.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/2.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/2.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/3.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/1.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/3.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/4.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/4.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/5.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/2.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/5.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/6.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/6.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/7.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/3.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/7.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/8.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/8.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/9.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/4.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/9.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/10.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/10.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/11.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/5.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/11.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/12.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/12.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/13.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/6.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/13.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/14.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/14.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/15.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/7.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/15.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\\u597d\\uff0c\\u8fd9\\u6b21\\u4e00\\u5b9a\\u6ca1\\u95ee\\u9898\\uff09\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/16.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/16.jpg\"]}}, {{\"UserName\": \"2\", \"Content\": \"\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\\u732a\\u732a\\u79cd\\u65cf\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/2/Moment/17.jpg\"]}}, {{\"UserName\": \"1231231123123\", \"Content\": \"1111\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1231231123123/Moment/8.jpg\"]}}, {{\"UserName\": \"1\", \"Content\": \"\\u5b63\\u6da6\\u6c11\\u662f\\u732a\", \"PhotoCount\": 1, \"PhotoUrls\": [\"http://1.12.46.157/get_photos/1/Moment/17.jpg\"]}}]\r\n}}";

                IMainModel.MomentsWrapper momentsWrapper = JsonUtility.FromJson<IMainModel.MomentsWrapper>(jsonResult);
                Debug.Log($"动态数据请求成功：{jsonResult}");
                Debug.Log($"动态数据个数：{momentsWrapper.moments.Count}");
                return momentsWrapper.moments;
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
                return null;
            }
        }
    }

    public async Task<List<IMainModel.Moment>> GetBBSPostItems(string sectionName)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_posts_by_section"))
        {
            Debug.Log($"开始请求动态数据");
            www.SendWebRequest();
            while(!www.isDone)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = www.downloadHandler.text;

                IMainModel.MomentsWrapper momentsWrapper = JsonUtility.FromJson<IMainModel.MomentsWrapper>(jsonResult);
                Debug.Log($"动态数据请求成功：{jsonResult}");
                Debug.Log($"动态数据个数：{momentsWrapper.moments.Count}");
                return momentsWrapper.moments;
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
                return null;
            }
        }
    }

    /// <summary>
    /// 异步加载所有分区板块数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<IMainModel.Section>> GetSections()
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_sections"))
        {
            Debug.Log($"开始请求分区数据");
            www.SendWebRequest();
            while(!www.isDone)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = www.downloadHandler.text;

                IMainModel.SectionsWrapper sectionsWrapper = JsonUtility.FromJson<IMainModel.SectionsWrapper>(jsonResult);
                Debug.Log($"板块数据请求成功：{jsonResult}");
                Debug.Log($"板块数据个数：{sectionsWrapper.sections.Count}");

                return sectionsWrapper.sections;
            }
            else
            {
                Debug.LogError($"Error: {www.error}");
                return null;
            }
        }
    }

    /// <summary>
    /// 删除用户相册文件夹
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    public void DeletaAlbumFolder(string account, string albumName)
    {
        StartCoroutine(DeleteFolder(account, albumName, UpdateAlbumEvent));
    }

    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photoIndex">图片实际索引</param>
    /// <param name="callback"></param>
    public void DeletePhoto(string account, string albumName, int photoIndex, Action<int> callback = null)
    {
        StartCoroutine(DeletePhoto($"{account}/{albumName}/{photoIndex}.jpg", callback));
    }

    /// <summary>
    /// 删除BBS论坛板块
    /// </summary>
    /// <param name="bBsTypeName">BBS论坛板块名字</param>
    /// <param name="callback">回调函数</param>
    public void DeleteBBSType(string bBsTypeName, Action callback = null)
    {
        StartCoroutine(DeleteSection(bBsTypeName, callback));
    }

    /// <summary>
    /// 创建照片墙动态
    /// </summary>
    /// <param name="account"></param>
    /// <param name="content"></param>
    /// <param name="photoSize"></param>
    /// <param name="callback"></param>
    public void UploadMomentItem(string account, string content, int photoSize, Action callback = null)
    {
        StartCoroutine(UploadMoment(account, content, photoSize, callback));
    }

    /// <summary>
    /// 创建空文件夹
    /// </summary>
    /// <param name="account">用户账号</param>
    /// <param name="folderName">相册名</param>
    /// <param name="callback">回调</param>
    /// <returns></returns>
    private IEnumerator CreateEmptyFolder(string folderPath, Action callback)
    {
        // 创建一个表单数据对象
        using(UnityWebRequest www = UnityWebRequest.Post($"{url}/createEmptyFolder/{folderPath}", ""))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"文件夹创建成功：{folderPath}");
                callback?.Invoke();
            }
            else
            {
                Debug.LogError($"Error creating album: {www.error}");
                callback?.Invoke();
            }
        }
    }

    /// <summary>
    /// 创建分区
    /// </summary>
    /// <param name="sectionName">分区名</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator CreateSection(string sectionName, Action callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("section_name", sectionName);
        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/create_section", form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Section创建成功：{sectionName}");
                callback?.Invoke();
            }
            else
            {
                Debug.LogError($"Error creating section: {www.error}");
                callback?.Invoke();
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
    /// 上传图像文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="bytes"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadPhotoFile(string account, string albumName, byte[] bytes, Action callback = null)
    {
        // 创建一个表单数据对象
        WWWForm form = new WWWForm();
        form.AddField("account", account); // 添加账户信息到表单
        form.AddField("album_name", albumName); // 添加账户信息到表单

        // 添加文件数据到表单
        form.AddBinaryData("file", bytes, Time.time.ToString() + ".png", "image/png");

        if(bytes == null)
        {
            Debug.LogError("图像错误或图像内容为空");
            yield break;
        }

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
    /// 通过文件路径，上传多图文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photos"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadPhotoFiles(string account, string albumName, string[] photoPath, Action callback = null)
    {
        for(int i = 0; i < photoPath.Length; i++)
        {
            Texture2D tex = CacheManager.LoadTexture(photoPath[i]).Scale(ConstDef.ScaleSize, ConstDef.ScaleSize);
            yield return StartCoroutine(UploadPhotoFile(account, albumName, tex.EncodeToPNG(), callback));
            Debug.Log($"上传进度:{i}/{photoPath.Length}");
        }
    }

    /// <summary>
    /// 通过文件字节数组，上传多图文件
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photos"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadPhotoFiles(string account, string albumName, byte[][] photos, Action callback = null)
    {
        for(int i = 0; i < photos.Length; i++)
        {
            yield return StartCoroutine(UploadPhotoFile(account, albumName, photos[i]));
            Debug.Log($"上传进度:{i}/{photos.Length}");
        }
        callback?.Invoke();
    }

    /// <summary>
    /// 上传动态信息
    /// </summary>
    /// <param name="account"></param>
    /// <param name="content">动态文案</param>
    /// <param name="photoSize">图片数量</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadMoment(string account, string content, int photoSize, Action callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("account", account);
        form.AddField("size", photoSize);
        form.AddField("text", content);

        //form.AddBinaryData("file", bytes, fileName, "image/jpg");

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload_moments", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // 禁用压缩
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Moment uploaded successfully");
                callback?.Invoke();
                UpdateMomentEvent?.Invoke();
            }
            else
            {
                Debug.LogError("Error uploading Moment: " + www.error);
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
            DownLoadUserIconEvent?.Invoke();
        }
        else
        {
            Debug.LogError($"网络请求错误: {url}/download/{filePath} {www.error}");
        }
    }

    [Obsolete]
    private IEnumerator GetMoments(Action<List<IMainModel.Moment>> callback)
    {
        yield return null;
    }

    [Obsolete]
    private IEnumerator GetMoment(Action<IMainModel.Moment> callback = null)
    {
        yield return null;
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
                Debug.LogWarning($"Error delete photo: {www.error}");
            }
        }
    }

    /// <summary>
    /// 删除板块
    /// </summary>
    /// <param name="sectionName">板块名字</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator DeleteSection(string sectionName, Action callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("sectionName", sectionName);


        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/delete_section", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // 禁用压缩
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"成功删除板块 {sectionName}");
                callback?.Invoke();
            }
            else
            {
                Debug.LogError($"删除板块 {sectionName} 失败: " + www.error);
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
        // public Texture2D[] tex2DCovers;
    }
}