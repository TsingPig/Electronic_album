using UnityEngine;

public interface IPhotoDetailModel : IModel
{
    /// <summary>
    /// ’’∆¨œ∏Ω⁄Œ∆¿Ì
    /// </summary>
    Texture2D Tex2DPhotoDetail { get; }

    /// <summary>
    /// ’’∆¨±‡∫≈
    /// </summary>
    int PhotoId { get; }

    void SetModel(Texture2D tex2DPhotoDetail, int photoId);
}