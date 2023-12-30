using UnityEngine;
using UnityEngine.UI;

public interface IPhotoDetailModel : IModel
{
    /// <summary>
    /// ÕÕÆ¬Image×é¼þ
    /// </summary>
    Image ImgPhotoDetail { get; }

    /// <summary>
    /// ÕÕÆ¬±àºÅ
    /// </summary>
    int PhotoId { get; }

    void SetModel(Image imgPhotoDetail, int photoId);
}