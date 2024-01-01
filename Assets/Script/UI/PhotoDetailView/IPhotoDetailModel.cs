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

    string AlbumName { get; }

    bool AllowDelete { get; }   

    void SetModel(Image imgPhotoDetail, int photoId, string albumName, bool allowDelete);
}