using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]
public class PhotoDetailView : ViewBase<IPhotoDetailPresenter>, IPhotoDetailView
{
    public ButtonManager _btnQuit;
    public ButtonManager _btnDeletePhoto;
    public Image _imgDetailPhoto;

    public ButtonManager BtnQuit => _btnQuit;

    public ButtonManager BtnDeletePhoto => _btnDeletePhoto;

    public Image ImgDetailPhoto { get => _imgDetailPhoto; set => _imgDetailPhoto = value; }

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("MainPanel/btnQuit");
        _btnDeletePhoto = _root.Find<ButtonManager>("MainPanel/btnDeletePhoto");
        _imgDetailPhoto = _root.Find<Image>("MainPanel/imgDetailPhoto");

        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnDeletePhoto.onClick.AddListener(_presenter.DeletePhoto);

    }
}