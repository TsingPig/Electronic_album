using MVPFrameWork;

public interface ICreatePhotoWallItemPresenter : IPresenter
{
    void Quit();

    void UploadPhotos();

    void UploadPhoto();

    void CreatePhotoWallItem();
}