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
    /// ����ip
    /// </summary>
    public string host = "http://114.132.233.105";

    public int post = 80;

    /// <summary>
    /// ����url
    /// </summary>
    public string url => $"{host}:{post}";

    /// <summary>
    /// ͷ��������ɺ󣬸����û���ʾ
    /// </summary>
    public Func<UserInformation> DownLoadUserIconEvent;

    /// <summary>
    /// ��������б��ص��¼�
    /// </summary>
    public Action<FolderList> UpdateAlbumEvent;

    /// <summary>
    /// ��̬���»ص��¼�
    /// </summary>
    public Action UpdateMomentEvent;

    /// <summary>
    /// BBSType���»ص��¼�
    /// </summary>
    public Action UpdateBBSTypeEvent;

    /// <summary>
    /// ͼƬ����
    /// </summary>
    private Dictionary<string, Sprite> _dicPhotoCache = new Dictionary<string, Sprite>();

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
    /// ��������ϴ�ͼ��
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
    /// ��������ϴ���ͼ�ļ�
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
    /// ��������ϴ���ͼ�ļ�
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
    public void CreateAlbumFolder(string account, string folderName, Action callback = null)
    {
        StartCoroutine(CreateEmptyFolder($"{account}/{folderName}", callback));
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="typeName">����������</param>
    /// <param name="callback"></param>
    public void CreateBBSType(string typeName, Action callback = null)
    {
        StartCoroutine(CreateSection(typeName, UpdateBBSTypeEvent));
    }

    /// <summary>
    /// ��ȡ�û�������б�
    /// </summary>
    /// <param name="account">�û���</param>
    public void GetAlbumFolder(string account)
    {
        StartCoroutine(GetFolders(account, UpdateAlbumEvent));
    }

    /// <summary>
    /// ��ȡ�û���������ͼƬ
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    public void GetAlbumSize(string account, string albumName, Action<int> callback = null)
    {
        StartCoroutine(GetConnectSize($"{account}/{albumName}", callback));
    }

    /// <summary>
    /// �첽����ͼƬ
    /// </summary>
    /// <param name="photoUrl">ͼƬԶ��Url</param>
    /// <param name="image">ͼƬ���</param>
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

                Debug.Log($"{photoUrl}�������");
                Texture2D photoTex = texD1.texture;
                image.sprite = Sprite.Create(photoTex, new Rect(0, 0, ConstDef.ScaleSize, ConstDef.ScaleSize), new Vector2(0.5f, 0.5f));
                _dicPhotoCache[photoUrl] = image.sprite;
            }
        }
    }

    /// <summary>
    /// �첽��������е�ͼƬ
    /// </summary>
    /// <param name="account">�˻�</param>
    /// <param name="albumName">�����</param>
    /// <param name="photoId">��Ƭid</param>
    /// <param name="image">ͼƬ���</param>
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

                Debug.Log($"{albumName}/{photoId}�������");
                Texture2D photoTex = texD1.texture;
                image.sprite = Sprite.Create(photoTex, new Rect(0, 0, ConstDef.ScaleSize, ConstDef.ScaleSize), new Vector2(0.5f, 0.5f));
            }
        }
    }

    /// <summary>
    /// �������ж�̬����
    /// </summary>
    /// <returns></returns>
    public async Task<List<IMainModel.Moment>> GetAllPhotoWallItems()
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_moments"))
        {
            Debug.Log($"��ʼ����̬����");
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
                Debug.Log($"��̬��������ɹ���{jsonResult}");
                Debug.Log($"��̬���ݸ�����{momentsWrapper.moments.Count}");
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
            Debug.Log($"��ʼ����̬����");
            www.SendWebRequest();
            while(!www.isDone)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = www.downloadHandler.text;

                IMainModel.MomentsWrapper momentsWrapper = JsonUtility.FromJson<IMainModel.MomentsWrapper>(jsonResult);
                Debug.Log($"��̬��������ɹ���{jsonResult}");
                Debug.Log($"��̬���ݸ�����{momentsWrapper.moments.Count}");
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
    /// �첽�������з����������
    /// </summary>
    /// <returns></returns>
    public async Task<List<IMainModel.Section>> GetSections()
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/get_sections"))
        {
            Debug.Log($"��ʼ�����������");
            www.SendWebRequest();
            while(!www.isDone)
            {
                await Task.Yield();
            }

            if(www.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = www.downloadHandler.text;

                IMainModel.SectionsWrapper sectionsWrapper = JsonUtility.FromJson<IMainModel.SectionsWrapper>(jsonResult);
                Debug.Log($"�����������ɹ���{jsonResult}");
                Debug.Log($"������ݸ�����{sectionsWrapper.sections.Count}");

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
    /// ɾ���û�����ļ���
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    public void DeletaAlbumFolder(string account, string albumName)
    {
        StartCoroutine(DeleteFolder(account, albumName, UpdateAlbumEvent));
    }

    /// <summary>
    /// ɾ��ͼƬ
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="photoIndex">ͼƬʵ������</param>
    /// <param name="callback"></param>
    public void DeletePhoto(string account, string albumName, int photoIndex, Action<int> callback = null)
    {
        StartCoroutine(DeletePhoto($"{account}/{albumName}/{photoIndex}.jpg", callback));
    }

    /// <summary>
    /// ɾ��BBS��̳���
    /// </summary>
    /// <param name="bBsTypeName">BBS��̳�������</param>
    /// <param name="callback">�ص�����</param>
    public void DeleteBBSType(string bBsTypeName, Action callback = null)
    {
        StartCoroutine(DeleteSection(bBsTypeName, callback));
    }

    /// <summary>
    /// ������Ƭǽ��̬
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
    /// �������ļ���
    /// </summary>
    /// <param name="account">�û��˺�</param>
    /// <param name="folderName">�����</param>
    /// <param name="callback">�ص�</param>
    /// <returns></returns>
    private IEnumerator CreateEmptyFolder(string folderPath, Action callback)
    {
        // ����һ�������ݶ���
        using(UnityWebRequest www = UnityWebRequest.Post($"{url}/createEmptyFolder/{folderPath}", ""))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"�ļ��д����ɹ���{folderPath}");
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
    /// ��������
    /// </summary>
    /// <param name="sectionName">������</param>
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
                Debug.Log($"Section�����ɹ���{sectionName}");
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
    /// ��������ϴ��ļ�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="fileName"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private IEnumerator UploadFile(string account, string fileName, byte[] bytes)
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
    /// �ϴ�ͼ���ļ�
    /// </summary>
    /// <param name="account"></param>
    /// <param name="albumName"></param>
    /// <param name="bytes"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator UploadPhotoFile(string account, string albumName, byte[] bytes, Action callback = null)
    {
        // ����һ�������ݶ���
        WWWForm form = new WWWForm();
        form.AddField("account", account); // ����˻���Ϣ����
        form.AddField("album_name", albumName); // ����˻���Ϣ����

        // ����ļ����ݵ���
        form.AddBinaryData("file", bytes, Time.time.ToString() + ".png", "image/png");

        if(bytes == null)
        {
            Debug.LogError("ͼ������ͼ������Ϊ��");
            yield break;
        }

        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/upload_photo", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // ����ѹ��
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
    /// ͨ���ļ�·�����ϴ���ͼ�ļ�
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
            Debug.Log($"�ϴ�����:{i}/{photoPath.Length}");
        }
    }

    /// <summary>
    /// ͨ���ļ��ֽ����飬�ϴ���ͼ�ļ�
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
            Debug.Log($"�ϴ�����:{i}/{photos.Length}");
        }
        callback?.Invoke();
    }

    /// <summary>
    /// �ϴ���̬��Ϣ
    /// </summary>
    /// <param name="account"></param>
    /// <param name="content">��̬�İ�</param>
    /// <param name="photoSize">ͼƬ����</param>
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
            www.downloadHandler = new DownloadHandlerBuffer(); // ����ѹ��
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
    /// �ӷ����������ļ�
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

            Debug.Log($"�ļ����سɹ���{filePath}");
            callback?.Invoke(fileData);
            DownLoadUserIconEvent?.Invoke();
        }
        else
        {
            Debug.LogError($"�����������: {url}/download/{filePath} {www.error}");
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
    ///  ���ĳ���ļ���·���µ������ļ���
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
    ///  ���ĳ���ļ���·���µ��ļ�����
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
                Debug.Log($"{folderPath}���ļ�����Ϊ{connectSize}");
            }
            else
            {
                Debug.LogError($"�޷���ȡ{folderPath}�µ��ļ�����: " + www.error);
            }
        }
    }

    /// <summary>
    /// ɾ��ĳ����Ŀ¼�µ����ļ��к�������
    /// </summary>
    /// <param name="folderPath">��Ŀ¼</param>
    /// <param name="folderName">���ļ�����</param>
    /// <param name="callback">�ص�</param>
    /// <returns></returns>
    private IEnumerator DeleteFolder(string folderPath, string folderName, Action<FolderList> callback = null)
    {
        using(UnityWebRequest www = UnityWebRequest.Get($"{url}/delete_folder/{folderPath}/{folderName}"))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"���ļ���ɾ���ɹ���{folderName}");
                yield return StartCoroutine(GetFolders($"{folderPath}", callback));
                Debug.Log($"ˢ���ļ����б�");
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
                Debug.Log($"��Ƭɾ���ɹ���{photoPath}");
            }
            else
            {
                Debug.LogWarning($"Error delete photo: {www.error}");
            }
        }
    }

    /// <summary>
    /// ɾ�����
    /// </summary>
    /// <param name="sectionName">�������</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator DeleteSection(string sectionName, Action callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("sectionName", sectionName);


        using(UnityWebRequest www = UnityWebRequest.Post($"{host}/delete_section", form))
        {
            www.downloadHandler = new DownloadHandlerBuffer(); // ����ѹ��
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"�ɹ�ɾ����� {sectionName}");
                callback?.Invoke();
            }
            else
            {
                Debug.LogError($"ɾ����� {sectionName} ʧ��: " + www.error);
            }
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

    [System.Serializable]
    public class FolderList
    {
        public string[] folders;
        // public Texture2D[] tex2DCovers;
    }
}