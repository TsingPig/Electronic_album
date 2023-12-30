using UnityEngine;

public class PhotoDetailModel : IPhotoDetailModel
{
    private Texture2D _tex2DPhotoDetail;
    private int _photoId;

    public Texture2D Tex2DPhotoDetail => _tex2DPhotoDetail;
    public int PhotoId => _photoId;


    public void SetModel(Texture2D tex2DPhotoDetail, int photoId)
    {
        _tex2DPhotoDetail = tex2DPhotoDetail;
        _photoId = photoId;
    }
}