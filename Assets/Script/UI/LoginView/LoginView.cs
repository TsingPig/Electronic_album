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
    private TMP_InputField _inptRegisterInputAccount;
    private TMP_InputField _inptRegisterInputPassword;
    private TMP_InputField _inptRegisterInputSurePassWord;
    private TMP_InputField _inptLoginInputAccount;
    private TMP_InputField _inptLoginInputPassword;
    private Toggle _tglLoginChangePasswordState;
    private ButtonManager _btnLogin;
    private ButtonManager _btnRegister;
    private ButtonManager _btnRegisterInputAccountClear;
    private ButtonManager _btnRegisterInputPasswordClear;
    private ButtonManager _btnRegisterInputSurePassWordClear;
    private ButtonManager _btnLoginInputAccountClear;
    private ButtonManager _btnLoginInputPassWordClear;


    public TMP_InputField InptRegisterInputAccount { get => _inptRegisterInputAccount; set => _inptRegisterInputAccount = value; }
    public TMP_InputField InptRegisterInputPassWord { get => _inptRegisterInputPassword; set => _inptRegisterInputPassword = value; }
    public TMP_InputField InptRegisterInputSurePassWord { get => _inptRegisterInputSurePassWord; set => _inptRegisterInputSurePassWord = value; }
    public TMP_InputField InptLoginInputAccount { get => _inptLoginInputAccount; set => _inptLoginInputAccount = value; }
    public TMP_InputField InptLoginInputPassWord { get => _inptLoginInputPassword; set => _inptLoginInputPassword = value; }
    public Toggle TglLoginChangePasswordState { get => _tglLoginChangePasswordState; set => _tglLoginChangePasswordState = value; }
    public ButtonManager BtnLogin { get => _btnLogin; set => _btnLogin = value; }  
    public ButtonManager BtnRegister { get => _btnRegister; set => _btnRegister = value; }
    public ButtonManager BtnRegisterInputAccountClear { get => _btnRegisterInputAccountClear; set => _btnRegisterInputAccountClear = value; }
    public ButtonManager BtnRegisterInputPassWordClear { get => _btnRegisterInputPasswordClear; set => _btnRegisterInputPasswordClear = value; }
    public ButtonManager BtnRegisterInputSurePassWordClear { get => _btnRegisterInputSurePassWordClear; set => _btnRegisterInputSurePassWordClear = value; }
    public ButtonManager BtnLoginInputAccountClear { get => _btnLoginInputAccountClear; set => _btnLoginInputAccountClear = value; }
    public ButtonManager BtnLoginInputPassWordClear { get => _btnLoginInputPassWordClear; set => _btnLoginInputPassWordClear = value; }


    protected override void OnCreate()
    {

        _inptRegisterInputAccount = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptAccount");
        _inptRegisterInputPassword = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptPassword");
        _inptRegisterInputSurePassWord = _root.Find<TMP_InputField>("Window Manager/Windows/Register/Content/inptSurePassword");

        _inptLoginInputAccount = _root.Find<TMP_InputField>("Window Manager/Windows/Login/Content/inptAccount");
        _inptLoginInputPassword = _root.Find<TMP_InputField>("Window Manager/Windows/Login/Content/inptPassword");
        _tglLoginChangePasswordState = _root.Find<Toggle>("Window Manager/Windows/Login/Content/tgglChangePasswordState");
        _tglLoginChangePasswordState.onValueChanged.AddListener((bool value) => _presenter.ChangePasswordState(value));
        
        _btnLogin = _root.Find<ButtonManager>("Window Manager/Windows/Login/btnLogin");
        _btnRegister = _root.Find<ButtonManager>("Window Manager/Windows/Register/btnRegister");
        _btnLogin.onClick.AddListener(_presenter.OnLogin);
        _btnRegister.onClick.AddListener(_presenter.OnRegister);

        _btnRegisterInputAccountClear = _root.Find<ButtonManager>("Window Manager/Windows/Register/Content/inptAccount/Clear");
        _btnRegisterInputPasswordClear = _root.Find<ButtonManager>("Window Manager/Windows/Register/Content/inptPassword/Clear");
        _btnRegisterInputSurePassWordClear = _root.Find<ButtonManager>("Window Manager/Windows/Register/Content/inptSurePassword/Clear");
        _btnLoginInputAccountClear = _root.Find<ButtonManager>("Window Manager/Windows/Login/Content/inptAccount/Clear");
        _btnLoginInputPassWordClear = _root.Find<ButtonManager>("Window Manager/Windows/Login/Content/inptPassword/Clear");


        _btnRegisterInputAccountClear.onClick.AddListener(delegate { _presenter.ClearInformation(InptRegisterInputAccount); });
        _btnRegisterInputPasswordClear.onClick.AddListener(delegate { _presenter.ClearInformation(InptRegisterInputPassWord); });
        _btnRegisterInputSurePassWordClear.onClick.AddListener(delegate { _presenter.ClearInformation(InptRegisterInputSurePassWord); });
        _btnLoginInputAccountClear.onClick.AddListener(delegate { _presenter.ClearInformation(InptLoginInputAccount); });
        _btnLoginInputPassWordClear.onClick.AddListener(delegate { _presenter.ClearInformation(InptLoginInputPassWord); });

    }
}
