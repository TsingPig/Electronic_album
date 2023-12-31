using UnityEngine;

public class CreatePhotoWallItemModel : ICreatePhotoWallItemModel
{
    private Texture2D[] _photos;

    public Texture2D[] Photos
    {
        get => _photos; set => _photos = value;
    }

    public void SetModel(Texture2D[] photos)
    {
        _photos = photos;
    }
}