using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVPFrameWork;

public class LoginView : ViewBase<ILoginPresenter>, ILoginView
{
    public string account { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string password { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool isHidden { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    protected override void OnCreate()
    {
        throw new System.NotImplementedException();
    }
}
