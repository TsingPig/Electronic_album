using UnityEngine.UI;

public class PhotoDetailModel : IPhotoDetailModel
{
    private Image _imgPhotoDetail;
    private int _photoId;
    private string _albumName;
    private bool _allowDelete;

    public Image ImgPhotoDetail => _imgPhotoDetail;

    public int PhotoId => _photoId;

    public string AlbumName => _albumName;

    public bool AllowDelete => _allowDelete;

    public void SetModel(Image imgPhotoDetail, int photoId, string albumName, bool allowDelete = true)
    {
        _imgPhotoDetail = imgPhotoDetail;
        _photoId = photoId;
        _albumName = albumName;
        _allowDelete = allowDelete;
    }
}