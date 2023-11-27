using System.IO;
using TsingPigSDK;
using UnityEngine.AddressableAssets;
using UIManager = MVPFrameWork.UIManager;

public class GameManager : Singleton<GameManager>
{
    public bool UserInformationCached = false;
    public void ApplicationEntry()
    {
        string filePath = Path.Combine("Assets/Resources/UserInformation", "userData.json");
        if(File.Exists(filePath))
        {
            UIManager.Instance.Enter(ViewId.LoginView);
            // UIManager.Instance.Enter(ViewId.MainView);
            UserInformationCached = true;
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