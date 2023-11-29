using TsingPigSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : Singleton<GameManager>
{

    private void Init()
    {
        Addressables.InitializeAsync();
        UIRegister.RegisterAll();
        Debug.Log("生成：" + CacheManager.Instance);
        //Debug.Log("生成：" + MySQLManager.Instance);
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
}