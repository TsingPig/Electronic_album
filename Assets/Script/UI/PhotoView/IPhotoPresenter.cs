using MVPFrameWork;

public interface IPhotoPresenter : IPresenter
{
    void Quit();

    void UploadPhoto();

    void DeleteAlbum();
}