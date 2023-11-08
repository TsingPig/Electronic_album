using MVPFrameWork;

public static class ViewId
{
    public const int LoginView = 1000;
    public const int MainView = 1001;


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
        Container.Regist<IMainPresenter, MainPresenter>();
        Container.Regist<IView, MainView>(ViewId.MainView);
    }
}
