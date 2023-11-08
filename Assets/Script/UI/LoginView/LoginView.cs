using Michsky.MUIP;
using MVPFrameWork;
using System;
using System.Diagnostics;
using TsingPigSDK;
using UnityEngine.Events;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class LoginView : ViewBase<ILoginPresenter>, ILoginView
{
    private Text _txtRegisterInputAccount;
    private Text _txtRegisterInputPassword;
    private Text _txtRegisterInputSurePassWord;
    private Text _txtLoginInputAccount;
    private Text _txtLoginInputPassword;
    private Toggle _tglLoginChangePasswordState;
    private ButtonManager _btnLogin;

    public Text TxtRegisterInputAccount { get => _txtRegisterInputAccount; set => _txtRegisterInputAccount = value; }
    public Text TxtRegisterInputPassWord { get => _txtRegisterInputPassword; set => _txtRegisterInputPassword = value; }
    public Text TxtRegisterInputSurePassWord { get => _txtRegisterInputSurePassWord; set => _txtRegisterInputSurePassWord = value; }
    public Text TxtLoginInputAccount { get => _txtLoginInputAccount; set => _txtLoginInputAccount = value; }
    public Text TxtLoginInputPassWord { get => _txtLoginInputPassword; set => _txtLoginInputPassword = value; }
    public Toggle TglLoginChangePasswordState { get => _tglLoginChangePasswordState; set => _tglLoginChangePasswordState = value; }
    public ButtonManager BtnLogin { get => _btnLogin; set => _btnLogin = value; }  

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
}
