using MVPFrameWork;

public static class ViewId
{
    public const int LoginView = 1000;

}

public static class ConstDef
{
    public const string CANVAS = "Canvas";

}


public static class UIRegister
{
    public static void RegisterAll()
    {
        Container.Regist<ILoginPresenter, LoginPresenter>();
        Container.Regist<IView, LoginView>(ViewId.LoginView);
        
    }
}
