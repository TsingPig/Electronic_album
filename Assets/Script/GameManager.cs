using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager>
{
   
 

    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("���ɣ�" + CacheManager.Instance);
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}