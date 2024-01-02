using MVPFrameWork;
using TMPro;
using TsingPigSDK;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;

public class LoginPresenter : PresenterBase<ILoginView>, ILoginPresenter
{
    public override void OnCreateCompleted()
    {
        Debug.Log("����" + MySQLManager.Instance);
        Debug.Log("����" + ServerManager.Instance);
    }

    public void OnLogin()
    {
        string LoginInputAccount = RestrictedStringToLettersOrNumbers(_view.InptLoginInputAccount.text);
        string LoginInputPassword = RestrictedStringToLettersOrNumbers(_view.InptLoginInputPassWord.text);

        if(MySQLManager.Instance.Login(LoginInputAccount, LoginInputPassword))
        {
            UIManager.Instance.Quit(ViewId.LoginView);

            string NickName = MySQLManager.Instance.GetNickName(LoginInputAccount);

            Texture2D randomIcon = new Texture2D(200, 200);
            randomIcon.RandomGenerate();

            CacheManager.Instance.SaveUserInformation(LoginInputAccount, NickName, randomIcon);
            UIManager.Instance.Enter(ViewId.MainView, new MainModel());

            //�ӷ���������ͷ������
            ServerManager.Instance.DownLoadUserIcon(LoginInputAccount);
        }
        else
        {
            Debug.Log("�˺Ż����������");
        }
    }

    public void OnRegister()
    {
        string RegisterInputAccount = RestrictedStringToLettersOrNumbers(_view.InptRegisterInputAccount.text);
        string RegisterInputPassWord = RestrictedStringToLettersOrNumbers(_view.InptRegisterInputPassWord.text);
        string RegisterInputSurePassWord = RestrictedStringToLettersOrNumbers(_view.InptRegisterInputSurePassWord.text);

        if(RegisterInputPassWord.Equals(RegisterInputSurePassWord))
        {
            if(MySQLManager.Instance.Register(RegisterInputAccount, RegisterInputAccount, RegisterInputPassWord))
            {
                Texture2D randomIcon = new Texture2D(200, 200);
                randomIcon.RandomGenerate();
                CacheManager.Instance.SaveUserInformation(RegisterInputAccount, RegisterInputAccount, randomIcon);

                CacheManager.Instance.UpdateIcon(randomIcon);

                UIManager.Instance.Quit(ViewId.LoginView);
                UIManager.Instance.Enter(ViewId.MainView, new MainModel());
            }
        }
    }

    public void OnSuperLogin()
    {
        string LoginInputSuperAccount = RestrictedStringToLettersOrNumbers(_view.InptLoginSuperInputAccount.text);
        string LoginInputSuperPassword = RestrictedStringToLettersOrNumbers(_view.InptLoginSuperInputPassword.text);

        if (MySQLManager.Instance.LoginSuper(LoginInputSuperAccount, LoginInputSuperPassword))
        {
            UIManager.Instance.Quit(ViewId.LoginView);

            string NickName = MySQLManager.Instance.GetNickName(LoginInputSuperAccount);

            Texture2D randomIcon = new Texture2D(200, 200);
            randomIcon.RandomGenerate();

            CacheManager.Instance.SaveUserInformation(LoginInputSuperAccount, NickName, randomIcon);
            UIManager.Instance.Enter(ViewId.MainView, new MainModel());

            //�ӷ���������ͷ������
            ServerManager.Instance.DownLoadUserIcon(LoginInputSuperAccount);
        }
        else
        {
            Debug.Log("�˺Ż����������");
        }


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

    /// <summary>
    /// ����ֻ�����Ϸ��ַ�����ĸ/���֣����ַ���
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string RestrictedStringToLettersOrNumbers(string str)
    {
        string restrictedString = string.Empty;
        foreach(char ch in str)
        {
            if(char.IsLetterOrDigit(ch))
            {
                restrictedString += ch;
            }
        }
        return restrictedString;
    }
}