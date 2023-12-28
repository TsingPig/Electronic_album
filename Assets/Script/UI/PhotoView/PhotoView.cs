using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class PhotoView : ViewBase<IPhotoPresenter>, IPhotoView
{
    private ButtonManager _btnQuit;
    private TMP_Text _txtAlbumName;
    private GridLayoutGroup _gridPhotoContent;

    public ButtonManager BtnQuit => _btnQuit;

    public TMP_Text TxtAlbumName => _txtAlbumName;

    public GridLayoutGroup GridPhotoContent => _gridPhotoContent;

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("AlbumPanel/btnQuit");
        _txtAlbumName = _root.Find<TMP_Text>("AlbumPanel/txtAlbumName");
        _gridPhotoContent = _root.Find<GridLayoutGroup>("AlbumPanel/ScrollbarView/Viewport/Content");

        _btnQuit.onClick.AddListener(_presenter.Quit);
    }
}