using UnityEngine;

public interface ICreatePhotoWallItemModel : IModel
{
    Texture2D[] Photos { get; set; }

    void SetModel(Texture2D[] photos);
}