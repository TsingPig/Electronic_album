using UnityEngine;

public class CreatePostItemModel : ICreatePostItemModel
{
    private Texture2D[] _photos;

    private string _title;
    private string _content;


    public Texture2D[] Photos
    {
        get => _photos; set => _photos = value;
    }

    public string Title
    {
        get => _title; set => _title = value;
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