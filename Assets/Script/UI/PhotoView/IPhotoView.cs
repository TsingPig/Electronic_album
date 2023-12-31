using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface IPhotoView : IView
{
    ButtonManager BtnQuit { get; }

    ButtonManager BtnUploadPhoto { get; }

    ButtonManager BtnUploadMultiPhotos { get; }

    ButtonManager BtnDeleteAlbum { get; }

    TMP_Text TxtAlbumName { get; }

    GridLayoutGroup GridPhotoContent { get; }
}