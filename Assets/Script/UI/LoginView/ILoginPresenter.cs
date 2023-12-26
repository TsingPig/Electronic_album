using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;
using TMPro;

public interface ILoginPresenter : IPresenter
{
    void OnLogin();

    void OnRegister();

    void OnSuperLogin();

    void ChangePasswordState(bool value);

    void ClearInformation(TMP_InputField info);
}
