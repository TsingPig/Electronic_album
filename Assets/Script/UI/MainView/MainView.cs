using Michsky.MUIP;
using MVPFrameWork;
using System.Collections.Generic;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class MainView : ViewBase<IMainPresenter>, IMainView
{
    #region TopPanel

    private ButtonManager _btnSetting;
    public ButtonManager BtnSetting { get => _btnSetting; set => _btnSetting = value; }

    #endregion



    #region UserInformationView

    private TMP_Text _txtUserName;
    private Button _btnUserIcon;
    private Button _btnUpdateUserIcon;
    private TMP_Text _txtNickName;
    private Button _btnUpdateNickName;
    private Button _btnEnterPhotoWall;
    private TMP_InputField _inptNickName;
    private Button _btnSureUpdateNickName;

    public TMP_Text TxtUserName { get => _txtUserName; set => _txtUserName = value; }
    public Button BtnUserIcon { get => _btnUserIcon; set => _btnUserIcon = value; }
    public Button BtnUpdateUserIcon { get => _btnUpdateUserIcon; set => _btnUpdateUserIcon = value; }
    public TMP_Text TxtNickName { get => _txtNickName; set => _txtNickName = value; }
    public Button BtnUpdateNickName { get => _btnUpdateNickName; set => _btnUpdateNickName = value; }
    public Button BtnEnterPhotoWall { get => _btnEnterPhotoWall; set => _btnEnterPhotoWall = value; }
    public TMP_InputField InptNickName { get => _inptNickName; set => _inptNickName = value; }
    public Button BtnSureUpdateNickName { get => _btnSureUpdateNickName; set => _btnSureUpdateNickName = value; }

    #endregion

    #region AlbumView

    private ButtonManager _btnCreateAlbum;
    private GridLayoutGroup _gridAlbumContent;
    private List<ButtonManager> _btnEnterAlbums;
    public ButtonManager BtnCreateAlbum => _btnCreateAlbum;
    public GridLayoutGroup GridAlbumContent => _gridAlbumContent;
    public List<ButtonManager> BtnEnterAlbums => _btnEnterAlbums;

    #endregion

    protected override void OnCreate()
    {
        #region TopPanel

        _btnSetting = _root.Find<ButtonManager>("TopPanel/btnSetting");
        _btnSetting.onClick.AddListener(_presenter.ClearUserInformationCache);

        #endregion

        #region UserInformationView

        _txtUserName = _root.Find<TMP_Text>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserName/txtUserName");
        _btnUserIcon = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserIconItem/btnUserIcon");
        _btnUpdateUserIcon = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserIconItem/btnUpdateUserIcon");
        _txtNickName = _root.Find<TMP_Text>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/txtNickName");
        _btnUpdateNickName = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/btnUpdateNickName");
        _btnEnterPhotoWall = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserPhotoWall/btnEnterPhotoWall");
        _inptNickName = _root.Find<TMP_InputField>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/inptNickName");
        _btnSureUpdateNickName = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/inptNickName/btnSureUpdateNickName");

        _btnUpdateNickName.onClick.AddListener(_presenter.UpdateNickName);
        _btnSureUpdateNickName.onClick.AddListener(_presenter.SureUpdateNickName);

        _btnUserIcon.onClick.AddListener(_presenter.UpdateUserIcon);

        #endregion

        #region AlbumView
        _btnCreateAlbum = _root.Find<ButtonManager>("Window Manager/Windows/AlbumView/ScrollbarView/Viewport/Content/CreateItem/btnCreateAlbum");
        _btnCreateAlbum.onClick.AddListener(_presenter.EnterAlbumCreateView);

        _gridAlbumContent = _root.Find<GridLayoutGroup>("Window Manager/Windows/AlbumView/ScrollbarView/Viewport/Content");
        
        #endregion
    }

}
