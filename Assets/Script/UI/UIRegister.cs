using MVPFrameWork;

public static class ViewId
{
    public const int LoginView = 0x1000;
    public const int MainView = 0x1001;
    public const int AlbumCreateView = 0x1002;
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
        Container.Regist<IAlbumCreatePresenter, AlbumCreatePresenter>();
        Container.Regist<IView, AlbumCreateView>(ViewId.AlbumCreateView);
    }
}
