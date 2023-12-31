using MVPFrameWork;

public static class ViewId
{
    public const int LoginView = 1000;
    public const int MainView = 1001;
    public const int AlbumCreateView = 1002;
    public const int PhotoView = 1003;
    public const int PhotoDetailView = 1004;
    public const int CreatePhotoWallItemView = 1005;
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

        Container.Regist<IPhotoPresenter, PhotoPresenter>();
        Container.Regist<IView, PhotoView>(ViewId.PhotoView);
        Container.Regist<IPhotoModel, PhotoModel>();

        Container.Regist<IPhotoDetailPresenter, PhotoDetailPresenter>();
        Container.Regist<IView, PhotoDetailView>(ViewId.PhotoDetailView);
        Container.Regist<IPhotoDetailModel, PhotoDetailModel>();

        Container.Regist<ICreatePhotoItemPresenter, CreatePhotoItemPresenter>();
        Container.Regist<IView, CreatePhotoItemView>(ViewId.CreatePhotoWallItemView);

    }
}