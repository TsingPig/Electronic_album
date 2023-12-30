using UnityEngine.UI;

public class PhotoDetailModel : IPhotoDetailModel
{
    private Image _imgPhotoDetail;
    private int _photoId;
    private string _albumName;

    public Image ImgPhotoDetail => _imgPhotoDetail;

    public int PhotoId => _photoId;

    public string AlbumName => _albumName;

    public void SetModel(Image imgPhotoDetail, int photoId, string albumName)
    {
        _imgPhotoDetail = imgPhotoDetail;
        _photoId = photoId;
        _albumName = albumName;
    }
}