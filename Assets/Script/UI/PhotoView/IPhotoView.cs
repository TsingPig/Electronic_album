using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface IPhotoModel : IModel
{
    public string Name { get; }
}

public class PhotoModel : IPhotoModel
{
    private string _name = "Name";

    string IPhotoModel.Name => _name;

    public void SetName(string name)
    {
        _name = name;
    }
}

public interface IPhotoView : IView
{
    ButtonManager BtnQuit { get; }

    TMP_Text TxtAlbumName { get; }

    GridLayoutGroup GridPhotoContent { get; }
}