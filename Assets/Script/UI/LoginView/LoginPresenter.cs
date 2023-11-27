using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using UnityEngine.UI;

public class LoginPresenter : PresenterBase<ILoginView>, ILoginPresenter
{
    public void OnLogin()
    {
        string LoginInputAccount = _view.TxtLoginInputAccount.text;
        string LoginInputPassword = _view.TxtLoginInputPassWord.text;

        if(MySQLManager.Instance.Login(LoginInputAccount, LoginInputPassword))
        {
            UIManager.Instance.Quit(ViewId.LoginView);
            UIManager.Instance.Enter(ViewId.MainView);
        }
        
    }

    public void OnRegister()
    {
        string RegisterInputAccount = _view.TxtRegisterInputAccount.text;
        string RegisterInputPassWord = _view.TxtRegisterInputPassWord.text;
        string RegisterInputSurePassWord = _view.TxtRegisterInputSurePassWord.text;

        if (RegisterInputPassWord.Equals(RegisterInputSurePassWord))
        {
            Debug.Log(2);
            UIManager.Instance.Quit(ViewId.LoginView);
            UIManager.Instance.Enter(ViewId.MainView);
            MySQLManager.Instance.Register(RegisterInputAccount, RegisterInputAccount, RegisterInputPassWord);
        }
        
    }

    public void OnSuperLogin()
    {
        
    }
}
