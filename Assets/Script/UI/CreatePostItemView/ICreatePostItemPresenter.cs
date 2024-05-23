using MVPFrameWork;

public interface ICreatePostItemPresenter : IPresenter
{
    void Quit();

    void UploadPhotos();

    void UploadPhoto();

    void CreatePhotoWallItem();
}