public interface IPhotoModel : IModel
{
    public string AlbumName { get; }

    public string AlbumType { get; }

    void SetModel(string albumName, string albumType);
}