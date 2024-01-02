using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        string url = $"{ServerManager.Instance.host}/download/{TxtUserName.text}/usericon.jpg";
        await ServerManager.Instance.GetPhotoAsync(url, UserIcon);
        foreach(string photoUrl in PhotoUrls)
        {
            GameObject momentPhotoItemObj = await Instantiater.InstantiateAsync(StrDef.MOMENT_PHOTO_ITEM_DATA_PATH, MomentPhotoItemRoot.transform);
            MomentPhotoItem momentPhotoItem = momentPhotoItemObj.GetComponent<MomentPhotoItem>();
            momentPhotoItem.PhotoUrl = photoUrl;
            momentPhotoItem.GetPhotoAsync();
        }
    }
}