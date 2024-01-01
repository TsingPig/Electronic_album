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
    public ButtonManager BtnEnterManager;
    public Image UserIcon;
    public TMP_Text TxtNickName;
    public TMP_Text TxtContent;
    public TMP_Text TxtHeartCount;
    public Button BtnHeart;
    public Button BtnComment;
    public GridLayoutGroup DetailPhotoItemRoot;

    protected override void OnClick()
    {
        base.OnClick();
        Debug.Log($"PhotoItem OnClick");
    }

    private void Start()
    {
        BtnItem = BtnEnterManager;
    }
}