using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using UIManager = MVPFrameWork.UIManager;

/// <summary>
/// ÿ�����ӵ���
/// </summary>
public class BBSPostItem : MonoBehaviour
{
    /// <summary>
    /// �Ƿ������ӿհ״����Խ���������ҳ�档ֻ����BBSViewҳ�����ҪΪtrue
    /// </summary>
    [HideInInspector]
    public bool AllowEnterPostView = true;

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
        if(AllowEnterPostView)
        {
            BtnEnterPost.onClick.AddListener(() =>
            {
                UIManager.Instance.Enter(ViewId.PostView, new PostModel() { Post = Post });
            });
        }
        else
        {
            BtnEnterPost.gameObject.SetActive(false);
        }
    }


    public async Task LoadPostItems()
    {
        foreach(string photoUrl in PhotoUrls)
        {
            GameObject postPhotoItemObj = await Instantiater.InstantiateAsync(StrDef.POST_PHOTO_ITEM_DATA_PATH, PostPhotoItemRoot.transform);
            MomentPhotoItem postPhotoItem = postPhotoItemObj.GetComponent<MomentPhotoItem>();
            postPhotoItem.PhotoUrl = photoUrl;
            postPhotoItem.GetPhotoAsync();
        }
    }
}