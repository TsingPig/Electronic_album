using UnityEngine;

public interface ICreatePostItemModel: IModel
{
    Texture2D[] Photos { get; set; }

    string Content { get; set; }

    string Title { get; set; }

    void SetModel(Texture2D[] photos, string content);
}