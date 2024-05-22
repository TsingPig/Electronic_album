using Michsky.MUIP;
using MVPFrameWork;
using TMPro;

using UnityEngine.UI;

public interface ICreatePostItemView : IView
{
    ButtonManager BtnQuit { get; }

    GridLayoutGroup GridPhotoContent { get; }

    ButtonManager BtnUploadPhoto { get; }

    ButtonManager BtnCreatePhotoWallItem { get; }

    TMP_InputField InptContent { get; }

    TMP_InputField InptTitle { get; }
}