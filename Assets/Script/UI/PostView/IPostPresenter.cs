using MVPFrameWork;

public interface IPostPresenter : IPresenter
{
    void Quit();

    void TryDeletePost();

    void SureCreateComment();
}