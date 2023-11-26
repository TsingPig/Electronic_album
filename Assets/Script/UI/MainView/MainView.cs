using Michsky.MUIP;
using MVPFrameWork;
using System;
using System.Diagnostics;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class MainView : ViewBase<IMainPresenter>, IMainView
{
    #region UserInformationView

    private Text _txtUserName;
    private Button _btnUserIcon;
    private Button _btnUpdateUserIcon;
    private Text _txtNickName;
    private Button _btnUpdateNickName;
    private Button _btnEnterPhotoWall;

    public Text TxtUserName { get => _txtUserName; set => _txtUserName = value; }
    public Button BtnUserIcon { get => _btnUserIcon; set => _btnUserIcon = value; }
    public Button BtnUpdateUserIcon { get => _btnUpdateUserIcon; set => _btnUpdateUserIcon = value; }
    public Text TxtNickName { get => _txtNickName; set => _txtNickName = value; }
    public Button BtnUpdateNickName { get => _btnUpdateNickName; set => _btnUpdateNickName = value; }
    public Button BtnEnterPhotoWall { get => _btnEnterPhotoWall; set => _btnEnterPhotoWall = value; }

    #endregion

    protected override void OnCreate()
    {
        #region UserInformationView

        _txtUserName = _root.Find<Text>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserName/txtUserName");
        _btnUserIcon = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserIconItem/btnUserIcon");
        _btnUserIcon = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserIconItem/btnUpdateUserIcon");
        _txtNickName = _root.Find<Text>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/txtNickName");
        _btnUpdateNickName = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/NickNameItem/btnUpdateNickName");
        _btnEnterPhotoWall = _root.Find<Button>("Window Manager/Windows/UserInformationView/UserInformationPanel/UserPhotoWall/btnEnterPhotoWall");
        Texture2D texture = new Texture2D(128, 128);
        _presenter.SaveUserInformation("zzy", "÷Ï’˝—Ù", texture.RandomGenerate());
        
        #endregion
    }

}
