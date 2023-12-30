using Michsky.MUIP;
using MVPFrameWork;
using TMPro;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class AlbumCreateView : ViewBase<IAlbumCreatePresenter>, IAlbumCreateView
{
    private TMP_InputField _inptAlbumName;
    private CustomDropdown _dropDownAlbumType;
    private ButtonManager _btnCreateAlbum;
    private ButtonManager _btnQuit;

    public TMP_InputField InptAlbumName => _inptAlbumName;

    public CustomDropdown DropDownAlbumType => _dropDownAlbumType;

    public ButtonManager BtnCreateAlbum => _btnCreateAlbum;

    public ButtonManager BtnQuit => _btnQuit;

    protected override void OnCreate()
    {
        _inptAlbumName = _root.Find<TMP_InputField>("IptAlbumName");
        _dropDownAlbumType = _root.Find<CustomDropdown>("DropDownAlbumType");
        _btnCreateAlbum = _root.Find<ButtonManager>("btnCreateAlbum");
        _btnQuit = _root.Find<ButtonManager>("btnQuit");

        _btnCreateAlbum.onClick.AddListener(_presenter.CreateAlbum);
        _btnQuit.onClick.AddListener(_presenter.Quit);
    }
}