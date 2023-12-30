using UnityEngine.UI;

public class PhotoDetailModel : IPhotoDetailModel
{
    private Image _imgPhotoDetail;
    private int _photoId;

    public Image ImgPhotoDetail => _imgPhotoDetail;

    public int PhotoId => _photoId;

    public void SetModel(Image imgPhotoDetail, int photoId)
    {
        _imgPhotoDetail = imgPhotoDetail;
        _photoId = photoId;
    }
}