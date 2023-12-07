using Michsky.MUIP;
using MVPFrameWork;
using System;
using System.Diagnostics;
using TMPro;
using TsingPigSDK;
using UnityEngine.Events;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class LoginView : ViewBase<ILoginPresenter>, ILoginView
{
    private TMP_Text _txtRegisterInputAccount;
    private TMP_InputField _txtRegisterInputPassword;
    private TMP_InputField _txtRegisterInputSurePassWord;
    private TMP_Text _txtLoginInputAccount;
    private TMP_InputField _txtLoginInputPassword;
    private Toggle _tglLoginChangePasswordState;
    private ButtonManager _btnLogin;
    private ButtonManager _btnRegister;


    public TMP_Text TxtRegisterInputAccount { get => _txtRegisterInputAccount; set => _txtRegisterInputAccount = value; }
    public TMP_InputField TxtRegisterInputPassWord { get => _txtRegisterInputPassword; set => _txtRegisterInputPassword = value; }
    public TMP_InputField TxtRegisterInputSurePassWord { get => _txtRegisterInputSurePassWord; set => _txtRegisterInputSurePassWord = value; }
    public TMP_Text TxtLoginInputAccount { get => _txtLoginInputAccount; set => _txtLoginInputAccount = value; }
    public TMP_InputField TxtLoginInputPassWord { get => _txtLoginInputPassword; set => _txtLoginInputPassword = value; }
    public Toggle TglLoginChangePasswordState { get => _tglLoginChangePasswordState; set => _tglLoginChangePasswordState = value; }
    public ButtonManager BtnLogin { get => _btnLogin; set => _btnLogin = value; }  
    public ButtonManager BtnRegister { get => _btnRegister; set => _btnRegister = value; }

    protected override void OnCreate()
    {

        _txtRegisterInputAccount = _root.Find<TMP_Text>("Window Manager/Windows/Register/Content/inptAccount/Text Area/Text");
        _txtRegisterInputPassword = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptPassword");
        _txtRegisterInputSurePassWord = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptSurePassword");

        _txtLoginInputAccount = _root.Find<TMP_Text>("Window Manager/Windows/Login/Content/inptAccount/Text Area/Text");
        _txtLoginInputPassword = _root.Find<TMP_InputField>("Window Manager/Windows/Login/Content/inptPassword");
        _tglLoginChangePasswordState = _root.Find<Toggle>("Window Manager/Windows/Login/Content/tgglChangePasswordState");
        _tglLoginChangePasswordState.onValueChanged.AddListener((bool value) => _presenter.ChangePasswordState(value));
        
        _btnLogin = _root.Find<ButtonManager>("Window Manager/Windows/Login/btnLogin");
        _btnRegister = _root.Find<ButtonManager>("Window Manager/Windows/Register/btnRegister");
        _btnLogin.onClick.AddListener(_presenter.OnLogin);
        _btnRegister.onClick.AddListener(_presenter.OnRegister);


    }
}
