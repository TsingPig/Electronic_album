using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;

public interface ILoginPresenter : IPresenter
{
    void OnLogin();

    void OnRegister();

    void OnSuperLogin();

    void ChangePasswordState(bool value);

}
