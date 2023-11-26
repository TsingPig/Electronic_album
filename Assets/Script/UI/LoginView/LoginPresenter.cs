using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;

public class LoginPresenter : PresenterBase<ILoginView>, ILoginPresenter
{
    public void OnLogin()
    {
        UIManager.Instance.Quit(ViewId.LoginView);
        UIManager.Instance.Enter(ViewId.MainView);
    }

    public void OnRegister()
    {
        
    }

    public void OnSuperLogin()
    {
        
    }
}
