using UnityEngine;

public class CreatePhotoWallItemModel : ICreatePhotoWallItemModel
{
    private Texture2D[] _photos;

    private string _content;

    public Texture2D[] Photos
    {
        get => _photos; set => _photos = value;
    }

    public string Content
    {
        get => _content; set => _content = value;
    }

    public void SetModel(Texture2D[] photos, string content)
    {
        _photos = photos;
        _content = content;
    }
}