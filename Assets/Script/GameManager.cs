using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TsingPigSDK;
using UIManager = MVPFrameWork.UIManager;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{
    public void ApplicationEntry()
    {
        if(MainPresenter.TryAutoLogin(out UserInformation userInformation))
        {
            UIManager.Instance.Enter(ViewId.MainView);
        }
        else
        {
            UIManager.Instance.Enter(ViewId.LoginView);
        }
    }
    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}