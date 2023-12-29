public class PhotoModel : IPhotoModel
{
    private string _albumName = "NULL";
    private string _albumType = "NULL";

    public string AlbumType => _albumType;

    public string AlbumName => _albumName;

    public void SetModel(string albumName, string albumType = "NULL")
    {
        _albumName = albumName;
        _albumType = albumType;
    }
}