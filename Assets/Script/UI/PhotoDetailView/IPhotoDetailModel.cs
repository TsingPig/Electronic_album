using UnityEngine.UI;

public interface IPhotoDetailModel : IModel
{
    /// <summary>
    /// ��ƬImage���
    /// </summary>
    Image ImgPhotoDetail { get; }

    /// <summary>
    /// ��Ƭ���
    /// </summary>
    int PhotoId { get; }

    string AlbumName { get; }

    void SetModel(Image imgPhotoDetail, int photoId, string albumName);
}