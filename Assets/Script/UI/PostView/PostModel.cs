using System.Collections.Generic;

public class PostModel : IPostModel
{
    private IBBSModel.Post _post;
    private List<IPostModel.Comment> _comments;

    public IBBSModel.Post Post { get => _post; set => _post = value; }
    public List<IPostModel.Comment> Comments { get => _comments; set => _comments = value;}

    public void SetModel(IBBSModel.Post post)
    {
        _post = post;
    }

}