using MVPFrameWork;

public interface IPhotoPresenter : IPresenter
{
    void Quit();

    void UploadPhoto();

    void DeleteAlbum();

    //void DeletePhoto(int idx);

    void UploadMultiPhotos();
}