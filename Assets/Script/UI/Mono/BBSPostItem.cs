using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

public class BBSPostItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button BtnEnterPost;
    public TMP_Text UserName;
    public TMP_Text Title;
    public TMP_Text Content;

    public GridLayoutGroup PostPhotoItemRoot;

    [HideInInspector]
    public List<string> PhotoUrls;
    void Start()
    {
        BtnEnterPost.onClick.AddListener(() =>
        {
            Debug.Log("季神大计基");

        });
    }

    void Update()
    {
        
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
