using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TsingPigSDK;

public class ServerManager : Singleton<ServerManager>
{
    private void Init()
    {

    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}