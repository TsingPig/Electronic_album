using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PhotoWallItem : ItemBase
{
    public Image UserIcon;
    public TMP_Text TxtUserName;
    public TMP_Text TxtContent;
    public TMP_Text TxtHeartCount;
    public Button BtnHeart;
    public Button BtnComment;
    public GridLayoutGroup MomentPhotoItemRoot;

    [HideInInspector]
    public List<string> PhotoUrls;

    private void Start()
    {
    }

    public async Task LoadMomentPhotoItems()
    {
        foreach(string photoUrl in PhotoUrls)
        {
            GameObject momentPhotoItemObj = await Instantiater.InstantiateAsync(StrDef.MOMENT_PHOTO_ITEM_DATA_PATH, MomentPhotoItemRoot.transform);
            MomentPhotoItem momentPhotoItem = momentPhotoItemObj.GetComponent<MomentPhotoItem>();
            momentPhotoItem.PhotoUrl = photoUrl;
            momentPhotoItem.GetPhotoAsync();
        }
    }
}