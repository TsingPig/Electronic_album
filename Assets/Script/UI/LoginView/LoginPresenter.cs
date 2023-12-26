using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using UIManager = MVPFrameWork.UIManager;
using TsingPigSDK;
using TMPro;

public class LoginPresenter : PresenterBase<ILoginView>, ILoginPresenter
{
    public override void OnCreateCompleted()
    {
        Debug.Log("生成" + MySQLManager.Instance);
        Debug.Log("生成" + ServerManager.Instance);
    }

    public void OnLogin()
    {
        string LoginInputAccount = _view.InptLoginInputAccount.text;
        string LoginInputPassword = _view.InptLoginInputPassWord.text;

        if(MySQLManager.Instance.Login(LoginInputAccount, LoginInputPassword))
        {
            UIManager.Instance.Quit(ViewId.LoginView);

            string NickName = MySQLManager.Instance.GetNickName(LoginInputAccount);

            Texture2D randomIcon = new Texture2D(200, 200);
            randomIcon.RandomGenerate();

            CacheManager.Instance.SaveUserInformation(LoginInputAccount, NickName, randomIcon);
            UIManager.Instance.Enter(ViewId.MainView);

            //从服务器下载头像数据
            ServerManager.Instance.DownLoadUserIcon(LoginInputAccount);

        }
        else
        {
            Debug.Log("账号或者密码错误");
        }

    }

    public void OnRegister()
    {
        string RegisterInputAccount = _view.InptRegisterInputAccount.text;
        string RegisterInputPassWord = _view.InptRegisterInputPassWord.text;
        string RegisterInputSurePassWord = _view.InptRegisterInputSurePassWord.text;

        if(RegisterInputPassWord.Equals(RegisterInputSurePassWord))
        {
            if(MySQLManager.Instance.Register(RegisterInputAccount, RegisterInputAccount, RegisterInputPassWord))
            {
                Texture2D randomIcon = new Texture2D(200, 200);
                randomIcon.RandomGenerate();
                CacheManager.Instance.SaveUserInformation(RegisterInputAccount, RegisterInputAccount, randomIcon);
                UIManager.Instance.Quit(ViewId.LoginView);
                UIManager.Instance.Enter(ViewId.MainView);
            }

        }

    }

    public void OnSuperLogin()
    {

    }

    public void ChangePasswordState(bool value)
    {

        if(value)
        {
            _view.InptLoginInputPassWord.inputType = TMPro.TMP_InputField.InputType.Standard;
        }
        else
        {
            _view.InptLoginInputPassWord.inputType = TMPro.TMP_InputField.InputType.Password;
        }

    }

    public void ClearInformation(TMP_InputField info)
    {
        if(info == null)
        {
            return;
        }
        info.text = "";
    }
}
