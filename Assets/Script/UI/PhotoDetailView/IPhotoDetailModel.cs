using UnityEngine;

public interface IPhotoDetailModel : IModel
{
    /// <summary>
    /// ��Ƭϸ������
    /// </summary>
    Texture2D Tex2DPhotoDetail { get; }

    /// <summary>
    /// ��Ƭ���
    /// </summary>
    int PhotoId { get; }

    void SetModel(Texture2D tex2DPhotoDetail, int photoId);
}