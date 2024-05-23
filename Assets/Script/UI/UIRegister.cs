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
        Container.Regist<IView, MomentView>(ViewId.MomentView);

        Container.Regist<IBBSTypeCreatePresenter, BBSTypeCreatePresenter>();
        Container.Regist<IView, BBSTypeCreateView>(ViewId.BBSTypeCreateView);

        Container.Regist<IBBSPresenter, BBSPresenter>();
        Container.Regist<IView, BBSView>(ViewId.BBSView);
        Container.Regist<IBBSModel, BBSModel>();

        Container.Regist<ICreatePostItemPresenter, CreatePostItemPresenter>();
        Container.Regist<IView, CreatePostItemView>(ViewId.CreatePostItemView);
        Container.Regist<ICreatePostItemModel, CreatePostItemModel>();

        Container.Regist<IPostPresenter, PostPresenter>();
        Container.Regist<IView, PostView>(ViewId.PostView);
        Container.Regist<IPostModel, PostModel>();

        Container.Regist<INotificationPresenter, NotificationPresenter>();
        Container.Regist<IView, NotificationView>(ViewId.NotificationView);
        Container.Regist<INotificationModel, NotificationModel>();
    }
}