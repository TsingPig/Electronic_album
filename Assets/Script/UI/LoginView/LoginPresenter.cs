using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using UIManager = MVPFrameWork.UIManager;
using TsingPigSDK;

public class LoginPresenter : PresenterBase<ILoginView>, ILoginPresenter
{
    public override void OnCreateCompleted()
    {
        Debug.Log("����" + MySQLManager.Instance);
    }

    public void OnLogin()
    {
        string LoginInputAccount = _view.TxtLoginInputAccount.text;
        string LoginInputPassword = _view.TxtLoginInputPassWord.text;

        if(MySQLManager.Instance.Login(LoginInputAccount, LoginInputPassword))
        {
            UIManager.Instance.Quit(ViewId.LoginView);

            string NickName = MySQLManager.Instance.GetNickName(LoginInputAccount);

            Texture2D randomIcon = new Texture2D(200, 200);
            randomIcon.RandomGenerate();
            CacheManager.Instance.SaveUserInformation(LoginInputAccount, NickName, randomIcon);

            UIManager.Instance.Enter(ViewId.MainView);

        }
        else
        {
            Debug.Log("�˺Ż����������");
        }

    }

    public void OnRegister()
    {
        string RegisterInputAccount = _view.TxtRegisterInputAccount.text;
        string RegisterInputPassWord = _view.TxtRegisterInputPassWord.text;
        string RegisterInputSurePassWord = _view.TxtRegisterInputSurePassWord.text;

        if(RegisterInputPassWord.Equals(RegisterInputSurePassWord))
        {
            
            UIManager.Instance.Quit(ViewId.LoginView);



            Texture2D randomIcon = new Texture2D(200, 200);
            randomIcon.RandomGenerate();


            CacheManager.Instance.SaveUserInformation(RegisterInputAccount, RegisterInputAccount, randomIcon);
            MySQLManager.Instance.Register(RegisterInputAccount, RegisterInputAccount, RegisterInputPassWord);
            
            UIManager.Instance.Enter(ViewId.MainView);
        }

    }

    public void OnSuperLogin()
    {

    }
}
