using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

[ParentInfo(FindType.FindWithName, StrDef.CANVAS)]
public class PhotoView : ViewBase<IPhotoPresenter>, IPhotoView
{
    private ButtonManager _btnQuit;
    private ButtonManager _btnUploadPhoto;
    private ButtonManager _btnUploadMultiPhotos;
    private ButtonManager _btnDeleteAlbum;
    private TMP_Text _txtAlbumName;
    private GridLayoutGroup _gridPhotoContent;

    public ButtonManager BtnQuit => _btnQuit;

    public TMP_Text TxtAlbumName => _txtAlbumName;

    public GridLayoutGroup GridPhotoContent => _gridPhotoContent;

    public ButtonManager BtnUploadPhoto => _btnUploadPhoto;

    public ButtonManager BtnUploadMultiPhotos => _btnUploadMultiPhotos;

    public ButtonManager BtnDeleteAlbum => _btnDeleteAlbum;

    protected override void OnCreate()
    {
        _btnQuit = _root.Find<ButtonManager>("AlbumPanel/btnQuit");
        _btnUploadPhoto = _root.Find<ButtonManager>("AlbumPanel/btnUploadPhoto");
        _btnUploadMultiPhotos = _root.Find<ButtonManager>("AlbumPanel/btnUploadMultiPhotos");
        _btnDeleteAlbum = _root.Find<ButtonManager>("AlbumPanel/btnDeleteAlbum");
        _txtAlbumName = _root.Find<TMP_Text>("AlbumPanel/txtAlbumName");
        _gridPhotoContent = _root.Find<GridLayoutGroup>("AlbumPanel/ScrollbarView/Viewport/Content");

        _btnQuit.onClick.AddListener(_presenter.Quit);
        _btnUploadPhoto.onClick.AddListener(_presenter.UploadPhoto);
        _btnUploadMultiPhotos.onClick.AddListener(_presenter.UploadMultiPhotos);
        _btnDeleteAlbum.onClick.AddListener(_presenter.DeleteAlbum);
    }
}