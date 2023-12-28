using Michsky.MUIP;
using TMPro;

/// <summary>
/// ����б���ĵ���¼�
/// </summary>
public class AlbumItem : ItemBase
{
    public TMP_Text AlbumTitle;
    public ButtonManager BtnEnterManager;


    protected override void OnClick()
    {
        base.OnClick();
        MVPFrameWork.UIManager.Instance.Enter(ViewId.PhotoView);
    }

    protected override void Start()
    {
        base.Start();
        AlbumTitle.text = gameObject.name;

        _btnItem = BtnEnterManager;
    }

}
