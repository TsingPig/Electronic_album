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


    protected override void OnCreate()
    {

        _txtRegisterInputAccount = _root.Find<Text>("Window Manager/Windows/Login/Content/inptAccount");
        _txtRegisterInputPassword = _root.Find<Text>("Window Manager/Windows/Login/Content/inptPassword");
        _txtRegisterInputSurePassWord = _root.Find<Text>("Window Manager/Windows/Login/Content/inptSurePassword");

        _txtLoginInputAccount = _root.Find<Text>("Window Manager/Windows/Login/Register/inptAccount");
        _txtLoginInputPassword = _root.Find<Text>("Window Manager/Windows/Login/Register/inptPassword");
        _tglLoginChangePasswordState = _root.Find<Toggle>("Window Manager/Windows/Login/Register/tgglChangePasswordState");

        _btnLogin = _root.Find<ButtonManager>("Window Manager/Windows/Login/btnLogin");
        _btnLogin.onClick.AddListener(_presenter.OnLogin);
    }
    protected override void OnCreate()
    {
     
    }
}
