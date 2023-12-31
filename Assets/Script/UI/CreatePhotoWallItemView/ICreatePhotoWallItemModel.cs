using UnityEngine;

public interface ICreatePhotoWallItemModel : IModel
{
    Texture2D[] Photos { get; set; }

    string Content { get; set; }

    void SetModel(Texture2D[] photos, string content);
}