using MVPFrameWork;
using UnityEngine;
using UIManager = MVPFrameWork.UIManager;

public class AlbumCreatePresenter : PresenterBase<IAlbumCreateView>, IAlbumCreatePresenter
{
    public void Quit()
    {
        UIManager.Instance.Quit(ViewId.AlbumCreateView);
        Debug.Log("Quit AlbumCreateView");
    }

    public void CreateAlbum()
    {
        Debug.Log($"{_view.InptAlbumName.text} {_view.DropDownAlbumType.selectedText}");
        ServerManager.Instance.CreateAlbumFolder(CacheManager.Instance.UserName, _view.InptAlbumName.text);
        UIManager.Instance.Quit(ViewId.AlbumCreateView);
        ServerManager.Instance.GetAlbumFolder(CacheManager.Instance.UserName);
    }
}
