using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/// <summary>
/// 相册列表项的点击事件
/// </summary>
public class PhotoWallItem : ItemBase
{
    public Image UserIcon;
    public TMP_Text TxtUserName;
    public TMP_Text TxtContent;
    public TMP_Text TxtHeartCount;
    public Button BtnHeart;
    public Button BtnComment;
    public GridLayoutGroup DetailPhotoItemRoot;

    private void Start()
    {

    }
}