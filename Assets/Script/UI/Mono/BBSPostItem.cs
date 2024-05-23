using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using UIManager = MVPFrameWork.UIManager;

/// <summary>
/// 每个帖子的项
/// </summary>
public class BBSPostItem : MonoBehaviour
{
    public Button BtnEnterPost;

    public TMP_Text UserName;
    public TMP_Text Title;
    public TMP_Text Content;

    public GridLayoutGroup PostPhotoItemRoot;

    
    [HideInInspector]
    public List<string> PhotoUrls;

    [HideInInspector]
    public IBBSModel.Post Post;

    private void Start()
    {
        BtnEnterPost.onClick.AddListener(() =>
        {
            UIManager.Instance.Enter(ViewId.PostView, new PostModel() { Post = Post });
        });
    }

    /// <summary>
    /// 加载帖子照片项
    /// </summary>
    /// <param name="addressablePath">指定是BBSPostPhotoItem / PostPhotoItem</param>
    /// <returns></returns>
    public async Task LoadPostPhotoItem(string addressablePath)
    {
        foreach(string photoUrl in PhotoUrls)
        {
            GameObject bBsPostPhotoItemObj = await Instantiater.InstantiateAsync(addressablePath, PostPhotoItemRoot.transform);
            MomentPhotoItem postPhotoItem = bBsPostPhotoItemObj.GetComponent<MomentPhotoItem>();
            postPhotoItem.PhotoUrl = photoUrl;
            postPhotoItem.GetPhotoAsync();
        }
    }
}