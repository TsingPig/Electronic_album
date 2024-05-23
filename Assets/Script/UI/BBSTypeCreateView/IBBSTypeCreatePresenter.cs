using MVPFrameWork;
using TMPro;

public interface IBBSTypeCreatePresenter : IPresenter
{
    void Create();

    void TryCreate();

    void ClearInformation(TMP_InputField info);

    void Quit();
}