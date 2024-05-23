public class PostModel : IPostModel
{
    private IBBSModel.Post _post;

    public IBBSModel.Post Post { get => _post; set => _post = value; }

    public void SetModel(IBBSModel.Post post)
    {
        _post = post;
    }

}