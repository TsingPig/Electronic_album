using MVPFrameWork;

public static class UIRegister
{
    public static void RegisterAll()
    {
        Container.Regist<ILoginPresenter, LoginPresenter>();
        Container.Regist<IView, LoginView>(ViewId.LoginView);

        Container.Regist<IMainPresenter, MainPresenter>();
        Container.Regist<IView, MainView>(ViewId.MainView);
        Container.Regist<IMainModel, MainModel>();

        Container.Regist<IAlbumCreatePresenter, AlbumCreatePresenter>();
        Container.Regist<IView, AlbumCreateView>(ViewId.AlbumCreateView);

        Container.Regist<IPhotoPresenter, PhotoPresenter>();
        Container.Regist<IView, PhotoView>(ViewId.PhotoView);
        Container.Regist<IPhotoModel, PhotoModel>();

        Container.Regist<IPhotoDetailPresenter, PhotoDetailPresenter>();
        Container.Regist<IView, PhotoDetailView>(ViewId.PhotoDetailView);
        Container.Regist<IPhotoDetailModel, PhotoDetailModel>();

        Container.Regist<ICreatePhotoWallItemPresenter, CreatePhotoWallItemPresenter>();
        Container.Regist<IView, CreatePhotoWallItemView>(ViewId.CreatePhotoWallItemView);
        Container.Regist<ICreatePhotoWallItemModel, CreatePhotoWallItemModel>();

        Container.Regist<IMomentPresenter, MomentPresenter>();
        Container.Regist<IView,  MomentView>(ViewId.MomentView);

    }
}