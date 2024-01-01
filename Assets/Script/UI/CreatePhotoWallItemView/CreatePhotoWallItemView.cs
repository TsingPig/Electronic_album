using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class CreatePhotoWallItemView : ViewBase<ICreatePhotoWallItemPresenter>, ICreatePhotoWallItemView
{
    private ButtonManager _btnQuit;
    private ButtonManager _btnUploadPhoto;
    private ButtonManager _btnCreatePhtotoWallItem;
    private GridLayoutGroup _gridPhotoContent;
    private TMP_InputField _inptContent;

    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnUploadPhoto => _btnUploadPhoto;

    public ButtonManager BtnCreatePhotoWallItem => _btnCreatePhtotoWallItem;

    public GridLayoutGroup GridPhotoContent => _gridPhotoContent;

    public TMP_InputField InptContent => _inptContent;


    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("MainPanel/btnQuit");
        _btnCreatePhtotoWallItem = _root.Find<ButtonManager>("MainPanel/btnCreatePhotoItem");
        _inptContent = _root.Find<TMP_InputField>("MainPanel/inptContent");
        _gridPhotoContent = _root.Find<GridLayoutGroup>("MainPanel/ScrollbarView/Viewport/Content");
        _btnUploadPhoto = _root.Find<ButtonManager>("MainPanel/ScrollbarView/Viewport/Content/btnUploadPhoto/btnUploadPhoto");

        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnUploadPhoto.onClick.AddListener(_presenter.UploadPhoto);
        _btnCreatePhtotoWallItem.onClick.AddListener(_presenter.CreatePhotoWallItem);

    }
}