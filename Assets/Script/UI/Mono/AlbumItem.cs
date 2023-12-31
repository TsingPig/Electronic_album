using Michsky.MUIP;
using TMPro;

/// <summary>
/// 相册列表项的点击事件
/// </summary>
public class AlbumItem : ItemBase
{
    public TMP_Text AlbumTitle;

    public ButtonManager BtnEnterManager;

    protected override void OnClick()
    {
        base.OnClick();
        PhotoModel model = new PhotoModel();
        model.SetModel(AlbumTitle.text);

        // Debug.Log(AlbumTitle.text);

        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoView, model);
    }

    private void Start()
    {
        AlbumTitle.text = gameObject.name;

        BtnItem = BtnEnterManager;
    }
}